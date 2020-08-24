using System;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using System.Security;
using System.Threading;


namespace SiteMigrationProject
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class ListItems
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
        private void BindListName(string SourceWebUrl, string SourceUserName, SecureString SourcePassword)
        {
            using (var ctx = new ClientContext(SourceWebUrl))
            {
                try
                {
                    ctx.Credentials = new SharePointOnlineCredentials(SourceUserName, SourcePassword);
                    ctx.RequestTimeout = Timeout.Infinite;
                    // Just to output the site details
                    Web web = ctx.Web;
                    ctx.Load(web);
                    ctx.ExecuteQueryRetry();
                    ListCollection listColl = web.Lists;
                    // Retrieve the list collection properties    
                    ctx.Load(listColl);
                    // Execute the query to the server.    
                    ctx.ExecuteQueryRetry();
                    // Loop through all the list    
                    foreach (List list in listColl)
                    {
                        ctx.Load(list.RootFolder);
                        // Execute the query to the server.    
                        ctx.ExecuteQueryRetry();
                        if (list.BaseType == BaseType.GenericList)
                        {
                            ChkListName.Items.Add(new System.Web.UI.WebControls.ListItem(list.Title, list.RootFolder.Name));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

        }
        private void btnSiteMigrate_Click(object sender, EventArgs e)
        {
            try
            {
                string StrTargetWebUrl = txtDestinationSiteName.Text;
                string StrUserName = txtDesUsername.Text;
                string StrPassword = txtDesPassword.Text;
                string StrFileLocation = txtFileLocation.Text;
                string StrFileName = txtDesXMLFileName.Text;
                SecureString SecurePassword = new SecureString();
                foreach (char c in StrPassword.ToCharArray()) SecurePassword.AppendChar(c);
                // APPLY the template to new site from
                ApplyProvisioningTemplate(StrTargetWebUrl, StrUserName, SecurePassword, StrFileLocation, StrFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

        private void btnXMLFile_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect information
                string StrSourceWebUrl = txtSourceSiteName.Text;
                string StrUserName = txtUsername.Text;
                string StrPassword = txtPassword.Text;
                string StrLocation = txtFileLocation.Text;
                SecureString SecurePassword = new SecureString();
                foreach (char c in StrPassword.ToCharArray()) SecurePassword.AppendChar(c);

                // GET the template from existing site and serialize
                // Serializing the template for later reuse is optional
                GetProvisioningTemplate(StrSourceWebUrl, StrUserName, SecurePassword, StrLocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }



        }
        private void GetProvisioningTemplate(string SourceWebUrl, string SourceUserName, SecureString SourcePassword, String SourceFileLocation)
        {
            using (var ctx = new ClientContext(SourceWebUrl))
            {
                try
                {
                    ctx.Credentials = new SharePointOnlineCredentials(SourceUserName, SourcePassword);
                    ctx.RequestTimeout = Timeout.Infinite;
                    // Just to output the site details
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title);
                    ctx.ExecuteQueryRetry();

                    ProvisioningTemplateCreationInformation ptci
                            = new ProvisioningTemplateCreationInformation(ctx.Web);
                    // Create FileSystemConnector to store a temporary copy of the template
                    ptci.FileConnector = new FileSystemConnector(SourceFileLocation, "");
                    // Execute actual extraction of the template
                    ProvisioningTemplate template = ctx.Web.GetProvisioningTemplate(ptci);
                    FileSystemConnector connector = new FileSystemConnector(SourceFileLocation, "");
                    template.Connector = connector;
                    string StrFileName = txtSourceFileName.Text;
                    bool IsAllSelected = false;
                    bool IsList = false, IsNavigation = false, IsSiteField = false, IsContentType = false, IsFiles = false, IsPage = false, IsCustomAction = false, IsFeatures = false, IsWorkFlows = false, IsSiteHeader = false, IsSiteFooter = false, IsTheme = false;
                    bool IsRegionalSetting = false, IsSitePolicy = false, IsSupportedUILanguages = false, IsAuditSettings = false, IsPropertyBagEntries = false, IsSecurity = false, IsComposedLook = false, IsPublishing = false, IsTenant = false;
                    bool IsTermGroups = false, IsSearchSetting = false, IsWebSettings = false, IsImageRenditions = false, IsApplicationLifecycleManagement = false, IsWebApiPermissions = false;

                    foreach (object itemChecked in chkTemplateOptions.CheckedItems)
                    {
                        if (itemChecked.ToString() == "All")
                        {
                            try
                            {
                                IsAllSelected = true;
                                if (ChkListName.CheckedItems.Count > 0)
                                {
                                    ListInstanceCollection LstInstanceColl = new ListInstanceCollection(template);
                                    foreach (var item in ChkListName.CheckedItems)
                                    {
                                        ListInstance ListInstance = template.Lists.Find(listTemp => listTemp.Title == ((System.Web.UI.WebControls.ListItem)item).Text);
                                        LstInstanceColl.Add(ListInstance);
                                    }
                                    template.Lists.Clear();
                                    foreach (var listInstance in LstInstanceColl)
                                    {
                                        template.Lists.Add(listInstance);
                                    }

                                    IsList = true;
                                }
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                           
                        }
                        else
                        {
                            if (itemChecked.ToString() == "Lists")
                            {
                                ListInstanceCollection LstInstanceColl = new ListInstanceCollection(template);
                                if (ChkListName.CheckedItems.Count > 0)
                                {
                                    try
                                    {
                                        foreach (var item in ChkListName.CheckedItems)
                                        {
                                            ListInstance ListInstance = template.Lists.Find(listTemp => listTemp.Title == ((System.Web.UI.WebControls.ListItem)item).Text);
                                            LstInstanceColl.Add(ListInstance);
                                        }
                                        template.Lists.Clear();
                                        foreach (var listInstance in LstInstanceColl)
                                        {
                                            template.Lists.Add(listInstance);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                 
                                }


                                IsList = true;
                            }
                            if (itemChecked.ToString() == "Navigation")
                            {
                                IsNavigation = true;
                            }
                            if (itemChecked.ToString() == "CustomActions")
                            {
                                IsCustomAction = true;
                            }
                            if (itemChecked.ToString() == "Features")
                            {
                                IsFeatures = true;
                            }
                            if (itemChecked.ToString() == "Files")
                            {
                                IsFiles = true;
                            }
                            if (itemChecked.ToString() == "Pages")
                            {
                                IsPage = true;
                            }
                            if (itemChecked.ToString() == "RegionalSetting")
                            {
                                IsRegionalSetting = true;
                            }
                            if (itemChecked.ToString() == "SearchSetting")
                            {
                                IsSearchSetting = true;
                            }
                            if (itemChecked.ToString() == "ContentTypes")
                            {
                                IsContentType = true;
                            }
                            if (itemChecked.ToString() == "SiteFields")
                            {
                                IsSiteField = true;
                            }
                            if (itemChecked.ToString() == "SiteHeader")
                            {
                                IsSiteHeader = true;
                            }
                            if (itemChecked.ToString() == "SiteFooter")
                            {
                                IsSiteFooter = true;
                            }
                            if (itemChecked.ToString() == "Theme")
                            {
                                IsTheme = true;
                            }
                            if (itemChecked.ToString() == "Workflows")
                            {
                                IsWorkFlows = true;

                            }
                            if (itemChecked.ToString() == "SitePolicy")
                            {
                                IsSitePolicy = true;
                            }
                            if (itemChecked.ToString() == "SupportedUILanguages")
                            {
                                IsSupportedUILanguages = true;
                            }
                            if (itemChecked.ToString() == "AuditSettings")
                            {
                                IsAuditSettings = true;
                            }
                            if (itemChecked.ToString() == "PropertyBagEntries")
                            {
                                IsPropertyBagEntries = true;
                            }
                            if (itemChecked.ToString() == "Security")
                            {
                                IsSecurity = true;
                            }
                            if (itemChecked.ToString() == "ComposedLook")
                            {
                                IsComposedLook = true;
                            }
                            if (itemChecked.ToString() == "Publishing")
                            {
                                IsPublishing = true;
                            }
                            if (itemChecked.ToString() == "Tenant")
                            {
                                IsTenant = true;
                            }
                            if (itemChecked.ToString() == "TermGroups")
                            {
                                IsTermGroups = true;
                            }
                            if (itemChecked.ToString() == "WebApiPermissions")
                            {
                                IsWebApiPermissions = true;
                            }
                            if (itemChecked.ToString() == "WebSettings")
                            {
                                IsWebSettings = true;
                            }
                            if (itemChecked.ToString() == "ImageRenditions")
                            {
                                IsImageRenditions = true;
                            }
                            if (itemChecked.ToString() == "ApplicationLifecycleManagement")
                            {
                                IsApplicationLifecycleManagement = true;
                            }
                        }

                    }
                    if (IsAllSelected == false)
                    {
                        if (IsCustomAction == false)
                        {
                            template.CustomActions = null;
                        }
                        if (IsNavigation == false)
                        {
                            template.Navigation = null;
                        }
                        if (IsSiteField == false)
                        {
                            template.SiteFields.Clear();
                        }
                        if (IsContentType == false)
                        {
                            template.ContentTypes.Clear();
                        }
                        if (IsFiles == false)
                        {
                            template.Files.Clear();
                        }
                        if (IsPage == false)
                        {
                            template.Pages.Clear();
                        }
                        if (IsFeatures == false)
                        {
                            template.Features = null;
                        }
                        if (IsRegionalSetting == false)
                        {
                            template.RegionalSettings = null;
                        }
                        if (IsSearchSetting == false)
                        {
                            template.SiteSearchSettings = null;
                        }
                        if (IsList == false)
                        {
                            template.Lists.Clear();
                        }
                        if (IsSecurity == false)
                        {
                            if (template.Security != null)
                                template.Security = null;

                        }
                        if (IsAuditSettings == false)
                        {
                            if (template.AuditSettings != null)
                                template.AuditSettings = null;
                        }
                        if (IsPropertyBagEntries == false)
                        {
                            template.PropertyBagEntries.Clear();
                        }
                        if (IsComposedLook == false)
                        {
                            template.ComposedLook = null;
                        }
                        if (IsWorkFlows == false)
                        {
                            template.Workflows = null;
                        }
                        if (IsSitePolicy == false)
                        {
                            template.SitePolicy = null;
                        }
                        if (IsSupportedUILanguages == false)
                        {
                            template.SupportedUILanguages.Clear();
                        }
                        if (IsPublishing == false)
                        {
                            if (template.Publishing != null)
                                template.Publishing = null;

                        }
                        if (IsTenant == false)
                        {
                            if (template.Tenant != null)
                                template.Tenant = null;
                        }
                        if (IsSiteHeader == false)
                        {
                            template.Header = null;

                        }
                        if (IsSiteFooter == false)
                        {
                            if (template.Footer != null)
                                template.Footer = null;

                        }
                        if (IsTheme == false)
                        {
                            if (template.Theme != null)
                                template.Theme = null;

                        }
                        if (IsTermGroups == false)
                        {
                            template.TermGroups.Clear();
                        }
                        if (IsWebApiPermissions == false)
                        {
                            if (template.Tenant != null)
                                template.Tenant.WebApiPermissions.Clear();
                        }
                        if (IsApplicationLifecycleManagement == false)
                        {
                            template.ApplicationLifecycleManagement = null;
                        }

                        if (IsWebSettings == false)
                        {
                            template.WebSettings = null;
                        }
                        if (IsImageRenditions == false)
                        {
                            if (template.Publishing != null)
                                template.Publishing.ImageRenditions.Clear();
                        }

                    }
                    // We can serialize this template to save and reuse it
                    // Optional step
                    XMLTemplateProvider provider =
                                new XMLFileSystemTemplateProvider(SourceFileLocation, "");
                    provider.SaveAs(template, StrFileName + ".xml");
                    //return template;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }
        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    txtDesXMLFileName.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        private void ApplyProvisioningTemplate(string TargetWebUrl, string TargetUserName, SecureString TargetSecurePassword, String TargetLocation, String TargetFile)
        {
            using (var ctx = new ClientContext(TargetWebUrl))
            {
                try
                {
                    ctx.Credentials = new SharePointOnlineCredentials(TargetUserName, TargetSecurePassword);
                    ctx.RequestTimeout = Timeout.Infinite;
                    // Just to output the site details
                    Web web = ctx.Web;
                    ctx.Load(web, w => w.Title);
                    ctx.ExecuteQueryRetry();
                    ProvisioningTemplateApplyingInformation ptai = new ProvisioningTemplateApplyingInformation();
                    XMLFileSystemTemplateProvider provider = new XMLFileSystemTemplateProvider(TargetLocation, TargetFile);
                    ProvisioningTemplate p2 = provider.GetTemplate(TargetFile);
                    web.ApplyProvisioningTemplate(p2, ptai);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
            // return template;
        }

        private void btnSourceBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    txtFileLocation.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void chkTemplateOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkTemplateOptions.SelectedItem == chkTemplateOptions.Items[0] && chkTemplateOptions.GetItemChecked(0) == true)
                {
                    for (int i = 0; i < chkTemplateOptions.Items.Count; i++)
                    {
                        chkTemplateOptions.SetItemChecked(i, true);
                    }
                }
                if (chkTemplateOptions.SelectedItem == chkTemplateOptions.Items[0] && chkTemplateOptions.GetItemChecked(0) == false)
                {
                    for (int i = 0; i < chkTemplateOptions.Items.Count; i++)
                    {
                        chkTemplateOptions.SetItemChecked(i, false);
                    }
                }
                for (int i = 0; i < chkTemplateOptions.Items.Count; i++)
                {
                    if (chkTemplateOptions.Items[i].ToString() == "Tenant" || chkTemplateOptions.Items[i].ToString() == "Workflows" || chkTemplateOptions.Items[i].ToString() == "TermGroups" || chkTemplateOptions.Items[i].ToString() == "Publishing" || chkTemplateOptions.Items[i].ToString() == "ImageRenditions" || chkTemplateOptions.Items[i].ToString() == "WebApiPermissions")
                    {
                        chkTemplateOptions.SetItemCheckState(i, CheckState.Indeterminate);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void txtSourceSiteName_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                string StrSourceWebUrl = txtSourceSiteName.Text;
                string StrUserName = txtUsername.Text;
                string StrPassword = txtPassword.Text;
                SecureString SecurePassword = new SecureString();
                foreach (char c in StrPassword.ToCharArray()) SecurePassword.AppendChar(c);
                // if (ChkListName.Items.Count == 0)
                // BindListName(StrSourceWebUrl, StrUserName, SecurePassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }


        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string StrSourceWebUrl = txtSourceSiteName.Text;
                string StrUserName = txtUsername.Text;
                string StrPassword = txtPassword.Text;
                SecureString SecurePassword = new SecureString();
                foreach (char c in StrPassword.ToCharArray()) SecurePassword.AppendChar(c);
                if (ChkListName.Items.Count == 0)
                    BindListName(StrSourceWebUrl, StrUserName, SecurePassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < chkTemplateOptions.Items.Count; i++)
            {
                if (chkTemplateOptions.Items[i].ToString() == "Tenant" || chkTemplateOptions.Items[i].ToString() == "Workflows" || chkTemplateOptions.Items[i].ToString() == "TermGroups" || chkTemplateOptions.Items[i].ToString() == "Publishing" || chkTemplateOptions.Items[i].ToString() == "ImageRenditions" || chkTemplateOptions.Items[i].ToString() == "WebApiPermissions")
                {
                    chkTemplateOptions.SetItemCheckState(i, CheckState.Indeterminate);
                }

            }

        }

    }
}
