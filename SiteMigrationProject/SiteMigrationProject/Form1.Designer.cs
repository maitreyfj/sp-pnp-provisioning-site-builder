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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabSiteMigration = new System.Windows.Forms.TabControl();
            this.tabSiteBuilder = new System.Windows.Forms.TabPage();
            this.rbExistingSite = new System.Windows.Forms.RadioButton();
            this.pnlProvision = new System.Windows.Forms.Panel();
            this.lblSiteBuilderErrorLog = new System.Windows.Forms.Label();
            this.SiteBuilderProgressBar = new System.Windows.Forms.ProgressBar();
            this.btnAssetPath = new System.Windows.Forms.Button();
            this.txtAssetsPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowseTemplate = new System.Windows.Forms.Button();
            this.txtTemplateName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPwd = new System.Windows.Forms.Label();
            this.lblSiteUrl = new System.Windows.Forms.Label();
            this.lblSiteTitle = new System.Windows.Forms.Label();
            this.btnProvision = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtSiteURLTitle = new System.Windows.Forms.TextBox();
            this.txtSiteUrlName = new System.Windows.Forms.TextBox();
            this.lblSiteCreation = new System.Windows.Forms.Label();
            this.rbSiteReplica = new System.Windows.Forms.RadioButton();
            this.rbNewSite = new System.Windows.Forms.RadioButton();
            this.tabSourceSite = new System.Windows.Forms.TabPage();
            this.btnXMlFileWithListData = new System.Windows.Forms.Button();
            this.lblSourceSiteErrorLog = new System.Windows.Forms.Label();
            this.SourceSiteProgressBar = new System.Windows.Forms.ProgressBar();
            this.btnLoadList = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
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
            this.tabTargetSite = new System.Windows.Forms.TabPage();
            this.btnAddGroups = new System.Windows.Forms.Button();
            this.lblTargetSiteErrorLog = new System.Windows.Forms.Label();
            this.TargetSiteProgressBar = new System.Windows.Forms.ProgressBar();
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
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnDataMigration = new System.Windows.Forms.Button();
            this.tabSiteMigration.SuspendLayout();
            this.tabSiteBuilder.SuspendLayout();
            this.pnlProvision.SuspendLayout();
            this.tabSourceSite.SuspendLayout();
            this.tabTargetSite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabSiteMigration
            // 
            this.tabSiteMigration.Controls.Add(this.tabSiteBuilder);
            this.tabSiteMigration.Controls.Add(this.tabSourceSite);
            this.tabSiteMigration.Controls.Add(this.tabTargetSite);
            this.tabSiteMigration.Location = new System.Drawing.Point(52, 12);
            this.tabSiteMigration.Name = "tabSiteMigration";
            this.tabSiteMigration.SelectedIndex = 0;
            this.tabSiteMigration.Size = new System.Drawing.Size(800, 700);
            this.tabSiteMigration.TabIndex = 0;
            // 
            // tabSiteBuilder
            // 
            this.tabSiteBuilder.Controls.Add(this.rbExistingSite);
            this.tabSiteBuilder.Controls.Add(this.pnlProvision);
            this.tabSiteBuilder.Controls.Add(this.lblSiteCreation);
            this.tabSiteBuilder.Controls.Add(this.rbSiteReplica);
            this.tabSiteBuilder.Controls.Add(this.rbNewSite);
            this.tabSiteBuilder.Location = new System.Drawing.Point(4, 22);
            this.tabSiteBuilder.Name = "tabSiteBuilder";
            this.tabSiteBuilder.Size = new System.Drawing.Size(792, 674);
            this.tabSiteBuilder.TabIndex = 2;
            this.tabSiteBuilder.Text = "Site Builder";
            this.tabSiteBuilder.UseVisualStyleBackColor = true;
            // 
            // rbExistingSite
            // 
            this.rbExistingSite.AutoSize = true;
            this.rbExistingSite.Location = new System.Drawing.Point(372, 42);
            this.rbExistingSite.Name = "rbExistingSite";
            this.rbExistingSite.Size = new System.Drawing.Size(158, 17);
            this.rbExistingSite.TabIndex = 14;
            this.rbExistingSite.Text = "Add template to existing Site";
            this.rbExistingSite.UseVisualStyleBackColor = true;
            this.rbExistingSite.CheckedChanged += new System.EventHandler(this.rbExistingSite_CheckedChanged);
            // 
            // pnlProvision
            // 
            this.pnlProvision.Controls.Add(this.lblSiteBuilderErrorLog);
            this.pnlProvision.Controls.Add(this.SiteBuilderProgressBar);
            this.pnlProvision.Controls.Add(this.btnAssetPath);
            this.pnlProvision.Controls.Add(this.txtAssetsPath);
            this.pnlProvision.Controls.Add(this.label2);
            this.pnlProvision.Controls.Add(this.btnBrowseTemplate);
            this.pnlProvision.Controls.Add(this.txtTemplateName);
            this.pnlProvision.Controls.Add(this.label1);
            this.pnlProvision.Controls.Add(this.lblEmail);
            this.pnlProvision.Controls.Add(this.lblPwd);
            this.pnlProvision.Controls.Add(this.lblSiteUrl);
            this.pnlProvision.Controls.Add(this.lblSiteTitle);
            this.pnlProvision.Controls.Add(this.btnProvision);
            this.pnlProvision.Controls.Add(this.txtEmail);
            this.pnlProvision.Controls.Add(this.btnCancel);
            this.pnlProvision.Controls.Add(this.txtPwd);
            this.pnlProvision.Controls.Add(this.txtSiteURLTitle);
            this.pnlProvision.Controls.Add(this.txtSiteUrlName);
            this.pnlProvision.Location = new System.Drawing.Point(110, 83);
            this.pnlProvision.Name = "pnlProvision";
            this.pnlProvision.Size = new System.Drawing.Size(564, 466);
            this.pnlProvision.TabIndex = 13;
            // 
            // lblSiteBuilderErrorLog
            // 
            this.lblSiteBuilderErrorLog.AutoSize = true;
            this.lblSiteBuilderErrorLog.Location = new System.Drawing.Point(40, 417);
            this.lblSiteBuilderErrorLog.Name = "lblSiteBuilderErrorLog";
            this.lblSiteBuilderErrorLog.Size = new System.Drawing.Size(0, 13);
            this.lblSiteBuilderErrorLog.TabIndex = 18;
            // 
            // SiteBuilderProgressBar
            // 
            this.SiteBuilderProgressBar.Location = new System.Drawing.Point(155, 353);
            this.SiteBuilderProgressBar.Name = "SiteBuilderProgressBar";
            this.SiteBuilderProgressBar.Size = new System.Drawing.Size(311, 23);
            this.SiteBuilderProgressBar.TabIndex = 17;
            this.SiteBuilderProgressBar.Value = 100;
            // 
            // btnAssetPath
            // 
            this.btnAssetPath.Location = new System.Drawing.Point(472, 90);
            this.btnAssetPath.Name = "btnAssetPath";
            this.btnAssetPath.Size = new System.Drawing.Size(75, 23);
            this.btnAssetPath.TabIndex = 16;
            this.btnAssetPath.Text = "Select Path";
            this.btnAssetPath.UseVisualStyleBackColor = true;
            this.btnAssetPath.Click += new System.EventHandler(this.btnAssetPath_Click);
            // 
            // txtAssetsPath
            // 
            this.txtAssetsPath.Location = new System.Drawing.Point(139, 87);
            this.txtAssetsPath.Name = "txtAssetsPath";
            this.txtAssetsPath.Size = new System.Drawing.Size(327, 20);
            this.txtAssetsPath.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Assets Location";
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Location = new System.Drawing.Point(472, 44);
            this.btnBrowseTemplate.Name = "btnBrowseTemplate";
            this.btnBrowseTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseTemplate.TabIndex = 13;
            this.btnBrowseTemplate.Text = "Browse";
            this.btnBrowseTemplate.UseVisualStyleBackColor = true;
            this.btnBrowseTemplate.Click += new System.EventHandler(this.btnBrowseTemplate_Click);
            // 
            // txtTemplateName
            // 
            this.txtTemplateName.Location = new System.Drawing.Point(139, 46);
            this.txtTemplateName.Name = "txtTemplateName";
            this.txtTemplateName.Size = new System.Drawing.Size(327, 20);
            this.txtTemplateName.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Template Name";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(27, 124);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Email";
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new System.Drawing.Point(27, 159);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(53, 13);
            this.lblPwd.TabIndex = 1;
            this.lblPwd.Text = "Password";
            // 
            // lblSiteUrl
            // 
            this.lblSiteUrl.AutoSize = true;
            this.lblSiteUrl.Location = new System.Drawing.Point(27, 199);
            this.lblSiteUrl.Name = "lblSiteUrl";
            this.lblSiteUrl.Size = new System.Drawing.Size(50, 13);
            this.lblSiteUrl.TabIndex = 2;
            this.lblSiteUrl.Text = "Site URL";
            // 
            // lblSiteTitle
            // 
            this.lblSiteTitle.AutoSize = true;
            this.lblSiteTitle.Location = new System.Drawing.Point(27, 241);
            this.lblSiteTitle.Name = "lblSiteTitle";
            this.lblSiteTitle.Size = new System.Drawing.Size(48, 13);
            this.lblSiteTitle.TabIndex = 3;
            this.lblSiteTitle.Text = "Site Title";
            // 
            // btnProvision
            // 
            this.btnProvision.Location = new System.Drawing.Point(318, 281);
            this.btnProvision.Name = "btnProvision";
            this.btnProvision.Size = new System.Drawing.Size(75, 23);
            this.btnProvision.TabIndex = 9;
            this.btnProvision.Text = "Provision";
            this.btnProvision.UseVisualStyleBackColor = true;
            this.btnProvision.Click += new System.EventHandler(this.btnProvision_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(139, 117);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(327, 20);
            this.txtEmail.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(174, 281);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(139, 153);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(327, 20);
            this.txtPwd.TabIndex = 5;
            // 
            // txtSiteURLTitle
            // 
            this.txtSiteURLTitle.Location = new System.Drawing.Point(139, 234);
            this.txtSiteURLTitle.Name = "txtSiteURLTitle";
            this.txtSiteURLTitle.Size = new System.Drawing.Size(327, 20);
            this.txtSiteURLTitle.TabIndex = 7;
            // 
            // txtSiteUrlName
            // 
            this.txtSiteUrlName.Location = new System.Drawing.Point(139, 197);
            this.txtSiteUrlName.Name = "txtSiteUrlName";
            this.txtSiteUrlName.Size = new System.Drawing.Size(327, 20);
            this.txtSiteUrlName.TabIndex = 6;
            // 
            // lblSiteCreation
            // 
            this.lblSiteCreation.AutoSize = true;
            this.lblSiteCreation.Location = new System.Drawing.Point(114, 42);
            this.lblSiteCreation.Name = "lblSiteCreation";
            this.lblSiteCreation.Size = new System.Drawing.Size(71, 13);
            this.lblSiteCreation.TabIndex = 12;
            this.lblSiteCreation.Text = "Select Option";
            // 
            // rbSiteReplica
            // 
            this.rbSiteReplica.AutoSize = true;
            this.rbSiteReplica.Location = new System.Drawing.Point(550, 42);
            this.rbSiteReplica.Name = "rbSiteReplica";
            this.rbSiteReplica.Size = new System.Drawing.Size(82, 17);
            this.rbSiteReplica.TabIndex = 11;
            this.rbSiteReplica.Text = "Site Replica";
            this.rbSiteReplica.UseVisualStyleBackColor = true;
            this.rbSiteReplica.CheckedChanged += new System.EventHandler(this.rbSiteReplica_CheckedChanged);
            // 
            // rbNewSite
            // 
            this.rbNewSite.AutoSize = true;
            this.rbNewSite.Checked = true;
            this.rbNewSite.Location = new System.Drawing.Point(213, 40);
            this.rbNewSite.Name = "rbNewSite";
            this.rbNewSite.Size = new System.Drawing.Size(141, 17);
            this.rbNewSite.TabIndex = 10;
            this.rbNewSite.TabStop = true;
            this.rbNewSite.Text = "Add template to new site";
            this.rbNewSite.UseVisualStyleBackColor = true;
            this.rbNewSite.CheckedChanged += new System.EventHandler(this.rbNewSite_CheckedChanged);
            // 
            // tabSourceSite
            // 
            this.tabSourceSite.Controls.Add(this.btnXMlFileWithListData);
            this.tabSourceSite.Controls.Add(this.lblSourceSiteErrorLog);
            this.tabSourceSite.Controls.Add(this.SourceSiteProgressBar);
            this.tabSourceSite.Controls.Add(this.btnLoadList);
            this.tabSourceSite.Controls.Add(this.btnBack);
            this.tabSourceSite.Controls.Add(this.ChkListName);
            this.tabSourceSite.Controls.Add(this.lblSelect);
            this.tabSourceSite.Controls.Add(this.chkTemplateOptions);
            this.tabSourceSite.Controls.Add(this.btnSourceBrowse);
            this.tabSourceSite.Controls.Add(this.txtSourceFileName);
            this.tabSourceSite.Controls.Add(this.lblSourceFileName);
            this.tabSourceSite.Controls.Add(this.btnXMLFile);
            this.tabSourceSite.Controls.Add(this.lblListName);
            this.tabSourceSite.Controls.Add(this.lblFileName);
            this.tabSourceSite.Controls.Add(this.txtFileLocation);
            this.tabSourceSite.Controls.Add(this.lblPassword);
            this.tabSourceSite.Controls.Add(this.txtPassword);
            this.tabSourceSite.Controls.Add(this.lblUsername);
            this.tabSourceSite.Controls.Add(this.txtUsername);
            this.tabSourceSite.Controls.Add(this.lblSourceSiteName);
            this.tabSourceSite.Controls.Add(this.txtSourceSiteName);
            this.tabSourceSite.Location = new System.Drawing.Point(4, 22);
            this.tabSourceSite.Name = "tabSourceSite";
            this.tabSourceSite.Padding = new System.Windows.Forms.Padding(3);
            this.tabSourceSite.Size = new System.Drawing.Size(792, 674);
            this.tabSourceSite.TabIndex = 0;
            this.tabSourceSite.Text = "Generate Site Template";
            this.tabSourceSite.UseVisualStyleBackColor = true;
            // 
            // btnXMlFileWithListData
            // 
            this.btnXMlFileWithListData.Location = new System.Drawing.Point(573, 436);
            this.btnXMlFileWithListData.Name = "btnXMlFileWithListData";
            this.btnXMlFileWithListData.Size = new System.Drawing.Size(128, 29);
            this.btnXMlFileWithListData.TabIndex = 21;
            this.btnXMlFileWithListData.Text = "XML File With Data";
            this.btnXMlFileWithListData.UseVisualStyleBackColor = true;
            this.btnXMlFileWithListData.Click += new System.EventHandler(this.btnXMlFileWithListData_Click);
            // 
            // lblSourceSiteErrorLog
            // 
            this.lblSourceSiteErrorLog.AutoSize = true;
            this.lblSourceSiteErrorLog.Location = new System.Drawing.Point(70, 596);
            this.lblSourceSiteErrorLog.Name = "lblSourceSiteErrorLog";
            this.lblSourceSiteErrorLog.Size = new System.Drawing.Size(0, 13);
            this.lblSourceSiteErrorLog.TabIndex = 20;
            // 
            // SourceSiteProgressBar
            // 
            this.SourceSiteProgressBar.Location = new System.Drawing.Point(185, 529);
            this.SourceSiteProgressBar.Name = "SourceSiteProgressBar";
            this.SourceSiteProgressBar.Size = new System.Drawing.Size(426, 23);
            this.SourceSiteProgressBar.TabIndex = 19;
            // 
            // btnLoadList
            // 
            this.btnLoadList.Location = new System.Drawing.Point(669, 77);
            this.btnLoadList.Name = "btnLoadList";
            this.btnLoadList.Size = new System.Drawing.Size(75, 23);
            this.btnLoadList.TabIndex = 18;
            this.btnLoadList.Text = "Load List";
            this.btnLoadList.UseVisualStyleBackColor = true;
            this.btnLoadList.Click += new System.EventHandler(this.btnLoadList_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(200, 436);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(109, 34);
            this.btnBack.TabIndex = 17;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // ChkListName
            // 
            this.ChkListName.FormattingEnabled = true;
            this.ChkListName.Location = new System.Drawing.Point(151, 228);
            this.ChkListName.MultiColumn = true;
            this.ChkListName.Name = "ChkListName";
            this.ChkListName.Size = new System.Drawing.Size(512, 124);
            this.ChkListName.TabIndex = 16;
            this.ChkListName.SelectedIndexChanged += new System.EventHandler(this.ChkListName_SelectedIndexChanged);
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Location = new System.Drawing.Point(58, 140);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(79, 13);
            this.lblSelect.TabIndex = 15;
            this.lblSelect.Text = "Select Schema";
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
            this.btnSourceBrowse.Location = new System.Drawing.Point(588, 358);
            this.btnSourceBrowse.Name = "btnSourceBrowse";
            this.btnSourceBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnSourceBrowse.TabIndex = 13;
            this.btnSourceBrowse.Text = "Browse";
            this.btnSourceBrowse.UseVisualStyleBackColor = true;
            this.btnSourceBrowse.Click += new System.EventHandler(this.btnSourceBrowse_Click);
            // 
            // txtSourceFileName
            // 
            this.txtSourceFileName.Location = new System.Drawing.Point(151, 394);
            this.txtSourceFileName.Name = "txtSourceFileName";
            this.txtSourceFileName.Size = new System.Drawing.Size(512, 20);
            this.txtSourceFileName.TabIndex = 12;
            // 
            // lblSourceFileName
            // 
            this.lblSourceFileName.AutoSize = true;
            this.lblSourceFileName.Location = new System.Drawing.Point(56, 401);
            this.lblSourceFileName.Name = "lblSourceFileName";
            this.lblSourceFileName.Size = new System.Drawing.Size(82, 13);
            this.lblSourceFileName.TabIndex = 11;
            this.lblSourceFileName.Text = "Template Name";
            // 
            // btnXMLFile
            // 
            this.btnXMLFile.Location = new System.Drawing.Point(372, 436);
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
            this.lblListName.Location = new System.Drawing.Point(56, 244);
            this.lblListName.Name = "lblListName";
            this.lblListName.Size = new System.Drawing.Size(54, 13);
            this.lblListName.TabIndex = 9;
            this.lblListName.Text = "List Name";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(56, 373);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(95, 13);
            this.lblFileName.TabIndex = 7;
            this.lblFileName.Text = "Template Location";
            // 
            // txtFileLocation
            // 
            this.txtFileLocation.Location = new System.Drawing.Point(151, 366);
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
            this.txtPassword.Text = "Tiger#digital87";
            this.txtPassword.UseSystemPasswordChar = true;
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
            this.txtUsername.Text = "admin@digitalworkhive.onmicrosoft.com";
            // 
            // lblSourceSiteName
            // 
            this.lblSourceSiteName.AutoSize = true;
            this.lblSourceSiteName.Location = new System.Drawing.Point(54, 9);
            this.lblSourceSiteName.Name = "lblSourceSiteName";
            this.lblSourceSiteName.Size = new System.Drawing.Size(50, 13);
            this.lblSourceSiteName.TabIndex = 1;
            this.lblSourceSiteName.Text = "Site URL";
            // 
            // txtSourceSiteName
            // 
            this.txtSourceSiteName.Location = new System.Drawing.Point(151, 6);
            this.txtSourceSiteName.Name = "txtSourceSiteName";
            this.txtSourceSiteName.Size = new System.Drawing.Size(512, 20);
            this.txtSourceSiteName.TabIndex = 0;
            this.txtSourceSiteName.Text = "https://digitalworkhive.sharepoint.com//sites/iacgroup";
            // 
            // tabTargetSite
            // 
            this.tabTargetSite.Controls.Add(this.btnDataMigration);
            this.tabTargetSite.Controls.Add(this.btnAddGroups);
            this.tabTargetSite.Controls.Add(this.lblTargetSiteErrorLog);
            this.tabTargetSite.Controls.Add(this.TargetSiteProgressBar);
            this.tabTargetSite.Controls.Add(this.txtDesXMLFileName);
            this.tabTargetSite.Controls.Add(this.btnBrowse);
            this.tabTargetSite.Controls.Add(this.btnSiteMigrate);
            this.tabTargetSite.Controls.Add(this.lblDesFileName);
            this.tabTargetSite.Controls.Add(this.txtDesPassword);
            this.tabTargetSite.Controls.Add(this.lblDesPassword);
            this.tabTargetSite.Controls.Add(this.txtDesUsername);
            this.tabTargetSite.Controls.Add(this.lblDesUsename);
            this.tabTargetSite.Controls.Add(this.txtDestinationSiteName);
            this.tabTargetSite.Controls.Add(this.lblDestinationSiteName);
            this.tabTargetSite.Location = new System.Drawing.Point(4, 22);
            this.tabTargetSite.Name = "tabTargetSite";
            this.tabTargetSite.Padding = new System.Windows.Forms.Padding(3);
            this.tabTargetSite.Size = new System.Drawing.Size(792, 674);
            this.tabTargetSite.TabIndex = 1;
            this.tabTargetSite.Text = "Apply Site Template";
            this.tabTargetSite.UseVisualStyleBackColor = true;
            // 
            // btnAddGroups
            // 
            this.btnAddGroups.Location = new System.Drawing.Point(191, 233);
            this.btnAddGroups.Name = "btnAddGroups";
            this.btnAddGroups.Size = new System.Drawing.Size(91, 35);
            this.btnAddGroups.TabIndex = 16;
            this.btnAddGroups.Text = "Add Groups";
            this.btnAddGroups.UseVisualStyleBackColor = true;
            this.btnAddGroups.Click += new System.EventHandler(this.btnAddGroups_Click);
            // 
            // lblTargetSiteErrorLog
            // 
            this.lblTargetSiteErrorLog.AutoSize = true;
            this.lblTargetSiteErrorLog.Location = new System.Drawing.Point(69, 412);
            this.lblTargetSiteErrorLog.Name = "lblTargetSiteErrorLog";
            this.lblTargetSiteErrorLog.Size = new System.Drawing.Size(0, 13);
            this.lblTargetSiteErrorLog.TabIndex = 15;
            // 
            // TargetSiteProgressBar
            // 
            this.TargetSiteProgressBar.Location = new System.Drawing.Point(195, 317);
            this.TargetSiteProgressBar.Name = "TargetSiteProgressBar";
            this.TargetSiteProgressBar.Size = new System.Drawing.Size(369, 23);
            this.TargetSiteProgressBar.TabIndex = 14;
            this.TargetSiteProgressBar.Value = 100;
            // 
            // txtDesXMLFileName
            // 
            this.txtDesXMLFileName.Location = new System.Drawing.Point(195, 160);
            this.txtDesXMLFileName.Name = "txtDesXMLFileName";
            this.txtDesXMLFileName.Size = new System.Drawing.Size(293, 20);
            this.txtDesXMLFileName.TabIndex = 10;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(494, 160);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(84, 23);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click_1);
            // 
            // btnSiteMigrate
            // 
            this.btnSiteMigrate.Location = new System.Drawing.Point(344, 233);
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
            this.lblDesFileName.Location = new System.Drawing.Point(69, 163);
            this.lblDesFileName.Name = "lblDesFileName";
            this.lblDesFileName.Size = new System.Drawing.Size(82, 13);
            this.lblDesFileName.TabIndex = 6;
            this.lblDesFileName.Text = "Template Name";
            // 
            // txtDesPassword
            // 
            this.txtDesPassword.Location = new System.Drawing.Point(195, 123);
            this.txtDesPassword.Name = "txtDesPassword";
            this.txtDesPassword.Size = new System.Drawing.Size(383, 20);
            this.txtDesPassword.TabIndex = 5;
            this.txtDesPassword.Text = "Lion#tech111";
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
            this.txtDesUsername.Text = "admin@techworkhive.onmicrosoft.com";
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
            this.txtDestinationSiteName.Text = "https://techworkhive.sharepoint.com/sites/iacgrouptestv1";
            // 
            // lblDestinationSiteName
            // 
            this.lblDestinationSiteName.AutoSize = true;
            this.lblDestinationSiteName.Location = new System.Drawing.Point(68, 49);
            this.lblDestinationSiteName.Name = "lblDestinationSiteName";
            this.lblDestinationSiteName.Size = new System.Drawing.Size(50, 13);
            this.lblDestinationSiteName.TabIndex = 0;
            this.lblDestinationSiteName.Text = "Site URL";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnDataMigration
            // 
            this.btnDataMigration.Location = new System.Drawing.Point(591, 233);
            this.btnDataMigration.Name = "btnDataMigration";
            this.btnDataMigration.Size = new System.Drawing.Size(129, 35);
            this.btnDataMigration.TabIndex = 17;
            this.btnDataMigration.Text = "Data Migrate";
            this.btnDataMigration.UseVisualStyleBackColor = true;
            this.btnDataMigration.Click += new System.EventHandler(this.btnDataMigration_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 749);
            this.Controls.Add(this.tabSiteMigration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Site Migration";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabSiteMigration.ResumeLayout(false);
            this.tabSiteBuilder.ResumeLayout(false);
            this.tabSiteBuilder.PerformLayout();
            this.pnlProvision.ResumeLayout(false);
            this.pnlProvision.PerformLayout();
            this.tabSourceSite.ResumeLayout(false);
            this.tabSourceSite.PerformLayout();
            this.tabTargetSite.ResumeLayout(false);
            this.tabTargetSite.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSiteMigration;
        private System.Windows.Forms.TabPage tabSourceSite;
        private System.Windows.Forms.TabPage tabTargetSite;
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
        private System.Windows.Forms.TabPage tabSiteBuilder;
        private System.Windows.Forms.Button btnProvision;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtSiteURLTitle;
        private System.Windows.Forms.TextBox txtSiteUrlName;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblSiteTitle;
        private System.Windows.Forms.Label lblSiteUrl;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Panel pnlProvision;
        private System.Windows.Forms.Label lblSiteCreation;
        private System.Windows.Forms.RadioButton rbSiteReplica;
        private System.Windows.Forms.RadioButton rbNewSite;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.TextBox txtTemplateName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button btnAssetPath;
        private System.Windows.Forms.TextBox txtAssetsPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbExistingSite;
        private System.Windows.Forms.Button btnLoadList;
        private System.Windows.Forms.ProgressBar SourceSiteProgressBar;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar SiteBuilderProgressBar;
        private System.Windows.Forms.ProgressBar TargetSiteProgressBar;
        private System.Windows.Forms.Label lblTargetSiteErrorLog;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblSourceSiteErrorLog;
        private System.Windows.Forms.Label lblSiteBuilderErrorLog;
        private System.Windows.Forms.Button btnAddGroups;
        private System.Windows.Forms.Button btnXMlFileWithListData;
        private System.Windows.Forms.Button btnDataMigration;
    }
}

