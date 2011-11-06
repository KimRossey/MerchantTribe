namespace CreateNewStore
{
    partial class CreateSiteForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.LocalFilePath = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SQLServerField = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SQLDatabaseField = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SQLPasswordField = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SQLUsernameField = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.WebSiteAddressField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(13, 389);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(327, 389);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(4);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(204, 31);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Create This Store";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // LocalFilePath
            // 
            this.LocalFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LocalFilePath.Location = new System.Drawing.Point(18, 49);
            this.LocalFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.LocalFilePath.Name = "LocalFilePath";
            this.LocalFilePath.Size = new System.Drawing.Size(409, 24);
            this.LocalFilePath.TabIndex = 1;
            this.LocalFilePath.Text = "c:\\projects\\MerchantTribe";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(435, 45);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 32);
            this.button2.TabIndex = 2;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Install Folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(15, 130);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "SQL Server Name";
            // 
            // SQLServerField
            // 
            this.SQLServerField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQLServerField.Location = new System.Drawing.Point(18, 152);
            this.SQLServerField.Margin = new System.Windows.Forms.Padding(4);
            this.SQLServerField.Name = "SQLServerField";
            this.SQLServerField.Size = new System.Drawing.Size(234, 24);
            this.SQLServerField.TabIndex = 4;
            this.SQLServerField.Text = "localhost";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(294, 130);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Database Name";
            // 
            // SQLDatabaseField
            // 
            this.SQLDatabaseField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQLDatabaseField.Location = new System.Drawing.Point(297, 152);
            this.SQLDatabaseField.Margin = new System.Windows.Forms.Padding(4);
            this.SQLDatabaseField.Name = "SQLDatabaseField";
            this.SQLDatabaseField.Size = new System.Drawing.Size(234, 24);
            this.SQLDatabaseField.TabIndex = 6;
            this.SQLDatabaseField.Text = "MerchantTribe";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(294, 195);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 18);
            this.label4.TabIndex = 11;
            this.label4.Text = "SQL Password";
            // 
            // SQLPasswordField
            // 
            this.SQLPasswordField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQLPasswordField.Location = new System.Drawing.Point(297, 217);
            this.SQLPasswordField.Margin = new System.Windows.Forms.Padding(4);
            this.SQLPasswordField.Name = "SQLPasswordField";
            this.SQLPasswordField.PasswordChar = '*';
            this.SQLPasswordField.Size = new System.Drawing.Size(234, 24);
            this.SQLPasswordField.TabIndex = 10;
            this.SQLPasswordField.Text = "password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(15, 195);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "SQL Username";
            // 
            // SQLUsernameField
            // 
            this.SQLUsernameField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQLUsernameField.Location = new System.Drawing.Point(18, 217);
            this.SQLUsernameField.Margin = new System.Windows.Forms.Padding(4);
            this.SQLUsernameField.Name = "SQLUsernameField";
            this.SQLUsernameField.Size = new System.Drawing.Size(234, 24);
            this.SQLUsernameField.TabIndex = 8;
            this.SQLUsernameField.Text = "username";
            this.SQLUsernameField.TextChanged += new System.EventHandler(this.SQLUsernameField_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(15, 291);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "Web Site Address";
            // 
            // WebSiteAddressField
            // 
            this.WebSiteAddressField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WebSiteAddressField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebSiteAddressField.Location = new System.Drawing.Point(18, 313);
            this.WebSiteAddressField.Margin = new System.Windows.Forms.Padding(4);
            this.WebSiteAddressField.Name = "WebSiteAddressField";
            this.WebSiteAddressField.Size = new System.Drawing.Size(513, 24);
            this.WebSiteAddressField.TabIndex = 12;
            this.WebSiteAddressField.Text = "http://www.sample.com/";
            // 
            // CreateSiteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 433);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.WebSiteAddressField);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SQLPasswordField);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SQLUsernameField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SQLDatabaseField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SQLServerField);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.LocalFilePath);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CreateSiteForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create a New Store";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox LocalFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SQLServerField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SQLDatabaseField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SQLPasswordField;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SQLUsernameField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox WebSiteAddressField;
    }
}