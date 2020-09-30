
namespace SiteMigrationProject
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.SharePoint.Client;

    using OfficeDevPnP.Core.Diagnostics;
    using OfficeDevPnP.Core.Framework.Provisioning.Model;

    using Field = Microsoft.SharePoint.Client.Field;

    public class FieldValueProvider
    {
        #region Fields

        private ObjectFieldValueBase _objectFieldValue = null;

        #endregion Fields

        #region Constructors

        public FieldValueProvider(Field field, Web web)
        {
            this.Field = field;
            this.Web = web;
        }

        #endregion Constructors

        #region Properties

        public Field Field
        {
            get; private set;
        }

        public Web Web
        {
            get; private set;
        }

        private ObjectFieldValueBase ObjectFieldValue
        {
            get
            {
                if (null == this._objectFieldValue)
                {
                    this._objectFieldValue = CreateObjectValueTyped();
                }
                return this._objectFieldValue;
            }
        }

        #endregion Properties

        #region Methods

        public object GetFieldValueTyped(string value)
        {
            object valueTyped = this.ObjectFieldValue.GetFieldValueTyped(value);
            return valueTyped;
        }

        public string GetValidatedValue(object value)
        {
            string dbValue = this.ObjectFieldValue.GetValidatedValue(value);
            return dbValue;
        }

        private ObjectFieldValueBase CreateObjectValueTyped()
        {
            if (this.Field.InternalName == "ID")
            {
                return new ObjectFieldValueID(this.Field, this.Web);
            }
            else
            {
                switch (this.Field.FieldTypeKind)
                {
                    case FieldType.Text:
                    case FieldType.Note:
                        return new ObjectFieldValueBase(this.Field, this.Web);
                    case FieldType.Boolean:
                        return new ObjectFieldValueBoolean(this.Field, this.Web);
                    case FieldType.Counter:
                    case FieldType.Number:
                    case FieldType.Currency:
                        return new ObjectFieldValueNumber(this.Field, this.Web);
                    case FieldType.DateTime:
                        return new ObjectFieldValueDateTime(this.Field, this.Web);
                    case FieldType.Lookup:
                        return new ObjectFieldValueLookup(this.Field, this.Web);
                    case FieldType.User:
                        return new ObjectFieldValueUser(this.Field, this.Web);
                    case FieldType.URL:
                        return new ObjectFieldValueURL(this.Field, this.Web);
                    case FieldType.Choice:
                        return new ObjectFieldValueChoice(this.Field, this.Web);
                    case FieldType.MultiChoice:
                        return new ObjectFieldValueChoiceMulti(this.Field, this.Web);
                    case FieldType.ContentTypeId:
                        return new ObjectFieldValueContentTypeId(this.Field, this.Web);
                    case FieldType.Geolocation:
                        return new ObjectFieldValueGeolocation(this.Field, this.Web);
                    default:
                        return new ObjectFieldValueBase(this.Field, this.Web);
                }
            }
        }

        #endregion Methods

        #region Nested Types

        internal class ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueBase(Field field, Web web)
            {
                this.Field = field;
                this.Web = web;
            }

            #endregion Constructors

            #region Properties

            public ClientRuntimeContext Context
            {
                get
                {
                    return this.Web.Context;
                }
            }

            public Field Field
            {
                get; private set;
            }

            public Web Web
            {
                get; private set;
            }

            #endregion Properties

            #region Methods

            public virtual object GetFieldValueTyped(string value)
            {
                object valueTyped = value;
                return valueTyped;
            }

            public virtual string GetValidatedValue(object value)
            {
                string str = "";
                if (null != value)
                {
                    str = value.ToString();
                }
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueBoolean : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueBoolean(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override string GetValidatedValue(object value)
            {
                string str = "";
                if (value is bool)
                {
                    str = ((bool)value) ? "TRUE" : "FALSE";
                }
                else
                {
                    str = base.GetValidatedValue(value);
                }
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueChoice : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueChoice(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors
        }

        internal class ObjectFieldValueChoiceMulti : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueChoiceMulti(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override string GetValidatedValue(object value)
            {
                string str = "";
                string[] values = value as string[];
                if (null != values)
                {
                    str = string.Join(";#", values);
                }
                else
                {
                    str = base.GetValidatedValue(value);
                }
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueContentTypeId : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueContentTypeId(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors
        }

        internal class ObjectFieldValueDateTime : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueDateTime(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override string GetValidatedValue(object value)
            {
                string str = "";
                if (value is DateTime)
                {
                    str = ((DateTime)value).ToString("o", CultureInfo.InvariantCulture);
                }
                else
                {
                    str = base.GetValidatedValue(value);
                }
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueGeolocation : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueGeolocation(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override object GetFieldValueTyped(string value)
            {
                object itemValue = value;
                var geolocationArray = value.Split(',');
                if (geolocationArray.Length == 4)
                {
                    var geolocationValue = new FieldGeolocationValue
                    {
                        Altitude = Double.Parse(geolocationArray[0]),
                        Latitude = Double.Parse(geolocationArray[1]),
                        Longitude = Double.Parse(geolocationArray[2]),
                        Measure = Double.Parse(geolocationArray[3]),
                    };
                    itemValue = geolocationValue;
                }
                return itemValue;
            }

            public override string GetValidatedValue(object value)
            {
                string str = "";
                FieldGeolocationValue geoValue = value as FieldGeolocationValue;
                if (null != geoValue)
                {
                    str = string.Format("{0},{1},{2},{3}", geoValue.Altitude, geoValue.Latitude, geoValue.Longitude, geoValue.Measure);
                }
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueID : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueID(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override string GetValidatedValue(object value)
            {
                double num = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                string str = Convert.ToString(num, CultureInfo.InvariantCulture);
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueLookup : ObjectFieldValueBase
        {
            #region Fields

            private const string SEPARATOR = ";#";

            #endregion Fields

            #region Constructors

            public ObjectFieldValueLookup(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override object GetFieldValueTyped(string value)
            {
                object valueTyped = null;
                FieldLookup fieldLookup = this.Field as FieldLookup;
                if (null != fieldLookup)
                {
                    if (fieldLookup.AllowMultipleValues)
                    {
                        List<FieldLookupValue> itemValues = new List<FieldLookupValue>();
                        string[] parts = value.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string str in parts)
                        {
                            int id;
                            if (int.TryParse(str, out id) && (0 < id))
                            {
                                itemValues.Add(new FieldLookupValue()
                                {
                                    LookupId = id
                                });
                            }
                        }
                        if (0 < itemValues.Count)
                        {
                            valueTyped = itemValues.ToArray();
                        }
                    }
                    else
                    {
                        int id;
                        if (int.TryParse(value, out id) && (0 < id))
                        {
                            valueTyped = new FieldLookupValue()
                            {
                                LookupId = id
                            };
                        }
                    }
                }
                return valueTyped;
            }

            public override string GetValidatedValue(object value)
            {
                string str = "";
                FieldLookupValue lookupValue = value as FieldLookupValue;
                if (null != lookupValue)
                {
                    if (0 < lookupValue.LookupId)
                    {
                        str = lookupValue.LookupId.ToString(CultureInfo.InvariantCulture); //lookupValue.LookupId.ToString(CultureInfo.InvariantCulture) + SEPARATOR + lookupValue.LookupValue;
                    }
                }
                else
                {
                    FieldLookupValue[] lookupValues = value as FieldLookupValue[];
                    if (null != lookupValues)
                    {
                        List<string> parts = new List<string>();
                        foreach (FieldLookupValue val in lookupValues)
                        {
                            parts.Add(val.LookupId.ToString(CultureInfo.InvariantCulture));
                            //parts.Add(val.LookupValue.Replace(";", ";;"));
                        }
                        str = string.Join(SEPARATOR, parts.ToArray());
                    }
                    else
                    {
                        str = base.GetValidatedValue(value);
                    }
                }
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueNumber : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueNumber(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override string GetValidatedValue(object value)
            {
                double num = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                string str = Convert.ToString(num, CultureInfo.InvariantCulture);
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueURL : ObjectFieldValueBase
        {
            #region Constructors

            public ObjectFieldValueURL(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override object GetFieldValueTyped(string value)
            {
                var linkValue = new FieldUrlValue();
                var idx = value.IndexOf(',');
                linkValue.Url = (-1 != idx) ? value.Substring(0, idx) : value;
                linkValue.Description = (-1 != idx) ? value.Substring(idx + 1) : value;
                return linkValue;
            }

            public override string GetValidatedValue(object value)
            {
                string str = "";
                FieldUrlValue urlValue = value as FieldUrlValue;
                if (null != urlValue)
                {
                    str = string.Format("{0},{1}", urlValue.Url, urlValue.Description);
                }
                else
                {
                    str = base.GetValidatedValue(value);
                }
                return str;
            }

            #endregion Methods
        }

        internal class ObjectFieldValueUser : ObjectFieldValueBase
        {
            #region Fields

            private Dictionary<int, string> m_dictUserCache = null;

            #endregion Fields

            #region Constructors

            public ObjectFieldValueUser(Field field, Web web)
                : base(field, web)
            {
            }

            #endregion Constructors

            #region Methods

            public override object GetFieldValueTyped(string value)
            {
                object userValue = null;
                FieldUser fieldUser = this.Field as FieldUser;
                if (null != fieldUser)
                {
                    string[] logins = null;
                    if (fieldUser.AllowMultipleValues)
                    {
                        logins = value.Split(';');
                    }
                    else
                    {
                        logins = new string[1]
                        {
                            value
                        };
                    }

                    if (fieldUser.AllowMultipleValues)
                    {
                        List<FieldUserValue> values = new List<FieldUserValue>();
                        foreach (string login in logins)
                        {
                            values.Add(FieldUserValue.FromUser(login));
                        }
                        userValue = values.ToArray();
                    }
                    else
                    {
                        userValue = FieldUserValue.FromUser(logins[0]);
                    }
                }
                return userValue;
            }

            public override string GetValidatedValue(object value)
            {
                string str = "";
                FieldUserValue userValue = value as FieldUserValue;
                if (null != userValue)
                {
                    str = GetUserLoginById(userValue);
                }
                else
                {
                    FieldUserValue[] userValues = value as FieldUserValue[];
                    if (null != userValues)
                    {
                        List<string> logins = new List<string>();
                        foreach (FieldUserValue val in userValues)
                        {
                            string login = GetUserLoginById(val);
                            logins.Add(login);
                        }
                        str = string.Join(";", logins.ToArray());
                    }
                    else
                    {
                        str = base.GetValidatedValue(value);
                    }
                }
                return str;
            }

            private string GetUserLoginById(FieldUserValue userValue)
            {
                string loginName = "";

                if (null == m_dictUserCache)
                {
                    m_dictUserCache = new Dictionary<int, string>();
                }
                string dictValue = "";
                if (m_dictUserCache.TryGetValue(userValue.LookupId, out dictValue))
                {
                    loginName = dictValue;
                }
                else
                {
                    try
                    {
                        var user = this.Web.GetUserById(userValue.LookupId);

                        this.Context.Load(user, u => u.LoginName);
                        this.Context.ExecuteQuery();
                        loginName = user.LoginName;
                    }
                    catch (Exception ex)
                    {
                        //Log.Error(ex, Constants.LOGGING_SOURCE, "Failed to get user by id. User Title: '{0}', User ID:{1}", userValue.LookupValue, userValue.LookupId);
                    }
                    m_dictUserCache.Add(userValue.LookupId, loginName);
                }
                return loginName;
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}
namespace OfficeDevPnP.Core.Framework.Provisioning.ObjectHandlers.Export.ListContent
{
   
}