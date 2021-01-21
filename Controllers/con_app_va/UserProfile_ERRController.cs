
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
//using RhemaCMS.Controllers.con_adhc;
//using RhemaCMS.Models;
//using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.CLNTModels;
//using RhemaCMS.Models.MSTRModels;
//using RhemaCMS.Models.ViewModels;
//using static RhemaCMS.Models.ViewModels.AppVenAdminVM;

//namespace RhemaCMS.Controllers.con_app_va
//{
//    public class UserProfile_ERRController : Controller
//    {
//        private readonly MSTR_DbContext _context;
//        private readonly ChurchModelContext _clientDBContext;

//        private bool isCurrValid = false;
//        private List<UserSessionPrivilege> oUserLogIn_Priv = null;
//        private readonly IWebHostEnvironment hostingEnvironment;


//        private List<DiscreteLookup> dlUserRoleTypes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlShareStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlOwnerStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlGenStatuses = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlChurchType = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlChuWorkStat = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlUserAuthTypes = new List<DiscreteLookup>(); 

//        public UserProfile_ERRController(MSTR_DbContext context, ChurchModelContext clientDBContext, IWebHostEnvironment hostingEnvironment)
//        {
//            _context = context;
//            _clientDBContext = clientDBContext; 

//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "D", Desc = "Deactive" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "E", Desc = "Expired" });

//            dlUserAuthTypes.Add(new DiscreteLookup() { Category = "UserAuthType", Val = "1", Desc = "Two-way Authentication" });
//            dlUserAuthTypes.Add(new DiscreteLookup() { Category = "UserAuthType", Val = "2", Desc = "Security Question Validation" }); 

//            ////SharingStatus { get; set; }  // A - Share with all sub-congregations, C - Share with child congregations only, N - Do not share
//            //dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "N", Desc = "Do not roll-down (share)" });
//            //dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "C", Desc = "Roll-down (share) for direct child congregations" });
//            //dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "A", Desc = "Roll-down (share) for all sub-congregations" });

//            //// OwnershipStatus { get; set; }  // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody
//            //dlOwnerStatus.Add(new DiscreteLookup() { Category = "OwnStat", Val = "O", Desc = "Originated" });
//            //dlOwnerStatus.Add(new DiscreteLookup() { Category = "OwnStat", Val = "I", Desc = "Inherited" });
             
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

//            //dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "", Desc = "N/A" });
//            //dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CH", Desc = "Congregation Head-unit" });
//            //dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CF", Desc = "Congregation" });

//            //dlChuWorkStat.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "S", Desc = "Structure Only" });
//            //dlChuWorkStat.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "O", Desc = "Operationalized" });
//        }

         

//        public string GetStatusDesc(string oCode)
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

//        public int  GetRoleTypeLevel (string oCode)
//        {
//            switch (oCode)
//            {
//                case "SYS": return 1;
//                case "SUP_ADMN": return 2;
//                case "SYS_ADMN": return 3;
//                case "SYS_CUST": return 4;
//               // case "SYS_CUST2": return 5;
//                    //
//                case "CH_ADMN": return 6;
//                case "CH_RGSTR": return 7;
//                case "CH_ACCT": return 8;
//                case "CH_CUST": return 9;
//               // case "CH_CUST2": return 10;
//                    //
//                case "CF_ADMN": return 11;
//                case "CF_RGSTR": return 12;
//                case "CF_ACCT": return 13;
//                case "CF_CUST": return 14; 
//               // case "CF_CUST2": return 15; 
//                    //
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


//        private bool userAuthorized = false;
//        private void SetUserLogged()
//        {
//            ////  oUserLogIn_Priv = TempData.Get<List<UserSessionPrivilege>>("UserLogIn_oUserPrivCol");

//            //List<UserSessionPrivilege> oUserLogIn_Priv = TempData.ContainsKey("UserLogIn_oUserPrivCol") ?
//            //                                                TempData["UserLogIn_oUserPrivCol"] as List<UserSessionPrivilege> : null;


//            var tempPrivList = TempData["UserLogIn_oUserPrivCol"] as string;
//            // De serialize the string to object
//            oUserLogIn_Priv = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);

//            isCurrValid = oUserLogIn_Priv?.Count > 0;
//            if (isCurrValid)
//            {
//                ViewBag.oChuBodyLogged = oUserLogIn_Priv[0].ChurchBody;
//                ViewBag.oUserLogged = oUserLogIn_Priv[0].UserProfile;

//                // check permission for Core life...  given the sets of permissions
//                userAuthorized = oUserLogIn_Priv.Count > 0; //(oUserLogIn_Priv.Find(x => x.PermissionName == "_A0__System_Administration" || x.PermissionName == "xxx") != null);
//            }

//        }

//        public ActionResult Index_CL(int? oCurrChuBodyId = null)  //(int? oDenomId = null, int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0, int? oParentId = null)  
//        {
//            //Request.Headers.Add("entityId", "COPDatabase");
//            //Request.Headers.TryGetValue("entityId", out var entityVal);
//            //var entityValue =  entityVal;

//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                UserProfileVM oUserProVM = TempData.ContainsKey("oVmCurr") ? TempData["oVmCurr"] as UserProfileVM : new UserProfileVM();
//               // var oCBConVM = new UserProfileVM(); TempData.Keep();

//                var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                // if (oCurrChuBodyLogOn == null) return View(oCBConVM);
//                if (oCurrChuBodyLogOn != null)
//                {
//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oCurrChuBodyLogOn.Id; }
//                    else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
//                }

//                // check permission for Core life...
//                if (!this.userAuthorized)
//                {
//                    //retain view
//                    return View(oUserProVM);
//                }

//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//              // if (oUserProfile == null) return View(oUserProVM);

//                var currChurchMemberLogged = _clientDBContext.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCBConVM);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }

//                var userList = (from t_up in _context.UserProfile.Where(c => c.ChurchBodyId==oCurrChuBodyId && c.ProfileScope == "C")
//                                from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == t_up.ChurchBodyId && c.UserProfileId == t_up.Id).DefaultIfEmpty()
//                                from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == t_up.ChurchBodyId && c.Id == t_upr.UserRoleId && c.RoleLevel > 2).DefaultIfEmpty()
//                                select t_up
//                                ).OrderBy(c=>c.UserDesc).ToList();

//                oUserProVM.lsUserProfiles = userList;
//                //                
//                oUserProVM.oAppGlolOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                oUserProVM.oChurchBodyId = oCurrChuBodyLogOn.Id;
//                oUserProVM.oCurrLoggedUserId = oUserProfile.Id;
//                //
//                oUserProVM.oChurchBody = oCurrChuBodyLogOn;
//                oUserProVM.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;
                                 
//                //
//                // TempData.Put("oVmCB_CNFG", oCBConVM);
//                TempData.Keep();
//                return View(oUserProVM);
//            }
//        }

//        public ActionResult Index_MS(int? oCurrChuBodyId = null, int setIndex = 0)  //(int? oDenomId = null, int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0, int? oParentId = null)  
//        {
//            //Request.Headers.Add("entityId", "COPDatabase");
//            //Request.Headers.TryGetValue("entityId", out var entityVal);
//            //var entityValue =  entityVal;

//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                UserProfileVM oUserProVM = TempData.ContainsKey("oVmCurr") ? TempData["oVmCurr"] as UserProfileVM : new UserProfileVM();
//                // var oCBConVM = new UserProfileVM(); TempData.Keep();

//                var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                // if (oCurrChuBodyLogOn == null) return View(oCBConVM);
//                if (oCurrChuBodyLogOn != null)
//                {
//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oCurrChuBodyLogOn.Id; }
//                    else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
//                }

//                // check permission for Core life...
//                if (!this.userAuthorized)
//                {
//                    //retain view
//                    return View(oUserProVM);
//                }

//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//                // if (oUserProfile == null) return View(oUserProVM);

//                var currChurchMemberLogged = _clientDBContext.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCBConVM);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }

//                var userList = (from t_up in _context.UserProfile.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "V")
//                                from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == t_up.ChurchBodyId && c.UserProfileId == t_up.Id).DefaultIfEmpty()
//                                from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == t_up.ChurchBodyId && c.Id == t_upr.UserRoleId && c.RoleLevel <= 2 ).DefaultIfEmpty()
//                                select t_up
//                ).OrderBy(c => c.UserDesc).ToList();
                 

//                oUserProVM.lsUserProfiles = userList;
//                //                
//                oUserProVM.oAppGlolOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                oUserProVM.oChurchBodyId = oCurrChuBodyLogOn.Id;
//                oUserProVM.oCurrLoggedUserId = oUserProfile.Id;
//                oUserProVM.setIndex = setIndex;
//                //
//                oUserProVM.oChurchBody = oCurrChuBodyLogOn;
//                oUserProVM.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;

//                //
//                // TempData.Put("oVmCB_CNFG", oCBConVM);
//                TempData.Keep();
//                return View(oUserProVM);
//            }
//        }

//        private List<UserPermission> GetPermissionsByRole(int? userRoleId = null, int? oCurrChuBodyId = null)
//        {  //System roles ... oCurrChuBodyId == null
//           // if (oCurrChuBodyId == null) return new List<UserPermission>();

//            var userPerms = (
//                        from t_upr in _context.UserRolePermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userRoleId == null || (userRoleId != null && c.UserRoleId == userRoleId)))
//                        from t_up in _context.UserPermission.Where(c => c.PermStatus == "A" && c.Id==t_upr.UserRoleId)                               
//                              select t_up 
//                               ) 
//                               .OrderBy(c => c.PermissionCode).ToList(); 

//            return userPerms;
//        }

//        public JsonResult GetPermissionsListByRole(int? userRoleId, int? oCurrChuBodyId = null)  //, bool addEmpty = false)
//        {  
//            var userPerms = (
//                        from t_upr in _context.UserRolePermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userRoleId == null || (userRoleId != null && c.UserRoleId == userRoleId)))
//                        from t_up in _context.UserPermission.Where(c => c.PermStatus == "A" && c.Id == t_upr.UserRoleId)                               
//                        select t_up
//                               ).OrderBy(c => c.PermissionCode).ToList()                                
//                                .Select(c => new SelectListItem()
//                                {
//                                    Value = c.Id.ToString(),
//                                    Text = c.PermissionName
//                                })
//                                .OrderBy(c => c.Text)
//                                .ToList(); 

//           // if (addEmpty) userPerms.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json(userPerms);
//        }

//        private List<UserPermission> GetPermissionsByGroup(int? userGroupId = null, int? oCurrChuBodyId = null)
//        {
//            if (oCurrChuBodyId == null) return new List<UserPermission>();

//            var userPerms = (
//                        from t_upr in _context.UserGroupPermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userGroupId == null || (userGroupId != null && c.UserGroupId == userGroupId)))
//                        from t_up in _context.UserPermission.Where(c =>  c.PermStatus == "A" && c.Id == t_upr.UserGroupId)
//                               .OrderBy(c => c.PermissionCode)
//                        select t_up
//                               )
//                               .ToList();

//            return userPerms;
//        }

//        public JsonResult GetPermissionsListByGroup(int? userGroupId, int? oCurrChuBodyId = null)  //, bool addEmpty = false)
//        {
//            var userPerms = (
//                        from t_upr in _context.UserGroupPermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userGroupId == null || (userGroupId != null && c.UserGroupId == userGroupId)))
//                        from t_up in _context.UserPermission.Where(c =>  c.PermStatus == "A" && c.Id == t_upr.UserGroupId)
//                        select t_up
//                               ).OrderBy(c => c.PermissionCode).ToList()
//                                .Select(c => new SelectListItem()
//                                {
//                                    Value = c.Id.ToString(),
//                                    Text = c.PermissionName
//                                })
//                                .OrderBy(c => c.Text)
//                                .ToList();

//            // if (addEmpty) userPerms.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json(userPerms);
//        }
      
//        private UserProfileVM populateLookups_UP_CL(UserProfileVM vmLkp, ChurchBody oCurrChuBody )
//        {
//            if (vmLkp == null || oCurrChuBody == null) return vmLkp;
//            //
//            vmLkp.lkpStatuses = new List<SelectListItem>();
//            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpUserRoles = _context.UserRole.Where(c=>c.ChurchBodyId == oCurrChuBody.Id && c.RoleStatus=="A" && c.RoleLevel > 2)
//                               .OrderBy(c=>c.RoleLevel)
//                               .Select(c => new SelectListItem()
//                               {
//                                   Value = c.Id.ToString(),
//                                   Text = c.RoleName.Trim()
//                               })
//                               // .OrderBy(c => c.Text)
//                               .ToList();
//            //  vmLkp.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//            vmLkp.lkpUserGroups = _context.UserGroup.Where(c =>  c.ChurchBodyId == oCurrChuBody.Id  && c.Status == "A" )
//                               .OrderBy(c => c.UserGroupCategoryId).ThenBy(c=>c.GroupName)
//                               .Select(c => new SelectListItem()
//                               {
//                                   Value = c.Id.ToString(),
//                                   Text = c.GroupName.Trim()
//                               })
//                               // .OrderBy(c => c.Text)
//                               .ToList();
            

//            vmLkp.lkpPwdSecQueList = _context.AppUtilityNVP.Where(c => c.NvpCode == "PWD_SEC_QUE")
//                      .OrderBy(c => c.RequireUserCustom).ThenBy(c => c.OrderIndex).ThenBy(c => c.NvpValue)
//                      .ToList()
//                      .Select(c => new SelectListItem()
//                      {
//                          Value = c.Id.ToString(),
//                          Text = c.NvpValue
//                      })
//                      // .OrderBy(c => c.Text)
//                      .ToList();
//            vmLkp.lkpPwdSecQueList.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//            //vmLkp.lkpPwdSecAnsList = _context.AppUtilityNVP.Where(c => c.NvpCode == "PWD_SEC_ANS")
//            //         .OrderBy(c => c.RequireUserCustom).ThenBy(c => c.OrderIndex).ThenBy(c => c.NvpValue)
//            //         .ToList()
//            //         .Select(c => new SelectListItem()
//            //         {
//            //             Value = c.Id.ToString(),
//            //             Text = c.NvpValue
//            //         })
//            //         // .OrderBy(c => c.Text)
//            //         .ToList();
//            //vmLkp.lkpPwdSecAnsList.Insert(0, new SelectListItem { Value = "", Text = "Select" });


//            return vmLkp;
//        }

//        private UserProfileVM populateLookups_UP_MS(UserProfileVM vmLkp, int? AppGloOwnId )
//        {
//            //if (vmLkp == null || oDenom == null) return vmLkp;
//            // 
//            vmLkp.lkpStatuses = new List<SelectListItem>();
//            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpUserRoles = _context.UserRole.Where(c => (AppGloOwnId == null || (AppGloOwnId != null && c.ChurchBody.AppGlobalOwnerId == AppGloOwnId)) && c.RoleStatus == "A" && c.RoleLevel == 2)
//                                .OrderBy(c => c.RoleLevel)
//                                .Select(c => new SelectListItem()
//                                {
//                                    Value = c.Id.ToString(),
//                                    Text = c.RoleName.Trim()
//                                })
//                                // .OrderBy(c => c.Text)
//                                .ToList();
//            //  vmLkp.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//            vmLkp.lkpUserGroups = _context.UserGroup.Where(c => (AppGloOwnId == null || (AppGloOwnId != null && c.ChurchBody.AppGlobalOwnerId == AppGloOwnId)) && c.Status == "A")
//                               .OrderBy(c => c.UserGroupCategoryId).ThenBy(c => c.GroupName)
//                               .Select(c => new SelectListItem()
//                               {
//                                   Value = c.Id.ToString(),
//                                   Text = c.GroupName.Trim()
//                               })
//                               // .OrderBy(c => c.Text)
//                               .ToList();
//            return vmLkp;
//        }


//        [HttpGet]
//        public IActionResult AddOrEdit_UP(int id = 0, int? oCurrChuBodyId = null, string profileScope = "C", int setIndex = 0)   // setIndex = 0 (SYS), setIndex = 1 (SUP_ADMN), = 2 (Create/update user), = 3 (reset Pwd) 
//        { 
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                UserProfileVM oCurrVmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileVM : new UserProfileVM();
//               // var oCBConVM = new UserProfileVM(); TempData.Keep();

//                var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                // if (oCurrChuBodyLogOn == null) return View(oCBConVM);
//                if (oCurrChuBodyLogOn != null)
//                {
//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oCurrChuBodyLogOn.Id; }
//                    else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
//                }

//                // check permission for Core life...
//                if (!this.userAuthorized)
//                {   //retain view
//                    return View(oCurrVmMod);
//                }

//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//              // if (oUserProfile == null) return View(oUserProVM);

//                var currChurchMemberLogged = _clientDBContext.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCBConVM);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }
                 
//                if (id == 0)
//                {
//                    //create user and init...
//                    oCurrVmMod.oUserProfile = new UserProfile();
//                    if (setIndex == 2) //both local user acc and sys admin acc
//                    {
//                        if (oCurrChuBodyLogOn != null)
//                        {
//                            oCurrVmMod.oUserProfile.AppGlobalOwnerId = oCurrChuBodyLogOn != null ? oCurrChuBodyLogOn.AppGlobalOwnerId : null;
//                            oCurrVmMod.oUserProfile.ChurchBodyId = (int)oCurrChuBodyLogOn.Id;
//                        }
                            
//                        oCurrVmMod.oUserProfile.OwnerId = (int)oUserProfile.Id;
//                        oCurrVmMod.oUserProfile.CreatedByUserId = (int)oUserProfile.Id;
//                        oCurrVmMod.oUserProfile.LastModByUserId = (int)oUserProfile.Id;                       
//                    }
                  
//                    var tm = DateTime.Now;
//                    oCurrVmMod.oUserProfile.Strt = tm;
//                    oCurrVmMod.oUserProfile.Expr = tm.AddDays(90);  //default to 30 days
//                  //  oCurrVmMod.oUserProfile.UserId = oCurrChuMemberId_LogOn;
                    
//                    oCurrVmMod.oUserProfile.UserScope = setIndex==1 ? "I" : "E"; // I-internal, E-external
                    
//                    if (oCurrVmMod.oUserProfile.UserScope=="I") oCurrVmMod.oUserProfile.ChurchMemberId = oCurrChuMemberId_LogOn;

//                    oCurrVmMod.oUserProfile.ProfileScope  = profileScope; // V-Vendor, C-Client

//                    oCurrVmMod.oUserProfile.ResetPwdOnNextLogOn = true;  
//                   oCurrVmMod.oUserProfile.PwdExpr = tm.AddDays(30);  //default to 90 days 
//                    oCurrVmMod.oUserProfile.UserStatus = "A"; // A-ctive...D-eactive
                     
//                    oCurrVmMod.oUserProfile.Created = tm;  
//                    oCurrVmMod.oUserProfile.LastMod = tm;                      
//                }

//                else
//                {  //fetch mem details
//                    if (oCurrChuBodyLogOn != null)
//                    {
//                        oCurrVmMod.oUserProfile = _context.UserProfile
//                            .Where(c => c.Id == id && c.AppGlobalOwnerId == oCurrChuBodyLogOn.AppGlobalOwnerId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == profileScope).FirstOrDefault();
//                    }
//                    else
//                    {
//                        oCurrVmMod.oUserProfile = _context.UserProfile
//                            .Where(c => c.Id == id && c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileScope == "V").FirstOrDefault();
//                    }
                        
//                    if (oCurrVmMod.oUserProfile != null)
//                    {
//                        var signOutToReset = true;
//                        if (oCurrVmMod.oUserProfile.Expr != null) if (oCurrVmMod.oUserProfile.Expr.Value >= DateTime.Now.Date) 
//                            { ViewBag.SignOutResetReason = "AccExpr"; signOutToReset = true; }   //use viewbag to comm to user
//                        if (!signOutToReset) if (oCurrVmMod.oUserProfile.PwdExpr != null) if (oCurrVmMod.oUserProfile.PwdExpr.Value >= DateTime.Now.Date) 
//                                { ViewBag.SignOutResetReason = "PwdExpr"; signOutToReset = true;}                       

//                        if (signOutToReset)
//                        {
//                            if (profileScope == "C")
//                            {
//                                oCurrVmMod.lsUserProfiles = (from t_upx in _context.UserProfile.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "C")
//                                                             from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.UserProfileId == t_upx.Id).DefaultIfEmpty()
//                                                             from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.Id == t_upr.UserRoleId && c.RoleLevel > 2).DefaultIfEmpty()
//                                                             select t_upx
//                                ).OrderBy(c => c.UserDesc).ToList();
//                            }
//                            else
//                            {
//                                oCurrVmMod.lsUserProfiles = (from t_upx in _context.UserProfile.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "V")
//                                                             from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.UserProfileId == t_upx.Id).DefaultIfEmpty()
//                                                             from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.Id == t_upr.UserRoleId && c.RoleLevel <= 2).DefaultIfEmpty()
//                                                             select t_upx
//                                                ).OrderBy(c => c.UserDesc).ToList();
//                            }

//                            // return to reset ... pwd or re-activate account
//                            return View(oCurrVmMod);
//                        }

//                        var t_up = oCurrVmMod.oUserProfile;
//                        oCurrVmMod.lsUserRoles = (
//                                        from t_upr in _context.UserProfileRole.Where(c => c.UserProfileId == t_up.Id && c.ChurchBodyId == t_up.ChurchBodyId)
//                                        from t_ur in _context.UserRole.Where(c => c.Id == t_upr.UserRoleId && c.ChurchBodyId == t_up.ChurchBodyId)
//                                        select t_ur
//                            ).ToList();

//                        oCurrVmMod.lsUserGroups = (
//                                        from t_upg in _context.UserProfileGroup.Where(c => c.UserProfileId == t_up.Id && c.ChurchBodyId == t_up.ChurchBodyId)
//                                        from t_ug in _context.UserGroup.Where(c => c.Id == t_upg.UserGroupId && c.ChurchBodyId == t_up.ChurchBodyId)
//                                        select t_ug
//                            ).ToList();

//                        oCurrVmMod.lsUserPermissions = (
//                                        from t_upr in _context.UserProfileRole.Where(c => c.UserProfileId == t_up.Id && c.ChurchBodyId == t_up.ChurchBodyId)
//                                        from t_urp in _context.UserRolePermission.Where(c => c.UserRoleId == t_upr.UserRoleId && c.ChurchBodyId == t_up.ChurchBodyId)
//                                        from t_up_r in _context.UserPermission.Where(c => c.Id == t_urp.UserRoleId  )
//                                        select t_up_r
//                            ).ToList();

//                        oCurrVmMod.lsUserPermissions.AddRange( (
//                                        from t_upg in _context.UserProfileGroup.Where(c => c.UserProfileId == t_up.Id && c.ChurchBodyId == t_up.ChurchBodyId)
//                                        from t_ugp in _context.UserRolePermission.Where(c => c.UserRoleId == t_upg.UserGroupId && c.ChurchBodyId == t_up.ChurchBodyId)
//                                        from t_up_g in _context.UserPermission.Where(c => c.Id == t_ugp.UserPermissionId )
//                                        select t_up_g
//                            ).ToList());
//                    }
                    

//                    //var oUser = (from t_up in _context.UserProfile.Where(c => c.Id==id && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "V"  )
//                    //                from t_upr in _context.UserProfileRole.Where(c => c.UserProfileId == t_up.Id && c.ChurchBodyId == t_up.ChurchBodyId ).DefaultIfEmpty()
//                    //                from t_upg in _context.UserProfileGroup.Where(c => c.UserProfileId == t_up.Id && c.ChurchBodyId == t_up.ChurchBodyId).DefaultIfEmpty() 
//                    //                from t_ur in _context.UserRole.Where(c => c.Id == t_upr.UserRoleId && c.ChurchBodyId == t_up.ChurchBodyId).DefaultIfEmpty()
//                    //                from t_ug in _context.UserGroup.Where(c => c.Id == t_upg.UserGroupId && c.ChurchBodyId == t_up.ChurchBodyId).DefaultIfEmpty() 
//                    //                from t_urp in _context.UserRolePermission.Where(c => c.UserRoleId == t_ur.Id && c.ChurchBodyId == t_up.ChurchBodyId ).DefaultIfEmpty()
//                    //                from t_ugp in _context.UserGroupPermission.Where(c => c.UserGroupId == t_ug.Id && c.ChurchBodyId == t_up.ChurchBodyId ).DefaultIfEmpty()
//                    //                from t_up_r in _context.UserPermission.Where(c => c.Id == t_urp.UserRoleId && c.ChurchBodyId == t_up.ChurchBodyId).DefaultIfEmpty()
//                    //                from t_up_g in _context.UserPermission.Where(c => c.Id == t_ugp.UserGroupId && c.ChurchBodyId == t_up.ChurchBodyId).DefaultIfEmpty()
//                    //                select new UserProfileVM()
//                    //                {
//                    //                   oUserProfile = t_up,
//                    //                   oUserProfileRole = t_upr,
//                    //                   oUserProfileGroup = t_upg,
//                    //                   oUserRole = t_ur,
//                    //                   oUserGroup  = t_ug,
//                    //                   oUserRolePermission  = t_urp,
//                    //                   oUserGroupPermission  = t_ugp, 
//                    //                   oUserPermission =   t_up_r // + t_up_g
//                    //                }
//                    //               ).FirstOrDefault();

//                    if (oCurrVmMod == null) return PartialView("_AddOrEdit_UP", oCurrVmMod); 
//                }

//                //
//                if (oCurrChuBodyLogOn != null)
//                {
//                    oCurrVmMod.oAppGlolOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                    oCurrVmMod.oChurchBodyId = oCurrChuBodyLogOn.Id;
//                    oCurrVmMod.oChurchBody = oCurrChuBodyLogOn;
//                }                

//                oCurrVmMod.oCurrLoggedUserId = oUserProfile.Id;               
//                oCurrVmMod.profileScope = profileScope ;
//                oCurrVmMod.setIndex = setIndex ;
//                // 
//                var currCB = _clientDBContext.ChurchBody.Where(c => c.GlobalChurchCode == oCurrChuBodyLogOn.GlobalChurchCode && c.SubscriptionKey== oCurrChuBodyLogOn.SubscriptionKey).FirstOrDefault();
//                var _vmMod = (oCurrChuBodyLogOn == null && profileScope == "V") ? populateLookups_UP_MS(oCurrVmMod, oCurrVmMod.oAppGlolOwnId) : populateLookups_UP_CL(oCurrVmMod, currCB);

//                TempData["oVmCurrMod"] = _vmMod;
//                TempData.Keep();
//                return PartialView("_AddOrEdit_UP", _vmMod);
//            }
//        }


//        public IActionResult AddOrEdit_UP_ChangePwd(int userId = 0, int? oCurrChuBodyId = null, string profileScope = "C", int setIndex = 0)   // setIndex = 0 (SYS), setIndex = 1 (SUP_ADMN), = 2 (Create/update user), = 3 (reset Pwd) 
//        {
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                ResetUserProfilePwdVM oCurrVmMod =  new ResetUserProfilePwdVM();

//                int? oAppGloOwnId = null;
//                var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                // if (oCurrChuBodyLogOn == null) return View(oCBConVM);
//                if (oCurrChuBodyLogOn != null)
//                {
//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oCurrChuBodyLogOn.Id; }
//                    else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//                    oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                }

//                // check permission for Core life...
//                if (!this.userAuthorized)
//                {   //retain view
//                    return View(oCurrVmMod);
//                }

//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//                // if (oUserProfile == null) return View(oUserProVM);

//                var currChurchMemberLogged = _clientDBContext.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCBConVM);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }

//                var tempUser = _context.UserProfile
//                         .Where(c => c.Id == userId && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == profileScope).FirstOrDefault();

//                if (tempUser == null) return PartialView("_AddOrEdit_UP_ChangePwd", oCurrVmMod);

//                // 
//                oCurrVmMod.oChurchBody = oCurrChuBodyLogOn; 
//                oCurrVmMod.setIndex = setIndex;
//                oCurrVmMod.oUserProfile = tempUser;
//                oCurrVmMod.Username = tempUser.Username;
//                oCurrVmMod.CurrentPassword = null;
//                oCurrVmMod.NewPassword  = null;
//                oCurrVmMod.RepeatPassword  = null ;
//                oCurrVmMod.SecurityQue = tempUser.PwdSecurityQue ;
//                oCurrVmMod.SecurityAns = null ;
//                oCurrVmMod.VerificationCode = null ;
//                oCurrVmMod.strLogUserDesc = tempUser.UserDesc ;
//                oCurrVmMod.AuthTypeUsed = null ;
                
//                // 
//                oCurrVmMod.lkpAuthTypes  = new List<SelectListItem>();
//                foreach (var dl in dlUserAuthTypes) { oCurrVmMod.lkpAuthTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//                var _vmMod = oCurrVmMod;  
//                TempData["oVmCurrMod"] = _vmMod;
//                TempData.Keep();
//                return PartialView("_AddOrEdit_UP_ChangePwd", _vmMod);
//            }
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_UP(UserProfileVM vmMod)
//        {
//            UserProfile _oChanges = vmMod.oUserProfile; 
//             vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileVM : vmMod; TempData.Keep();
             
//            var oCV = vmMod.oUserProfile;
//            oCV.ChurchBody = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oUserProfile.AppGlobalOwnerId");
//                ModelState.Remove("oUserProfile.ChurchBodyId");
//                ModelState.Remove("oUserProfile.ChurchMemberId");
//                ModelState.Remove("oUserProfile.CreatedByUserId");
//                ModelState.Remove("oUserProfile.LastModByUserId");
//                ModelState.Remove("oUserProfile.OwnerId");

//                // ChurchBody == null
//                //var churchCode = "000000"; _oChanges.Username = "SysAdmin"; _oChanges.Pwd = "$ys@dmin1";
//                _oChanges.Pwd = "123456";  //temp pwd... to reset @ next login
//                _oChanges.Pwd = AppUtilties.ComputeSha256Hash(_oChanges.Username + _oChanges.Pwd);

//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", signOutToLogIn = false });

//                if (string.IsNullOrEmpty(_oChanges.Username) || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide username and password.", signOutToLogIn = false });
//                }
//                if (_oChanges.PwdSecurityQue != null && string.IsNullOrEmpty(_oChanges.PwdSecurityAns))  //Congregant... ChurcCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please respond to security question specified.", signOutToLogIn = false });
//                }
//                if (string.IsNullOrEmpty(_oChanges.UserDesc))  //Congregant... ChurchCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the user description or name of user.", signOutToLogIn = false });
//                }

//                if (_oChanges.Expr != null)
//                {
//                    if (_oChanges.Expr.Value >= DateTime.Now.Date)
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please user account has expired. Activate account first.", signOutToLogIn = false });
//                }

//                if (_oChanges.Email != null) //_oChanges.ChurchMemberId != null && 
//                {
//                    var oExistUser = _context.MSTRContactInfo.Where(c => c.RefUserId != _oChanges.Id && c.Email == _oChanges.Email).FirstOrDefault();
//                    if (oExistUser != null)  // ModelState.AddModelError(_oChanges.Id.ToString(), "Email of member must be unique. >> Hint: Already used by another member: "  + GetConcatMemberName(_oChanges.Title, _oChanges.FirstName, _oChanges.MiddleName, _oChanges.LastName) + "[" + oCM.ChurchBody.Name + "]");
//                        return Json(new
//                        {
//                            taskSuccess = false,
//                            oCurrId = _oChanges.Id,
//                            userMess = "User email must be unique. >> Hint: Already used by another: [User: " + _oChanges.UserDesc + "]", //  GetConcatMemberName(_oChanges.ChurchMember.Title, _oChanges.ChurchMember.FirstName, _oChanges.ChurchMember.MiddleName, _oChanges.ChurchMember.LastName) + "[" + oCV.ChurchBody.Name + "]",
//                            signOutToLogIn = false
//                        });
//                }

//                //if (_oChanges.ChurchMemberId != null && _oChanges.Email != null)
//                //{
//                //    var oExistUser = _context.MSTRContactInfo //.Include(t=>t.ChurchMember)
//                //        .Where(c => c.ChurchMemberId != _oChanges.ChurchMemberId && c.Id != _oChanges.ChurchMember.ContactInfoId && c.ChurchBodyId == _oChanges.ChurchBodyId && c.Email == _oChanges.Email).FirstOrDefault();
//                //    if (oExistUser != null)  // ModelState.AddModelError(_oChanges.Id.ToString(), "Email of member must be unique. >> Hint: Already used by another member: "  + GetConcatMemberName(_oChanges.Title, _oChanges.FirstName, _oChanges.MiddleName, _oChanges.LastName) + "[" + oCM.ChurchBody.Name + "]");
//                //        return Json(new
//                //        {
//                //            taskSuccess = false,
//                //            oCurrId = _oChanges.Id,
//                //            userMess = "Email of member must be unique. >> Hint: Already used by another member: " + GetConcatMemberName(_oChanges.ChurchMember.Title, _oChanges.ChurchMember.FirstName, _oChanges.ChurchMember.MiddleName, _oChanges.ChurchMember.LastName) + "[" + oCV.ChurchBody.Name + "]",
//                //            signOutToLogIn = false
//                //        });

//                //    //if (_oChanges == null)
//                //    //{
//                //    //    return Json(new
//                //    //    {
//                //    //        taskSuccess = false,
//                //    //        oCurrId = _oChanges.Id,
//                //    //        userMess = "Member status [ current state of the person - active, dormant, invalid, deceased etc. ] is required"
//                //    //    });
//                //    //}

//                //    ////member must be active, NOT deceased
//                //    //if (_oChanges_MS.ChurchMemStatusId == null)
//                //    //{
//                //    //    return Json(new
//                //    //    {
//                //    //        taskSuccess = false,
//                //    //        oCurrId = _oChanges.Id,
//                //    //        userMess = "Select the Member status [current state of the person - active, dormant, invalid, deceased etc.] as applied"
//                //    //    });
//                //    //}
//                //}

              
//                _oChanges.LastMod = DateTime.Now;
//                string uniqueFileName = null;

//                var oFormFile = vmMod.UserPhotoFile;
//                if (oFormFile != null && oFormFile.Length > 0)
//                {
//                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
//                    uniqueFileName = Guid.NewGuid().ToString() + "_" + oFormFile.FileName;
//                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
//                    oFormFile.CopyTo(new FileStream(filePath, FileMode.Create));
//                }

//                else
//                    if (_oChanges.Id != 0) uniqueFileName = _oChanges.UserPhoto;

//                _oChanges.UserPhoto = uniqueFileName;

//                var tm = DateTime.Now;
//                _oChanges.LastMod = tm;
//                _oChanges.LastModByUserId = vmMod.oCurrLoggedUserId;

//                //validate...
//                if (_oChanges.Id == 0)
//                {
//                    _oChanges.Created = tm;
//                    _oChanges.CreatedByUserId = vmMod.oCurrLoggedUserId;
//                    _context.Add(_oChanges);

//                    ViewBag.UserMsg = "Saved user profile successfully.";
//                }
//                else
//                { 
//                    _context.Update(_oChanges);
//                    ViewBag.UserMsg = "User profile updated successfully."; 
//                }


//                // oCM_NewConvert.Created = DateTime.Now;
//               // _context.Add(_oChanges);

//                //save user profile first... 
//                await _context.SaveChangesAsync();


//                //user roles / user groups and/or user permissions [ tick ... pick from the attendance concept]
//                //var oMemRoles = currCtx.MemberChurchRole.Include(t => t.LeaderRole).ThenInclude(t => t.LeaderRoleCategory)
//                //    .Where(c => c.LeaderRole.ChurchBodyId == oChuTransf.FromChurchBodyId && c.ChurchMemberId == oCurrChuMember.Id && c.IsCurrServing == true).ToList();
//                //foreach (var oMR in oMemRoles)
//                //{
//                //    oMR.IsCurrServing = false;
//                //    oMR.Completed = oChuTransf.TransferDate;
//                //    oMR.CompletionReason = oChuTransf.TransferType;
//                //    oMR.LastMod = DateTime.Now;
//                //    //
//                //    currCtx.Update(oMR);
//                //}
//                //ViewBag.UserMsg += " Church visitor added to church as New Convert successfully. Update of member details may however be required."  


//                //save everything
//               // await _context.SaveChangesAsync();


//                TempData["oVmCurrMod"]= vmMod;
//                TempData.Keep(); 
                
//                //if (_oChanges.PwdExpr != null)
//                //{
//                //    if (_oChanges.PwdExpr.Value >= DateTime.Now.Date)
//                //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please user password has expired. Check email/phone for confirm code to activate password.", signOutToLogIn = true  });
//                //}
                
//                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = false   });
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving user profile details. Err: " + ex.Message, signOutToLogIn = false });
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_ChangePwd(ResetUserProfilePwdVM vmMod)
//        {
//            UserProfile _oChanges = vmMod.oUserProfile;
//            vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as ResetUserProfilePwdVM : vmMod; TempData.Keep();

//            var oCV = vmMod.oUserProfile;
//            oCV.ChurchBody = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oUserProfile.AppGlobalOwnerId");
//                ModelState.Remove("oUserProfile.ChurchBodyId");
//                ModelState.Remove("oUserProfile.ChurchMemberId");
//                ModelState.Remove("oUserProfile.CreatedByUserId");
//                ModelState.Remove("oUserProfile.LastModByUserId");
//                ModelState.Remove("oUserProfile.OwnerId");

//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", signOutToLogIn = false });


//                //var tm = DateTime.Now;
//                //_oChanges.LastMod = tm;
//                //_oChanges.LastModByUserId = vmMod.oCurrLoggedUserId;

//                if (oCV.ChurchBody != null)
//                {
//                    var userProList = (from t_upx in _context.UserProfile.Where(c => c.AppGlobalOwnerId== vmMod.oAppGlolOwnId && c.ChurchBodyId == vmMod.oChurchBodyId && 
//                                                                                    c.ProfileScope == _oChanges.ProfileScope && c.Id==oCV.Id)
//                                     //  from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.UserProfileId == t_upx.Id).DefaultIfEmpty()
//                                     //  from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.Id == t_upr.UserRoleId && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN").DefaultIfEmpty()
//                                       select t_upx
//                                 ).OrderBy(c => c.UserDesc).ToList();

//                    if (userProList.Count == 0)
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "User account was not found. Please rfresh and try again.", signOutToLogIn = false });

//                    if (_oChanges.Expr != null)
//                    {
//                        if (_oChanges.Expr.Value >= DateTime.Now.Date)
//                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please user account has expired. Activate account first.", signOutToLogIn = false });
//                    }
//                    if (_oChanges.Pwd != null )
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide user password (minimum 6-digit, upper and lower cases, special character)", signOutToLogIn = false });

//                    if (_oChanges.Pwd != _oChanges.ConfirmPwd)
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Password mismatch. Provide user password again.", signOutToLogIn = false  });

//                    if (vmMod.AuthTypeUsed == null)
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please indicate authentication type to confirm user profile.", signOutToLogIn = false });

//                    if (vmMod.AuthTypeUsed == 1)  //2-way
//                    {
//                        if (vmMod.VerificationCode != "12345678")
//                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Enter correct verification code.", signOutToLogIn = false });
//                    }
//                    else
//                    {
//                        var _secAns = AppUtilties.ComputeSha256Hash(_oChanges.PwdSecurityQue + _oChanges.PwdSecurityAns);
//                        if (vmMod.SecurityQue.ToLower().Equals(_oChanges.PwdSecurityQue.ToLower()) && vmMod.SecurityAns.Equals(_secAns))
//                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Security answer provided is not correct.", signOutToLogIn = false });
//                    }
//                }

//                //create user and init...
//                _oChanges = new UserProfile();
//                //_oChanges.AppGlobalOwnerId = null; // oCV.ChurchBody != null ? oCV.ChurchBody.AppGlobalOwnerId : null;
//                //_oChanges.ChurchBodyId = null; //(int)oCV.ChurchBody.Id;
//                //_oChanges.OwnerId =null; // (int)vmMod.oCurrLoggedUserId;

//                var tm = DateTime.Now;
//                _oChanges.Strt = tm;
//                //_oChanges.Expr = null; // tm.AddDays(90);  //default to 30 days
//                //  oCurrVmMod.oUserProfile.UserId = oCurrChuMemberId_LogOn;
//                //_oChanges.ChurchMemberId = null; // vmMod.oCurrLoggedMemberId;
//               // _oChanges.UserScope = "E"; // I-internal, E-external
//                //_oChanges.ProfileScope = "V"; // V-Vendor, C-Client

//                _oChanges.ResetPwdOnNextLogOn = false;
//                _oChanges.PwdExpr = tm.AddDays(30);  //default to 90 days 
//                _oChanges.UserStatus = "A"; // A-ctive...D-eactive

//               // _oChanges.Created = tm;
//                _oChanges.LastMod = tm; ;
//              //  _oChanges.CreatedByUserId = null; // (int)vmMod.oCurrLoggedUserId;
//                _oChanges.LastModByUserId = null; // (int)vmMod.oCurrLoggedUserId;
                 
//                _oChanges.Pwd = AppUtilties.ComputeSha256Hash(_oChanges.Username + _oChanges.Pwd);
//                //_oChanges.UserDesc = "Super Admin";
//                //_oChanges.UserPhoto = null;
//                //_oChanges.UserId = null;
//                //_oChanges.PhoneNum = null;
//                //_oChanges.Email = null; 

//                // 
//                ViewBag.UserMsg = "Password changed successfully.";

//                //save everything
//                await _context.SaveChangesAsync();
//                TempData["oVmCurrMod"] = vmMod;
//                TempData.Keep();
//                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = true });
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving user profile details. Err: " + ex.Message, signOutToLogIn = false});
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult AddOrEdit_SYS(UserProfileVM vmMod, string churchCode)
//        { 
//            try
//            {
//                //UserProfile _oChanges = vmMod.oUserProfile;
//                // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileVM : vmMod; TempData.Keep();

//                var _churchCode = churchCode;

//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = _churchCode, userMess = "Failed to create requested user. Please refresh and try again.", signOutToLogIn = false });


//                //var tm = DateTime.Now;
//                //_oChanges.LastMod = tm;
//                //_oChanges.LastModByUserId = vmMod.oCurrLoggedUserId;

//                if (!string.IsNullOrEmpty(_churchCode))
//                {
//                    var userProList = (from t_upx in _context.UserProfile.Where(c => _churchCode == "000000" && c.ChurchBodyId == null && c.ProfileScope == "V" && c.UserStatus == "A")
//                                       from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == null && c.UserProfileId == t_upx.Id && c.ProfileRoleStatus == "A").DefaultIfEmpty()
//                                       from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId && c.RoleStatus == "A" && c.RoleLevel == 0 && c.RoleType == "SYS").DefaultIfEmpty()
//                                       select t_upx
//                                 ).OrderBy(c => c.UserDesc).ToList();

//                    if (userProList.Count > 0)
//                        return Json(new { taskSuccess = false, oCurrId = _churchCode, userMess = "SYS account profile already created. There could only be one SYS account.", signOutToLogIn = false });

//                }


//                //create user and init...
//                var _oChanges = new UserProfile();

//                //_oChanges.AppGlobalOwnerId = null; // oCV.ChurchBody != null ? oCV.ChurchBody.AppGlobalOwnerId : null;
//                //_oChanges.ChurchBodyId = null; //(int)oCV.ChurchBody.Id;
//                //_oChanges.OwnerId =null; // (int)vmMod.oCurrLoggedUserId;

//                var tm = DateTime.Now;
//                _oChanges.Strt = tm;
//                // ChurchBody == null

//                //_oChanges.Expr = null; // tm.AddDays(90);  //default to 30 days
//                //  oCurrVmMod.oUserProfile.UserId = oCurrChuMemberId_LogOn;
//                //_oChanges.ChurchMemberId = null; // vmMod.oCurrLoggedMemberId;

//                _oChanges.UserScope = "E"; // I-internal, E-external
//                _oChanges.ProfileScope = "V"; // V-Vendor, C-Client
//                _oChanges.ResetPwdOnNextLogOn = true;
//                _oChanges.PwdSecurityQue = "What account is this?";
//                _oChanges.PwdSecurityAns = "Rhema-SYS";
//                _oChanges.Email = "samuel@rhema-systems.com";
//               // _oChanges.PhoneNum = "233242188212";
//                _oChanges.UserDesc = "Sys Profile";
                
//                var cc = "000000"; _oChanges.Username = "Sys"; _oChanges.Pwd = "654321"; // [ get the raw data instead ]
//                _oChanges.Pwd = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username + _oChanges.Pwd);

//                _oChanges.PwdExpr = tm.AddDays(30);  //default to 90 days 
//                _oChanges.UserStatus = "A"; // A-ctive...D-eactive

//                _oChanges.Created = tm;
//                _oChanges.LastMod = tm; ;
//                _oChanges.CreatedByUserId = null; // (int)vmMod.oCurrLoggedUserId;
//                _oChanges.LastModByUserId = null; // (int)vmMod.oCurrLoggedUserId;
                               
//                //_oChanges.UserPhoto = null;
//                //_oChanges.UserId = null;
//                //_oChanges.PhoneNum = null;
//                //_oChanges.Email = null; 

//                // 
//                ViewBag.UserMsg = "Saved SYS account profile successfully. Sign-out and then sign-in to create Super Admin profile to perform the required settings /client configurations.";

//                _context.Add(_oChanges);
//                //save everything
//                _context.SaveChanges();

//                // TempData["oVmCurrMod"] = vmMod;
//                // TempData.Keep();
//                // return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = true });


//                //succesful...  login required
//                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = true });
//                // return RedirectToAction("LoginUserAcc", "UserLogin");
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = churchCode, userMess = "Failed saving SYS account profile. Err: " + ex.Message, signOutToLogIn = false });
//            }

//        }

//        public async Task<IActionResult> Delete_UP(int? id)
//        {
//            var res = false;
//            var UserProfile = await _context.UserProfile.FindAsync(id);
//            if (UserProfile != null)
//            {
//                //check all member related modules for references to deny deletion

//                res = true;
//                if (res)
//                {
//                    _context.UserProfile.Remove(UserProfile);
//                    await _context.SaveChangesAsync();
//                }
//                else
//                {
//                    ModelState.AddModelError(UserProfile.Username , "Delete failed. User Profile data is referenced elsewhere in the Application.");
//                    ViewBag.UserDelMsg = "Delete failed. User Profile data is referenced elsewhere in the Application.";
//                }

//                return Json(res);
//            }

//            else
//                return Json(false);
//        }




//    }
//}
