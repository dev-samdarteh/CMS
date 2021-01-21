using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RhemaCMS.Controllers.con_adhc;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
// using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using RhemaCMS.Models.ViewModels;
using static RhemaCMS.Controllers.con_adhc.AppUtilties;
//using static RhemaCMS.Models.ViewModels.AppVenAdminVM;

namespace RhemaCMS.Controllers.con_app_va
{
    public class UserLoginController : Controller
    {
       // private readonly  IConfiguration _configuration;
        private readonly MSTR_DbContext _context;

       //will be initialized at successful login
       // private readonly string _clientDBConnString;
       // private readonly MSTR_DbContext _masterContextLog;
       // private readonly ChurchModelContext _clientDBContext;

        private List<DiscreteLookup> dlUserAuthTypes = new List<DiscreteLookup>();
        
             
        public UserLoginController(MSTR_DbContext context) //, ChurchModelContext clientDBContext) //, IConfiguration configuration)
        {
            // initialize DBs 
            //_configuration = configuration;
            _context = context;

            //_clientDBContext = clientDBContext;
            // _masterContextLog = new MSTR_DbContext();

            // var conn = new SqlConnectionStringBuilder(_context.Database.GetDbConnection().ConnectionString);
            //  "DefaultConnection": "Server=RHEMA-SDARTEH;Database=DBRCMS_MS_DEV;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true"
            // conn.DataSource = "RHEMA-SDARTEH"; conn.InitialCatalog = "DBRCMS_CL_TEST"; conn.UserID = "sa"; conn.Password = "sadmin"; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
            
            //will be initialized at successful login
            // this._clientDBConnString = ""; //conn.ConnectionString;

            //var a = _configuration.GetConnectionString("DefaultConnection");

            //var bb = context.Database.CanConnect();
            //if (context.Database.GetDbConnection().State != System.Data.ConnectionState.Open) 
            //    context.Database.GetDbConnection().Open();
            //context.Database.GetDbConnection().ChangeDatabase("DBRCMS_CL_TEST");
            //var b = context.Database.GetDbConnection().ConnectionString;

            //var c = _configuration.GetConnectionString("DefaultConnection");

            dlUserAuthTypes.Add(new DiscreteLookup() { Category = "UserAuthType", Val = "1", Desc = "Two-way Authentication" });
            dlUserAuthTypes.Add(new DiscreteLookup() { Category = "UserAuthType", Val = "2", Desc = "Security Question Validation" });
            //options => options
        }



        [HttpPost]
        public IActionResult InitializeRCMS()
        {
            /////  DEFAULT USERS....  CHECK AND CREATE --- ONLY FOR EMPTY DATABASE
            ///
            /// 

            //SYS account can only be 1... check if it exists .... verify if add roles, perms or sys user profiles first ... thus for SYS acc only ... once SUP_ADMN created... NEVER execute this code.... Restore to default can be done by SUP_ADMN unless SYS acc > SUP_ADMN acc
            var dbUsers = _context.UserProfile.ToList(); 
            var dbSYSUser = _context.UserProfile.Where(c=>c.Username.ToLower()=="SYS".ToLower() && c.UserStatus=="A" && c.UserKey==ac2).FirstOrDefault(); 
            if (dbUsers.Count() == 0 || (dbUsers.Count > 0 && dbSYSUser == null)) // (checkSYSAccOnlyAndNone <= 1 && AppUtilties.ComputeSha256Hash(model.ChurchCode) == ac1 && AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower()) == ac2 && AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower() + model.Password) == ac3)
            //6-digit vendor code 6-digit code for churches ... [church code: 0000000000 + ?? userid + ?? pwd] + no existing SUPADMIN user ... pop up SUPADMIN for new SupAdmin()
            {
                LogOnVM model = new LogOnVM();

                model.UserProfiles = _context.UserProfile.ToList();
                ViewData["strAppName"] = "RHEMA-CMS";
                model.ChurchCode = "000000"; //vendor code but only in ViewModel -- not in db

              //  var logoutCurrUser = false;
                UserProfile oUser_MSTR = null;

                var _userTask = "Initializing RCMS setup"; var _tm = DateTime.Now;
                _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                     "RCMS System", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));

                try
                { 
                    var tm = DateTime.Now;
                    //if no SYS... create one and only 1... then create other users
                    var userList = (from t_up in _context.UserProfile.Where(c => model.ChurchCode == "000000" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileScope == "V" && c.UserStatus == "A" && c.Username.ToLower() == "SYS".ToLower())   //UserKey
                                    from t_upr in _context.UserProfileRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.UserProfileId == t_up.Id && c.ProfileRoleStatus == "A").DefaultIfEmpty()
                                    from t_ur in _context.UserRole.Where(c => c.AppGlobalOwnerId == null &&  c.ChurchBodyId == null && c.Id == t_upr.UserRoleId && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SYS").DefaultIfEmpty()
                                    select t_up
                                    ).ToList();


                    //SYS acc exists... and more users... sign out
                    // if (userList.Count() > 1) return RedirectToAction("LoginUserAcc", "UserLogin");

                    // create SYS account and login again...
                    var oSYSUser = new UserProfile();
                    if (userList.Count() == 0)
                    {
                        //create the SYS acc...
                        //var oUserVm = new AppVenAdminVM.UserProfileVM();
                        //var upc = new UserProfileController(_context, _clientDBContext, null);
                        //var p = upc.AddOrEdit_SYS(oUserVm, model.ChurchCode) as JsonResult;
                        //var mes = p.Value.ToString();

                        oSYSUser = new UserProfile();                        
                        //oSYSUser.AppGlobalOwnerId = null; // oCV.ChurchBody != null ? oCV.ChurchBody.AppGlobalOwnerId : null; //oSYSUser.ChurchBodyId = null; //(int)oCV.ChurchBody.Id; //oSYSUser.OwnerId =null; // (int)vmMod.oCurrLoggedUserId;

                        oSYSUser.Strt = tm;
                        // ChurchBody == null //oSYSUser.Expr = null; // tm.AddDays(90);  //default to 30 days //  oCurrVmMod.oUserProfile.UserId = oCurrChuMemberId_LogOn;  //oSYSUser.ChurchMemberId = null; // vmMod.oCurrLoggedMemberId;

                        oSYSUser.UserScope = "E"; // I-internal, E-external
                        oSYSUser.ProfileScope = "V"; // V-Vendor, C-Client
                        oSYSUser.ResetPwdOnNextLogOn = false; // true;
                        oSYSUser.PwdSecurityQue = "What account is this?"; oSYSUser.PwdSecurityAns = "RHEMA-SYS";
                        oSYSUser.PwdSecurityAns = AppUtilties.ComputeSha256Hash(oSYSUser.PwdSecurityQue + oSYSUser.PwdSecurityAns);
                        //oSYSUser.Email =  "samuel@rhema-systems.com";  // ???   ... user unknown [ what email to use ? ]
                        // oSYSUser.PhoneNum = "233242188212";   // ???  ... user unknown [ what phone to use ? ]
                        oSYSUser.UserDesc = "SYS Profile";
                    ///
                        var cc = "000000"; 
                        oSYSUser.Username = "SYS"; oSYSUser.Pwd = "654321"; // [ get the raw hashed data instead ]
                        oSYSUser.UserKey = AppUtilties.ComputeSha256Hash(cc + oSYSUser.Username.Trim().ToLower());                            
                        oSYSUser.Pwd = AppUtilties.ComputeSha256Hash(cc + oSYSUser.Username.Trim().ToLower() + oSYSUser.Pwd);

                        oSYSUser.PwdExpr = tm.AddDays(30);  //default to 90 days 
                        oSYSUser.UserStatus = "A"; // A-ctive...D-eactive

                        oSYSUser.Created = tm;
                        oSYSUser.LastMod = tm; ;
                        oSYSUser.CreatedByUserId = null; // (int)vmMod.oCurrLoggedUserId;
                        oSYSUser.LastModByUserId = null; // (int)vmMod.oCurrLoggedUserId;

                        //oSYSUser.UserPhoto = null; //oSYSUser.UserId = null; //oSYSUser.PhoneNum = null; //oSYSUser.Email = null;  
                        _context.Add(oSYSUser);

                        //save everything
                        _context.SaveChanges();
                    ///
                    _userTask = "Created SYS control profile"; _tm = DateTime.Now;
                    _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                            "RCMS User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));

                    //  logoutCurrUser = true;
                    }
                    else
                    {
                        oSYSUser = userList[0];
                    }


                    // string strRootCode = AppUtilties.ComputeSha256Hash(model.ChurchCode); //church code ...0x6... 91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203
                    //  string _strRootCode = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username);  // user  ...0x6... f7b11509f4d675c3c44f0dd37ca830bb02e8cfa58f04c46283c4bfcbdce1ff45
                    //  string strRootCode0 = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username + model.Password);  // pwd ...$0x6... 78415a1535ca0ef885aa7c0278a4de274b85d0c139932cc138ba6ee5cac4a00b

                    //create the user permissions
                    var oUtil = new AppUtilties();
                    var permList = oUtil.GetSystem_Administration_Permissions();
                    var permList1 = oUtil.GetAppDashboard_Permissions();
                    var permList2 = oUtil.GetAppConfigurations_Permissions();
                    var permList3 = oUtil.GetMemberRegister_Permissions();
                    var permList4 = oUtil.GetChurchlifeAndEvents_Permissions();
                    var permList5 = oUtil.GetChurchAdministration_Permissions();
                    var permList6 = oUtil.GetFinanceManagement_Permissions();
                    var permList7 = oUtil.GetReportsAnalytics_Permissions();

                    //var permList3 = oUtil.get();
                    permList = AppUtilties.CombineCollection(permList, permList1, permList2, permList3, permList4);
                    permList = AppUtilties.CombineCollection(permList, permList5, permList6, permList7);
                    // 
                    var _permChanges = 0;
                    for (var i = 0; i < permList.Count; i++)
                    {
                        var checkExist = _context.UserPermission.Where(c => c.PermissionName.ToLower().Equals(permList[i].PermissionName.ToLower())).FirstOrDefault();
                        if (checkExist == null)
                        {
                            _context.Add(new UserPermission()
                            {
                                PermissionName = permList[i].PermissionName,
                                // UserPermCategoryId = null,
                                PermissionCode = AppUtilties.GetPermissionCode_FromName(permList[i].PermissionName),
                                PermStatus = "A",
                                Created = tm,
                                LastMod = tm,
                                CreatedByUserId = oSYSUser.Id,
                                LastModByUserId = oSYSUser.Id
                            });

                            _permChanges++;
                        }
                    }

                    // logoutCurrUser = _permChanges > 0;
                    if (_permChanges > 0)
                    {
                        _context.SaveChanges();
                        ///
                        _userTask = "Created " + _permChanges + " default user permissions"; _tm = DateTime.Now;
                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                             "RCMS User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));
                    }


                    //create the user roles 
                    var roleList = oUtil.GetSystemDefaultRoles();

                    var _roleChanges = 0;
                    for (var i = 0; i < roleList.Count; i++)
                    {
                        var checkExist = _context.UserRole.Where(c => c.RoleName.ToLower().Equals(roleList[i].RoleName.ToLower())).FirstOrDefault();
                        if (checkExist == null)
                        {
                            _context.Add(new UserRole()
                            {
                                ChurchBodyId = null,
                                RoleName = roleList[i].RoleName,
                                RoleType = roleList[i].RoleType,
                                RoleDesc = roleList[i].RoleDesc,
                                RoleLevel = roleList[i].RoleLevel,
                                RoleStatus = "A",
                                Created = tm,
                                LastMod = tm,
                                CreatedByUserId = oSYSUser.Id,
                                LastModByUserId = oSYSUser.Id
                            });

                            _roleChanges++;
                        }
                    }

                    // logoutCurrUser = _roleChanges > 0;
                    if (_roleChanges > 0)
                    {
                        _context.SaveChanges();
                        ///
                        _userTask = "Created the default SYS role"; _tm = DateTime.Now;
                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                             "RCMS User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));
                    }


                    //create user groups
                     


                    // add perms to new role (s) created.. SYS role - SUP_ADMN perm  :: lsPerms.Add(new UserPermission(0, null, "A0_00", "_A0_00__Super_Admin_Account", null, "A", null, null, null, null, null));   // for SYS account only

                    var userRole = _context.UserRole.Where(c => c.RoleName == "SYS" && c.RoleStatus == "A").FirstOrDefault();
                    var userPerm = _context.UserPermission.Where(c => c.PermissionCode == "A0_00" && c.PermStatus == "A").FirstOrDefault();
                    var _URPChanges = 0;
                    if (userRole != null && userPerm != null)
                    {
                        //for (var i = 0; i < permList.Count; i++)
                        //{
                        var checkURPExist = _context.UserRolePermission.Where(c => c.UserPermissionId == userPerm.Id && c.UserRoleId == userRole.Id && c.Status == "A").FirstOrDefault();
                        if (checkURPExist == null)
                        {
                            _context.Add(new UserRolePermission()
                            {
                                ChurchBodyId = null,
                                UserPermissionId = userPerm.Id,
                                UserRoleId = userRole.Id,
                                Status = "A",
                                ViewPerm = true,
                                CreatePerm = true,
                                EditPerm = true,
                                DeletePerm = true,
                                ManagePerm = true,
                                Created = tm,
                                LastMod = tm,
                                CreatedByUserId = oSYSUser.Id,
                                LastModByUserId = oSYSUser.Id
                            });

                            _URPChanges++;
                        }
                    }
                    //}

                    // logoutCurrUser = _URPChanges > 0;
                    if (_URPChanges > 0)
                    { 
                     _context.SaveChanges();
                     ///
                     _userTask = "Assigned " + _URPChanges + " permission(s) to default SYS role"; _tm = DateTime.Now;
                     _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                             "RCMS User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));
                    }
                   

                    // add role (s) for the new acc created.. SYS acc - SYS role - SYS permission : lsPerms.Add(new UserPermission(0, null, "A0_00", "_A0_00__Super_Admin_Account", null, "A", null, null, null, null, null));   // for SYS account only
                    string strUserKeyHashedData = AppUtilties.ComputeSha256Hash("000000" + "SYS".Trim().ToLower()); 
                    var userProfile = _context.UserProfile.Where(c => c.AppGlobalOwnerId==null && c.ChurchBodyId==null && c.UserStatus == "A" && 
                                                            c.Username.Trim().ToLower() == "SYS".Trim().ToLower() && c.UserKey==strUserKeyHashedData ).FirstOrDefault(); 
                    ///
                    oUser_MSTR = userProfile;

                    //var userRole = _context.UserRole.Where(c => c.RoleName == "SYS" && c.RoleStatus == "A").FirstOrDefault();
                    var _UPRChanges = 0;
                    if (userRole != null && userProfile != null)
                    {
                        //for (var i = 0; i < permList.Count; i++)
                        //{
                        var checkUPRExist = _context.UserProfileRole.Where(c => c.UserProfileId == userProfile.Id && c.UserRoleId == userRole.Id && c.ProfileRoleStatus == "A").FirstOrDefault();
                        if (checkUPRExist == null)
                        {
                            _context.Add(new UserProfileRole()
                            {
                                ChurchBodyId = null,
                                UserProfileId = userProfile.Id,
                                UserRoleId = userRole.Id,
                                ProfileRoleStatus = "A",
                                Strt = tm,
                                Expr = null,
                                Created = tm,
                                LastMod = tm,
                                CreatedByUserId = oSYSUser.Id,
                                LastModByUserId = oSYSUser.Id
                            });

                            _UPRChanges++;
                        }
                    }
                    //}

                    //  logoutCurrUser = _UPRChanges > 0;
                    if (_UPRChanges > 0)
                    { 
                        _context.SaveChanges();
                        ///
                        _userTask = "Assigned " + _UPRChanges + " role(s) to SYS profile"; _tm = DateTime.Now;
                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                                "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));
                    }


                    // initialize the COUNTRY
                    var countriesList = AppUtilties.GetMS_Countries();
                    var _ctryCount = 0;
                    if (countriesList.Count > 0)
                    {
                        foreach (var oCtry in countriesList)
                        {
                            var checkCTRYExist = _context.MSTRCountry.Where(c => c.CtryAlpha3Code == oCtry.CtryAlpha3Code ).FirstOrDefault();
                            if (checkCTRYExist == null)
                            {
                                _context.Add(new MSTRCountry()
                                {
                                    CtryAlpha3Code = oCtry.CtryAlpha3Code,  //key
                                    EngName = oCtry.EngName,                                   
                                    CtryAlpha2Code = oCtry.CtryAlpha2Code,
                                    CurrEngName = oCtry.CurrEngName,
                                    CurrLocName = oCtry.CurrLocName,
                                    CurrSymbol = oCtry.CurrSymbol,
                                    Curr3LISOSymbol = oCtry.Curr3LISOSymbol,
                                  //  SharingStatus = "N", 
                                    Created = tm,
                                    LastMod = tm,
                                    CreatedByUserId = oSYSUser.Id,
                                    LastModByUserId = oSYSUser.Id
                                });

                                _ctryCount++;
                            }
                        }
                    }

                  
                    if (_ctryCount > 0)
                    {
                        _context.SaveChanges();
                        ///
                        _userTask = "Created " + _ctryCount + " countries."; _tm = DateTime.Now;
                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                                "RCMS-Admin: Country", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));
                    }


                    ////logout ...login to authenticate
                    //if (logoutCurrUser == true) return RedirectToAction("LoginUserAcc", "UserLogin");
                }

                catch (Exception ex)
                {
                    throw;
                }
            }

            return RedirectToAction("LoginUserAcc", "UserLogin");
        }



        [HttpGet]
        public IActionResult LoginUserAcc()
        {
            // SHOULD BE ON THE PAGE directly.... < modify this later > clear the cache, stored logging details... @refresh... redirect to login page
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //
            HttpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Response.Headers.Add("Pragma", "no-cache");
            HttpContext.Response.Headers.Add("Expires", "0");
            HttpContext.Session.Clear();
            
            // HttpContext.Response.Cookies.Delete();
            ///
            TempData.Clear();            
            
            ///
            LogOnVM model = new LogOnVM();
                        
            ViewData["strAppName"] = "RHEMA-CMS";

            /////  DEFAULT USERS....  CHECK AND CREATE --- ONLY FOR EMPTY DATABASE
            /// 
            
            var isDBUserNull = _context.UserProfile.Count() == 0;
            if (isDBUserNull)
            {
               return InitializeRCMS();
                // perform  --->>>>  DEFAULT USERS and ROLES
                // return RedirectToAction("InitializeRCMS"); 
            }
             
           else
            {
                model.UserProfiles = _context.UserProfile.ToList();
                ViewData["VerificationCodeEnabled"] = false;
                return View(model);
            }              
        }

        private const string ac1 = "91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203"; // ChurchCode =  encrypt(cc)  //"91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203";
        private const string ac2 = "d38e8e28f06fbd35e89e67ea132da62c976af6dff36e02877d2236b6a12961ca"; // UserKey = encrypt(cc + usr) //"f7b11509f4d675c3c44f0dd37ca830bb02e8cfa58f04c46283c4bfcbdce1ff45";
        private const string ac3 = "10c16e2d260b87e96096c18991b57d9233453ae4eb3125ed0e34ecde2af3fa36"; // User Pwd = encrypt(cc + usr + pwd) "78415a1535ca0ef885aa7c0278a4de274b85d0c139932cc138ba6ee5cac4a00b"; check thru codes


        [HttpPost]
        [ValidateAntiForgeryToken]
        //  public  ActionResult LoginUserAcc([Bind(include: "Username,Pwd")] UserProfile userProfile) 
        public ActionResult LoginUserAcc(LogOnVM model, string strIsValidated = "F", bool navBack = false) //, string returnUrl)
        {
            //clear the model state first before adding ... any more error
            //if (ModelState.ContainsKey("")) ModelState[""].Errors.Clear();
            //ModelState[""].ValidationState = ModelValidationState.Valid;

            //foreach (var key in ModelState.Keys)
            //{
            //    ModelState[key].Errors.Clear();
            //    ModelState[key].ValidationState = ModelValidationState.Valid;
            //}



            //if (strIsValidated == null) strIsValidated = "F";
            ViewData["VerificationCodeEnabled"] = false;

            var _userTask = "User attempt login to RCMS initiated"; 
            var _tm = DateTime.Now;
            _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "L",
                                     "RCMS Login"  , AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));
            //

            if (navBack)
            {
                model.IsValidated = "F"; 
                return View(model);
            }

            if (model == null)
            {
                model.IsValidated = "F";
                ModelState.AddModelError("", "Failed to retrieve data. Please try again.");
                return View(model);
            }

            var err = false;
            if (model.ChurchCode == null) { err = true; ModelState.AddModelError("", "Enter Church code"); }
            if (model.Username == null) { err = true; ModelState.AddModelError("", "Enter username."); }
            if (model.Password == null) { err = true; ModelState.AddModelError("", "Enter user password."); }

            if (err==true)
            {
                model.IsValidated = "F";                
                return View(model);
            }

            model.IsValidated = strIsValidated == null ? "F" : strIsValidated;
                   

            //Id,ChurchBodyId,OwnerId,UserDesc,Username,Pwd,Email,PhoneNum,STRT,EXPR,UserStatus,Created,LastMod
            //ModelState.Remove("AppGlobalOwnerId");

            ModelState.Remove("ChurchBodyId");
            ModelState.Remove("Email");
            ModelState.Remove("PhoneNum");
            //foreach (var key in ModelState.Keys)
            //{
            //    ModelState.Remove(key);
            //    // ModelState.Remove("BasicCredentialsVerified");
            //    // ModelState[key].Errors.Clear();
            //}

            //   ModelState.Remove("BasicCredentialsVerified");
            ModelState.Remove("IsBasicVerified");

            //ViewBag.VerificationCodeEnabled = false;

            //  bool valid = false;
            var isUserValidated = false;
            List<UserSessionPrivilege> oUserPrivilegeCol = null;

            // UserProfile oUser_MSTR = null;

            if ((model.IsValidated != "T" && ModelState.IsValid) || (!ModelState.IsValid)) //model.Username == null || model.Password == null))
            //((model.BasicCredentialsVerified) || (model.BasicCredentialsVerified == false && ModelState.IsValid))
            {
                var _cc = "000000";   //"10c16e2d260b87e96096c18991b57d9233453ae4eb3125ed0e34ecde2af3fa36"
                string _ac1 = AppUtilties.ComputeSha256Hash(_cc);
                string _ac2 = AppUtilties.ComputeSha256Hash(_cc + model.Username.ToLower());
                string _ac3 = AppUtilties.ComputeSha256Hash(_cc + model.Username.ToLower() + model.Password);

                //string _strPwdHashedData1 = AppUtilties.ComputeSha256Hash(model.Username.Trim().ToLower() + model.Password);

                //string strRootCode = AppUtilties.ComputeSha256Hash(model.ChurchCode); //church code ... 91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203
                //string _strRootCode = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower());  // user  ... f7b11509f4d675c3c44f0dd37ca830bb02e8cfa58f04c46283c4bfcbdce1ff45
                //string strRootCode0 = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower() + model.Password);  // pwd ... 78415a1535ca0ef885aa7c0278a4de274b85d0c139932cc138ba6ee5cac4a00b

                // string strRootCode1 = AppUtilties.ComputeSha256Hash(model.Username + "$rhemacloud"); // lotto scam: 0557 58 38 46
                // string strRootCode = AppUtilties.ComputeSha256Hash(model.Username + "RHEMA_SYS1");

                //  string strRootCode1 = AppUtilties.ComputeSha256Hash("danwool" + "$rhemacloud");
                // string strRootCode1 = AppUtilties.ComputeSha256Hash("joe" + "$rhemacloud");
                //  string strRootCode = AppUtilties.ComputeSha256Hash(model.Username + "RHEMA_Sup_Admn1");
                //string strRootCode3 = AppUtilties.ComputeSha256Hash("joe" + "RHEMA_Sup_Admn1");
                //string strRootCode4 = AppUtilties.ComputeSha256Hash("dabrokwah" + "RHEMA_Sup_Admn1");
                //string strRootCode5 = AppUtilties.ComputeSha256Hash("test" + "RHEMA_Sup_Admn1");
                //string strRootCode6 = AppUtilties.ComputeSha256Hash("test" + "$rhemacloud");

                // bool userValidated = model.IsBasicVerified == "T";
                // if user is NOT valiadated...  ---- >>> validate

                
                if (model.IsValidated != "T")
                {
                   // var logoutCurrUser = false;

                    UserProfile oUser_MSTR = null;

                    //SYS account can only be 1... check if it exists .... verify if add roles, perms or sys user profiles first ... thus for SYS acc only ... once SUP_ADMN created... NEVER execute this code.... Restore to default can be done by SUP_ADMN unless SYS acc > SUP_ADMN acc
                    //6-digit vendor code 6-digit code for churches ... [church code: 0000000000 + ?? userid + ?? pwd] + no existing SUPADMIN user ... pop up SUPADMIN for new SupAdmin()
                    var checkSYSAcc_Count = _context.UserProfile.Count();
                    if (checkSYSAcc_Count == 0 && // no user created yet; IntializeSetup should be called by now
                        AppUtilties.ComputeSha256Hash(model.ChurchCode) == ac1 && // church code
                        AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower()) == ac2 && // user key
                        AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower() + model.Password) == ac3) // pwd
                    {
                        InitializeRCMS();

                        //try
                        //{
                        //    // string strRootCode = AppUtilties.ComputeSha256Hash(model.ChurchCode); //church code ...0x6... 91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203
                        //    //  string _strRootCode = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username);  // user  ...0x6... f7b11509f4d675c3c44f0dd37ca830bb02e8cfa58f04c46283c4bfcbdce1ff45
                        //    //  string strRootCode0 = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username + model.Password);  // pwd ...$0x6... 78415a1535ca0ef885aa7c0278a4de274b85d0c139932cc138ba6ee5cac4a00b

                        //    //create the user permissions
                        //    var oUtil = new AppUtilties();
                        //    var permList = oUtil.GetSystem_Administration_Permissions();
                        //    var permList1 = oUtil.GetAppDashboard_Permissions();
                        //    var permList2 = oUtil.GetAppConfigurations_Permissions();
                        //    var permList3 = oUtil.GetMemberRegister_Permissions();
                        //    var permList4 = oUtil.GetChurchlifeAndEvents_Permissions();
                        //    var permList5 = oUtil.GetChurchAdministration_Permissions();
                        //    var permList6 = oUtil.GetFinanceManagement_Permissions();
                        //    var permList7 = oUtil.GetReportsAnalytics_Permissions();

                        //    //var permList3 = oUtil.get();
                        //    permList = AppUtilties.CombineCollection(permList, permList1, permList2, permList3, permList4);
                        //    permList = AppUtilties.CombineCollection(permList, permList5, permList6, permList7);
                        //    //
                        //    var tm = DateTime.Now;
                        //    var _permChanges = 0;
                        //    for (var i = 0; i < permList.Count; i++)
                        //    {
                        //        var checkExist = _context.UserPermission.Where(c => c.PermissionName.ToLower().Equals(permList[i].PermissionName.ToLower())).FirstOrDefault();
                        //        if (checkExist == null)
                        //        {
                        //            _context.Add(new UserPermission()
                        //            { 
                        //                PermissionName = permList[i].PermissionName,
                        //                PermissionCode = AppUtilties.GetPermissionCode_FromName(permList[i].PermissionName),
                        //                PermStatus = "A",
                        //                Created = tm,
                        //                LastMod = tm,
                        //                CreatedByUserId = null,
                        //                LastModByUserId = null
                        //            });

                        //            _permChanges++;
                        //        }
                        //    }

                        //    logoutCurrUser = _permChanges > 0;
                        //    if (_permChanges > 0) _context.SaveChanges();


                        //    //create the user roles 
                        //    var roleList = oUtil.GetSystemDefaultRoles();

                        //    var _roleChanges = 0;
                        //    for (var i = 0; i < roleList.Count; i++)
                        //    {
                        //        var checkExist = _context.UserRole.Where(c => c.RoleName.ToLower().Equals(roleList[i].RoleName.ToLower())).FirstOrDefault();
                        //        if (checkExist == null)
                        //        {
                        //            _context.Add(new UserRole()
                        //            {
                        //                ChurchBodyId = null,
                        //                RoleName = roleList[i].RoleName,
                        //                RoleType = roleList[i].RoleType,
                        //                RoleDesc = roleList[i].RoleDesc,
                        //                RoleLevel = roleList[i].RoleLevel,
                        //                RoleStatus = "A",
                        //                Created = tm,
                        //                LastMod = tm,
                        //                CreatedByUserId = null,
                        //                LastModByUserId = null
                        //            });

                        //            _roleChanges++;
                        //        }
                        //    }

                        //    logoutCurrUser = _roleChanges > 0;
                        //    if (_roleChanges > 0) _context.SaveChanges();


                        //    //create user groups


                        //    //if no SYS... create one and only 1... then create other users
                        //    var userList = (from t_up in _context.UserProfile.Where(c => model.ChurchCode == "000000" && c.ChurchBodyId == null && c.ProfileScope == "V" && c.UserStatus == "A")
                        //                    from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id && c.ProfileRoleStatus == "A").DefaultIfEmpty()
                        //                    from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId && c.RoleStatus == "A" && c.RoleLevel == 0 && c.RoleType == "SYS").DefaultIfEmpty()
                        //                    select t_up
                        //                    ).ToList();


                        //    //SYS acc exists... and more users... sign out
                        //    if (userList.Count > 1) return RedirectToAction("LoginUserAcc", "UserLogin");
                            
                        //    // create SYS account and login again...
                        //    if (userList.Count == 0)
                        //    {
                        //        //create the SYS acc...
                        //        //var oUserVm = new AppVenAdminVM.UserProfileVM();
                        //        //var upc = new UserProfileController(_context, _clientDBContext, null);
                        //        //var p = upc.AddOrEdit_SYS(oUserVm, model.ChurchCode) as JsonResult;
                        //        //var mes = p.Value.ToString();

                        //        var _oChanges = new UserProfile();

                        //        //_oChanges.AppGlobalOwnerId = null; // oCV.ChurchBody != null ? oCV.ChurchBody.AppGlobalOwnerId : null; //_oChanges.ChurchBodyId = null; //(int)oCV.ChurchBody.Id; //_oChanges.OwnerId =null; // (int)vmMod.oCurrLoggedUserId;
                                 
                        //        _oChanges.Strt = tm;
                        //        // ChurchBody == null //_oChanges.Expr = null; // tm.AddDays(90);  //default to 30 days //  oCurrVmMod.oUserProfile.UserId = oCurrChuMemberId_LogOn;  //_oChanges.ChurchMemberId = null; // vmMod.oCurrLoggedMemberId;

                        //        _oChanges.UserScope = "I"; // I-internal, E-external
                        //        _oChanges.ProfileScope = "V"; // V-Vendor, C-Client
                        //        _oChanges.ResetPwdOnNextLogOn = false; // true;
                        //        _oChanges.PwdSecurityQue = "What account is this?"; _oChanges.PwdSecurityAns = "Rhema-SYS";
                        //        _oChanges.PwdSecurityAns = AppUtilties.ComputeSha256Hash(_oChanges.PwdSecurityQue + _oChanges.PwdSecurityAns);
                        //        //_oChanges.Email =  "samuel@rhema-systems.com";  // ???   ... user unknown [ what email to use ? ]
                        //       // _oChanges.PhoneNum = "233242188212";   // ???  ... user unknown [ what phone to use ? ]
                        //        _oChanges.UserDesc = "Sys Profile";

                        //        var cc = "000000"; _oChanges.Username = "Sys"; _oChanges.Pwd = "654321"; // [ get the raw data instead ]
                        //        _oChanges.Pwd = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username.Trim().ToLower() + _oChanges.Pwd);

                        //        _oChanges.PwdExpr = tm.AddDays(30);  //default to 90 days 
                        //        _oChanges.UserStatus = "A"; // A-ctive...D-eactive

                        //        _oChanges.Created = tm;
                        //        _oChanges.LastMod = tm; ;
                        //        _oChanges.CreatedByUserId = null; // (int)vmMod.oCurrLoggedUserId;
                        //        _oChanges.LastModByUserId = null; // (int)vmMod.oCurrLoggedUserId;

                        //        //_oChanges.UserPhoto = null; //_oChanges.UserId = null; //_oChanges.PhoneNum = null; //_oChanges.Email = null;  
                        //        _context.Add(_oChanges);

                        //        //save everything
                        //        _context.SaveChanges();
                        //        logoutCurrUser = true ;
                        //    }

                        //    // add perms to new role (s) created.. SYS role - SUP_ADMN perm  :: lsPerms.Add(new UserPermission(0, null, "A0_00", "_A0_00__Super_Admin_Account", null, "A", null, null, null, null, null));   // for SYS account only

                        //    var userRole = _context.UserRole.Where(c => c.RoleName == "SYS" && c.RoleStatus == "A").FirstOrDefault();
                        //    var userPerm = _context.UserPermission.Where(c => c.PermissionCode == "A0_00" && c.PermStatus == "A").FirstOrDefault();
                        //    var _URPChanges = 0;
                        //    if (userRole != null && userPerm != null)
                        //    {
                        //        //for (var i = 0; i < permList.Count; i++)
                        //        //{
                        //        var checkURPExist = _context.UserRolePermission.Where(c => c.UserPermissionId == userPerm.Id && c.UserRoleId == userRole.Id && c.Status == "A").FirstOrDefault();
                        //        if (checkURPExist == null)
                        //        {
                        //            _context.Add(new UserRolePermission()
                        //            {
                        //                ChurchBodyId = null,
                        //                UserPermissionId = userPerm.Id,
                        //                UserRoleId = userRole.Id,
                        //                Status = "A",
                        //                ViewPerm = true,
                        //                CreatePerm = true,
                        //                EditPerm = true,
                        //                DeletePerm = true,
                        //                ManagePerm = true,
                        //                Created = tm,
                        //                LastMod = tm,
                        //                CreatedByUserId = null,
                        //                LastModByUserId = null
                        //            });

                        //            _URPChanges++;
                        //        }
                        //    }
                        //    //}

                        //    logoutCurrUser = _URPChanges > 0;
                        //    if (_URPChanges > 0) _context.SaveChanges();

                        //    // add role (s) for the new acc created.. SYS acc - SYS role - SYS permission : lsPerms.Add(new UserPermission(0, null, "A0_00", "_A0_00__Super_Admin_Account", null, "A", null, null, null, null, null));   // for SYS account only
                        //    var userProfile = _context.UserProfile.Where(c => c.Username.Trim().ToLower() == "SYS".Trim().ToLower() && c.UserStatus == "A").FirstOrDefault();
                        //    oUser_MSTR = userProfile;
                        //    //var userRole = _context.UserRole.Where(c => c.RoleName == "SYS" && c.RoleStatus == "A").FirstOrDefault();
                        //    var _UPRChanges = 0;
                        //    if (userRole != null && userProfile != null)
                        //    {
                        //        //for (var i = 0; i < permList.Count; i++)
                        //        //{
                        //        var checkUPRExist = _context.UserProfileRole.Where(c => c.UserProfileId == userProfile.Id && c.UserRoleId == userRole.Id && c.ProfileRoleStatus == "A").FirstOrDefault();
                        //        if (checkUPRExist == null)
                        //        {
                        //            _context.Add(new UserProfileRole()
                        //            {
                        //                ChurchBodyId = null,
                        //                UserProfileId = userProfile.Id,
                        //                UserRoleId = userRole.Id,
                        //                ProfileRoleStatus = "A",
                        //                Strt = tm,
                        //                Expr = null,
                        //                Created = tm,
                        //                LastMod = tm,
                        //                CreatedByUserId = null,
                        //                LastModByUserId = null
                        //            });

                        //            _UPRChanges++;
                        //        }
                        //    }
                        //    //}

                        //    logoutCurrUser = _UPRChanges > 0;
                        //    if (_UPRChanges > 0) _context.SaveChanges();


                        //    //logout ...login to authenticate
                        //    if (logoutCurrUser==true) return RedirectToAction("LoginUserAcc", "UserLogin");
                        //}

                        //catch (Exception ex)
                        //{
                        //    throw;
                        //} 
                    }
                     

                    // default users and roles done.... EXISTING USERS .... >>>>> GET the USER... account logged ---->>>>
                    else
                    {
                        try
                        {
                            //CHECK ACCOUNT EXIST, ACCOUNT ACTIVE      //authenticate app sys admins...  @vendor COMPULSORY -- admin users must confirm via Email or SMS                            
                            ///
                            if (AppUtilties.ComputeSha256Hash(model.ChurchCode) == ac1)
                            {
                                var cc = "000000";
                               // string strPwdHashedData = AppUtilties.ComputeSha256Hash(cc + model.Username.Trim().ToLower() + model.Password);
                                string strUserKeyHashedData = AppUtilties.ComputeSha256Hash(cc + model.Username.Trim().ToLower());
                                oUser_MSTR = (from t_up in _context.UserProfile  //.Include(t => t.ChurchBody)  //.Include(t => t.ChurchMember)
                                                   .Where(c => c.ProfileScope == "V" && // c.UserStatus == "A" &&
                                                   c.Username.Trim().ToLower() == model.Username.Trim().ToLower() && c.UserKey == strUserKeyHashedData) // && c.Pwd == strPwdHashedData)
                                             //from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == t_up.ChurchMemberId)
                                         select t_up
                                                  ).FirstOrDefault();
                            }

                            //authenticate users... @client
                            else
                            {
                                // string strPwdHashedData = AppUtilties.ComputeSha256Hash(model.Username.Trim().ToLower() + model.Password.Trim());
                              //  string strPwdHashedData = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower() + model.Password);
                                string strUserKeyHashedData = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower());
                                oUser_MSTR = (from t_up in _context.UserProfile  //.Include(t => t.ChurchBody)  //.Include(t => t.ChurchMember)
                                                   .Where(c => c.ChurchBody.GlobalChurchCode == model.ChurchCode && c.ProfileScope == "C" && //c.UserStatus == "A" && // (c.Expr == null || userAccExpr >= DateTime.Now.Date) &&
                                                            c.Username.Trim().ToLower() == model.Username.Trim().ToLower() && c.UserKey == strUserKeyHashedData) // && c.Pwd == strPwdHashedData)
                                             //from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == t_up.ChurchMemberId)
                                         select t_up
                                                  ).FirstOrDefault();
                            }


                            //check for user....
                            if (oUser_MSTR == null)
                            {
                                ModelState.AddModelError("", "User credentials provided invalid");
                                model.IsValidated = "F";
                                ///
                                return View(model);
                            }


                            //CHECK ... ACCOUNT EXPR 
                            var userAccExpr = oUser_MSTR.Expr != null ? oUser_MSTR.Expr.Value : oUser_MSTR.Expr;
                            userAccExpr = userAccExpr != null ? userAccExpr.Value : userAccExpr; //  oUser_MSTR.UserStatus != "A" || 
                            if (oUser_MSTR.UserStatus != "A" || (userAccExpr != null && userAccExpr < DateTime.Now.Date))
                            {
                                ModelState.AddModelError("", "User credentials provided invalid [account may be expired or not active]");
                                model.IsValidated = "F";
                                ///
                                return View(model);
                            }


                           //check if PWD-RESET OR PWD-EXPR
                           //check Pwd expiry / Acc expiry  --- RESET PWD
                           ///
                            var userPwdExpr = oUser_MSTR.Expr != null ? oUser_MSTR.PwdExpr.Value : oUser_MSTR.PwdExpr;
                            userPwdExpr = userPwdExpr != null ? userPwdExpr.Value : userPwdExpr;
                            ///
                            if (oUser_MSTR.ResetPwdOnNextLogOn == true || userPwdExpr < DateTime.Now.Date)
                            {//int userId = 0, int? oCurrChuBodyId = null, string profileScope = "C", int setIndex = 0

                                if (oUser_MSTR != null)
                                    oUserPrivilegeCol = AppUtilties.GetUserPrivilege(_context, model.ChurchCode, oUser_MSTR);

                                var _privList = Newtonsoft.Json.JsonConvert.SerializeObject(oUserPrivilegeCol);
                                TempData["UserLogIn_oUserPrivCol"] = _privList; TempData.Keep(); 

                                //  return RedirectToAction("GetChangeUserPwdDetail", "UserLogin");

                                // public IActionResult AddOrEdit_ChangeUserPwd(string churchCode, string username, int setIndex) 
                                var routeValues = new RouteValueDictionary {
                                                          { "churchCode", model.ChurchCode },
                                                          { "username", model.Username },
                                                          { "setIndex", model.ChurchCode == "000000" ? 1 : 2 }
                                                        };

                                return RedirectToAction("AddOrEdit_ChangeUserPwd", "UserLogin", routeValues);


                                //change pwd... afterward... sign out!
                                // public IActionResult AddOrEdit_UP_ChangePwd(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int subSetIndex = 0,
                                // int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) 

                                //return RedirectToAction("AddOrEdit_UP_ChangePwd", "AppVenAdminController", new { oAppGloOwnId = (int?)null, oCurrChuBodyId = (int?)null, 
                                //                id = oUser_MSTR.Id, setIndex = 1, subSetIndex = 1, oAGOId_Logged = oUser_MSTR.AppGlobalOwnerId, oCBId_Logged = oUser_MSTR.ChurchBodyId, oUserId_Logged = oUser_MSTR.Id });


                                //return RedirectToAction("AddOrEdit_UP_ChangePwd", "UserProfile", new { userId = oUser_MSTR.Id, oCurrChuBodyId = oUser_MSTR.ChurchBodyId, profileScope = oUser_MSTR.ProfileScope, setIndex = 4 });

                                //return RedirectToAction( "Main", new RouteValueDictionary( new { controller = controllerName, action = "Main", Id = Id } ) );
                                //return RedirectToAction("Action", new { id = 99 });
                                // return RedirectToAction("AddOrEdit_UP_ChangePwd", "UserProfile", );
                            }



                            //AUTHENTICATE ACCOUNT PWD -- 
                            //authenticate app sys admins...  @vendor COMPULSORY -- admin users must confirm via Email or SMS
                            var isUserPwdAuthenticated = false;
                            if (AppUtilties.ComputeSha256Hash(model.ChurchCode) == ac1) 
                            {
                                var cc = "000000";
                                string strPwdHashedData = AppUtilties.ComputeSha256Hash(cc + model.Username.Trim().ToLower() + model.Password);
                                // string strUserKeyHashedData = AppUtilties.ComputeSha256Hash(cc + model.Username.Trim().ToLower());

                                isUserPwdAuthenticated = oUser_MSTR.Pwd == strPwdHashedData;

                                //oUser_MSTR = (from t_up in _context.UserProfile  //.Include(t => t.ChurchBody)  //.Include(t => t.ChurchMember)
                                //                   .Where(c =>  c.ProfileScope == "V" && c.UserStatus == "A" &&
                                //                   c.Username.Trim().ToLower() == model.Username.Trim().ToLower() && c.UserKey == strUserKeyHashedData && c.Pwd == strPwdHashedData)
                                //             //from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == t_up.ChurchMemberId)
                                //         select t_up
                                //                  ).FirstOrDefault();
                            }
                            
                            //authenticate users... @client
                            else
                            {
                                // string strPwdHashedData = AppUtilties.ComputeSha256Hash(model.Username.Trim().ToLower() + model.Password.Trim());
                                string strPwdHashedData = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower() + model.Password);
                                // string strUserKeyHashedData = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username.Trim().ToLower());

                                isUserPwdAuthenticated = oUser_MSTR.Pwd == strPwdHashedData;

                                //oUser_MSTR = (from t_up in _context.UserProfile  //.Include(t => t.ChurchBody)  //.Include(t => t.ChurchMember)
                                //                   .Where(c => c.ChurchBody.GlobalChurchCode == model.ChurchCode && c.ProfileScope == "C" && c.UserStatus == "A" &&  
                                //                            c.Username.Trim().ToLower() == model.Username.Trim().ToLower() && c.UserKey == strUserKeyHashedData && c.Pwd == strPwdHashedData)
                                //             //from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == t_up.ChurchMemberId)
                                //         select t_up
                                //                  ).FirstOrDefault();
                            }


                            //check for user....
                            if (oUser_MSTR == null || !isUserPwdAuthenticated)
                            {
                                ModelState.AddModelError("", "User credentials [username or password] provided invalid"); 
                                model.IsValidated = "F";
                                ///
                                return View(model);
                            }
                              

                            //if (oUser != null)
                            //{
                            //check for user--member stuff 
                            ///
                            if (oUser_MSTR.AppGlobalOwnerId == null && oUser_MSTR.ChurchBodyId == null && oUser_MSTR.ProfileScope == "V")   // vendor only
                            {
                                // authenticated...
                                isUserValidated = true; model.IsValidated = "T";


                                // check... initialize the COUNTRY
                                if (_context.MSTRCountry.Count() == 0)   // else UPDATE on-demand
                                {
                                    var countriesList = AppUtilties.GetMS_Countries();
                                    var _ctryCount = 0;
                                    if (countriesList.Count > 0)
                                    {
                                        foreach (var oCtry in countriesList)
                                        {
                                            var checkCTRYExist = _context.MSTRCountry.Where(c => c.CtryAlpha3Code == oCtry.CtryAlpha3Code).FirstOrDefault();
                                            if (checkCTRYExist == null)
                                            {
                                                _context.Add(new MSTRCountry()
                                                {
                                                    CtryAlpha3Code = oCtry.CtryAlpha3Code,  //key
                                                    EngName = oCtry.EngName,
                                                    CtryAlpha2Code = oCtry.CtryAlpha2Code,
                                                    CurrEngName = oCtry.CurrEngName,
                                                    CurrLocName = oCtry.CurrLocName,
                                                    CurrSymbol = oCtry.CurrSymbol,
                                                    Curr3LISOSymbol = oCtry.Curr3LISOSymbol,
                                                    //  SharingStatus = "N", 
                                                    Created = DateTime.Now,
                                                    LastMod = DateTime.Now,
                                                    CreatedByUserId = oUser_MSTR.Id,
                                                    LastModByUserId = oUser_MSTR.Id
                                                });

                                                _ctryCount++;
                                            }
                                        }
                                    }


                                    if (_ctryCount > 0)
                                    {
                                        _context.SaveChanges();
                                        ///
                                        _userTask = "Created " + _ctryCount + " countries."; _tm = DateTime.Now;
                                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                                                "RCMS-Admin: Country", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, null, _tm, _tm, null, null));
                                    }
                                } 
                            }

                            else if (oUser_MSTR.AppGlobalOwnerId != null && oUser_MSTR.ChurchBodyId != null && oUser_MSTR.ProfileScope == "C")   // clients only  ... load pre-data
                            {
                                // false until proven true
                                isUserValidated = false; model.IsValidated = "F";


                                // Get the client database details.... db connection string                        
                                var oClientConfig = _context.ClientAppServerConfig.Where(c => c.AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId && c.Status == "A").FirstOrDefault();
                                if (oClientConfig == null)
                                {
                                    ModelState.AddModelError("", "Client database details not found. Please try again or contact System Admin"); //model.IsValidated = "F"; 
                                    return View(model);
                                }

                                // get and mod the conn
                                var _clientDBConnString = "";
                                var conn = new SqlConnectionStringBuilder(_context.Database.GetDbConnection().ConnectionString);
                                conn.DataSource = oClientConfig.ServerName; conn.InitialCatalog = oClientConfig.DbaseName; conn.UserID = oClientConfig.SvrUserId; conn.Password = oClientConfig.SvrPassword; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
                                _clientDBConnString = conn.ConnectionString;

                                // test the NEW DB conn
                                var _clientContext = new ChurchModelContext(_clientDBConnString);
                                if (!_clientContext.Database.CanConnect())
                                {
                                    ModelState.AddModelError("", "Failed to connect client database. Please try again or contact System Admin"); //model.IsValidated = "F";
                                    return View(model);
                                }

                                if (oUser_MSTR.IsChurchMember && oUser_MSTR.ChurchMemberId == null)
                                {
                                    ModelState.AddModelError("", "Church membership of user could not be verified. Please enter correct login credentials."); // model.IsValidated = "F";
                                    return View(model);
                                }

                                else if (oUser_MSTR.IsChurchMember && oUser_MSTR.ChurchMemberId != null)
                                {   //making sure user is active member of the church  ... might not be compulsory anyway... cos church may employ persons from other faiths
                                    var chkUserMemExist = (
                                        from t_ms in _clientContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == model.ChurchCode && c.IsCurrent == true && c.ChurchMemberId == oUser_MSTR.ChurchMemberId)
                                        select t_ms
                                                 ).FirstOrDefault();
                                    ///
                                    if (chkUserMemExist == null) 
                                    {
                                        ModelState.AddModelError("", "Church membership of user could not be verified [inactive or unavailable]. Please enter correct login credentials.");   // model.IsValidated = "F"; // isUserValidated = false;
                                        return View(model);
                                    }
                                }


                                // DONE WITH AUTHENTICATION...
                                model.IsValidated = "T";
                                isUserValidated = true;



                                //if (oUser_MSTR.IsChurchMember && oUser_MSTR.ChurchMemberId != null) 
                                //{   
                                //    if (oClientConfig != null) 
                                //    {
                                //        // get and mod the conn
                                //        var _clientDBConnString = "";
                                //        var conn = new SqlConnectionStringBuilder(_context.Database.GetDbConnection().ConnectionString);
                                //        conn.DataSource = oClientConfig.ServerName; conn.InitialCatalog = oClientConfig.DbaseName; conn.UserID = oClientConfig.SvrUserId; conn.Password = oClientConfig.SvrPassword; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
                                //        _clientDBConnString = conn.ConnectionString;

                                //        // test the NEW DB conn
                                //        var _clientContext = new ChurchModelContext(_clientDBConnString);
                                //        if (_clientContext.Database.CanConnect()) 
                                //        {
                                //            //making sure user is active member of the church  ... might not be compulsory anyway... cos church may employ persons from other faiths
                                //            var chkUserMemExist = (
                                //                from t_ms in _clientContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == model.ChurchCode && c.IsCurrent == true && c.ChurchMemberId == oUser_MSTR.ChurchMemberId)
                                //                select t_ms
                                //                         ).FirstOrDefault();

                                //            ///
                                //            if (chkUserMemExist != null)
                                //            {
                                //                model.IsValidated = "T";
                                //                isUserValidated = true;
                                //            }
                                //            else
                                //            {
                                //                ModelState.AddModelError("", "Church membership of user could not be verified [inactive or unavailable]. Please enter correct login credentials.");   // model.IsValidated = "F"; // isUserValidated = false;
                                //            }
                                //            // oUser_MSTR = chkUserMemExist != null ? oUser : null;
                                //        }
                                //        else
                                //        { // give appropriate user prompts
                                //            ModelState.AddModelError("", "Failed to connect client database. Please try again or contact System Admin"); // isUserValidated = false;
                                //        }
                                //    }
                                //    else
                                //    {
                                //        ModelState.AddModelError("", "Client database details not found. Please try again or contact System Admin"); // isUserValidated = false;
                                //    }
                                //} 
                                //else
                                //{
                                //    ModelState.AddModelError("", "Church membership of user could not be verified. Please enter correct login credentials."); // isUserValidated = false;
                                //}
                                 



                                /// synchronize CTRY, AGO, CL, CB...  from MSTR to CLIENT
                                /// 
                                // initialize the COUNTRY  ... jux the country list standard countries ::: Use [CountryCustom] to config per denomination
                                var _updCount = 0; var tm = DateTime.Now; var strDesc = "Country";
                                var countriesList = AppUtilties.GetClientCountries();
                                var oCTRYCount = _clientContext.Country.Count();
                                ///
                                if (oCTRYCount != countriesList.Count() && countriesList.Count > 0)
                                {
                                    foreach (var oCtry in countriesList)
                                    {
                                        var oCTRY = _clientContext.Country.Where(c => c.CtryAlpha3Code == oCtry.CtryAlpha3Code).FirstOrDefault();
                                        if (oCTRY == null)
                                        {
                                            _clientContext.Add(new Country()
                                            {
                                                CtryAlpha3Code = oCtry.CtryAlpha3Code,
                                                //AppGlobalOwnerId = oAGO.Id,
                                                // ChurchBodyId = 
                                                EngName = oCtry.EngName,                                                
                                                CtryAlpha2Code = oCtry.CtryAlpha2Code,
                                                CurrEngName = oCtry.CurrEngName,
                                                CurrLocName = oCtry.CurrLocName,
                                                CurrSymbol = oCtry.CurrSymbol,
                                                Curr3LISOSymbol = oCtry.Curr3LISOSymbol,
                                                // SharingStatus = "N",
                                                Created = tm,
                                                LastMod = tm,
                                                CreatedByUserId = oUser_MSTR.Id,
                                                LastModByUserId = oUser_MSTR.Id
                                            });

                                            _updCount++;
                                        }
                                        else  // update country data
                                        {
                                            oCTRY.CtryAlpha3Code = oCtry.CtryAlpha3Code;
                                            oCTRY.EngName = oCtry.EngName;                                                
                                            oCTRY.CtryAlpha2Code = oCtry.CtryAlpha2Code;
                                            oCTRY.CurrEngName = oCtry.CurrEngName;
                                            oCTRY.CurrLocName = oCtry.CurrLocName;
                                            oCTRY.CurrSymbol = oCtry.CurrSymbol;
                                            oCTRY.Curr3LISOSymbol = oCtry.Curr3LISOSymbol;
                                            //oCTRY. SharingStatus = "N";
                                            // oCTRY.Created = tm;
                                            oCTRY.LastMod = tm;
                                            // oCTRY.CreatedByUserId = oUser_MSTR.Id;
                                            oCTRY.LastModByUserId = oUser_MSTR.Id;

                                            _clientContext.Update(oCTRY);
                                            _updCount++;
                                        }
                                    }                                    

                                    if (_updCount > 0)
                                    {
                                        _clientContext.SaveChanges();

                                        ///
                                        /////update country of oAGO
                                        //if (oAGO_MSTR != null)
                                        //{
                                        //    oAGO.CtryAlpha3Code = oAGO_MSTR.CtryAlpha3Code;
                                        //    oAGO.LastMod = tm; oAGO.LastModByUserId = oUser_MSTR.Id;
                                        //    _clientContext.Update(oAGO);

                                        //    /// save updated...
                                        //    _clientContext.SaveChanges();
                                        //}

                                        /// update user trail
                                        _userTask = "Created " + _updCount + " countries."; _tm = DateTime.Now;
                                        // record ... @client 
                                        _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, oUser_MSTR.AppGlobalOwnerId, oUser_MSTR.ChurchBodyId, "T",
                                                         "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, oUser_MSTR.Id, _tm, _tm, oUser_MSTR.Id, oUser_MSTR.Id)
                                                        , _clientDBConnString);
                                    }
                                }


                                // initialize the AGO    ...var oAGOClientListCount = _clientContext.AppGlobalOwner.Count();
                                strDesc = "Denomination (Church)"; tm = DateTime.Now; //  _updCount = 0;
                                var oAGO_MSTR = _context.MSTRAppGlobalOwner.AsNoTracking().Include(t => t.FaithTypeCategory).ThenInclude(t => t.FaithTypeClass)
                                                    .Where(c => c.Id == oUser_MSTR.AppGlobalOwnerId ).FirstOrDefault();   //&& c.GlobalChurchCode==oUser_MSTR.strChurchCode_AGO

                                if (oAGO_MSTR == null)
                                {
                                    ModelState.AddModelError("", "Denomination (Church) of user could not be verified [by Vendor]. Please enter correct login credentials.");   // model.IsValidated = "F"; // isUserValidated = false;
                                    return View(model);
                                }


                                var oAGOClient = _clientContext.AppGlobalOwner.Where(c => c.MSTR_AppGlobalOwnerId == oAGO_MSTR.Id || c.GlobalChurchCode == oAGO_MSTR.GlobalChurchCode || c.OwnerName == oAGO_MSTR.OwnerName).FirstOrDefault();
                                if (oAGOClient == null)  // create AGO and CI
                                {
                                    var oCI_MSTR = _context.MSTRContactInfo.Where(c => c.AppGlobalOwnerId == oAGO_MSTR.Id && c.Id == oAGO_MSTR.ContactInfoId).FirstOrDefault();

                                    ContactInfo oCI = null;
                                    if (oCI_MSTR != null)
                                    {
                                        oCI = new ContactInfo()
                                        {
                                            //Id = 0,
                                            // AppGlobalOwnerId = oCB_MSTR.AppGlobalOwnerId,
                                            // ChurchBodyId = oCB_MSTR.Id,
                                            // RefUserId = oCI_MSTR.RefUserId,
                                            ContactName = oCI_MSTR.ContactName,
                                            ResidenceAddress = oCI_MSTR.ResidenceAddress,
                                            Location = oCI_MSTR.Location,
                                            City = oCI_MSTR.City,
                                            CtryAlpha3Code = oCI_MSTR.CtryAlpha3Code,
                                            //RegionId = oCI_MSTR.RegionId,
                                            ResAddrSameAsPostAddr = oCI_MSTR.ResAddrSameAsPostAddr,
                                            PostalAddress = oCI_MSTR.PostalAddress,
                                            DigitalAddress = oCI_MSTR.DigitalAddress,
                                            Telephone = oCI_MSTR.Telephone,
                                            MobilePhone1 = oCI_MSTR.MobilePhone1,
                                            MobilePhone2 = oCI_MSTR.MobilePhone2,
                                            Email = oCI_MSTR.Email,
                                            Website = oCI_MSTR.Website,
                                            ///
                                            Created = tm,
                                            LastMod = tm,
                                            CreatedByUserId = oUser_MSTR.Id,
                                            LastModByUserId = oUser_MSTR.Id
                                        };

                                        _clientContext.Add(oCI);
                                    }


                                    var oAGONew = new AppGlobalOwner()
                                    {
                                        //Id = 0,
                                        MSTR_AppGlobalOwnerId = oAGO_MSTR.Id,
                                        OwnerName = oAGO_MSTR.OwnerName,
                                        GlobalChurchCode = oAGO_MSTR.GlobalChurchCode,
                                        RootChurchCode = oAGO_MSTR.RootChurchCode,
                                        TotalLevels = oAGO_MSTR.TotalLevels,
                                        Acronym = oAGO_MSTR.Acronym,
                                        PrefixKey = oAGO_MSTR.PrefixKey,
                                        Allias = oAGO_MSTR.Allias,
                                        Motto = oAGO_MSTR.Motto,
                                        Slogan = oAGO_MSTR.Slogan,
                                        ChurchLogo = oAGO_MSTR.ChurchLogo,
                                        Status = oAGO_MSTR.Status,
                                        Comments = oAGO_MSTR.Comments,
                                        // CountryId = 0,
                                        CtryAlpha3Code = oAGO_MSTR.CtryAlpha3Code,
                                        strFaithTypeCategory = oAGO_MSTR.FaithTypeCategory != null ? oAGO_MSTR.FaithTypeCategory.FaithDescription : "",
                                        strFaithStream = oAGO_MSTR.FaithTypeCategory != null ? (oAGO_MSTR.FaithTypeCategory.FaithTypeClass != null ? oAGO_MSTR.FaithTypeCategory.FaithTypeClass.FaithDescription : "") : "",
                                        // FaithTypeCategoryId = oAGO_MSTR.FaithTypeCategoryId, // jux keep the Id... get the [strFaithTypeCategory, strFaithTypeStream] ...from MSTR @queries                                         
                                        ContactInfoId = oCI != null ? oCI.Id : (int?)null, // copy details and create this to the local CI                                            
                                        ///
                                        Created = tm,
                                        LastMod = tm,
                                        CreatedByUserId = oUser_MSTR.Id,
                                        LastModByUserId = oUser_MSTR.Id
                                    };

                                    _clientContext.Add(oAGONew);

                                    // _updCount++;


                                    //if (_updCount > 0)
                                    //{

                                    _clientContext.SaveChanges();


                                    // do some update here...
                                    if (oCI != null)
                                    {
                                        oCI.AppGlobalOwnerId = oAGONew.Id;
                                        oCI.LastMod = tm; oCI.LastModByUserId = oUser_MSTR.Id;
                                        ///
                                        _clientContext.Update(oCI);
                                        _clientContext.SaveChanges();
                                    }

                                    // record ... @client
                                    _userTask = "Created " + _updCount + " " + strDesc.ToLower(); _tm = DateTime.Now;

                                    _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, oUser_MSTR.AppGlobalOwnerId, oUser_MSTR.ChurchBodyId, "T",
                                                         "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, oUser_MSTR.Id, _tm, _tm, oUser_MSTR.Id, oUser_MSTR.Id)
                                                        , _clientDBConnString);
                                    //  }
                                }

                                /// some client UPDATE /sync! ...  check the localized data... using the MSTR data
                                else  // update AGO
                                {
                                    if (oAGOClient.MSTR_AppGlobalOwnerId == null || string.IsNullOrEmpty(oAGOClient.GlobalChurchCode) || string.IsNullOrEmpty(oAGOClient.OwnerName) || string.IsNullOrEmpty(oAGOClient.strFaithTypeCategory) || string.IsNullOrEmpty(oAGOClient.strFaithStream))
                                    {
                                        //var oAGO_MSTR = _context.MSTRAppGlobalOwner.AsNoTracking().Include(t => t.FaithTypeCategory).ThenInclude(t => t.FaithTypeClass)
                                        //            .Where(c => c.Id == oUser_MSTR.AppGlobalOwnerId).FirstOrDefault();

                                            if (oAGOClient.MSTR_AppGlobalOwnerId == null)
                                                oAGOClient.MSTR_AppGlobalOwnerId = oAGO_MSTR.Id;

                                            if (string.IsNullOrEmpty(oAGOClient.GlobalChurchCode) || oAGOClient.GlobalChurchCode != oAGO_MSTR.GlobalChurchCode)
                                                oAGOClient.GlobalChurchCode = oAGO_MSTR.GlobalChurchCode;

                                            if (string.IsNullOrEmpty(oAGOClient.OwnerName) || oAGOClient.OwnerName != oAGO_MSTR.OwnerName)
                                                oAGOClient.OwnerName = oAGO_MSTR.OwnerName;

                                            if (string.IsNullOrEmpty(oAGOClient.strFaithTypeCategory))
                                                oAGOClient.strFaithTypeCategory = oAGO_MSTR.FaithTypeCategory != null ? oAGO_MSTR.FaithTypeCategory.FaithDescription : "";

                                            if (string.IsNullOrEmpty(oAGOClient.strFaithStream))
                                                oAGOClient.strFaithStream = oAGO_MSTR.FaithTypeCategory != null ? (oAGO_MSTR.FaithTypeCategory.FaithTypeClass != null ? oAGO_MSTR.FaithTypeCategory.FaithTypeClass.FaithDescription : "") : "";

                                            _clientContext.Update(oAGOClient);
                                            _clientContext.SaveChanges();

                                            ViewBag.UserMsg = strDesc + " updated successfully.";
                                            ///
                                            _userTask = "Updated " + strDesc.ToLower() + ", " + oAGOClient.OwnerName.ToUpper() + " successfully";
                                            _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, oUser_MSTR.AppGlobalOwnerId, oUser_MSTR.ChurchBodyId, "T",
                                                         "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, oUser_MSTR.Id, _tm, _tm, oUser_MSTR.Id, oUser_MSTR.Id)
                                                        , _clientDBConnString);
                                        
                                    }
                                }


                                // Get the denomination/church || c.GlobalChurchCode == oAGO_MSTR.GlobalChurchCode
                                var oAGO = _clientContext.AppGlobalOwner.Where(c => c.MSTR_AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId).FirstOrDefault();                                 


                                // initialize the CL                                
                                var oCL_MSTRList = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId);
                                var oCLClientListCount = _clientContext.ChurchLevel.Count(c => c.MSTR_AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId);
                                ///
                                if (oCLClientListCount != oCL_MSTRList.Count())
                                {
                                    strDesc = "Church Level";
                                    _updCount = 0; tm = DateTime.Now;
                                    if (oCL_MSTRList.Count() > 0 && oAGO != null)
                                    {
                                        foreach (var oCL_MSTR in oCL_MSTRList)
                                        {
                                            var checkCLExist = _clientContext.ChurchLevel.Where(c => c.MSTR_AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId &&
                                                                                (c.Name.ToLower() == oCL_MSTR.Name.ToLower() || c.CustomName.ToLower() == oCL_MSTR.CustomName.ToLower())).FirstOrDefault();
                                            if (checkCLExist == null)
                                            {
                                                _clientContext.Add(new ChurchLevel()
                                                {
                                                    //Id = 0,
                                                    MSTR_AppGlobalOwnerId = oCL_MSTR.AppGlobalOwnerId,
                                                    MSTR_ChurchLevelId = oCL_MSTR.Id,
                                                    ///
                                                    AppGlobalOwnerId = oAGO.Id,
                                                    Name = oCL_MSTR.Name,
                                                    CustomName = oCL_MSTR.CustomName,
                                                    LevelIndex = oCL_MSTR.LevelIndex,
                                                    Acronym = oCL_MSTR.Acronym,
                                                    SharingStatus = oCL_MSTR.SharingStatus,
                                                    ///
                                                    Created = tm,
                                                    LastMod = tm,
                                                    CreatedByUserId = oUser_MSTR.Id,
                                                    LastModByUserId = oUser_MSTR.Id
                                                });

                                                _updCount++;
                                            }
                                        }


                                        if (_updCount > 0)
                                        {
                                            _clientContext.SaveChanges();
                                            ///
                                            // record ... @client
                                            _userTask = "Created " + _updCount + " " + strDesc.ToLower(); _tm = DateTime.Now;
                                            _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, oUser_MSTR.AppGlobalOwnerId, oUser_MSTR.ChurchBodyId, "T",
                                                             "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, oUser_MSTR.Id, _tm, _tm, oUser_MSTR.Id, oUser_MSTR.Id)
                                                            , _clientDBConnString);
                                        }
                                    }
                                }


                                // initialize the CB  ... ONLY create the CB that subscribed even in the same Denomination [ 1 CB at a time ]                       
                                //var oAGO = _clientContext.AppGlobalOwner.Where(c => c.MSTR_AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId || c.GlobalChurchCode == oAGO_MSTR.GlobalChurchCode).FirstOrDefault();
                                var oUserCB_MSTR = _context.MSTRChurchBody.AsNoTracking()  //.Include(t => t.FaithTypeCategory).ThenInclude(t => t.FaithTypeClass)
                                                     .Where(c => c.AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId && c.Id == oUser_MSTR.ChurchBodyId).FirstOrDefault();
                                if (oUserCB_MSTR == null)
                                {
                                    ModelState.AddModelError("", "Church unit of user could not be verified [by Vendor]. Please enter correct login credentials.");   // model.IsValidated = "F"; // isUserValidated = false;
                                    return View(model);
                                }

                                // for single subscription... until user logs in, CB is not created yet on Client server/DB
                                var oCB_MSTRList = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId && c.Id == oUserCB_MSTR.Id);  // use subscription key /shared subscriptio keys for Multiple subsciption sync
                                var oCBClientListCount = _clientContext.ChurchBody.Count(c => c.MSTR_AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId && c.MSTR_ChurchBodyId == oUserCB_MSTR.Id);
                                strDesc = "Church unit";
                                ///                                
                                if (oCBClientListCount != oCB_MSTRList.Count())
                                {                                    
                                    _updCount = 0; tm = DateTime.Now;
                                    if (oCB_MSTRList.Count() > 0 && oAGO != null)
                                    {
                                        foreach (var oCB_MSTR in oCB_MSTRList)
                                        {
                                            var oCBExist = _clientContext.ChurchBody.Where(c => (c.OrganisationType == "CH" || c.OrganisationType == "CN") && c.MSTR_AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId && 
                                                                (c.MSTR_ChurchBodyId == oUser_MSTR.ChurchBodyId || c.GlobalChurchCode == oCB_MSTR.GlobalChurchCode)).FirstOrDefault();
                                            if (oCBExist == null)
                                            {
                                                // Get Church level
                                                var oCL = _clientContext.ChurchLevel.Where(c => c.MSTR_AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId && c.MSTR_ChurchLevelId == oCB_MSTR.ChurchLevelId).FirstOrDefault();

                                                if (oCL != null)
                                                {
                                                    var oCI_MSTR = _context.MSTRContactInfo.Where(c => c.AppGlobalOwnerId == oCB_MSTR.AppGlobalOwnerId && c.ChurchBodyId == oCB_MSTR.Id && c.Id == oCB_MSTR.ContactInfoId).FirstOrDefault();

                                                    ContactInfo oCI = null;
                                                    if (oCI_MSTR != null)
                                                    {
                                                        oCI = new ContactInfo()
                                                        {
                                                            //Id = 0,
                                                            AppGlobalOwnerId = oCB_MSTR.AppGlobalOwnerId,
                                                            ChurchBodyId = oCB_MSTR.Id,
                                                            //RefUserId = oCI_MSTR.RefUserId,
                                                            ContactName = oCI_MSTR.ContactName,
                                                            ResidenceAddress = oCI_MSTR.ResidenceAddress,
                                                            Location = oCI_MSTR.Location,
                                                            City = oCI_MSTR.City,
                                                            CtryAlpha3Code = oCI_MSTR.CtryAlpha3Code,
                                                            //RegionId = oCI_MSTR.RegionId,
                                                            ResAddrSameAsPostAddr = oCI_MSTR.ResAddrSameAsPostAddr,
                                                            PostalAddress = oCI_MSTR.PostalAddress,
                                                            DigitalAddress = oCI_MSTR.DigitalAddress,
                                                            Telephone = oCI_MSTR.Telephone,
                                                            MobilePhone1 = oCI_MSTR.MobilePhone1,
                                                            MobilePhone2 = oCI_MSTR.MobilePhone2,
                                                            Email = oCI_MSTR.Email,
                                                            Website = oCI_MSTR.Website,
                                                            ///
                                                            Created = tm,
                                                            LastMod = tm,
                                                            CreatedByUserId = oUser_MSTR.Id,
                                                            LastModByUserId = oUser_MSTR.Id
                                                        };

                                                        _clientContext.Add(oCI);
                                                    }

                                                    _clientContext.Add(new ChurchBody()
                                                    {
                                                        //Id = 0,
                                                        MSTR_AppGlobalOwnerId = oCB_MSTR.AppGlobalOwnerId,
                                                        MSTR_ChurchBodyId = oCB_MSTR.Id,
                                                        MSTR_ParentChurchBodyId = oCB_MSTR.ParentChurchBodyId,
                                                        MSTR_ChurchLevelId = oCB_MSTR.ChurchLevelId,
                                                        ///
                                                        AppGlobalOwnerId = oAGO.Id,
                                                        ChurchLevelId = oCL.Id,
                                                        Name = oCB_MSTR.Name,
                                                        GlobalChurchCode = oCB_MSTR.GlobalChurchCode,
                                                        RootChurchCode = oCB_MSTR.RootChurchCode,
                                                        OrganisationType = oCB_MSTR.OrganisationType,
                                                        SubscriptionKey = oCB_MSTR.SubscriptionKey,
                                                        // ParentChurchBodyId = null,  // update after first batch...   ***
                                                        ContactInfoId = oCI != null ? oCI.Id : (int?)null,  // create from the MSTR CI data-values ***
                                                        CtryAlpha3Code = oCB_MSTR.CtryAlpha3Code,  //country GHA, USA, GBR 
                                                                                                   // CountryRegionId = null, // oCB_MSTR.CountryRegionId, ***
                                                        Comments = oCB_MSTR.Comments,
                                                        Status = oCB_MSTR.Status,
                                                        ///
                                                        Created = tm,
                                                        LastMod = tm,
                                                        CreatedByUserId = oUser_MSTR.Id,
                                                        LastModByUserId = oUser_MSTR.Id
                                                    });

                                                    _updCount++;
                                                }
                                            } 
                                        }

                                        /// NEW only else... on-demand update ... so that this code is run jux once... NOT @ every login
                                        if (_updCount > 0)
                                        {
                                            _clientContext.SaveChanges();

                                            /// set Parent ChurchBody
                                            var oCBList = _clientContext.ChurchBody.Where(c => c.AppGlobalOwnerId == oAGO.Id);
                                            _updCount = 0; tm = DateTime.Now;
                                            if (oCBList.Count() > 0)
                                            {
                                                foreach (var oCB in oCBList)
                                                {
                                                    var oCBPar = _clientContext.ChurchBody.Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.MSTR_AppGlobalOwnerId == oCB.MSTR_AppGlobalOwnerId &&
                                                                                    c.GlobalChurchCode == oCB.GlobalChurchCode && c.MSTR_ChurchBodyId == oCB.MSTR_ParentChurchBodyId).FirstOrDefault();
                                                    if (oCBPar != null)
                                                    {
                                                        if (oCB.ParentChurchBodyId != oCBPar.Id)
                                                        {
                                                            oCB.ParentChurchBodyId = oCBPar.Id;
                                                            oCB.LastMod = tm; oCB.LastModByUserId = oUser_MSTR.Id;
                                                            ///
                                                            _clientContext.Update(oCB);
                                                            _updCount++;
                                                        }
                                                    }
                                                }

                                                /// save updated...
                                                if (_updCount > 0)
                                                    _clientContext.SaveChanges();
                                            }


                                            // record ... @client
                                            _userTask = "Created/updated " + _updCount + " " + strDesc.ToLower() + "s"; _tm = DateTime.Now;
                                            _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, oUser_MSTR.AppGlobalOwnerId, oUser_MSTR.ChurchBodyId, "T",
                                                             "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, oUser_MSTR.Id, _tm, _tm, oUser_MSTR.Id, oUser_MSTR.Id)
                                                            , _clientDBConnString);
                                        }
                                    }
                                }
                                 

                                var oCBClient = _clientContext.ChurchBody.AsNoTracking().Include(t => t.ChurchLevel)
                                                .Where(c => c.MSTR_AppGlobalOwnerId == oUserCB_MSTR.AppGlobalOwnerId && (c.MSTR_ChurchBodyId == oUserCB_MSTR.Id || c.GlobalChurchCode == oUserCB_MSTR.GlobalChurchCode || (c.MSTR_ParentChurchBodyId==oUserCB_MSTR.ParentChurchBodyId && c.Name == oUserCB_MSTR.Name))).FirstOrDefault();
                                //var oCBClient = _clientContext.ChurchBody.AsNoTracking().Where(c => (c.OrganisationType == "CH" || c.OrganisationType == "CN") && c.MSTR_AppGlobalOwnerId == oUser_MSTR.AppGlobalOwnerId &&
                                //                                (c.MSTR_ChurchBodyId == oUser_MSTR.ChurchBodyId || c.GlobalChurchCode == oCB_MSTR.GlobalChurchCode)).FirstOrDefault();
                                
                                if (oCBClient != null)
                                {
                                    if (oCBClient.MSTR_AppGlobalOwnerId == null || oCBClient.MSTR_ChurchBodyId == null || oCBClient.MSTR_ChurchLevelId == null || string.IsNullOrEmpty(oCBClient.GlobalChurchCode) || string.IsNullOrEmpty(oCBClient.Name))
                                    {  
                                        if (oCBClient.MSTR_AppGlobalOwnerId == null)
                                            oCBClient.MSTR_AppGlobalOwnerId = oUserCB_MSTR.AppGlobalOwnerId;

                                        if (oCBClient.MSTR_ChurchBodyId == null)
                                            oCBClient.MSTR_ChurchBodyId = oUserCB_MSTR.Id;

                                        if (oCBClient.MSTR_ChurchLevelId == null)
                                            oCBClient.MSTR_ChurchLevelId = oUserCB_MSTR.ChurchLevelId;

                                        if (string.IsNullOrEmpty(oCBClient.GlobalChurchCode) || oAGOClient.GlobalChurchCode != oUserCB_MSTR.GlobalChurchCode)
                                            oCBClient.GlobalChurchCode = oUserCB_MSTR.GlobalChurchCode;

                                        if (string.IsNullOrEmpty(oCBClient.Name) || oCBClient.Name != oUserCB_MSTR.Name)
                                            oCBClient.Name = oUserCB_MSTR.Name;


                                        _clientContext.Update(oCBClient);
                                        _clientContext.SaveChanges();

                                        ViewBag.UserMsg = strDesc + " updated successfully.";
                                        _userTask = "Updated " + strDesc.ToLower() + ", " + oCBClient.Name.ToUpper() + " successfully";                                        
                                    }
                                }
                            }

                            // }

                            if (!isUserValidated) //oUser_MSTR == null)
                            {
                                model.IsValidated = "F";
                                ///
                                return View(model);
                            }
                        }

                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                      

                    //??
                    // RESET User password...... @pwd_reset, expired user, expired pwd ...
                    ///
                    if (oUser_MSTR != null && isUserValidated)
                    {
                        ////check Pwd expiry / Acc expiry  --- RESET PWD
                        /////
                        //var userAccExpr = oUser_MSTR.Expr != null ? oUser_MSTR.Expr.Value : oUser_MSTR.Expr;
                        //userAccExpr = userAccExpr != null ? userAccExpr.Value : userAccExpr; 
                        /////
                        //if (oUser_MSTR.ResetPwdOnNextLogOn == true || oUser_MSTR.UserStatus != "A" || userAccExpr <= DateTime.Now.Date )
                        //{//int userId = 0, int? oCurrChuBodyId = null, string profileScope = "C", int setIndex = 0

                        //    if (oUser != null)
                        //        oUserPrivilegeCol = AppUtilties.GetUserPrivilege(_context, model.ChurchCode, oUser);

                        //    var _privList = Newtonsoft.Json.JsonConvert.SerializeObject(oUserPrivilegeCol);
                        //    TempData["UserLogIn_oUserPrivCol"] = _privList; TempData.Keep();

                        //    //  return RedirectToAction("GetChangeUserPwdDetail", "UserLogin");

                        //    // public IActionResult AddOrEdit_ChangeUserPwd(string churchCode, string username, int setIndex) 
                        //    var routeValues = new RouteValueDictionary {
                        //                                  { "churchCode", model.ChurchCode },
                        //                                  { "username", model.Username },
                        //                                  { "setIndex", model.ChurchCode == "000000" ? 1 : 2 }
                        //                                };

                        //    return RedirectToAction("AddOrEdit_ChangeUserPwd", "UserLogin", routeValues);


                        //    //change pwd... afterward... sign out!
                        //    // public IActionResult AddOrEdit_UP_ChangePwd(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int subSetIndex = 0,
                        //    // int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) 

                        //    //return RedirectToAction("AddOrEdit_UP_ChangePwd", "AppVenAdminController", new { oAppGloOwnId = (int?)null, oCurrChuBodyId = (int?)null, 
                        //    //                id = oUser_MSTR.Id, setIndex = 1, subSetIndex = 1, oAGOId_Logged = oUser_MSTR.AppGlobalOwnerId, oCBId_Logged = oUser_MSTR.ChurchBodyId, oUserId_Logged = oUser_MSTR.Id });


                        //    //return RedirectToAction("AddOrEdit_UP_ChangePwd", "UserProfile", new { userId = oUser_MSTR.Id, oCurrChuBodyId = oUser_MSTR.ChurchBodyId, profileScope = oUser_MSTR.ProfileScope, setIndex = 4 });

                        //    //return RedirectToAction( "Main", new RouteValueDictionary( new { controller = controllerName, action = "Main", Id = Id } ) );
                        //    //return RedirectToAction("Action", new { id = 99 });
                        //    // return RedirectToAction("AddOrEdit_UP_ChangePwd", "UserProfile", );
                        //}


                        oUserPrivilegeCol = AppUtilties.GetUserPrivilege(_context, model.ChurchCode, oUser_MSTR);

                        // oUserPrivilegeCol = AppUtilties.ValidateUser( _context, _clientDBContext, model.ChurchCode, model.Username, model.Password);
                        //TempData.Put("UserLogIn_oUserPrivCol", oUserPrivilegeCol); TempData.Keep();

                        var privList = Newtonsoft.Json.JsonConvert.SerializeObject(oUserPrivilegeCol);
                        TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();

                        //ViewBag.oAppGloOwnId_Logged = oUserPrivilegeCol[0].ChurchBody.AppGlobalOwnerId;
                        //ViewBag.oChuBodyId_Logged = oUserPrivilegeCol[0].ChurchBody.Id;
                        ////
                        //ViewBag.oUserId_Logged = oUserPrivilegeCol[0].UserProfile.Id;
                        //ViewBag.oMemberId_Logged = oUserPrivilegeCol[0].UserProfile.ChurchMemberId;



                        //TempData.Put("oModel", model); TempData.Keep();
                        //TempData.Put("oVerifUserPriv", oUserPrivilegeCol); TempData.Keep();

                        //model.currChurchCode = model.ChurchCode;
                        //model.currUsername = model.Username;
                        //model.currPassword = model.Password;

                        isUserValidated = oUserPrivilegeCol != null;
                    }
                }

                else
                {
                    // oUserPrivilegeCol = UserLogOnUtility.ValidateUser(_context, model.currChurchCode, model.currUsername, model.currPassword);                   
                    // oUserPrivilegeCol = TempData.Get<List<UserSessionPrivilege>>("UserLogIn_oUserPrivCol");

                    var tempPrivList = TempData["UserLogIn_oUserPrivCol"] as string;
                    // De serialize the string to object
                    oUserPrivilegeCol = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);
                    isUserValidated = oUserPrivilegeCol != null;
                }

                
              //  model.IsValidated = isUserValidated ? "T" : "F";
                 
                //  var userValidated = true;
                // user verification ... 2 way check (sms / email) or security quetion -- token to be sent once per active session

                if (isUserValidated)
                {
                    // false until proven true
                    model.IsValidated = "F";


                    // VERIFY ... user
                    //oUser_MSTR = _context.UserProfile .Include(u => u.ChurchBody)
                    //   .Where(c => c.ChurchBody.ChurchCode == model.ChurchCode && c.Username == model.Username && c.UserStatus == "A").FirstOrDefault();

                    if (AppUtilties.TrustedClients.ValidateClient(model.Username, null, null) == false) //Request.UserHostAddress, Request.UserAgent))
                    {
                        model.IsValidated = "T";  //    valid = true;
                    }
                    else

                    {   //
                        //clear this line when done... testing only!!!   AppUtilties.ComputeSha256Hash("12345678") 
                        model.VerificationCode = "12345678" ;  TempData ["oVmLogin"] = AppUtilties.ComputeSha256Hash("12345678") ; TempData.Keep();     
                        //

                        if (string.IsNullOrEmpty(model.VerificationCode))
                        {
                            // string vCode = "12345678"; 
                            string vCode = CodeGenerator.GenerateCode();
                            string vCodeEncrypt = AppUtilties.ComputeSha256Hash(vCode);
                            // UserLogOnUtility.StoreValidationCode(model.UserName, vCodeEncrypt);
                             
                            //  TempData ["oVmLogin"] = vCodeEncrypt; TempData.Keep();

                            //var privList = Newtonsoft.Json.JsonConvert.SerializeObject(vCodeEncrypt);
                            //TempData["oVmLogin"] = privList; TempData.Keep();

                            TempData["oVmLogin"] = vCodeEncrypt; TempData.Keep();

                            // AppUtilties.SendSMSNotification(UserLogOnUtility.GetUserPhone(_context, model.Username), "233", string.Format("Your account verification code is {0}.", vCode));
                            ViewData["VerificationCodeEnabled"] = true;
                            var tempDesc = model.Username;
                            if (oUserPrivilegeCol[0].UserProfile != null)
                            {
                                tempDesc = oUserPrivilegeCol[0].UserProfile.UserDesc;
                                //
                                // can be done after user gone in... and client identified
                                //if (oUserPrivilegeCol[0].UserProfile.ChurchMemberId != null)
                                //{
                                //    var tcm = _clientDBContext.ChurchMember.Where(c => c.AppGlobalOwnerId == oUserPrivilegeCol[0].UserProfile.AppGlobalOwnerId && c.ChurchBodyId == oUserPrivilegeCol[0].UserProfile.ChurchBodyId && c.Id == oUserPrivilegeCol[0].UserProfile.ChurchMemberId).FirstOrDefault();

                                //    if (!string.IsNullOrEmpty(tcm.FirstName))
                                //        tempDesc = tcm.FirstName;
                                //    else if (!string.IsNullOrEmpty(tcm.LastName))
                                //        tempDesc = tcm.LastName;
                                //    else if (!string.IsNullOrEmpty(tcm.MiddleName))
                                //        tempDesc = tcm.MiddleName; 
                                //}
                                ////else
                                ////    tempDesc = oUserPrivilegeCol[0].UserProfile.UserDesc;
                            }            
                                    
                            //
                            model.strLogUserDesc = tempDesc;
                            ModelState.ClearValidationState("");

                            return View(model);
                        }

                        else
                        {
                            //var vCode = TempData.Get<string>("oVmLogin");
                            var vCodeEncrypt = TempData["oVmLogin"] as string ;

                            //var arrData = "";
                            //arrData = TempData.ContainsKey("oVmLogin") ? TempData["oVmLogin"] as string : arrData;
                            //var vCode = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<string>(arrData) : "";

                            if (vCodeEncrypt != null)
                            {   // if (UserLogOnUtility.ReadValidationCode(_context, model.UserName) == model.VerificationCode)
                                if (vCodeEncrypt != AppUtilties.ComputeSha256Hash(model.VerificationCode))
                                {   // TrustedClients.AddClient(model.UserName, Request.UserHostAddress, Request.UserAgent);
                                    model.IsValidated = "T"; // valid = true;
                                }
                                else
                                {
                                    ModelState.AddModelError("", "The verification code is incorrect."); // isUserValidated = false;   
                                    ViewData["VerificationCodeEnabled"] = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    model.IsValidated = "F";
                    ModelState.AddModelError("", "Validation failed. Username or password provided is incorrect.");
                }
            }


            // user authentication and authorization done... go to --->>> 1) Admin Profile or 2) Client Dashboard  ... by client data configurations
            if (model.IsValidated == "T")  // valid==true
            {
                // if (ModelState.ContainsKey("")) ModelState[""].Errors.Clear();
                //ModelState.vali ModelState.Remove["{}"];
                // ModelState[""].ValidationState = ModelValidationState.Valid;

                //foreach (var key in ModelState.Keys)
                //{
                //    ModelState[key].Errors.Clear();
                //    ModelState[key].ValidationState = ModelValidationState.Valid;
                //}

                //ViewBag.vwCurrChurchBodyId = oUserPrivilegeCol[0].ChurchBody.Id;
                //ViewBag.vwCurrChurchBody = oUserPrivilegeCol[0].ChurchBody;

                var privList = Newtonsoft.Json.JsonConvert.SerializeObject(oUserPrivilegeCol);
                TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();
                // return RedirectToAction("Index", "ChurchWorkbench");

                //authentication done...   //successfull login... audit!
                var tm = DateTime.Now;

                ViewData["strAppName"] = "RHEMA-CMS";

                //vendor home
                if (AppUtilties.ComputeSha256Hash(model.ChurchCode) == ac1)
                {
                    //_ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, oUserPrivilegeCol[0].AppGlobalOwner != null ? oUserPrivilegeCol[0].AppGlobalOwner.Id : (int?)null, oUserPrivilegeCol[0].ChurchBody != null ? oUserPrivilegeCol[0].ChurchBody.Id : (int?)null,
                    //            "L", "RCMS-Admin: User Login", AppUtilties.GetRawTarget(HttpContext.Request), "Logged in successfully to RHEMA-CMS", tm, oUserPrivilegeCol[0].UserProfile.Id, tm, tm, oUserPrivilegeCol[0].UserProfile.Id, oUserPrivilegeCol[0].UserProfile.Id));
                    //
                   // ModelState.ClearValidationState("");

                    return RedirectToAction("Index_sa", "Home"); 

                   // return RedirectToAction("Index_AGO", "AppVenAdmin"); 
                    
                }                    

                //...client home
                else
                {
                    // get the connection... to client db
                    //var conn = new SqlConnectionStringBuilder(_context.Database.GetDbConnection().ConnectionString);
                    ////  "DefaultConnection": "Server=RHEMA-SDARTEH;Database=DBRCMS_MS_DEV;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true"
                    //conn.DataSource = "RHEMA-SDARTEH"; conn.InitialCatalog = "DBRCMS_CL_TEST"; conn.UserID = "sa"; conn.Password = "sadmin"; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
                    //this._clientDBConnString = conn.ConnectionString;


                    //_ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, oUserPrivilegeCol[0].AppGlobalOwner != null ? oUserPrivilegeCol[0].AppGlobalOwner.Id : (int?)null, oUserPrivilegeCol[0].ChurchBody != null ? oUserPrivilegeCol[0].ChurchBody.Id : (int?)null,
                    //            "L", "RCMS Client: User Login", AppUtilties.GetRawTarget(HttpContext.Request), "Logged in successfully to RHEMA-CMS", tm, oUserPrivilegeCol[0].UserProfile.Id, tm, tm, oUserPrivilegeCol[0].UserProfile.Id, oUserPrivilegeCol[0].UserProfile.Id));
                    //
                   // ModelState.ClearValidationState("");

                    return RedirectToAction("Index", "Home");
                }
                   

                //// FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                //if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1
                //    && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                //{
                //    return Redirect(returnUrl);
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Home");
                //}
            }

            // If we got this far, something failed, redisplay form
            return View(model);             
        }


        //private async Task LogUserActivity_AppMainUserAuditTrail(UserAuditTrail oUserTrail)
        //{ // var oUserTrail = _masterContext.UserAuditTrail.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null && churchCode=="000000") || (c.AppGlobalOwnerId== oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId))
        //    if (oUserTrail != null)
        //    {  
        //        _masterContextLog.UserAuditTrail.Add(oUserTrail);
        //        await _masterContextLog.SaveChangesAsync();
        //    }
        //}


        private async Task LogUserActivity_AppMainUserAuditTrail(UserAuditTrail oUserTrail)
        { // var oUserTrail = _masterContext.UserAuditTrail.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null && churchCode=="000000") || (c.AppGlobalOwnerId== oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId))
            if (oUserTrail != null)
            {
                // var tempCtx = _context;
                using (var logCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString)) // ("Server=RHEMA-SDARTEH;Database=DBRCMS_MS_TEST;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true") ) // AppUtilties.GetNewDBContext_MS(_context, "DBRCMS_CL_TEST"))  // MSTR_DbContext()) //
                {
                    if (logCtx.Database.CanConnect() == false) logCtx.Database.OpenConnection();
                    else if (logCtx.Database.GetDbConnection().State != System.Data.ConnectionState.Open) logCtx.Database.OpenConnection();
                    //
                    // var a = logCtx.Database.GetDbConnection().ConnectionString;
                    // var b = _context.Database.GetDbConnection().ConnectionString;
                    /// 
                    logCtx.UserAuditTrail.Add(oUserTrail);
                    await logCtx.SaveChangesAsync();

                    //logCtx.SaveChanges();

                    logCtx.Entry(oUserTrail).State = EntityState.Detached;
                    ///
                    //DetachAllEntities(logCtx); 

                    // close connection
                    logCtx.Database.CloseConnection();

                    //logCtx.Dispose();
                }
            }
        }

        private async Task LogUserActivity_ClientUserAuditTrail(UserAuditTrail_CL oUserTrail, string clientDBConnString)
        { // var oUserTrail = _masterContext.UserAuditTrail.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null && churchCode=="000000") || (c.AppGlobalOwnerId== oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId))
            if (oUserTrail != null)
            {
                // var tempCtx = _context;
                if (!string.IsNullOrEmpty(clientDBConnString))
                {
                    using (var logCtx = new ChurchModelContext(clientDBConnString)) // ("Server=RHEMA-SDARTEH;Database=DBRCMS_MS_TEST;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true") ) // AppUtilties.GetNewDBContext_MS(_context, "DBRCMS_CL_TEST"))  // MSTR_DbContext()) //
                    {
                        //logCtx = _context;
                        //var conn = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(_context.Database.GetDbConnection().ConnectionString);
                        ////  "DefaultConnection": "Server=RHEMA-SDARTEH;Database=DBRCMS_MS_DEV;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true"
                        //conn.DataSource = "RHEMA-SDARTEH"; conn.InitialCatalog = "DBRCMS_CL_TEST"; conn.UserID = "sa"; conn.Password = "sadmin"; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
                        /////
                        //logCtx.Database.GetDbConnection().ConnectionString = conn.ConnectionString;

                        if (logCtx.Database.CanConnect() == false) logCtx.Database.OpenConnection();
                        else if (logCtx.Database.GetDbConnection().State != System.Data.ConnectionState.Open) logCtx.Database.OpenConnection();
                        
                        // var a = logCtx.Database.GetDbConnection().ConnectionString;
                        // var b = _masterContext.Database.GetDbConnection().ConnectionString;

                        /// 
                        logCtx.UserAuditTrail_CL.Add(oUserTrail);
                        await logCtx.SaveChangesAsync();

                        //logCtx.SaveChanges();

                        logCtx.Entry(oUserTrail).State = EntityState.Detached;
                        ///
                        //DetachAllEntities(logCtx); 

                        // close connection
                        logCtx.Database.CloseConnection();

                        //logCtx.Dispose();
                    }
                } 
            }
        }

        public void DetachAllEntities(MSTR_DbContext ctx)
        {
            var changedEntriesCopy = ctx.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }


        private bool isCurrValid = false;
        private List<UserSessionPrivilege> oUserLogIn_Priv = null;
        private bool userAuthorized = false;
        private void SetUserLogged()
        {
            ////  oUserLogIn_Priv = TempData.Get<List<UserSessionPrivilege>>("UserLogIn_oUserPrivCol");

            //List<UserSessionPrivilege> oUserLogIn_Priv = TempData.ContainsKey("UserLogIn_oUserPrivCol") ?
            //                                                TempData["UserLogIn_oUserPrivCol"] as List<UserSessionPrivilege> : null;


            var tempPrivList = TempData["UserLogIn_oUserPrivCol"] as string;
            // De serialize the string to object
            oUserLogIn_Priv = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);

            isCurrValid = oUserLogIn_Priv?.Count > 0;
            if (isCurrValid)
            {
                ViewData["oAppGloOwnLogged"] = oUserLogIn_Priv[0].AppGlobalOwner;
                ViewData["oChuBodyLogged"] = oUserLogIn_Priv[0].ChurchBody;
                ViewData["oUserLogged"] = oUserLogIn_Priv[0].UserProfile;

                // check permission for Core life...  given the sets of permissions
                userAuthorized = oUserLogIn_Priv.Count > 0; //(oUserLogIn_Priv.Find(x => x.PermissionName == "_A0__System_Administration" || x.PermissionName == "xxx") != null);
            }
        }


        [HttpGet]
        public IActionResult GetChangeUserPwdDetail(int pageIndex = 1)
        {
            var oUserResetModel = new ResetUserPasswordModel();
             
            // oUserResetModel.pageIndex = 1; 
             //
            return View("AddOrEdit_ChangeUserPwd", oUserResetModel);
        }


        [HttpGet]   // public IActionResult AddOrEdit_UP_ChangePwd(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int subSetIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)  //(int userId = 0, int setIndex = 0) // int? oCurrChuBodyId = null, string profileScope = "C", int setIndex = 0)   // setIndex = 0 (SYS), setIndex = 1 (SUP_ADMN), = 2 (Create/update user), = 3 (reset Pwd) 
        public IActionResult AddOrEdit_ChangeUserPwd(string churchCode, string username, int setIndex, int pageIndex = 1)  //, int? oUserId_Logged = null  (int userId = 0, int setIndex = 0) // int? oCurrChuBodyId = null, string profileScope = "C", int setIndex = 0)   // setIndex = 0 (SYS), setIndex = 1 (SUP_ADMN), = 2 (Create/update user), = 3 (reset Pwd) 
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                // check permission 
                var _oUserPrivilegeCol = oUserLogIn_Priv;
                var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();
                //
                int? oAGOId_Logged = null;
                int? oCBId_Logged = null;
                int? oUserId_Logged = null;
                UserProfile oLoggedUser = null;
                UserRole oLoggedRole = null;
                MSTRChurchBody oChuBody_Logged = null;
                //
                if (!this.userAuthorized) return View(new UserProfileModel()); //retain view    
                if (oUserLogIn_Priv[0] != null)
                {
                    oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                    oLoggedRole = oUserLogIn_Priv[0].UserRole; 
                    oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody;

                    if (oLoggedUser != null) oUserId_Logged = oLoggedUser.Id;
                    if (oChuBody_Logged != null) { oCBId_Logged = oChuBody_Logged.Id; oAGOId_Logged = oChuBody_Logged.AppGlobalOwnerId;}
                    if (oAGOId_Logged == null && oUserLogIn_Priv[0].AppGlobalOwner != null) oAGOId_Logged = oUserLogIn_Priv[0].AppGlobalOwner.Id; 
                }
                 
                //
                var oUserResetModel = new ResetUserPasswordModel();
                // 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
                setIndex = churchCode == "000000" ? 1 : 2;
                var proScope = setIndex == 1 ? "V" : "C";
                var subScope = setIndex == 2 ? "D" : setIndex == 3 ? "A" : "";

                var _userTask = "Attempt to change password. Retrieved user details"; var _tm = DateTime.Now; var userDenom = "";
                //int? oAppGloOwnId = null; int? oCurrChuBodyId = null;
                //int? oAGOId_Logged = null; int? oCBId_Logged = null;
                // 
                //var oUser_MSTR = _context.UserProfile
                //         .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == proScope).FirstOrDefault();

                const string ac1 = "91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203";
                var h_pwd = AppUtilties.ComputeSha256Hash(churchCode); 
                string strUserKeyHashedData = AppUtilties.ComputeSha256Hash(churchCode + username.Trim().ToLower()); 
                var oUser_MSTR = _context.UserProfile.Include(t=>t.AppGlobalOwner).Include(t=>t.ChurchBody)
                                .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode)) && x.ProfileScope == proScope && x.UserStatus == "A" &&
                                    x.Username.Trim().ToLower() == username.Trim().ToLower() && x.UserKey == strUserKeyHashedData).FirstOrDefault();
                
                if (oUser_MSTR == null) 
                {
                    userDenom = setIndex == 1 ? "Vendor Admin" : "Client";
                   _userTask = "Attempt to change password. User account could not be retrieved - " + username + " [" + userDenom + "]";
                    _tm = DateTime.Now;
                   _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     setIndex == 1 ? "RCMS-Admin:" : "RCMS-Client:" + " User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, oUserId_Logged, _tm, _tm, oUserId_Logged, oUserId_Logged));
                    //
                    oUserResetModel.pageIndex = 1;
                    oUserResetModel.userMess = "Attempt to change password. User account could not be retrieved.";
                    return View(oUserResetModel); // PartialView("_AddOrEdit_UP_ChangePwd", oUserResetModel);
                }
 
                // 
                //oCurrVmMod.oChurchBody = oCurrChuBodyLogOn;  
                oUserResetModel.oUserProfile = oUser_MSTR;
                //
                oUserResetModel.ChurchCode = churchCode;
                oUserResetModel.Username = oUser_MSTR.Username;
                oUserResetModel.strLogUserDesc = oUser_MSTR.UserDesc;
                ViewData["strAppName"] = "RHEMA-CMS";
                ///
                oUserResetModel.CurrentPassword = null;
                oUserResetModel.NewPassword = null;
                oUserResetModel.RepeatPassword = null;
                // oCurrVmMod.SecurityQue = oUser_MSTR.PwdSecurityQue;
                // oCurrVmMod.SecurityAns = null;
                oUserResetModel.VerificationCode = null; // via email, sms                
                oUserResetModel.AuthTypeUsed = null;

                //var _oCurrVmMod = oCurrVmMod;
                //TempData["oVmCurrMod"] = _vmMod;
                //TempData.Keep();

                //var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(_oCurrVmMod);
                //TempData["oVmCurrMod"] = _vmMod; TempData.Keep();


                oUserResetModel.setIndex = setIndex;
                oUserResetModel.pageIndex = 2; // pageIndex;
                oUserResetModel.profileScope = proScope;
                oUserResetModel.subScope = subScope;
                //
                oUserResetModel.oCurrUserId_Logged = oUserId_Logged;
                oUserResetModel.oAppGloOwnId_Logged = oAGOId_Logged;
                oUserResetModel.oChurchBodyId_Logged = oCBId_Logged;
                oUserResetModel.oMemberId_Logged = oUser_MSTR.ChurchMemberId; // oCurrChuBodyId;
                //
                oUserResetModel.oAppGloOwnId = oUser_MSTR.AppGlobalOwnerId; // oAppGloOwnId;
                oUserResetModel.oChurchBodyId = oUser_MSTR.ChurchBodyId; // oCurrChuBodyId;
                
                 
                oUserResetModel.lkpAuthTypes = new List<SelectListItem>();
                foreach (var dl in dlUserAuthTypes) 
                { 
                    oUserResetModel.lkpAuthTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc });
                    oUserResetModel.lkpAuthTypes.Insert(0, new SelectListItem { Value = "", Text = "Select authentication type", Disabled=true }); 
                }

                // send valiadation code to user...

                //email recipients... applicant, church   ... specific e-mail content
                MailAddressCollection listToAddr = new MailAddressCollection();
                MailAddressCollection listCcAddr = new MailAddressCollection();
                MailAddressCollection listBccAddr = new MailAddressCollection();
                // string strUrl = string.Concat(this.Request.Scheme, "://", this.Request.Host, this.Request.Path, this.Request.QueryString);
                 
               
                var vCode = CodeGenerator.GenerateCode();
                oUserResetModel.SentVerificationCode = AppUtilties.ComputeSha256Hash(vCode); // "12345678";
               // TempData["oVmAuthCode"] = vCode; TempData.Keep();

                listToAddr.Add(new MailAddress(oUser_MSTR.Email, oUser_MSTR.UserDesc));
                var res = AppUtilties.SendEmailNotification("RHEMA-CMS", "User Password Change Request - Authentication", "Hello " + oUser_MSTR.UserDesc + ", " +
                    Environment.NewLine + "Password Change Request Authentication code: " + vCode, listToAddr, listCcAddr, listBccAddr, null);


                userDenom = setIndex == 1 ? "Vendor Admin" : (oUser_MSTR.ChurchBody != null ? oUser_MSTR.ChurchBody.Name + "-" : "") + (oUser_MSTR.ChurchBody != null ? oUser_MSTR.AppGlobalOwner.OwnerName + "-" : "");
                _userTask = "Attempt to change pwd. Retrieved user account - " + oUser_MSTR.Username + " [" + userDenom + "]";
               _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     setIndex == 1 ? "RCMS-Admin:" : "RCMS-Client:" + " User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, oUserId_Logged, _tm, _tm, oUserId_Logged, oUserId_Logged));
                //
                oUserResetModel.strLogUserDesc = (userDenom + ": " + oUser_MSTR.UserDesc).ToUpper();

                //
                //var _oUserResetModel = Newtonsoft.Json.JsonConvert.SerializeObject(oUserResetModel);
                //TempData["oVmCurrMod"] = _oUserResetModel; TempData.Keep();

                return View(oUserResetModel); //  PartialView("_AddOrEdit_UP_ChangePwd", oUserResetModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_ChangeUserPwd(ResetUserPasswordModel vmMod)
        {
           // UserProfile _oChanges;  // = vmMod .oUserProfile;
            if (vmMod == null) 
                return Json(new { taskSuccess = false, oCurrId = -1, userMess = "User account was not found.", signOutToLogIn = false });

            // var vmModSave = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as ResetUserPasswordModel : vmMod; TempData.Keep();
            
            // default (vendor) church code - ******
            const string ac1 = "91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203";
            /// compare...
            var h_pwd = AppUtilties.ComputeSha256Hash(vmMod.ChurchCode); // church code checked
            string strUserKeyHashedData = AppUtilties.ComputeSha256Hash(vmMod.ChurchCode + vmMod.Username.Trim().ToLower()); 
            var _oChanges = _context.UserProfile.Include(t => t.AppGlobalOwner).Include(t => t.ChurchBody)
                            .Where(x => ((h_pwd == ac1 && x.AppGlobalOwnerId==null && x.ChurchBodyId==null) || x.ChurchBody.GlobalChurchCode == vmMod.ChurchCode) && 
                                    x.ProfileScope == vmMod.profileScope && x.UserStatus == "A" &&
                                x.Username.Trim().ToLower() == vmMod.Username.Trim().ToLower() && x.UserKey==strUserKeyHashedData).FirstOrDefault();
            
            if (_oChanges == null)
                return Json(new { taskSuccess = false, oCurrId = -1, userMess = "User account was not found. Please refresh and try again.", signOutToLogIn = false });

            //var arrData = "";
            //arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            //vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ResetUserPasswordModel>(arrData) : vmMod;

            //var oUP = _context.UserProfile.Where(c => c.Username == vmMod.Username).FirstOrDefault();  // vmMod.oUserProfile;
            //oUP.ChurchBody = vmMod.oChurchBody;

            try
            { 
                ModelState.Remove("oAppGlolOwnId");
                ModelState.Remove("oChurchBodyId");
                ModelState.Remove("ChurchCode");
                ModelState.Remove("Username");
                ModelState.Remove("CurrentPassword");
                ModelState.Remove("NewPassword");
                ModelState.Remove("RepeatPassword");
                

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", signOutToLogIn = false });

                //var userProList = (from t_upx in _context.UserProfile.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId &&
                //                                                                     c.ProfileScope == _oChanges.ProfileScope && c.Id == _oChanges.Id)
                //                       //  from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.UserProfileId == t_upx.Id).DefaultIfEmpty()
                //                       //  from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.Id == t_upr.UserRoleId && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN").DefaultIfEmpty()
                //                   select t_upx
                //                  ).OrderBy(c => c.UserDesc).ToList();

                //if (userProList.Count == 0)
                //    return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "User account was not found. Please refresh and try again.", signOutToLogIn = false });

                //if (vmMod.AuthTypeUsed == null)
                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please indicate authentication type to confirm user profile.", signOutToLogIn = false });

                vmMod.AuthTypeUsed = 1;
                if (vmMod.AuthTypeUsed == 1)  //2-way
                {
                    if (vmMod.VerificationCode == null)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Enter the verification code.", signOutToLogIn = false });

                    if (vmMod.SentVerificationCode != AppUtilties.ComputeSha256Hash(vmMod.VerificationCode)) //"12345678") // latest code sent to user's email, sms
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Enter correct verification code.", signOutToLogIn = false });
                }

                //else
                //{
                //    var _secAns = AppUtilties.ComputeSha256Hash(vmMod.SecurityQue + vmMod.SecurityAns);
                //    if (vmMod.SecurityQue.ToLower().Equals(vmMod.SecurityQue.ToLower()) && vmMod.SecurityAns.Equals(_secAns))
                //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Security answer provided is not correct.", signOutToLogIn = false });
                //}


                if (_oChanges.Expr != null)
                {
                    if (_oChanges.Expr.Value <= DateTime.Now.Date)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please user account has expired. Activate account first.", signOutToLogIn = false });
                }

                //if (_oChanges.Pwd != AppUtilties.ComputeSha256Hash(vmMod.CurrentPassword))
                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide correct current password", signOutToLogIn = false });

                //if (vmMod.CurrentPassword == null)
                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide current password", signOutToLogIn = false });

                if (vmMod.NewPassword == null)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide new password (minimum 6-digit; use strong passwords:- UPPER and lower cases, digits (0-9) and $pecial characters)", signOutToLogIn = false });

                if (vmMod.RepeatPassword == null)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please confirm user password (minimum 6-digit; use strong passwords:- UPPER and lower cases, digits (0-9) and $pecial characters)", signOutToLogIn = false });

                if (vmMod.NewPassword != vmMod.RepeatPassword)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Password mismatch. Provide user password again.", signOutToLogIn = false });


                var _userTask = "Attempted to changed user password."; var strUserDenom = "Vendor Admin";
                using (var _pwdCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    var cc = "";
                    if (vmMod.ChurchCode == "000000" && _oChanges.AppGlobalOwnerId == null && _oChanges.ChurchBodyId == null && _oChanges.ProfileScope == "V")
                    {
                        cc = "000000";    //var churchCode = "000000"; _oChanges.Username = "SysAdmin"; _oChanges.Pwd = "$ys@dmin1";  
                    }
                    else
                    {
                        var oAGO = _context.MSTRAppGlobalOwner.Find(vmMod.oAppGloOwnId);
                        var oCB = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == vmMod.oAppGloOwnId && c.Id == vmMod.oChurchBodyId).FirstOrDefault();

                        if (oAGO == null || oCB == null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specified denomination and church unit could not be retrieved. Please refresh and try again.", signOutToLogIn = false });
                      
                        if (string.IsNullOrEmpty(oCB.GlobalChurchCode))
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church code not specified. The unique global church code of church unit is required. Please verify with System Admin and try again.", signOutToLogIn = false });

                       
                        strUserDenom = oCB.Name + (!string.IsNullOrEmpty(oAGO.Acronym) ? ", " + oAGO.Acronym : oAGO.OwnerName);
                        strUserDenom = "--" + (string.IsNullOrEmpty(strUserDenom) ? "Denomination: " + strUserDenom : strUserDenom);

                        cc = oCB.GlobalChurchCode;

                        // _oChanges.Pwd = AppUtilties.ComputeSha256Hash(_oChanges.Username + _oChanges.Pwd);
                    }
                     

                    //create user and init...
                    // _oChanges = new UserProfile();
                    //_oChanges.AppGlobalOwnerId = null; // oUP.ChurchBody != null ? oUP.ChurchBody.AppGlobalOwnerId : null;
                    //_oChanges.ChurchBodyId = null; //(int)oUP.ChurchBody.Id;
                    //_oChanges.OwnerId =null; // (int)vmMod.oCurrLoggedUserId;


                    var tm = DateTime.Now;
                    _oChanges.Strt = tm;
                    //_oChanges.Expr = null; // tm.AddDays(90);  //default to 30 days
                    //  oCurrVmMod.oUserProfile.UserId = oCurrChuMemberId_LogOn;
                    //_oChanges.ChurchMemberId = null; // vmMod.oCurrLoggedMemberId;
                    // _oChanges.UserScope = "E"; // I-internal, E-external
                    //_oChanges.ProfileScope = "V"; // V-Vendor, C-Client

                    _oChanges.UserKey = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username.ToLower());
                    //_oChanges.Pwd = "123456";  //temp pwd... to reset @ next login 

                    _oChanges.Pwd = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username.ToLower() + vmMod.NewPassword);

                    _oChanges.ResetPwdOnNextLogOn = false;
                    _oChanges.PwdExpr = tm.AddDays(30);  //default to 90 days 
                    _oChanges.UserStatus = "A"; // A-ctive...D-eactive

                    // _oChanges.Created = tm;
                    _oChanges.LastMod = tm; 
                    //  _oChanges.CreatedByUserId = null; // (int)vmMod.oCurrLoggedUserId;
                    _oChanges.LastModByUserId = null; // (int)vmMod.oCurrLoggedUserId;

                    //cc + model.Username.Trim().ToLower() + model.Password
                
                    //if (vmMod.ChurchCode == "000000")
                    //    _oChanges.Pwd = AppUtilties.ComputeSha256Hash(vmMod.ChurchCode + vmMod.Username.ToLower() + vmMod.NewPassword);
                    // else
                    //    _oChanges.Pwd = AppUtilties.ComputeSha256Hash(vmMod.Username + vmMod.NewPassword);
                 

                    if (vmMod.AuthTypeUsed == 2)
                    {
                        _oChanges.PwdSecurityQue = vmMod.SecurityQue;
                        _oChanges.PwdSecurityAns = vmMod.SecurityAns != null ? AppUtilties.ComputeSha256Hash(vmMod.SecurityAns) : vmMod.SecurityAns;
                    }

                    //_oChanges.UserDesc = "Super Admin";
                    //_oChanges.UserPhoto = null;
                    //_oChanges.UserId = null;
                    //_oChanges.PhoneNum = null;
                    //_oChanges.Email = null; 

                    //  

                    _userTask = "Changed user password successfully - " + _oChanges.Username + strUserDenom;
                   // var _userTask = "Changed user password successfully.";
                   ViewBag.UserMsg = "Password changed successfully.";


                    //save everything
                    //  await _context.SaveChangesAsync();

                    _pwdCtx.UserProfile.Update(_oChanges);
                    //save everything
                    await _pwdCtx.SaveChangesAsync();


                    DetachAllEntities(_pwdCtx);
                }

                //audit...
                var _tm = DateTime.Now;
                _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vmMod.oCurrUserId_Logged, _tm, _tm, vmMod.oCurrUserId_Logged, vmMod.oCurrUserId_Logged));


                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = true });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving user profile details. Err: " + ex.Message, signOutToLogIn = false });
            }
        }
         

    }
}
