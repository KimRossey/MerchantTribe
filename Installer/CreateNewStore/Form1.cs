using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CreateNewStore
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
                CreateSiteForm instance = new CreateSiteForm();
                instance.Show();
                //instance.Focus();

                //InstructionsForm i = new InstructionsForm();
                //i.Show();
                //i.ShowInstructions();
                //i.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "Version " + Application.ProductVersion;
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InstructionsForm i = new InstructionsForm();
            i.Show();
            i.ShowInstructions();
            i.Focus();
        }
        
        
    }
}
