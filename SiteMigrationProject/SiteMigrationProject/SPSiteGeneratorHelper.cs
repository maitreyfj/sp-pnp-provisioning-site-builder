using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SiteMigrationProject
{
    public static class CommonHelper
    {
        public const string CLASS_NAME = "CommonHelper";
        public static int[] AllowedBaseTemplates = { CommonConstants.CUSTOM_LIST_BASETEMPLATE,
                                                   CommonConstants.DOCUMENT_LIBRARY_BASETEMPLATE,
                                                   CommonConstants.PROMOTED_LINKS_BASETEMPLATE,
                                                    CommonConstants.PICTURE_LIBRARY_BASETEMPLATE,
                                                   CommonConstants.CALENDER_BASETEMPLATE,
                                                   CommonConstants.TASK_BASETEMPLATE,
                                                   CommonConstants.TASKSWITHTIMELINEANDHIERARCHY_BASETEMPLATE,
                                                   CommonConstants.WEBPAGELIBRARY_BASETEMPLATE,
                                                   CommonConstants.DISCUSSION_BOARD_BASETEMPLATE,
                                                   CommonConstants.ANNOUNCEMENTS_BASETEMPLATE,
                                                   CommonConstants.LINKS_BASETEMPLATE};
        
      
    }
}
