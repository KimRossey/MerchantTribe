using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CreateNewStore
{
    public partial class InstructionsForm : Form
    {
        public InstructionsForm()
        {
            InitializeComponent();
        }

        public void ShowInstructions()
        {
            string file = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Instructions.rtf");
            try
            {
                this.richTextBox1.LoadFile(file);
            }
            catch
            {
                this.richTextBox1.Text = "Unable to locate " + file;
            }
        }

        private void btnCloseInstructions_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }

    }
}
