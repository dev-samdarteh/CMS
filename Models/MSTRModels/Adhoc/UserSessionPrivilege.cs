using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhemaCMS.Models.MSTRModels
{
    public class UserSessionPrivilege
    {

        public UserSessionPrivilege()
        { }

        public MSTRAppGlobalOwner AppGlobalOwner { get; set; }
        public MSTRChurchBody ChurchBody { get; set; }
        public UserProfile UserProfile { get; set; }
        public UserPermission UserPermission { get; set; }
        public UserRole UserRole { get; set; }
        public UserGroup UserGroup { get; set; }
        // public string logUserDesc { get; set; }
        public string PermissionName { get; set; }
        public bool PermissionValue { get; set; }

        public string RoleName { get; set; }
        public string GroupName { get; set; }
        public string strChurchCode_AGO { get; set; }
        public string strChurchCode_CB { get; set; }

        public bool ViewPerm { get; set; }
        public bool CreatePerm { get; set; }
        public bool EditPerm { get; set; }
        public bool DeletePerm { get; set; }
        public bool ManagePerm { get; set; } 
    }


    //public class DiscreteLookup
    //{
    //    public DiscreteLookup() { }

    //    public string Val { get; set; }
    //    public string Desc { get; set; }
    //    public string Category { get; set; }

    //    public List<DiscreteLookup> EntityStatusList { get; set; }
    //}

    //public class NumberDiscreteLookup
    //{
    //    public NumberDiscreteLookup() { }

    //    public decimal Val { get; set; }
    //    public string Desc { get; set; }
    //    public string Category { get; set; }

    //    public List<NumberDiscreteLookup> EntityStatusList { get; set; }
    //}
}

 