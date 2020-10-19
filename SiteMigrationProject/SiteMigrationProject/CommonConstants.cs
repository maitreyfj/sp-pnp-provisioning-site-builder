using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMigrationProject
{
    class CommonConstants
    {
        public const string FIELDTYPE_LOOKUP = "Lookup";
        public const string FIELDTYPE_LOOKUP_MULTI = "LookupMulti";
        public const string FIELDTYPE_USER = "User";
        public const string FIELDTYPE_URL = "URL";
        public const int LOOKUP_COLUMN_BUNCH = 8; // specifies bunch of lookup columns to be processed while fetching updating  
        public const string ATTACHMENTS = "Attachments";
        public const string CONTENT_TYPE = "ContentType";
        public const string TITLE = "Title";
        public const string ID = "ID";

        public const int CUSTOM_LIST_BASETEMPLATE = 100;
        public const int DOCUMENT_LIBRARY_BASETEMPLATE = 101;
        public const int PICTURE_LIBRARY_BASETEMPLATE = 109;
        public const int CALENDER_BASETEMPLATE = 106;
        public const int TASK_BASETEMPLATE = 107;
        public const int TASKSWITHTIMELINEANDHIERARCHY_BASETEMPLATE = 171;
        public const int PROMOTED_LINKS_BASETEMPLATE = 170;
        public const int WEBPAGELIBRARY_BASETEMPLATE = 119;
        public const int DISCUSSION_BOARD_BASETEMPLATE = 108;
        public const int ANNOUNCEMENTS_BASETEMPLATE = 104;
        public const int LINKS_BASETEMPLATE = 103;

        public const int BUNCH_CHECK_ITEMS_MORE_THAN = 5000;
        public const int GET_ITEMS_BUNCH_SIZE = 50;
        public const int RUN_GC_ITEMS_COUNT = 50;
    }
}
