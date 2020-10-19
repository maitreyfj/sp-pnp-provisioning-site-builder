using System;
using System.Windows.Forms;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using System.Security;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Field = Microsoft.SharePoint.Client.Field;
using List = Microsoft.SharePoint.Client.List;
using OfficeDevPnP.Core.Diagnostics;
using Microsoft.Online.SharePoint.TenantAdministration;
using System.IO;
using System.Collections;
using System.Xml;
using SP = Microsoft.SharePoint.Client;
using System.Xml.Linq;
using System.Text;

namespace SiteMigrationProject
{

    public partial class Form1 : System.Windows.Forms.Form
    {
        #region Fields
        //Initialize Variable
        private Dictionary<string, FieldValueProvider> m_dictFieldValueProviders = null;
        private Dictionary<Guid, ListItemsProvider> m_listContentProviders = null;
        bool XMLWithData = false;
        #endregion //Fields

        #region Constructors
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(List list, Web web, ProvisioningTemplate template)
        {
            this.List = list;
            this.Web = web;
        }
        #endregion //Constructors

        #region Properties
        public List List
        {
            get; private set;
        }
        public Web Web
        {
            get; private set;
        }

        #endregion //Properties

        #region Methods
        //Function use for Add listname on checked listbox.
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
                    int i = 10;
                    int ListCount = listColl.Count;
                    // Loop through all the list    
                    Thread.Sleep(100);
                    ChkListName.Items.Add("All");
                    foreach (List list in listColl)
                    {
                        ctx.Load(list.RootFolder);
                        // Execute the query to the server.    
                        ctx.ExecuteQueryRetry();
                        if (list.BaseType == BaseType.GenericList || list.BaseType == BaseType.DocumentLibrary)
                        {
                            ChkListName.Items.Add(new System.Web.UI.WebControls.ListItem(list.Title, list.RootFolder.Name));
                        }
                        if (ListCount > 0)
                        {
                            backgroundWorker1.ReportProgress(i);
                            i++;
                            ListCount--;
                        }
                        if (ListCount == 0)
                            SourceSiteProgressBar.Value = 100;
                    }

                }
                catch (Exception ex)
                {
                    lblSourceSiteErrorLog.Text = ex.ToString();
                }
            }
        }

        //This function is use for call the function MigrateListItems for Data migration.
        private void DataMigrate()
        {
            try
            {
                string StrSourceWebUrl = txtSourceSiteName.Text;
                string StrSourceUserName = txtUsername.Text;
                string StrSourcePassword = txtPassword.Text;
                SecureString SecureSourcePassword = new SecureString();
                foreach (char c in StrSourcePassword.ToCharArray()) SecureSourcePassword.AppendChar(c);
                string StrTargetWebUrl = txtDestinationSiteName.Text;
                string StrTargetUserName = txtDesUsername.Text;
                string StrTargetPassword = txtDesPassword.Text;
                SecureString SecureTargetPassword = new SecureString();
                foreach (char c in StrTargetPassword.ToCharArray()) SecureTargetPassword.AppendChar(c);
                var ctxSource = new ClientContext(StrSourceWebUrl);
                ctxSource.Credentials = new SharePointOnlineCredentials(StrSourceUserName, SecureSourcePassword);
                ctxSource.RequestTimeout = Timeout.Infinite;
                // Just to output the site details
                Web web = ctxSource.Web;
                ctxSource.Load(web, w => w.Title);
                ctxSource.ExecuteQuery();
                var ctxDest = new ClientContext(StrTargetWebUrl);
                ctxDest.Credentials = new SharePointOnlineCredentials(StrTargetUserName, SecureTargetPassword);
                ctxDest.RequestTimeout = Timeout.Infinite;
                // Just to output the site details
                Web Desweb = ctxDest.Web;
                ctxDest.Load(Desweb, w => w.Title);
                ctxDest.ExecuteQuery();
                List<string> lstGetSelectedLists = new List<string>();
                //Calling function.
                lstGetSelectedLists = GetSelectedValuesFromCheckBoxList(ChkListName);
                //Calling function.
                MigrateListItems(ctxSource, ctxDest, lstGetSelectedLists);
            }
            catch (Exception ex)
            {
                lblTargetSiteErrorLog.Text = ex.ToString();
            }


        }

        // This function is use for get the name of all selected list from checkbox.
        private List<string> GetSelectedValuesFromCheckBoxList(CheckedListBox chkBox)
        {
            List<string> lstResult = new List<string>();
            try
            {
                string strValues = string.Empty;
                for (int i = 0; i < chkBox.Items.Count; i++)
                {
                    if (chkBox.GetItemChecked(i)
                        && !string.IsNullOrEmpty(Convert.ToString(chkBox.GetItemText(chkBox.Items[i]))))
                    {
                        strValues += ";" + Convert.ToString(chkBox.GetItemText(chkBox.Items[i]));
                    }
                }
                strValues = strValues.TrimStart(';');

                if (!string.IsNullOrEmpty(strValues.Trim()))
                {
                    string[] arrValues = strValues.Split(';');

                    lstResult = arrValues.ToList<string>();
                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }
            return lstResult;
        }
        //This function is use for get the template of Source Site.
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
                    OfficeDevPnP.Core.Framework.Provisioning.Model.ProvisioningTemplate template = ctx.Web.GetProvisioningTemplate(ptci);
                    FileSystemConnector connector = new FileSystemConnector(SourceFileLocation, "");
                    template.Connector = connector;
                    string StrFileName = txtSourceFileName.Text;
                    bool IsAllSelected = false;
                    bool IsList = false, IsNavigation = false, IsSiteField = false, IsContentType = false, IsFiles = false, IsPage = false, IsCustomAction = false, IsFeatures = false, IsWorkFlows = false, IsSiteHeader = false, IsSiteFooter = false, IsTheme = false;
                    bool IsRegionalSetting = false, IsSitePolicy = false, IsSupportedUILanguages = false, IsAuditSettings = false, IsPropertyBagEntries = false, IsSecurity = false, IsComposedLook = false, IsPublishing = false, IsTenant = false;
                    bool IsTermGroups = false, IsSearchSetting = false, IsWebSettings = false, IsImageRenditions = false, IsApplicationLifecycleManagement = false, IsWebApiPermissions = false;
                    template.Navigation.GlobalNavigation.StructuralNavigation.RemoveExistingNodes = true;
                    template.Navigation.CurrentNavigation.StructuralNavigation.RemoveExistingNodes = true;
                    foreach (object itemChecked in chkTemplateOptions.CheckedItems)
                    {
                        if (itemChecked.ToString() == "All")
                        {
                            try
                            {
                                IsAllSelected = true;
                                if (XMLWithData == true)
                                {
                                    AddListItemWithData(ctx, template);
                                }
                                else
                                {
                                    AddListItemWithoutData(ctx, template, web);
                                }
                                break;
                            }
                            catch (Exception ex)
                            {
                                lblSourceSiteErrorLog.Text = ex.ToString();
                            }

                        }
                        else
                        {
                            if (itemChecked.ToString() == "Lists")
                            {
                                if (XMLWithData == true)
                                {
                                    AddListItemWithData(ctx, template);
                                }
                                else
                                {
                                    AddListItemWithoutData(ctx, template, web);
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
                            //if (itemChecked.ToString() == "ComposedLook")
                            //{
                            //    IsComposedLook = true;
                            //}
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


                    XMLTemplateProvider provider =
                                new XMLFileSystemTemplateProvider(SourceFileLocation, "");
                    provider.SaveAs(template, StrFileName + ".xml");
                    //return template;
                }
                catch (Exception ex)
                {
                    lblSourceSiteErrorLog.Text = ex.ToString();
                }

            }
        }
        //This function is use for get all list structure without data.
        private void AddListItemWithoutData(ClientContext ctx, ProvisioningTemplate template, Web web)
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
                    lblSourceSiteErrorLog.Text = ex.ToString();
                }

            }


        }
        //This function is use for get all list structure with data.
        private void AddListItemWithData(ClientContext ctx, ProvisioningTemplate template)
        {
            try
            {
                string StrSourceWebUrl = string.Empty;
                string StrSourceUserName = string.Empty;
                string StrSourcePassword = string.Empty;
                if (txtSourceSiteName.Text != string.Empty)
                {
                    StrSourceWebUrl = txtSourceSiteName.Text;
                }
                if (txtUsername.Text != string.Empty)
                {
                    StrSourceUserName = txtUsername.Text;
                }
                if (txtPassword.Text != string.Empty)
                {
                    StrSourcePassword = txtPassword.Text;
                }
                SecureString SecureSourcePassword = new SecureString();
                foreach (char c in StrSourcePassword.ToCharArray()) SecureSourcePassword.AppendChar(c);
                string StrTargetWebUrl = string.Empty;
                string StrTargetUserName = string.Empty;
                string StrTargetPassword = string.Empty;
                if (txtDestinationSiteName.Text != string.Empty)
                {
                    StrTargetWebUrl = txtDestinationSiteName.Text;
                }

                if (txtDesUsername.Text != string.Empty)
                {
                    StrTargetUserName = txtDesUsername.Text;
                }
                if (txtDesPassword.Text != string.Empty)
                {
                    StrTargetPassword = txtDesPassword.Text;
                }
                SecureString SecureTargetPassword = new SecureString();
                foreach (char c in StrTargetPassword.ToCharArray()) SecureTargetPassword.AppendChar(c);
                var ctxSource = new ClientContext(StrSourceWebUrl);
                ctxSource.Credentials = new SharePointOnlineCredentials(StrSourceUserName, SecureSourcePassword);
                ctxSource.RequestTimeout = Timeout.Infinite;
                // Just to output the site details
                Web web = ctxSource.Web;
                ctxSource.Load(web, w => w.Title);
                ctxSource.ExecuteQuery();
                var ctxDest = new ClientContext(StrTargetWebUrl);
                ctxDest.Credentials = new SharePointOnlineCredentials(StrTargetUserName, SecureTargetPassword);
                ctxDest.RequestTimeout = Timeout.Infinite;
                // Just to output the site details
                Web Desweb = ctxDest.Web;
                ctxDest.Load(Desweb, w => w.Title);
                ctxDest.ExecuteQuery();
                List<string> lstGetSelectedLists = new List<string>();
                lstGetSelectedLists = GetSelectedValuesFromCheckBoxList(ChkListName);
                ListInstanceCollection LstInstanceColl = new ListInstanceCollection(template);
                if (ChkListName.CheckedItems.Count > 0)
                {
                    try
                    {
                        ListCollection lstSourceLists = ctxSource.Web.Lists;
                        ctxSource.Load(lstSourceLists, x => x.Include(y => y.Title, y => y.BaseTemplate));
                        ctxSource.ExecuteQuery();

                        ListCollection lstDestLists = ctxDest.Web.Lists;
                        ctxDest.Load(lstDestLists, x => x.Include(y => y.Title, y => y.BaseTemplate));
                        ctxDest.ExecuteQuery();
                        if (lstSourceLists != null && lstDestLists != null && lstSourceLists.Count > 0 && lstDestLists.Count > 0)
                        {
                            string[] arrSourceLists = (from p in lstSourceLists
                                                       where
                                                           CommonHelper.AllowedBaseTemplates.Contains(p.BaseTemplate)
                                                       select p.Title).ToArray();
                            string[] arrDestLists = (from p in lstDestLists where CommonHelper.AllowedBaseTemplates.Contains(p.BaseTemplate) select p.Title).ToArray();

                            string[] arrCommonLists = arrSourceLists.Intersect(arrDestLists).ToArray();

                            arrCommonLists = arrCommonLists.Where(y => lstGetSelectedLists.Contains(y)).ToArray();

                            if (arrCommonLists.Length > 0)
                            {
                                List<LookupColumnDetails> lstLookupColumnDetails = new List<LookupColumnDetails>();
                                // ShowUpdates("Fetching List Lookup Column Details : " + DateTime.Now);

                                Dictionary<string, List<string>> dicListLookupDetails = GetListLookupDetailsDictionary(ctxSource, arrCommonLists,
                                    ref lstLookupColumnDetails);

                                if (dicListLookupDetails != null && dicListLookupDetails.Count > 0)
                                {
                                    List<string> lstSelfLookupLists = new List<string>();
                                    List<string> lstFinalListOrder = new List<string>();
                                    List<string> lstParentLists = new List<string>();

                                    //  ShowUpdates("Identifying List Migration Order : " + DateTime.Now);
                                    ProcessLists(ctxSource, ref dicListLookupDetails, ref lstFinalListOrder, ref lstSelfLookupLists,
                                        ref lstParentLists);

                                    //if (dicListLookupDetails.Count == 0)
                                    //{
                                    //string strListOrder = string.Empty;
                                    StringBuilder sb = new StringBuilder();

                                    sb.Append(Environment.NewLine);
                                    sb.Append("LIST MIGRATION ORDER (based on the lookup Columns)");
                                    sb.Append(Environment.NewLine);
                                    sb.Append(Environment.NewLine);

                                    // all lists order has been set
                                    foreach (var t in lstFinalListOrder)
                                    {
                                        sb.Append(t);
                                        sb.Append(Environment.NewLine);
                                    }

                                    sb.Append(Environment.NewLine);
                                    sb.Append("LIST Deletion Script Lists");
                                    sb.Append(Environment.NewLine);
                                    sb.Append(Environment.NewLine);

                                    // all lists order has been set
                                    foreach (var t in lstFinalListOrder.Reverse<string>())
                                    {
                                        sb.Append(t);
                                        sb.Append(",");
                                    }

                                    lblSourceSiteErrorLog.Text = sb.ToString();

                                    foreach (var chkitem in lstFinalListOrder)
                                    {
                                        ListInstance ListInstance = template.Lists.Find(listTemp => listTemp.Title == chkitem);
                                        ///
                                        Dictionary<string, string> dataValues = new Dictionary<string, string>();
                                        List ListObject = ctx.Web.Lists.GetByTitle(chkitem);
                                        ctx.Load(ListObject);
                                        ctx.ExecuteQuery();
                                        // This creates a CamlQuery" 
                                        // so that it grabs all list items, regardless of the folder they are in. 
                                        CamlQuery query = CamlQuery.CreateAllItemsQuery();
                                        ListItemCollection itemCollection = ListObject.GetItems(query);
                                        // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
                                        ctx.Load(itemCollection);
                                        ctx.ExecuteQuery();
                                        Microsoft.SharePoint.Client.FieldCollection fieldscoll = ListObject.Fields;
                                        ctx.Load(fieldscoll);
                                        ctx.ExecuteQuery();
                                        List<Field> fields = new List<Field>();
                                        foreach (Field field in fieldscoll)
                                        {
                                            if (CanFieldContentBeIncluded(field, true, ListObject))
                                            {
                                                fields.Add(field);
                                            }
                                        }
                                        foreach (ListItem item in itemCollection)
                                        {
                                            try
                                            {
                                                Dictionary<string, string> values = new Dictionary<string, string>();
                                                foreach (Microsoft.SharePoint.Client.Field field in fields)
                                                {
                                                    if (CanFieldContentBeIncluded(field, true, ListObject))
                                                    {
                                                        string str = "";
                                                        object value = null; ;
                                                        try
                                                        {
                                                            value = item[field.InternalName];
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            lblSourceSiteErrorLog.Text = ex.ToString();
                                                        }
                                                        if (null != value)
                                                        {
                                                            try
                                                            {
                                                                FieldValueProvider Fieldprovider = GetFieldValueProvider(field, web);
                                                                str = Fieldprovider.GetValidatedValue(value);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                lblSourceSiteErrorLog.Text = ex.ToString();
                                                            }
                                                            if (!string.IsNullOrEmpty(str))
                                                            {
                                                                values.Add(field.InternalName, str);
                                                            }
                                                        }
                                                    }
                                                }
                                                DataRow dataRow = new DataRow(values);
                                                ListInstance.DataRows.Add(dataRow);
                                            }
                                            catch (Exception ex)
                                            {
                                                lblSourceSiteErrorLog.Text = ex.ToString();
                                            }
                                        }
                                        LstInstanceColl.Add(ListInstance);


                                    }
                                    template.Lists.Clear();
                                    foreach (var listInstance in LstInstanceColl)
                                    {
                                        template.Lists.Add(listInstance);
                                    }
                                }
                                else
                                {
                                    // Process further
                                }
                                //}
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblSourceSiteErrorLog.Text = ex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }

        }
        //This function is use for update lookup value in list after data is inserted.
        private void UpdateLookupValues(Web web)
        {
            try
            {
                if (null != m_listContentProviders)
                {
                    foreach (KeyValuePair<Guid, ListItemsProvider> pair in m_listContentProviders)
                    {
                        Guid listId = pair.Key;
                        ListItemsProvider provider = pair.Value;

                        provider.UpdateLookups(GetLookupDependentProvider);
                    }
                }
            }
            catch (Exception ex)
            {
                lblTargetSiteErrorLog.Text = ex.ToString();
            }
            
        }
        //This function is use for to get the lookup dependent provider.
        private ListItemsProvider GetLookupDependentProvider(Guid listId)
        {
            try
            {
                ListItemsProvider provider;
                if ((null != m_listContentProviders) && m_listContentProviders.TryGetValue(listId, out provider))
                {
                    return provider;
                }
               
            }
            catch (Exception ex)
            {
                lblTargetSiteErrorLog.Text = ex.ToString();
            }
            return null;
        }
        //This function is use for apply the template for target site.
        private void ApplyProvisioningTemplate(string TargetWebUrl, string TargetUserName, SecureString TargetSecurePassword, String TargetLocation, String TargetFile)
        {
            using (var ClientContext = new ClientContext(TargetWebUrl))
            {
                try
                {
                    ClientContext.Credentials = new SharePointOnlineCredentials(TargetUserName, TargetSecurePassword);
                    ClientContext.RequestTimeout = Timeout.Infinite;
                    // Just to output the site details
                    Web web = ClientContext.Web;
                    ClientContext.Load(web, w => w.Title);
                    ClientContext.ExecuteQueryRetry();

                    RemoveCalculatedColumnBeforeApplyTemplate(ClientContext);
                    ProvisioningTemplateApplyingInformation ptai = new ProvisioningTemplateApplyingInformation();
                    XMLFileSystemTemplateProvider provider = new XMLFileSystemTemplateProvider(TargetLocation, TargetFile);
                    OfficeDevPnP.Core.Framework.Provisioning.Model.ProvisioningTemplate ApplyTemplateFile = provider.GetTemplate(TargetFile);
                    
                    //****************
                    //This code is use for replace the ";#"  with "," to solve the issue of multilookup values.

                    //XmlDocument xmlDoc = new XmlDocument();
                    //xmlDoc.LoadXml(ApplyTemplateFile.ToXML());
                    //XDocument doc = XDocument.Load(TargetFile);
                    //xmlDoc.InnerXml = xmlDoc.InnerXml.Replace(";#", ",");
                    //xmlDoc.Save(TargetFile);
                    //OfficeDevPnP.Core.Framework.Provisioning.Model.ProvisioningTemplate AfterReplaceApplyTemplateFile = provider.GetTemplate(TargetFile);
                    //ReplaceAll(element, doc, ApplyTemplateFile);

                    //************
                    var parser = new TokenParser(ClientContext.Web, ApplyTemplateFile);
                    foreach (var listInstance in ApplyTemplateFile.Lists)
                    {
                        if (listInstance.DataRows != null && listInstance.DataRows.Any())
                        {
                            // Retrieve the target list
                            var list = web.Lists.GetByTitle(listInstance.Title);
                            web.Context.Load(list);
                            web.Context.ExecuteQueryRetry();

                            ListItemsProvider Listitemprovider = new ListItemsProvider(list, web, ApplyTemplateFile);
                            Listitemprovider.AddListItems(listInstance.DataRows, ApplyTemplateFile, parser);
                            if (null == m_listContentProviders)
                            {
                                m_listContentProviders = new Dictionary<Guid, ListItemsProvider>();
                            }
                            m_listContentProviders[list.Id] = Listitemprovider;
                        }
                    }

                    UpdateLookupValues(web);
                }
                catch (Exception ex)
                {
                    lblTargetSiteErrorLog.Text = ex.ToString();
                    if (ex.ToString().Contains("The formula refers to a column that does not exist.") || ex.ToString().Contains("Name cannot begin with the '\"' character, hexadecimal value 0x22. Line 1, position 321.") || ex.ToString().Contains("Input String was not in correct format"))
                    {
                        return;
                    }
                }

            }
            // return template;
        }
        //This function is use for copy the files and folder from one directory to other directoy. 
        private void DirectoryCopy(
            string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                DirectoryInfo[] dirs = dir.GetDirectories();

                // If the source directory does not exist, throw an exception.
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                DirectoryInfo dirInfo = new DirectoryInfo(destDirName);
                if (dirInfo.Exists == false)
                    System.IO.Directory.CreateDirectory(destDirName);


                // Get the file contents of the directory to copy.
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    // Create the path to the new copy of the file.
                    string temppath = Path.Combine(destDirName, file.Name);

                    // Copy the file.
                    file.CopyTo(temppath, false);
                }

                // If copySubDirs is true, copy the subdirectories.
                if (copySubDirs)
                {

                    foreach (DirectoryInfo subdir in dirs)
                    {
                        // Create the subdirectory.
                        string temppath = Path.Combine(destDirName, subdir.Name);

                        // Copy the subdirectories.
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }
            }
            catch (Exception ex)
            {
                lblSiteBuilderErrorLog.Text = ex.ToString();
            }
            
        }
        //Delete the folder and files from bin folder of project solution .
        private void DeleteDirectory(string path)
        {
            try
            {
                if (System.IO.Directory.Exists(path))
                {
                    //Delete all files from the Directory
                    foreach (string file in System.IO.Directory.GetFiles(path))
                    {
                        System.IO.File.Delete(file);
                    }
                    //Delete all child Directories
                    foreach (string directory in System.IO.Directory.GetDirectories(path))
                    {
                        DeleteDirectory(directory);
                    }
                    //Delete a Directory
                    System.IO.Directory.Delete(path);
                }
            }
            catch (Exception ex)
            {
              lblSiteBuilderErrorLog.Text=  ex.ToString();
            }
         
        }
        //This function is use for check the internal name and type of the field.
        private bool CanFieldContentBeIncluded(Field field, bool serialize, List ListObj)
        {
            bool result = false;
            try
            {
                if (field.InternalName.Equals("ID", StringComparison.OrdinalIgnoreCase))
                {
                    result = serialize;
                }
                else if (field.InternalName.Equals("ContentTypeId", StringComparison.OrdinalIgnoreCase) && ListObj.ContentTypesEnabled)
                {
                    result = true;
                }
                else if (field.InternalName.Equals("Attachments", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else
                {
                    if (!field.Hidden && !field.ReadOnlyField && (field.FieldTypeKind != FieldType.Computed))
                    {
                        //Temporary disabled for custom fields
                        if (field.FieldTypeKind != FieldType.Invalid)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }
            
            return result;
        }
        //This function is use for get the provider of field.
        private FieldValueProvider GetFieldValueProvider(Field field, Web web)
        {
            FieldValueProvider provider = null;
            try
            {
                if (null == m_dictFieldValueProviders)
                {
                    m_dictFieldValueProviders = new Dictionary<string, FieldValueProvider>();
                }
                if (!m_dictFieldValueProviders.TryGetValue(field.InternalName, out provider))
                {
                    provider = new FieldValueProvider(field, web);
                    m_dictFieldValueProviders.Add(field.InternalName, provider);
                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }
         
            return provider;
        }
        //This function is use for create the new site as
        private void CreateSite(string SiteURL, SecureString SourcePassword, string Email, string SiteTitle, int progress)
        {
            bool NewSite = false;
            string Tenantname = string.Empty;
            string[] splitEmail = Email.Split('@');
            string[] splitwithdot = splitEmail[1].Split('.');
            Tenantname = splitwithdot[0];
            string SiteUrlName = string.Empty;
            if (rbNewSite.Checked)
                SiteUrlName = "https://" + Tenantname + ".sharepoint.com/sites/" + SiteURL;
            else
                SiteUrlName = SiteURL;
            string strTemplateName = string.Empty;
            string strSiteTemplateString = string.Empty;
            strTemplateName = txtTemplateName.Text;
            if (rbNewSite.Checked)
                strSiteTemplateString = "STS#3";
            //if (chkContosoTeamSite.Checked)
            //{
            //    strTemplateName = "ContosoTeamSite.xml";
            //    strSiteTemplateString = "STS#3";
            //}
            //if (chkLeadershipSite.Checked)
            //{
            //    strTemplateName = "LeadershipSite.xml";
            //    strSiteTemplateString = "SITEPAGEPUBLISHING#0";
            //}
            //if (chkMarketingLanding.Checked)
            //{
            //    strTemplateName = "Landing.xml";
            //    strSiteTemplateString = "SITEPAGEPUBLISHING#0";
            //}
            //Open the Tenant Administration Context with the Tenant Admin Url
            using (ClientContext tenantContext = new ClientContext("https://" + Tenantname + "-admin.sharepoint.com/"))
            {
                if (rbNewSite.Checked)
                {
                    try
                    {
                        NewSite = true;
                        tenantContext.Credentials = new SharePointOnlineCredentials(Email, SourcePassword);
                        tenantContext.RequestTimeout = Timeout.Infinite;
                        // Just to output the site details
                        Web web = tenantContext.Web;
                        tenantContext.Load(web, w => w.Title);
                        tenantContext.ExecuteQueryRetry();
                        //Authenticate with a Tenant Administrator
                        tenantContext.Credentials = new SharePointOnlineCredentials("admin@" + Tenantname + ".onmicrosoft.com", SourcePassword);
                        var tenant = new Tenant(tenantContext);
                        //Properties of the New SiteCollection
                        var siteCreationProperties = new SiteCreationProperties();
                        //New SiteCollection Url
                        siteCreationProperties.Url = SiteUrlName;
                        //Title of the Root Site
                        siteCreationProperties.Title = SiteTitle;
                        //Login name of Owner
                        siteCreationProperties.Owner = Email;

                        //Template of the Root Site. Using Team Site for now.
                        siteCreationProperties.Template = strSiteTemplateString;
                        //Storage Limit in MB
                        siteCreationProperties.StorageMaximumLevel = 100;
                        //UserCode Resource Points Allowed
                        siteCreationProperties.UserCodeMaximumLevel = 50;
                        //Create the SiteCollection
                        SpoOperation spo = tenant.CreateSite(siteCreationProperties);
                        tenantContext.Load(tenant);
                        //We will need the IsComplete property to check if the provisioning of the Site Collection is complete.
                        tenantContext.Load(spo, i => i.IsComplete);
                        tenantContext.ExecuteQuery();
                        backgroundWorker1.ReportProgress(progress);
                        progress++;
                        //Check if provisioning of the SiteCollection is complete.
                        while (!spo.IsComplete)
                        {
                            //Wait for 30 seconds and then try again
                            System.Threading.Thread.Sleep(30000);
                            spo.RefreshLoad();
                            tenantContext.ExecuteQuery();
                        }
                        if (progress == 50)
                            SiteBuilderProgressBar.Value = progress;
                    }
                    catch (Exception ex)
                    {
                        lblSiteBuilderErrorLog.Text = ex.ToString();
                    }
                }
                else
                {
                    try
                    {
                        backgroundWorker1.ReportProgress(progress);
                        progress++;
                        ClientContext ctx = new ClientContext(SiteURL);
                        ctx.Credentials = new SharePointOnlineCredentials(Email, SourcePassword);
                        ctx.RequestTimeout = Timeout.Infinite;
                        Web web = ctx.Web;
                        ctx.Load(web);
                        SiteBuilderProgressBar.Value = progress;
                        ctx.ExecuteQueryRetry();
                        string name = web.WebTemplate;
                        string Id = name + "#" + web.Configuration.ToString();
                        strSiteTemplateString = Id;
                        if (progress == 50)
                            SiteBuilderProgressBar.Value = progress;
                    }
                    catch (Exception ex)
                    {
                        lblSiteBuilderErrorLog.Text = ex.ToString();
                    }
                }
                try
                {

                    ClientContext ctx = new ClientContext(SiteUrlName);
                    ctx.Credentials = new SharePointOnlineCredentials(Email, SourcePassword);
                    ctx.RequestTimeout = Timeout.Infinite;
                    Web Siteweb = ctx.Web;
                    ctx.Load(Siteweb, w => w.Title);
                    ctx.ExecuteQueryRetry();
                    string strPath = strTemplateName;
                    FileInfo fileInfo = new FileInfo(strPath);
                    XMLTemplateProvider provider =
                        new XMLFileSystemTemplateProvider(fileInfo.DirectoryName, "");

                    var provisioningTemplate = provider.GetTemplate(fileInfo.Name);

                    ProvisioningTemplateApplyingInformation ptai = new ProvisioningTemplateApplyingInformation();
                    ///
                    string StrPath = txtAssetsPath.Text;
                    string[] strAssetsPath = txtAssetsPath.Text.Split('\\');
                    int Count = strAssetsPath.Length - 1;
                    string strFolder = strAssetsPath[Count];
                    string Path = Environment.CurrentDirectory + "\\" + strFolder;
                    DirectoryCopy(StrPath, strFolder, true);
                    ////
                    if (strSiteTemplateString.Equals(provisioningTemplate.BaseSiteTemplate) || NewSite==true)
                    {
                        Siteweb.ApplyProvisioningTemplate(provisioningTemplate, ptai);
                        DeleteDirectory(Path);
                    }
                    else
                    {
                        lblSiteBuilderErrorLog.Text = "Please Select Appropriate template as per site (Ex:Team Site,Communication Site)";
                    }
                    backgroundWorker1.ReportProgress(progress);
                    progress++;
                    if (progress == 100)
                        SiteBuilderProgressBar.Value = progress;
                }
                catch (Exception ex)
                {
                    lblSiteBuilderErrorLog.Text = ex.ToString();
                }

            }
        }
        //This function is use for check the calculated field from list in target site and remove before we apply the template.
        private void RemoveCalculatedColumnBeforeApplyTemplate(ClientContext clientContext)
        {
            try
            {
                Web web = clientContext.Web;
                clientContext.Load(web);
                clientContext.ExecuteQueryRetry();
                ListCollection listColl = web.Lists;
                // Retrieve the list collection properties    
                clientContext.Load(listColl);
                // Execute the query to the server.    
                clientContext.ExecuteQueryRetry();
                foreach (List list in listColl)
                {
                    SP.FieldCollection oFieldCollection = list.Fields;
                    clientContext.Load(oFieldCollection, includes => includes.Include(Field => Field.Title,
                           Field => Field.TypeAsString,
                           Field => Field.FieldTypeKind
                         ));

                    clientContext.ExecuteQuery();

                    foreach (Field oField in oFieldCollection.ToList())
                    {
                        if (oField.TypeAsString == "Calculated")
                        {
                            oField.DeleteObject();
                            clientContext.ExecuteQueryRetry();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblTargetSiteErrorLog.Text = ex.ToString();
            }
          
        }
        //This function is use for migrate list item from one site to other.
        private void MigrateListItems(ClientContext ctxSource, ClientContext ctxDest, List<string> lstSelectedSourceSiteLists)
        {
            try
            {
                ListCollection lstSourceLists = ctxSource.Web.Lists;
                ctxSource.Load(lstSourceLists, x => x.Include(y => y.Title, y => y.BaseTemplate));
                ctxSource.ExecuteQuery();

                ListCollection lstDestLists = ctxDest.Web.Lists;
                ctxDest.Load(lstDestLists, x => x.Include(y => y.Title, y => y.BaseTemplate));
                ctxDest.ExecuteQuery();
                if (lstSourceLists != null && lstDestLists != null && lstSourceLists.Count > 0 && lstDestLists.Count > 0)
                {
                    string[] arrSourceLists = (from p in lstSourceLists
                                               where
                                                   CommonHelper.AllowedBaseTemplates.Contains(p.BaseTemplate)
                                               select p.Title).ToArray();
                    string[] arrDestLists = (from p in lstDestLists where CommonHelper.AllowedBaseTemplates.Contains(p.BaseTemplate) select p.Title).ToArray();

                    string[] arrCommonLists = arrSourceLists.Intersect(arrDestLists).ToArray();

                    arrCommonLists = arrCommonLists.Where(y => lstSelectedSourceSiteLists.Contains(y)).ToArray();

                    if (arrCommonLists.Length > 0)
                    {
                        List<LookupColumnDetails> lstLookupColumnDetails = new List<LookupColumnDetails>();
                        // ShowUpdates("Fetching List Lookup Column Details : " + DateTime.Now);

                        Dictionary<string, List<string>> dicListLookupDetails = GetListLookupDetailsDictionary(ctxSource, arrCommonLists,
                            ref lstLookupColumnDetails);

                        if (dicListLookupDetails != null && dicListLookupDetails.Count > 0)
                        {
                            List<string> lstSelfLookupLists = new List<string>();
                            List<string> lstFinalListOrder = new List<string>();
                            List<string> lstParentLists = new List<string>();

                            //  ShowUpdates("Identifying List Migration Order : " + DateTime.Now);
                            ProcessLists(ctxSource, ref dicListLookupDetails, ref lstFinalListOrder, ref lstSelfLookupLists,
                                ref lstParentLists);

                            //if (dicListLookupDetails.Count == 0)
                            //{
                            //string strListOrder = string.Empty;
                            StringBuilder sb = new StringBuilder();

                            sb.Append(Environment.NewLine);
                            sb.Append("LIST MIGRATION ORDER (based on the lookup Columns)");
                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);

                            // all lists order has been set
                            foreach (var t in lstFinalListOrder)
                            {
                                sb.Append(t);
                                sb.Append(Environment.NewLine);
                            }

                            sb.Append(Environment.NewLine);
                            sb.Append("LIST Deletion Script Lists");
                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);

                            // all lists order has been set
                            foreach (var t in lstFinalListOrder.Reverse<string>())
                            {
                                sb.Append(t);
                                sb.Append(",");
                            }

                            //LogHelper.Log(sb.ToString());


                        }
                        else
                        {
                            // Process further
                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }
        }

        //This function is use for set the sequence of the list based on master and transaction list.
        private void ProcessLists(ClientContext ctxSource, ref Dictionary<string, List<string>> dicListLookupDetails,
            ref List<string> lstFinalListOrder, ref List<string> lstSelfLookupLists, ref List<string> lstParentLists,
            int intPreviousFinalListOrderCount = 0)
        {
            try
            {
                if (dicListLookupDetails.Count > 0)
                {
                    foreach (var dicItem in dicListLookupDetails)
                    {
                        try
                        {
                            string strListTitle = dicItem.Key;
                            List<string> lstLookupColumnParentListTitles = dicItem.Value;

                            bool blIsAllParentListHasData = true;
                            foreach (string strLookupParentListTitle in lstLookupColumnParentListTitles)
                            {
                                if (!lstFinalListOrder.Contains(strLookupParentListTitle))
                                {
                                    if (strListTitle == strLookupParentListTitle)
                                    {
                                        if (!lstSelfLookupLists.Contains(strListTitle))
                                        {
                                            lstSelfLookupLists.Add(strListTitle);
                                        }
                                    }
                                    else
                                    {
                                        blIsAllParentListHasData = false;
                                    }
                                }

                                if (!lstParentLists.Contains(strLookupParentListTitle))
                                {
                                    lstParentLists.Add(strLookupParentListTitle);
                                }
                            }

                            if (blIsAllParentListHasData)
                            {
                                lstFinalListOrder.Add(strListTitle);
                                dicListLookupDetails.Remove(strListTitle);
                                ProcessLists(ctxSource, ref dicListLookupDetails, ref lstFinalListOrder, ref lstSelfLookupLists,
                                    ref lstParentLists, lstFinalListOrder.Count);
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            //ex.HandleException(CLASS_NAME, "ProcessLists");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }
        }

        //This function is use for get the lookup details from particular list.
        private Dictionary<string, List<string>> GetListLookupDetailsDictionary(ClientContext ctxSource, string[] arrCommonLists,
             ref List<LookupColumnDetails> lstLookupColumnDetails, bool blLoadAllLists = false)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            Web sourceWeb = ctxSource.Web;
            string strMainListName = string.Empty;
            string strFieldName = string.Empty;

            try
            {
                if (arrCommonLists == null && blLoadAllLists)
                {
                    ListCollection lstCollection = ctxSource.Web.Lists;
                    ctxSource.Load(lstCollection, x => x.Include(y => y.Title));
                    ctxSource.ExecuteQuery();

                    if (lstCollection != null && lstCollection.Count > 0)
                    {
                        arrCommonLists = lstCollection.Select(x => x.Title).ToArray();
                    }
                }

                foreach (string strListTitle in arrCommonLists)
                {
                    strMainListName = strListTitle;
                    try
                    {
                        List lstCurrentList = sourceWeb.Lists.GetByTitle(strListTitle);
                        ctxSource.Load(lstCurrentList);
                        ctxSource.ExecuteQuery();

                        if (lstCurrentList != null)
                        {
                            SP.FieldCollection fieldCollection = lstCurrentList.Fields;
                            ctxSource.Load(fieldCollection);
                            ctxSource.ExecuteQuery();

                            if (fieldCollection != null && fieldCollection.Count > 0)
                            {
                                List<string> lstLookupColumnParentListTitles = new List<string>();

                                foreach (Field field in fieldCollection)
                                {
                                    try
                                    {
                                        strFieldName = field.Title;
                                    }
                                    catch (Exception ex)
                                    {
                                        lblSourceSiteErrorLog.Text = ex.ToString();
                                    }

                                    try
                                    {
                                        if ((field.TypeAsString == "Lookup" || field.TypeAsString == "LookupMulti"))
                                        {
                                            ctxSource.Load(field);
                                            ctxSource.ExecuteQuery();

                                            Guid guid; string strFieldXML = field.SchemaXml;
                                            var lookupListGUId = XElement.Parse(strFieldXML).Attributes().First(s => s.Name == "List").Value;

                                            if (!string.IsNullOrEmpty(lookupListGUId) && Guid.TryParse(lookupListGUId, out guid))
                                            {
                                                List parentList = null;

                                                parentList = sourceWeb.Lists.GetById(new Guid(lookupListGUId));
                                                ctxSource.Load(parentList, x => x.Title);
                                                ctxSource.ExecuteQuery();

                                                if (parentList != null)
                                                {

                                                    string strParentListTitle = parentList.Title;

                                                    // IMPORTANT : Uncomment below IF condition if any issue occurs

                                                    //if (arrCommonLists.Contains(strParentListTitle))
                                                    //{
                                                    lstLookupColumnParentListTitles.Add(strParentListTitle);

                                                    var ShowField = XElement.Parse(strFieldXML).Attributes().First(s => s.Name == "ShowField").Value;

                                                    if (ShowField.Contains(','))
                                                    {
                                                        ShowField = ShowField.Split(',')[0];
                                                    }

                                                    LookupColumnDetails objLookupColumnDetails = new LookupColumnDetails();
                                                    objLookupColumnDetails.CurrentListTitle = strListTitle;
                                                    objLookupColumnDetails.CurrentColumnColumnTitle = field.Title;
                                                    objLookupColumnDetails.CurrentColumnColumnInternalName = field.InternalName;
                                                    objLookupColumnDetails.LookupParentListTitle = strParentListTitle;
                                                    objLookupColumnDetails.LookupColumnShowField = ShowField;
                                                    objLookupColumnDetails.LookupColumnShowFieldType = GetFieldType(ctxSource,
                                                                                                    ShowField, strParentListTitle);

                                                    lstLookupColumnDetails.Add(objLookupColumnDetails);
                                                    //}
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        lblSourceSiteErrorLog.Text = ex.ToString() + "GetListLookupDetailsDictionar  " + "List Title : " + strMainListName + "; Field Title : " + strFieldName;
                                    }
                                    strFieldName = "";
                                }

                                result.Add(strListTitle, lstLookupColumnParentListTitles.Distinct().ToList<string>());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblSourceSiteErrorLog.Text = ex.ToString() + "GetListLookupDetailsDictionar  " + "List Title : " + strMainListName + "; Field Title : " + strFieldName;

                    }
                    strMainListName = "";
                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text=ex.ToString()+ "GetListLookupDetailsDictionary" + "List Title : " + strMainListName + "; Field Title : " + strFieldName;
            }
            return result;
        }
        //This function is use for get the type of the field.
        private string GetFieldType(ClientContext clientContext, string strFieldTitle, string strListTitle)
        {
            string strFieldType = "";
            try
            {
                List lst = clientContext.Web.Lists.GetByTitle(strListTitle);
                clientContext.Load(lst);
                clientContext.ExecuteQuery();

                if (lst != null)
                {
                    SP.FieldCollection fieldCollection = lst.Fields;
                    clientContext.Load(fieldCollection, y => y.Include(x => x.Title, x => x.TypeAsString));
                    clientContext.ExecuteQuery();

                    Microsoft.SharePoint.Client.Field field = fieldCollection.Where(x => x.Title == strFieldTitle).FirstOrDefault();

                    if (field != null)
                    {
                        strFieldType = field.TypeAsString;
                    }

                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();

            }
            return strFieldType;
        }
        #endregion //Methods

        #region Implementation
        //This is the SiteMigrate button click event in which call the Applytemplate fucntion.
        private void btnSiteMigrate_Click(object sender, EventArgs e)
        {
            try
            {
                string StrTargetWebUrl = txtDestinationSiteName.Text;
                string StrUserName = txtDesUsername.Text;
                string StrPassword = txtDesPassword.Text;
                string StrFileName = txtDesXMLFileName.Text;
                var pathArray = StrFileName.Split('\\');
                var newPathname = "";
                for (int j = 0; j < pathArray.Length - 1; j++)
                {
                    newPathname += pathArray[j];
                    newPathname += "\\";
                }
                string StrFileLocation = newPathname;
                SecureString SecurePassword = new SecureString();
                foreach (char c in StrPassword.ToCharArray()) SecurePassword.AppendChar(c);
                TargetSiteProgressBar.Visible = true;
                backgroundWorker1.WorkerReportsProgress = true;
                int i = 10;
                Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i);
                i++;


                // APPLY the template to new site from
                ApplyProvisioningTemplate(StrTargetWebUrl, StrUserName, SecurePassword, StrFileLocation, StrFileName);

                i = 0;
                TargetSiteProgressBar.Value = 100;
            }
            catch (Exception ex)
            {
                lblTargetSiteErrorLog.Text = ex.ToString();
            }


        }
        //This is the GenerateXmlfile button clcik event in which call the Apply template function
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
                int i = 10;
                Thread.Sleep(100);
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.ReportProgress(i);
                i++;
                // GET the template from existing site and serialize
                // Serializing the template for later reuse is optional
                // GetTenantNameFromUrl(StrSourceWebUrl);
                //  GetTenantProvisioningTemplate(StrSourceWebUrl, StrUserName, SecurePassword, StrLocation);
                GetProvisioningTemplate(StrSourceWebUrl, StrUserName, SecurePassword, StrLocation);
                i = 0;
                SourceSiteProgressBar.Value = 100;
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }



        }

        public static string GetLookupFieldOptions(ClientContext clientContext, string ListID, string ColumnName, int LookupId)
        {
            string LookupValue = "";
            try
            {
                //List<LookupValues> ListLookupValues = new List<LookupValues> { };
                Guid ListGUID = new Guid(ListID);
                SP.List oList = clientContext.Web.Lists.GetById(ListGUID);

                CamlQuery camlQuery = new CamlQuery();
                // camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name ='Country'/><Value Type ='Lookup'>5</Value></Eq></Where></Query></View>";

                ListItemCollection collListItem = oList.GetItems(camlQuery);

                clientContext.Load(collListItem);

                clientContext.ExecuteQuery();

                foreach (ListItem oListItem in collListItem)
                {
                    LookupValue = Convert.ToString(oListItem["Title"]);
                }
                return LookupValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //This is the browse button click event in which set the xml file location in textbox.
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
                lblTargetSiteErrorLog.Text = ex.ToString();
            }

        }
        //This is the browse button click event in which set the xml file location in textbox.
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
                lblSourceSiteErrorLog.Text = ex.ToString();
                return;
            }

        }
        //This is the selected index change event of checkbox list item in which set the code to checked All options when select "All" from the option.
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
                    if (chkTemplateOptions.Items[i].ToString() == "Tenant" || chkTemplateOptions.Items[i].ToString() == "TermGroups" || chkTemplateOptions.Items[i].ToString() == "Publishing" || chkTemplateOptions.Items[i].ToString() == "ImageRenditions" || chkTemplateOptions.Items[i].ToString() == "WebApiPermissions")
                    {
                        chkTemplateOptions.SetItemCheckState(i, CheckState.Indeterminate);
                    }

                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }

        }

        //In form load function add the code for show and hide some control.
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < chkTemplateOptions.Items.Count; i++)
            {
                if (chkTemplateOptions.Items[i].ToString() == "TermGroups" || chkTemplateOptions.Items[i].ToString() == "Publishing" || chkTemplateOptions.Items[i].ToString() == "ImageRenditions" || chkTemplateOptions.Items[i].ToString() == "WebApiPermissions")
                {
                    chkTemplateOptions.SetItemCheckState(i, CheckState.Indeterminate);
                }

            }
            // pnlProvision.Hide();
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSiteBuilder);
            backgroundWorker1.WorkerReportsProgress = false;
            SourceSiteProgressBar.Visible = false;
            TargetSiteProgressBar.Visible = false;
            SiteBuilderProgressBar.Visible = false;
        }

        //This is new site radiobutton checked event in which set hide/show of some control.
        private void rbNewSite_CheckedChanged(object sender, EventArgs e)
        {
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSiteBuilder);
            lblSiteTitle.Show();
            txtSiteURLTitle.Show();
            pnlProvision.Show();
        }
        //This is StieReplica radiobutton checked event in which set hide/show of some control.
        private void rbSiteReplica_CheckedChanged(object sender, EventArgs e)
        {
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSourceSite);
            tabSiteMigration.Controls.Add(tabTargetSite);
        }
        //This is provision button click event in which call the create site function.
        private void btnProvision_Click(object sender, EventArgs e)
        {
            try
            {
                SiteBuilderProgressBar.Visible = true;
                SecureString passWord = new SecureString();
                backgroundWorker1.WorkerReportsProgress = true;
                foreach (char c in txtPwd.Text.ToCharArray()) passWord.AppendChar(c);
                int i = 10;
                Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i);
                i++;
                CreateSite(txtSiteUrlName.Text, passWord, txtEmail.Text, txtSiteURLTitle.Text, i);
                i = 0;
                SiteBuilderProgressBar.Value = 100;
            }
            catch (Exception ex)
            {
                lblSiteBuilderErrorLog.Text = ex.ToString();
            }

        }

        //This is the back button click event in which set the code for back on the main page.
        private void btnBack_Click(object sender, EventArgs e)
        {
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSiteBuilder);
        }

        //This is the browse button click event in which set the xml file location in textbox.
        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog2.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    txtTemplateName.Text = openFileDialog2.FileName;
                }
            }
            catch (Exception ex)
            {
                lblSiteBuilderErrorLog.Text = ex.ToString();
            }
        }
        //This is Asset path button click event in which set the Asset path location in textbox.
        private void btnAssetPath_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    txtAssetsPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                lblSiteBuilderErrorLog.Text = ex.ToString();
            }
        }

        //This is Existing site radiobutton checked event in which set hide/show of some control.
        private void rbExistingSite_CheckedChanged(object sender, EventArgs e)
        {
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSiteBuilder);
            lblSiteTitle.Hide();
            txtSiteURLTitle.Hide();
            pnlProvision.Show();
        }

        //This is loadlist button click event in which call the bindlistname function.
        private void btnLoadList_Click(object sender, EventArgs e)
        {
            try
            {
                SourceSiteProgressBar.Visible = true;
                backgroundWorker1.WorkerReportsProgress = true;
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
                lblSourceSiteErrorLog.Text = ex.ToString();
            }
        }

        //This is progress bar change event.
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar  
            SourceSiteProgressBar.Value = e.ProgressPercentage;
            // Set the text.  
            this.Text = "Progress: " + e.ProgressPercentage.ToString() + "%";
        }

        //This the listname checkbox listitem selected index change event. in which set the code for selection of options.
        private void ChkListName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkListName.SelectedItem == ChkListName.Items[0] && ChkListName.GetItemChecked(0) == true)
                {
                    for (int i = 0; i < ChkListName.Items.Count; i++)
                    {
                        ChkListName.SetItemChecked(i, true);
                    }
                }
                if (ChkListName.SelectedItem == ChkListName.Items[0] && ChkListName.GetItemChecked(0) == false)
                {
                    for (int i = 0; i < ChkListName.Items.Count; i++)
                    {
                        ChkListName.SetItemChecked(i, false);
                    }
                }
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }
        }

        //This is the Add group button click event in which set the code for add site group with permission from source to target site.
        private void btnAddGroups_Click(object sender, EventArgs e)
        {
            string StrSourceWebUrl = txtSourceSiteName.Text;
            string StrUserName = txtUsername.Text;
            string StrPassword = txtPassword.Text;
            SecureString SecurePassword = new SecureString();
            foreach (char c in StrPassword.ToCharArray()) SecurePassword.AppendChar(c);
            string StrTargetWebUrl = txtDestinationSiteName.Text;
            string StrTargetUserName = txtDesUsername.Text;
            string StrTargetPassword = txtDesPassword.Text;
            SecureString SecureTargetSitePassword = new SecureString();
            foreach (char c in StrTargetPassword.ToCharArray()) SecureTargetSitePassword.AppendChar(c);
            using (var clientContext = new ClientContext(StrSourceWebUrl))
            {
                using (var TargetClientContext = new ClientContext(StrTargetWebUrl))
                {
                    clientContext.Credentials = new SharePointOnlineCredentials(StrUserName, SecurePassword);
                    clientContext.RequestTimeout = Timeout.Infinite;
                    // Just to output the site details
                    Web web = clientContext.Web;
                    clientContext.Load(web);
                    clientContext.Load(web.SiteGroups);
                    clientContext.Load(web.RoleDefinitions);
                    // clientContext.ExecuteQueryRetry();
                    TargetClientContext.Credentials = new SharePointOnlineCredentials(StrTargetUserName, SecureTargetSitePassword);
                    TargetClientContext.RequestTimeout = Timeout.Infinite;
                    Web Targetweb = TargetClientContext.Web;
                    TargetClientContext.Load(Targetweb);
                    TargetClientContext.Load(Targetweb.SiteGroups);
                    TargetClientContext.Load(Targetweb.RoleDefinitions);

                    Microsoft.SharePoint.Client.RoleAssignmentCollection roleAssignments = web.RoleAssignments;
                    clientContext.Load(roleAssignments, roleAssignement => roleAssignement.Include(r => r.Member, r => r.RoleDefinitionBindings));
                    //  clientContext.Load(roleAssignments);
                    clientContext.ExecuteQuery();
                    TargetClientContext.ExecuteQuery();
                    bool IsPermissionAlreadyExist = false;
                    bool IsGroupAvailable = false;
                    try
                    {
                        foreach (var RoleDefinition in clientContext.Web.RoleDefinitions)
                        {

                            var roleDefinitions = Targetweb.RoleDefinitions;
                            foreach (var TargetRoleDefinition in roleDefinitions)
                            {
                                if (RoleDefinition.Name == TargetRoleDefinition.Name)
                                {
                                    IsPermissionAlreadyExist = true;
                                    break;
                                }
                                else
                                {
                                    IsPermissionAlreadyExist = false;
                                }
                            }
                            if (IsPermissionAlreadyExist == false)
                            {
                                RoleDefinitionCreationInformation roleDefinitionCreationInformation = new RoleDefinitionCreationInformation();
                                roleDefinitionCreationInformation.Name = RoleDefinition.Name.ToString();
                                if (RoleDefinition.Description != null)
                                    roleDefinitionCreationInformation.Description = RoleDefinition.Description.ToString();
                                roleDefinitionCreationInformation.BasePermissions = RoleDefinition.BasePermissions;
                                roleDefinitions.Add(roleDefinitionCreationInformation);
                                TargetClientContext.Load(roleDefinitions);
                                TargetClientContext.ExecuteQuery();
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        lblTargetSiteErrorLog.Text = ex.ToString();
                    }
                    foreach (SP.RoleAssignment grp in roleAssignments)
                    {
                        GroupCollection TargetSiteGroups = Targetweb.SiteGroups;
                        foreach (Group targetGroup in TargetSiteGroups)
                        {
                            if (grp.Member.Title == targetGroup.Title)
                            {
                                IsGroupAvailable = true;
                                break;
                            }
                            else
                            {
                                IsGroupAvailable = false;
                            }

                        }
                        if (IsGroupAvailable == false)
                        {
                            GroupCreationInformation objCreateGroup = new GroupCreationInformation();
                            if (!grp.Member.Title.Contains("Members") && !grp.Member.Title.Contains("Owners") && !grp.Member.Title.Contains("Visitors"))
                            {
                                objCreateGroup.Title = grp.Member.Title;
                                objCreateGroup.Description = grp.Member.Title;
                                Group grpAdd = TargetClientContext.Site.RootWeb.SiteGroups.Add(objCreateGroup);
                                SP.RoleDefinition rd = null;
                                RoleDefinitionBindingCollection rdb = new RoleDefinitionBindingCollection(TargetClientContext);

                                foreach (SP.RoleDefinition oRoleDefinition in grp.RoleDefinitionBindings)
                                {
                                    if (!oRoleDefinition.Name.Equals(string.Empty))
                                    {
                                        bool permissionMatch = false;
                                        if (oRoleDefinition.Name.Equals("Read", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            rd = TargetClientContext.Web.RoleDefinitions.GetByType(RoleType.Reader);
                                            permissionMatch = true;
                                        }
                                        if (oRoleDefinition.Name.Equals("Full Control", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            rd = TargetClientContext.Web.RoleDefinitions.GetByType(RoleType.Administrator);
                                            permissionMatch = true;
                                        }
                                        if (oRoleDefinition.Name.Equals("Design", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            rd = TargetClientContext.Web.RoleDefinitions.GetByType(RoleType.WebDesigner);
                                            permissionMatch = true;
                                        }
                                        if (oRoleDefinition.Name.Equals("Edit", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            rd = TargetClientContext.Web.RoleDefinitions.GetByType(RoleType.Editor);
                                            permissionMatch = true;
                                        }
                                        if (oRoleDefinition.Name.Equals("Contribute", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            rd = TargetClientContext.Web.RoleDefinitions.GetByType(RoleType.Contributor);
                                            permissionMatch = true;
                                        }
                                        if (!permissionMatch)
                                            rd = TargetClientContext.Web.RoleDefinitions.GetByName(oRoleDefinition.Name);
                                        if (!oRoleDefinition.Name.Contains("Limited Access"))
                                        {
                                            rdb.Add(rd);
                                        }
                                        // else
                                        //{
                                        //    rd = TargetClientContext.Web.RoleDefinitions.GetByType(RoleType.Reader);
                                        //    rdb.Add(rd);
                                        //}

                                    }

                                }
                                TargetClientContext.Web.RoleAssignments.Add(grpAdd, rdb);
                                TargetClientContext.ExecuteQuery();
                            }
                        }
                    }

                    //try
                    //{
                    //    GroupCollection groups = web.SiteGroups;
                    //    foreach (Group grp in groups)
                    //    {
                    //        GroupCollection TargetSiteGroups = Targetweb.SiteGroups;
                    //        foreach (Group targetGroup in TargetSiteGroups)
                    //        {
                    //            if (grp.Title == targetGroup.Title)
                    //            {
                    //                IsGroupAvailable = true;
                    //                break;
                    //            }
                    //            else
                    //            {
                    //                IsGroupAvailable = false;
                    //            }

                    //        }
                    //        if (IsGroupAvailable == false)
                    //        {
                    //            GroupCreationInformation groupCreationInfo = new GroupCreationInformation();
                    //            groupCreationInfo.Title = grp.Title;
                    //            groupCreationInfo.Description = grp.Description;
                    //            Group oGroup = Targetweb.SiteGroups.Add(groupCreationInfo);
                    //            RoleDefinitionBindingCollection collRoleDefinitionBinding = new RoleDefinitionBindingCollection(TargetClientContext);

                    //            SP.RoleDefinition oRoleDefinition = Targetweb.RoleDefinitions.GetByType(RoleType.Contributor);
                    //            collRoleDefinitionBinding.Add(oRoleDefinition);
                    //            Targetweb.RoleAssignments.Add(oGroup, collRoleDefinitionBinding);
                    //            TargetClientContext.Load(oGroup);

                    //            TargetClientContext.Load(oRoleDefinition);

                    //            TargetClientContext.ExecuteQuery();
                    //        }


                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    lblTargetSiteErrorLog.Text = ex.ToString();
                    //}

                }
            }
        }
        //This is the XMlFileWithListData button click event in which call the getprovisioning template function.
        private void btnXMlFileWithListData_Click(object sender, EventArgs e)
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
                int i = 10;
                Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i);
                i++;
                XMLWithData = true;
                GetProvisioningTemplate(StrSourceWebUrl, StrUserName, SecurePassword, StrLocation);
                i = 0;
                SourceSiteProgressBar.Value = 100;
            }
            catch (Exception ex)
            {
                lblSourceSiteErrorLog.Text = ex.ToString();
            }
        }

        //This is the Data migration button click event in which call the DataMigrate function.
        private void btnDataMigration_Click(object sender, EventArgs e)
        {
            try
            {
                TargetSiteProgressBar.Visible = true;
                backgroundWorker1.WorkerReportsProgress = true;
                int i = 10;
                Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i);
                i++;
                DataMigrate();
                TargetSiteProgressBar.Value = 100;
            }
            catch (Exception ex)
            {
                lblTargetSiteErrorLog.Text = ex.ToString();
            }
        }
        #endregion //Implementation   

    }
}
