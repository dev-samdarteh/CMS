//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using RhemaCMS.Controllers.con_adhc;
//using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.CLNTModels;
//using RhemaCMS.Models.MSTRModels;
//using RhemaCMS.Models.ViewModels.vm_app_ven;

//namespace RhemaCMS.Controllers.con_app_va
//{
//    public class ChurchBodyConfigController : Controller
//    {
//        private readonly MSTR_DbContext _masterContext;
//        private readonly ChurchModelContext _context; // _clientContext;
//        private readonly IHostingEnvironment hostingEnvironment;

//        private bool isCurrValid = false;
//        private List<UserSessionPrivilege> oUserLogIn_Priv = null;
//        private List<DiscreteLookup> dlStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlAssocType = new List<DiscreteLookup>();


//        private List<DiscreteLookup> dlCBDivOrgTypes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlShareStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlOwnerStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlGenStatuses = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlChurchType = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlChuWorkStat = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlUserRoleTypes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlUserAuthTypes = new List<DiscreteLookup>();


//        public ChurchBodyConfigController(ChurchModelContext context, MSTR_DbContext ctx, IHostingEnvironment hostingEnvironment)
//        { 
//            _context = context;
//            _masterContext = ctx; // _clientContext = ctx;
//            this.hostingEnvironment = hostingEnvironment; 


//            dlStatus.Add(new DiscreteLookup() { Category = "Status", Val = "A", Desc = "Active" });
//            dlStatus.Add(new DiscreteLookup() { Category = "Status", Val = "D", Desc = "Deactive" });
//            dlStatus.Add(new DiscreteLookup() { Category = "Status", Val = "E", Desc = "Expired" });

//            //CF--church fellowship, CH--Church hierarchy   N = NTWK, F = FRLC
//            dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CH", Desc = "Hierarchy" });
//            dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CF", Desc = "Congregation" });

//            dlAssocType.Add(new DiscreteLookup() { Category = "AssocType", Val = "N", Desc = "Networked" });
//            dlAssocType.Add(new DiscreteLookup() { Category = "AssocType", Val = "F", Desc = "Freelance" });



//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "D", Desc = "Deactive" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "E", Desc = "Expired" });

//            //SharingStatus { get; set; }  // A - Share with all sub-congregations, C - Share with child congregations only, N - Do not share
//            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "N", Desc = "Do not roll-down (share)" });
//            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "C", Desc = "Roll-down (share) for direct child congregations" });
//            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "A", Desc = "Roll-down (share) for all sub-congregations" });

//            // OwnershipStatus { get; set; }  // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody
//            dlOwnerStatus.Add(new DiscreteLookup() { Category = "OwnStat", Val = "O", Desc = "Originated" });
//            dlOwnerStatus.Add(new DiscreteLookup() { Category = "OwnStat", Val = "I", Desc = "Inherited" });


//            // dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CR", Desc = "Church Root" });
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "GB", Desc = "Governing Body" });
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CO", Desc = "Church Office" });
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "DP", Desc = "Department" });  //Ministry
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CG", Desc = "Church Grouping" });
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "SC", Desc = "Standing Committee" });
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CE", Desc = "Church Enterprise" });
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "TM", Desc = "Team" });
//            //dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CP", Desc = "Church Position" });
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "IB", Desc = "Independent Unit" });
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CN", Desc = "Congregation" });  //to look up congregation by church code [short or full path]

//            dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "", Desc = "N/A" });
//            dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CH", Desc = "Congregation Head-unit" });
//            dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CF", Desc = "Congregation" });

//            dlChuWorkStat.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "S", Desc = "Structure Only" });
//            dlChuWorkStat.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "O", Desc = "Operationalized" });


//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "SYS", Desc = "System" }); // 0
//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "SUP_ADMN", Desc = "Super Admin" }); // 1
//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "SYS_ADMN", Desc = "System Admin" });  // 2

//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CH_ADMN", Desc = "Church Admin" }); // 3
//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CH_RGSTR", Desc = "Church Registrar" }); // 4
//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CH_ACCT", Desc = "Church Accountant" });// 5
//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CH_CUST", Desc = "Church Custom" }); // 6

//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CF_ADMN", Desc = "Congregation Admin" }); //  7             
//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CF_RGSTR", Desc = "Congregation Registrar" }); // 8          
//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CF_ACCT", Desc = "Congregation Accountant" });  // 9          
//            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CF_CUST", Desc = "Congregation Custom" }); // 10  

//            dlUserAuthTypes.Add(new DiscreteLookup() { Category = "UserAuthType", Val = "1", Desc = "Two-way Authentication" });
//            dlUserAuthTypes.Add(new DiscreteLookup() { Category = "UserAuthType", Val = "2", Desc = "Security Question Validation" });

//        }

//        public string GetStatusDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "A": return "Active";
//                case "D": return "Deactive";
//                case "E": return "Expired"; 

//                default: return oCode;
//            }
//        }
//        public static string GetStatusDescExt(string oCode)
//        {
//            switch (oCode)
//            {
//                case "A": return "Active";
//                case "D": return "Deactive";
//                case "P": return "Pending";
//                case "E": return "Expired";


//                default: return oCode;
//            }
//        }
//        public int GetRoleTypeLevel(string oCode)
//        {
//            switch (oCode)
//            {
//                case "SYS": return 1;
//                case "SUP_ADMN": return 2;
//                case "SYS_ADMN": return 3;
//                case "SYS_CUST": return 4;
//                // case "SYS_CUST2": return 5;
//                //
//                case "CH_ADMN": return 6;
//                case "CH_RGSTR": return 7;
//                case "CH_ACCT": return 8;
//                case "CH_CUST": return 9;
//                // case "CH_CUST2": return 10;
//                //
//                case "CF_ADMN": return 11;
//                case "CF_RGSTR": return 12;
//                case "CF_ACCT": return 13;
//                case "CF_CUST": return 14;
//                // case "CF_CUST2": return 15; 
//                //
//                default: return 0;
//            }
//        }
//        public string GetConcatMemberName(string title, string fn, string mn, string ln, bool displayName = false)
//        {
//            if (displayName)
//                return ((((!string.IsNullOrEmpty(title) ? title : "") + ' ' + fn).Trim() + " " + mn).Trim() + " " + ln).Trim();
//            else
//                return (((fn + ' ' + mn).Trim() + " " + ln).Trim() + " " + (!string.IsNullOrEmpty(title) ? "(" + title + ")" : "")).Trim();
//        }

//        public List<ChurchBody> GetChurchBodyList(int oChurchBodyId)
//        {
//            var oChurchBody = _context.ChurchBody.Include(t => t.ChurchLevel).Include(t => t.AppGlobalOwner).Where(x => x.Id == oChurchBodyId).FirstOrDefault();
//            //only Top leaders can perform Transfers == CM, CA, CL
//            List<ChurchBody> chuBodyList = new List<ChurchBody>();
//            if (oChurchBody == null) return chuBodyList;

//            chuBodyList = _context.ChurchBody
//                .Where(c => c.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId)
//                .OrderBy(c => c.ParentChurchBodyId).ThenBy(c => c.Name)
//                .ToList();

//            //.Select(c => new SelectListItem()
//            //{
//            //    Value = c.Id.ToString(),
//            //    Text = c.Name
//            //})
//            // .OrderBy(c => c.Text)
//            //.ToList();

//            //index 0
//            //chuMemberList.Insert(0, new MemberProfileVM { MemberId = 0, strMemberFullName = "Select" });

//            return chuBodyList; //Json(new SelectList(chuMemberList, "MemberId", "strMemberFullName"));            
//        }


//        public List<ChurchBody> GetChurchBodySubCategoryList(int oAppOwnId, int? oCBCategoryId, int? oCurrChurchBodyId, int qryIndx, bool inclCurrLevel = false)
//        {
//            List<ChurchBody> chuBodyList = new List<ChurchBody>();
//            // var oCurrCategChurchBody = _context.ChurchBody.Include(t => t.ChurchLevel).Where(x => x.Id == oCBCategoryId).FirstOrDefault();
//            //if (oCurrCategChurchBody == null) { ViewBag.strChurchCategLevel = "Level Unknown"; return chuBodyList; }
//            //else { ViewBag.strChurchCategLevel = oCurrCategChurchBody.ChurchLevel?.CustomName; }

//            if (qryIndx == 1) // top categories... exclude top-most
//            {  //get the index... root category
//                if (inclCurrLevel)   //ie.  start from root index
//                {
//                    chuBodyList = _context.ChurchBody.Include(t => t.ChurchLevel)
//                        .Where(c => c.AppGlobalOwnerId == oAppOwnId && //c.ChurchType == "CH" && 
//                        c.ParentChurchBodyId == null).ToList();
//                }
//                else
//                {
//                    var oCBCategory = _context.ChurchBody.Include(t => t.ChurchLevel)
//                        .Where(c => c.AppGlobalOwnerId == oAppOwnId && //c.ChurchType == "CH" && 
//                        c.ParentChurchBodyId == null).FirstOrDefault();
//                    if (oCBCategory != null)
//                    {
//                        oCBCategoryId = oCBCategory.Id;
//                        chuBodyList = _context.ChurchBody.Include(t => t.ChurchLevel).Include(t => t.AppGlobalOwner)
//                        .Where(c => c.AppGlobalOwnerId == oAppOwnId && //c.ChurchType == "CH" && 
//                                                        c.ParentChurchBodyId == oCBCategoryId &&
//                        c.ChurchLevel.LevelIndex == _context.ChurchLevel.Where(y => y.AppGlobalOwnerId == oAppOwnId).Min(y => y.LevelIndex) + 1 &&   //(inclCurrLevel ? 0 : 1)
//                        c.ChurchLevel.LevelIndex < _context.ChurchLevel.Where(y => y.AppGlobalOwnerId == oAppOwnId).Max(y => y.LevelIndex))
//                        .OrderBy(c => c.ParentChurchBodyId).ThenBy(c => c.Name)
//                        .ToList();
//                    }
//                }

//                if (chuBodyList.Count > 0) ViewBag.strChurchCategLevel = chuBodyList[0].ChurchLevel?.CustomName;
//                else ViewBag.strChurchCategLevel = "Level Unknown";
//            }
//            else if (qryIndx == 2) // next sub categories
//            {
//                if (oCBCategoryId != null)
//                {
//                    chuBodyList = _context.ChurchBody.Include(t => t.ChurchLevel).Include(t => t.AppGlobalOwner)
//                   .Where(c => c.AppGlobalOwnerId == oAppOwnId && // c.ChurchType == "CH" && 
//                                                    c.ParentChurchBodyId == oCBCategoryId &&
//                    c.ChurchLevel.LevelIndex < _context.ChurchLevel.Where(y => y.AppGlobalOwnerId == oAppOwnId).Max(y => y.LevelIndex))
//                    .OrderBy(c => c.ParentChurchBodyId).ThenBy(c => c.Name)
//                    .ToList();
//                }

//                if (chuBodyList.Count > 0) ViewBag.strChurchCategLevel = chuBodyList[0].ChurchLevel?.CustomName;
//                else ViewBag.strChurchCategLevel = "Level Unknown";
//            }
//            else if (qryIndx == 3) // bottom categories... congregations
//            {
//                if (oCBCategoryId != null)
//                {
//                    chuBodyList = _context.ChurchBody.Include(t => t.ChurchLevel).Include(t => t.AppGlobalOwner)
//                    .Where(c => c.AppGlobalOwnerId == oAppOwnId && // c.ChurchType == "CF" && 
//                                                c.ParentChurchBodyId == oCBCategoryId && c.Id != oCurrChurchBodyId &&
//                    // c.ChurchLevel.LevelIndex == _context.ChurchLevel.Where(y => y.AppGlobalOwnerId == oAppOwnId).Min(y => y.LevelIndex) + 1 &&
//                    c.ChurchLevel.LevelIndex == _context.ChurchLevel.Where(y => y.AppGlobalOwnerId == oAppOwnId).Max(y => y.LevelIndex))
//                    .OrderBy(c => c.ParentChurchBodyId).ThenBy(c => c.Name)
//                    .ToList();
//                }

//                if (chuBodyList.Count > 0) ViewBag.strChurchCategLevel = chuBodyList[0].ChurchLevel?.CustomName;
//                else ViewBag.strChurchCategLevel = "Level Unknown";
//            }

//            return chuBodyList;
//        }

         


//        private bool userAuthorized = false;
//        private void SetUserLogged()
//        {
//            ////  oUserLogIn_Priv = TempData.Get<List<UserSessionPrivilege>>("UserLogIn_oUserPrivCol");

//            //List<UserSessionPrivilege> oUserLogIn_Priv = TempData.ContainsKey("UserLogIn_oUserPrivCol") ?
//            //                                                TempData["UserLogIn_oUserPrivCol"] as List<UserSessionPrivilege> : null;

//            if (TempData.ContainsKey("UserLogIn_oUserPrivCol"))
//            {
//                var tempPrivList = TempData["UserLogIn_oUserPrivCol"] as string;
//                if (string.IsNullOrEmpty(tempPrivList))
//                    RedirectToAction("LoginUserAcc", "UserLogin");
//                // De serialize the string to object
//                oUserLogIn_Priv = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);

//                isCurrValid = oUserLogIn_Priv?.Count > 0;
//                if (isCurrValid)
//                {
//                    ViewBag.oChuBodyLogged = oUserLogIn_Priv[0].ChurchBody;
//                    ViewBag.oUserLogged = oUserLogIn_Priv[0].UserProfile;

//                    // check permission for Core life...  given the sets of permissions
//                    userAuthorized = oUserLogIn_Priv.Count > 0; //(oUserLogIn_Priv.Find(x => x.PermissionName == "_A0__System_Administration" || x.PermissionName == "xxx") != null);
//                }
//            }
//            else RedirectToAction("LoginUserAcc", "UserLogin");


//        }

//        // GET: ChurchBodyConfig 
//        public ActionResult Index(int? oCurrChuBodyId=null, int setIndex = 0, int? oParentId = null, int? oChuCategId = null, bool oShowAllCong = true) //, int? currFilterVal = null) //, ChurchBodyConfigMDL oCurrCBConfig = null)
//        {
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                //if (setIndex == 1 || setIndex == 2)                
//                //    if (oCurrCBConfig.lsChurchFaithTypes != null)
//                //    {
//                //        oCurrCBConfig.lsChurchFaithTypes = oCurrCBConfig.lsChurchFaithTypes;
//                //        TempData.Put("oVmCB_CNFG", oCurrCBConfig); TempData.Keep(); return View(oCurrCBConfig);
//                //    }               
                    

//                var oCBConfigMDL = new ChurchBodyConfigMDL();  TempData.Keep();

//                var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                if (oCurrChuBodyLogOn == null) return View(oCBConfigMDL);
//                if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//                else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//                // check permission for Core life...
//                if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null)
//                    return View(oCBConfigMDL);

//                //int? oCurrChuMemberId_LogOn = null;
//                //ChurchMember oCurrChuMember_LogOn = null;
//                if (oUserProfile == null) return View(oCBConfigMDL);

//                //if (oUserProfile.ChurchMember == null) return View(oCBConfigMDL); 

//                //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//                //oCurrChuMember_LogOn = oUserProfile.ChurchMember;
//                //
//                oCBConfigMDL.oCurrLoggedMemberId = oUserProfile.ChurchMemberId; // oCurrChuMemberId_LogOn;
//             //   oCBConfigMDL.oCurrChurchBody = oCurrChuBodyLogOn;
//                oCBConfigMDL.setIndex = setIndex;
//                //                

//                if (setIndex == 1 || setIndex == 2) { //if (oCurrCBConfig.lsChurchFaithTypes != null) oCBConfigMDL.lsChurchFaithTypes = oCurrCBConfig.lsChurchFaithTypes;
//                   // oCBConfigMDL.lsChurchFaithTypes = GetChurchFaithTypes(setIndex); 
//                    oCBConfigMDL.strCurrTask = setIndex == 1 ? "Church sect (Faith stream)" : "Denomination class";
//                } // "Faith Types"; }

//                if (setIndex == 3) {
//                    oCBConfigMDL.oAppGlobalOwn_MDL = new AppGlobalOwner_MDL();
//                    oCBConfigMDL.oAppGlobalOwn_MDL.lsAppGlobalOwn_MDL = GetDenominations();
//                    oCBConfigMDL.strCurrTask = "Denominations & Church levels";
//                }
//                if (setIndex == 4) {
//                    oCBConfigMDL.oChurchLevel_MDL = new ChurchLevel_MDL();
//                    //get lookups
//                    oCBConfigMDL.oChurchLevel_MDL = this.popLookups_CL(oCBConfigMDL.oChurchLevel_MDL);
//                    oCBConfigMDL.oChurchLevel_MDL.oCurrAppGloId_Filter4 = oParentId; 
//                    //
//                    oCBConfigMDL.oChurchLevel_MDL.lsChurchLevel_MDL = GetChurchLevels(oParentId);
//                    oCBConfigMDL.strCurrTask = "Church Levels"; 
//                }
//                if (setIndex == 5)
//                {
//                    oCBConfigMDL.oChurchBody_MDL = new ChurchBody_MDL();
//                    //get lookups
//                    oCBConfigMDL.oChurchBody_MDL = this.popLookups_CB(oCBConfigMDL.oChurchBody_MDL, null, null);
//                    oCBConfigMDL.oChurchBody_MDL.oCurrAppGloId_Filter5 = oParentId;
//                    oCBConfigMDL.oChurchBody_MDL.oCurrChuCategId_Filter5 = oChuCategId;
//                    //
//                    oCBConfigMDL.oChurchBody_MDL.lsChurchBody_MDL = GetChurchBodies(oParentId, oChuCategId, oShowAllCong);
//                    oCBConfigMDL.strCurrTask = "Congregations"; 
//                }
//              //  if (setIndex == 6) { oCBConfigMDL.lsAppSubscriptions = GetAppSubscriptions(); oCBConfigMDL.strCurrTask = "App Subscriptions"; }
//                //if (setIndex == 7) { oCBConfigMDL.lsUserProfiles = GetUserProfiles(oCBConfigMDL.oChurchBody.Id); oCBConfigMDL.strCurrTask = "User Profiles"; }
//                //if (setIndex == 10) { oCBConfigMDL.lsCountries = GetCountries(); oCBConfigMDL.strCurrTask = "Countries & Regions"; }
//                //if (setIndex == 11) { oCBConfigMDL.lsCountryRegions = GetCountryRegions(oParentId); oCBConfigMDL.strCurrTask = "Country Regions"; }

//                //
//               // TempData.Put("oVmCB_CNFG", oCBConfigMDL);
//                TempData.Keep();
//                return View(oCBConfigMDL);
//            }
//        }
        
//        private List<AppGlobalOwner_MDL> GetDenominations()
//        {
//            //return _context.AppGlobalOwner.ToList();
//            return (
//                   from t_ago in _context.AppGlobalOwner.AsNoTracking().Include(t => t.ChurchLevels) //.Include(t => t.FaithTypeCategory).Where(c=> c.Id == id)
//                  // from t_cl in _context.ChurchLevel.AsNoTracking().Where(c=> c.AppGlobalOwnerId==t_ago.Id ).DefaultIfEmpty()
//                  // from t_ft in  _context.ChurchFaithType.AsNoTracking().Where(c=> c.Id==t_ago.FaithTypeCategoryId).DefaultIfEmpty()
//                   from t_ctr in _context.Country.AsNoTracking().Where(c=> c.Id == t_ago.CountryId).DefaultIfEmpty()
//                   select new AppGlobalOwner_MDL()
//                   {
//                       oAppGlobalOwn = t_ago,
//                       lsChurchLevels = t_ago.ChurchLevels,
//                       strDenomination = t_ago != null ? t_ago.OwnerName : "",
//                       strCountry = t_ctr != null ? t_ctr.Name : "",
//                       strFaithTypeCategory = t_ago.strFaithTypeCategory // t_ft != null ? t_ft.FaithDescription : "",
//                   }
//                   ).OrderBy(c => c.oAppGlobalOwn.CountryId).ThenBy(c => c.oAppGlobalOwn.FaithTypeCategoryId)
//                   .ThenBy(c => c.strDenomination).ToList();
//        }
//        private List<Country_MDL> GetCountries()
//        {
//            //return _context.AppGlobalOwner.ToList();
//            return (
//                   from t_ctr in _context.Country.AsNoTracking().Include(t => t.CountryRegions)  
//                   from t_rgn in _context.CountryRegion.AsNoTracking().Where(c=> c.CountryId == t_ctr.Id).DefaultIfEmpty()
//                   select new Country_MDL()
//                   {
//                       oCountry = t_ctr,
//                       lsCountryRegions = t_ctr.CountryRegions, 
//                       strCountry = t_ctr != null ? t_ctr.Name : ""
//                   }
//                   ).OrderBy(c => c.oCountry.Name).ToList();
//        }
//        private List<CountryRegion_MDL> GetCountryRegions(int? oCtryId)
//            {  //  return _context.AppGlobalOwner.ToList();
//            return (     
//                   from t_rgn in _context.CountryRegion.AsNoTracking().Include(t => t.Country).Where(c => c.CountryId == oCtryId) 
//                   select new CountryRegion_MDL()
//                   {
//                       oCountryRegion = t_rgn,
//                       oCountry = t_rgn != null ? t_rgn.Country : null, 
//                       strCountry = t_rgn != null ? t_rgn.Country != null ? t_rgn.Country.Name : "" : ""
//                   }
//                   ).OrderBy(c => c.oCountry.Name).ToList();
//        }
//        private List<ChurchBody_MDL> GetChurchBodies(int? oAppOwnId, int? oParCongId, bool oShowAllCong)
//        {
//            return (
//                   from t_cb in _context.ChurchBody.AsNoTracking()
//                       // .Include(t => t.AppGlobalOwner).Include(t => t.ParentChurchBody)
//                        .Include(t => t.Country).Include(t=>t.SubChurchUnits)
//                        .Where(c=>c.AppGlobalOwnerId == oAppOwnId && ((oParCongId==null && oShowAllCong) || (oShowAllCong==false && c.ParentChurchBodyId == null) || c.ParentChurchBodyId == oParCongId))
//                   from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(c=> c.Id == t_cb.AppGlobalOwnerId).DefaultIfEmpty()
//                   from t_cl in _context.ChurchLevel.AsNoTracking().Where(c=> c.Id == t_cb.ChurchLevelId).DefaultIfEmpty()
//                   from t_cb_c in _context.ChurchBody.AsNoTracking().Where(c=> c.Id == t_cb.ParentChurchBodyId).DefaultIfEmpty()
//                   select new ChurchBody_MDL()
//                   {
//                       oChurchBody = t_cb,
//                       strAppGlobalOwn = t_ago != null ? t_ago.OwnerName : "",
//                       lsSubChurchBodies = t_cb.SubChurchUnits.ToList(),
//                       strParentChurchBody = t_cb_c != null ? t_cb_c.Name : "",                       
//                       strChurchLevel = t_cl != null ? !string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name : "",
//                       strCountry = t_cb.Country != null ? t_cb.Country.Name : "",
//                       strCountryRegion = t_cb_c != null ? t_cb_c.Name : "",
//                     //  strChurchType = t_cb.ChurchType == "CH" ? "Hierarchy" : "Congregation",
//                      /// strAssociationType = t_cb.AssociationType == "N" ? "Networked" : "Freelance"
//                   }
//                   ).OrderBy(c => c.oChurchBody.ParentChurchBodyId).ThenBy(c => c.oChurchBody.Name).ToList();
//        }

//        //private List<ChurchBody> GetChurchBodyDetail(int? oAppOwnId) //AppGlobalOwner oAppOwn)
//        //{
//        //    return _context.ChurchBody.Include(t=>t.ChurchLevel).Where(c => c.AppGlobalOwnerId == oAppOwnId).ToList();
//        //}

//        private List<ChurchLevel_MDL> GetChurchLevels(int? oAppOwnId)
//        {
//            return (
//                   from t_cb in _context.ChurchLevel.AsNoTracking().Include(t => t.AppGlobalOwner)
//                        .Where(c => c.AppGlobalOwnerId == oAppOwnId)  
//                   from t_cb_c in _context.ChurchBody.AsNoTracking().Where(c=> c.ParentChurchBodyId == t_cb.Id).DefaultIfEmpty()
//                   select new ChurchLevel_MDL()
//                   {
//                       oChurchLevel = t_cb,
//                       strAppGlobalOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : ""
//                   }
//                   ).OrderBy(c => c.oChurchLevel.AppGlobalOwnerId).ThenBy(c => c.oChurchLevel.LevelIndex)
//                   .ToList();
//        }

//        public IActionResult PopChurchLevel(int? oCurrChuBodyId = null, int setIndex = 0, int? oParentId = null)
//        { 
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            { 
//                var oCBConfigMDL = new ChurchBodyConfigMDL(); TempData.Keep();

//                var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                if (oCurrChuBodyLogOn == null) return View(oCBConfigMDL);
//                if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//                else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//                // check permission for Core life...
//                if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null)
//                    return View(oCBConfigMDL);

//                //int? oCurrChuMemberId_LogOn = null;
//                //ChurchMember oCurrChuMember_LogOn = null;
//                //if (oUserProfile == null) return View(oCBConfigMDL);
//                //if (oUserProfile.ChurchMember == null) return View(oCBConfigMDL);

//                //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//                //oCurrChuMember_LogOn = oUserProfile.ChurchMember;
//                //
//                oCBConfigMDL.oCurrLoggedMemberId = oUserProfile.ChurchMemberId; // oCurrChuMemberId_LogOn;
//               // oCBConfigMDL.oCurrChurchBody = oCurrChuBodyLogOn;
//                oCBConfigMDL.setIndex = setIndex;

//                //                
//                oCBConfigMDL.oChurchLevel_MDL = new ChurchLevel_MDL();
//                oCBConfigMDL.oChurchLevel_MDL.lsChurchLevel_MDL = GetChurchLevels(oParentId);
//                TempData.Keep();
//                return View(oCBConfigMDL);
//            }
//        }
         
//        private IActionResult PopChurchBody(int? oCurrChuBodyId = null, int setIndex = 0, int? oParentId = null)
//        {
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                var oCBConfigMDL = new ChurchBodyConfigMDL(); TempData.Keep();

//                var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                if (oCurrChuBodyLogOn == null) return View(oCBConfigMDL);
//                if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//                else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//                // check permission for Core life...
//                if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null)
//                    return View(oCBConfigMDL);

//                //int? oCurrChuMemberId_LogOn = null;
//                //ChurchMember oCurrChuMember_LogOn = null;
//                //if (oUserProfile == null) return View(oCBConfigMDL);
//                //if (oUserProfile.ChurchMember == null) return View(oCBConfigMDL);

//                //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//                //oCurrChuMember_LogOn = oUserProfile.ChurchMember;
//                //
//                oCBConfigMDL.oCurrLoggedMemberId = oUserProfile.ChurchMemberId; //oCurrChuMemberId_LogOn;
//              //  oCBConfigMDL.oCurrChurchBody = oCurrChuBodyLogOn;
//                oCBConfigMDL.setIndex = setIndex;

//                //                
//                oCBConfigMDL.oChurchBody_MDL = new ChurchBody_MDL();
//                oCBConfigMDL.oChurchBody_MDL.lsChurchBody_MDL = GetChurchBodies(oParentId, null, false);
//                TempData.Keep();
//                return View(oCBConfigMDL);
//            }
//        }

//        public JsonResult GetAppGlobalOwn(bool addEmpty = false)
//        {
//            List<AppGlobalOwner> oCategList = new List<AppGlobalOwner>();

//            oCategList = _context.AppGlobalOwner.Where(c => c.Status == "A")
//                                            .OrderBy(c => c.OwnerName).ToList();
//            if (addEmpty) oCategList.Insert(0, new AppGlobalOwner { Id = 0, OwnerName = "Select" });
//            return Json(new SelectList(oCategList, "Id", "OwnerName"));
             
//        }

//        //private List<ChurchFaithType_MDL> GetChurchFaithTypes(int setIndex )
//        //{  //FS == Faith Sect like Catholism, Protestantism, Pentecostalism/Charismatism, FC == Faith Class like Presbyterian, Methodist, Catholic, Charismatic
//        //    var ls=  (
//        //           from t_cft in _context.ChurchFaithType.Include(t => t.FaithTypeClass).Include(t => t.SubFaithTypes)
//        //                .Where(c=> c.Category==(setIndex == 1 ? "FS" : "FC"))
//        //           from t_cft_c in _context.ChurchFaithType.AsNoTracking().Where(c=> c.Id == t_cft.FaithTypeClassId).DefaultIfEmpty()
//        //           select new ChurchFaithType_MDL()
//        //           {
//        //               oChurchFaithType = t_cft, 
//        //               strFaithTypeClass = t_cft_c != null ? t_cft_c.FaithDescription : ""
//        //           })
//        //           .OrderBy(c=>c.oChurchFaithType.FaithTypeClassId).ThenBy(c => c.oChurchFaithType.FaithDescription)
//        //           .ToList();
//        //    return ls;
//        //}

//        //private List<AppSubscription_MDL> GetAppSubscriptions()
//        //{
//        //    return _context.AppSubscription.OrderByDescending(c => c.SubscriptionDate).ToList();
//        //}

//        private List<UserProfile_MDL> GetUserProfiles(int? oChurchBodyId) // null CB means ... SUPER USER .. get all accounts at toplevel
//        {
//            if (oChurchBodyId==null)  //Administrative account
//            {
//                return (
//                   from t_up in _masterContext.UserProfile.AsNoTracking()//.Include(t => t.ChurchMember).Where(c=> c.ChurchBodyId==oChurchBodyId)
//                   from t_cb in _masterContext.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchBodyId)
//                 //  from t_cm in _clientContext.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()
//                   from t_upr in _masterContext.UserProfileRole.AsNoTracking().Include(t => t.UserRoles).Where(c => c.ChurchBodyId == oChurchBodyId && c.UserProfileId == t_up.Id)//.DefaultIfEmpty()
//                   from t_ur in _masterContext.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == oChurchBodyId && c.Id == t_upr.UserRoleId && (c.RoleType == "CH_ADMN" || c.RoleType == "CF_ADMN"))  //.DefaultIfEmpty()
//                   from t_urp in _masterContext.UserRolePermission.AsNoTracking().Include(t => t.UserPermissions).Where(c => c.ChurchBodyId == oChurchBodyId && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
//                   from t_upg in _masterContext.UserProfileGroup.AsNoTracking().Include(t => t.UserGroups).Where(c => c.ChurchBodyId == oChurchBodyId && c.UserProfileId == t_up.Id).DefaultIfEmpty()
//                   from t_ugp in _masterContext.UserGroupPermission.AsNoTracking().Include(t => t.UserPermissions).Where(c => c.ChurchBodyId == oChurchBodyId && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()
//                   select new UserProfile_MDL()
//                   {
//                       oUserProfile = t_up,
//                       lsUserGroups = t_upg.UserGroups,
//                       lsUserRoles = t_upr.UserRoles,
//                       lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),
//                       strCongregation = t_cb != null ? t_cb.Name : "",
//                       strDenomination = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
//                       strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
//                        strUserProfile = t_up.UserDesc // ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim()
//                   }
//                   ).OrderBy(c => c.oUserProfile.ChurchBodyId).ThenBy(c => c.strUserProfile)
//                   .ToList();
//            }
//            else   //other accounts... custom created
//            {
//                return (
//                   from t_up in _masterContext.UserProfile.AsNoTracking()//.Include(t => t.ChurchMember).Where(c=> c.ChurchBodyId==oChurchBodyId)
//                   from t_cb in _masterContext.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchBodyId)
//                 //  from t_cm in _clientContext.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId)
//                   from t_upr in _masterContext.UserProfileRole.AsNoTracking().Include(t => t.UserRoles).Where(c => c.ChurchBodyId == oChurchBodyId && c.UserProfileId == t_up.Id).DefaultIfEmpty()
//                   from t_ur in _masterContext.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == oChurchBodyId && c.Id == t_upr.UserRoleId && (c.RoleType == "CH_ADMN" || c.RoleType == "CF_ADMN"))  //.DefaultIfEmpty()
//                   from t_urp in _masterContext.UserRolePermission.AsNoTracking().Include(t => t.UserPermissions).Where(c => c.ChurchBodyId == oChurchBodyId && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
//                   from t_upg in _masterContext.UserProfileGroup.AsNoTracking().Include(t => t.UserGroups).Where(c => c.ChurchBodyId == oChurchBodyId && c.UserProfileId == t_up.Id).DefaultIfEmpty()
//                   from t_ugp in _masterContext.UserGroupPermission.AsNoTracking().Include(t => t.UserPermissions).Where(c => c.ChurchBodyId == oChurchBodyId && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()
//                   select new UserProfile_MDL()
//                   {
//                       oUserProfile = t_up,
//                       lsUserGroups = t_upg.UserGroups,
//                       lsUserRoles = t_upr.UserRoles,
//                       lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),
//                       strCongregation = t_cb != null ? t_cb.Name : "",
//                       strDenomination = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
//                       strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
//                       strUserProfile = t_up.UserDesc // ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim()
//                   }
//                   ).OrderBy(c => c.oUserProfile.ChurchBodyId).ThenBy(c => c.strUserProfile)
//                   .ToList();
//            }
             
//        }

//        private List<UserPermission> CombineCollection(List<UserPermission> list1, List<UserPermission> list2, 
//            List<UserPermission> list3 =null, List<UserPermission> list4 = null, List<UserPermission> list5 = null)
//        {
//            list1.ToList().AddRange(list2.ToList());
//            if (list3 != null) list1.ToList().AddRange(list3.ToList());
//            if (list4 != null) list1.ToList().AddRange(list4.ToList());
//            if (list5 != null) list1.ToList().AddRange(list5.ToList());
//            //
//            return list1.ToList();
//        }


//        public IActionResult AddOrEdit_CNFG(int? oCurrChuBodyId, int id = 0, int? oParentId = null, int setIndex = 0)
//        {
//            SetUserLogged();
//            if (!isCurrValid) //prompt!
//                return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                switch (setIndex)
//                {
//                    default: return View();
//                    case 1: //FS
//                        var oMdl1 = AddOrEdit_CFT(oCurrChuBodyId, id, setIndex);
//                        if (oMdl1 != null) return PartialView("_AddOrEdit_CFT", oMdl1); // break;
//                        else
//                        {  return Index(null, setIndex, null); }  // View(oMdl1);

//                    case 2:  //FC
//                        var oMdl2 = AddOrEdit_CFT(oCurrChuBodyId, id, setIndex);
//                        if (oMdl2 != null) return PartialView("_AddOrEdit_CFT", oMdl2); // break;
//                        else return View(oMdl2);

//                    case 3:
//                        var oMdl3 = AddOrEdit_AGO(oCurrChuBodyId, id, setIndex);
//                        if (oMdl3 != null) return PartialView("_AddOrEdit_AGO", oMdl3); // break;
//                        else return View(oMdl3);

//                    case 4:
//                        var oMdl4 = AddOrEdit_CL(oCurrChuBodyId, oParentId, id, setIndex);
//                        if (oMdl4 != null) return PartialView("_AddOrEdit_CL", oMdl4); // break;
//                        else return View(oMdl4);                

//                    case 5:
//                        var oMdl5 = AddOrEdit_CB(oCurrChuBodyId, oParentId, id, setIndex);
//                        if (oMdl5 != null) return PartialView("_AddOrEdit_CB", oMdl5); // break;
//                        else return View(oMdl5);

//                        //subscriptions
//                    //case 6:
//                    //    var oMdl6 = AddOrEdit_SCRB(oCurrChuBodyId, id, setIndex);
//                    //    if (oMdl6 != null) return PartialView("_AddOrEdit_SCRB", oMdl6); // break;
//                    //    else return View(oMdl6);

//                    case 7:
//                        var oMdl7 = AddOrEdit_UPR(oCurrChuBodyId, id, setIndex);
//                        if (oMdl7 != null) return PartialView("_AddOrEdit_UP", oMdl7); // break;
//                        else return View(oMdl7);

//                    case 10:
//                        var oMdl10 = AddOrEdit_CTRY(oCurrChuBodyId, id, setIndex);
//                        if (oMdl10 != null) return PartialView("_AddOrEdit_CTRY", oMdl10); // break;
//                        else return View(oMdl10);

//                    case 11:
//                        var oMdl11 = AddOrEdit_RGN(oCurrChuBodyId, id, setIndex);
//                        if (oMdl11 != null) return PartialView("_AddOrEdit_RGN", oMdl11); // break;
//                        else return View(oMdl11);
//                }
//            }             
//        }

//        public ChurchFaithType_MDL AddOrEdit_CFT(int? oCurrChuBodyId, int id = 0, int setIndex = 0)  
//        {                   
//             if (setIndex==0) return null;  //oCFT_MDL; oCFT_MDL.setIndex = setIndex;                
//                //
//                var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                if (oCurrChuBodyLogOn == null)   return null;   //prompt!
//            if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//                else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//                // check permission for Core life...
//                if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
//                return null;

//            //int? oCurrChuMemberId_LogOn = null;
//            //ChurchMember oCurrChuMember_LogOn = null;
//            // if (oUserProfile == null) return null;
//            //if (oUserProfile.ChurchMember == null)  return null;

//            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

//            var oCFT_MDL = new ChurchFaithType_MDL();
//            if (id == 0)
//                {   
//                    //create user and init... 
//                    oCFT_MDL.oChurchFaithType = new ChurchFaithType();
//                    oCFT_MDL.oChurchFaithType.Level = 1;
//                    if (setIndex > 0) oCFT_MDL.oChurchFaithType.Category = setIndex == 1 ? "FS" : "FC";
//                }

//                else
//                {
//                    oCFT_MDL = (
//                         from t_cft in _masterContext.ChurchFaithType.AsNoTracking().Include(t => t.FaithTypeClass)
//                             .Where(x => x.Id == id)
//                         select new ChurchFaithType_MDL()
//                         {
//                             oChurchFaithType = t_cft,
//                             strFaithTypeClass = t_cft.FaithTypeClass == null ? t_cft.FaithTypeClass.FaithDescription : ""
//                         }
//                        ).FirstOrDefault();                     
//                }

//                if (oCFT_MDL.oChurchFaithType == null) return null; 

//                oCFT_MDL.setIndex = setIndex;  
//                oCFT_MDL.oCurrAppGlobalOwner = oCurrChuBodyLogOn.AppGlobalOwner;
//                oCFT_MDL.oChurchBody = oCurrChuBodyLogOn;
//              //  oCFT_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
//              //  oCFT_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;

//                if (oCFT_MDL.setIndex == 2) // Denomination classes av church sects
//                    oCFT_MDL = this.popLookups_CFT(oCFT_MDL, oCFT_MDL.oChurchFaithType);
                 
//                TempData["oVmCurr"] = oCFT_MDL;
//                TempData.Keep();

//                return oCFT_MDL;
             
//        }  
        
//        public ChurchFaithType_MDL popLookups_CFT(ChurchFaithType_MDL vm, ChurchFaithType oCurrCFT)
//        {
//            if (vm != null)
//            { 
//                vm.lkpFaithTypeClasses = _masterContext.ChurchFaithType.Where(c => c.Id != oCurrCFT.Id && c.Category=="FS" && !string.IsNullOrEmpty(c.FaithDescription))
//                                              .OrderBy(c => c.FaithDescription).ToList()
//                                              .Select(c => new SelectListItem()
//                                              {
//                                                  Value = c.Id.ToString(),
//                                                  Text = c.FaithDescription
//                                              }) 
//                                              .ToList();

//                vm.lkpFaithTypeClasses.Insert(0, new SelectListItem { Value = "", Text = "Select" }); 
//            }

//            return vm;
//        }
        
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_CFT(ChurchFaithType_MDL vmMod)
//        {
//            ChurchFaithType oCFTChanges = vmMod.oChurchFaithType;
//           // vmMod = TempData.Get<ChurchFaithType_MDL>("oVmCurr"); TempData.Keep();

//            var tempPrivList = TempData["oVmCurr"] as string;
//            if (string.IsNullOrEmpty(tempPrivList)) RedirectToAction("LoginUserAcc", "UserLogin");
//            // De serialize the string to object
//            vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<ChurchFaithType_MDL>(tempPrivList); TempData.Keep();

//            var oCFT = vmMod.oChurchFaithType; // new ChurchFaithType();
//            oCFT.FaithDescription = oCFTChanges.FaithDescription;
//            oCFT.FaithTypeClassId = oCFTChanges.FaithTypeClassId;
//            var hdrDesc = vmMod.setIndex == 1 ? "Church sect (Faith stream)" : "Denomination class";

//            try
//            {                               
//                oCFT.Level = vmMod.oChurchFaithType.Level;
//                //
//                ModelState.Remove("oChurchFaithType.FaithTypeClassId");  //.
//                ModelState.Remove("oChurchFaithType.Level");

//                //finally check error state...
//                if (ModelState.IsValid==false)
//                    return Json(new { taskSuccess = false, oCurrId= oCFT.Id, userMess = "Failed to load the data to save. Please refresh and try again." });
                  
//                if (oCFT.Id == 0)
//                {
//                    if (oCFT.FaithTypeClassId != null)
//                    {
//                        var oCFTClass = _masterContext.ChurchFaithType.Find(oCFT.FaithTypeClassId);
//                        if (oCFTClass != null) oCFT.Level = oCFTClass.Level + 1;                        
//                    }
//                    var oCFTValid = _masterContext.ChurchFaithType.Where(c => c.FaithTypeClassId == oCFT.FaithTypeClassId && c.FaithDescription == oCFT.FaithDescription).FirstOrDefault();
//                    if (oCFTValid != null)
//                        return Json(new { taskSuccess = false, oCurrId = oCFT.Id, userMess = hdrDesc.ToLower() + " already exists." });
//                    //
//                    oCFT.Created = DateTime.Now;
//                    oCFT.LastMod = DateTime.Now;
//                    _context.Add(oCFT);

//                    ViewBag.UserMsg = "Saved " + hdrDesc.ToLower() + " successfully.";
//                }
//                else
//                {
//                    if (oCFT.FaithTypeClass != null) oCFT.Level = oCFT.FaithTypeClass.Level + 1;
//                    var oCFTValid = _masterContext.ChurchFaithType.Where(c => c.Id != oCFT.Id && c.FaithTypeClassId == oCFT.FaithTypeClassId && c.FaithDescription == oCFT.FaithDescription).FirstOrDefault();
//                    if (oCFTValid != null)
//                        return Json(new { taskSuccess = false, oCurrId = oCFT.Id, userMess = hdrDesc + " already exists." });
//                    //
//                    oCFT.LastMod = DateTime.Now; 
//                    _context.Update(oCFT);
//                    ViewBag.UserMsg = "Updated " + hdrDesc.ToLower() + " successfully.";
//                }

//                //save details... locAddr
//                try
//                {
//                    if (string.IsNullOrEmpty(oCFT.Category))
//                        oCFT.Category = vmMod.setIndex == 1 ? "FS" : "FC";
//                    await _context.SaveChangesAsync();
                     
//                    vmMod.oChurchFaithType = oCFT; 
//                    TempData["oVmCurr"] = vmMod;
//                    TempData.Keep();

//                    return Json(new { taskSuccess = true, oCurrId = oCFT.Id, userMess = ViewBag.UserMsg });
//                }

//                catch (Exception ex)
//                { 
//                    return Json(new { taskSuccess = false, oCurrId = oCFT.Id, userMess = "Failed saving " + hdrDesc.ToLower() + ". Err: " + ex.Message });
//                }
                 
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = oCFT.Id, userMess = "Failed saving " + hdrDesc.ToLower() + ". Err: " + ex.Message });
//            }
//        }
        
//        // GET: ChurchBody/Delete/5 
//        //public async Task<IActionResult> Delete_CFT(int? id, ChurchFaithType_MDL oCurrMdl = null)   //int? oCurrChuBodyId, 
//        public IActionResult Delete_CFT(int id, int setIndex, bool delConfirmed=false) //ChurchFaithType_MDL oCurrMdl = null)
//        {
//            var hdrDesc = "church configuration";
//            //if (oCurrMdl != null)  hdrDesc = oCurrMdl.setIndex == 1 ? "Church sect (Faith stream)" : "Denomination class";
//            hdrDesc = setIndex == 1 ? "Church sect (Faith stream)" : "Denomination class";

//            try
//            { 
//                /*var oCFT = await _context.ChurchFaithType.Include(c => c.SubFaithTypes).Where(c => c.Id == id).FirstOrDefaultAsync();*/  //_context.ChurchFaithType.FindAsync(id);
//                var oCFT = _masterContext.ChurchFaithType.Include(c => c.SubFaithTypes).Where(c => c.Id == id).FirstOrDefault();
//                if (oCFT != null)
//                {
//                    var saveDelete = true;
//                    //ensuring cascade delete where there's none!
//                    if (oCFT.SubFaithTypes.Count > 0)
//                    {
//                        if (delConfirmed)
//                        {
//                            foreach (var child in oCFT.SubFaithTypes.ToList())
//                            _masterContext.ChurchFaithType.Remove(child);
//                        }
//                        else
//                        {
//                            saveDelete = false;
//                            return Json(new { taskSuccess = false, oCurrId = id, userMess = "Specified " + 
//                                (setIndex == 1 ? "church sect (faith stream) has some denomination classes configured under its domain. Delete cannot be done unless child classes are deleted first" : 
//                                "denomination class failed to delete.") });
//                        }
//                    }
                  
//                    if (saveDelete)
//                    {
//                        _masterContext.ChurchFaithType.Remove(oCFT);
//                        // await _context.SaveChangesAsync();
//                        _context.SaveChanges();

//                        //ViewBag.promptUser = true;
//                        //ViewBag.promptUserMsg = hdrDesc + " successfully deleted.";
//                        //return Index(null, currSetIndex);   //reload

//                        return Json(new { taskSuccess = true, oCurrId = oCFT.Id, userMess = hdrDesc.ToLower() + " successfully deleted." });
//                    }
                    

//                    //  return Json(new { taskSuccess = true, userMess = "Church " + hdrDesc.ToLower() + " successfully deleted." });               
//                }

//                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Failed deleting " + hdrDesc.ToLower() });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Failed deleting " + hdrDesc.ToLower() + ". Err: " + ex.Message });
//            }
            

//            //var vmMod = TempData.Get<ChurchBodyConfigMDL>("oVmCB_CNFG"); TempData.Keep();
//            //ModelState.AddModelError(string.Empty, "Failed deleting " + hdrDesc.ToLower() + ".");
//            //return Index(null, currSetIndex, null, vmMod);

//            //if (oCurrMdl != null)
//            //    return View(oCurrMdl);  //return PartialView("_AddOrEdit_CFT", oCurrMdl);
//            //else
//            //    return Index(null, oCurrMdl.setIndex);

//            //return RedirectToAction(nameof(Index));
//            // return Json(new { taskSuccess = false, userMess = "Failed deleting church faith type."});
//        }

//        public AppGlobalOwner_MDL AddOrEdit_AGO(int? oCurrChuBodyId, int id = 0, int setIndex = 0)
//        {
//            var oAGO_MDL = new AppGlobalOwner_MDL(); TempData.Keep();
//            if (setIndex == 0) return oAGO_MDL;
//            oAGO_MDL.setIndex = setIndex;
//            //
//            var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//            var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//            if (oCurrChuBodyLogOn == null  ) return oAGO_MDL;

//            if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//            else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//            // check permission for Core life...
//            if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
//                return oAGO_MDL;

//            //int? oCurrChuMemberId_LogOn = null;
//            //ChurchMember oCurrChuMember_LogOn = null;
//            //if (oUserProfile == null) //prompt!
//            //    return oAGO_MDL;
//            //if (oUserProfile.ChurchMember == null) //prompt!
//            //    return oAGO_MDL;

//            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

//            if (id == 0)
//            {
//                //create user and init... 
//                oAGO_MDL.oAppGlobalOwn = new AppGlobalOwner();
//                oAGO_MDL.oAppGlobalOwn.TotalLevels = 1;
//                oAGO_MDL.oAppGlobalOwn.Status = "A";
//            }

//            else
//            {
//                oAGO_MDL = (
//                     from t_ago in _context.AppGlobalOwner.AsNoTracking().Include(t => t.Country).Include(t => t.ChurchLevels)//.Include(t => t.FaithTypeCategory)
//                        .Where(x => x.Id == id)
//                    // from t_ft in  _context.ChurchFaithType.AsNoTracking().Where(c=> c.Id==t_ago.FaithTypeCategoryId).DefaultIfEmpty()  //c.Category=="FC" &&                     
//                     select new AppGlobalOwner_MDL()
//                     {
//                         oAppGlobalOwn = t_ago,
//                         lsChurchLevels = t_ago.ChurchLevels,
//                         strFaithTypeCategory = t_ago.strFaithTypeCategory, // t_ft != null ? t_ft.FaithDescription : "",
//                         strCountry = t_ago.Country != null ? t_ago.Country.Name : "",
//                         strStatus = GetStatusDesc(t_ago.Status) //t_ago.Status == "A" ? "Active" : t_ago.Status == "D" ? "Deactive" : t_ago.Status == "E" ? "Expired" : "" //ACTV, DCTV, EXPR
//                     }
//                    ).FirstOrDefault();
//            }

//            if (oAGO_MDL.oAppGlobalOwn == null) return oAGO_MDL;
//            //
//            //set current stuff
//          //  oAGO_MDL.oCurrAppGlobalOwner = oCurrChuBodyLogOn.AppGlobalOwner;
//      //     oAGO_MDL.oCurrChurchBody = oCurrChuBodyLogOn;
//            //oAGO_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
//            //oAGO_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;
//            //
//            //get the lokupsfor this view
//            oAGO_MDL = this.popLookups_AGO(oAGO_MDL, oAGO_MDL.oAppGlobalOwn);

//            TempData["oVmCurr"] = oAGO_MDL;
//            TempData.Keep();

//            return oAGO_MDL;

//        }
//        public AppGlobalOwner_MDL popLookups_AGO(AppGlobalOwner_MDL vm, AppGlobalOwner oCurrAGO)
//        {
//            if (vm != null)
//            {
//                vm.lkpStatuses = new List<SelectListItem>();
//                foreach (var dl in dlStatus) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//                //vm.lkpFaithTypeCategories = _masterContext.ChurchFaithType .Where(c => c.Category=="FC")
//                //                              .OrderBy(c => c.FaithDescription).ToList()
//                //                              .Select(c => new SelectListItem()
//                //                              {
//                //                                  Value = c.Id.ToString(),
//                //                                  Text = c.FaithDescription
//                //                              })
//                //                              .ToList();

//                vm.lkpFaithTypeCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//                //
//                vm.lkpCountries = _context.Country //.Where(c => c.Display == true)
//                                .ToList()
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.Name
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//                vm.lkpCountries.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            }

//            return vm;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_AGO(AppGlobalOwner_MDL vmMod)
//        {
//            var _vmMod = vmMod;
//            var oAGO = vmMod.oAppGlobalOwn;  

//            try
//            { 
//              //  vmMod = TempData.Get<AppGlobalOwner_MDL>("oVmCurr"); TempData.Keep();

//                var tempVm = TempData["oVmCurr"] as string;
//                if (string.IsNullOrEmpty(tempVm)) RedirectToAction("LoginUserAcc", "UserLogin");
//                // De serialize the string to object
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<AppGlobalOwner_MDL>(tempVm); TempData.Keep();

//                ModelState.Remove("oAppGlobalOwner.FaithTypeCategoryId");
//                ModelState.Remove("oAppGlobalOwner.CountryId");
//                ModelState.Remove("oAppGlobalOwn.Status"); 

//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = oAGO.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

//                if (oAGO.Id > 0)
//                {
//                    var oAGOValid = _context.AppGlobalOwner.Where(c => c.Id != oAGO.Id && c.OwnerName == oAGO.OwnerName).FirstOrDefault();
//                    if (oAGOValid != null)
//                        return Json(new { taskSuccess = false, oCurrId = oAGO.Id, userMess = "Denomination already exists." });
//                }

//                string strFilename = null;
//                if (_vmMod.ChurchLogoFile != null && _vmMod.ChurchLogoFile.Length > 0)
//                {
//                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
//                    strFilename = Guid.NewGuid().ToString() + "_" + _vmMod.ChurchLogoFile.FileName;
//                    string filePath = Path.Combine(uploadFolder, strFilename);
//                    _vmMod.ChurchLogoFile.CopyTo(new FileStream(filePath, FileMode.Create));
//                }
//                else
//                {
//                    if (_vmMod.oAppGlobalOwn.Id != 0)
//                        strFilename = _vmMod.strChurchLogo;
//                }

//                oAGO.ChurchLogo = strFilename;
//                oAGO.Created = DateTime.Now;
//                if (oAGO.Id == 0)
//                {
//                    var oAGOValid = _context.AppGlobalOwner.Where(c => c.OwnerName == oAGO.OwnerName) .FirstOrDefault();
//                    if (oAGOValid != null)
//                        return Json(new { taskSuccess = false, oCurrId = oAGO.Id, userMess = "Denomination already exists." });
//                    // 
                   
//                    oAGO.LastMod = DateTime.Now;
//                    _context.Add(oAGO);
//                    ViewBag.UserMsg = "Saved denomination successfully. ";
//                }
//                else
//                { 
//                    oAGO.LastMod = DateTime.Now;
//                    _context.Update(oAGO);
//                    ViewBag.UserMsg = "Updated denomination successfully. ";
//                }

//                //save details... locAddr
//                try
//                { 
//                    await _context.SaveChangesAsync();

//                    vmMod.oAppGlobalOwn = oAGO;
//                    TempData["oVmCurr"] = vmMod; //TempData.Put("oVmCurr", vmMod);
//                    TempData.Keep();

//                    //auto-update the church levels
//                    var oChLevelCnt = 0;
//                    if (_vmMod.oAppGlobalOwn.Id==0)
//                    {
//                        for (int i = 1; i <= oAGO.TotalLevels; i++)
//                        {
//                            ChurchLevel oCL = new ChurchLevel();
//                            oCL.Name = "Level_" + i;
//                            oCL.CustomName = "Level " + i;
//                            oCL.LevelIndex = i;
//                            oCL.AppGlobalOwnerId = oAGO.Id;
//                            oCL.Created = DateTime.Now;
//                            oCL.LastMod = DateTime.Now;

//                            oChLevelCnt++;
//                            _context.Add(oCL);
//                        }

//                        if (oChLevelCnt > 0) ViewBag.UserMsg += Environment.NewLine + Environment.NewLine + "Created " + oAGO.TotalLevels + " church levels. Customisation may be necessary";
//                    }
//                    else
//                    {
//                        for (int i = 1; i <= oAGO.TotalLevels; i++)
//                        {
//                            var oExistCL = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == oAGO.Id && c.Name == "Level_" + i).FirstOrDefault();
//                            if (oExistCL==null && oChLevelCnt < oAGO.TotalLevels)
//                            {
//                                ChurchLevel oCL = new ChurchLevel();
//                                oCL.Name = "Level_" + i;
//                                oCL.CustomName = "Level " + i;
//                                oCL.LevelIndex = i;
//                                oCL.AppGlobalOwnerId = oAGO.Id;
//                                oCL.Created = DateTime.Now;
//                                oCL.LastMod = DateTime.Now;
//                                //
//                                oChLevelCnt++;
//                                _context.Add(oCL);
//                            } 
//                            else
//                            {
//                                oExistCL.Name = "Level_" + i;
//                                oExistCL.CustomName = "Level " + i;
//                                oExistCL.LevelIndex = i;
//                                oExistCL.AppGlobalOwnerId = oAGO.Id;
//                                oExistCL.LastMod = DateTime.Now;
//                                //
//                                oChLevelCnt++;
//                                _context.Update(oExistCL);
//                            }
//                        }

//                        if (oChLevelCnt > 0) ViewBag.UserMsg +=Environment.NewLine + Environment.NewLine + "Denomination's " + oAGO.TotalLevels + " church levels updated. Customisation may be necessary.";
//                    }
                    
//                    await _context.SaveChangesAsync();                     
//                    return Json(new { taskSuccess = true, oCurrId = oAGO.Id, userMess = ViewBag.UserMsg });
//                }

//                catch (Exception ex)
//                {
//                    return Json(new { taskSuccess = false, oCurrId = oAGO.Id, userMess = "Failed saving denomination. Err: " + ex.Message });
//                }
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = oAGO.Id, userMess = "Failed saving denomination. Err: " + ex.Message });
//            }
//        }

//        public IActionResult Delete_AGO(int id, int setIndex, bool delConfirmed = false)  
//        { 
//            try
//            {
//                var oAGO = _context.AppGlobalOwner  //.Include(c => c.ChurchBodies)
//                    .Include(c => c.ChurchLevels)  //.Include(c => c.AppSubscriptions)
//                    //.Include(c => c.ChurchMembers)
//                    .Where(c => c.Id == id).FirstOrDefault();
//                if (oAGO != null)
//                {
//                    var saveDelete = true;
//                    //ensuring cascade delete where there's none!  CHeck the code below
//                    if (oAGO != null)  //(oAGO.ChurchBodies.Count + oAGO.ChurchLevels.Count + oAGO.AppSubscriptions.Count + oAGO.ChurchMembers.Count > 0)
//                    {
//                        if (delConfirmed)
//                        {
//                            try
//                            {
//                                //foreach (var child in oAGO.ChurchBodies.ToList())
//                                //    _context.ChurchBody.Remove(child);
//                                //foreach (var child in oAGO.ChurchLevels.ToList())
//                                //    _context.ChurchLevel.Remove(child);
//                                //foreach (var child in oAGO.AppSubscriptions.ToList())
//                                //    _context.AppSubscription.Remove(child);
//                                //foreach (var child in oAGO.ChurchMembers.ToList())
//                                //    _clientContext .ChurchMember.Remove(child);
//                            }
//                            catch (Exception ex)
//                            {
//                                saveDelete = false;
//                                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Error occured while deleting specified denomination: " + ex.Message + ". Reload and try to delete again." });
                                 
//                            }                            
//                        } 
//                        else
//                        {
//                            saveDelete = false;
//                            return Json(new { taskSuccess = false, oCurrId = id, userMess = "Specified denomination has connections with other external data. Delete cannot be done unless child refeneces are removed." });
                             
//                        }
//                    }

//                    if (saveDelete)
//                    {
//                        _context.AppGlobalOwner.Remove(oAGO);  
//                        _context.SaveChanges(); 

//                        return Json(new { taskSuccess = true, oCurrId = oAGO.Id, userMess = "Denomination successfully deleted." });
//                    }               
//                }

//                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Failed deleting denomination" });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Failed deleting denomination" + ". Err: " + ex.Message });
//            } 
//        }


//        public ChurchLevel_MDL AddOrEdit_CL(int? oCurrChuBodyId, int? oAppOwnId=null, int id = 0, int setIndex = 0)
//        {
//            var oCL_MDL = new ChurchLevel_MDL(); TempData.Keep();
//            if (setIndex == 0) return oCL_MDL;
//            oCL_MDL.setIndex = setIndex;
//            //
//            var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//            var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//            if (oAppOwnId == null || oCurrChuBodyLogOn == null) return oCL_MDL;// && 

//            if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//            else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//            // check permission for Core life...
//            if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
//                return oCL_MDL;

//            //int? oCurrChuMemberId_LogOn = null;
//            //ChurchMember oCurrChuMember_LogOn = null;
//            //if (oUserProfile == null) //prompt!
//            //    return oCL_MDL;
//            //if (oUserProfile.ChurchMember == null) //prompt!
//            //    return oCL_MDL;

//            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

//            if (id == 0)
//            {
//                //create user and init... 
//                oCL_MDL.oChurchLevel = new ChurchLevel();
//                var oCLIndex = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == oAppOwnId).Count() + 1;
//                var oAppOwn = _context.AppGlobalOwner.Find(oAppOwnId);
//                if (oAppOwn==null) return oCL_MDL;

//                oCL_MDL.oChurchLevel.Name = "Level_" + oCLIndex;
//                oCL_MDL.oChurchLevel.CustomName = "Level " + oCLIndex;
//                oCL_MDL.oChurchLevel.LevelIndex = oCLIndex;
//                oCL_MDL.oChurchLevel.AppGlobalOwnerId = (int)oAppOwnId;
//                oCL_MDL.strAppGlobalOwn = oAppOwn.OwnerName;

//                oCL_MDL.oChurchLevel.Created = DateTime.Now;
//                oCL_MDL.oChurchLevel.LastMod = DateTime.Now;
//            }

//            else
//            {
//                oCL_MDL = (
//                     from t_cl in _context.ChurchLevel.AsNoTracking()//.Include(t => t.AppGlobalOwner)
//                            .Where(x => x.Id == id)
//                     from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(c=>c.Id==t_cl.AppGlobalOwnerId).DefaultIfEmpty()                     
//                     select new ChurchLevel_MDL()
//                     {
//                         oChurchLevel = t_cl,
//                         strAppGlobalOwn = t_ago != null ? t_ago.OwnerName : ""                         
//                     }
//                    ).FirstOrDefault();
//            }

//            if (oCL_MDL == null) return oCL_MDL;
//            if (oCL_MDL.oChurchLevel == null) return oCL_MDL;
//            //
//            //oCL_MDL.oCurrAppGlobalOwner = oCurrChuBodyLogOn.AppGlobalOwner;
//            //oCL_MDL.oCurrChurchBody = oCurrChuBodyLogOn;
//            //oCL_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
//            //oCL_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;

//            //get lookups
//            oCL_MDL = this.popLookups_CL(oCL_MDL); //, oCL_MDL.oChurchLevel);

//            TempData["oVmCurr"] = oCL_MDL;
//            TempData.Keep();

//            return oCL_MDL;

//        }
//        public ChurchLevel_MDL popLookups_CL(ChurchLevel_MDL vm )
//        {
//            if (vm != null)
//            {
//                vm.lkpAppGlobalOwn = _context.AppGlobalOwner.Where(c => c.Status=="A")
//                                              .OrderBy(c => c.OwnerName).ToList()
//                                              .Select(c => new SelectListItem()
//                                              {
//                                                  Value = c.Id.ToString(),
//                                                  Text = c.OwnerName
//                                              })
//                                              .ToList();

//                vm.lkpAppGlobalOwn.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            }

//            return vm;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_CL(ChurchLevel_MDL vmMod)
//        {
//            try
//            {
//                var _oCL = vmMod.oChurchLevel; // new ChurchFaithType();
//               // vmMod = TempData.Get<ChurchLevel_MDL>("oVmCurr"); TempData.Keep();
//                var tempVm = TempData["oVmCurr"] as string;
//                if (string.IsNullOrEmpty(tempVm)) RedirectToAction("LoginUserAcc", "UserLogin");
//                // De serialize the string to object
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<ChurchLevel_MDL>(tempVm); TempData.Keep();

//                var oCL = vmMod.oChurchLevel;

//                ModelState.Remove("oChurchLevel.AppGlobalOwnerId"); 
//                ModelState.Remove("oChurchLevel.Name");
//                ModelState.Remove("oCurrChurchBody.Id");
//                ModelState.Remove("oCurrChurchBody.Name");
//                ModelState.Remove("oCurrChurchBody.ChurchType");
//                ModelState.Remove("oCurrChurchBody.AssociationType");


//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = oCL.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

//                oCL.LevelIndex = _oCL.LevelIndex;
//                oCL.CustomName = _oCL.CustomName;
//                if (oCL.Id == 0)
//                {
//                    var oCLVal = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId==oCL.AppGlobalOwnerId && c.CustomName == oCL.CustomName).FirstOrDefault();
//                    if (oCLVal != null) return Json(new { taskSuccess = false, oCurrId = oCL.Id, userMess = "Church level already exists." });
//                    //                    
//                    oCL.Created = DateTime.Now;
//                    oCL.LastMod = DateTime.Now;
//                    _context.Add(oCL);

//                    ViewBag.UserMsg = "Saved church level successfully.";
//                }
//                else
//                {
//                    var oCLVal = _context.ChurchLevel.Where(c => c.Id != oCL.Id && c.AppGlobalOwnerId == oCL.AppGlobalOwnerId && c.CustomName == oCL.CustomName).FirstOrDefault();
//                    if (oCLVal != null) return Json(new { taskSuccess = false, oCurrId = oCL.Id, userMess = "Church level already exists." });
//                    //
//                    oCL.LastMod = DateTime.Now;
//                    _context.Update (oCL);
//                    ViewBag.UserMsg = "Updated church level successfully.";
//                }

//                //save details... locAddr
//                try
//                {
//                    await _context.SaveChangesAsync();
                     
//                    vmMod.oChurchLevel = oCL;
//                    TempData["oVmCurr"] = vmMod;
//                    TempData.Keep();

//                    return Json(new { taskSuccess = true, oCurrId = oCL.Id, userMess = ViewBag.UserMsg });
//                }

//                catch (Exception ex)
//                {
//                    return Json(new { taskSuccess = false, oCurrId = oCL.Id, userMess = "Failed saving church level. Err: " + ex.Message });
//                }

//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, userMess = "Failed saving church level. Err: " + ex.Message });
//            }
//        }

//        // GET: ChurchBody/Delete/5 
//        public IActionResult Delete_CL(int id, int setIndex, bool delConfirmed = false)
//        {
//            try
//            {
//                var oCL = _context.ChurchLevel.Include(c => c.AppGlobalOwner) //.Include(c => c.TransferTypeChurchLevels)
//                    .Include(c => c.ChurchBodies)  //.Include(c => c.AppSubscriptions)
//                   // .Include(c => c.ApprovalProcesses)//.Include(c => c.AppGlobalOwns)
//                    .Where(c => c.Id == id).FirstOrDefault();
//                if (oCL != null)
//                {
//                    var saveDelete = true;
//                    //ensuring cascade delete where there's none!
//                    if (oCL != null)    //(oCL.ChurchBodies.Count + oCL.TransferTypeChurchLevels.Count + oCL.AppSubscriptions.Count  + oCL.ApprovalProcesses.Count > 0)
//                    {  //+ oCL.AppGlobalOwns.Count
//                        if (delConfirmed)
//                        {
//                            try
//                            { 
//                                foreach (var child in oCL. ChurchBodies.ToList())
//                                    _context.ChurchBody.Remove(child);
//                                //foreach (var child in oCL.AppSubscriptions.ToList())
//                                //    _context.AppSubscription.Remove(child);
//                                //foreach (var child in oCL.ApprovalProcesses.ToList())
//                                //    _context.ApprovalProcess.Remove(child);
//                                //foreach (var child in oCL.TransferTypeChurchLevels.ToList())
//                                //    _context.TransferTypeChurchLevel.Remove(child);

//                                //var list_AppGlobalOwns = _context.
//                                //foreach (var child in list_AppGlobalOwns )
//                                //    _context.AppGlobalOwner.Remove(child);
//                            }
//                            catch (Exception ex)
//                            {
//                                saveDelete = false;
//                                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Error occured while deleting specified church level: " + ex.Message + ". Reload and try to delete again." });

//                            }
//                        }
//                        else
//                        {
//                            saveDelete = false;
//                            return Json(new { taskSuccess = false, oCurrId = id, userMess = "Specified church level has connections with other external data. Delete cannot be done unless child refeneces are removed." });

//                        }
//                    } 

//                        if (saveDelete)
//                        {
//                            _context.ChurchLevel.Remove(oCL);
//                            _context.SaveChanges();

//                            return Json(new { taskSuccess = true, oCurrId = oCL.Id, userMess = "church level successfully deleted." });
//                        }
                                     
//                }

//                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Church level to delete not available" });
//            }
//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Failed deleting church level" + ". Err: " + ex.Message });
//            }
//        }
        

//        public Country_MDL AddOrEdit_CTRY(int? oCurrChuBodyId, int id = 0, int setIndex = 0)
//        {
//            var oCTRY_MDL = new Country_MDL(); TempData.Keep();
//            if (setIndex == 0) return oCTRY_MDL;
//            oCTRY_MDL.setIndex = setIndex;
//            //
//            var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//            var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//            if (oCurrChuBodyLogOn == null) //prompt!
//                return oCTRY_MDL;
//            if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//            else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//            // check permission for Core life...
//            if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
//                return oCTRY_MDL;

//            //int? oCurrChuMemberId_LogOn = null;
//            //ChurchMember oCurrChuMember_LogOn = null;
//            if (oUserProfile == null) //prompt!
//                return oCTRY_MDL;
//            //if (oUserProfile.ChurchMember == null) //prompt!
//            //    return oCTRY_MDL;

//            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

//            if (id == 0)
//            {
//                //create user and init... 
//                oCTRY_MDL.oCountry = new Country();
                
//            }

//            else
//            {
//                oCTRY_MDL = (
//                     from t_ctr in _context.Country.AsNoTracking().Include(t => t.CountryRegions) 
//                         .Where(x => x.Id == id)
//                     select new Country_MDL()
//                     {
//                         oCountry = t_ctr,
//                         lsCountryRegions = t_ctr.CountryRegions
//                     }
//                    ).FirstOrDefault();
//            }

//            if (oCTRY_MDL.oCountry == null)
//                return oCTRY_MDL;
//            //oCTRY_MDL.oCurrAppGlobalOwner = oCurrChuBodyLogOn.AppGlobalOwner;
//            //oCTRY_MDL.oCurrChurchBody = oCurrChuBodyLogOn;
//            //oCTRY_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
//            //oCTRY_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;
//            oCTRY_MDL = this.popLookups_CTRY(oCTRY_MDL);//, oCTRY_MDL.oCountry);

//            TempData["oVmCurr"] = oCTRY_MDL;
//            TempData.Keep();

//            return oCTRY_MDL;
//        }
        
//        public Country_MDL  popLookups_CTRY(Country_MDL vm) //, AppGlobalOwner oCurrCTRY)
//        {
//            if (vm != null)
//            {
//                //vm.lkpCountries = new List<SelectListItem>();
//                //foreach (var dl in dlStatus) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }
                 
//                //
//                vm.lkpCountries = _context.Country.ToList()  //.Where(c => c.Display == true)
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.Name
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//                vm.lkpCountries.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            }

//            return vm;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_CTRY(Country_MDL vmMod)
//        {
//            try
//            {
//                var _vmMod = vmMod;
//                var oCTRY = vmMod.oCountry; // new oCountry();
//              //  vmMod = TempData.Get<Country_MDL>("oVmCurr"); TempData.Keep();
//                var tempVm = TempData["oVmCurr"] as string;
//                if (string.IsNullOrEmpty(tempVm)) RedirectToAction("LoginUserAcc", "UserLogin");
//                // De serialize the string to object
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<Country_MDL>(tempVm); TempData.Keep();


//                //ModelState.Remove("oCTRY.FaithTypeCategoryId");
//                //ModelState.Remove("oCTRY.CountryId");

//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, userMess = "Failed to load the data to save. Please refresh and try again." });
                 
                 
//                oCTRY.Created = DateTime.Now;
//                if (oCTRY.Id == 0)
//                {
//                    var oCTRYValid = _context.Country.Where(c => c.Name == oCTRY.Name).FirstOrDefault();
//                    if (oCTRYValid != null)
//                        return Json(new { taskSuccess = false, userMess = "Country already exists." });
//                    // 

//                    oCTRY.LastMod = DateTime.Now;
//                    _context.Add(oCTRY);
//                    ViewBag.UserMsg = "Saved country successfully.";
//                }
//                else
//                {
//                    var oCTRYValid = _context.Country.Where(c => c.Id != oCTRY.Id && c.Name == oCTRY.Name).FirstOrDefault();
//                    if (oCTRYValid != null)
//                        return Json(new { taskSuccess = false, userMess = "Country already exists." });

//                    oCTRY.LastMod = DateTime.Now;
//                    _context.Update(oCTRY);
//                    ViewBag.UserMsg = "Updated country successfully.";
//                }

//                //save details... locAddr
//                try
//                { 
//                    await _context.SaveChangesAsync();

//                    vmMod.oCountry = oCTRY;
//                    TempData["oVmCurr"] = vmMod;
//                    TempData.Keep();
                     
//                    return Json(new { taskSuccess = true, userMess = ViewBag.UserMsg });
//                }

//                catch (Exception ex)
//                {
//                    return Json(new { taskSuccess = false, userMess = "Failed saving country. Err: " + ex.Message });
//                }
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, userMess = "Failed saving country. Err: " + ex.Message });
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> ImportCountry()
//        {           
//            if (ModelState.IsValid)
//            {
//                //validations... unique name
//                var ctryList = AppUtilties.GetClientCountries();
//                var importList = ctryList
//                    .Where(c => _context.Country.Where(x => x.Name == c.Name).Count() == 0)
//                    .ToList();

//                int res = 0;
//                foreach (Country ctry in importList)
//                {
//                    ctry.Created = DateTime.Now;
//                    ctry.LastMod = DateTime.Now;
//                    _context.Add(ctry);
//                    res++;
//                }

//                await _context.SaveChangesAsync(); 
//                return Json(new { taskSuccess = true, userMess = res + (res > 1 ? " countries" : " country") + " imported successfully." });
//            }
//            else
//                return Json(new { taskSuccess = false, userMess = "Error occured while saving country data. Reload and try again." });
//        }

//        // GET: ChurchBody/Delete/5 
//        public async Task<IActionResult> Delete_CTRY(int? id)   //int? oCurrChuBodyId, 
//        {
//            var oCTRY = await _context.Country.FindAsync(id);
//            if (oCTRY != null)
//            {
//                _context.Country.Remove(oCTRY);
//                await _context.SaveChangesAsync();

//                //return Json(new { taskSuccess = true, userMess = "Church faith type successfully deleted." });               
//            }
//            return RedirectToAction(nameof(Index));
//            //return Json(new { taskSuccess = false, userMess = "Failed deleting church faith type."});
//        }


//        public CountryRegion_MDL AddOrEdit_RGN(int? oCurrChuBodyId, int? oCurrCtryId = null, int id = 0, int setIndex = 0)
//        {
//            var oRGN_MDL = new CountryRegion_MDL(); TempData.Keep();
//            if (setIndex == 0) return oRGN_MDL;
//            oRGN_MDL.setIndex = setIndex;
//            //
//            var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//            var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//            if (oCurrChuBodyLogOn == null || oCurrCtryId == null) return oRGN_MDL;

//            if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//            else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//            // check permission for Core life...
//            if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
//                return oRGN_MDL;

//            //int? oCurrChuMemberId_LogOn = null;
//            //ChurchMember oCurrChuMember_LogOn = null;
//            if (oUserProfile == null) //prompt!
//                return oRGN_MDL;
//            //if (oUserProfile.ChurchMember == null) //prompt!
//            //    return oRGN_MDL;

//            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

//            if (id == 0)
//            {
//                //create user and init... 
//                oRGN_MDL.oCountryRegion = new CountryRegion(); 
                 
//                oRGN_MDL.oCountryRegion.CountryId = oCurrCtryId;
//                oRGN_MDL.oCountryRegion.Created = DateTime.Now;
//                oRGN_MDL.oCountryRegion.LastMod = DateTime.Now;
//            }

//            else
//            {
//                oRGN_MDL = (
//                     from t_cr in _context.CountryRegion.AsNoTracking().Include(t => t.Country)
//                         .Where(x => x.Id == id)
//                     select new CountryRegion_MDL()
//                     {
//                         oCountryRegion = t_cr,
//                         strCountry = t_cr.Country == null ? t_cr.Country.Name : ""
//                     }
//                    ).FirstOrDefault();
//            }

//            if (oRGN_MDL.oCountryRegion == null) return oRGN_MDL;
//            //oRGN_MDL.oCurrAppGlobalOwner = oCurrChuBodyLogOn.AppGlobalOwner;
//            //oRGN_MDL.oCurrChurchBody = oCurrChuBodyLogOn;
//            //oRGN_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
//            //oRGN_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;
//            oRGN_MDL = this.popLookups_RGN(oRGN_MDL); //, oRGN_MDL.oCountryRegion);

//            TempData["oVmCurr"] = oRGN_MDL;
//            TempData.Keep();

//            return oRGN_MDL;

//        }

//        public CountryRegion_MDL popLookups_RGN(CountryRegion_MDL vm)
//        {
//            if (vm != null)
//            {
//                vm.lkpCountries = _context.Country.ToList()  //.Where(c => c.Display == true)
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.Name
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//                vm.lkpCountries.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            }

//            return vm;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_RGN(CountryRegion_MDL vmMod)
//        {
//            try
//            {
//                var oRGN = vmMod.oCountryRegion; // new ChurchFaithType();
//               // vmMod = TempData.Get<CountryRegion_MDL>("oVmCurr"); TempData.Keep();
//                var tempVm = TempData["oVmCurr"] as string;
//                if (string.IsNullOrEmpty(tempVm)) RedirectToAction("LoginUserAcc", "UserLogin");
//                // De serialize the string to object
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<CountryRegion_MDL>(tempVm); TempData.Keep();

//                ModelState.Remove("oCountryRegion.CountryId");
//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, userMess = "Failed to load the data to save. Please refresh and try again." });

//                if (oRGN.Id == 0)
//                {
//                    var oRGNValid = _context.CountryRegion.Where(c => c.CountryId == oRGN.CountryId && c.Name == oRGN.Name).FirstOrDefault();
//                    if (oRGNValid != null)
//                        return Json(new { taskSuccess = false, userMess = "Country region already exists." });
//                    //
//                    oRGN.Created = DateTime.Now;
//                    oRGN.LastMod = DateTime.Now;
//                    _context.Add(oRGN);

//                    ViewBag.UserMsg = "Saved country region successfully.";
//                }
//                else
//                {
//                    var oRGNValid = _context.CountryRegion.Where(c => c.Id != oRGN.Id && c.CountryId == oRGN.CountryId && c.Name == oRGN.Name).FirstOrDefault();
//                    if (oRGNValid != null)
//                        return Json(new { taskSuccess = false, userMess = "Country region already exists." });
//                    //
//                    oRGN.LastMod = DateTime.Now;
//                    _context.Update(oRGN);
//                    ViewBag.UserMsg = "Updated country region successfully.";
//                }

//                //save details... locAddr
//                try
//                {
//                    await _context.SaveChangesAsync();

//                    vmMod.oCountryRegion = oRGN;
//                    TempData["oVmCurr"] = vmMod;
//                    TempData.Keep();

//                    return Json(new { taskSuccess = true, userMess = ViewBag.UserMsg });
//                }

//                catch (Exception ex)
//                {
//                    return Json(new { taskSuccess = false, userMess = "Failed saving country region. Err: " + ex.Message });
//                }

//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, userMess = "Failed saving country region. Err: " + ex.Message });
//            }
//        }

//        // GET: ChurchBody/Delete/5 
//        public async Task<IActionResult> Delete_RGN(int? id)   //int? oCurrChuBodyId, 
//        {
//            var oRGN = await _context.CountryRegion.FindAsync(id);
//            if (oRGN != null)
//            {
//                _context.CountryRegion.Remove(oRGN);
//                await _context.SaveChangesAsync();

//                //return Json(new { taskSuccess = true, userMess = "country region successfully deleted." });               
//            }
//            return RedirectToAction(nameof(Index));
//            //return Json(new { taskSuccess = false, userMess = "Failed deleting country region."});
//        }



//        public ChurchBody_MDL AddOrEdit_CB(int? oCurrChuBodyId, int? oAppOwnId = null, int id = 0, int setIndex = 0)
//        {
//            var oCB_MDL = new ChurchBody_MDL(); TempData.Keep();
//            if (setIndex == 0) return oCB_MDL;
//            oCB_MDL.setIndex = setIndex;
//            //
//            var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//            var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//            if (oCurrChuBodyLogOn == null && oAppOwnId == null) return oCB_MDL;

//            if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//            else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//            // check permission for Core life...
//            if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
//                return oCB_MDL;

//            //int? oCurrChuMemberId_LogOn = null;
//            //ChurchMember oCurrChuMember_LogOn = null;
//            if (oUserProfile == null) //prompt!
//                return oCB_MDL;
//            //if (oUserProfile.ChurchMember == null) //prompt!
//            //    return oCB_MDL;

//            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

//            if (id == 0)
//            { 
//                var oAppOwn = _context.AppGlobalOwner.Find(oAppOwnId);
//                if (oAppOwn == null) return oCB_MDL;

//                //create user and init... 
//                oCB_MDL.oChurchBody = new ChurchBody();
//                oCB_MDL.oChurchBody.AppGlobalOwnerId = oAppOwnId;


//                var currCnt = _context.ChurchBody.AsNoTracking()
//                    .Where(c => c.AppGlobalOwnerId == oAppOwnId && c.Status=="A") // (c.OrganisationType == "GB" || c.OrganisationType == "CN")  && (c.ChurchType=="CH" || c.ChurchType == "CF"))
//                    .Count() + 1;

//                //denom - 000 - 000 - 000 etc. 
//                string parCBCode = "";  // get the parent church body ... CB to be created first by Vendor... and picked up by the subscribers at the ChurchStructure ... congregation
//                var strCBFullCode = !string.IsNullOrEmpty(parCBCode) ? parCBCode.ToUpper() + "-": "" + currCnt.ToString();
//               // var strLocalChuCode = (!string.IsNullOrEmpty(oAppOwn.Acronym) ? oAppOwn.Acronym.ToUpper() : "") + strCBCode;  //add preceding zero's ... 

//                // denom + ***  ... permanent  + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString()
//                var strGloChuCode = (!string.IsNullOrEmpty(oAppOwn.Acronym) ? oAppOwn.Acronym.ToUpper() : "")  + currCnt.ToString();  //add preceding zero's

//                //oCB_MDL.oChurchBody.CountryId = oCurrCtryId;
//                oCB_MDL.oChurchBody.GlobalChurchCode = strGloChuCode;
//               // oCB_MDL.oChurchBody.ChurchCodeFullPath = strCBFullCode;
//                oCB_MDL.oChurchBody.Created = DateTime.Now;
//                oCB_MDL.oChurchBody.LastMod = DateTime.Now;
//                oCB_MDL.strAppGlobalOwn = oAppOwn.OwnerName;
//            }

//            else
//            {
//                oCB_MDL = (
//                     from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner)  //.Include(t => t.ParentChurchBody)
//                     .Include(t => t.ChurchLevel)
//                     .Include(t => t.Country).Include(t => t.SubChurchUnits)
//                        .Where(x=> x.AppGlobalOwnerId == oAppOwnId && x.Id==id) //from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(x=> x.Id == t_cb.AppGlobalOwnerId).DefaultIfEmpty()
//                     from t_cl in _context.ChurchLevel.AsNoTracking().Where(x=> x.Id == t_cb.ChurchLevelId).DefaultIfEmpty()
//                     from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(x => x.Id == t_cb.AppGlobalOwnerId).DefaultIfEmpty()
//                     from t_cb_c in _context.ChurchBody.AsNoTracking().Where(x=> x.ParentChurchBodyId == t_cb.Id).DefaultIfEmpty()
//                     select new ChurchBody_MDL()
//                     {
//                         oChurchBody = t_cb,
//                         lsSubChurchBodies = t_cb.SubChurchUnits.ToList(),
//                         strParentChurchBody = t_cb_c != null ? t_cb_c.Name : "",
//                         strAppGlobalOwn = t_ago != null ? t_ago.OwnerName : "",
//                         strChurchLevel = t_cl != null ? !string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name : "",
//                         strCountry = t_cb.Country != null ? t_cb.Country.Name : "",
//                         strCountryRegion = t_cb_c != null ? t_cb_c.Name : "",
//                       //  strChurchType = t_cb.ChurchType == "CH" ? "Hierarchy" : "Congregation",
//                       //  strAssociationType = t_cb.ChurchType == "N" ? "Networked" : "Freelance"
//                     }
//                    ).FirstOrDefault();
//            }

//            if (oCB_MDL.oChurchBody == null) return oCB_MDL;
//            //
//          //  oCB_MDL.oCurrAppGlobalOwner = oCurrChuBodyLogOn.AppGlobalOwner;
//            //oCB_MDL.oCurrChurchBody = oCurrChuBodyLogOn;
//            //oCB_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
//            //oCB_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;
//            //
//            oCB_MDL = this.popLookups_CB(oCB_MDL, oCB_MDL.oChurchBody, oAppOwnId);

//            TempData["oVmCurr"] = oCB_MDL;
//            TempData.Keep();

//            return oCB_MDL;

//        }

//        public JsonResult GetCountryRegionsByCountry( int? ctryId, bool addEmpty = false)
//        {
//            var countryList = _context.CountryRegion.Include(t => t.Country)
//                .Where(c => c.CountryId == ctryId).OrderBy(c => c.Name).ToList()  //c.Country.Display == true && 
//            .Select(c => new SelectListItem()
//             {
//                 Value = c.Id.ToString(),
//                 Text = c.Name                 
//             })
//            .OrderBy(c => c.Text)
//            .ToList();

//            /// if (addEmpty) countryList.Insert(0, new CountryRegion { Id = "", Name = "Select" });             
//            //return Json(new SelectList(countryList, "Id", "Name"));  

//            if (addEmpty) countryList.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json(countryList);
//        }
         
//        public ChurchBody_MDL popLookups_CB(ChurchBody_MDL vm, ChurchBody oCurrChurchBody, int? oAppOwnId)
//        {
//            if (vm != null)
//            {
//                vm.lkpAppGlobalOwn = _context.AppGlobalOwner.Where(c => c.Status == "A")
//                                               .OrderBy(c => c.OwnerName).ToList()
//                                               .Select(c => new SelectListItem()
//                                               {
//                                                   Value = c.Id.ToString(),
//                                                   Text = c.OwnerName
//                                               })
//                                               .ToList();

//                vm.lkpAppGlobalOwn.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//                if (oCurrChurchBody != null && oAppOwnId != null)
//                {
//                vm.lkpAssociationTypes = new List<SelectListItem>();
//                foreach (var dl in dlAssocType) { vm.lkpAssociationTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//                vm.lkpChurchTypes = new List<SelectListItem>();
//                foreach (var dl in dlChurchType) { vm.lkpChurchTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }
                 
//                vm.lkpChurchCategories = _context.ChurchBody.Include(t=>t.ChurchLevel)
//                        .Where(c => c.Id != oCurrChurchBody.Id && //c.Id != oCurrChurchBody.ParentChurchBodyId &&
//                                         c.AppGlobalOwnerId == oAppOwnId ) // &&  c.ChurchType=="CH"  )
//                                              //  (c.ChurchLevel.LevelIndex == oCurrChurchBody.ChurchLevel.LevelIndex + 1 || c.ChurchLevel.LevelIndex == oCurrChurchBody.ChurchLevel.LevelIndex - 1))
//                                              .OrderBy(c => c.ChurchLevel.LevelIndex).ThenBy(c => c.Name).ToList()
//                                              .Select(c => new SelectListItem()
//                                              {
//                                                  Value = c.Id.ToString(),
//                                                  Text = c.Name
//                                              })
//                                              .ToList();

//                vm.lkpChurchCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//                    //
//                    vm.lkpCountries = _context.Country.ToList() //.Where(c => c.Display == true)
//                                               .Select(c => new SelectListItem()
//                                               {
//                                                   Value = c.Id.ToString(),
//                                                   Text = c.Name
//                                               })
//                                               .OrderBy(c => c.Text)
//                                               .ToList();
//                    vm.lkpCountries.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//                    if (oCurrChurchBody.Id <= 0)
//                    {
//                        var ctry = _context.Country.FirstOrDefault(); //.Where(c => c.Display == true && c.DefaultCtry)
//                        if (ctry != null)
//                            foreach (var c in vm.lkpCountries)
//                            {
//                                if (!string.IsNullOrEmpty(c.Value))
//                                    if (int.Parse(c.Value) == ctry.Id)
//                                        {c.Selected = true; break;}
//                                            }
//                    } 
                
//                //
//                vm.lkpCountryRegions = _context.CountryRegion.Include(t=>t.Country).ToList()  //.Where(c => c.Country.Display == true)
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.Name
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//                vm.lkpCountryRegions.Insert(0, new SelectListItem { Value = null, Text = "Select" });
//                //
//                vm.lkpChurchLevels = _context.ChurchLevel.Where(c =>  c.AppGlobalOwnerId == oAppOwnId)
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = !string.IsNullOrEmpty(c.CustomName) ? c.CustomName : c.Name
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//                vm.lkpChurchLevels.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            }
//          }

//            return vm;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_CB(ChurchBody_MDL vmMod)
//        {
//            var oCB = vmMod.oChurchBody; // new ChurchFaithType();
//          //  vmMod = TempData.Get<ChurchBody_MDL>("oVmCurr"); TempData.Keep();
//            var tempVm = TempData["oVmCurr"] as string;
//            if (string.IsNullOrEmpty(tempVm)) RedirectToAction("LoginUserAcc", "UserLogin");
//            // De serialize the string to object
//            vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<ChurchBody_MDL>(tempVm); TempData.Keep();

//            var _oCB = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oChurchBody.AppGlobalOwnerId");
//                ModelState.Remove("oChurchBody.CountryId");
//                ModelState.Remove("oChurchBody.CountryRegionId");
//                ModelState.Remove("oChurchBody.ParentChurchBodyId");
//                ModelState.Remove("oChurchBody.ContactInfoId");
//                ModelState.Remove("oChurchBody.ChurchLevelId");
//               // ModelState.Remove("oChurchBody.AssociationType");
//                ModelState.Remove("oChurchBody.ChurchType");
//                //
//                ModelState.Remove("oCurrChurchBody.Id");
//                ModelState.Remove("oCurrChurchBody.Name");
//                ModelState.Remove("oCurrChurchBody.ChurchType");
//              //  ModelState.Remove("oCurrChurchBody.AssociationType");

//                //finally check error state...
//                if (ModelState.IsValid == false) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Data submitted have errors. Please refresh and try again." });

//                //if (oCB.AssociationType=="N" && oCB.ParentChurchBodyId==null)
//                //    return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Networked congregation must have a church category. Hint: Change to 'Freelance'" });
                 
//                if (string.IsNullOrEmpty(oCB.GlobalChurchCode)) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Unique Church code-1 within denomination is required." });
//              //  if (string.IsNullOrEmpty(oCB.ChurchCodeFullPath)) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Unique Church code-2 within denomination is required." });

//                if (oCB.Id == 0)
//                {
//                    var oCBVal = _context.ChurchBody  //.Include(t=>t.ParentChurchBody)
//                        .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ParentChurchBodyId == oCB.ParentChurchBodyId && c.Name == oCB.Name).FirstOrDefault();
//                    if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Congregation " +
//                              oCBVal.Name + //(oCBVal.ParentChurchBody != null ? " of " + oCBVal.ParentChurchBody.Name : "") + 
//                              " already exists." });
                    
//                    oCBVal = _context.ChurchBody  //.Include(t => t.ParentChurchBody)
//                        .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && 
//                                (c.GlobalChurchCode == oCB.GlobalChurchCode //||  c.ChurchCodeFullPath == oCB.ChurchCodeFullPath || 
//                               // (oCB.ChurchCodeCustom != null && c.ChurchCodeCustom == oCB.ChurchCodeCustom)
//                               )).FirstOrDefault();
//                    if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Church codes must be unique." + Environment.NewLine +
//                              oCBVal.Name + //(oCBVal.ParentChurchBody != null ? " [of " + oCBVal.ParentChurchBody.Name : "]") + 
//                              " has same church code."
//                    });

//                    //
//                    oCB.Created = DateTime.Now;
//                    oCB.LastMod = DateTime.Now;
//                    _context.Add(oCB);

//                    ViewBag.UserMsg = "Saved congregation successfully.";
//                }

//                else
//                {
//                    var oCBVal = _context.ChurchBody  //.Include(t => t.ParentChurchBody)
//                        .Where(c => c.Id != oCB.Id && c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ParentChurchBodyId == oCB.ParentChurchBodyId && c.Name == oCB.Name).FirstOrDefault();
//                    if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Congregation " +
//                            oCBVal.Name + // (oCBVal.ParentChurchBody != null ? " of " + oCBVal.ParentChurchBody.Name : "") 
//                             " already exists." });

//                    // oCBVal = _context.ChurchBody.Include(t => t.ParentChurchBody).Where(c => c.Id != oCB.Id && c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ChurchCode == oCB.ChurchCode ).FirstOrDefault();
//                    //if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Church code must be unique." + Environment.NewLine + 
//                    //        oCBVal.Name + (oCBVal.ParentChurchBody != null ? " of " + oCBVal.ParentChurchBody.Name : "") + " has  same code."});
//                    ////
//                    oCBVal = _context.ChurchBody   //.Include(t => t.ParentChurchBody)
//                        .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.Id != oCB.Id &&
//                               (c.GlobalChurchCode == oCB.GlobalChurchCode //|| c.ChurchCodeFullPath == oCB.ChurchCodeFullPath ||
//                               // (oCB.ChurchCodeCustom != null && c.ChurchCodeCustom == oCB.ChurchCodeCustom)
//                               )).FirstOrDefault();
//                    if (oCBVal != null) return Json(new
//                    {
//                        taskSuccess = false,
//                        oCurrId = oCB.Id,
//                        userMess = "Church codes must be unique." + Environment.NewLine +
//                              oCBVal.Name + //(oCBVal.ParentChurchBody != null ? " [of " + oCBVal.ParentChurchBody.Name : "]") + 
//                              " has  same church code."
//                    });
//                    //
//                    oCB.LastMod = DateTime.Now;
//                    _context.Update(oCB);
//                        ViewBag.UserMsg = "Updated congregation successfully."; 
//                }

//                try
//                {
//                    _context.SaveChanges();
//                    return Json(new { taskSuccess = true, oCurrId = oCB.Id, userMess = ViewBag.UserMsg });

//                }
//                catch (Exception ex)
//                {
//                    return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Failed saving congregation. Err: " + ex.Message });
//                }

//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Failed saving congregation. Err: " + ex.Message });
//            }
//        }

//        // GET: ChurchBody/Delete/5 
//        public IActionResult Delete_CB(int id, int setIndex, bool delConfirmed = false)
//        { //handle this later.... too big

//            try
//            {
//                //    var oCB = _context.ChurchBody.Include(c => c.AppGlobalOwner).Include(c => c.TransferTypeChurchBodys)
//                //        .Include(c => c.AppGlobalOwns).Include(c => c.ChurchBodies).Include(c => c.AppSubscriptions).Include(c => c.ApprovalProcesses)
//                //        .Where(c => c.Id == id).FirstOrDefault();
//                //    if (oCB != null)
//                //    {
//                //        var saveDelete = true;
//                //        //ensuring cascade delete where there's none!
//                //        if (oCB.ChurchBodies.Count + oCB.TransferTypeChurchBodys.Count + oCB.AppSubscriptions.Count + oCB.AppGlobalOwns.Count + oCB.ApprovalProcesses.Count > 0)
//                //        {
//                //            if (delConfirmed)
//                //            {
//                //                try
//                //                {
//                //                    foreach (var child in oCB.AppGlobalOwns.ToList())
//                //                        _context.AppGlobalOwner.Remove(child);
//                //                    foreach (var child in oCB.ChurchBodies.ToList())
//                //                        _context.ChurchBody.Remove(child);
//                //                    foreach (var child in oCB.AppSubscriptions.ToList())
//                //                        _context.AppSubscription.Remove(child);
//                //                    foreach (var child in oCB.ApprovalProcesses.ToList())
//                //                        _context.ApprovalProcess.Remove(child);
//                //                    foreach (var child in oCB.TransferTypeChurchBodys.ToList())
//                //                        _context.TransferTypeChurchBody.Remove(child);
//                //                }
//                //                catch (Exception ex)
//                //                {
//                //                    saveDelete = false;
//                //                    return Json(new { taskSuccess = false, oCurrId = id, userMess = "Error occured while deleting specified congregation: " + ex.Message + ". Reload and try to delete again." });

//                //                }
//                //            }
//                //            else
//                //            {
//                //                saveDelete = false;
//                //                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Specified congregation has connections with other external data. Delete cannot be done unless child refeneces are removed." });

//                //            }
//                //        }

//                //        if (saveDelete)
//                //        {
//                //            _context.ChurchBody.Remove(oCB);
//                //            _context.SaveChanges();

//                //            return Json(new { taskSuccess = true, oCurrId = oCB.Id, userMess = "congregation successfully deleted." });
//                //        }
//                //    }

//                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Failed deleting congregation" });
//        }
//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = id, userMess = "Failed deleting congregation" + ". Err: " + ex.Message });
//            }
//        }



//        public UserProfile_MDL AddOrEdit_UPR(int? oCurrChuBodyId, int id = 0, int setIndex = 0)
//        {
//            var oUPR_MDL = new UserProfile_MDL(); TempData.Keep();
//            if (setIndex == 0) return oUPR_MDL;
//            oUPR_MDL.setIndex = setIndex;
//            //
//            var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//            var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//            if (oCurrChuBodyLogOn == null ) return oUPR_MDL;

//            if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
//            else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//            // check permission for Core life...
//            if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
//                return oUPR_MDL;

//            //int? oCurrChuMemberId_LogOn = null;
//            //ChurchMember oCurrChuMember_LogOn = null;
//            if (oUserProfile == null) //prompt!
//                return oUPR_MDL;
//            //if (oUserProfile.ChurchMember == null) //prompt!
//            //    return oUPR_MDL;

//            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
//            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

//            if (id == 0)
//            {
//                //create user and init... 
//                oUPR_MDL.oUserProfile = new UserProfile();

//                oUPR_MDL.oUserProfile.ChurchBodyId = oCurrChuBodyId;
//                //oUPR_MDL.oUserProfile.CountryId = oCurrCtryId;
//                oUPR_MDL.oUserProfile.Created = DateTime.Now;
//                oUPR_MDL.oUserProfile.LastMod = DateTime.Now;
//            }

//            else
//            {
//                oUPR_MDL = (
//                      from t_up in _masterContext.UserProfile.AsNoTracking()//.Include(t => t.ChurchMember)
//                      .Where(x=> x.ChurchBodyId == oCurrChuBodyId && x.Id==id )
//                      from t_cb in _masterContext.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(x=> x.Id == oCurrChuBodyId && x.Id == t_up.ChurchBodyId)
//                    //  from t_cm in _clientContext.ChurchMember.AsNoTracking().Where(x=> x.Id == oCurrChuBodyId && x.Id == t_up.ChurchMemberId)
//                      from t_upr in _masterContext.UserProfileRole.AsNoTracking().Include(t => t.UserRoles).Where(x=> x.ChurchBodyId == oCurrChuBodyId && x.UserProfileId == t_up.Id).DefaultIfEmpty()
//                      from t_urp in _masterContext.UserRolePermission.AsNoTracking().Include(t => t.UserPermissions).Where(x=> x.ChurchBodyId == oCurrChuBodyId && x.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
//                      from t_upg in _masterContext.UserProfileGroup.AsNoTracking().Include(t => t.UserGroups).Where(x=> x.ChurchBodyId == oCurrChuBodyId && x.UserProfileId == t_up.Id).DefaultIfEmpty()
//                      from t_ugp in _masterContext.UserGroupPermission.AsNoTracking().Include(t => t.UserPermissions).Where(x=> x.ChurchBodyId == oCurrChuBodyId && x.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()
//                      select new UserProfile_MDL()
//                      {
//                          oUserProfile = t_up,
//                          lsUserGroups = t_upg.UserGroups,
//                          lsUserRoles = t_upr.UserRoles,
//                          lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),
//                          strCongregation = t_cb != null ? t_cb.Name : "",
//                          strDenomination = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
//                          strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
//                          strUserProfile = t_up.UserDesc // ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim()
//                      }
//                    ).FirstOrDefault();
//            }

//            if (oUPR_MDL.oCurrChurchBody == null) return oUPR_MDL;

//            //oUPR_MDL.oCurrAppGlobalOwner = oCurrChuBodyLogOn.AppGlobalOwner;
//            //oUPR_MDL.oCurrChurchBody = oCurrChuBodyLogOn;
//            //oUPR_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
//            //oUPR_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;
//            oUPR_MDL = this.popLookups_UPR(oUPR_MDL, oUPR_MDL.oCurrChurchBody, oUPR_MDL.oUserProfile);

//            TempData["oVmCurr"] = oUPR_MDL;
//            TempData.Keep();

//            return oUPR_MDL;

//        }

//        public UserProfile_MDL popLookups_UPR(UserProfile_MDL vm, ChurchBody oCurrChurchBody, UserProfile oCurrUserProfile)
//        {
//            if (vm != null)
//            {
//                vm.lkpStatuses = new List<SelectListItem>();
//                foreach (var dl in dlStatus) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }


//                //var qry = new ChurchTransferController(_context, null).GetChurchBodyList(oCurrChurchBody); // (int)oCurrChurchBody.Id);
//                var qry =  GetChurchBodyList(oCurrChurchBody.Id ); // (int)oCurrChurchBody.Id);
//                vm.lkpCongregations = qry.Select(c => new SelectListItem()
//                {
//                    Value = c.Id.ToString(),
//                    Text = c.Name
//                })
//                    .ToList();
//                vm.lkpCongregations.Insert(0, new SelectListItem { Value = "", Text = "Select" });


//                //GetChurchBodySubCategoryList
//                if (oCurrChurchBody.AppGlobalOwner.TotalLevels > 2)
//                {
//                    var qry_1 = GetChurchBodySubCategoryList((int)oCurrChurchBody.AppGlobalOwnerId, (int)oCurrChurchBody.Id, (int)oCurrChurchBody.Id, 1);  //(int)oCurrChurchBody.ParentChurchBodyId
//                    var ls_1 = qry_1.Select(c => new SelectListItem()
//                    {
//                        Value = c.Id.ToString(),
//                        Text = c.Name
//                    })
//                            .ToList();
//                    ls_1.Insert(0, new SelectListItem { Value = null, Text = "Select" });

//                    vm.lkpCongNextCategory = ls_1;

//                    if (qry_1.Count > 0)
//                    {
//                        var tempCB = qry_1[0]; // ToChurchBody.ParentChurchBody;  // next category
//                        vm.strToChurchLevel_1 = tempCB.ChurchLevel.CustomName;
//                        //  vm.ToChurchBodyId_Categ1 = tempCB.Id;
//                    }
//                }

//                else
//                {
//                    //...to be loaded @runtime
//                    var _qry = qry.Where(c => c.Id != oCurrChurchBody.Id).ToList(); // && c.ChurchType == "CF"
//                    var ls_1 = _qry.Select(c => new SelectListItem()
//                    {
//                        Value = c.Id.ToString(),
//                        Text = c.Name
//                    })
//                        .ToList();
//                    ls_1.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//                    if (_qry.Count > 0)
//                    {
//                        var tempCB = _qry[0]; // ToChurchBody.ParentChurchBody;  // next category
//                        vm.strToChurchLevel_1 = ""; // tempCB.ChurchLevel.CustomName;
//                        vm.ToChurchBodyId_Categ1 = null; // tempCB.Id;
//                    }

//                    vm.lkpTargetCongregations = ls_1;
//                    vm.strToChurchLevel = oCurrChurchBody.ChurchLevel.CustomName;
//                }

//                //change code later
//                var qry_2 = _context.ChurchMember.ToList();  // new GetChurchMembersList((int)oCurrChurchBody.Id, null);  //Members = null, Church Leaders = CL, Clergy = CM
//                vm.lkpChurchMembers = qry_2.Select(c => new SelectListItem()
//                {
//                    Value = c.Id.ToString(),
//                    Text = c.FirstName + " " + c.LastName//c.strMemberFullName
//                }) //.OrderBy(c => c.Text)
//                .ToList();
//                vm.lkpChurchMembers.Insert(0, new SelectListItem { Value = "", Text = "Select" });



//                //c.Id != oCurrChurchBody.Id && 
//                vm.lkpCongregations = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCurrChurchBody.AppGlobalOwnerId)
//                                              .OrderBy(c => c.Name).ToList()
//                                              .Select(c => new SelectListItem()
//                                              {
//                                                  Value = c.Id.ToString(),
//                                                  Text = c.Name
//                                              })
//                                              .ToList();

//                vm.lkpCongregations.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//                //
//                vm.lkpDenominations = _context.AppGlobalOwner.Where(c => c.Status=="A").ToList()
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.OwnerName
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//                vm.lkpDenominations.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//                //
//                vm.lkpUserGroups = _masterContext.UserGroup.Include(t => t.ChurchBody).Include(t => t.UserGroupCategory)
//                    .Where(c => c.ChurchBody.AppGlobalOwnerId == oCurrChurchBody.AppGlobalOwnerId && c.ChurchBodyId == oCurrChurchBody.Id && c.Status== "A") 
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.GroupName
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//                vm.lkpUserGroups.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//                //
//                vm.lkpUserRoles = _masterContext.UserRole.Include(t=>t.ChurchBody)
//                    .Where(c => c.ChurchBody.AppGlobalOwnerId == oCurrChurchBody.AppGlobalOwnerId && c.ChurchBodyId == oCurrChurchBody.Id && c.RoleStatus=="A")
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.RoleName
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//                vm.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//                //
//                var cbList = _masterContext.UserProfile.Include(t => t.ChurchBody)
//                                .Where(c => c.ChurchBody.AppGlobalOwnerId == oCurrChurchBody.AppGlobalOwnerId && c.ChurchBodyId == oCurrChurchBody.Id && c.UserStatus == "A");
//                var qry_3 = (from t_up in cbList
//                             from t_cm in _context.ChurchMember.Where(c => c.ChurchBody.AppGlobalOwnerId == oCurrChurchBody.AppGlobalOwnerId && c.Id == t_up.ChurchMemberId)
//                             select t_cm).ToList();

//                vm.lkpChurchMembers = qry_3.Select(c => new SelectListItem()
//                {
//                    Value = c.Id.ToString(),
//                    Text = ((((!string.IsNullOrEmpty(c.Title) ? c.Title : "") + ' ' + c.FirstName).Trim() + " " + c.MiddleName).Trim() + " " + c.LastName).Trim()
//                })
//                                          .OrderBy(c => c.Text)
//                                          .ToList();
//                vm.lkpUserProfiles.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            }

//            return vm;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_UPR(UserProfile_MDL vmMod)
//        {
//            try
//            {
//                var oUPR = vmMod.oUserProfile; // new ChurchFaithType();
//              //  vmMod = TempData.Get<UserProfile_MDL>("oVmCurr"); TempData.Keep();
//                var tempVm = TempData["oVmCurr"] as string;
//                if (string.IsNullOrEmpty(tempVm)) RedirectToAction("LoginUserAcc", "UserLogin");
//                // De serialize the string to object
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfile_MDL>(tempVm); TempData.Keep();

//                ModelState.Remove("oUserProfile.CountryId");
//                ModelState.Remove("oUserProfile.CountryRegionId");
//                ModelState.Remove("oUserProfile.ParentChurchBodyId");
//                ModelState.Remove("oUserProfile.ContactInfoId");
//                ModelState.Remove("oUserProfile.AppGlobalOwnerId");
//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, userMess = "Failed to load the data to save. Please refresh and try again." });

//                if (oUPR.Id == 0)
//                {
//                    var oUPRValid = _masterContext.UserProfile.Where(c => c.ChurchBodyId==oUPR.ChurchBodyId  && c.Username == oUPR.Username).FirstOrDefault();
//                    if (oUPRValid != null)
//                        return Json(new { taskSuccess = false, userMess = "User profile already exists." });
//                    //
//                    oUPR.Created = DateTime.Now;
//                    oUPR.LastMod = DateTime.Now;
//                    _context.Add(oUPR);

//                    ViewBag.UserMsg = "Saved user profile successfully.";
//                }
//                else
//                {
//                    var oUPRValid = _masterContext.UserProfile.Where(c => c.Id != oUPR.Id &&
//                                c.ChurchBodyId == oUPR.ChurchBodyId && c.Username == oUPR.Username).FirstOrDefault();
//                    if (oUPRValid != null)
//                        return Json(new { taskSuccess = false, userMess = "User profile already exists." });
//                    //
//                    oUPR.LastMod = DateTime.Now;
//                    _context.Update(oUPR);
//                    ViewBag.UserMsg = "Updated user profile successfully.";
//                }

//                //save details... locAddr
//                try
//                {
//                    await _context.SaveChangesAsync();

//                    vmMod.oUserProfile = oUPR;
//                    TempData["oVmCurr"] = vmMod;
//                    TempData.Keep();

//                    return Json(new { taskSuccess = true, userMess = ViewBag.UserMsg });
//                }

//                catch (Exception ex)
//                {
//                    return Json(new { taskSuccess = false, userMess = "Failed saving user profile. Err: " + ex.Message });
//                }

//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, userMess = "Failed saving congregation. Err: " + ex.Message });
//            }
//        }

//        // GET: UserProfile/Delete/5 
//        public async Task<IActionResult> Delete_UPR(int? id)   //int? oCurrChuBodyId, 
//        {
//            var oUPR = await _masterContext.UserProfile.FindAsync(id);
//            if (oUPR != null)
//            {
//                _masterContext.UserProfile.Remove(oUPR);
//                await _masterContext.SaveChangesAsync();

//                //return Json(new { taskSuccess = true, userMess = "country region successfully deleted." });               
//            }
//            return RedirectToAction(nameof(Index));
//            //return Json(new { taskSuccess = false, userMess = "Failed deleting country region."});
//        }


 

//    }

//}