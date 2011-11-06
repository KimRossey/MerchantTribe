using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BVSoftware.CreateStoreCore;

namespace CreateNewStore
{
    public partial class CreateSiteForm : Form
    {
        public CreateSiteForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
                        
            SiteData storeData = new SiteData();
            storeData.Location = this.LocalFilePath.Text.Trim();
            storeData.SQLServer = this.SQLServerField.Text.Trim();
            storeData.SQLDatabase = this.SQLDatabaseField.Text.Trim();
            storeData.SQLUsername = this.SQLUsernameField.Text.Trim();
            storeData.SQLPassword = this.SQLPasswordField.Text.Trim();
            storeData.SourceFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\src\\";

            storeData.WebSiteAddress = this.WebSiteAddressField.Text.Trim();
            storeData.PrepArgs();
            
            storeData.InstallSourceCode = true;

            CreateSiteWorker worker = new CreateSiteWorker();
            worker.Show();
            worker.Focus();
            worker.CreateStore(storeData);

            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dd = new FolderBrowserDialog();
            dd.SelectedPath = this.LocalFilePath.Text.Trim();
            if (dd.ShowDialog() == DialogResult.OK)
            {
                this.LocalFilePath.Text = dd.SelectedPath;
            }
            dd.Dispose();
        }

        private void SQLUsernameField_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
