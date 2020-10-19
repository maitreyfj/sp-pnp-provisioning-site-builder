using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMigrationProject
{
    class SPSiteGeneratorModels
    {
    }
    public class LookupColumnDetails
    {
        public string CurrentListTitle { get; set; }
        public string CurrentColumnColumnTitle { get; set; }

        public string CurrentColumnColumnInternalName { get; set; }
        public string LookupParentListTitle { get; set; }
        public string LookupColumnShowField { get; set; }
        public string LookupColumnShowFieldType { get; set; }
    }

    public class SourceDestIDMapping
    {
        public string ListTitle { get; set; }
        public int SourceItemID { get; set; }

        public int DestItemID { get; set; }
    }
}
