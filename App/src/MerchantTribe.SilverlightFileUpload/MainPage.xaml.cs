using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Browser;
using System.IO;
using System.Threading;
using System.Runtime.Serialization;

namespace MerchantTribe.SilverlightFileUpload
{
    public partial class MainPage : UserControl
    {
        private static BackgroundWorker worker1;

        public static string ErrorMessages = string.Empty;
        public static string LastJsonResult = string.Empty;

        public static string FileName { get; set; }
        public static System.IO.FileStream FileStream { get; set; }
        public static long FileSizeInBytes { get; set; }
        public static bool IsFinishedWithChunk { get; set; }
        public static int CurrentChunkId { get; set; }
        public static double ChunkCount { get; set; }

        public static string UploadUrl { get; set; }
        public static string ScriptName { get; set; }

        public const int ChunkSize = 8192;
        public const int SendTimeoutInMilliSeconds = 5000;
            
        public MainPage()
        {
            InitializeComponent();            
            this.StatusLabel.Content = string.Empty;
            this.pbar.Visibility = System.Windows.Visibility.Collapsed;                       
        }

        private void ResetProgressBar()
        {
            this.pbar.Maximum = 100;
            this.pbar.Minimum = 0;
            this.pbar.Value = 0;
            this.pbar.Visibility = System.Windows.Visibility.Visible;

        }
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (worker1 != null && worker1.IsBusy)
            {
                worker1.CancelAsync();
                this.btnUpload.Content = "Cancelling...";
                this.btnUpload.IsEnabled = false;
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = false;
                ofd.Filter = "All files (*.*)|*.*|Images (*.png,*.jpg,*.gif)|*.png;*.jpg;*.gif";

                bool? retval = ofd.ShowDialog();
                if (retval != null && retval == true)
                {                                        
                    FileStream = ofd.File.OpenRead();
                    FileName = ofd.File.Name;
                    FileSizeInBytes = ofd.File.Length;
                    UploadFile();
                }
                else
                {
                    StatusLabel.Content = "No File Selected";
                }
            }
        }

        private void UploadFile()
        {            
            ResetProgressBar();
            StatusLabel.Content = "Uploading...";

            ErrorMessages = string.Empty;

            worker1 = new BackgroundWorker();
            worker1.WorkerReportsProgress = true;
            worker1.WorkerSupportsCancellation = true;            
            worker1.ProgressChanged += new ProgressChangedEventHandler(worker1_ProgressChanged);
            worker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker1_RunWorkerCompleted);
            worker1.DoWork += new DoWorkEventHandler(worker1_DoWork);

            this.btnUpload.Content = "Cancel";

            ChunkCount = 0;
            ChunkCount = (double)FileSizeInBytes / (double)ChunkSize;
              //ErrorMessages = "FileSize = " + FileSizeInBytes.ToString() + " | Chunks of " + ChunkSize.ToString() + " = " + ChunkCount.ToString();
            ChunkCount = Math.Ceiling(ChunkCount);
              //ErrorMessages += "After Ceiling = " + ChunkCount.ToString();

            CurrentChunkId = 0;            
            IsFinishedWithChunk = true;
            worker1.RunWorkerAsync();
        }

        void worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    // Pause for a reasonable time
                    System.Threading.Thread.Sleep(30);
                    BackgroundWorker worker = sender as BackgroundWorker;

                    // Report Progress
                    double percentageDone = (((double)CurrentChunkId - 1) / ChunkCount) * 100;
                    if (percentageDone < 0) percentageDone = 0;
                    worker1.ReportProgress((int)percentageDone);

                    // Check for Cancel
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        LastJsonResult = "{\"success\":\"0\",\"imageurl\":\"\",\"filename\":\"\"}";
                        return;
                    }
                    else
                    {                      
                        if (IsFinishedWithChunk)
                        {
                            
                            IsFinishedWithChunk = false;
                            if (CurrentChunkId < ChunkCount)
                            {
                                SendNextChunk();
                            }
                            else
                            {
                                worker1.ReportProgress(100);
                                return;
                            }
                        }
                    }
          
                    e.Result = "File Uploaded was " + FileName;
                }
            }
            catch (Exception ex)
            {
                ErrorMessages += ex.Message + " <br /> " + ex.StackTrace;
            }

        }

        static void SendNextChunk()
        {
            CurrentChunkId += 1;
                        
            // Get Url to Send To
            string serviceUrl = UploadUrl;
            serviceUrl += "?filename=" + HttpUtility.UrlEncode(FileName);
            if (CurrentChunkId == 1)
            {
                serviceUrl += "&firstpart=1";
            }
            else
            {
                serviceUrl += "&firstpart=0";
            }

            // Make Request
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(serviceUrl);
            webrequest.Method = "POST";
            webrequest.BeginGetRequestStream(new AsyncCallback(WritePostData), webrequest);
                                  
        }

        private static void WritePostData(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webrequest = (HttpWebRequest)asynchronousResult.AsyncState;            
            Stream requestStream = webrequest.EndGetRequestStream(asynchronousResult);

            // Get A Chunk
            byte[] chunk = null;
            int startIndex = (CurrentChunkId - 1) * ChunkSize;
            if (startIndex + ChunkSize > FileSizeInBytes)
            {
                chunk = new byte[FileSizeInBytes - startIndex];
            }
            else
            {
                chunk = new byte[ChunkSize];
            }
            if (FileStream.CanRead)
            {
                int chunkresult = FileStream.Read(chunk, 0, chunk.Length);
            }
            else
            {
                ErrorMessages += "Can't read file stream anymore";
            }

            // Write Chunk
            requestStream.Write(chunk, 0, chunk.Length);
            requestStream.Flush();
            requestStream.Close();

            // Get Response
            webrequest.BeginGetResponse(new AsyncCallback(ReadCallback), webrequest);            
        }
        
        [DataContract]
        private class FilePostResult
        {
            [DataMember]
            public string success {get;set;}
            [DataMember]
            public string imageurl {get;set;}

            public FilePostResult()
            {
                success = "0";
                imageurl = string.Empty;
            }
        }

        private static void ReadCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webrequest = (HttpWebRequest)asynchronousResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)webrequest.EndGetResponse(asynchronousResult);
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string responsestring = reader.ReadToEnd();
            reader.Close();

            LastJsonResult = responsestring;
            
            // Check Response String Here
            IsFinishedWithChunk = true;
        }

        void worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.pbar.Visibility = System.Windows.Visibility.Collapsed;
            this.btnUpload.IsEnabled = true;
            this.btnUpload.Content = "Upload";

            if (e.Cancelled)
            {
                StatusLabel.Content = "Cancelled!";
                HtmlPage.Window.Invoke(ScriptName, "{\"success\":\"0\",\"imageurl\":\"\",\"filename\":\"\"}");                
            }
            else
            {                
                StatusLabel.Content = "Done!";
                HtmlPage.Window.Invoke(ScriptName, LastJsonResult);
            }                        

            if (ErrorMessages.Length > 0)
            {
                HtmlPage.Window.Invoke("SilverlightFileUploadError", ErrorMessages);
            }
        }

        void worker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {        
            pbar.Value = e.ProgressPercentage;        
            this.StatusLabel.Content = pbar.Value.ToString() + "% ";
            long sizeInBytes = (long)e.UserState;            
            this.StatusLabel.Content += FriendlyFormatBytes(sizeInBytes);                                    
        }

        private string FriendlyFormatBytes(long sizeInBytes)
        {
            if (sizeInBytes < 1024)
            {
                return sizeInBytes + " bytes";
            }
            else
            {
                if (sizeInBytes < 1048576)
                {
                    return Math.Round((double)sizeInBytes / 1024, 1) + " KB";
                }
                else
                {
                    return Math.Round((double)sizeInBytes / 1048576, 1) + " MB";
                }
            }
        }
           
        
   }
}
