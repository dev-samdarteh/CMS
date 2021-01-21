
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhemaCMS.Models.Adhoc
{
    public class GenUtility
    {
        public GenUtility() 
        { }

       // public ChurchBody ChurchBody { get; set; }
        //public UserProfile UserProfile { get; set; }
        //// public string logUserDesc { get; set; }
        //public string PermissionName { get; set; }
        //public bool PermissionValue { get; set; }
    }


    public class DiscreteLookup
    {
        public DiscreteLookup() { }

        public string Val { get; set; }
        public string Desc { get; set; }
        public string Category { get; set; }

        public List<DiscreteLookup> EntityStatusList { get; set; }
    }

    public class NumberDiscreteLookup
    {
        public NumberDiscreteLookup() { }

        public decimal Val { get; set; }
        public string Desc { get; set; }
        public string Category { get; set; }

        public List<NumberDiscreteLookup> EntityStatusList { get; set; }
    }
}
