using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using OfficeDevPnP.Core.Diagnostics;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using List = Microsoft.SharePoint.Client.List;

namespace SiteMigrationProject
{
   public class ListItemsProvider
    {
        private Dictionary<string, FieldValueProvider> m_dictFieldValueProviders = null;
        private Dictionary<string, LookupDataRef> m_lookups = null;
        private Dictionary<int, int> m_mappingIDs = null;

        public ClientRuntimeContext Context
        {
            get
            {
                return this.Web.Context;
            }
        }

        public List List
        {
            get; private set;
        }

        public Web Web
        {
            get; private set;
        }

        public ListItemsProvider(List list, Web web, ProvisioningTemplate template)
        {
            this.List = list;
            this.Web = web;
        }
        public void UpdateLookups(Func<System.Guid, ListItemsProvider> fnGetLookupDependentProvider)
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
                            //scope.LogError(ex, "Failed to get source list for lookup field. Field Name: {0}, Source List: {1}.",
                             //   lookupData.Field.InternalName, lookupData.Field.LookupList);
                        }
                        if (!Guid.Empty.Equals(sourceListId))
                        {
                            ListItemsProvider sourceProvider = fnGetLookupDependentProvider(sourceListId);
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
                        //scope.LogError(ex, "Failed to set lookup values. List: '{0}', Lookup Fields: {1}.", this.List.Title, lookupFieldNames);
                    }
                }
            }
        }
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
    }
}
