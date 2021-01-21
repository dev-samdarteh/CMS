using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RhemaCMS.Models;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using RhemaCMS.Models.MSTRModels;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Controllers.con_adhc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using RhemaCMS.Models.Adhoc;
using Microsoft.AspNetCore.Hosting;
using RhemaCMS.Models.ViewModels.vm_cl;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace RhemaCMS.Controllers.con_ch_config
{
    public class ClientSetupParametersController : Controller
    {
        private readonly MSTR_DbContext _masterContext;
        private ChurchModelContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        ///
        private string _strClientConn;
        private UserProfile _oLoggedUser;
        private UserRole _oLoggedRole;
        private MSTRChurchBody _oLoggedMSTR_CB;
        private MSTRAppGlobalOwner _oLoggedMSTR_AGO;         

        /// localized
        private ChurchBody _oLoggedCB;
        private AppGlobalOwner _oLoggedAGO;

        private bool isCurrValid = false;
        private List<UserSessionPrivilege> oUserLogIn_Priv = null;
        ///        
        private List<DiscreteLookup> dlCBDivOrgTypes = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlShareStatus = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlOwnerStatus = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlGenStatuses = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlChuWorkStats = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlNVPCodes = new List<DiscreteLookup>();

        public ClientSetupParametersController(MSTR_DbContext masterContext, IWebHostEnvironment hostingEnvironment, 
            IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            _masterContext = masterContext;
            // _context = context;

            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
             

            ///
            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "P", Desc = "Pending" });
            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "B", Desc = "Blocked" });
            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "D", Desc = "Deactive" });
            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "E", Desc = "Expired" });

            //SharingStatus { get; set; }  // A - Share with all sub-congregations, C - Share with child congregations only, N - Do not share
            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "N", Desc = "Do not roll-down (share)" });
            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "C", Desc = "Roll-down (share) for direct child congregations" });
            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "A", Desc = "Roll-down (share) for all sub-congregations" });

            // OwnershipStatus { get; set; }  // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody
            dlOwnerStatus.Add(new DiscreteLookup() { Category = "OwnStat", Val = "O", Desc = "Originated" });
            dlOwnerStatus.Add(new DiscreteLookup() { Category = "OwnStat", Val = "I", Desc = "Inherited" });

            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CR", Desc = "Church Root (Denomination)" }); //--CB[church body]
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "GB", Desc = "Governing Body" }); //--CB
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CO", Desc = "Church Office" }); //--CB
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "DP", Desc = "Department" });  //Ministry        -- CSU [church sector unit]
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CG", Desc = "Church Grouping" }); //--CSU
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "SC", Desc = "Standing Committee" }); // Working Committee   -- CSU
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CE", Desc = "Church Enterprise" }); //--CB
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "TM", Desc = "Team" });   // Working Team .. group of roles/pos   -- CR  [church roles]
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CP", Desc = "Church Position" });             // -- CR
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "IB", Desc = "Independent Unit" }); //--CB 
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CH", Desc = "Congregation Head-unit" });  // oversight directly on congregations   -- CB
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CN", Desc = "Congregation" });  //to look up congregation by church code [short or full path]  -- CB

            dlChuWorkStats.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "S", Desc = "Structure Only" });
            dlChuWorkStats.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "O", Desc = "Operationalized" });

            // NVP Tags
            dlNVPCodes.Add(new DiscreteLookup() { Category = "NVPCode", Val = "UOM", Desc = "Units of Measure" });
            dlNVPCodes.Add(new DiscreteLookup() { Category = "NVPCode", Val = "TTL", Desc = "Titles" });
            dlNVPCodes.Add(new DiscreteLookup() { Category = "NVPCode", Val = "ASS_CATEG", Desc = "Asset Categories" });
            dlNVPCodes.Add(new DiscreteLookup() { Category = "NVPCode", Val = "CH_TRNF", Desc = "Church Transfer Settings" });
            dlNVPCodes.Add(new DiscreteLookup() { Category = "NVPCode", Val = "VIS_AGE_BRC", Desc = "Visitor Age Brackets" });
            dlNVPCodes.Add(new DiscreteLookup() { Category = "NVPCode", Val = "CLA_BAP_CNF", Desc = "Baptism and Confirmation Details" });  //CLA_BAP_PRC, CLA_CNF_PRC   [...Practice] = Y/N

        }



        public static string GetStatusDesc(string oCode)
        {
            switch (oCode)
            {
                case "A": return "Active";
                case "B": return "Blocked";
                case "D": return "Deactive";
                case "P": return "Pending";
                case "E": return "Expired";

                default: return oCode;
            }
        }
         
        public static string GetNVPTagDesc(string oCode)
        {
            switch (oCode)
            {
                case "UOM": return "Unit of Measure";
                case "TTL": return "Title";
                case "ASS_CATEG": return "Asset Category";
                case "CH_TRNF": return "Church Transfer Setting";
                case "VIS_AGE_BRC": return "Visitor Age Bracket";
                case "CLA_BAP_CNF": return "Baptism and Confirmation Detail";

                default: return oCode;
            }
        }



        //AuditType -- TRANSACTIONAL = T, NAVIGATIONAL = N, LOGIN /LOGOUT = L 
        public static string GetAuditTypeDesc(string oCode)
        {
            switch (oCode)
            {
                case "L": return "Login/Logout";
                case "N": return "Navigational";
                case "T": return "Transactional";

                default: return oCode;
            }
        }


        public static string GetChuOrgTypeDesc(string oCode)
        {
            switch (oCode)
            {
                case "CR": return "Church Root (Denomination)";
                case "GB": return "Governing Body";
                case "CO": return "Church Office";
                case "DP": return "Church Department";
                case "CG": return "Church Grouping";
                case "SC": return "Standing Committee";
                case "CE": return "Church Enterprise";
                case "TM": return "Team";
                case "CP": return "Church Position";
                case "IB": return "Independent Unit";
                case "CH": return "Congregation Head-unit";
                case "CN": return "Congregation";

                default: return oCode;
            }
        }

        public static object GetChuOrgTypeDetail(string oCode, bool returnSetIndex)
        {
            switch (oCode)
            {
                case "CR": if (returnSetIndex) return 0; else return "Church Root (Denomination)";
                case "GB": if (returnSetIndex) return 1; else return "Governing Body";
                case "CO": if (returnSetIndex) return 2; else return "Church Office";
                case "DP": if (returnSetIndex) return 3; else return "Church Department";
                case "CG": if (returnSetIndex) return 4; else return "Church Grouping";
                case "SC": if (returnSetIndex) return 5; else return "Standing Committee";
                case "CE": if (returnSetIndex) return 6; else return "Church Enterprise";
                case "TM": if (returnSetIndex) return 7; else return "Team";
                case "CP": if (returnSetIndex) return 8; else return "Church Position";
                case "IB": if (returnSetIndex) return 9; else return "Independent Unit";  // Independent Body e.g. Boards, Trustees
                case "CH": if (returnSetIndex) return 10; else return "Congregation Head-unit";
                case "CN": if (returnSetIndex) return 11; else return "Congregation";

                default: return oCode;
            }
        }

        public static string GetChuOrgTypeCode(int setIndex)
        {
            switch (setIndex)
            {
                case 0: return "CR";
                case 1: return "GB";
                case 2: return "CO";
                case 3: return "DP";
                case 4: return "CG";
                case 5: return "SC";
                case 6: return "CE";
                case 7: return "TM";
                case 8: return "CP";
                case 9: return "IB";
                case 10: return "CH";
                case 11: return "CN";

                default: return "";
            }
        }

        public static string GetAdhocStatusDesc(string oCode)
        {
            switch (oCode)
            {
                case "N": return "Do not roll-down (share)";
                case "C": return "Roll-down (share) for direct child congregations";
                case "A": return "Roll-down (share) for all sub-congregations";

                case "T": return "Tenure";
                case "Y": return "Age (years)"; //years

                case "O": return "Originated";
                case "I": return "Inherited";

                case "GA": return "General Activity";//GA-- General activ, ER-Event Role,  MC--Member Churchlife Activity related, EV-Church E-vent related
                case "ER": return "Event Role";
                case "MC": return "Member Churchlife";
                case "EV": return "Event";

                default: return oCode;
            }
        }

        public static string GetDayOfWeeksDesc(string oCode, bool days = false)
        {
            switch (oCode)
            {
                case "Su": return "Sunday";
                case "Mo": if (days) return "Monday"; else return "Monthly";
                case "Tu": return "Tuesday";
                case "We": return "Wednesday";
                case "Th": return "Thursday";
                case "Fr": return "Friday";
                case "Sa": return "Saturday";

                case "Da": return "Daily";
                case "Wk": return "Weekly";
                case "Bw": return "Bi-Weekly";
                //case "Mo": return "Monthly";
                case "Bm": return "Bi-Monthly";
                case "Qt": return "Quarterly";
                case "Yr": return "Yearly";

                default: return oCode;
            }
        }

         
        private bool InitializeUserLogging()
        {
            try
            {
                SetUserLogged();
                if (!isCurrValid) 
                { 
                    ViewData["strUserLoginFailMess"] = "Client user profile validation unsuccessful.";
                    //RedirectToAction("LoginUserAcc", "UserLogin"); 
                    return false;
                }

                if (oUserLogIn_Priv[0].UserProfile == null)
                { 
                    ViewData["strUserLoginFailMess"] = "Client user profile not found. Please try again or contact System Admin";
                    // RedirectToAction("LoginUserAcc", "UserLogin"); 
                    return false;
                }

                // store login in session 
                var _oUserPrivilegeCol = oUserLogIn_Priv;
                var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();
                
                ///
                _oLoggedRole = oUserLogIn_Priv[0].UserRole;
                _oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                _oLoggedMSTR_CB = oUserLogIn_Priv[0].ChurchBody;
                _oLoggedMSTR_AGO = oUserLogIn_Priv[0].AppGlobalOwner;
                _oLoggedUser.strChurchCode_AGO = _oLoggedMSTR_AGO != null ? _oLoggedMSTR_AGO.GlobalChurchCode : "";
                _oLoggedUser.strChurchCode_CB = _oLoggedMSTR_CB != null ? _oLoggedMSTR_CB.GlobalChurchCode : "";

                _context = GetClientDBContext(_oLoggedUser);

                if (_context == null)
                {
                    ViewData["strUserLoginFailMess"] = "Client database connection unsuccessful. Please try again or contact System Admin";
                    // return RedirectToAction("LoginUserAcc", "UserLogin"); 
                    ModelState.AddModelError("", "Client database connection unsuccessful. Please try again or contact System Admin");
                    ///
                    return false;
                    // RedirectToAction("Index", "Home");  //return View(oHomeDash);
                }

                this._strClientConn = _context.Database.GetDbConnection().ConnectionString;

                /// synchronize AGO, CL, CB, CTRY  or @login 

                /// get the localized data... using the MSTR data
                _oLoggedAGO = _context.AppGlobalOwner.AsNoTracking().Where(c => c.MSTR_AppGlobalOwnerId == _oLoggedUser.AppGlobalOwnerId && c.GlobalChurchCode == _oLoggedUser.strChurchCode_AGO).FirstOrDefault();  // one record table...
                _oLoggedCB = _context.ChurchBody.AsNoTracking().Include(t => t.ChurchLevel)
                                            .Where(c => c.MSTR_AppGlobalOwnerId == _oLoggedUser.AppGlobalOwnerId && c.MSTR_ChurchBodyId == _oLoggedUser.ChurchBodyId &&
                                                        c.GlobalChurchCode == _oLoggedUser.strChurchCode_CB).FirstOrDefault();
                
                if (_oLoggedAGO == null || _oLoggedCB == null)
                { 
                    ViewData["strUserLoginFailMess"] = "Client Church unit details could not be verified. Please try again or contact System Admin";
                    ///
                    // RedirectToAction("LoginUserAcc", "UserLogin"); 
                    return false;
                }

                /// master control DB
                ViewData["strAppName"] = "Rhema-CMS";
                ViewData["strAppNameMod"] = "Church Dashboard";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(_oLoggedUser.UserDesc) ? _oLoggedUser.UserDesc : "[Current user]";
                ViewData["oMSTR_AppGloOwnId_Logged"] = _oLoggedUser.AppGlobalOwnerId;
                ViewData["oMSTR_ChurchBodyId_Logged"] = _oLoggedUser.ChurchBodyId;
                ViewData["strAppCurrUser_ChRole"] = _oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = _oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUserPhoto_Filename"] = _oLoggedUser.UserPhoto;
                ///
                /// client control DB
                ViewData["oAppGloOwnId_Logged"] = _oLoggedAGO.Id;
                ViewData["oChurchBodyId_Logged"] = _oLoggedCB.Id;
                ViewData["oChurchBodyOrgType_Logged"] = _oLoggedCB.OrganisationType;
                ViewData["strClientLogo_Filename"] = _oLoggedAGO?.ChurchLogo;
                ViewData["strAppLogo_Filename"] = "~/frontend/dist/img/rhema_logo.png"; // oAppGloOwn?.ChurchLogo;
                ViewData["strClientChurchName"] = _oLoggedAGO.OwnerName;
                ViewData["strClientBranchName"] = _oLoggedCB.Name;
                ViewData["strClientChurchLevel"] = !string.IsNullOrEmpty(_oLoggedCB.ChurchLevel?.CustomName) ? _oLoggedCB.ChurchLevel?.CustomName : _oLoggedCB.ChurchLevel?.Name;  // Assembly, Presbytery etc

                // refreshValues...
                // LoadClientDashboardValues(this._strClientConn, this._oLoggedUser);

                return true;
            }

            catch (Exception)
            {
                throw;
            }
        }
         

        private bool userAuthorized = false;
        private void SetUserLogged()
        { 
            if (TempData == null)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

                if (tempData.ContainsKey("UserLogIn_oUserPrivCol"))
                {
                    var tempPrivList = tempData["UserLogIn_oUserPrivCol"] as string;
                    if (string.IsNullOrEmpty(tempPrivList)) RedirectToAction("LoginUserAcc", "UserLogin");
                    // De serialize the string to object
                    this.oUserLogIn_Priv = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);
                    //
                    isCurrValid = oUserLogIn_Priv?.Count > 0;
                    if (isCurrValid)
                    {
                        //ViewBag.oAppGloOwnLogged = oUserLogIn_Priv[0].AppGlobalOwner;
                        //ViewBag.oChuBodyLogged = oUserLogIn_Priv[0].ChurchBody;
                        //ViewBag.oUserLogged = oUserLogIn_Priv[0].UserProfile;

                        // check permission for Core life...  given the sets of permissions
                        userAuthorized = oUserLogIn_Priv.Count > 0; //(oUserLogIn_Priv.Find(x => x.PermissionName == "_A0__System_Administration" || x.PermissionName == "xxx") != null);
                    }
                }

                else RedirectToAction("LoginUserAcc", "UserLogin");
            }
            else
            {
                if (TempData.ContainsKey("UserLogIn_oUserPrivCol"))
                {
                    var tempPrivList = TempData["UserLogIn_oUserPrivCol"] as string;
                    if (string.IsNullOrEmpty(tempPrivList)) RedirectToAction("LoginUserAcc", "UserLogin");
                    // De serialize the string to object
                    this.oUserLogIn_Priv = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);
                    //
                    isCurrValid = oUserLogIn_Priv?.Count > 0;
                    if (isCurrValid)
                    {
                        //ViewBag.oAppGloOwnLogged = oUserLogIn_Priv[0].AppGlobalOwner;
                        //ViewBag.oChuBodyLogged = oUserLogIn_Priv[0].ChurchBody;
                        //ViewBag.oUserLogged = oUserLogIn_Priv[0].UserProfile;

                        // check permission for Core life...  given the sets of permissions
                        userAuthorized = oUserLogIn_Priv.Count > 0; //(oUserLogIn_Priv.Find(x => x.PermissionName == "_A0__System_Administration" || x.PermissionName == "xxx") != null);
                    }
                }

                else RedirectToAction("LoginUserAcc", "UserLogin");
            } 
        }


        private ChurchModelContext GetClientDBContext(UserProfile oUserLogged)
        {
            var oClientConfig = _masterContext.ClientAppServerConfig.Where(c => c.AppGlobalOwnerId == oUserLogged.AppGlobalOwnerId && c.Status == "A").FirstOrDefault();
            if (oClientConfig != null)
            {
                // get and mod the conn
                var _clientDBConnString = "";
                var conn = new SqlConnectionStringBuilder(_masterContext.Database.GetDbConnection().ConnectionString);
                conn.DataSource = oClientConfig.ServerName; conn.InitialCatalog = oClientConfig.DbaseName; conn.UserID = oClientConfig.SvrUserId; conn.Password = oClientConfig.SvrPassword; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
                _clientDBConnString = conn.ConnectionString;

                // test the NEW DB conn
                var _clientContext = new ChurchModelContext(_clientDBConnString);
                if (_clientContext.Database.CanConnect())
                    return _clientContext;
            }

            //
            return null;
        }

        private void LoadClientDashboardValues(string clientDBConnString, UserProfile oLoggedUser)
        {
            // using (var dashContext = new ChurchModelContext(clientDBConnString))
            using (var clientContext = new ChurchModelContext(clientDBConnString)) // ("Server=RHEMA-SDARTEH;Database=DBRCMS_MS_TEST;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true") ) // AppUtilties.GetNewDBContext_MS(_context, "DBRCMS_CL_TEST"))  // MSTR_DbContext()) //
            {
                if (clientContext.Database.CanConnect() == false) clientContext.Database.OpenConnection();
                else if (clientContext.Database.GetDbConnection().State != System.Data.ConnectionState.Open) clientContext.Database.OpenConnection();

                //get Currency
                var curr = clientContext.Currency.Where(c => c.AppGlobalOwnerId == oLoggedUser.AppGlobalOwnerId && c.ChurchBodyId == oLoggedUser.ChurchBodyId && c.IsBaseCurrency == true).FirstOrDefault();
                ViewData["CB_CurrUsed"] = curr != null ? curr.Acronym : ""; // "GHS"

                var clientAGO = clientContext.AppGlobalOwner.Where(c => c.MSTR_AppGlobalOwnerId == oLoggedUser.AppGlobalOwnerId && c.Status == "A").FirstOrDefault();
                var clientCB = clientContext.ChurchBody.Where(c => c.MSTR_AppGlobalOwnerId == oLoggedUser.AppGlobalOwnerId && c.MSTR_ChurchBodyId == oLoggedUser.ChurchBodyId && c.Status == "A").FirstOrDefault();
                ///
                var qrySuccess = false;
                if (clientAGO != null && clientCB != null)
                {
                    var res = (from dummyRes in new List<string> { "X" }
                               join tcb_sb in clientContext.ChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN") &&
                                                   c.AppGlobalOwnerId == clientAGO.Id && c.ParentChurchBodyId == clientCB.Id) on 1 equals 1 into _tcb_sb
                               // join tcb in clientContext.MSTRChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN")) on 1 equals 1 into _tcb
                               // join tsr in clientContext.UserRole.Where(c => c.RoleStatus == "A" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null) on 1 equals 1 into _tsr
                               //join tcm in clientContext.ChurchMember.Where(c => c.Status == "A" &&
                               //                    c.AppGlobalOwnerId == clientAGO.Id && c.ChurchBodyId == clientCB.Id) on 1 equals 1 into _tcm
                               // join tms in clientContext.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A") on 1 equals 1 into _tms
                               // join tsubs in clientContext.AppSubscription.Where(c => c.Slastatus == "A") on 1 equals 1 into _tsubs
                               // join ttc in clientContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date) on 1 equals 1 into _ttc
                               // join tdb in clientContext.ClientAppServerConfig.Select(c => c.DbaseName).Distinct() on 1 equals 1 into _tdb

                               select new
                               {
                                   cnt_tcb_sb = _tcb_sb.Count(),
                                   // cnt_tcm = _tcm.Count(),
                                   ///
                                   //cnt_tms = _tms.Count(),
                                   //cnt_tsubs = _tsubs.Count(),
                                   //cnt_tdb = _tdb.Count(),
                                   // cnt_ttc = _ttc.Count(),
                                   //cnt_tcln_d = _tcln_d.Count(),
                                   //cnt_tcln_a = _tcln_a.Count()
                               })
                            .ToList();
                    //.ToListAsync();

                    ///
                    if (res.Count() > 0)
                    {
                        qrySuccess = true;
                        ViewData["CB_SubCongCount"] = String.Format("{0:N0}", res[0].cnt_tcb_sb);
                        ViewData["CB_MemListCount"] = String.Format("{0:N0}", 100); // res[0].cnt_tcm); 
                        ViewData["CBWeek_NewMemListCount"] = String.Format("{0:N0}", 100); // res[0].cnt_tsubs);
                        ViewData["CBWeek_NewConvertsCount"] = String.Format("{0:N0}", 100); //res[0].cnt_tdb);
                        ViewData["CBWeek_VisitorsCount"] = String.Format("{0:N0}", 100); //res[0].cnt_tcln_a);
                        ViewData["CBWeek_ReceiptsAmt"] = String.Format("{0:N0}", 100); //res[0].cnt_tcln_d);
                        ViewData["CBWeek_PaymentsAmt"] = String.Format("{0:N0}", 100); //res[0].cnt_tcln_d); 
                    }

                    var resAudits = _masterContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date);
                    // var cnt_ttc = resAudits.Count();
                    ViewData["TodaysAuditCount"] = String.Format("{0:N0}", resAudits.Count());


                    ////String.Format(1234 % 1 == 0 ? "{0:N0}" : "{0:N2}", 1234);
                    //var curr = _context.Currency.Where(c => c.AppGlobalOwnerId == oAppGloOwnId_Logged && c.ChurchBodyId == oChuBodyId_Logged && c.IsBaseCurrency == true).FirstOrDefault(); 
                    //oHomeDash.strCurrUsed = curr != null ? curr.Acronym : ""; // "GHS";
                    //oHomeDash.SupCongCount = String.Format("{0:N0}", 25);
                    //oHomeDash.MemListCount = String.Format("{0:N0}", 4208); ViewBag.MemListCount = oHomeDash.MemListCount;
                    //oHomeDash.NewMemListCount = String.Format("{0:N0}", 17); ViewBag.NewMemListCount = oHomeDash.NewMemListCount;
                    //oHomeDash.NewConvertsCount = String.Format("{0:N0}", 150); ViewBag.NewConvertsCount = oHomeDash.NewConvertsCount;
                    //oHomeDash.VisitorsCount = String.Format("{0:N0}", 9); ViewBag.VisitorsCount = oHomeDash.VisitorsCount;
                    //oHomeDash.ReceiptsAmt = String.Format("{0:N2}", 1700);
                    //oHomeDash.PaymentsAmt = String.Format("{0:N2}", 105.491); 
                }

                if (!qrySuccess)
                {
                    ViewData["numCB_SubCongCount"] = String.Format("{0:N0}", 0);
                    ViewData["numCB_MemListCount"] = String.Format("{0:N0}", 0);
                    ViewData["numCBWeek_NewMemListCount"] = String.Format("{0:N0}", 0);
                    ViewData["numCBWeek_NewConvertsCount"] = String.Format("{0:N0}", 0);
                    ViewData["numCBWeek_VisitorsCount"] = String.Format("{0:N0}", 0);
                    ViewData["numCBWeek_ReceiptsAmt"] = String.Format("{0:N0}", 0);
                    ViewData["numCBWeek_PaymentsAmt"] = String.Format("{0:N0}", 0);
                    ///
                    ViewData["numCBToday_AuditCount"] = String.Format("{0:N0}", 0);
                }

                // close connection
                clientContext.Database.CloseConnection();
            }
        }



        private async Task LogUserActivity_AppMainUserAuditTrail(UserAuditTrail oUserTrail)
        { // var oUserTrail = _masterContext.UserAuditTrail.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null && churchCode=="000000") || (c.AppGlobalOwnerId== oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId))
            if (oUserTrail != null)
            {
                // var tempCtx = _context;
                using (var logCtx = new MSTR_DbContext(_masterContext.Database.GetDbConnection().ConnectionString)) // ("Server=RHEMA-SDARTEH;Database=DBRCMS_MS_TEST;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true") ) // AppUtilties.GetNewDBContext_MS(_context, "DBRCMS_CL_TEST"))  // MSTR_DbContext()) //
                {
                    if (logCtx.Database.CanConnect() == false) logCtx.Database.OpenConnection();
                    else if (logCtx.Database.GetDbConnection().State != System.Data.ConnectionState.Open) logCtx.Database.OpenConnection();

                    // var a = logCtx.Database.GetDbConnection().ConnectionString;
                    // var b = _masterContext.Database.GetDbConnection().ConnectionString;

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


        public void DetachAllEntities(ChurchModelContext ctx)
        {
            var changedEntriesCopy = ctx.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        private static bool IsAncestor_ChurchBody(ChurchBody oAncestorChurchBody, ChurchBody oCurrChurchBody)
        {
            if (oAncestorChurchBody == null || oCurrChurchBody == null) return false;
            //string ChurchCodeFullPath { get; set; }  //R0000-0000-0000-0000-0000-0000 
            if (oAncestorChurchBody.Id == oCurrChurchBody.ParentChurchBodyId) return true;
            if (string.IsNullOrEmpty(oAncestorChurchBody.RootChurchCode) || string.IsNullOrEmpty(oCurrChurchBody.RootChurchCode)) return false;

            string[] arr = new string[] { oCurrChurchBody.RootChurchCode };
            if (oCurrChurchBody.RootChurchCode.Contains("--")) arr = oCurrChurchBody.RootChurchCode.Split("--");  // else it should be the ROOT... and would not get this far

            if (arr.Length > 0)
            {
                var ancestorCode = oAncestorChurchBody.RootChurchCode;
                var tempCode = oCurrChurchBody.RootChurchCode;
                var k = arr.Length - 1;
                for (var i = arr.Length - 1; i >= 0; i--)
                {
                    if (tempCode.Contains("--" + arr[i])) tempCode = tempCode.Replace("--" + arr[i], "");
                    if (string.Compare(ancestorCode, tempCode) == 0) return true;
                }
            }

            return false;
        }

        private static bool IsAncestor_ChurchBody(string strAncestorRootCode, string strCurrChurchBodyRootCode, int? strAncestorId = null, int? strCurrChurchBodyId = null)
        {
            // if (oAncestorChurchBody == null) return false;
            //string ChurchCodeFullPath { get; set; }  //R0000-0000-0000-0000-0000-0000 

            if (strCurrChurchBodyId != null && strAncestorId == strCurrChurchBodyId) return true;

            if (string.IsNullOrEmpty(strAncestorRootCode) || string.IsNullOrEmpty(strCurrChurchBodyRootCode)) return false;

            string[] arr = new string[] { strCurrChurchBodyRootCode };
            if (strCurrChurchBodyRootCode.Contains("--")) arr = strCurrChurchBodyRootCode.Split("--");

            if (arr.Length > 0)
            {
                var ancestorCode = strAncestorRootCode;
                var tempCode = strCurrChurchBodyRootCode;
                var k = arr.Length - 1;
                for (var i = arr.Length - 1; i >= 0; i--)
                {
                    if (tempCode.Contains("--" + arr[i])) tempCode = tempCode.Replace("--" + arr[i], "");
                    if (string.Compare(ancestorCode, tempCode) == 0) return true;
                }
            }

            return false;
        }


        private ClientSetupParametersModel GetInitialClientSetupList()
        {
            try
            {
                var arrData = "";
                arrData = TempData.ContainsKey("oVmCSPModel") ? TempData["oVmCSPModel"] as string : arrData;
                var vm = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ClientSetupParametersModel>(arrData) : null;
                ///
                if (vm != null) return vm;


                /// once it gets to this point... then the details not found!
                return new ClientSetupParametersModel();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ClientSetupParametersModel LoadClientSetupList(int setIndex = 0, int subSetIndex = 0, int filterIndex = 1) //, bool loadSectionOnly=false string clientDBConnString, UserProfile oLoggedUser)
        {
            try
            {
                var oCSPModel = GetInitialClientSetupList(); // new ClientSetupParametersModel();

                ///
                if (setIndex == 0 || setIndex == 1)
                { 
                    //var oAGO_List = _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == _oLoggedAGO.Id).ToList();
                    //if (oAGO_List.Count() == 0) return null;
                    /////
                    //var oCFTC_MSTRList = _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FC").ToList();
                    //var oCFTS_MSTRList = _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FS").ToList(); 
                    ///
                    var oAGO_MDL = (
                              from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == _oLoggedAGO.Id) // oAGO_List.Take(1).ToList()
                              //from t_cftc in oCFTC_MSTRList.Where(c => c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()
                              //from t_cfts in oCFTS_MSTRList.Where(c => c.Id == t_cftc.FaithTypeClassId).DefaultIfEmpty()
                              from t_ctry in _context.Country.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()

                                  // from t_cft in _context.ChurchFaithType.Include(t => t.FaithTypeClass).AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()

                            select new AppGlobalOwnerModel()
                              {
                                  oAppGlobalOwn = t_ago,
                                // lsChurchLevels = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == t_ago.Id).ToList(),
                                //       
                                numTotalChurchLevelsConfig = _context.ChurchLevel.Count(c => c.AppGlobalOwnerId == t_ago.Id),
                                //TotalCongregations = _context.ChurchBody.Count(c => c.AppGlobalOwnerId == t_ago.Id && c.Status == "A"),
                                // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 
                               
                                  strAppGloOwn = t_ago.OwnerName,
                                  //strFaithTypeCategory = t_cftc != null ? t_ago.strFaithTypeCategory : "",
                                  //strChurchStream = t_ago.strFaithTypeStream,
                                  strCountry = t_ctry != null ? (!string.IsNullOrEmpty(t_ctry.EngName) ? t_ctry.EngName : t_ctry.CtryAlpha3Code) : t_ago.CtryAlpha3Code,
                                  strSlogan = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(0, t_ago.Slogan.IndexOf("|"))).Replace("|", "") : t_ago.Slogan,
                                  strSloganResponse = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(t_ago.Slogan.IndexOf("|"))).Replace("|", "") : "",
                               // strFaithTypeCategory = t_ago != null ? ((!string.IsNullOrEmpty(t_ago.strFaithTypeCategory) && !string.IsNullOrEmpty(t_ago.strFaithTypeStream) ? t_ago.strFaithTypeCategory + ", " + t_ago.strFaithTypeStream : t_ago.strFaithTypeCategory + t_ago.strFaithTypeStream).Trim()) : "",
                                //   
                                blStatusActivated = t_ago.Status == "A",
                                  strStatus = GetStatusDesc(t_ago.Status)
                              })
                          .FirstOrDefault();

                    oCSPModel.oAppGlobalOwnModel = oAGO_MDL; // GetAddOrEdit_AGO(_oLoggedAGO.Id, _oLoggedAGO.Id, _oLoggedCB.Id, _oLoggedUser.Id);

                    /// specific parameter...
                    if (setIndex != 0) return oCSPModel;
                }


                // CL    
                if (setIndex == 0 || setIndex == 2)
                {
                    var oCL_List = (
                         from t_cl in _context.ChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == _oLoggedAGO.Id)
                         from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == t_cl.AppGlobalOwnerId).DefaultIfEmpty()
                         from t_ci_ago in _context.ContactInfo.Include(t => t.Country).AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cl.AppGlobalOwnerId && c.ChurchBodyId == null && c.Id == t_ago.ContactInfoId).DefaultIfEmpty()

                         select new ChurchLevelModel()
                         {
                             oChurchLevel = t_cl,
                             numChurchLevel = t_cl.LevelIndex,
                             strChurchLevelName = !string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name,
                             strAppGloOwn = t_ago.OwnerName + (!string.IsNullOrEmpty(t_ago.OwnerName) && t_ci_ago.Country != null ? ", " + t_ci_ago.Country.EngName : (t_ci_ago.Country != null ? t_ci_ago.Country.EngName : "")),
                         })
                         .OrderBy(c => c.numChurchLevel)
                         .ToList();


                    oCSPModel.lsChurchLevelModels = oCL_List; 
                    //ViewData["oSetupData_CL_List"] = oCL_List;

                    /// specific parameter...
                    if (setIndex != 0) return oCSPModel;
                }


                // CB  ... subscribed body

                //var oCFTC_MSTRList = _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FC").ToList();
                //var oCFTS_MSTRList = _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FS").ToList();
                //var oCFT_List = (from t_cftc in _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FC")
                //                 from t_cfts in _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FS" && c.Id == t_cftc.FaithTypeClassId)
                //                 select new
                //                 {
                //                     FaithCategoryId = t_cftc.Id,
                //                     FaithClassId = t_cfts != null ? t_cfts.Id : (int?)null,
                //                     strFaithCategory = t_cftc.FaithDescription,
                //                     strFaithStream = t_cfts != null ? t_cfts.FaithDescription : ""
                //                 })
                //                 .ToList();

                if (setIndex == 0 || setIndex == 3)
                {  // view only the subscribed CB
                    var oCB_List = (
                        from t_cb in _context.ChurchBody.Include(t => t.CountryRegion).Include(t => t.Country).AsNoTracking()
                                .Where(c => c.AppGlobalOwnerId == _oLoggedAGO.Id && c.Id == _oLoggedCB.Id) // && (c.OrganisationType == "CH" || c.OrganisationType == "CN")c.OrganisationType == "CR" ||  // jux for structure
                        from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == t_cb.AppGlobalOwnerId)
                        from t_cl in _context.ChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb.ChurchLevelId).DefaultIfEmpty()
                        from t_cb_p in _context.ChurchBody.AsNoTracking()
                                .Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb.ParentChurchBodyId && (c.OrganisationType != "CN")).DefaultIfEmpty()  // (c.OrganisationType != "CR" || c.OrganisationType == "CH" || c.OrganisationType == "CN")
                        from t_cl_p in _context.ChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb_p.ChurchLevelId).DefaultIfEmpty()
                        from t_ci in _context.ContactInfo.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.ChurchBodyId == t_cb.Id && c.Id == t_cb.ContactInfoId).DefaultIfEmpty()
                        from t_ci_ago in _context.ContactInfo.Include(t => t.Country).AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cl.AppGlobalOwnerId && c.ChurchBodyId == null && c.Id == t_ago.ContactInfoId).DefaultIfEmpty()

                        select new ChurchBodyModel()
                        {
                            oAppGlobalOwn = t_ago,
                            oChurchBody = t_cb,
                            strOrgType = GetChuOrgTypeDesc(t_cb.OrganisationType),
                            strParentChurchBody = t_cb_p.Name,
                            //
                            //
                           // strFaithTypeCategory = t_cftc != null ? t_cftc.FaithDescription : "",
                           // strChurchStream = t_cfts != null ? t_cfts.FaithDescription : "", 
                            
                            strFaithTypeCategory = t_ago.strFaithTypeCategory, // t_ago != null ? ((!string.IsNullOrEmpty(t_ago.strFaithTypeCategory) && !string.IsNullOrEmpty(t_ago.strFaithTypeStream) ? t_ago.strFaithTypeCategory + ", " + t_ago.strFaithTypeStream : t_ago.strFaithTypeCategory + t_ago.strFaithTypeStream).Trim()) : "",
                            strCountry = t_cb.Country != null ? (!string.IsNullOrEmpty(t_cb.Country.EngName) ? t_cb.Country.EngName : t_cb.Country.CtryAlpha3Code) : t_ago.CtryAlpha3Code,  //t_cb.Country != null ? t_cb.Country.EngName : "",
                            strCountryRegion = t_cb.CountryRegion != null ? t_cb.CountryRegion.Name : "",
                            strAppGloOwn = t_ago.OwnerName + (!string.IsNullOrEmpty(t_ago.OwnerName) && t_ci_ago.Country != null ? ", " + t_ci_ago.Country.EngName : (t_ci_ago.Country != null ? t_ci_ago.Country.EngName : "")),
                            strChurchBody = t_cb.Name,
                            strParentCB_HeaderDesc = !string.IsNullOrEmpty(t_cl.CustomName) ? t_cl_p.CustomName : "Parent Unit",
                            strChurchLevel = (t_cb.ChurchLevelId == null && t_cb.OrganisationType == "CR") ? "Church Root" : (!string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name),
                            numChurchLevel_Index = t_cl.LevelIndex,
                            // strCongLoc = t_ci.Location + (!string.IsNullOrEmpty(t_ci.Location) && !string.IsNullOrEmpty(t_ci.City) ? ", " + t_ci.City : t_ci_ago.City),
                            strCongLoc = (!string.IsNullOrEmpty(t_ci.Location) && !string.IsNullOrEmpty(t_ci.City) ? t_ci.Location + ", " + t_ci.City : t_ci.Location + t_ci.City).Trim(),
                            strCongLoc2 = (t_cb.CountryRegion != null && t_cb.Country != null ? t_cb.CountryRegion.Name + ", " + t_cb.Country.EngName : t_cb.CountryRegion.Name + t_cb.Country.EngName).Trim(),
                            blStatusActivated = t_cb.Status == "A",
                            dtCreated = t_cb.Created,
                            //   
                            strStatus = GetStatusDesc(t_cb.Status)
                        })
                        .OrderByDescending(c => c.dtCreated) //.OrderBy(c => c.strCountry).OrderBy(c => c.numCLIndex).OrderBy(c => c.strChurchBody)
                        .ToList();


                    oCSPModel.lsChurchBodyModels = oCB_List;
                   // oCSPModel.oChurchBodyModel = oCB_List.Count() > 0 ? oCB_List[0] : null;
                    // ViewData["oSetupData_CN_List"] = oCB_List;


                    var oCBModel = oCB_List.Count() > 0 ? oCB_List[0] : null;
                    oCBModel.oCBLevelCount = oCBModel.numChurchLevel_Index - 1;        // oCBLevelCount -= 2;  // less requesting CB
                    List<ChurchLevel> oCBLevelList = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.LevelIndex > 0 && c.LevelIndex < oCBModel.numChurchLevel_Index).ToList().OrderBy(c => c.LevelIndex).ToList();
                    ///
                    if (oCBModel.oCBLevelCount > 0 && oCBLevelList.Count > 0)
                    {
                        oCBModel.strChurchLevel_1 = !string.IsNullOrEmpty(oCBLevelList[0].CustomName) ? oCBLevelList[0].CustomName : oCBLevelList[0].Name;
                        ViewBag.strChurchLevel_1 = oCBModel.strChurchLevel_1;
                        ///
                        var oCB_1 = _context.ChurchBody.Include(t => t.ChurchLevel)
                                          .Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && // c.Status == "A" && 
                                                c.ChurchLevel.LevelIndex == 1 && c.OrganisationType == "CR") //c.ChurchLevelId == oCBLevelList[0].Id &&
                                          .FirstOrDefault();

                        if (oCB_1 != null)
                        { oCBModel.ChurchBodyId_1 = oCB_1.Id; oCBModel.strChurchBody_1 = oCB_1.Name + " [Church Root]"; }

                        ViewBag.ChurchBodyId_1 = oCBModel.ChurchBodyId_1;
                        ViewBag.strChurchBody_1 = oCBModel.strChurchBody_1;

                        ///
                        if (oCBModel.oCBLevelCount > 1)
                        {
                            oCBModel.strChurchLevel_2 = !string.IsNullOrEmpty(oCBLevelList[1].CustomName) ? oCBLevelList[1].CustomName : oCBLevelList[1].Name;
                            ViewBag.strChurchLevel_2 = oCBModel.strChurchLevel_2;
                            ///
                            var lsCB_2 = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[1].Id).ToList();
                            var oCB_2 = lsCB_2.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                            if (oCB_2.Count() != 0)
                            { oCBModel.ChurchBodyId_2 = oCB_2[0].Id; oCBModel.strChurchBody_2 = oCB_2[0].Name; }
                            ViewBag.ChurchBodyId_2 = oCBModel.ChurchBodyId_2; ViewBag.strChurchBody_2 = oCBModel.strChurchBody_2;
                              
                            if (oCBModel.oCBLevelCount > 2)
                            {
                                oCBModel.strChurchLevel_3 = !string.IsNullOrEmpty(oCBLevelList[2].CustomName) ? oCBLevelList[2].CustomName : oCBLevelList[2].Name;
                                ViewBag.strChurchLevel_3 = oCBModel.strChurchLevel_3;

                                var lsCB_3 = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[2].Id).ToList();
                                var oCB_3 = lsCB_3.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                if (oCB_3.Count() != 0)
                                { oCBModel.ChurchBodyId_3 = oCB_3[0].Id; oCBModel.strChurchBody_3 = oCB_3[0].Name;}
                                ViewBag.ChurchBodyId_3 = oCBModel.ChurchBodyId_3; ViewBag.strChurchBody_3 = oCBModel.strChurchBody_3; 


                                if (oCBModel.oCBLevelCount > 3)
                                {
                                    oCBModel.strChurchLevel_4 = !string.IsNullOrEmpty(oCBLevelList[3].CustomName) ? oCBLevelList[3].CustomName : oCBLevelList[3].Name;
                                    ViewBag.strChurchLevel_4 = oCBModel.strChurchLevel_4;

                                    var lsCB_4 = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[3].Id).ToList();
                                    var oCB_4 = lsCB_4.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                    if (oCB_4.Count() != 0)
                                    { oCBModel.ChurchBodyId_4 = oCB_4[0].Id; oCBModel.strChurchBody_4 = oCB_4[0].Name;}
                                    ViewBag.ChurchBodyId_4 = oCBModel.ChurchBodyId_4; ViewBag.strChurchBody_4 = oCBModel.strChurchBody_4; 


                                    if (oCBModel.oCBLevelCount > 4)
                                    {
                                        oCBModel.strChurchLevel_5 = !string.IsNullOrEmpty(oCBLevelList[4].CustomName) ? oCBLevelList[4].CustomName : oCBLevelList[4].Name;
                                        ViewBag.strChurchLevel_5 = oCBModel.strChurchLevel_4;

                                        var lsCB_5 = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[4].Id).ToList();
                                        var oCB_5 = lsCB_5.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                        if (oCB_5.Count() != 0)
                                        { oCBModel.ChurchBodyId_5 = oCB_5[0].Id; oCBModel.strChurchBody_5 = oCB_5[0].Name;}
                                        ViewBag.ChurchBodyId_5 = oCBModel.ChurchBodyId_5; ViewBag.strChurchBody_5 = oCBModel.strChurchBody_5; 
                                    }
                                }
                            }
                        }
                    }

                     

                    oCSPModel.oChurchBodyModel = oCBModel;

                    /// specific parameter...
                    if (setIndex != 0) return oCSPModel;
                }


                // NVP      
                if (setIndex == 0 || setIndex == 6)
                {
                    var oNVP_List = (
                                    from t_nvp in _context.AppUtilityNVP.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.AppGlobalOwnerId == this._oLoggedAGO.Id && c.ChurchBodyId == this._oLoggedCB.Id)
                                    from t_nvp_c in _context.AppUtilityNVP.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_nvp.AppGlobalOwnerId && c.Id == t_nvp.NVPCategoryId).DefaultIfEmpty()

                                    select new AppUtilityNVPModel()
                                    {
                                        oAppUtilityNVP = t_nvp,
                                        strAppGloOwn = t_nvp.AppGlobalOwner != null ? t_nvp.AppGlobalOwner.OwnerName : "",
                                        //strNVPCode = t_nvp.NVPCode,
                                        strNVPTag = GetNVPTagDesc(t_nvp.NVPCode),
                                       // numOrderIndex =  (int)t_nvp.OrderIndex,
                                        // strAppUtilityNVPName = t_nvp.NVPValue,
                                        strNVPCategory = t_nvp_c != null ? t_nvp_c.NVPValue : "",
                                        strNVPStatus = GetStatusDesc(t_nvp.NVPStatus)
                                    })
                                    //.OrderBy(c => c.strNVPCode).ThenBy(c => c.numOrderIndex).ThenBy(c => c.NVPValue)
                                    .ToList();

                    if (oNVP_List.Count > 0)
                        oNVP_List = oNVP_List
                                        .OrderBy(c => c.oAppUtilityNVP.NVPCode).ThenBy(c => c.oAppUtilityNVP.OrderIndex).ThenBy(c => c.oAppUtilityNVP.NVPValue)
                                        .ToList();
                     

                    oCSPModel.lsAppUtilityNVPModels = oNVP_List;
                    ViewData["oSetupData_GAS_List"] = oNVP_List;


                    /// specific parameter...
                    if (setIndex != 0 ) return oCSPModel;
                }


                // CRC_CTRY, CRC_CURR 
                if (setIndex == 0 || setIndex == 7)
                { 
                    // CRC_ctry  - customized to CB ... using the default country config with AGO/CB
                    if (setIndex == 0 || subSetIndex == 1 ) // CTRY ..all/cus : 1/2
                    {
                        var oCRC_CTRY_List = (
                                                    from t_ctry in _context.Country.AsNoTracking() 
                                                    from t_ctry_c in _context.CountryCustom.AsNoTracking().Include(t => t.AppGlobalOwner)
                                                        .Where(c => (c.AppGlobalOwnerId == this._oLoggedAGO.Id && c.ChurchBodyId == this._oLoggedCB.Id && c.CtryAlpha3Code == t_ctry.CtryAlpha3Code) &&
                                                                        (filterIndex == 1 || (filterIndex == 2 && c.IsDisplay==true) || (filterIndex == 3 && c.IsChurchCountry == true) || (filterIndex == 4 && c.IsDefaultCountry == true))
                                                                        ).DefaultIfEmpty()

                                                    select new CountryModel()
                                                    {
                                                        oCountry = t_ctry,
                                                        strCountry = t_ctry != null ? (!string.IsNullOrEmpty(t_ctry.EngName) ? t_ctry.EngName : t_ctry.CtryAlpha3Code) : t_ctry.CtryAlpha3Code,
                                                        isCustomDisplay = t_ctry_c != null
                                                    })
                                                    .OrderBy(c => c.strCountry)
                                                    .ToList();

                        var oCTRYModel = new CountryModel();
                        oCTRYModel.lsCountryModels = oCRC_CTRY_List;
                        oCTRYModel.pageIndex = 1;
                        oCSPModel.oCountryModel = oCTRYModel;
                        ViewData["oCountryModel"] = oCTRYModel;
                        ViewData["oSetupData_CRC_CTRY_List"] = oCRC_CTRY_List;
                    }
                     
                    if (subSetIndex == 0 || subSetIndex == 2  ) // RGN ..all/cus : 1/2  - customized to CB ... using the default country config with AGO/CB
                    {
                        var oCRC_CTRY_RGN_List = (
                                                    from t_ctry_regn in _context.CountryRegion.AsNoTracking().Include(t => t.AppGlobalOwner).Include(t => t.ChurchBody)
                                                    from t_ctry in _context.Country.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ctry_regn.CtryAlpha3Code)
                                                        //(c.OwnedByChurchBodyId == _oLoggedCB.Id ||
                                                        //(c.OwnedByChurchBodyId != _oLoggedCB.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == _oLoggedCB.ParentChurchBodyId) ||
                                                        //(c.OwnedByChurchBodyId != _oLoggedCB.Id && c.SharingStatus == "A"))) //&& IsAncestor_ChurchBody(c.OwnedByChurchBody, _oLoggedCB))))

                                                    select new CountryRegionModel()
                                                    {
                                                        oAppGloOwnId = t_ctry_regn.AppGlobalOwnerId,
                                                        oAppGlobalOwn = t_ctry_regn.AppGlobalOwner,
                                                        oChurchBodyId = t_ctry_regn.ChurchBodyId,
                                                        oChurchBody = t_ctry_regn.ChurchBody,
                                                        ///
                                                        strCountryRegion = t_ctry_regn != null ? t_ctry_regn.Name : "",
                                                        oCountryRegion = t_ctry_regn,
                                                        strCountry = t_ctry != null ? (!string.IsNullOrEmpty(t_ctry.EngName) ? t_ctry.EngName : t_ctry.CtryAlpha3Code) : t_ctry.CtryAlpha3Code
                                                    })
                                                    .OrderBy(c => c.strCountryRegion)
                                                    .ToList();

                        oCSPModel.lsCountryRegionModels = oCRC_CTRY_RGN_List;
                        ViewData["oSetupData_CRC_CTRY_RGN_List"] = oCRC_CTRY_RGN_List;
                    }

                    if (subSetIndex == 0 || subSetIndex == 3)   // CURR
                    {
                        var oCRC_CURR_CUS_List = (
                                                from t_curr_c in _context.CurrencyCustom.AsNoTracking().Include(t => t.AppGlobalOwner).Include(t => t.ChurchBody)
                                                    .Where(c => c.AppGlobalOwnerId == _oLoggedAGO.Id && c.ChurchBodyId == _oLoggedCB.Id && c.IsDisplay == true)
                                                from t_curr in _context.Country.AsNoTracking().Where(c => c.CtryAlpha3Code == t_curr_c.CtryAlpha3Code)

                                                select new CurrencyCustomModel()
                                                {
                                                    oAppGloOwnId = t_curr_c.AppGlobalOwnerId,
                                                    oAppGlobalOwn = t_curr_c.AppGlobalOwner,
                                                    oChurchBodyId = t_curr_c.ChurchBodyId,
                                                    oChurchBody = t_curr_c.ChurchBody,
                                                        ///
                                                    oCurrencyCustom = t_curr_c, // pick the currency related stuff
                                                    strAppGloOwn = t_curr_c.AppGlobalOwner != null ? t_curr_c.AppGlobalOwner.OwnerName : "",
                                                    strCurrEngName = t_curr.CurrEngName,
                                                    strCurrSymbol = t_curr.CurrSymbol,
                                                    strCurr3LISOSymbol = t_curr.Curr3LISOSymbol,
                                                    strCountry = t_curr != null ? (!string.IsNullOrEmpty(t_curr.EngName) ? t_curr.EngName : t_curr.CtryAlpha3Code) : t_curr_c.CtryAlpha3Code
                                                })
                                                .OrderBy(c => c.strCountry)
                                                .ToList();

                        oCSPModel.lsCurrencyCustomModels = oCRC_CURR_CUS_List;
                        ViewData["oSetupData_CRC_CTRY_CUS_List"] = oCRC_CURR_CUS_List;
                    }


                    /// specific parameter...
                    if (setIndex != 0) return oCSPModel;
                }


                // LSPK - customized to CB  
                if (setIndex == 0 || setIndex == 8)
                {
                    var oLSPK_CUS_List = (
                                from t_lskp_c in _context.LanguageSpokenCustom.AsNoTracking()
                                    .Where(c => c.AppGlobalOwnerId == _oLoggedAGO.Id && c.ChurchBodyId == _oLoggedCB.Id && c.IsDisplay == true)
                                from t_lskp in _context.LanguageSpoken.AsNoTracking().Include(t => t.Country)
                                    .Where(c => c.AppGlobalOwnerId == t_lskp_c.AppGlobalOwnerId && c.Id == t_lskp_c.LanguageSpokenId)

                                select new LanguageSpokenCustom()
                                {
                                    AppGlobalOwnerId = t_lskp_c.AppGlobalOwnerId,
                                    ChurchBodyId = t_lskp_c.ChurchBodyId,
                                    LanguageSpokenId = t_lskp_c.LanguageSpokenId,
                                    IsDisplay = t_lskp_c.IsDisplay,
                                    IsDefaultLanguage = t_lskp_c.IsDefaultLanguage,
                                    IsChurchLanguage = t_lskp_c.IsChurchLanguage,
                                    Created = t_lskp_c.Created,
                                    CreatedByUserId = t_lskp_c.CreatedByUserId,
                                    LastMod = t_lskp_c.LastMod,
                                    LastModByUserId = t_lskp_c.LastModByUserId,
                                    ///
                                    strLanguage = t_lskp != null ? t_lskp.Name : "",
                                    strCountry = t_lskp != null ? (t_lskp.Country != null ? (!string.IsNullOrEmpty(t_lskp.Country.EngName) ? t_lskp.Country.EngName : t_lskp.Country.CtryAlpha3Code) : t_lskp.CtryAlpha3Code) : "",
                                   // strCountry = t_lskp_c != null ? (t_lskp.Country != null ? t_lskp.Country.EngName : "") : "",
                                })
                                .OrderBy(c => c.strLanguage)
                                .ToList();


                    oCSPModel.lsLanguageSpokenCustoms = oLSPK_CUS_List;
                    ViewData["oSetupData_LSPK_CUS_List"] = oLSPK_CUS_List;

                    /// specific parameter...
                    if (setIndex != 0) return oCSPModel;
                }


                // CPR    
                if (setIndex == 0 || setIndex == 9)
                {
                    var oCPR_List = (
                                      from t_cpr in _context.ChurchPeriod.AsNoTracking().Include(t => t.OwnedByChurchBody)  //.Include(t => t.ChurchBody).ThenInclude(t => t.ParentChurchBody)
                                           .Where(c => c.AppGlobalOwnerId == _oLoggedAGO.Id && // c.ChurchBodyId == _oLoggedCB.Id &&
                                                    (c.OwnedByChurchBodyId == _oLoggedCB.Id ||
                                                    (c.OwnedByChurchBodyId != _oLoggedCB.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == _oLoggedCB.ParentChurchBodyId) //||  
                                                   // (c.OwnedByChurchBodyId != _oLoggedCB.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, _oLoggedCB))
                                                    ))

                                      select new ChurchPeriod()
                                      {
                                          Id = t_cpr.Id,
                                          AppGlobalOwnerId = t_cpr.AppGlobalOwnerId,
                                          ChurchBodyId = t_cpr.ChurchBodyId,
                                          PeriodDesc = t_cpr.PeriodDesc,
                                          OwnedByChurchBodyId = t_cpr.OwnedByChurchBodyId,
                                          From = t_cpr.From,
                                          To = t_cpr.To,
                                          LengthInDays = t_cpr.LengthInDays,
                                          PeriodType = t_cpr.PeriodType,
                                          Status = t_cpr.Status,
                                          SharingStatus = t_cpr.SharingStatus,
                                          Created = t_cpr.Created,
                                          CreatedByUserId = t_cpr.CreatedByUserId,
                                          LastMod = t_cpr.LastMod,
                                          LastModByUserId = t_cpr.LastModByUserId,
                                          ///
                                          strFrom = t_cpr.From != null ? String.Format("{0:d-MMM-yyyy}", (DateTime)t_cpr.From) : "N/A",
                                          strTo = t_cpr.To != null ? String.Format("{0:d-MMM-yyyy}", (DateTime)t_cpr.To) : "N/A",
                                          strStatus = GetStatusDesc(t_cpr.Status),
                                          strSharingStatus = GetAdhocStatusDesc(t_cpr.SharingStatus),
                                          strOwnerStatus = _oLoggedCB.Id == t_cpr.OwnedByChurchBodyId ? "O" : "I",   //  I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody
                                          strOwnerStatusDesc = GetAdhocStatusDesc(_oLoggedCB.Id == t_cpr.OwnedByChurchBodyId ? "O" : "I"),
                                          strOwnerChurchBody = t_cpr.OwnedByChurchBody != null ? t_cpr.OwnedByChurchBody.Name : ""
                                      })
                                      .OrderBy(c => c.From).ThenBy(c => c.To).ThenBy(c => c.Status).ThenBy(c => c.PeriodDesc)
                                      .ToList();

                    /// store the whole set
                    oCSPModel.lsChurchPeriods = oCPR_List;
                    ViewData["oSetupData_CPR_List"] = oCPR_List;


                    // CPR_CY
                    if (subSetIndex == 0 || subSetIndex == 1)
                    {
                        var oCPR_CY_List = oCPR_List.Where(c => c.PeriodType == "CY").ToList();

                        oCSPModel.lsChurchPeriods_CY = oCPR_CY_List;
                        ViewData["oSetupData_CPR_CY_List"] = oCPR_CY_List;
                    }

                    // CPR_AY
                    if (subSetIndex == 0 || subSetIndex == 2)
                    {
                        var oCPR_AY_List = oCPR_List.Where(c => c.PeriodType == "AY").ToList();

                        oCSPModel.lsChurchPeriods_AY = oCPR_AY_List;
                        ViewData["oSetupData_CPR_AY_List"] = oCPR_AY_List;
                    }

                    /// specific parameter...
                    if (setIndex != 0) return oCSPModel;
                }


                // NIDT   ... any CB within AGO allowed to OWN -- add/edit/delete/share (none| some | all) 
                if (setIndex == 0 || setIndex == 10)
                {
                    var oNIDT_List = (
                                from t_nidt in _context.National_IdType.AsNoTracking()  //.Include(t => t.OwnedByChurchBody)  
                                .Where(c => c.AppGlobalOwnerId == _oLoggedAGO.Id && c.CtryAlpha3Code == _oLoggedCB.CtryAlpha3Code &&
                                 (c.OwnedByChurchBodyId == _oLoggedCB.Id ||
                                 (c.OwnedByChurchBodyId != _oLoggedCB.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == _oLoggedCB.ParentChurchBodyId) ||
                                 (c.OwnedByChurchBodyId != _oLoggedCB.Id && c.SharingStatus == "A"))) //&& IsAncestor_ChurchBody(c.OwnedByChurchBody, _oLoggedCB))))

                                select t_nidt)
                                .OrderBy(c => c.IdTypeDesc)
                                .ToList();

                    oCSPModel.lsNational_IdTypes = oNIDT_List;
                    ViewData["oSetupData_NIDT_List"] = oNIDT_List;

                    /// specific parameter...
                    if (setIndex != 0) return oCSPModel;
                }

                 

                /// all parameters...
                return oCSPModel;

            }

            catch (Exception ex)
            {
                throw;
            }
        }


        // Setup...
        public ActionResult Index(int? setIndex = 0, int? subSetIndex = 0, bool loadSectionOnly = false, int filterIndex = 1, int pageIndex = 1)  // , int? subSetIndex = 0  int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0) //, int? oParentId = null, int? id = null, int pageIndex = 1)             
        {
            try
            {
                if (!InitializeUserLogging()) 
                    return RedirectToAction("LoginUserAcc", "UserLogin");

                // check permission --- checked in constructor   -- class vaiables: logger details
                // var oUserId_Logged = this._oLoggedUser.Id;
                //var lsCSPMdl = (
                //    from t_CSP in _context.MSTRAppGlobalOwner.AsNoTracking() //.Include(t => t.ChurchLevels)
                //     from t_cft in _context.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_CSP.FaithTypeCategoryId).DefaultIfEmpty()  //.Include(t => t.FaithTypeClass)
                //     from t_ctry in _context.MSTRCountry.AsNoTracking().Where(c => c.CtryAlpha3Code == t_CSP.CtryAlpha3Code).DefaultIfEmpty()

                //    select new ClientSetupParametersModel()
                //    {
                //        oAppGlobalOwn = t_CSP,
                //         // lsChurchLevels = t_CSP.ChurchLevels,
                //         //       
                //         TotalChurchLevels = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == t_CSP.Id),
                //        TotalCongregations = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == t_CSP.Id && c.Status == "A"),
                //         // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 
                //         strAppGloOwn = t_CSP.OwnerName,
                //        strFaithCategory = t_cft != null ? t_cft.FaithDescription : "",
                //        strCountry = t_ctry != null ? t_ctry.Name : "",
                //        strSlogan = t_CSP.Slogan.Contains("|") ? (t_CSP.Slogan.Substring(0, t_CSP.Slogan.IndexOf("|"))).Replace("|", "") : t_CSP.Slogan,
                //        strSloganResponse = t_CSP.Slogan.Contains("|") ? (t_CSP.Slogan.Substring(t_CSP.Slogan.IndexOf("|"))).Replace("|", "") : "",
                //         //strChurchStream = t_cft.FaithTypeClass != null ? t_cft.FaithTypeClass.FaithDescription : "",
                //         //   
                //         blStatusActivated = t_CSP.Status == "A",
                //        strStatus = GetStatusDesc(t_CSP.Status)
                //    })
                //    .OrderBy(c => c.strCountry).OrderBy(c => c.strAppGloOwn)
                //    .ToList();

                //oCSPModel.lsAppGlobalOwnModels = lsCSPMdl;


                var oCSPModel = LoadClientSetupList((int)setIndex, (int)subSetIndex, filterIndex);
                oCSPModel.oUserId_Logged = _oLoggedUser.Id;
                oCSPModel.oChurchBodyId_Logged = _oLoggedCB.Id;
                oCSPModel.oAppGloOwnId_Logged = _oLoggedAGO.Id;
                oCSPModel.pageIndex = pageIndex;
                oCSPModel.setIndex = (int)setIndex;
                oCSPModel.subSetIndex = (int)subSetIndex; 

                if (loadSectionOnly) 
                { 
                    switch (setIndex)
                    {
                        case 6: return PartialView("_vwParamNVP", oCSPModel.lsAppUtilityNVPModels); 
                        case 7: 
                            if (subSetIndex == 1) { if (oCSPModel.oCountryModel != null) oCSPModel.oCountryModel.pageIndex = pageIndex; return PartialView("_vwParamCTRY", oCSPModel.oCountryModel); }
                            else if (subSetIndex == 2) { if (oCSPModel.oCountryRegionModel != null) oCSPModel.oCountryRegionModel.pageIndex = pageIndex; return PartialView("_vwParamCTRY_RGN", oCSPModel.oCountryRegionModel); }                            
                            else if (subSetIndex == 3) { if (oCSPModel.oCurrencyCustomModel != null) oCSPModel.oCurrencyCustomModel.pageIndex = pageIndex; return PartialView("_vwParamCURR", oCSPModel.oCurrencyCustomModel); }                            
                            else return View();

                        default: return View();
                    }
                }
                  
                 
                var strDesc = "Client Setup Parameters";
                var _userTask = "Viewed " + strDesc.ToLower() + " list";
                oCSPModel.strCurrTask = strDesc;
                   
                //oCSPModel.oAppGloOwnId = oAppGloOwnId;
                //oCSPModel.oChurchBodyId = oCurrChuBodyId;
                 
                var tm = DateTime.Now;
                _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, _oLoggedUser.Id, tm, tm, _oLoggedUser.Id, _oLoggedUser.Id));
                 

                ///
                var _oCSPModel = Newtonsoft.Json.JsonConvert.SerializeObject(oCSPModel);
                TempData["oVmCSPModel"] = _oCSPModel; TempData.Keep();


                return View("Index", oCSPModel);
            }

            catch (Exception ex)
            {
                throw;
                ////page not found error
                //Response.StatusCode = 500;
                //return View("ErrorPage");
            }
        }


        //[HttpGet]
        public AppGlobalOwnerModel GetAddOrEdit_AGO(int id = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) // (int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            try
            {
                //var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                //var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                //// int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                ////int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                //// int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                //oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                //oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                //oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                var strDesc = "Denomination (Church)";
                var _userTask = "Attempted accessing/modifying " + strDesc.ToLower();  // _userTask = "Attempted creating new denomination (church)"; // _userTask = "Opened denomination (church)-" + oCFT_MDL.oChurchFaithType.FaithDescription;

                // var oAGO_MDL = new AppGlobalOwnerModel();
                //if (id == 0)
                //{
                //    //create user and init... 
                //    oAGO_MDL.oAppGlobalOwn = new AppGlobalOwner();
                //    oAGO_MDL.oAppGlobalOwn.TotalLevels = 1;
                //    //oAGO_MDL.oAppGlobalOwn.Status = "A";
                //    oAGO_MDL.blStatusActivated = true;

                //    _userTask = "Attempted creating new " + strDesc.ToLower();
                //}

                //else
                //{ 
                //var oAGO_MDL_List = (
                //        from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(x => x.Id == id)
                //        from t_ctry in _context.Country.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()

                //             // from t_cft in _context.ChurchFaithType.Include(t => t.FaithTypeClass).AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()

                //         select new AppGlobalOwnerModel()
                //        {
                //            oAppGlobalOwn = t_ago,
                //            lsChurchLevels = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == t_ago.Id).ToList(),
                //             //       
                //             TotalChurchLevels = _context.ChurchLevel.Count(c => c.AppGlobalOwnerId == t_ago.Id),
                //            TotalCongregations = _context.ChurchBody.Count(c => c.AppGlobalOwnerId == t_ago.Id && c.Status == "A"),
                //             // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 
                //             strAppGloOwn = t_ago.OwnerName,
                //            //strFaithCategory = t_cftc != null ? t_cftc.FaithDescription : "",
                //            //strChurchStream = t_cfts.FaithTypeClass != null ? t_cfts.FaithTypeClass.FaithDescription : "",
                //            strCountry = t_ctry != null ? t_ctry.Name : "",
                //            strSlogan = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(0, t_ago.Slogan.IndexOf("|"))).Replace("|", "") : t_ago.Slogan,
                //            strSloganResponse = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(t_ago.Slogan.IndexOf("|"))).Replace("|", "") : "",
                //             //   
                //             blStatusActivated = t_ago.Status == "A",
                //            strStatus = GetStatusDesc(t_ago.Status)
                //        })
                //    .ToList();



                //var oAGO_List = _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == _oLoggedAGO.Id).ToList();
                //if (oAGO_List.Count() == 0) return null;
                /////
                //var oCFTC_MSTRList = _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FC").ToList();
                //var oCFTS_MSTRList = _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FS").ToList();
                ///
                var oAGO_MDL = (
                              from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == _oLoggedAGO.Id) 
                              from t_ctry in _context.Country.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()

                                  // from t_cft in _context.ChurchFaithType.Include(t => t.FaithTypeClass).AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()

                              select new AppGlobalOwnerModel()
                              {
                                  oAppGlobalOwn = t_ago,
                                  // lsChurchLevels = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == t_ago.Id).ToList(),
                                  //       
                                  numTotalChurchLevelsConfig = _context.ChurchLevel.Count(c => c.AppGlobalOwnerId == t_ago.Id),
                                  //TotalCongregations = _context.ChurchBody.Count(c => c.AppGlobalOwnerId == t_ago.Id && c.Status == "A"),
                                  // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 

                                  strAppGloOwn = t_ago.OwnerName,
                                  //strFaithTypeCategory = t_cftc != null ? t_ago.strFaithTypeCategory : "",
                                  //strChurchStream = t_ago.strFaithTypeStream,
                                  strCountry = t_ctry != null ? (!string.IsNullOrEmpty(t_ctry.EngName) ? t_ctry.EngName : t_ctry.CtryAlpha3Code) : t_ago.CtryAlpha3Code,
                                  strSlogan = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(0, t_ago.Slogan.IndexOf("|"))).Replace("|", "") : t_ago.Slogan,
                                  strSloganResponse = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(t_ago.Slogan.IndexOf("|"))).Replace("|", "") : "",
                                  // strFaithTypeCategory = t_ago != null ? ((!string.IsNullOrEmpty(t_ago.strFaithTypeCategory) && !string.IsNullOrEmpty(t_ago.strFaithTypeStream) ? t_ago.strFaithTypeCategory + ", " + t_ago.strFaithTypeStream : t_ago.strFaithTypeCategory + t_ago.strFaithTypeStream).Trim()) : "",
                                  //   
                                  blStatusActivated = t_ago.Status == "A",
                                  strStatus = GetStatusDesc(t_ago.Status)
                              })
                          .FirstOrDefault();



                /////
                //oAGO_MDL = (
                //         from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(x => x.Id == id)                             
                //         from t_ctry in _context.Country.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()
                //         from t_cftc in oCFTC_MSTRList.Where(c => c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()
                //         from t_cfts in oCFTS_MSTRList.Where(c => c.Id == t_cftc.FaithTypeClassId).DefaultIfEmpty()

                //         // from t_cft in _context.ChurchFaithType.Include(t => t.FaithTypeClass).AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()

                //     select new AppGlobalOwnerModel()
                //     {
                //         oAppGlobalOwn = t_ago,
                //         lsChurchLevels = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == t_ago.Id).ToList(),
                //             //       
                //         TotalChurchLevels = _context.ChurchLevel.Count(c => c.AppGlobalOwnerId == t_ago.Id),
                //         TotalCongregations = _context.ChurchBody.Count(c => c.AppGlobalOwnerId == t_ago.Id && c.Status == "A"),
                //             // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 
                //         strAppGloOwn = t_ago.OwnerName,
                //         strFaithCategory = t_cftc != null ? t_cftc.FaithDescription : "",
                //         strChurchStream = t_cfts.FaithTypeClass != null ? t_cfts.FaithTypeClass.FaithDescription : "",
                //         strCountry = t_ctry != null ? t_ctry.Name : "",
                //         strSlogan = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(0, t_ago.Slogan.IndexOf("|"))).Replace("|", "") : t_ago.Slogan,
                //         strSloganResponse = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(t_ago.Slogan.IndexOf("|"))).Replace("|", "") : "", 
                //             //   
                //         blStatusActivated = t_ago.Status == "A",
                //         strStatus = GetStatusDesc(t_ago.Status)  
                //     })
                //     .FirstOrDefault();



                if (oAGO_MDL.oAppGlobalOwn == null) return null;

                    //if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.PrefixKey))
                    //{
                    //    var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                    //    var jsCode = GetNextCodePrefixByAcronym_jsonString(oAGO_MDL.oAppGlobalOwn.Acronym);  // string json1 = @"{'Name':'James'}";
                    //    var jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                    //    if (jsOut != null)
                    //        if (bool.Parse(jsOut.taskSuccess) == true)
                    //            oAGO_MDL.oAppGlobalOwn.PrefixKey = jsOut.strRes;
                    //}


                    //if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.PrefixKey))
                    //    oAGO_MDL.oAppGlobalOwn.PrefixKey = GetNextCodePrefixByAcronym_jsonString(oAGO_MDL.oAppGlobalOwn.Acronym);

                    ////church code  
                    //if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.GlobalChurchCode) && !string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.PrefixKey))
                    //{
                    //    oAGO_MDL.oAppGlobalOwn.GlobalChurchCode = oAGO_MDL.oAppGlobalOwn.PrefixKey + string.Format("{0:D3}", 0);
                    //    oAGO_MDL.oAppGlobalOwn.RootChurchCode = oAGO_MDL.oAppGlobalOwn.GlobalChurchCode;
                    //}

                    ////root church code  
                    //if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.RootChurchCode) && !string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.GlobalChurchCode))
                    //    oAGO_MDL.oAppGlobalOwn.RootChurchCode = oAGO_MDL.oAppGlobalOwn.GlobalChurchCode;


                    _userTask = "Opened " + strDesc.ToLower() + ", " + oAGO_MDL.oAppGlobalOwn.OwnerName;
                //}


                // oAGO_MDL.setIndex = setIndex;
                // oAGO_MDL.subSetIndex = subSetIndex;

                oAGO_MDL.oUserId_Logged = oUserId_Logged;
                oAGO_MDL.oAppGloOwnId_Logged = oAGOId_Logged;
                oAGO_MDL.oChurchBodyId_Logged = oCBId_Logged;
                //
                // oAGO_MDL.oAppGloOwnId = oAppGloOwnId;
                // oAGO_MDL.oChurchBodyId = oCurrChuBodyId;
                //  var oCurrChuBody = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
                // oAGO_MDL.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null;

                //   if (oAGO_MDL.subSetIndex == 2) // Denomination classes av church sects

                oAGO_MDL = this.popLookups_AGO(oAGO_MDL, oAGO_MDL.oAppGlobalOwn);

                var tm = DateTime.Now;
                // record ... @client
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, _oLoggedAGO.Id, _oLoggedCB.Id, "N",
                                 "RCMS-Client: Denomination", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, _oLoggedUser.Id, tm, tm, _oLoggedUser.Id, _oLoggedUser.Id)
                                , this._strClientConn);

                //var _oAGO_MDL = Newtonsoft.Json.JsonConvert.SerializeObject(oAGO_MDL);
                //TempData["oVmCurrMod"] = _oAGO_MDL; TempData.Keep();

                return oAGO_MDL;
                // return View("_AddOrEdit_AGO", oAGO_MDL);
            }

            catch (Exception ex)
            {
                return null;

                ////page not found error
                //Response.StatusCode = 500;
                //return View("ErrorPage");
            }
        }

        [HttpGet]
        public IActionResult AddOrEdit_AGO(int id = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) // (int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            try
            {
                if (id > 0)
                {
                    if (!InitializeUserLogging())
                        return RedirectToAction("LoginUserAcc", "UserLogin");

                    //var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                    //var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                    //// int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                    ////int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                    //// int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                    //oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                    //oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                    //oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                    var strDesc = "Denomination (Church)";
                    var _userTask = "Attempted accessing/modifying " + strDesc.ToLower();  // _userTask = "Attempted creating new denomination (church)"; // _userTask = "Opened denomination (church)-" + oCFT_MDL.oChurchFaithType.FaithDescription;

               // var oAGO_MDL = new AppGlobalOwnerModel();
                    //if (id == 0)
                    //{
                    //    //create user and init... 
                    //    oAGO_MDL.oAppGlobalOwn = new AppGlobalOwner();
                    //    oAGO_MDL.oAppGlobalOwn.TotalLevels = 1;
                    //    //oAGO_MDL.oAppGlobalOwn.Status = "A";
                    //    oAGO_MDL.blStatusActivated = true;

                    //    _userTask = "Attempted creating new " + strDesc.ToLower();
                    //}


                    //var oAGO_List = _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == _oLoggedAGO.Id).ToList();
                    //if (oAGO_List.Count() == 0) return null;
                    /////
                    //var oCFTC_MSTRList = _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FC").ToList();
                    //var oCFTS_MSTRList = _masterContext.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FS").ToList();
                    ///

                    var oAGO_MDL = (
                              from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == _oLoggedAGO.Id)
                              from t_ctry in _context.Country.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()

                                  // from t_cft in _context.ChurchFaithType.Include(t => t.FaithTypeClass).AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()

                              select new AppGlobalOwnerModel()
                              {
                                  oAppGlobalOwn = t_ago,
                                  // lsChurchLevels = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == t_ago.Id).ToList(),
                                  //       
                                  numTotalChurchLevelsConfig = _context.ChurchLevel.Count(c => c.AppGlobalOwnerId == t_ago.Id),
                                  //TotalCongregations = _context.ChurchBody.Count(c => c.AppGlobalOwnerId == t_ago.Id && c.Status == "A"),
                                  // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 

                                  strAppGloOwn = t_ago.OwnerName,
                                  //strFaithTypeCategory = t_cftc != null ? t_ago.strFaithTypeCategory : "",
                                  //strChurchStream = t_ago.strFaithTypeStream,
                                  strCountry = t_ctry != null ? (!string.IsNullOrEmpty(t_ctry.EngName) ? t_ctry.EngName : t_ctry.CtryAlpha3Code) : t_ago.CtryAlpha3Code,
                                  strSlogan = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(0, t_ago.Slogan.IndexOf("|"))).Replace("|", "") : t_ago.Slogan,
                                  strSloganResponse = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(t_ago.Slogan.IndexOf("|"))).Replace("|", "") : "",
                                  // strFaithTypeCategory = t_ago != null ? ((!string.IsNullOrEmpty(t_ago.strFaithTypeCategory) && !string.IsNullOrEmpty(t_ago.strFaithTypeStream) ? t_ago.strFaithTypeCategory + ", " + t_ago.strFaithTypeStream : t_ago.strFaithTypeCategory + t_ago.strFaithTypeStream).Trim()) : "",
                                  //   
                                  blStatusActivated = t_ago.Status == "A",
                                  strStatus = GetStatusDesc(t_ago.Status)
                              })
                          .FirstOrDefault();


                    /////
                    //oAGO_MDL = (
                    //         from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(x => x.Id == id)                             
                    //         from t_ctry in _context.Country.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()
                    //         from t_cftc in oCFTC_MSTRList.Where(c => c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()
                    //         from t_cfts in oCFTS_MSTRList.Where(c => c.Id == t_cftc.FaithTypeClassId).DefaultIfEmpty()

                    //         // from t_cft in _context.ChurchFaithType.Include(t => t.FaithTypeClass).AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()

                    //     select new AppGlobalOwnerModel()
                    //     {
                    //         oAppGlobalOwn = t_ago,
                    //         lsChurchLevels = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == t_ago.Id).ToList(),
                    //             //       
                    //         TotalChurchLevels = _context.ChurchLevel.Count(c => c.AppGlobalOwnerId == t_ago.Id),
                    //         TotalCongregations = _context.ChurchBody.Count(c => c.AppGlobalOwnerId == t_ago.Id && c.Status == "A"),
                    //             // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 
                    //         strAppGloOwn = t_ago.OwnerName,
                    //         strFaithCategory = t_cftc != null ? t_cftc.FaithDescription : "",
                    //         strChurchStream = t_cfts.FaithTypeClass != null ? t_cfts.FaithTypeClass.FaithDescription : "",
                    //         strCountry = t_ctry != null ? t_ctry.Name : "",
                    //         strSlogan = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(0, t_ago.Slogan.IndexOf("|"))).Replace("|", "") : t_ago.Slogan,
                    //         strSloganResponse = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(t_ago.Slogan.IndexOf("|"))).Replace("|", "") : "", 
                    //             //   
                    //         blStatusActivated = t_ago.Status == "A",
                    //         strStatus = GetStatusDesc(t_ago.Status)  
                    //     })
                    //     .FirstOrDefault();



                    if (oAGO_MDL.oAppGlobalOwn == null)
                    {
                        //page not found error
                        Response.StatusCode = 500;
                        return View("ErrorPage");
                    }

                    //if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.PrefixKey))
                    //{
                    //    var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                    //    var jsCode = GetNextCodePrefixByAcronym_jsonString(oAGO_MDL.oAppGlobalOwn.Acronym);  // string json1 = @"{'Name':'James'}";
                    //    var jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                    //    if (jsOut != null)
                    //        if (bool.Parse(jsOut.taskSuccess) == true)
                    //            oAGO_MDL.oAppGlobalOwn.PrefixKey = jsOut.strRes;
                    //}


                    //if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.PrefixKey))
                    //    oAGO_MDL.oAppGlobalOwn.PrefixKey = GetNextCodePrefixByAcronym_jsonString(oAGO_MDL.oAppGlobalOwn.Acronym);

                    ////church code  
                    //if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.GlobalChurchCode) && !string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.PrefixKey))
                    //{
                    //    oAGO_MDL.oAppGlobalOwn.GlobalChurchCode = oAGO_MDL.oAppGlobalOwn.PrefixKey + string.Format("{0:D3}", 0);
                    //    oAGO_MDL.oAppGlobalOwn.RootChurchCode = oAGO_MDL.oAppGlobalOwn.GlobalChurchCode;
                    //}

                    ////root church code  
                    //if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.RootChurchCode) && !string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.GlobalChurchCode))
                    //    oAGO_MDL.oAppGlobalOwn.RootChurchCode = oAGO_MDL.oAppGlobalOwn.GlobalChurchCode;


                    _userTask = "Opened " + strDesc.ToLower() + ", " + oAGO_MDL.oAppGlobalOwn.OwnerName;
                

                    oAGO_MDL.setIndex = 1;
                    // oAGO_MDL.subSetIndex = subSetIndex;

                    oAGO_MDL.oUserId_Logged = oUserId_Logged;
                    oAGO_MDL.oAppGloOwnId_Logged = oAGOId_Logged;
                    oAGO_MDL.oChurchBodyId_Logged = oCBId_Logged;
                    //
                    // oAGO_MDL.oAppGloOwnId = oAppGloOwnId;
                    // oAGO_MDL.oChurchBodyId = oCurrChuBodyId;
                    //  var oCurrChuBody = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
                    // oAGO_MDL.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null;

                    //   if (oAGO_MDL.subSetIndex == 2) // Denomination classes av church sects

                    oAGO_MDL = this.popLookups_AGO(oAGO_MDL, oAGO_MDL.oAppGlobalOwn);

                    var tm = DateTime.Now;
                    // record ... @client
                    _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, _oLoggedAGO.Id, _oLoggedCB.Id, "N",
                                     "RCMS-Client: Denomination", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, _oLoggedUser.Id, tm, tm, _oLoggedUser.Id, _oLoggedUser.Id)
                                    , this._strClientConn);

                    //var _oAGO_MDL = Newtonsoft.Json.JsonConvert.SerializeObject(oAGO_MDL);
                    //TempData["oVmCurrMod"] = _oAGO_MDL; TempData.Keep();

                  //  return oAGO_MDL;.


                    return PartialView("_AddOrEdit_AGO", oAGO_MDL);

                }


                // shouldn't get this far
                //page not found error
                Response.StatusCode = 500;
                return View("ErrorPage");
            }

            catch (Exception ex)
            {
                //page not found error
                Response.StatusCode = 500;
                return View("ErrorPage");
            }
        }
        public AppGlobalOwnerModel popLookups_AGO(AppGlobalOwnerModel vm, AppGlobalOwner oCurrAGO)
        {
            if (vm == null || oCurrAGO == null) return vm;
            //
            vm.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            //vm.lkpFaithCategories = _context.ChurchFaithType.Where(c => c.Category == "FC" && !string.IsNullOrEmpty(c.FaithDescription))
            //                              .OrderBy(c => c.FaithDescription).ToList()
            //                              .Select(c => new SelectListItem()
            //                              {
            //                                  Value = c.Id.ToString(),
            //                                  Text = c.FaithDescription
            //                              })
            //                              .ToList();
            //  vm.lkpFaithCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            vm.lkpCountries = _context.Country.ToList()  //.Where(c => c.Display == true)
                                           .Select(c => new SelectListItem()
                                           {
                                               Value = c.CtryAlpha3Code, //.ToString(),
                                               Text = c.EngName
                                           })
                                           .OrderBy(c => c.Text)
                                           .ToList();
            // vm.lkpCountries.Insert(0, new SelectListItem { Value = "", Text = "Select" });


            return vm;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit_AGO(AppGlobalOwnerModel vm)
        {
            if (!InitializeUserLogging())
                return RedirectToAction("LoginUserAcc", "UserLogin");

            var strDesc = "Denomination (Church)";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });
            if (vm.oAppGlobalOwn == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });

            AppGlobalOwner _oChanges = vm.oAppGlobalOwn;  // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();
             
            //check if the configured levels <= total levels under AppGloOwn
            var lsCL = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == _oChanges.Id).ToList();
            var countCL = lsCL.Count();
            if (countCL > _oChanges.TotalLevels)
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Total church levels allowed for denomination, " + _oChanges.OwnerName + " [" + _oChanges.TotalLevels + "] exceeded. Hint: You may adjust either way [Denomination or Church level details]" });

            foreach (var oCL in lsCL)
            {
                if (oCL.LevelIndex <= 0 || oCL.LevelIndex > _oChanges.TotalLevels)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please indicate correct level index. Hint: Must be within total church levels [" + _oChanges.TotalLevels + "]. Hint: You may adjust either way [Denomination or Church level details]" });
            }

            if (string.IsNullOrEmpty(_oChanges.OwnerName)) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide name for " + strDesc.ToLower() });
            }

            //check if the configured levels <= total levels under AppGloOwn
            var lsCBs = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == _oChanges.Id).ToList();

            if ((_oChanges.Id == 0 || (_oChanges.Id > 0 && lsCBs.Count() == 0)) && string.IsNullOrEmpty(_oChanges.PrefixKey) && string.IsNullOrEmpty(_oChanges.Acronym)) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide acronym or church prefix for " + strDesc.ToLower() });
            }
            //if (_oChanges.FaithTypeCategoryId == null)  // you can create 'Others' to cater for non-category
            //{
            //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the church faith stream or category." });
            //}

            if (_oChanges.CtryAlpha3Code == null)  // you can create 'Others' to cater for non-category
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the base country." });
            }


            //validations done!
            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<AppGlobalOwnerModel>(arrData) : vm;

            var oAGO = vmMod.oAppGlobalOwn;
            // oAGO.ChurchBody = vmMod.oChurchBody;

            try
            {
                ModelState.Remove("oAppGlobalOwn.CtryAlpha3Code");
                ModelState.Remove("oAppGlobalOwn.FaithTypeCategoryId");
                ModelState.Remove("oAppGlobalOwn.CreatedByUserId");
                ModelState.Remove("oAppGlobalOwn.LastModByUserId");

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

                // church logo
                if (vm.ChurchLogoFile != null) //&& _oChanges.ChurchLogo != null
                {
                    if (_oChanges.ChurchLogo != vm.ChurchLogoFile.FileName)
                    {
                        string strFilename = null;
                        if (vm.ChurchLogoFile != null && vm.ChurchLogoFile.Length > 0)
                        {
                            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img_db");
                            strFilename = Guid.NewGuid().ToString() + "_" + vm.ChurchLogoFile.FileName;
                            string filePath = Path.Combine(uploadFolder, strFilename);
                            vm.ChurchLogoFile.CopyTo(new FileStream(filePath, FileMode.Create));
                        }
                        else
                        {
                            if (vm.oAppGlobalOwn.Id != 0) strFilename = vm.strChurchLogo;
                        }

                        _oChanges.ChurchLogo = strFilename;
                    }
                }

                //
                var tm = DateTime.Now;
                _oChanges.LastMod = tm;
                _oChanges.LastModByUserId = vm.oUserId_Logged;
                //_oChanges.Status = vm.blStatusActivated ? "A" : "D";

                //
                _oChanges.Slogan = (!string.IsNullOrEmpty(vm.strSlogan) ? vm.strSlogan : "") +
                                                    (!string.IsNullOrEmpty(vm.strSlogan) && !string.IsNullOrEmpty(vm.strSloganResponse) ? "|" : "") +
                                                                                (!string.IsNullOrEmpty(vm.strSloganResponse) ? vm.strSloganResponse : "");
                //
                ////get the prefix, church code, root code from acronym
                ////get the prefix code  
                //if (string.IsNullOrEmpty(_oChanges.PrefixKey))
                //{
                //    //var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                //    //var jsCode = GetNextCodePrefixByAcronym_jsonString(_oChanges.Acronym);  // string json1 = @"{'Name':'James'}";
                //    //var jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                //    //if (jsOut != null)
                //    //    if (bool.Parse(jsOut.taskSuccess) == true)
                //    //        _oChanges.PrefixKey = jsOut.strRes;

                //    _oChanges.PrefixKey = GetNextCodePrefixByAcronym_jsonString(_oChanges.Acronym);
                //}

                ////church code  
                //if (string.IsNullOrEmpty(_oChanges.GlobalChurchCode) && !string.IsNullOrEmpty(_oChanges.PrefixKey))
                //{
                //    _oChanges.GlobalChurchCode = _oChanges.PrefixKey + string.Format("{0:D3}", 0);
                //    _oChanges.RootChurchCode = _oChanges.GlobalChurchCode;
                //}

                //var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                //jsCode = GetNextGlobalChurchCodeByAcronym_jsonString(_oChanges.PrefixKey, _oChanges.Id);  // string json1 = @"{'Name':'James'}";
                //jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                //if (jsOut != null)
                //    if (bool.Parse(jsOut.taskSuccess) == true)
                //        _oChanges.GlobalChurchCode = jsOut.strRes;



                ////root church code  
                //if (string.IsNullOrEmpty(_oChanges.RootChurchCode) && !string.IsNullOrEmpty(_oChanges.GlobalChurchCode))
                //{
                //    _oChanges.RootChurchCode = _oChanges.GlobalChurchCode;
                //}

                //jsCode = GetNextRootChurchCodeByParentCB_jsonString(_oChanges.PrefixKey, _oChanges.Id, null);  // string json1 = @"{'Name':'James'}";
                //jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                //if (jsOut != null)
                //    if (bool.Parse(jsOut.taskSuccess) == true)
                //        _oChanges.RootChurchCode = jsOut.strRes;

                if (string.IsNullOrEmpty(_oChanges.PrefixKey) || string.IsNullOrEmpty(_oChanges.GlobalChurchCode) || string.IsNullOrEmpty(_oChanges.RootChurchCode)) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Prefix code, Church code and Root church code for " + strDesc.ToLower() + " must be specified" });
                }


                //validate...
                var _userTask = "Attempted saving " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper();  // _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                var _reset = _oChanges.Id == 0;


                //using (var _agoCtx = new ChurchModelContext(_context.Database.GetDbConnection().ConnectionString))
                //{
                    if (_oChanges.Id > 0)
                    //{
                    //    var existAGO = _context.AppGlobalOwner.Where(c => c.OwnerName.ToLower() == _oChanges.OwnerName.ToLower()).ToList();
                    //    if (existAGO.Count() > 0)
                    //    { return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " -- " + _oChanges.OwnerName + " already exist." }); }

                    //    _oChanges.Created = tm;
                    //    _oChanges.CreatedByUserId = vm.oUserId_Logged;

                    //_context.Add(_oChanges);

                    //    ViewBag.UserMsg = "Saved " + strDesc.ToLower() + " " + (!string.IsNullOrEmpty(_oChanges.OwnerName) ? " -- " + _oChanges.OwnerName : "") + " successfully.";
                    //    _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                    //}

                    //else

                    {
                        var existAGO = _context.AppGlobalOwner.Where(c => c.Id != _oChanges.Id && c.OwnerName.ToLower() == _oChanges.OwnerName.ToLower()).ToList();
                        if (existAGO.Count() > 0) 
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " -- " + _oChanges.OwnerName + " already exist." });

                        if (_oChanges.MSTR_AppGlobalOwnerId == null)
                            _oChanges.MSTR_AppGlobalOwnerId = _oLoggedAGO.Id;

                        if (string.IsNullOrEmpty(_oChanges.strFaithTypeCategory) || string.IsNullOrEmpty(_oChanges.strFaithStream))
                        {
                            var oAGO_MSTR = _masterContext.MSTRAppGlobalOwner.AsNoTracking().Include(t => t.FaithTypeCategory).ThenInclude(t => t.FaithTypeClass)
                                                        .Where(c => c.Id == _oChanges.MSTR_AppGlobalOwnerId).FirstOrDefault();
                        // if (_oChanges.strFaithTypeCategory.ToLower() != oAGO_MSTR.strFaithTypeCategory.ToLower())
                            _oChanges.strFaithTypeCategory = oAGO_MSTR.FaithTypeCategory != null ? oAGO_MSTR.FaithTypeCategory.FaithDescription : "";
                         
                       // if (_oChanges.strFaithStream.ToLower() != oAGO_MSTR.strFaithStream.ToLower())
                            _oChanges.strFaithStream = oAGO_MSTR.FaithTypeCategory != null ? (oAGO_MSTR.FaithTypeCategory.FaithTypeClass != null ? oAGO_MSTR.FaithTypeCategory.FaithTypeClass.FaithDescription : "") : "";
                        }

                        //retain the pwd details... hidden fields
                        _context.Update(_oChanges);
                        //var _strDesc = strDesc.Length > 0 ? strDesc.Substring(0, 1).ToUpper() + strDesc.Substring(1) : "Denomination ";

                        ViewBag.UserMsg = strDesc + " updated successfully.";
                        _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                    }

                //save denomination first... 
                _context.SaveChanges(); // await _agoCtx.SaveChangesAsync();


                 //   DetachAllEntities(_agoCtx);

                //}


                var _tm = DateTime.Now;
                _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));


                //////auto-update the church levels
                ////using (var _clCtx = new ChurchModelContext(_context.Database.GetDbConnection().ConnectionString))
                ////{
                //    var oChLevelCntAdd = 0; var oChLevelCntUpd = 0;
                //    //  _userTask = "Attempted saving church level, " + _oChanges.ToUpper();  // _userTask = "Added new church level, " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated church level, " + _oChanges.OwnerName.ToUpper() + " successfully";
                //    if (vmMod.oAppGlobalOwn.Id == 0)
                //    {
                //        for (int i = 1; i <= _oChanges.TotalLevels; i++) // oAGO.TotalLevels; i++)
                //        {
                //            ChurchLevel oCL = new ChurchLevel()
                //            {
                //                Name = "Level_" + i,
                //                CustomName = "Level " + i,
                //                LevelIndex = i,
                //                AppGlobalOwnerId = _oChanges.Id,
                //                SharingStatus = "N",
                //                Created = DateTime.Now,
                //                LastMod = DateTime.Now,
                //            };
                //            //
                //            oChLevelCntAdd++;
                //            _context.Add(oCL);
                //        }

                //        if (oChLevelCntAdd > 0)
                //        {
                //            _userTask = "Added new " + oChLevelCntAdd + " church levels for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                //            ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oChLevelCntAdd + " church levels. Customization may be necessary";
                //        }
                //    }
                //    else
                //    {
                //        for (int i = 1; i <= _oChanges.TotalLevels; i++)
                //        {
                //            var oExistCL = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == _oChanges.Id && c.Name == "Level_" + i).FirstOrDefault();
                //            if (oExistCL == null && (countCL + oChLevelCntAdd) < _oChanges.TotalLevels)  //add new ... the missing levels
                //            {
                //                ChurchLevel oCL = new ChurchLevel()
                //                {
                //                    Name = "Level_" + i,
                //                    CustomName = "Level " + i,
                //                    LevelIndex = i,
                //                    AppGlobalOwnerId = _oChanges.Id,
                //                    SharingStatus = "N",
                //                    Created = DateTime.Now,
                //                    LastMod = DateTime.Now,
                //                };

                //                //
                //                oChLevelCntAdd++;
                //                _context.Add(oCL);  //_clCtx.Add(oCL);
                //            }

                //            // UPDATE unecessary!
                //            //else if (oExistCL != null && (countCL + oChLevelCntAdd ) <= _oChanges.TotalLevels)  // upd
                //            //{
                //            //    oExistCL.Name = "Level_" + i;
                //            //    oExistCL.CustomName = "Level " + i;
                //            //    oExistCL.LevelIndex = i;
                //            //    oExistCL.AppGlobalOwnerId = _oChanges.Id;
                //            //    oExistCL.SharingStatus = "N";
                //            //    oExistCL.LastMod = DateTime.Now;
                //            //    //
                //            //    oChLevelCntUpd++;
                //            //    ctx.Update(oExistCL);
                //            //}
                //        }

                //        if ((oChLevelCntAdd + oChLevelCntUpd) > 0)
                //        {
                //            if (oChLevelCntAdd > 0)
                //            {
                //                _userTask = "Added new " + oChLevelCntAdd + " church levels for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                //                ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oChLevelCntAdd + " church levels. Customization may be necessary";
                //            }

                //            if (oChLevelCntUpd > 0)
                //            {
                //                _userTask = !string.IsNullOrEmpty(_userTask) ? _userTask + ". " : "" + "Updated " + oChLevelCntUpd + " church levels for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                //                ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Denomination's " + oChLevelCntUpd + " church levels updated. Customization may be necessary.";
                //            }
                //        }
                //    }

                //    if ((oChLevelCntAdd + oChLevelCntUpd) > 0)
                //    {
                //        _context.SaveChanges(); // await _clCtx.SaveChangesAsync();

                //        // DetachAllEntities(_context);

                //        _tm = DateTime.Now;
                //        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                //                         "RCMS-Admin: Church Level", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));
                //    }

                ////}


                ////auto-update the church root - church body : RCM000
                //using (var _cbCtx = new ChurchModelContext(_context.Database.GetDbConnection().ConnectionString))
                //{
                //    var oCBCntAdd = 0; var oCBCntUpd = 0;
                //    //  _userTask = "Attempted saving church level, " + _oChanges.ToUpper();  // _userTask = "Added new church level, " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated church level, " + _oChanges.OwnerName.ToUpper() + " successfully";

                //    var oCL_1 = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == _oChanges.Id && c.LevelIndex == 1).FirstOrDefault();
                //    if (vmMod.oAppGlobalOwn.Id == 0)
                //    {
                //        ChurchBody oCB = new ChurchBody()
                //        {
                //            Name = _oChanges.OwnerName,
                //            OrganisationType = "CR",
                //            AppGlobalOwnerId = _oChanges.Id,
                //            ChurchLevelId = oCL_1 != null ? oCL_1.Id : (int?)null,
                //            // CountryId = _oChanges.CountryId,
                //            CtryAlpha3Code = _oChanges.CtryAlpha3Code,
                //            CountryRegionId = null,
                //            GlobalChurchCode = _oChanges.GlobalChurchCode,
                //            RootChurchCode = _oChanges.RootChurchCode,
                //            //ChurchUnitLogo = _oChanges.ChurchLogo,
                //            ParentChurchBodyId = null,
                //            Status = "A",
                //            Created = DateTime.Now,
                //            LastMod = DateTime.Now,
                //            CreatedByUserId = _oChanges.CreatedByUserId,
                //            LastModByUserId = _oChanges.LastModByUserId
                //        };

                //        oCBCntAdd++;
                //        _cbCtx.Add(oCB);

                //        if (oCBCntAdd > 0)
                //        {
                //            _userTask = "Added Church Root unit for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                //            ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oCBCntAdd + " church root unit";
                //        }
                //    }
                //    else
                //    {
                //        var oCBList = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == _oChanges.Id && c.OrganisationType == "CR" && c.Status == "A").ToList();
                //        if (oCBList.Count() == 0)
                //        {
                //            ChurchBody oCB = new ChurchBody()
                //            {
                //                Name = _oChanges.OwnerName,
                //                OrganisationType = "CR",
                //                AppGlobalOwnerId = _oChanges.Id,
                //                ChurchLevelId = oCL_1 != null ? oCL_1.Id : (int?)null,
                //                CtryAlpha3Code = _oChanges.CtryAlpha3Code,
                //                CountryRegionId = null,
                //                GlobalChurchCode = _oChanges.GlobalChurchCode,
                //                RootChurchCode = _oChanges.RootChurchCode,
                //                //ChurchUnitLogo = _oChanges.ChurchLogo,
                //                ParentChurchBodyId = null,
                //                Status = "A",
                //                Created = DateTime.Now,
                //                LastMod = DateTime.Now,
                //                CreatedByUserId = _oChanges.CreatedByUserId,
                //                LastModByUserId = _oChanges.LastModByUserId
                //            };

                //            oCBCntAdd++;
                //            _cbCtx.Add(oCB);
                //        }
                //        else
                //        {
                //            var recUpdated = false;
                //            if (string.Compare(oCBList[0].Name, _oChanges.OwnerName, true) != 0) { oCBList[0].Name = _oChanges.OwnerName; recUpdated = true; }
                //            if (oCBList[0].AppGlobalOwnerId != _oChanges.Id) { oCBList[0].AppGlobalOwnerId = _oChanges.Id; recUpdated = true; }
                //            if (oCBList[0].ChurchLevelId != (oCL_1 != null ? oCL_1.Id : (int?)null)) { oCBList[0].ChurchLevelId = (oCL_1 != null ? oCL_1.Id : (int?)null); recUpdated = true; }
                //            if (oCBList[0].ParentChurchBodyId != null) { oCBList[0].ParentChurchBodyId = null; recUpdated = true; }
                //            if (oCBList[0].CtryAlpha3Code != _oChanges.CtryAlpha3Code) { oCBList[0].CtryAlpha3Code = _oChanges.CtryAlpha3Code; recUpdated = true; }
                //            if (string.Compare(oCBList[0].GlobalChurchCode, _oChanges.GlobalChurchCode, true) != 0) { oCBList[0].GlobalChurchCode = _oChanges.GlobalChurchCode; recUpdated = true; }
                //            if (string.Compare(oCBList[0].RootChurchCode, _oChanges.RootChurchCode, true) != 0) { oCBList[0].RootChurchCode = _oChanges.RootChurchCode; recUpdated = true; }

                //            if (recUpdated)
                //            {
                //                oCBList[0].LastMod = DateTime.Now;
                //                oCBList[0].LastModByUserId = _oChanges.LastModByUserId;
                //                //
                //                oCBCntUpd++;
                //                _cbCtx.Update(oCBList[0]);
                //            }
                //        }

                //        if ((oCBCntAdd + oCBCntUpd) > 0)
                //        {
                //            if (oCBCntAdd > 0)
                //            {
                //                _userTask = "Added Church Root unit for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                //                ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oCBCntAdd + " church root unit";
                //            }

                //            if (oCBCntUpd > 0)
                //            {
                //                _userTask = !string.IsNullOrEmpty(_userTask) ? _userTask + ". " : "" + "Updated Church Root unit for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                //                ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Denomination's " + oCBCntUpd + " Church Root unit updated.";
                //            }
                //        }
                //    }

                //    if ((oCBCntAdd + oCBCntUpd) > 0)
                //    {
                //        await _cbCtx.SaveChangesAsync();

                //        using (var agoCtx = new ChurchModelContext(_context.Database.GetDbConnection().ConnectionString))

                //            DetachAllEntities(_cbCtx);

                //        _tm = DateTime.Now;
                //        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                //                         "RCMS-Admin: Church Unit", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));
                //    }

                //}

                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving denomination (church) details. Err: " + ex.Message });
            }
 
        }


        /// CB --- sync this from the Vendor's central DB
        [HttpGet]
        public IActionResult AddOrEdit_CB(int id = 0, int? oAppGloOwnId = null, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) // (int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            try
            {
                if (id > 0)
                {
                    if (!InitializeUserLogging())
                        return RedirectToAction("LoginUserAcc", "UserLogin");
                     
                    var strDesc = "Denomination (Church)";
                    var _userTask = "Attempted accessing/modifying " + strDesc.ToLower();
                     

                    var oCBModel = (
                        from t_cb in _context.ChurchBody.Include(t => t.CountryRegion).Include(t => t.Country).AsNoTracking()
                                .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == id) // && (c.OrganisationType == "CH" || c.OrganisationType == "CN")c.OrganisationType == "CR" ||  // jux for structure
                        from t_ago in _context.AppGlobalOwner.AsNoTracking().Where(c => c.Id == t_cb.AppGlobalOwnerId)
                        from t_cl in _context.ChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb.ChurchLevelId).DefaultIfEmpty()
                        from t_cb_p in _context.ChurchBody.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb.ParentChurchBodyId && (c.OrganisationType != "CN")).DefaultIfEmpty()  // (c.OrganisationType != "CR" || c.OrganisationType == "CH" || c.OrganisationType == "CN")
                        from t_cl_p in _context.ChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb_p.ChurchLevelId).DefaultIfEmpty()
                        from t_ci in _context.ContactInfo.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.ChurchBodyId == t_cb.Id && c.Id == t_cb.ContactInfoId).DefaultIfEmpty()
                        from t_ci_ago in _context.ContactInfo.Include(t => t.Country).AsNoTracking()
                                        .Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.ChurchBodyId == null && c.Id == t_ago.ContactInfoId).DefaultIfEmpty()

                        select new ChurchBodyModel()
                        {
                            oAppGlobalOwn = t_ago,
                            oChurchBody = t_cb,
                            strOrgType = GetChuOrgTypeDesc(t_cb.OrganisationType),
                            strParentChurchBody = t_cb_p.Name,
                            //
                            //
                            // strFaithTypeCategory = t_cftc != null ? t_cftc.FaithDescription : "",
                            // strChurchStream = t_cfts != null ? t_cfts.FaithDescription : "", 

                            strFaithTypeCategory = t_ago.strFaithTypeCategory, // t_ago != null ? ((!string.IsNullOrEmpty(t_ago.strFaithTypeCategory) && !string.IsNullOrEmpty(t_ago.strFaithTypeStream) ? t_ago.strFaithTypeCategory + ", " + t_ago.strFaithTypeStream : t_ago.strFaithTypeCategory + t_ago.strFaithTypeStream).Trim()) : "",
                            strCountry = t_cb.Country != null ? (!string.IsNullOrEmpty(t_cb.Country.EngName) ? t_cb.Country.EngName : t_cb.Country.CtryAlpha3Code) : t_ago.CtryAlpha3Code,  //t_cb.Country != null ? t_cb.Country.EngName : "",
                            strCountryRegion = t_cb.CountryRegion != null ? t_cb.CountryRegion.Name : "",
                            strAppGloOwn = t_ago.OwnerName + (!string.IsNullOrEmpty(t_ago.OwnerName) && t_ci_ago.Country != null ? ", " + t_ci_ago.Country.EngName : (t_ci_ago.Country != null ? t_ci_ago.Country.EngName : "")),
                            strChurchBody = t_cb.Name,
                            strParentCB_HeaderDesc = !string.IsNullOrEmpty(t_cl.CustomName) ? t_cl_p.CustomName : "Parent Unit",
                            strChurchLevel = (t_cb.ChurchLevelId == null && t_cb.OrganisationType == "CR") ? "Church Root" : (!string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name),
                            numChurchLevel_Index = t_cl.LevelIndex,
                            // strCongLoc = t_ci.Location + (!string.IsNullOrEmpty(t_ci.Location) && !string.IsNullOrEmpty(t_ci.City) ? ", " + t_ci.City : t_ci_ago.City),
                            strCongLoc = (!string.IsNullOrEmpty(t_ci.Location) && !string.IsNullOrEmpty(t_ci.City) ? t_ci.Location + ", " + t_ci.City : t_ci.Location + t_ci.City).Trim(),
                            strCongLoc2 = (t_cb.CountryRegion != null && t_cb.Country != null ? t_cb.CountryRegion.Name + ", " + t_cb.Country.EngName : t_cb.CountryRegion.Name + t_cb.Country.EngName).Trim(),
                            blStatusActivated = t_cb.Status == "A",
                            dtCreated = t_cb.Created,
                            //   
                            strStatus = GetStatusDesc(t_cb.Status)
                        })
                        .OrderByDescending(c => c.dtCreated) //.OrderBy(c => c.strCountry).OrderBy(c => c.numCLIndex).OrderBy(c => c.strChurchBody)
                        .FirstOrDefault();


                   // oCSPModel.lsChurchBodyModels = oCB_List;
                    // oCSPModel.oChurchBodyModel = oCB_List.Count() > 0 ? oCB_List[0] : null;
                    // ViewData["oSetupData_CN_List"] = oCB_List;

                   // var oCBModel = oCB_List.Count() > 0 ? oCB_List[0] : null;

                    oCBModel.oCBLevelCount = oCBModel.numChurchLevel_Index - 1;        // oCBLevelCount -= 2;  // less requesting CB
                    List<ChurchLevel> oCBLevelList = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.LevelIndex > 0 && c.LevelIndex < oCBModel.numChurchLevel_Index).ToList().OrderBy(c => c.LevelIndex).ToList();
                    ///
                    if (oCBModel.oCBLevelCount > 0 && oCBLevelList.Count > 0)
                    {
                        oCBModel.strChurchLevel_1 = !string.IsNullOrEmpty(oCBLevelList[0].CustomName) ? oCBLevelList[0].CustomName : oCBLevelList[0].Name;
                        ViewBag.strChurchLevel_1 = oCBModel.strChurchLevel_1;
                        ///
                        var oCB_1 = _context.ChurchBody.Include(t => t.ChurchLevel)
                                          .Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && // c.Status == "A" && 
                                                c.ChurchLevel.LevelIndex == 1 && c.OrganisationType == "CR") //c.ChurchLevelId == oCBLevelList[0].Id &&
                                          .FirstOrDefault();

                        if (oCB_1 != null)
                        { oCBModel.ChurchBodyId_1 = oCB_1.Id; oCBModel.strChurchBody_1 = oCB_1.Name + " [Church Root]"; }

                        ViewBag.ChurchBodyId_1 = oCBModel.ChurchBodyId_1;
                        ViewBag.strChurchBody_1 = oCBModel.strChurchBody_1;

                        ///
                        if (oCBModel.oCBLevelCount > 1)
                        {
                            oCBModel.strChurchLevel_2 = !string.IsNullOrEmpty(oCBLevelList[1].CustomName) ? oCBLevelList[1].CustomName : oCBLevelList[1].Name;
                            ViewBag.strChurchLevel_2 = oCBModel.strChurchLevel_2;
                            ///
                            var lsCB_2 = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[1].Id).ToList();
                            var oCB_2 = lsCB_2.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                            if (oCB_2.Count() != 0)
                            { oCBModel.ChurchBodyId_2 = oCB_2[0].Id; oCBModel.strChurchBody_2 = oCB_2[0].Name; }
                            ViewBag.ChurchBodyId_2 = oCBModel.ChurchBodyId_2; ViewBag.strChurchBody_2 = oCBModel.strChurchBody_2;

                            if (oCBModel.oCBLevelCount > 2)
                            {
                                oCBModel.strChurchLevel_3 = !string.IsNullOrEmpty(oCBLevelList[2].CustomName) ? oCBLevelList[2].CustomName : oCBLevelList[2].Name;
                                ViewBag.strChurchLevel_3 = oCBModel.strChurchLevel_3;

                                var lsCB_3 = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[2].Id).ToList();
                                var oCB_3 = lsCB_3.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                if (oCB_3.Count() != 0)
                                { oCBModel.ChurchBodyId_3 = oCB_3[0].Id; oCBModel.strChurchBody_3 = oCB_3[0].Name; }
                                ViewBag.ChurchBodyId_3 = oCBModel.ChurchBodyId_3; ViewBag.strChurchBody_3 = oCBModel.strChurchBody_3;


                                if (oCBModel.oCBLevelCount > 3)
                                {
                                    oCBModel.strChurchLevel_4 = !string.IsNullOrEmpty(oCBLevelList[3].CustomName) ? oCBLevelList[3].CustomName : oCBLevelList[3].Name;
                                    ViewBag.strChurchLevel_4 = oCBModel.strChurchLevel_4;

                                    var lsCB_4 = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[3].Id).ToList();
                                    var oCB_4 = lsCB_4.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                    if (oCB_4.Count() != 0)
                                    { oCBModel.ChurchBodyId_4 = oCB_4[0].Id; oCBModel.strChurchBody_4 = oCB_4[0].Name; }
                                    ViewBag.ChurchBodyId_4 = oCBModel.ChurchBodyId_4; ViewBag.strChurchBody_4 = oCBModel.strChurchBody_4;


                                    if (oCBModel.oCBLevelCount > 4)
                                    {
                                        oCBModel.strChurchLevel_5 = !string.IsNullOrEmpty(oCBLevelList[4].CustomName) ? oCBLevelList[4].CustomName : oCBLevelList[4].Name;
                                        ViewBag.strChurchLevel_5 = oCBModel.strChurchLevel_4;

                                        var lsCB_5 = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[4].Id).ToList();
                                        var oCB_5 = lsCB_5.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                        if (oCB_5.Count() != 0)
                                        { oCBModel.ChurchBodyId_5 = oCB_5[0].Id; oCBModel.strChurchBody_5 = oCB_5[0].Name; }
                                        ViewBag.ChurchBodyId_5 = oCBModel.ChurchBodyId_5; ViewBag.strChurchBody_5 = oCBModel.strChurchBody_5;
                                    }
                                }
                            }
                        }
                    }
                     
                   // oCSPModel.oChurchBodyModel = oCBModel;
                    
                    if (oCBModel.oAppGlobalOwn == null)
                    {
                        //page not found error
                        Response.StatusCode = 500;
                        return View("ErrorPage");
                    }
                      
                    _userTask = "Opened " + strDesc.ToLower() + ", " + oCBModel.oAppGlobalOwn.OwnerName;

                    oCBModel.setIndex = 2;
                    // oAGO_MDL.subSetIndex = subSetIndex;

                    oCBModel.oUserId_Logged = oUserId_Logged;
                    oCBModel.oAppGloOwnId_Logged = oAGOId_Logged;
                    oCBModel.oChurchBodyId_Logged = oCBId_Logged;
                     
                     
                    oCBModel = this.popLookups_CB(oCBModel, oCBModel.oChurchBody);

                    var tm = DateTime.Now;
                    _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, null, null, "T",
                                     "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oUserId_Logged, tm, tm, oUserId_Logged, oUserId_Logged)
                        , this._strClientConn);


                    var _oCBModel = Newtonsoft.Json.JsonConvert.SerializeObject(oCBModel);
                    TempData["oVmCurrMod"] = _oCBModel; TempData.Keep();

                    return PartialView("_AddOrEdit_CB", oCBModel);  
                }

                // shouldn't get this far
                //page not found error
                Response.StatusCode = 500;
                return View("ErrorPage");
            }

            catch (Exception ex)
            {
                //page not found error
                Response.StatusCode = 500;
                return View("ErrorPage");
            }
        }
        public ChurchBodyModel popLookups_CB(ChurchBodyModel vm, ChurchBody oCurrChurchBody)
        {
            if (vm == null || oCurrChurchBody == null) return vm;
            //
            vm.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            vm.lkpOrgTypes = new List<SelectListItem>();
            foreach (var dl in dlCBDivOrgTypes) { vm.lkpOrgTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }
             
            vm.lkpChurchLevels = _context.ChurchLevel.Where(c => c.AppGlobalOwnerId == oCurrChurchBody.AppGlobalOwnerId)
                                              .OrderByDescending(c => c.LevelIndex)
                                              .Select(c => new SelectListItem()
                                              {
                                                  Value = c.Id.ToString(),
                                                  Text = !string.IsNullOrEmpty(c.CustomName) ? c.CustomName : c.Name
                                              })
                                              .ToList(); 

            vm.lkpCountries = _context.Country.ToList()  //.Where(c => c.Display == true)
                                          .Select(c => new SelectListItem()
                                          {
                                              Value = c.CtryAlpha3Code, // .ToString(),
                                              Text = c.EngName
                                          })
                                          .OrderBy(c => c.Text)
                                          .ToList(); 
            return vm;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit_CB(ChurchBodyModel vm)
        {
            if (!InitializeUserLogging())
                return RedirectToAction("LoginUserAcc", "UserLogin");
            var strDesc = "Church unit";
            // var _userTask = "Attempted accessing/modifying " + strDesc.ToLower(); 

            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = vm.pageIndex });
            if (vm.oChurchBody == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = vm.pageIndex });

            ChurchBody _oChanges = vm.oChurchBody;


            /// server validations
            ///   
            if (string.IsNullOrEmpty(_oChanges.OrganisationType))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church unit organisation type [Congregation or Congregation Head-unit] is not specified", pageIndex = vm.pageIndex });
            strDesc = _oChanges.OrganisationType == "CH" ? "Congregation Head-unit" : _oChanges.OrganisationType == "CN" ? "Congregation" : "Church unit";

            if (_oChanges.AppGlobalOwnerId == null)
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify the denomination (church).", pageIndex = vm.pageIndex });

            var oAGO = _context.AppGlobalOwner.Find(_oChanges.AppGlobalOwnerId);
            if (oAGO == null)  // let's know the denomination... for prefic code
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Denomination (church) for " + strDesc.ToLower() + " could not be found. Please refresh and try again", pageIndex = vm.pageIndex });

            // check...
            if (string.IsNullOrEmpty(oAGO.PrefixKey))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church prefix code has not been specified. Hint: configure via Denominations" });

            if (string.IsNullOrEmpty(_oChanges.Name))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the " + strDesc.ToLower() + " name", pageIndex = vm.pageIndex });

            if (_oChanges.ChurchLevelId == null)
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify the church level.", pageIndex = vm.pageIndex });

            var oCBLevel = _context.ChurchLevel.Find(_oChanges.ChurchLevelId);
            if (oCBLevel == null)  // ... parent church level > church unit level
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church unit level could not be found. Please refresh and try again", pageIndex = vm.pageIndex });

            /// get the parent id            
            var parDesc = "church unit";
            switch (vm.oCBLevelCount)
            {
                case 1: _oChanges.ParentChurchBodyId = vm.ChurchBodyId_1; parDesc = vm.strChurchLevel_1; break;
                case 2: _oChanges.ParentChurchBodyId = vm.ChurchBodyId_2; parDesc = vm.strChurchLevel_2; break;
                case 3: _oChanges.ParentChurchBodyId = vm.ChurchBodyId_3; parDesc = vm.strChurchLevel_3; break;
                case 4: _oChanges.ParentChurchBodyId = vm.ChurchBodyId_4; parDesc = vm.strChurchLevel_4; break;
                case 5: _oChanges.ParentChurchBodyId = vm.ChurchBodyId_5; parDesc = vm.strChurchLevel_5; break;
            }

            if (_oChanges.ParentChurchBodyId == null)
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church structure is networked. Provide the parent " + parDesc.ToLower(), pageIndex = vm.pageIndex });

            var oCBParent = _context.ChurchBody.Include(t => t.ChurchLevel).Where(c => c.Id == _oChanges.ParentChurchBodyId).FirstOrDefault();
            if (oCBParent == null)  // let's know the parent church unit... parent church level > church unit level
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parent church unit could not be found. Please refresh and try again", pageIndex = vm.pageIndex });

            if (oCBLevel.LevelIndex <= oCBParent.ChurchLevel.LevelIndex)  // ... parent church level > church unit level
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church unit level cannot be higher or same as parent church unit. Please select the correct parent unit or change church unit level", pageIndex = vm.pageIndex });

            if (_oChanges.CtryAlpha3Code == null)   // auto-fill the country and regions using the parent details...
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the base country.", pageIndex = vm.pageIndex });

            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ChurchBodyModel>(arrData) : vm;
            var oCB = vmMod.oChurchBody;

            try
            {
                ModelState.Remove("oChurchBody.AppGlobalOwnerId");
                //ModelState.Remove("oChurchBody.CountryId");
                ModelState.Remove("oChurchBody.CtryAlpha3Code");
                ModelState.Remove("oChurchBody.CountryRegionId");
                ModelState.Remove("oChurchBody.ParentChurchBodyId");
                ModelState.Remove("oChurchBody.ContactInfoId");
                ModelState.Remove("oChurchBody.ChurchLevelId");
                ModelState.Remove("oChurchBody.OrganisationType");
                ModelState.Remove("oChurchBody.GlobalChurchCode");
                ModelState.Remove("oChurchBody.RootChurchCode");
                //  ModelState.Remove("oAppGlobalOwn.OwnerName");
                //
                //ModelState.Remove("oCurrChurchBody.Id");
                //ModelState.Remove("oCurrChurchBody.Name");
                //

                ModelState.Remove("oChurchBody.CreatedByUserId");
                ModelState.Remove("oChurchBody.LastModByUserId");

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", pageIndex = vm.pageIndex });


                ////church code  
                //if (string.IsNullOrEmpty(_oChanges.GlobalChurchCode) && !string.IsNullOrEmpty(oAGO.PrefixKey))
                //{
                //    //var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                //    var jsCode = GetNextGlobalChurchCodeByAcronym_jsonString(oAGO.PrefixKey, _oChanges.AppGlobalOwnerId);  // string json1 = @"{'Name':'James'}";
                //    _oChanges.GlobalChurchCode = jsCode;
                //}


                ////root church code  
                //if (string.IsNullOrEmpty(_oChanges.RootChurchCode) && !string.IsNullOrEmpty(_oChanges.GlobalChurchCode))
                //{
                //    // var template = new { taskSuccess = String.Empty, strRes = String.Empty };
                //    var jsCode = GetNextRootChurchCodeByParentCB_jsonString(oAGO.PrefixKey, _oChanges.AppGlobalOwnerId, _oChanges.ParentChurchBodyId, _oChanges.GlobalChurchCode);
                //    _oChanges.RootChurchCode = jsCode;
                //}

                // check...
                if (string.IsNullOrEmpty(_oChanges.GlobalChurchCode) || string.IsNullOrEmpty(_oChanges.RootChurchCode))
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church code and Root church code for " + strDesc.ToLower() + " must be specified", pageIndex = vm.pageIndex });


                //// church logo
                //if (_oChanges.ChurchUnitLogo.ToLower() != (Guid.NewGuid().ToString() + "_" + vm.ChurchLogoFile.FileName).ToLower())
                //{
                //    string strFilename = null;
                //    if (vm.ChurchLogoFile != null && vm.ChurchLogoFile.Length > 0)
                //    {
                //        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                //        strFilename = Guid.NewGuid().ToString() + "_" + vm.ChurchLogoFile.FileName;
                //        string filePath = Path.Combine(uploadFolder, strFilename);
                //        vm.ChurchLogoFile.CopyTo(new FileStream(filePath, FileMode.Create));
                //    }
                //    else
                //    {
                //        if (vm.oChurchBody.Id != 0) strFilename = vm.strChurchLogo;
                //    }

                //    _oChanges.ChurchUnitLogo = strFilename;
                //}

                //
                var tm = DateTime.Now;
                _oChanges.LastMod = tm;
                _oChanges.LastModByUserId = vm.oUserId_Logged;
                _oChanges.Status = vm.blStatusActivated ? "A" : "D";

                var _reset = _oChanges.Id == 0;

                //validate...
                var _userTask = "Attempted saving " + strDesc.ToLower() + ", " + _oChanges.Name.ToUpper();  // _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.Name.ToUpper() + " successfully";   //  _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.Name.ToUpper() + " successfully";
                using (var _cbCtx = new ChurchModelContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    if (_oChanges.Id == 0)
                    {
                        var oCBVal = _context.ChurchBody  //.Include(t=>t.ParentChurchBody)
                            .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ParentChurchBodyId == oCB.ParentChurchBodyId && c.Name == oCB.Name).FirstOrDefault();
                        if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = strDesc + ", " + oCBVal.Name + " already exists.", pageIndex = vm.pageIndex });

                        oCBVal = _context.ChurchBody  //.Include(t => t.ParentChurchBody)
                            .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId &&
                                    (c.GlobalChurchCode == oCB.GlobalChurchCode //||  c.RootChurchCode == oCB.RootChurchCode || 
                                                                                // (oCB.ChurchCodeCustom != null && c.ChurchCodeCustom == oCB.ChurchCodeCustom)
                                   )).FirstOrDefault();

                        if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Church codes must be unique." + Environment.NewLine + oCBVal.Name + " has same church code.", pageIndex = vm.pageIndex });

                        _oChanges.Created = tm;
                        _oChanges.CreatedByUserId = vm.oUserId_Logged;
                        _cbCtx.Add(_oChanges);

                        ViewBag.UserMsg = "Saved " + strDesc.ToLower() + (!string.IsNullOrEmpty(_oChanges.Name) ? ", " + _oChanges.Name : "") + " successfully.";
                        _userTask = "Added new " + strDesc.ToLower() + (!string.IsNullOrEmpty(_oChanges.Name) ? ", " + _oChanges.Name : "") + " successfully";
                    }

                    else
                    {
                        var oCBVal = _context.ChurchBody  //.Include(t => t.ParentChurchBody)
                            .Where(c => c.Id != oCB.Id && c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ParentChurchBodyId == oCB.ParentChurchBodyId && c.Name == oCB.Name).FirstOrDefault();
                        if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = strDesc + ", " + oCBVal.Name + " already exists.", pageIndex = vm.pageIndex });

                        // oCBVal = _context.ChurchBody.Include(t => t.ParentChurchBody).Where(c => c.Id != oCB.Id && c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ChurchCode == oCB.ChurchCode ).FirstOrDefault();
                        //if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Church code must be unique." + Environment.NewLine + 
                        //        oCBVal.Name + (oCBVal.ParentChurchBody != null ? " of " + oCBVal.ParentChurchBody.Name : "") + " has  same code."});


                        oCBVal = _context.ChurchBody   //.Include(t => t.ParentChurchBody)
                            .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.Id != oCB.Id &&
                                   (c.GlobalChurchCode == oCB.GlobalChurchCode //|| c.RootChurchCode == oCB.RootChurchCode ||
                                                                               // (oCB.ChurchCodeCustom != null && c.ChurchCodeCustom == oCB.ChurchCodeCustom)
                                   )).FirstOrDefault();

                        if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Church codes must be unique." + Environment.NewLine + oCBVal.Name + " has same church code.", pageIndex = vm.pageIndex });


                        //retain the pwd details... hidden fields


                        _context.Update(_oChanges);
                        //var _strDesc = strDesc.Length > 0 ? strDesc.Substring(0, 1).ToUpper() + strDesc.Substring(1) : "Denomination ";

                        ViewBag.UserMsg = strDesc + (!string.IsNullOrEmpty(_oChanges.Name) ? ", " + _oChanges.Name : "") + " updated successfully.";
                        _userTask = "Updated " + strDesc.ToLower() + (!string.IsNullOrEmpty(_oChanges.Name) ? ", " + _oChanges.Name : "") + " successfully";
                    }

                    //save denomination first... 
                    _context.SaveChanges(); // await  _cbCtx.SaveChangesAsync();


                    //DetachAllEntities(_cbCtx);
                }



                var _tm = DateTime.Now;
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, null, null, "T",
                                 "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oCurrUserId_Logged, _tm, _tm, vm.oCurrUserId_Logged, vm.oCurrUserId_Logged),
                                 _strClientConn);


                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg, pageIndex = vm.pageIndex });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving church unit details. Err: " + ex.Message, pageIndex = vm.pageIndex });
            }
        }

         


        /// NVP --- General App Lookups 
        [HttpGet]
        public IActionResult AddOrEdit_NVP(int id = 0, int? oAppGloOwnId = null, int? oChurchBodyId = null, int? oUserId = null)  
        {
            try
            {
                
                if (!InitializeUserLogging()) 
                    return RedirectToAction("LoginUserAcc", "UserLogin");

                if (oAppGloOwnId == null) oAppGloOwnId = this._oLoggedAGO.Id;
                if (oChurchBodyId == null) oChurchBodyId = this._oLoggedCB.Id;
                if (oUserId == null) oUserId = this._oLoggedUser.Id;    

                var strDesc = "App Lookup Parameter";
                    var _userTask = "Attempted accessing/modifying " + strDesc.ToLower();  // _userTask = "Attempted creating new denomination (church)"; // _userTask = "Opened denomination (church)-" + oCFT_MDL.oChurchFaithType.FaithDescription;

                var oNVP_MDL = new AppUtilityNVPModel();
                if (id == 0)
                {
                    //create and init...  
                    oNVP_MDL.oAppUtilityNVP = new AppUtilityNVP();
                    var oCLIndex = _context.AppUtilityNVP.Count(c => c.AppGlobalOwnerId == this._oLoggedAGO.Id && c.ChurchBodyId == this._oLoggedCB.Id) + 1;
                    oNVP_MDL.oAppUtilityNVP.OrderIndex = oCLIndex;
                    oNVP_MDL.oAppUtilityNVP.NVPStatus = "A"; 

                    _userTask = "Attempted creating new " + strDesc.ToLower();
                }
                else
                {
                    oNVP_MDL = (
                              from t_nvp in _context.AppUtilityNVP.AsNoTracking().Include(t=>t.AppGlobalOwner).Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId)
                              from t_nvp_c in _context.AppUtilityNVP.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_nvp.AppGlobalOwnerId && c.Id == t_nvp.NVPCategoryId).DefaultIfEmpty()

                              select new AppUtilityNVPModel()
                              {
                                  oAppUtilityNVP = t_nvp, 
                                  strAppGloOwn = t_nvp.AppGlobalOwner != null ? t_nvp.AppGlobalOwner.OwnerName : "",
                                  strNVPTag = GetNVPTagDesc(t_nvp.NVPCode),
                                //  strAppUtilityNVPName = t_nvp.NVPValue ,
                                  strNVPCategory = t_nvp_c != null ? t_nvp_c.NVPValue : "", 
                                  strNVPStatus = GetStatusDesc(t_nvp.NVPStatus) 
                              })
                          .FirstOrDefault();
                     

                    if (oNVP_MDL.oAppUtilityNVP == null)
                    {
                        //page not found error
                        Response.StatusCode = 500;
                        return View("ErrorPage");
                    }
                     
                    _userTask = "Opened " + strDesc.ToLower() + ", " + oNVP_MDL.oAppUtilityNVP.NVPCode + ": " + oNVP_MDL.oAppUtilityNVP.NVPValue ; 
                }



                oNVP_MDL.setIndex = 6;
                // oNVP_MDL.subSetIndex = subSetIndex;

                oNVP_MDL.oAppUtilityNVP.AppGlobalOwnerId = oAppGloOwnId;
                oNVP_MDL.oAppUtilityNVP.ChurchBodyId = oChurchBodyId;

                oNVP_MDL.oUserId_Logged = this._oLoggedUser.Id;
                oNVP_MDL.oAppGloOwnId_Logged = this._oLoggedAGO.Id;
                oNVP_MDL.oChurchBodyId_Logged = this._oLoggedCB.Id;
                // 
                oNVP_MDL = this.popLookups_NVP(oNVP_MDL, oNVP_MDL.oAppUtilityNVP);

                var tm = DateTime.Now;
                // record ... @client
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, _oLoggedCB.Id, "N",
                                 "RCMS-Client: App Lookup Parameter", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, _oLoggedUser.Id, tm, tm, _oLoggedUser.Id, _oLoggedUser.Id)
                                , this._strClientConn);

                //var _oNVP_MDL = Newtonsoft.Json.JsonConvert.SerializeObject(oNVP_MDL);
                //TempData["oVmCurrMod"] = _oNVP_MDL; TempData.Keep();

                //  return oNVP_MDL;.


                return PartialView("_AddOrEdit_NVP", oNVP_MDL);

                // shouldn't get this far
                //page not found error
                //Response.StatusCode = 500;
                //return View("ErrorPage");
            }

            catch (Exception ex)
            {
                //page not found error
                Response.StatusCode = 500;
                return View("ErrorPage");
            }
        }

        public AppUtilityNVPModel popLookups_NVP(AppUtilityNVPModel vm, AppUtilityNVP oCurrNVP)
        {
            if (vm == null || oCurrNVP == null) return vm;
            //
            vm.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) 
            { if (dl.Val == "A" || dl.Val=="B") vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); } 
            
            vm.lkpAppParameterTags = new List<SelectListItem>();
            foreach (var dl in dlNVPCodes) { vm.lkpAppParameterTags.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            vm.lkpNVPCategories = new List<SelectListItem>();
            // get other values except current value... in that NVPCode
            vm.lkpNVPCategories = _context.AppUtilityNVP.Where(c => c.AppGlobalOwnerId == oCurrNVP.AppGlobalOwnerId && c.ChurchBodyId == oCurrNVP.AppGlobalOwnerId && 
                                                                    c.NVPCode == oCurrNVP.NVPCode && c.Id != oCurrNVP.Id)
                                             .OrderBy(c => c.OrderIndex)
                                             .ThenBy(c => c.NVPValue)
                                             .Select(c => new SelectListItem()
                                             {
                                                 Value = c.Id.ToString(),
                                                 Text = c.NVPValue
                                             })
                                             .ToList();
            //vm.lkpNVPCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });


            return vm;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit_NVP(AppUtilityNVPModel vm)
        {
            if (!InitializeUserLogging())
                return RedirectToAction("LoginUserAcc", "UserLogin");

            var strDesc = "App Lookup Parameter";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });
            if (vm.oAppUtilityNVP == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });

            AppUtilityNVP _oChanges = vm.oAppUtilityNVP;  // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();

            if (string.IsNullOrEmpty(_oChanges.NVPCode))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parameter code for " + strDesc.ToLower() + " must be specified" });

            if (string.IsNullOrEmpty(_oChanges.NVPValue))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parameter value for " + strDesc.ToLower() + " must be specified else delete parameter" });

            // initial validations done!
            var strTag = GetNVPTagDesc(_oChanges.NVPCode); 

            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<AppUtilityNVPModel>(arrData) : vm;

            var oNVP = vmMod.oAppUtilityNVP;
            // oNVP.ChurchBody = vmMod.oChurchBody;

            try
            {
                ModelState.Remove("oAppUtilityNVP.AppGlobalOwnerId");
                ModelState.Remove("oAppUtilityNVP.ChurchBodyId");
                ModelState.Remove("oAppUtilityNVP.NVPCategoryId");
                ModelState.Remove("oAppUtilityNVP.NVPCode");
                ModelState.Remove("oAppUtilityNVP.CreatedByUserId");
                ModelState.Remove("oAppUtilityNVP.LastModByUserId");

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

                //// church logo
                //if (vm.ChurchLogoFile != null) //&& _oChanges.ChurchLogo != null
                //{
                //    if (_oChanges.ChurchLogo != vm.ChurchLogoFile.FileName)
                //    {
                //        string strFilename = null;
                //        if (vm.ChurchLogoFile != null && vm.ChurchLogoFile.Length > 0)
                //        {
                //            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img_db");
                //            strFilename = Guid.NewGuid().ToString() + "_" + vm.ChurchLogoFile.FileName;
                //            string filePath = Path.Combine(uploadFolder, strFilename);
                //            vm.ChurchLogoFile.CopyTo(new FileStream(filePath, FileMode.Create));
                //        }
                //        else
                //        {
                //            if (vm.oAppGlobalOwn.Id != 0) strFilename = vm.strChurchLogo;
                //        }

                //        _oChanges.ChurchLogo = strFilename;
                //    }
                //}

                //
                var tm = DateTime.Now;
                _oChanges.LastMod = tm;
                _oChanges.LastModByUserId = vm.oUserId_Logged;
                //_oChanges.Status = vm.blStatusActivated ? "A" : "D"; 
                  
                //validate...
                var _userTask = "Attempted saving " + strDesc.ToLower() + ", " + strTag;  // _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                var _reset = _oChanges.Id == 0;


                //using (var _NVPCtx = new ChurchModelContext(_context.Database.GetDbConnection().ConnectionString))
                //{
                if (_oChanges.Id == 0)
                {
                    var existNVP = _context.AppUtilityNVP.Where(c => c.NVPCategoryId == _oChanges.NVPCategoryId && c.NVPValue.ToLower() == _oChanges.NVPValue.ToLower()).ToList();
                    if (existNVP.Count() > 0)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " -- [" + (strTag + ":- " + _oChanges.NVPValue) + "] already exist." }); 

                    _oChanges.Created = tm;
                    _oChanges.CreatedByUserId = vm.oUserId_Logged;

                    _context.Add(_oChanges);

                    ViewBag.UserMsg = "Saved " + strDesc.ToLower() + " [" + (!string.IsNullOrEmpty(strTag + _oChanges.NVPValue) ? (strTag + ":- " + _oChanges.NVPValue) : strTag + _oChanges.NVPValue) + "] successfully.";
                    _userTask = "Added new " + strDesc.ToLower() + ", [" + (strTag + ":- " + _oChanges.NVPValue) + "] successfully";
                }

                else

                {
                    var existNVP = _context.AppUtilityNVP.Where(c => c.Id != _oChanges.Id && c.NVPCategoryId == _oChanges.NVPCategoryId && c.NVPValue.ToLower() == _oChanges.NVPValue.ToLower()).ToList();
                    if (existNVP.Count() > 0)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " -- [" + (strTag + ":- " + _oChanges.NVPValue) + "] already exist." });
                      
                    _context.Update(_oChanges); 

                    ViewBag.UserMsg = strDesc + " updated successfully.";
                    _userTask = "Updated " + strDesc.ToLower() + ", [" + (strTag + ":- " + _oChanges.NVPValue) + "] successfully";
                }

                //save denomination first... 
                _context.SaveChanges(); // await _NVPCtx.SaveChangesAsync();

                //   DetachAllEntities(_NVPCtx);
                //}


                var _tm = DateTime.Now; 
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, _oLoggedCB.Id, "N",
                                     "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, _oLoggedUser.Id, tm, tm, _oLoggedUser.Id, _oLoggedUser.Id)
                                    , this._strClientConn);

                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving app lookup parameter details. Err: " + ex.Message });
            }

        }
        public IActionResult Delete_NVP(int? oAppGloOwnId, int? oChurchBodyId, int? loggedUserId, int id, bool forceDeleteConfirm = false)
        {
            if (!InitializeUserLogging())
                return RedirectToAction("LoginUserAcc", "UserLogin");

            // var strDesc = setIndex == 1 ? "System profile" : setIndex == 2 ? "Church admin profile" : "Church user profile";
            var strDesc = "App lookup parameter";
            var tm = DateTime.Now; var _tm = DateTime.Now; var _userTask = "Attempted saving  " + strDesc;
            //
            try
            {
                var strUserDenom = "RCMS Client";

                //if (setIndex != 1)
                //{
                //    if (oAppGloOwnId == null || oChurchBodyId == null)
                //        return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Denomination/church of " + strDesc + " unknown. Please refesh and try again." });

                //    var oAGO = _context.AppGlobalOwner.Find(oAppGloOwnId);
                //    var oCB = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oChurchBodyId).FirstOrDefault();

                //    if (oAGO == null || oCB == null)
                //        return Json(new { taskSuccess = false, oCurrId = "", userMess = "Specified denomination and church unit could not be retrieved. Please refresh and try again.", signOutToLogIn = false });

                //    strUserDenom = oCB.Name + (!string.IsNullOrEmpty(oAGO.Acronym) ? ", " + oAGO.Acronym : oAGO.OwnerName);
                //    strUserDenom = "--" + (string.IsNullOrEmpty(strUserDenom) ? "Denomination: " + strUserDenom : strUserDenom);
                //}

                
                var oNVP = _context.AppUtilityNVP.Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId).FirstOrDefault();// .Include(c => c.ChurchUnits)
                if (oNVP == null)
                {
                    _userTask = "Attempted deleting " + strDesc.ToLower(); // + ", " + (strTag + ":- " + oNVP.NVPValue);  // var _userTask = "Attempted saving  " + strDesc;
                    _tm = DateTime.Now; 
                    _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                         "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                        , this._strClientConn);

                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " to delete could not be retrieved." });
                }
                
                var strTag = GetNVPTagDesc(oNVP.NVPCode);          
                var saveDelete = true;
                // ensuring cascade delete where there's none!

                //check NVPCategory for this UP to delete 

                var NVPs = _context.AppUtilityNVP.Where(c => c.NVPCategoryId == oNVP.Id).ToList();

                //using (var _userCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                //{
                    if (NVPs.Count() > 0) // + UPGs.Count() + oUPRs.Count() //+oUser.ChurchMembers.Count )
                    {
                        if (forceDeleteConfirm == false)
                        {
                            var strConnTabs = "Parameter category";  //User profile role, User profile group and 
                            saveDelete = false;

                            // check user privileges to determine... administrator rights
                            //log...
                            _userTask = "Attempted deleting " + strDesc.ToLower() + ", [" + (strTag + ":- " + oNVP.NVPValue) + "]";
                            _tm = DateTime.Now; 
                            _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                                 "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                                , this._strClientConn);

                            return Json(new
                            {
                                taskSuccess = false,
                                tryForceDelete = false,
                                oCurrId = id,
                                userMess = "Specified " + strDesc.ToLower() +
                                                " to delete has been used elsewhere in the system [" + strConnTabs + "]. Delete cannot be done unless dependent-references are removed."
                            });
                        }

                        //to be executed only for higher privileges... // FORCE-DELETE...
                    }

                    //successful...
                    if (saveDelete)
                    {
                    _context.AppUtilityNVP.Remove(oNVP);
                    _context.SaveChanges();

                     //   DetachAllEntities(_userCtx);

                        //audit...
                        _userTask = "Deleted " + strDesc.ToLower() + ", [" + (strTag + ":- " + oNVP.NVPValue) + "]";
                        _tm = DateTime.Now;
                    _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                             "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                            , this._strClientConn);

                    return Json(new { taskSuccess = true, tryForceDelete = false, oCurrId = oNVP.Id, userMess = strDesc + " successfully deleted." });
                    }

                //}

                
                _userTask = "Attempted deleting " + strDesc.ToLower() + ", [" + (strTag + ":- " + oNVP.NVPValue) + "] -- but FAILED. Data unavailable.";   // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                                 "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                                , this._strClientConn);

                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "No " + strDesc.ToLower() + " data available to delete. Try again" });
            }

            catch (Exception ex)
            {
                _userTask = "Attempted deleting " + strDesc.ToLower() + ", [ ID= " + id + "] FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                                    "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                                   , this._strClientConn);
                //
                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Failed deleting " + strDesc.ToLower() + ". Err: " + ex.Message });
            }
        }




        ///// CTRY --- General App Lookups 
        //[HttpGet]
        //public IActionResult AddOrEdit_CTRY(int id = 0, int? oAppGloOwnId = null, int? oChurchBodyId = null, int? oUserId = null)
        //{
        //    try
        //    {

        //        if (!InitializeUserLogging())
        //            return RedirectToAction("LoginUserAcc", "UserLogin");

        //        if (oAppGloOwnId == null) oAppGloOwnId = this._oLoggedAGO.Id;
        //        if (oChurchBodyId == null) oChurchBodyId = this._oLoggedCB.Id;
        //        if (oUserId == null) oUserId = this._oLoggedUser.Id;

        //        var strDesc = "App Lookup Parameter";
        //        var _userTask = "Attempted accessing/modifying " + strDesc.ToLower();  // _userTask = "Attempted creating new denomination (church)"; // _userTask = "Opened denomination (church)-" + oCFT_MDL.oChurchFaithType.FaithDescription;

        //        var oCTRY_MDL = new CountryModel();
        //        if (id == 0)
        //        {
        //            //create and init...  
        //            oCTRY_MDL.oCountry = new Country();
        //            var oCLIndex = _context.Country.Count(c => c.AppGlobalOwnerId == this._oLoggedAGO.Id && c.ChurchBodyId == this._oLoggedCB.Id) + 1;
        //            oCTRY_MDL.oCountry.OrderIndex = oCLIndex;
        //            oCTRY_MDL.oCountry.CTRYStatus = "A";

        //            _userTask = "Attempted creating new " + strDesc.ToLower();
        //        }
        //        else
        //        {
        //            oCTRY_MDL = (
        //                      from t_CTRY in _context.Country.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId)
        //                      from t_CTRY_c in _context.Country.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_CTRY.AppGlobalOwnerId && c.Id == t_CTRY.CTRYCategoryId).DefaultIfEmpty()

        //                      select new CountryModel()
        //                      {
        //                          oCountry = t_CTRY,
        //                          strAppGloOwn = t_CTRY.AppGlobalOwner != null ? t_CTRY.AppGlobalOwner.OwnerName : "",
        //                          strCTRYTag = GetCTRYTagDesc(t_CTRY.CTRYCode),
        //                          //  strCountryName = t_CTRY.CTRYValue ,
        //                          strCTRYCategory = t_CTRY_c != null ? t_CTRY_c.CTRYValue : "",
        //                          strCTRYStatus = GetStatusDesc(t_CTRY.CTRYStatus)
        //                      })
        //                  .FirstOrDefault();


        //            if (oCTRY_MDL.oCountry == null)
        //            {
        //                //page not found error
        //                Response.StatusCode = 500;
        //                return View("ErrorPage");
        //            }

        //            _userTask = "Opened " + strDesc.ToLower() + ", " + oCTRY_MDL.oCountry.CTRYCode + ": " + oCTRY_MDL.oCountry.CTRYValue;
        //        }



        //        oCTRY_MDL.setIndex = 6;
        //        // oCTRY_MDL.subSetIndex = subSetIndex;

        //        oCTRY_MDL.oCountry.AppGlobalOwnerId = oAppGloOwnId;
        //        oCTRY_MDL.oCountry.ChurchBodyId = oChurchBodyId;

        //        oCTRY_MDL.oUserId_Logged = this._oLoggedUser.Id;
        //        oCTRY_MDL.oAppGloOwnId_Logged = this._oLoggedAGO.Id;
        //        oCTRY_MDL.oChurchBodyId_Logged = this._oLoggedCB.Id;
        //        // 
        //        oCTRY_MDL = this.popLookups_CTRY(oCTRY_MDL, oCTRY_MDL.oCountry);

        //        var tm = DateTime.Now;
        //        // record ... @client
        //        _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, _oLoggedCB.Id, "N",
        //                         "RCMS-Client: App Lookup Parameter", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, _oLoggedUser.Id, tm, tm, _oLoggedUser.Id, _oLoggedUser.Id)
        //                        , this._strClientConn);

        //        //var _oCTRY_MDL = Newtonsoft.Json.JsonConvert.SerializeObject(oCTRY_MDL);
        //        //TempData["oVmCurrMod"] = _oCTRY_MDL; TempData.Keep();

        //        //  return oCTRY_MDL;.


        //        return PartialView("_AddOrEdit_CTRY", oCTRY_MDL);

        //        // shouldn't get this far
        //        //page not found error
        //        //Response.StatusCode = 500;
        //        //return View("ErrorPage");
        //    }

        //    catch (Exception ex)
        //    {
        //        //page not found error
        //        Response.StatusCode = 500;
        //        return View("ErrorPage");
        //    }
        //}

        //public CountryModel popLookups_CTRY(CountryModel vm, Country oCurrCTRY)
        //{
        //    if (vm == null || oCurrCTRY == null) return vm;
        //    //
        //    vm.lkpStatuses = new List<SelectListItem>();
        //    foreach (var dl in dlGenStatuses)
        //    { if (dl.Val == "A" || dl.Val == "B") vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

        //    vm.lkpAppParameterTags = new List<SelectListItem>();
        //    foreach (var dl in dlCTRYCodes) { vm.lkpAppParameterTags.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

        //    vm.lkpCTRYCategories = new List<SelectListItem>();
        //    // get other values except current value... in that CTRYCode
        //    vm.lkpCTRYCategories = _context.Country.Where(c => c.AppGlobalOwnerId == oCurrCTRY.AppGlobalOwnerId && c.ChurchBodyId == oCurrCTRY.AppGlobalOwnerId &&
        //                                                            c.CTRYCode == oCurrCTRY.CTRYCode && c.Id != oCurrCTRY.Id)
        //                                     .OrderBy(c => c.OrderIndex)
        //                                     .ThenBy(c => c.CTRYValue)
        //                                     .Select(c => new SelectListItem()
        //                                     {
        //                                         Value = c.Id.ToString(),
        //                                         Text = c.CTRYValue
        //                                     })
        //                                     .ToList();
        //    //vm.lkpCTRYCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });


        //    return vm;
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AddOrEdit_CTRY(CountryModel vm)
        //{
        //    if (!InitializeUserLogging())
        //        return RedirectToAction("LoginUserAcc", "UserLogin");

        //    var strDesc = "App Lookup Parameter";
        //    if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });
        //    if (vm.oCountry == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });

        //    Country _oChanges = vm.oCountry;  // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();

        //    if (string.IsNullOrEmpty(_oChanges.CTRYCode))
        //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parameter code for " + strDesc.ToLower() + " must be specified" });

        //    if (string.IsNullOrEmpty(_oChanges.CTRYValue))
        //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parameter value for " + strDesc.ToLower() + " must be specified else delete parameter" });

        //    // initial validations done!
        //    var strTag = GetCTRYTagDesc(_oChanges.CTRYCode);

        //    var arrData = "";
        //    arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
        //    var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<CountryModel>(arrData) : vm;

        //    var oCTRY = vmMod.oCountry;
        //    // oCTRY.ChurchBody = vmMod.oChurchBody;

        //    try
        //    {
        //        ModelState.Remove("oCountry.AppGlobalOwnerId");
        //        ModelState.Remove("oCountry.ChurchBodyId");
        //        ModelState.Remove("oCountry.CTRYCategoryId");
        //        ModelState.Remove("oCountry.CTRYCode");
        //        ModelState.Remove("oCountry.CreatedByUserId");
        //        ModelState.Remove("oCountry.LastModByUserId");

        //        //finally check error state...
        //        if (ModelState.IsValid == false)
        //            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

        //        //// church logo
        //        //if (vm.ChurchLogoFile != null) //&& _oChanges.ChurchLogo != null
        //        //{
        //        //    if (_oChanges.ChurchLogo != vm.ChurchLogoFile.FileName)
        //        //    {
        //        //        string strFilename = null;
        //        //        if (vm.ChurchLogoFile != null && vm.ChurchLogoFile.Length > 0)
        //        //        {
        //        //            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img_db");
        //        //            strFilename = Guid.NewGuid().ToString() + "_" + vm.ChurchLogoFile.FileName;
        //        //            string filePath = Path.Combine(uploadFolder, strFilename);
        //        //            vm.ChurchLogoFile.CopyTo(new FileStream(filePath, FileMode.Create));
        //        //        }
        //        //        else
        //        //        {
        //        //            if (vm.oAppGlobalOwn.Id != 0) strFilename = vm.strChurchLogo;
        //        //        }

        //        //        _oChanges.ChurchLogo = strFilename;
        //        //    }
        //        //}

        //        //
        //        var tm = DateTime.Now;
        //        _oChanges.LastMod = tm;
        //        _oChanges.LastModByUserId = vm.oUserId_Logged;
        //        //_oChanges.Status = vm.blStatusActivated ? "A" : "D"; 

        //        //validate...
        //        var _userTask = "Attempted saving " + strDesc.ToLower() + ", " + strTag;  // _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
        //        var _reset = _oChanges.Id == 0;


        //        //using (var _CTRYCtx = new ChurchModelContext(_context.Database.GetDbConnection().ConnectionString))
        //        //{
        //        if (_oChanges.Id == 0)
        //        {
        //            var existCTRY = _context.Country.Where(c => c.CTRYCategoryId == _oChanges.CTRYCategoryId && c.CTRYValue.ToLower() == _oChanges.CTRYValue.ToLower()).ToList();
        //            if (existCTRY.Count() > 0)
        //                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " -- [" + (strTag + ":- " + _oChanges.CTRYValue) + "] already exist." });

        //            _oChanges.Created = tm;
        //            _oChanges.CreatedByUserId = vm.oUserId_Logged;

        //            _context.Add(_oChanges);

        //            ViewBag.UserMsg = "Saved " + strDesc.ToLower() + " [" + (!string.IsNullOrEmpty(strTag + _oChanges.CTRYValue) ? (strTag + ":- " + _oChanges.CTRYValue) : strTag + _oChanges.CTRYValue) + "] successfully.";
        //            _userTask = "Added new " + strDesc.ToLower() + ", [" + (strTag + ":- " + _oChanges.CTRYValue) + "] successfully";
        //        }

        //        else

        //        {
        //            var existCTRY = _context.Country.Where(c => c.Id != _oChanges.Id && c.CTRYCategoryId == _oChanges.CTRYCategoryId && c.CTRYValue.ToLower() == _oChanges.CTRYValue.ToLower()).ToList();
        //            if (existCTRY.Count() > 0)
        //                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " -- [" + (strTag + ":- " + _oChanges.CTRYValue) + "] already exist." });

        //            _context.Update(_oChanges);

        //            ViewBag.UserMsg = strDesc + " updated successfully.";
        //            _userTask = "Updated " + strDesc.ToLower() + ", [" + (strTag + ":- " + _oChanges.CTRYValue) + "] successfully";
        //        }

        //        //save denomination first... 
        //        _context.SaveChanges(); // await _CTRYCtx.SaveChangesAsync();

        //        //   DetachAllEntities(_CTRYCtx);
        //        //}


        //        var _tm = DateTime.Now;
        //        _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, _oLoggedCB.Id, "N",
        //                             "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, _oLoggedUser.Id, tm, tm, _oLoggedUser.Id, _oLoggedUser.Id)
        //                            , this._strClientConn);

        //        var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
        //        TempData["oVmCurr"] = _vmMod; TempData.Keep();

        //        return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg });
        //    }

        //    catch (Exception ex)
        //    {
        //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving app lookup parameter details. Err: " + ex.Message });
        //    }

        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEditBLK_CTRY(CountryModel vm, IFormCollection f) //ChurchMemAttendList oList)      // public IActionResult Index_Attendees(ChurchMemAttendList oList) //List<ChurchMember> oList)  //, int? reqChurchBodyId = null, string strAttendee="M", string strLongevity="C" ) //, char? filterIndex = null, int? filterVal = null)
        {
            var strDesc = "Country";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = 2 });
            if (vm.lsCountryModels == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = "No changes made to " + strDesc + " data.", pageIndex = vm.pageIndex });
            if (vm.lsCountryModels.Count == 0) return Json(new { taskSuccess = false, oCurrId = "", userMess = "No changes made to " + strDesc + " data.", pageIndex = vm.pageIndex });

            //    if (vm.oClientAppServerConfig == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });

            //  ClientAppServerConfig _oChanges = vm.oClientAppServerConfig;  // vm = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vm; TempData.Keep();

            //check if the configured levels <= total levels under AppGloOwn 
            //var oVal = f["oAppGloOwnId"].ToString();
            //var oAGOId = !string.IsNullOrEmpty(oVal) ? int.Parse(oVal) : (int?)null;
            //var countCL = _context.ClientAppServerConfig.Count(c => c.AppGlobalOwnerId == oAGOId);
            //var oAGO = _context.MSTRAppGlobalOwner.Find(oAGOId);
            //if (oAGO == null)
            //    return Json(new { taskSuccess = false, oCurrId = -1, userMess = "Specify denomination (church)" });

            //if (countCL > oAGO.TotalLevels)
            //    return Json(new { taskSuccess = false, oCurrId = -1, userMess = "Total " + strDesc.ToLower() + "s allowed for denomination, " + oAGO.OwnerName + " [" + oAGO.TotalLevels + "] exceeded.", pageIndex = vm.pageIndex });


            // return View(vm);
            if (ModelState.IsValid == false)
                return Json(new { taskSuccess = false, oCurrId = "", userMess = "Saving data failed. Please refresh and try again", pageIndex = vm.pageIndex });

            //if (vm == null)
            //    return Json(new { taskSuccess = false, userMess = "Data to update not found. Please refresh and try again", pageIndex = vm.pageIndex });

            //if (vm.lsCountryModels == null)
            //    return Json(new { taskSuccess = false, userMess = "No changes made to attendance data.", pageIndex = vm.pageIndex });

            //if (vm.lsCountryModels.Count == 0)
            //    return Json(new { taskSuccess = false, userMess = "No changes made to attendance data", pageIndex = vm.pageIndex });



            //if ((_oChanges.Id == 0 && countCL >= oAGO.TotalLevels) || (_oChanges.Id > 0 && countCL > oAGO.TotalLevels))
            //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Total " + strDesc.ToLower() + "s allowed for denomination, " + oAGO.OwnerName + " [" + oAGO.TotalLevels + "] reached." });

            //if (_oChanges.LevelIndex <= 0 || _oChanges.LevelIndex > oAGO.TotalLevels)
            //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please indicate correct level index. Hint: Must be within total Countrys [" + oAGO.TotalLevels + "]" });

            //if (string.IsNullOrEmpty(_oChanges.Name))
            //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the " + strDesc.ToLower() + " name" });



            //get the global var
            // var oCbId = f["_hdnAppGloOwnId"].ToString();
            //var oCbId = f["oChurchBodyId"].ToString();
            //var oDt = f["m_DateAttended"].ToString();
            //var oEv = f["m_ChurchEventId"].ToString();

            // if (oCbId == null)
            //   return Json(new { taskSuccess = false, oCurrId = -1, userMess = "Denomination (church) is required. Please specify denomination.", currTask = vmMod.currAttendTask, oCurrId = -1, evtId = -1, evtDate = -1 });

            //var oCBId = int.Parse(oCbId);
            //var dtEv = DateTime.Parse(oDt);
            //var oEvId = int.Parse(oEv);


            if (vm.oAppGloOwnId_Logged == null)
                return Json(new { taskSuccess = false, oCurrId = vm.oAppGloOwnId_Logged, userMess = "Specify denomination (church) to configure", pageIndex = vm.pageIndex });
            //var oAGO = _context.AppGlobalOwner.Find(vm.oAppGloOwnId_Logged);
            //var strAGO = oAGO != null ? oAGO.OwnerName : "";

            if (vm.oChurchBodyId_Logged == null)
                return Json(new { taskSuccess = false, oCurrId = vm.oChurchBodyId_Logged, userMess = "Specify church unit to configure", pageIndex = vm.pageIndex });
            //var oCB = _context.ChurchBody.Find(vm.oChurchBodyId_Logged);
            //var strCB = oCB != null ? oCB.Name : "";


            foreach (var d in vm.lsCountryModels)
            {
                if (d.oCountry == null)
                    return Json(new { taskSuccess = false, oCurrId = (int?)null, userMess = "Choose the country to custom", pageIndex = vm.pageIndex });
                 
                //if (d.oCountry != null)
                //{
                //    var _oChanges = d.oCountry;
                //    //if (_oChanges.AppGlobalOwnerId == null)
                //    //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify denomination (church) to configure", pageIndex = vm.pageIndex });

                //    //var oAGO = _context.MSTRAppGlobalOwner.Find(_oChanges.AppGlobalOwnerId);
                //    //var strAGO = oAGO != null ? oAGO.OwnerName : "";

                //    //if (_oChanges.oCountry == null)
                //    //    return Json(new { taskSuccess = false, oCurrId = (int?)null, userMess = "Choose the country to custom", pageIndex = vm.pageIndex });

                //    //if (string.IsNullOrEmpty(_oChanges.DbaseName))
                //    //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the database name" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : ""), pageIndex = vm.pageIndex });

                //    //// oCTRYModel.oCountry.DbaseName = "DBRCMS_CL_" + oAppOwn.Acronym.ToUpper(); //check uniqueness
                //    //if (_oChanges.DbaseName.StartsWith("DBRCMS_CL_") == false)
                //    //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name must begin with 'DBRCMS_CL_'" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : ""), pageIndex = vm.pageIndex });

                //    //if (string.IsNullOrEmpty(_oChanges.SvrUserId))
                //    //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the server user" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : ""), pageIndex = vm.pageIndex });

                //    //if (string.IsNullOrEmpty(_oChanges.SvrPassword))
                //    //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the server password" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : ""), pageIndex = vm.pageIndex });


                //    //if (d.oCountry.Id > 0)  // update
                //    //{
                //    //    var existCTRY = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.Id != _oChanges.Id && c.Status == "A").FirstOrDefault();
                //    //    if (existCTRY != null)
                //    //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Country already exist" + (existCTRY.AppGlobalOwner != null ? ". Denomination (Church): " + existCTRY.AppGlobalOwner.OwnerName : ""), pageIndex = vm.pageIndex });


                //    //    var oCTRY_Db = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.Id != _oChanges.Id && c.DbaseName.ToLower().Equals(_oChanges.DbaseName.ToLower())).FirstOrDefault();
                //    //    if (oCTRY_Db != null)
                //    //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name already used for another denomination" + (oCTRY_Db.AppGlobalOwner != null ? ", " + oCTRY_Db.AppGlobalOwner.OwnerName : ""), pageIndex = vm.pageIndex });

                //    //}

                //    //else if (d.oCountry.Id == 0)  //add
                //    //{
                //    //    var existCTRY = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.Status == "A").FirstOrDefault();
                //    //    if (existCTRY != null)
                //    //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Country already exist" + (existCTRY.AppGlobalOwner != null ? ". Denomination (Church): " + existCTRY.AppGlobalOwner.OwnerName : ""), pageIndex = vm.pageIndex });

                //    //    var oCTRY_Db = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.DbaseName.ToLower().Equals(_oChanges.DbaseName.ToLower())).FirstOrDefault();
                //    //    if (oCTRY_Db != null)
                //    //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name already used for another denomination" + (oCTRY_Db.AppGlobalOwner != null ? ", " + oCTRY_Db.AppGlobalOwner.OwnerName : ""), pageIndex = vm.pageIndex });

                //    //}
                //}
            }


            // all clear.....  get the CountryCustom
            var lsCTRY_CUST = _context.CountryCustom.Where(c => c.AppGlobalOwnerId == vm.oAppGloOwnId_Logged && c.ChurchBodyId == vm.oChurchBodyId_Logged).ToList();


            //using (var _ctryCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
            //{
            var oCTRY_CntAdd = 0; var oCTRY_CntUpd = 0;
                foreach (var d in vm.lsCountryModels)
                {
                    if (d.oCountry != null  && d.isCustomDisplay == true)
                    {
                        var oCTRY_CUST = lsCTRY_CUST.Where(c => c.CtryAlpha3Code == d.oCountry.CtryAlpha3Code).FirstOrDefault();
                        if (oCTRY_CUST != null) // update
                        {
                            oCTRY_CUST.AppGlobalOwnerId = vm.oAppGloOwnId_Logged;
                            oCTRY_CUST.ChurchBodyId = vm.oChurchBodyId_Logged;
                            oCTRY_CUST.CtryAlpha3Code = d.oCountry.CtryAlpha3Code;
                            oCTRY_CUST.IsDisplay = d.isCustomDisplay;
                            oCTRY_CUST.IsChurchCountry = d.isCustomChurchCountry;
                            oCTRY_CUST.IsDefaultCountry = d.isCustomDefaultCountry;  // cannot allow more than one country
                            oCTRY_CUST.LastMod = DateTime.Now;
                            //
                            oCTRY_CntUpd++;
                            _context.Update(oCTRY_CUST);
                        }
                        else  // new .. add to custom table
                        {
                            CountryCustom oCTRY_CUSTAdd = new CountryCustom()
                            {
                                AppGlobalOwnerId = vm.oAppGloOwnId_Logged,
                                ChurchBodyId = vm.oChurchBodyId_Logged,
                                CtryAlpha3Code = d.oCountry.CtryAlpha3Code,
                                IsDisplay = d.isCustomDisplay,
                                IsChurchCountry = d.isCustomChurchCountry,
                                IsDefaultCountry = d.isCustomDefaultCountry,  // cannot allow more than one country 
                                Created = DateTime.Now,
                                LastMod = DateTime.Now,
                            };

                            //
                            oCTRY_CntAdd++;
                            _context.Add(oCTRY_CUSTAdd);
                        }
                         
                    }
                }


                var _userTask = "";
                if ((oCTRY_CntAdd + oCTRY_CntUpd) > 0)
                {
                    if (oCTRY_CntAdd > 0)
                    {
                        _userTask = "Customized " + oCTRY_CntAdd + " new Countries for " + strDesc.ToLower() + " successfully.";
                        ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oCTRY_CntAdd + " Countries.";
                    }

                    if (oCTRY_CntUpd > 0)
                    {
                        _userTask = !string.IsNullOrEmpty(_userTask) ? _userTask + ". " : "" + "Updated " + oCTRY_CntUpd + " customized Countries for " + strDesc.ToLower() + " successfully.";
                        ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + oCTRY_CntUpd + " Countries updated.";
                    }

                //save all...
                _context.SaveChanges(); // await _ctryCtx.SaveChangesAsync();

                var _tm = DateTime.Now;
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                     "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, this._oLoggedUser.Id, _tm, _tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                    , this._strClientConn);

                //var _tm = DateTime.Now;
                //    _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                //                     "RCMS-Admin: Country", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));
                 
                    return Json(new { taskSuccess = true, userMess = ViewBag.UserMsg, pageIndex = vm.pageIndex });
                }

            //}

            return Json(new { taskSuccess = false, userMess = "Saving data failed. Please refresh and try again.", pageIndex = vm.pageIndex });
        }


        public IActionResult Delete_CTRY(int? oAppGloOwnId, int? oChurchBodyId, int? loggedUserId, int id, bool forceDeleteConfirm = false)
        {
            if (!InitializeUserLogging())
                return RedirectToAction("LoginUserAcc", "UserLogin");

            // var strDesc = setIndex == 1 ? "System profile" : setIndex == 2 ? "Church admin profile" : "Church user profile";
            var strDesc = "App lookup parameter";
            var tm = DateTime.Now; var _tm = DateTime.Now; var _userTask = "Attempted saving  " + strDesc;
            //

            try
            {
                var strUserDenom = "RCMS Client";

                //if (setIndex != 1)
                //{
                //    if (oAppGloOwnId == null || oChurchBodyId == null)
                //        return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Denomination/church of " + strDesc + " unknown. Please refesh and try again." });

                //    var oAGO = _context.AppGlobalOwner.Find(oAppGloOwnId);
                //    var oCB = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oChurchBodyId).FirstOrDefault();

                //    if (oAGO == null || oCB == null)
                //        return Json(new { taskSuccess = false, oCurrId = "", userMess = "Specified denomination and church unit could not be retrieved. Please refresh and try again.", signOutToLogIn = false });

                //    strUserDenom = oCB.Name + (!string.IsNullOrEmpty(oAGO.Acronym) ? ", " + oAGO.Acronym : oAGO.OwnerName);
                //    strUserDenom = "--" + (string.IsNullOrEmpty(strUserDenom) ? "Denomination: " + strUserDenom : strUserDenom);
                //}


                var oCTRY = _context.CountryCustom.Include(c => c.Country).Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId).FirstOrDefault();// 
                if (oCTRY == null)
                {
                    _userTask = "Attempted deleting " + strDesc.ToLower(); // + ", " + (strTag + ":- " + oCTRY.CTRYValue);  // var _userTask = "Attempted saving  " + strDesc;
                    _tm = DateTime.Now;
                    _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                         "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                        , this._strClientConn);

                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " to delete could not be retrieved." });
                }

                var strTag = oCTRY.Country != null ? oCTRY.Country.EngName : "[Country]";
                var saveDelete = true;

                // ensuring cascade delete where there's none!

                //check CTRYCategory for this UP to delete 
                // UNTIL actual dependencies found...
                var CTRYs = new List<string>();   // _context.CountryCustom.Where(c => c.CTRYCategoryId == oCTRY.Id).ToList();

                //using (var _userCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                //{
                if (CTRYs.Count() > 0) // + UPGs.Count() + oUPRs.Count() //+oUser.ChurchMembers.Count )
                {
                    if (forceDeleteConfirm == false)
                    {
                        var strConnTabs = "Parameter category";  //User profile role, User profile group and 
                        saveDelete = false;

                        // check user privileges to determine... administrator rights
                        //log...
                        _userTask = "Attempted deleting " + strDesc.ToLower() + ", [" + strTag + "]";
                        _tm = DateTime.Now;
                        _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                             "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                            , this._strClientConn);

                        return Json(new
                        {
                            taskSuccess = false,
                            tryForceDelete = false,
                            oCurrId = id,
                            userMess = "Specified " + strDesc.ToLower() +
                                            " to delete has been used elsewhere in the system [" + strConnTabs + "]. Delete cannot be done unless dependent-references are removed."
                        });
                    }

                    //to be executed only for higher privileges... // FORCE-DELETE...
                }

                //successful...
                if (saveDelete)
                {
                    _context.CountryCustom.Remove(oCTRY);
                    _context.SaveChanges();

                    //   DetachAllEntities(_userCtx);

                    //audit...
                    _userTask = "Deleted " + strDesc.ToLower() + ", [" + strTag + "]";
                    _tm = DateTime.Now;
                    _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                             "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                            , this._strClientConn);

                    return Json(new { taskSuccess = true, tryForceDelete = false, oCurrId = oCTRY.Id, userMess = strDesc + " successfully deleted." });
                }

                //}


                _userTask = "Attempted deleting " + strDesc.ToLower() + ", [" + strTag + "] -- but FAILED. Data unavailable.";   // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                                 "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                                , this._strClientConn);

                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "No " + strDesc.ToLower() + " data available to delete. Try again" });
            }

            catch (Exception ex)
            {
                _userTask = "Attempted deleting " + strDesc.ToLower() + ", [ ID= " + id + "] FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, this._oLoggedAGO.Id, this._oLoggedCB.Id, "N",
                                                    "RCMS-Client: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, this._oLoggedUser.Id, tm, tm, this._oLoggedUser.Id, this._oLoggedUser.Id)
                                                   , this._strClientConn);
                //
                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Failed deleting " + strDesc.ToLower() + ". Err: " + ex.Message });
            }
        }






    }
}
