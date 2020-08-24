namespace SiteMigrationProject
{
    partial class Form1
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
            this.tabSourceSite = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ChkListName = new System.Windows.Forms.CheckedListBox();
            this.lblSelect = new System.Windows.Forms.Label();
            this.chkTemplateOptions = new System.Windows.Forms.CheckedListBox();
            this.btnSourceBrowse = new System.Windows.Forms.Button();
            this.txtSourceFileName = new System.Windows.Forms.TextBox();
            this.lblSourceFileName = new System.Windows.Forms.Label();
            this.btnXMLFile = new System.Windows.Forms.Button();
            this.lblListName = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileLocation = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblSourceSiteName = new System.Windows.Forms.Label();
            this.txtSourceSiteName = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtDesXMLFileName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnSiteMigrate = new System.Windows.Forms.Button();
            this.lblDesFileName = new System.Windows.Forms.Label();
            this.txtDesPassword = new System.Windows.Forms.TextBox();
            this.lblDesPassword = new System.Windows.Forms.Label();
            this.txtDesUsername = new System.Windows.Forms.TextBox();
            this.lblDesUsename = new System.Windows.Forms.Label();
            this.txtDestinationSiteName = new System.Windows.Forms.TextBox();
            this.lblDestinationSiteName = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabSourceSite.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSourceSite
            // 
            this.tabSourceSite.Controls.Add(this.tabPage1);
            this.tabSourceSite.Controls.Add(this.tabPage2);
            this.tabSourceSite.Location = new System.Drawing.Point(52, 12);
            this.tabSourceSite.Name = "tabSourceSite";
            this.tabSourceSite.SelectedIndex = 0;
            this.tabSourceSite.Size = new System.Drawing.Size(800, 700);
            this.tabSourceSite.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ChkListName);
            this.tabPage1.Controls.Add(this.lblSelect);
            this.tabPage1.Controls.Add(this.chkTemplateOptions);
            this.tabPage1.Controls.Add(this.btnSourceBrowse);
            this.tabPage1.Controls.Add(this.txtSourceFileName);
            this.tabPage1.Controls.Add(this.lblSourceFileName);
            this.tabPage1.Controls.Add(this.btnXMLFile);
            this.tabPage1.Controls.Add(this.lblListName);
            this.tabPage1.Controls.Add(this.lblFileName);
            this.tabPage1.Controls.Add(this.txtFileLocation);
            this.tabPage1.Controls.Add(this.lblPassword);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.lblUsername);
            this.tabPage1.Controls.Add(this.txtUsername);
            this.tabPage1.Controls.Add(this.lblSourceSiteName);
            this.tabPage1.Controls.Add(this.txtSourceSiteName);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 674);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SourceSiteDetails";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ChkListName
            // 
            this.ChkListName.FormattingEnabled = true;
            this.ChkListName.Location = new System.Drawing.Point(151, 293);
            this.ChkListName.MultiColumn = true;
            this.ChkListName.Name = "ChkListName";
            this.ChkListName.Size = new System.Drawing.Size(512, 124);
            this.ChkListName.TabIndex = 16;
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Location = new System.Drawing.Point(58, 140);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(37, 13);
            this.lblSelect.TabIndex = 15;
            this.lblSelect.Text = "Select";
            // 
            // chkTemplateOptions
            // 
            this.chkTemplateOptions.FormattingEnabled = true;
            this.chkTemplateOptions.Items.AddRange(new object[] {
            "All",
            "CustomActions",
            "Features",
            "Lists",
            "Files",
            "Pages",
            "RegionalSetting",
            "SearchSetting",
            "Workflows",
            "ContentTypes",
            "SiteFields",
            "Navigation",
            "SiteFooter",
            "SiteHeader",
            "Theme",
            "SitePolicy",
            "SupportedUILanguages",
            "AuditSettings",
            "PropertyBagEntries",
            "Security",
            "ComposedLook",
            "Publishing",
            "Tenant",
            "TermGroups",
            "PageContents",
            "WebSettings",
            "ImageRenditions",
            "ApplicationLifecycleManagement",
            "WebApiPermissions"});
            this.chkTemplateOptions.Location = new System.Drawing.Point(151, 103);
            this.chkTemplateOptions.MultiColumn = true;
            this.chkTemplateOptions.Name = "chkTemplateOptions";
            this.chkTemplateOptions.Size = new System.Drawing.Size(512, 109);
            this.chkTemplateOptions.TabIndex = 14;
            this.chkTemplateOptions.SelectedIndexChanged += new System.EventHandler(this.chkTemplateOptions_SelectedIndexChanged);
            // 
            // btnSourceBrowse
            // 
            this.btnSourceBrowse.Location = new System.Drawing.Point(588, 218);
            this.btnSourceBrowse.Name = "btnSourceBrowse";
            this.btnSourceBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnSourceBrowse.TabIndex = 13;
            this.btnSourceBrowse.Text = "Browse";
            this.btnSourceBrowse.UseVisualStyleBackColor = true;
            this.btnSourceBrowse.Click += new System.EventHandler(this.btnSourceBrowse_Click);
            // 
            // txtSourceFileName
            // 
            this.txtSourceFileName.Location = new System.Drawing.Point(151, 254);
            this.txtSourceFileName.Name = "txtSourceFileName";
            this.txtSourceFileName.Size = new System.Drawing.Size(512, 20);
            this.txtSourceFileName.TabIndex = 12;
            // 
            // lblSourceFileName
            // 
            this.lblSourceFileName.AutoSize = true;
            this.lblSourceFileName.Location = new System.Drawing.Point(56, 261);
            this.lblSourceFileName.Name = "lblSourceFileName";
            this.lblSourceFileName.Size = new System.Drawing.Size(54, 13);
            this.lblSourceFileName.TabIndex = 11;
            this.lblSourceFileName.Text = "File Name";
            // 
            // btnXMLFile
            // 
            this.btnXMLFile.Location = new System.Drawing.Point(227, 476);
            this.btnXMLFile.Name = "btnXMLFile";
            this.btnXMLFile.Size = new System.Drawing.Size(163, 34);
            this.btnXMLFile.TabIndex = 10;
            this.btnXMLFile.Text = "Creating XML File";
            this.btnXMLFile.UseVisualStyleBackColor = true;
            this.btnXMLFile.Click += new System.EventHandler(this.btnXMLFile_Click);
            // 
            // lblListName
            // 
            this.lblListName.AutoSize = true;
            this.lblListName.Location = new System.Drawing.Point(56, 309);
            this.lblListName.Name = "lblListName";
            this.lblListName.Size = new System.Drawing.Size(54, 13);
            this.lblListName.TabIndex = 9;
            this.lblListName.Text = "List Name";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(56, 233);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(67, 13);
            this.lblFileName.TabIndex = 7;
            this.lblFileName.Text = "File Location";
            // 
            // txtFileLocation
            // 
            this.txtFileLocation.Location = new System.Drawing.Point(151, 226);
            this.txtFileLocation.Name = "txtFileLocation";
            this.txtFileLocation.Size = new System.Drawing.Size(418, 20);
            this.txtFileLocation.TabIndex = 6;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(58, 77);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(151, 77);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(512, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(56, 39);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(151, 39);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(512, 20);
            this.txtUsername.TabIndex = 2;
            // 
            // lblSourceSiteName
            // 
            this.lblSourceSiteName.AutoSize = true;
            this.lblSourceSiteName.Location = new System.Drawing.Point(54, 9);
            this.lblSourceSiteName.Name = "lblSourceSiteName";
            this.lblSourceSiteName.Size = new System.Drawing.Size(93, 13);
            this.lblSourceSiteName.TabIndex = 1;
            this.lblSourceSiteName.Text = "Source Site Name";
            // 
            // txtSourceSiteName
            // 
            this.txtSourceSiteName.Location = new System.Drawing.Point(151, 6);
            this.txtSourceSiteName.Name = "txtSourceSiteName";
            this.txtSourceSiteName.Size = new System.Drawing.Size(512, 20);
            this.txtSourceSiteName.TabIndex = 0;
            this.txtSourceSiteName.MouseLeave += new System.EventHandler(this.txtSourceSiteName_MouseLeave);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtDesXMLFileName);
            this.tabPage2.Controls.Add(this.btnBrowse);
            this.tabPage2.Controls.Add(this.btnSiteMigrate);
            this.tabPage2.Controls.Add(this.lblDesFileName);
            this.tabPage2.Controls.Add(this.txtDesPassword);
            this.tabPage2.Controls.Add(this.lblDesPassword);
            this.tabPage2.Controls.Add(this.txtDesUsername);
            this.tabPage2.Controls.Add(this.lblDesUsename);
            this.tabPage2.Controls.Add(this.txtDestinationSiteName);
            this.tabPage2.Controls.Add(this.lblDestinationSiteName);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 674);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TargetSiteDetails";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtDesXMLFileName
            // 
            this.txtDesXMLFileName.Location = new System.Drawing.Point(195, 161);
            this.txtDesXMLFileName.Name = "txtDesXMLFileName";
            this.txtDesXMLFileName.Size = new System.Drawing.Size(293, 20);
            this.txtDesXMLFileName.TabIndex = 10;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(494, 161);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(84, 23);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click_1);
            // 
            // btnSiteMigrate
            // 
            this.btnSiteMigrate.Location = new System.Drawing.Point(230, 217);
            this.btnSiteMigrate.Name = "btnSiteMigrate";
            this.btnSiteMigrate.Size = new System.Drawing.Size(199, 35);
            this.btnSiteMigrate.TabIndex = 8;
            this.btnSiteMigrate.Text = "Site Migrate";
            this.btnSiteMigrate.UseVisualStyleBackColor = true;
            this.btnSiteMigrate.Click += new System.EventHandler(this.btnSiteMigrate_Click);
            // 
            // lblDesFileName
            // 
            this.lblDesFileName.AutoSize = true;
            this.lblDesFileName.Location = new System.Drawing.Point(69, 164);
            this.lblDesFileName.Name = "lblDesFileName";
            this.lblDesFileName.Size = new System.Drawing.Size(54, 13);
            this.lblDesFileName.TabIndex = 6;
            this.lblDesFileName.Text = "File Name";
            // 
            // txtDesPassword
            // 
            this.txtDesPassword.Location = new System.Drawing.Point(195, 123);
            this.txtDesPassword.Name = "txtDesPassword";
            this.txtDesPassword.Size = new System.Drawing.Size(383, 20);
            this.txtDesPassword.TabIndex = 5;
            this.txtDesPassword.UseSystemPasswordChar = true;
            // 
            // lblDesPassword
            // 
            this.lblDesPassword.AutoSize = true;
            this.lblDesPassword.Location = new System.Drawing.Point(68, 130);
            this.lblDesPassword.Name = "lblDesPassword";
            this.lblDesPassword.Size = new System.Drawing.Size(53, 13);
            this.lblDesPassword.TabIndex = 4;
            this.lblDesPassword.Text = "Password";
            // 
            // txtDesUsername
            // 
            this.txtDesUsername.Location = new System.Drawing.Point(195, 84);
            this.txtDesUsername.Name = "txtDesUsername";
            this.txtDesUsername.Size = new System.Drawing.Size(383, 20);
            this.txtDesUsername.TabIndex = 3;
            // 
            // lblDesUsename
            // 
            this.lblDesUsename.AutoSize = true;
            this.lblDesUsename.Location = new System.Drawing.Point(68, 91);
            this.lblDesUsename.Name = "lblDesUsename";
            this.lblDesUsename.Size = new System.Drawing.Size(55, 13);
            this.lblDesUsename.TabIndex = 2;
            this.lblDesUsename.Text = "Username";
            // 
            // txtDestinationSiteName
            // 
            this.txtDestinationSiteName.Location = new System.Drawing.Point(195, 46);
            this.txtDestinationSiteName.Name = "txtDestinationSiteName";
            this.txtDestinationSiteName.Size = new System.Drawing.Size(383, 20);
            this.txtDestinationSiteName.TabIndex = 1;
            // 
            // lblDestinationSiteName
            // 
            this.lblDestinationSiteName.AutoSize = true;
            this.lblDestinationSiteName.Location = new System.Drawing.Point(68, 49);
            this.lblDestinationSiteName.Name = "lblDestinationSiteName";
            this.lblDestinationSiteName.Size = new System.Drawing.Size(112, 13);
            this.lblDestinationSiteName.TabIndex = 0;
            this.lblDestinationSiteName.Text = "Destination Site Name";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 749);
            this.Controls.Add(this.tabSourceSite);
            this.Name = "Form1";
            this.Text = "Site Migration";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabSourceSite.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSourceSite;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblSourceSiteName;
        private System.Windows.Forms.TextBox txtSourceSiteName;
        private System.Windows.Forms.Label lblListName;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileLocation;
        private System.Windows.Forms.Label lblDesFileName;
        private System.Windows.Forms.TextBox txtDesPassword;
        private System.Windows.Forms.Label lblDesPassword;
        private System.Windows.Forms.TextBox txtDesUsername;
        private System.Windows.Forms.Label lblDesUsename;
        private System.Windows.Forms.TextBox txtDestinationSiteName;
        private System.Windows.Forms.Label lblDestinationSiteName;
        private System.Windows.Forms.Button btnSiteMigrate;
        private System.Windows.Forms.Button btnXMLFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtDesXMLFileName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtSourceFileName;
        private System.Windows.Forms.Label lblSourceFileName;
        private System.Windows.Forms.Button btnSourceBrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.CheckedListBox chkTemplateOptions;
        private System.Windows.Forms.CheckedListBox ChkListName;
    }
}

