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
using Microsoft.SharePoint.Client.WorkflowServices;
using OfficeDevPnP.Core.Utilities;
using Newtonsoft.Json.Linq;
using OfficeDevPnP.Core.Framework.Provisioning.Model.Configuration;
using AppCatalog = Microsoft.SharePoint.Client.AppCatalog;
using OfficeDevPnP.Core.UPAWebService;


namespace SiteMigrationProject
{

    public partial class Form1 : System.Windows.Forms.Form
    {
        //Initialize Variable
        //string path = "D:/Antima/SiteMigrationProject/SiteMigrationProject/Templates/";
        private Dictionary<string, FieldValueProvider> m_dictFieldValueProviders = null;
        private Dictionary<string, LookupDataRef> m_lookups = null;
        private Dictionary<int, int> m_mappingIDs = null;
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(List list, Web web, ProvisioningTemplate template)
        {
            this.List = list;
            this.Web = web;
        }
        public List List
        {
            get; private set;
        }
        public Web Web
        {
            get; private set;
        }
        public ClientRuntimeContext Context
        {
            get
            {
                return this.Web.Context;
            }
        }

        public class ListItems
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
        public int CompatibilityLevel { get; }
        public uint Lcid { get; }
        // public string Name { get; set; }
        public Guid ProductId { get; set; }
        public AppSource Source { get; set; }
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
                        if (list.BaseType == BaseType.GenericList)
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
        //public static ProvisioningHierarchy GetTenantTemplate(this Tenant tenant, ExtractConfiguration configuration)
        //{
        //    return new SiteToTemplateConversion().GetTenantTemplate(tenant, configuration);
        //}

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
                    //ctx.Load(this.List, l => l.RootFolder.ServerRelativeUrl);
                    //ctx.ExecuteQueryRetry();
                    // Microsoft.SharePoint.Client.FieldCollection fields = this.List.Fields;
                    /// ctx.Load(fields);

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
                    // template.Navigation.GlobalNavigation.StructuralNavigation.RemoveExistingNodes = true;
                    //template.Navigation.CurrentNavigation.StructuralNavigation.RemoveExistingNodes = true;
                    foreach (object itemChecked in chkTemplateOptions.CheckedItems)
                    {
                        if (itemChecked.ToString() == "All")
                        {
                            try
                            {
                                IsAllSelected = true;
                                AddListItemWithData(ctx, template, web);
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
                                AddListItemWithData(ctx, template, web);
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

                    // We can serialize this template to save and reuse it
                    // Optional step
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
        private void AddListItemWithData(ClientContext ctx, OfficeDevPnP.Core.Framework.Provisioning.Model.ProvisioningTemplate template, Web web)
        {
            ListInstanceCollection LstInstanceColl = new ListInstanceCollection(template);
            if (ChkListName.CheckedItems.Count > 0)
            {
                try
                {
                    foreach (var chkitem in ChkListName.CheckedItems)
                    {
                        if (!chkitem.Equals("All"))
                        {
                            var items = new List<ListItemInfo>();
                            OfficeDevPnP.Core.Framework.Provisioning.Model.ListInstance ListInstance = template.Lists.Find(listTemp => listTemp.Title == ((System.Web.UI.WebControls.ListItem)chkitem).Text);
                            ///
                            Dictionary<string, string> dataValues = new Dictionary<string, string>();
                            List ListObject = ctx.Web.Lists.GetByTitle(((System.Web.UI.WebControls.ListItem)chkitem).Text);
                            ctx.Load(ListObject);
                            ctx.ExecuteQuery();
                            // This creates a CamlQuery" 
                            // so that it grabs all list items, regardless of the folder they are in. 
                            CamlQuery query = CamlQuery.CreateAllItemsQuery();
                            // CamlQuery camlQuery = new CamlQuery();
                            ListItemCollection itemCollection = ListObject.GetItems(query);
                            // camlQuery.ViewXml = "<View><RowLimit>100</RowLimit></View>";
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
                                        if (field.TypeAsString != "Computed" && field.TypeAsString != "Calculated" && field.InternalName != "ContentType" && field.Hidden == false && field.ReadOnlyField == false && field.Sealed == false && field.InternalName != "ImageSize" && field.InternalName != "FileType")
                                        {
                                            //          Field field = fields.FirstOrDefault(
                                            //f => f.InternalName ==Convert.ToString(item.Client_Title));
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
                                                        ////
                                                        if (null != Fieldprovider)
                                                        {
                                                            object itemValue = Fieldprovider.GetFieldValueTyped(str);
                                                            if (null != itemValue)
                                                            {
                                                                if (field.FieldTypeKind == FieldType.Lookup)
                                                                {
                                                                    //XmlDocument xmlDocSchemaXml = new XmlDocument();
                                                                    //xmlDocSchemaXml.LoadXml(field.SchemaXml);
                                                                    //if (xmlDocSchemaXml.DocumentElement.Attributes["List"] != null)
                                                                    //{
                                                                    //    string ListId = xmlDocSchemaXml.DocumentElement.Attributes["List"].Value;
                                                                    //    string ColumnName = xmlDocSchemaXml.DocumentElement.Attributes["ShowField"].Value;
                                                                    //    int ItemId = Convert.ToInt32(((Microsoft.SharePoint.Client.FieldLookupValue)itemValue).LookupId);
                                                                    //    str = GetLookupFieldOptions(ctx, ListId, ColumnName, ItemId);
                                                                    //}
                                                                    FieldLookup lookupField = (FieldLookup)field;
                                                                    RegisterLookupReference(lookupField, item, itemValue);
                                                                }
                                                                else
                                                                {
                                                                    item[field.InternalName] = itemValue;
                                                                }
                                                            }
                                                        }
                                                        ///
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
                                    }

                                    DataRow dataRow = new DataRow(values);
                                    ListInstance.DataRows.Add(dataRow);
                                    items.Add(new ListItemInfo()
                                    {
                                        Item = item,
                                        Row = dataRow
                                    });
                                }
                                catch (Exception ex)
                                {
                                    lblSourceSiteErrorLog.Text = ex.ToString();
                                }

                                foreach (ListItemInfo itemInfo in items)
                                {
                                    try
                                    {
                                        var listitem = itemInfo.Item;
                                        var dataRow = itemInfo.Row;
                                        if (0 < listitem.Id)
                                        {
                                            AddIDMappingEntry(listitem, dataRow);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }

                            LstInstanceColl.Add(ListInstance);
                        }

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
        private void RegisterLookupReference(FieldLookup lookupField, ListItem listitem, object itemValue)
        {
            if (null != itemValue)
            {
                if (null == m_lookups)
                {
                    m_lookups = new Dictionary<string, LookupDataRef>();
                }

                FieldLookupValue[] value = itemValue as FieldLookupValue[];

                LookupDataRef lookupRef = null;
                if (!m_lookups.TryGetValue(lookupField.InternalName, out lookupRef))
                {
                    lookupRef = new LookupDataRef(lookupField);
                    m_lookups.Add(lookupField.InternalName, lookupRef);
                }
                lookupRef.ItemLookupValues.Add(listitem, itemValue);
            }
        }
        public void UpdateLookups(Func<Guid, Form1> fnGetLookupDependentProvider)
        {
            if (null != m_lookups)
            {
                bool valueUpdated = false;
                foreach (KeyValuePair<string, LookupDataRef> pair in m_lookups)
                {
                    LookupDataRef lookupData = pair.Value;
                    if (0 < lookupData.ItemLookupValues.Count)
                    {
                        Guid sourceListId = Guid.Empty;
                        try
                        {
                            sourceListId = new Guid(lookupData.Field.LookupList);
                        }
                        catch (Exception ex)
                        {
                            // scope.LogError(ex, "Failed to get source list for lookup field. Field Name: {0}, Source List: {1}.",
                            //  lookupData.Field.InternalName, lookupData.Field.LookupList);
                        }
                        if (!Guid.Empty.Equals(sourceListId))
                        {
                            Form1 sourceProvider = fnGetLookupDependentProvider(sourceListId);
                            if ((null != sourceProvider) && (null != sourceProvider.m_mappingIDs))
                            {
                                foreach (KeyValuePair<ListItem, object> lookupPair in lookupData.ItemLookupValues)
                                {
                                    ListItem item = lookupPair.Key;
                                    object newItemValue = null;
                                    object oldItemValue = lookupPair.Value;
                                    FieldLookupValue oldLookupValue = oldItemValue as FieldLookupValue;
                                    if (null != oldLookupValue)
                                    {
                                        int lookupId = oldLookupValue.LookupId;
                                        int newId;
                                        if (sourceProvider.m_mappingIDs.TryGetValue(lookupId, out newId) && (0 < newId))
                                        {
                                            newItemValue = new FieldLookupValue()
                                            {
                                                LookupId = newId
                                            };
                                        }
                                    }
                                    else
                                    {
                                        List<FieldLookupValue> newLookupValues = new List<FieldLookupValue>();
                                        FieldLookupValue[] oldLookupValues = oldItemValue as FieldLookupValue[];
                                        if ((null != oldLookupValues) && (0 < oldLookupValues.Length))
                                        {
                                            foreach (FieldLookupValue val in oldLookupValues)
                                            {
                                                int newId;
                                                if (sourceProvider.m_mappingIDs.TryGetValue(val.LookupId, out newId) && (0 < newId))
                                                {
                                                    newLookupValues.Add(new FieldLookupValue()
                                                    {
                                                        LookupId = newId
                                                    });
                                                }
                                            }
                                        }
                                        if (0 < newLookupValues.Count)
                                        {
                                            newItemValue = newLookupValues.ToArray();
                                        }
                                    }
                                    if (null != newItemValue)
                                    {
                                        item[lookupData.Field.InternalName] = newItemValue;
                                        item.Update();

                                        valueUpdated = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (valueUpdated)
                {
                    try
                    {
                        this.Context.ExecuteQueryRetry();
                    }
                    catch (Exception ex)
                    {
                        string lookupFieldNames = string.Join(", ", m_lookups.Select(pair => pair.Value.Field.InternalName).ToArray());
                        // scope.LogError(ex, "Failed to set lookup values. List: '{0}', Lookup Fields: {1}.", this.List.Title, lookupFieldNames);
                    }
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
                lblTargetSiteErrorLog.Text = ex.ToString();
            }

        }
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

                    //   RemoveCalculatedColumnBeforeApplyTemplate(ClientContext);
                    ProvisioningTemplateApplyingInformation ptai = new ProvisioningTemplateApplyingInformation();
                    XMLFileSystemTemplateProvider provider = new XMLFileSystemTemplateProvider(TargetLocation, TargetFile);
                    OfficeDevPnP.Core.Framework.Provisioning.Model.ProvisioningTemplate ApplyTemplateFile = provider.GetTemplate(TargetFile);

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(ApplyTemplateFile.ToXML());
                    XDocument doc = XDocument.Load(TargetFile);
                    // XElement element = doc.Root;
                    //string element = xmlDoc.InnerText;
                    xmlDoc.InnerXml = xmlDoc.InnerXml.Replace(";#", ",");
                    xmlDoc.Save(TargetFile);
                    // doc.Save(TargetFile);
                    OfficeDevPnP.Core.Framework.Provisioning.Model.ProvisioningTemplate AfterReplaceApplyTemplateFile = provider.GetTemplate(TargetFile);
                    //ReplaceAll(element, doc, ApplyTemplateFile);
                    //string StrPath = txtAssetsPath.Text;
                    //string[] strAssetsPath = txtAssetsPath.Text.Split('\\');
                    //int Count = strAssetsPath.Length - 1;
                    //string strFolder = strAssetsPath[Count];
                    //string Path = Environment.CurrentDirectory + "\\" + strFolder;
                    //DirectoryCopy(StrPath, strFolder, true);
                    web.ApplyProvisioningTemplate(AfterReplaceApplyTemplateFile, ptai);
                    ListCollection ListColl = web.Lists;
                    ClientContext.Load(ListColl);
                    foreach (List LstGuid in ListColl)
                    {
                        ListItemsProvider itemProvider = new ListItemsProvider(List, web, AfterReplaceApplyTemplateFile);
                        Func<Guid, ListItemsProvider> LookupSourceProvider = null;
                        itemProvider.UpdateLookups(LookupSourceProvider);
                    }
                    //DeleteDirectory(Path);
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
        static void ReplaceAll(XElement element, XDocument document, OfficeDevPnP.Core.Framework.Provisioning.Model.ProvisioningTemplate ApplyTemplateFile)
        {
            foreach (var item in element.Elements())
            {
                if (item.HasElements)
                {
                    ReplaceAll(element, document, ApplyTemplateFile);
                }
            }
            element.Value.Replace(";#", ",");
            document.Save(ApplyTemplateFile.ToXML());

        }
        //private List<Field> GetListContentSerializableFields(List Listname,bool serialize)
        //{
        //    Microsoft.SharePoint.Client.FieldCollection spfields = Listname.Fields;
        //    List<Field> fields = new List<Field>();
        //    foreach (Field field in spfields)
        //    {
        //        if (CanFieldContentBeIncluded(field, serialize))
        //        {
        //            fields.Add(field);
        //        }
        //    }
        //    return fields;
        //}
        //public List<DataRow> ExtractItems(ProvisioningTemplateCreationInformation creationInfo, TokenParser parser, PnPMonitoredScope scope)
        //{
        //    List<DataRow> dataRows = new List<DataRow>();

        //    bool isPageLib = (List.BaseTemplate == (int)ListTemplateType.WebPageLibrary) ||
        //        (List.BaseTemplate == (int)ListTemplateType.HomePageLibrary);
        //    //||
        //        //(List.BaseTemplate == (int)ListTemplateType.PublishingPages);

        //    CamlQuery query = isPageLib ? CamlQuery.CreateAllFoldersQuery() : CamlQuery.CreateAllItemsQuery();
        //    query.DatesInUtc = true;
        //    ListItemCollection items = this.List.GetItems(query);
        //    this.Context.Load(items, col => col.IncludeWithDefaultProperties(i => i.HasUniqueRoleAssignments));
        //    this.Context.Load(this.List.Fields);
        //    this.Context.Load(this.List, l => l.RootFolder.ServerRelativeUrl, l => l.BaseType);
        //    this.Context.ExecuteQueryRetry();

        //    ItemPathProvider itemPathProvider = new ItemPathProvider(this.List, this.Web);

        //    List<Field> fields = GetListContentSerializableFields(true);
        //    foreach (ListItem item in items)
        //    {
        //        try
        //        {
        //            Dictionary<string, string> values = new Dictionary<string, string>();
        //            foreach (Field field in fields)
        //            {
        //                if (CanFieldContentBeIncluded(field, true))
        //                {
        //                    string str = "";
        //                    object value = null; ;
        //                    try
        //                    {
        //                        value = item[field.InternalName];
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        scope.LogWarning(ex,
        //                            "Failed to read item field value. List:{0}, Item ID:{1}, Field: {2}", this.List.Title, item.Id, field.InternalName);
        //                    }
        //                    if (null != value)
        //                    {
        //                        try
        //                        {
        //                            FieldValueProvider provider = GetFieldValueProvider(field, this.Web);
        //                            str = provider.GetValidatedValue(value);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            scope.LogWarning(ex,
        //                                "Failed to serialize item field value. List:{0}, Item ID:{1}, Field: {2}", this.List.Title, item.Id, field.InternalName);
        //                        }
        //                        if (!string.IsNullOrEmpty(str))
        //                        {
        //                            values.Add(field.InternalName, str);
        //                        }
        //                    }
        //                }
        //            }

        //            string fileSrc;
        //            itemPathProvider.ExtractItemPathValues(item, values, creationInfo, out fileSrc);

        //            if (values.Any())
        //            {
        //                ObjectSecurity security = null;
        //                if (item.HasUniqueRoleAssignments)
        //                {
        //                    try
        //                    {
        //                      //  security = item.GetSecurity(parser);
        //                       // security.ClearSubscopes = true;
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        scope.LogWarning(ex, "Failed to get item security. Item ID: {0}, List: '{1}'.", item.Id, this.List.Title);
        //                    }
        //                }

        //                DataRow row = new DataRow(values, security, fileSrc);
        //                dataRows.Add(row);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            scope.LogError(ex, "Failed to save item in template. Item ID: {0}, List: '{1}'.", item.Id, this.List.Title);
        //        }
        //    }

        //    return dataRows;
        //}
        private void DirectoryCopy(
            string sourceDirName, string destDirName, bool copySubDirs)
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
            //List<String> AllFiles = System.IO.Directory
            //                   .GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories).ToList();

            //foreach (string file in AllFiles)
            //{
            //    FileInfo File = new FileInfo(file);
            //    // to remove name collisions
            //    if (new FileInfo(dir + "\\" + File.Name).Exists == false)
            //    {
            //        File.MoveTo(dir + "\\" + File.Name);
            //    }
            //  }
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
        private void DeleteDirectory(string path)
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
        private List<Field> GetListContentSerializableFields(bool v)
        {
            throw new NotImplementedException();
        }

        private bool CanFieldContentBeIncluded(Field field, bool serialize, List ListObj)
        {
            bool result = false;
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
            return result;
        }
        private FieldValueProvider GetFieldValueProvider(Field field, Web web)
        {
            FieldValueProvider provider = null;

            if (null == m_dictFieldValueProviders)
            {
                m_dictFieldValueProviders = new Dictionary<string, FieldValueProvider>();
            }
            if (!m_dictFieldValueProviders.TryGetValue(field.InternalName, out provider))
            {
                provider = new FieldValueProvider(field, web);
                m_dictFieldValueProviders.Add(field.InternalName, provider);
            }

            return provider;
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
                lblSourceSiteErrorLog.Text = ex.ToString();
                return;
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

        private void rbNewSite_CheckedChanged(object sender, EventArgs e)
        {
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSiteBuilder);
            pnlProvision.Show();
        }

        private void rbSiteReplica_CheckedChanged(object sender, EventArgs e)
        {
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSourceSite);
            tabSiteMigration.Controls.Add(tabTargetSite);
        }

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
                CreateSite(txtSiteUrlName.Text, passWord, txtEmail.Text, txtSiteURLTitle.Text,i);
                i = 0;
                SiteBuilderProgressBar.Value = 100;
            }
            catch (Exception ex)
            {
                lblSiteBuilderErrorLog.Text = ex.ToString();
            }
           
        }
        private void CreateSite(string SiteURL, SecureString SourcePassword, string Email, string SiteTitle,int progress)
        {
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
                        if(progress == 50)
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
                    if (strSiteTemplateString.Equals(provisioningTemplate.BaseSiteTemplate))
                    {
                        Siteweb.ApplyProvisioningTemplate(provisioningTemplate,ptai);
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSiteBuilder);
        }

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

        private void btnTargetFileLocation_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    txtTargetFileLocation.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                lblTargetSiteErrorLog.Text = ex.ToString();
            }
        }

        private void rbExistingSite_CheckedChanged(object sender, EventArgs e)
        {
            tabSiteMigration.Controls.Clear();
            tabSiteMigration.Controls.Add(tabSiteBuilder);
            pnlProvision.Show();
        }

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


        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar  
            SourceSiteProgressBar.Value = e.ProgressPercentage;
            // Set the text.  
            this.Text = "Progress: " + e.ProgressPercentage.ToString() + "%";
        }

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
            catch (Exception)
            {

                throw;
            }
        }
        private void AddIDMappingEntry(ListItem item, DataRow dataRow)
        {
            int oldId;
            string strId;
            if (dataRow.Values.TryGetValue("Country", out strId) && int.TryParse(strId, out oldId) && (0 < oldId))
            {
                if (null == m_mappingIDs)
                {
                    m_mappingIDs = new Dictionary<int, int>();
                }
                m_mappingIDs[oldId] = item.Id;

            }
        }
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
                    // bool IsGroupAvailable = false;
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
                        string strGroup = "";
                        strGroup += grp.Member.Title + " : ";
                        GroupCreationInformation groupCreationInfo = new GroupCreationInformation();
                        groupCreationInfo.Title = grp.Member.Title;
                        // groupCreationInfo.Description = ;
                        Group oGroup = Targetweb.SiteGroups.Add(groupCreationInfo);
                        TargetClientContext.Load(oGroup);
                        RoleDefinitionBindingCollection collRoleDefinitionBinding = new RoleDefinitionBindingCollection(TargetClientContext);
                        foreach (SP.RoleDefinition oRoleDefinition in grp.RoleDefinitionBindings)
                        {
                            // strGroup += rd.Name + " ";
                            collRoleDefinitionBinding.Add(oRoleDefinition);
                            TargetClientContext.Load(oRoleDefinition);
                        }
                        Targetweb.RoleAssignments.Add(oGroup, collRoleDefinitionBinding);
                        TargetClientContext.ExecuteQuery();
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
        private Dictionary<string, string> GetSitePermissionDetails(ClientContext clientContext)
        {
            IEnumerable roles = clientContext.LoadQuery(clientContext.Web.RoleAssignments.Include(roleAsg => roleAsg.Member,
            roleAsg => roleAsg.RoleDefinitionBindings.Include(roleDef => roleDef.Name)));
            clientContext.ExecuteQuery();

            Dictionary<string, string> permisionDetails = new Dictionary<string, string>();
            foreach (Microsoft.SharePoint.Client.RoleAssignment ra in roles)
            {
                var rdc = ra.RoleDefinitionBindings;
                string permission = string.Empty;
                foreach (var rdbc in rdc)
                {
                    permission += rdbc.Name.ToString() + ", ";
                }


                if (!permisionDetails.ContainsKey(ra.Member.Title))
                    permisionDetails.Add(ra.Member.Title, permission);
            }
            return permisionDetails;
        }

        private void RemoveCalculatedColumnBeforeApplyTemplate(ClientContext clientContext)
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

        private static string GetTenantNameFromUrl(string tenantUrl)
        {
            if (tenantUrl.ToLower().Contains("-admin.sharepoint."))
            {
                return GetSubstringFromMiddle(tenantUrl, "https://", "-admin.sharepoint.");
            }
            else
            {
                return GetSubstringFromMiddle(tenantUrl, "https://", ".sharepoint.");
            }
        }

        private static string GetSubstringFromMiddle(string originalString, string prefix, string suffix)
        {
            var index = originalString.IndexOf(suffix, StringComparison.OrdinalIgnoreCase);
            return index != -1 ? originalString.Substring(prefix.Length, index - prefix.Length) : null;
        }

        #region Nested Types

        internal class LookupDataRef
        {
            #region Constructors

            public LookupDataRef(FieldLookup field)
            {
                this.Field = field;
                this.ItemLookupValues = new Dictionary<ListItem, object>();
            }

            #endregion Constructors

            #region Properties

            public FieldLookup Field
            {
                get; private set;
            }

            public Dictionary<ListItem, object> ItemLookupValues
            {
                get; private set;
            }

            #endregion Properties
        }

        private class ListItemInfo
        {
            #region Properties

            public ListItem Item
            {
                get; set;
            }

            public DataRow Row
            {
                get; set;
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}
