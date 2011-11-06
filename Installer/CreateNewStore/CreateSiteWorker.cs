using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BVSoftware.CreateStoreCore;

namespace CreateNewStore
{
    public partial class CreateSiteWorker : Form
    {

        private bool InstallWorked { get; set; }
        private SiteData localData { get; set; }

        public CreateSiteWorker()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.Enabled = false;
            this.backgroundWorker1.CancelAsync();
        }

        public void CreateStore(SiteData data)
        {
            this.InstallWorked = false;
            localData = data;
            this.OutputField.Text = string.Empty;
            this.backgroundWorker1.RunWorkerAsync(data);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {            
            SiteData data = (SiteData)e.Argument;
            SiteBuilder builder = new SiteBuilder();
            builder.ProgressReport += new SiteBuilder.ProgressReportDelegate(builder_ProgressReport);
            this.InstallWorked = builder.CreateSite(data);
        }

        void builder_ProgressReport(string message)
        {
            this.backgroundWorker1.ReportProgress(0, message);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.OutputField.AppendText((string)e.UserState + System.Environment.NewLine);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnCancel.Enabled = false;
            this.btnClose.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.InstallWorked)
            {
                //// Show Instructions
                //InstructionsForm i = new InstructionsForm();
                //i.Show();
                //i.ShowInstructions();
                //i.Focus();
            }
            this.Dispose();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InstructionsForm i = new InstructionsForm();
            i.Show();
            i.ShowInstructions();
            i.Focus();
        }

    }
}
