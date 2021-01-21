//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.CLNTModels;
//using RhemaCMS.Models.MSTRModels;
//using RhemaCMS.Models.ViewModels;

//namespace RhemaCMS.Controllers.con_ch_fin
//{
//    public class TitheTransController : Controller
//    {
//        private readonly ChurchModelContext _context;
//        private readonly MSTR_DbContext _MSTRContext;
//        private readonly IWebHostEnvironment _hostingEnvironment;

//        private bool isCurrValid = false;
//        private List<UserSessionPrivilege> oUserLogIn_Priv = null;

//        private List<DiscreteLookup> dlShareStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlGenStatuses = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlPmntModes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlTitheModes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlTitherScopes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlPostModes = new List<DiscreteLookup>();

//        public TitheTransController(ChurchModelContext context, MSTR_DbContext ctx,
//             IWebHostEnvironment hostingEnvironment)
//        {
//            _context = context;
//            _MSTRContext = ctx;
//            _hostingEnvironment = hostingEnvironment;


//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "S", Desc = "Draft" });  //Saved Copy
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "D", Desc = "Deactive" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "E", Desc = "Expired" });

//            //SharingStatus { get; set; }  // A - Share with all sub-congregations, C - Share with child congregations only, N - Do not share
//            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "N", Desc = "Do not roll-down (share)" });
//            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "C", Desc = "Roll-down (share) for direct child congregations" });
//            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "A", Desc = "Roll-down (share) for all sub-congregations" });

            
//            dlPmntModes.Add(new DiscreteLookup() { Category = "PmntMode", Val = "CSH", Desc = "Cash" });
//            dlPmntModes.Add(new DiscreteLookup() { Category = "PmntMode", Val = "CHQ", Desc = "Cheque" });
//            dlPmntModes.Add(new DiscreteLookup() { Category = "PmntMode", Val = "MM", Desc = "Mobile Money" });
//            dlPmntModes.Add(new DiscreteLookup() { Category = "PmntMode", Val = "POS", Desc = "POS" });
//            dlPmntModes.Add(new DiscreteLookup() { Category = "PmntMode", Val = "O", Desc = "Other" });

//            // TitheMode { get; set; }   // M-Member-Specific, G-Church Group, E-External Body, A-Anonymous
//            dlTitheModes.Add(new DiscreteLookup() { Category = "TitheMode", Val = "A", Desc = "Anonymous" });
//            dlTitheModes.Add(new DiscreteLookup() { Category = "TitheMode", Val = "M", Desc = "Member-based" });
//            dlTitheModes.Add(new DiscreteLookup() { Category = "TitheMode", Val = "G", Desc = "Corporate/Group" });
//            dlTitheModes.Add(new DiscreteLookup() { Category = "TitheMode", Val = "O", Desc = "Open" });

//            // TitherScope L-Tither from local congregation, D-Tither from denomination, E-Corporate /Group 
//            dlTitherScopes.Add(new DiscreteLookup() { Category = "TitherScope", Val = "L", Desc = "Local congregation" });
//            dlTitherScopes.Add(new DiscreteLookup() { Category = "TitherScope", Val = "D", Desc = "Denomination" });
//            dlTitherScopes.Add(new DiscreteLookup() { Category = "TitherScope", Val = "E", Desc = "External" });

//            ///O-pen P-osted to GL C-ancelled
//            dlPostModes.Add(new DiscreteLookup() { Category = "PostMode", Val = "O", Desc = "Open" });
//            dlPostModes.Add(new DiscreteLookup() { Category = "PostMode", Val = "P", Desc = "Posted" });
//            dlPostModes.Add(new DiscreteLookup() { Category = "PostMode", Val = "R", Desc = "Reversed" });
//        }


//        public static string GetStatusDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "A": return "Active";
//                case "S": return "Draft";
//                case "D": return "Deactive";
//                case "P": return "Pending";
//                case "E": return "Expired";


//                default: return oCode;
//            }
//        }

//        public static string GetPostStatusDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "O": return "Open";
//                case "P": return "Posted to GL";
//                case "R": return "Revesed";
//                // case "C": return "Cancelled";


//                default: return oCode;
//            }
//        }

//        public static string GetTitheModeDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "A": return "Anonymous";
//                case "M": return "Member-based";
//                case "G": return "Corporate /Group";
//                case "O": return "Open"; //Any stream
//                // case "C": return "Cancelled";


//                default: return oCode;
//            }
//        } 
        
//        public static string GetTitherScopeDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "L": return "Local congregation";
//                case "D": return "Denomination";
//                case "E": return "External"; 


//                default: return oCode;
//            }
//        }


//        public string GetConcatMemberName(string title, string fn, string mn, string ln, bool displayName = false)
//        {
//            if (displayName)
//                return ((((!string.IsNullOrEmpty(title) ? title : "") + ' ' + fn).Trim() + " " + mn).Trim() + " " + ln).Trim();
//            else
//                return (((fn + ' ' + mn).Trim() + " " + ln).Trim() + " " + (!string.IsNullOrEmpty(title) ? "(" + title + ")" : "")).Trim();
//        }

//        public static string StaticGetConcatMemberName(string title, string fn, string mn, string ln, bool displayName = false)
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

//            if (TempData.ContainsKey("UserLogIn_oUserPrivCol"))
//            {
//                var tempPrivList = TempData["UserLogIn_oUserPrivCol"] as string;
//                if (string.IsNullOrEmpty(tempPrivList)) RedirectToAction("LoginUserAcc", "UserLogin");
//                // De serialize the string to object
//                oUserLogIn_Priv = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);

//                isCurrValid = oUserLogIn_Priv?.Count > 0;
//                if (isCurrValid)
//                {
//                    ViewBag.oAppGloOwnLogged = oUserLogIn_Priv[0].AppGlobalOwner;
//                    ViewBag.oChuBodyLogged = oUserLogIn_Priv[0].ChurchBody;
//                    ViewBag.oUserLogged = oUserLogIn_Priv[0].UserProfile;

//                    // check permission for Core life...  given the sets of permissions
//                    userAuthorized = oUserLogIn_Priv.Count > 0; //(oUserLogIn_Priv.Find(x => x.PermissionName == "_A0__System_Administration" || x.PermissionName == "xxx") != null);
//                }
//            }
//            else RedirectToAction("LoginUserAcc", "UserLogin");


//        }

//        private bool IsAncestor_ChurchBody(ChurchBody oAncestorChurchBody, ChurchBody oCurrChurchBody)
//        {
//            if (oAncestorChurchBody == null) return false;
//            //string RootChurchCode { get; set; }  //R0000-0000-0000-0000-0000-0000 
//            if (oAncestorChurchBody.Id == oCurrChurchBody.ParentChurchBodyId) return true;
//            if (string.IsNullOrEmpty(oCurrChurchBody.RootChurchCode)) return false;

//            string[] arr = new string[] { oCurrChurchBody.RootChurchCode };
//            if (oCurrChurchBody.RootChurchCode.Contains("-")) arr = oCurrChurchBody.RootChurchCode.Split('-');

//            if (arr.Length > 0)
//            {
//                var ancestorCode = oAncestorChurchBody.RootChurchCode;
//                var tempCode = oCurrChurchBody.RootChurchCode;
//                var k = arr.Length - 1;
//                for (var i = arr.Length - 1; i >= 0; i--)
//                {
//                    if (tempCode.Contains("-" + arr[i])) tempCode = tempCode.Replace("-" + arr[i], "");
//                    if (string.Compare(ancestorCode, tempCode) == 0) return true;
//                }
//            }

//            return false;
//        }

//        public JsonResult GetTitheModeByScope(string scope) //, bool addEmpty = false)
//        {
//            //var userPerms = (
//            //            from t_upr in _context.UserRolePermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userRoleId == null || (userRoleId != null && c.UserRoleId == userRoleId)))
//            //            from t_up in _context.UserPermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.PermStatus == "A" && c.Id == t_upr.UserRoleId)
//            //            select t_up
//            //                   ).OrderBy(c => c.PermissionCode).ToList()
//            //                    .Select(c => new SelectListItem()
//            //                    {
//            //                        Value = c.Id.ToString(),
//            //                        Text = c.PermissionName
//            //                    })
//            //                    .OrderBy(c => c.Text)
//            //                    .ToList();


//            var ls =  new List<SelectListItem>();
//            if (scope == "L")
//            {
//                foreach (var dl in dlTitheModes)
//                {
//                    if (dl.Val == "M" || dl.Val == "A" || dl.Val == "O") ls.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc });
//                }
//            }
//            else if (scope == "D" || scope == "E")
//            {
//                foreach (var dl in dlTitheModes)
//                {
//                    if (dl.Val == "G" || dl.Val == "A" || dl.Val == "O") ls.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc });
//                }
//            }
//            //else if (scope == "E")
//            //{
//            //    foreach (var dl in dlTitheModes)
//            //    {
//            //        if (dl.Val == "G" || dl.Val == "A" || dl.Val == "O") ls.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc });
//            //    }
//            //}

//            // if (addEmpty) ls.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json(ls);
//        }

//        public ActionResult Index(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 0) //, int? oParentId = null, int? id = null, int pageIndex = 1) //, int? oChuCategId = null, bool oShowAllCong = true) //, int? currFilterVal = null) //, ChurchBodyConfigMDL oCurrCBConfig = null)
//        {  // int subSetIndex = 0, 
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                // check permission 
//                if (!this.userAuthorized) return View(new TitheModel()); //retain view                  
//                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;  // if (oCurrChuBodyLogOn == null) return View(oCurrMdl);
//                if (oLoggedUser == null) return View(new TitheModel());
//                //
//                var oCurrMdl = new TitheModel(); //TempData.Keep();  
//                                                     // int? oAppGloOwnId = null;
//                var oChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                //
//                int? oAppGloOwnId_Logged = null;
//                int? oChuBodyId_Logged = null; 
//                if (oChuBodyLogOn != null)
//                {
//                    oAppGloOwnId_Logged = oChuBodyLogOn.AppGlobalOwnerId;
//                    oChuBodyId_Logged = oChuBodyLogOn.Id;
//                   // oUserId_Logged = oTitheTrans.Id;

//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBodyLogOn.Id; }
//                    if (oAppGloOwnId == null) { oAppGloOwnId = oChuBodyLogOn.AppGlobalOwnerId; }
//                    //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
//                    //
//                    // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                }

//                var oUserId_Logged = oLoggedUser.Id;
//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//                var currChurchMemberLogged = _context.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oLoggedUser.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCurrMdl);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }


//                var lsTitheMdl = (
//                   from t_tt in _context.TitheTrans.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_tt.ChurchBodyId && c.AppGlobalOwnerId == t_tt.AppGlobalOwnerId)   //c.Id == oChurchBodyId && 
//                   from t_ap in _context.AccountPeriod.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_tt.AccountPeriodId)   //c.Id == oChurchBodyId && 
//                                                                                                                                                                             // from t_an in _context.AppUtilityNVP.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_tt.TitheModeId).DefaultIfEmpty()
//                   from t_cb_corp in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_tt.Corporate_ChurchBodyId && c.AppGlobalOwnerId == t_tt.AppGlobalOwnerId).DefaultIfEmpty()
//                   from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_tt.ChurchMemberId).DefaultIfEmpty()
//                   from t_curr in _context.Currency.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_tt.CurrencyId).DefaultIfEmpty()

//                   select new TitheModel()
//                   {
//                       oTitheTrans = t_tt,
//                       oAppGlolOwnId = t_tt.AppGlobalOwnerId,
//                       oAppGlolOwn = t_tt.AppGlobalOwner,
//                       oChurchBodyId = t_tt.ChurchBodyId,
//                       oChurchBody = t_tt.ChurchBody,
//                       //                       
//                       strTithedBy = t_tt.TitheMode == "M" ? (t_cm != null ? StaticGetConcatMemberName(t_cm.Title, t_cm.FirstName, t_cm.MiddleName, t_cm.LastName, true) : t_tt.TitherDesc) : 
//                                                                    (t_tt.TitheMode=="C" ? (t_cb_corp != null ? t_cb_corp.Name : t_tt.TitherDesc) : t_tt.TitherDesc),
//                       strTitheMode = GetTitheModeDesc (t_tt.TitheMode),  
//                       strTitherScope = GetTitherScopeDesc(t_tt.TithedByScope),  
//                      // strRelatedEvent = t_cce != null ? t_cce.Subject : "",
//                       strAccountPeriod = t_ap != null ? t_ap.PeriodDesc : "",
//                       strCurrency = t_curr != null ? t_curr.Acronym : "",
                            
//                       //
//                       strAmount = String.Format("{0:N2}", t_tt.TitheAmount),
//                       dt_TitheDate = t_tt.TitheDate,
//                       strTitheDate = t_tt.TitheDate != null ? DateTime.Parse(t_tt.TitheDate.ToString()).ToString("ddd, dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",
//                       strPostDate = t_tt.PostedDate != null ? DateTime.Parse(t_tt.PostedDate.ToString()).ToString("ddd, dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",
//                       strPostStatus = GetPostStatusDesc(t_tt.PostStatus) 
//                   }
//                   )
//                   .OrderByDescending(c => c.dt_TitheDate)
//                   .ToList();


//                oCurrMdl.lsTitheModel = lsTitheMdl;

//                //
//                oCurrMdl.oAppGlolOwnId = oAppGloOwnId;
//                oCurrMdl.oChurchBodyId = oCurrChuBodyId;
//                //
//                oCurrMdl.oUserId_Logged = oUserId_Logged;
//                oCurrMdl.oChurchBodyId_Logged = oChuBodyId_Logged;
//                oCurrMdl.oAppGloOwnId_Logged = oAppGloOwnId_Logged;
//                oCurrMdl.oMemberId_Logged = oCurrChuMemberId_LogOn;
//                //
//                oCurrMdl.setIndex = setIndex;
//                //       

//                // oCurrMdl.strChurchLevelDown = "Assemblies";
//                oCurrMdl.strAppName = "RhemaCMS"; ViewBag.strAppName = oCurrMdl.strAppName;
//                oCurrMdl.strAppNameMod = "Admin Palette"; ViewBag.strAppNameMod = oCurrMdl.strAppNameMod;
//                oCurrMdl.strAppCurrUser = "Dan Abrokwa"; ViewBag.strAppCurrUser = oCurrMdl.strAppCurrUser;
//                // oHomeDash.strChurchType = "CH"; ViewBag.strChurchType = oHomeDash.strChurchType;
//                // oHomeDash.strChuBodyDenomLogged = "Rhema Global Church"; ViewBag.strChuBodyDenomLogged = oHomeDash.strChuBodyDenomLogged;
//                //  oHomeDash.strChuBodyLogged = "Rhema Comm Chapel"; ViewBag.strChuBodyLogged = oHomeDash.strChuBodyLogged; 
//                //           
//                ViewBag.strAppCurrUser_ChRole = "System Adminitrator";
//                ViewBag.strAppCurrUser_RoleCateg = "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
//                ViewBag.strAppCurrUser_PhotoFilename = "2020_dev_sam.jpg";
//                // ViewBag.strAppCurrChu_LogoFilename = "14dc86a7-81ae-462c-b73e-4581bd4ee2b2_church-of-pentecost.png";
//                ViewBag.strUserSessionDura = "Logged: 10 minutes ago";

//                //
//                oCurrMdl.strCurrTask = "Church Tithes";

//                // TempData.Put("oVmCB_CNFG", oCurrMdl);
//                TempData.Keep();
//                return View(oCurrMdl);
//            }
//        }


//        [HttpGet]
//        public IActionResult AddOrEdit_TITHE(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, 
//                                                int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) //, int pageIndex = 1)
//        {   //int subSetIndex = 0,

//            var oCurrMdl = new TitheModel(); TempData.Keep();
//            if (setIndex == 0) return PartialView("_AddOrEdit_TITHE", oCurrMdl);

            
//            if (id == 0)
//            {
//                var oTransTITHE = new TitheTrans();
                
               
//                oTransTITHE.AppGlobalOwnerId = oAppGloOwnId;
//                oTransTITHE.ChurchBodyId = oCurrChuBodyId;
//                var accPer = _context.AccountPeriod.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.PeriodStatus == "O" && c.LongevityStatus == "C").FirstOrDefault();
//                oTransTITHE.AccountPeriodId = accPer != null ? accPer.Id : (int?)null;
//                var curr = _context.Currency.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.IsBaseCurrency == true).FirstOrDefault();
//                oTransTITHE.CurrencyId = curr != null ? curr.Id : (int?)null;
//                oTransTITHE.TithedByScope = "L";     // Local cong            
//                oTransTITHE.TitheMode = "A";     //  Anonymous           
//                oTransTITHE.PostStatus = "O";     // O-Open, P-Posted to GL, R-Reversed            
//                oTransTITHE.Status = "A"; //Active
//                oTransTITHE.TitheDate = DateTime.Now;
//                oTransTITHE.ReceivedByUserId = oUserId_Logged;

//                oCurrMdl.strCurrency = curr != null ? curr.Acronym : "";
//                oCurrMdl.strPostStatus = GetPostStatusDesc(oTransTITHE.PostStatus);
//                if (oTransTITHE != null)
//                {
//                    var oUser = _MSTRContext.UserProfile.Where(c => c.Id == oTransTITHE.ReceivedByUserId).FirstOrDefault();
//                    if (oUser != null) oCurrMdl.strReceivedBy = oUser.UserDesc;
//                }

//                oCurrMdl.oTitheTrans = oTransTITHE;
//            }

//            else
//            {
//                var TitheMdl = (
//                   from t_tt in _context.TitheTrans.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_tt.ChurchBodyId && c.AppGlobalOwnerId == t_tt.AppGlobalOwnerId)   //c.Id == oChurchBodyId && 
//                   from t_ap in _context.AccountPeriod.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_tt.AccountPeriodId)   //c.Id == oChurchBodyId && 
//                                                                                                                                                                             // from t_an in _context.AppUtilityNVP.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_tt.TitheModeId).DefaultIfEmpty()
//                   from t_cb_corp in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_tt.Corporate_ChurchBodyId && c.AppGlobalOwnerId == t_tt.AppGlobalOwnerId).DefaultIfEmpty()
//                   from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_tt.ChurchMemberId).DefaultIfEmpty()
//                   from t_curr in _context.Currency.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_tt.CurrencyId).DefaultIfEmpty()

//                   select new TitheModel()
//                   {
//                       oTitheTrans = t_tt,
//                       oAppGlolOwnId = t_tt.AppGlobalOwnerId,
//                       oAppGlolOwn = t_tt.AppGlobalOwner,
//                       oChurchBodyId = t_tt.ChurchBodyId,
//                       oChurchBody = t_tt.ChurchBody,
//                       //                      
//                       strTithedBy = t_tt.TitheMode == "M" ? (t_cm != null ? StaticGetConcatMemberName(t_cm.Title, t_cm.FirstName, t_cm.MiddleName, t_cm.LastName, true) : t_tt.TitherDesc) :
//                                                                    (t_tt.TitheMode == "C" ? (t_cb_corp != null ? t_cb_corp.Name : t_tt.TitherDesc) : t_tt.TitherDesc),
//                       strTitheMode = GetTitheModeDesc(t_tt.TitheMode),
//                       strTitherScope = GetTitherScopeDesc(t_tt.TithedByScope),
//                       // strRelatedEvent = t_cce != null ? t_cce.Subject : "",
//                       strAccountPeriod = t_ap != null ? t_ap.PeriodDesc : "",
//                       strCurrency = t_curr != null ? t_curr.Acronym : "",

//                       //
//                       strAmount = String.Format("{0:N2}", t_tt.TitheAmount),
//                       dt_TitheDate = t_tt.TitheDate,
//                       strTitheDate = t_tt.TitheDate != null ? DateTime.Parse(t_tt.TitheDate.ToString()).ToString("ddd, dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",
//                       strPostDate = t_tt.PostedDate != null ? DateTime.Parse(t_tt.PostedDate.ToString()).ToString("ddd, dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",
//                       strPostStatus = GetPostStatusDesc(t_tt.PostStatus)
//                   }
//                   ).FirstOrDefault();

//                if (TitheMdl != null)
//                    if (TitheMdl.oTitheTrans != null)
//                    {
//                        var oUser = _MSTRContext.UserProfile.Where(c => c.Id == TitheMdl.oTitheTrans.ReceivedByUserId).FirstOrDefault();
//                        if (oUser != null) TitheMdl.strReceivedBy = oUser.UserDesc;
//                    }

//                oCurrMdl = TitheMdl;
//            }

//            oCurrMdl.setIndex = setIndex;
//            oCurrMdl.oUserId_Logged = oUserId_Logged;
//            oCurrMdl.oAppGlolOwnId_Logged = oAGOId_Logged;
//            oCurrMdl.oChurchBodyId_Logged = oCBId_Logged;
//            //
//            oCurrMdl.oAppGlolOwnId = oAppGloOwnId;
//            oCurrMdl.oChurchBodyId = oCurrChuBodyId;
//            var oCurrChuBody = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
//            oCurrMdl.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null;


//           // ChurchBody oChurchBody = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
//            oCurrMdl = this.populateLookups_TITHE(oCurrMdl, oCurrChuBody);
//            // oCurrMdl.strCurrTask = "Church Tithe";

//            var _oCurrMdl = Newtonsoft.Json.JsonConvert.SerializeObject(oCurrMdl);
//            TempData["oVmCurrMod"] = _oCurrMdl; TempData.Keep();

//            return PartialView("_AddOrEdit_TITHE", oCurrMdl);
//        }

//        private TitheModel populateLookups_TITHE(TitheModel vmLkp, ChurchBody oCurrChuBody)
//        {
//            if (vmLkp == null || oCurrChuBody == null) return vmLkp;
//            //
//            vmLkp.lkpStatuses = new List<SelectListItem>();
//            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpPaymentModes = new List<SelectListItem>();
//            foreach (var dl in dlPmntModes) { vmLkp.lkpPaymentModes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }  
            
//            vmLkp.lkpTitheModes = new List<SelectListItem>();
//            foreach (var dl in dlTitheModes) { vmLkp.lkpTitheModes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }
             
//            vmLkp.lkpTitherScopes = new List<SelectListItem>();
//            foreach (var dl in dlTitherScopes) { vmLkp.lkpTitherScopes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }


//            //vmLkp.lkpTitheModes = _context.AppUtilityNVP.Where(c => c.NvpCode == "TITHE_TYP")
//            //           .OrderBy(c => c.RequireUserCustom).ThenBy(c => c.OrderIndex).ThenBy(c => c.NvpValue)
//            //           .ToList()
//            //           .Select(c => new SelectListItem()
//            //           {
//            //               Value = c.Id.ToString(),
//            //               Text = c.NvpValue
//            //           })
//            //           // .OrderBy(c => c.Text)
//            //           .ToList();
//            //vmLkp.lkpTitheModes.Insert(0, new SelectListItem { Value = "", Text = "Select" });


//            var chMemList = (from t_cm in _context.ChurchMember //.Where(c => c.ChurchBodyId == oCurrChuBody.Id && c.MemberClass == "C" && c.IsActivated == true)
//                             //from t_ms in _context.MemberStatus.Where(c => c.ChurchBodyId == oCurrChuBody.Id && c.ChurchMemberId == t_cm.Id &&  //.Include(t => t.ChurchMemStatus)
//                             //                                           c.IsCurrent == true && c.ChurchMemStatus.Available == true && c.ChurchMemStatus.Deceased == false)
//                             select new ChurchMember
//                             {
//                                 Id = t_cm.Id,
//                                 //FirstName = t_cm.FirstName,
//                                 //MiddleName = t_cm.MiddleName ,
//                                 //LastName = t_cm.LastName,
//                                 MemberGlobalId = t_cm.MemberGlobalId,
//                                 strMemberName = t_cm != null ? StaticGetConcatMemberName(t_cm.Title, t_cm.FirstName, t_cm.MiddleName, t_cm.LastName, false) : "",
//                                 //(((t_cm.FirstName + ' ' + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() + " " + (!string.IsNullOrEmpty(t_cm.Title) ? "(" + t_cm.Title + ")" : "")).Trim()
//                             })

//                            // .OrderBy(c => c.strMemberName) //((c.FirstName + ' ' + c.MiddleName).Trim() + " " + c.LastName).Trim()) //strMemberName
//                             .ToList();

//          //  var chMemList = _context.ChurchMember.ToList();
//            vmLkp.lkpChurchMembers_Local = chMemList
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = (c.MemberGlobalId + ' ' + c.strMemberName).Trim()
//                                           })
//                                           // .OrderBy(c => c.Text)
//                                           .ToList();
//            vmLkp.lkpChurchMembers_Local.Insert(0, new SelectListItem { Value = "", Text = "Select" });


//            //var attnDate = DateTime.Now;
//            //var dtv = attnDate != null ? ((DateTime)attnDate).Date : (DateTime?)null;
//            //vmLkp.lkpChurchEvents = _context.ChurchCalendarEvent.Include(t => t.ChurchBody)  //.Include(t => t.RelatedChurchLifeActivity)
//            //    .Where(c => !string.IsNullOrEmpty(c.Subject) && c.EventActive == true && c.ChurchBody.AppGlobalOwnerId == oCurrChuBody.AppGlobalOwnerId //&&   //c.ChurchBody.CountryId == oCurrChuBody.CountryId && 
//            //                                                                                                                                            //((DateTime)c.EventFrom.Value).Date == dtv &&

//            //        //(c.EventFrom.HasValue ? ((DateTime)c.EventFrom.Value).Date==dts : c.EventFrom==dts) &&
//            //        //((oAttendVM.m_DateAttended.HasValue==false && c.EventFrom == null) || 
//            //        //   (oAttendVM.m_DateAttended.HasValue==true && ((DateTime)c.EventFrom).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) == dt)) &&
//            //        //(c.OwnedByChurchBodyId == oCurrChuBody.Id ||
//            //        //(c.OwnedByChurchBodyId != oCurrChuBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oCurrChuBody.ParentChurchBodyId) ||
//            //        //(c.OwnedByChurchBodyId != oCurrChuBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oCurrChuBody)))
//            //        )
//            //         .OrderByDescending(c => c.EventFrom).ThenByDescending(c => c.EventTo)
//            //         .ToList()
//            //                 .Select(c => new SelectListItem()
//            //                 {
//            //                     Value = c.Id.ToString(),
//            //                     Text = c.Subject + ":- " +
//            //                                         (c.IsFullDay == true ?
//            //                                             (c.EventFrom != null ? DateTime.Parse(c.EventFrom.ToString()).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "").Trim() :
//            //                                             (c.EventFrom != null ? DateTime.Parse(c.EventFrom.ToString()).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "").Trim() +
//            //                                             (c.EventFrom != null && c.EventTo != null ? " -- " : "") +
//            //                                             (c.EventTo != null ? DateTime.Parse(c.EventTo.ToString()).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "").Trim()
//            //                                          )
//            //                 })
//            //                 //.OrderBy(c => c.Text)
//            //                 .ToList();

//            //vmLkp.lkpChurchEvents.Insert(0, new SelectListItem { Value = "", Text = "Select" });


//            vmLkp.lkpCurrencies = _context.Currency.OrderBy(c => c.ChurchBodyId == oCurrChuBody.Id && c.Status == "A").ToList()
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.Acronym
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();
//            // vmLkp.lkpCurrencies.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//            // ... AccessStatus = Open (O), Blocked (B), Closed (C)  .. Longevity = Current (C), Previous (P), History (H)
//            vmLkp.lkpAccountPeriods = _context.AccountPeriod.OrderBy(c => c.ChurchBodyId == oCurrChuBody.Id && c.PeriodStatus == "O")
//                                            .OrderBy(c=>c.PeriodIndex)
//                                            .ToList()
//                                          .Select(c => new SelectListItem()
//                                          {
//                                              Value = c.Id.ToString(),
//                                              Text = c.PeriodDesc,
//                                              Selected = c.LongevityStatus == "C"  //Current
//                                          })
//                                          //.OrderBy(c => c.Text)
//                                          .ToList();
//            //  vmLkp.lkpAccountPeriods.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//            return vmLkp;
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_TITHE(TitheModel vmMod)
//        {
//            TitheTrans _oChanges = vmMod.oTitheTrans;   //  vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as TitheModel : vmMod; TempData.Keep();
            
//            var arrData = ""; // TempData["oVmCurrMod"] as string;
//            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
//            vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<TitheModel>(arrData) : vmMod;
//            //
//            var oMdlData = vmMod.oTitheTrans;
//            oMdlData.ChurchBody = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oTitheTrans.AppGlobalOwnerId");
//                ModelState.Remove("oTitheTrans.ChurchBodyId");
//                ModelState.Remove("oTitheTrans.ChurchMemberId");
//                ModelState.Remove("oTitheTrans.Corporate_ChurchBodyId");
//                ModelState.Remove("oTitheTrans.TitheModeId");
//                ModelState.Remove("oTitheTrans.RelatedEventId");
//                ModelState.Remove("oTitheTrans.AccountPeriodId");
//                ModelState.Remove("oTitheTrans.CurrencyId");

//                ModelState.Remove("oTitheTrans.CreatedByUserId");
//                ModelState.Remove("oTitheTrans.LastModByUserId");
//                ModelState.Remove("oUserId_Logged");

//                // ChurchBody == null 

//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

//                if (_oChanges.TitheAmount == null || _oChanges.TitheAmount == 0) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Enter the tithe amount" });
//                }
//                if (_oChanges.CurrencyId == null) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please select currency." });
//                }
//                if (_oChanges.AccountPeriodId == null) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please select account period." });
//                }
//                if (_oChanges.TitheMode == "M" && _oChanges.ChurchMemberId == null)  // 
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please specify member paying tithe else change the tithe mode [hint: try choosing 'Anonymous']." });
//                }
//                if ((_oChanges.TithedByScope == "D" && _oChanges.TitheMode == "G" && _oChanges.Corporate_ChurchBodyId == null) ||
//                    (_oChanges.TithedByScope == "E" && _oChanges.TitheMode == "G" && string.IsNullOrEmpty(_oChanges.TitherDesc)))
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please specify group or corporate body paying tithe else change the tithe mode [hint: try choosing 'Anonymous']." });
//                }
//                if (_oChanges.TitheMode == "O" && string.IsNullOrEmpty(_oChanges.TitherDesc))  // 
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please specify tither else change the tithe mode [hint: try choosing 'Anonymous']." });
//                }
//                if (_oChanges.TitheDate == null)  //Congregant... ChurchCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the date." });
//                }

//                if (_oChanges.TitheDate != null)
//                {
//                    if (_oChanges.TitheDate.Value > DateTime.Now.Date)
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Select the correct date. Date cannot be later than today." });

//                    if (_oChanges.PostedDate != null)
//                        if (_oChanges.TitheDate.Value > _oChanges.PostedDate.Value)
//                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Payment date cannot be later than posting date" });
//                }

//                var _reset = _oChanges.Id == 0;

//                //   _oChanges.LastMod = DateTime.Now;
//                //  _oChanges.LastModByUserId = vmMod.oCurrUserId_Logged;

//                //save for documents...
//                // string uniqueFileName = null;
//                //var oFormFile = vmMod.UserPhotoFile;
//                //if (oFormFile != null && oFormFile.Length > 0)
//                //{
//                //    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img_db");  //~/frontend/dist/img_db
//                //    uniqueFileName = Guid.NewGuid().ToString() + "_" + oFormFile.FileName;
//                //    string filePath = Path.Combine(uploadFolder, uniqueFileName);
//                //    oFormFile.CopyTo(new FileStream(filePath, FileMode.Create));
//                //}

//                //else
//                //    if (_oChanges.Id != 0) uniqueFileName = _oChanges.UserPhoto;

//                //_oChanges.UserPhoto = uniqueFileName;



//                var tm = DateTime.Now;
//                _oChanges.LastMod = tm;
//                _oChanges.LastModByUserId = vmMod.oUserId_Logged;
//                _oChanges.ReceivedByUserId = vmMod.oUserId_Logged;

//                //validate...
//                if (_oChanges.Id == 0)
//                {
//                    _oChanges.Created = tm;
//                    _oChanges.CreatedByUserId = vmMod.oUserId_Logged;
//                    _context.Add(_oChanges);

//                    ViewBag.UserMsg = "Saved tithe data successfully.";
//                }
//                else
//                {
//                    //retain the pwd details... hidden fields 
//                    _context.Update(_oChanges);
//                    ViewBag.UserMsg = "User tithe data updated successfully.";
//                }

//                //save user profile first... 
//                await _context.SaveChangesAsync();


//                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
//                TempData["oVmCurrMod"] = _vmMod; TempData.Keep(); 

//                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg });
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving tithe details. Err: " + ex.Message });
//            }

//        }

//    }
//}
