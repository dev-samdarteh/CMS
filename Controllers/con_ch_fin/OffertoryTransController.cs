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
//    public class OffertoryTransController : Controller
//    {
//        private readonly ChurchModelContext _context;
//        private readonly MSTR_DbContext _MSTRContext;
//        private readonly IWebHostEnvironment _hostingEnvironment;

//        private bool isCurrValid = false;
//        private List<UserSessionPrivilege> oUserLogIn_Priv = null;

//        private List<DiscreteLookup> dlShareStatus = new List<DiscreteLookup>(); 
//        private List<DiscreteLookup> dlGenStatuses = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlPmntModes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlPostModes = new List<DiscreteLookup>();

//        public OffertoryTransController(ChurchModelContext context, MSTR_DbContext ctx,
//             IWebHostEnvironment hostingEnvironment)
//        {
//            _context = context;
//            _MSTRContext = ctx;
//            _hostingEnvironment = hostingEnvironment;


//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
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
//               // case "C": return "Cancelled";


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


//        public ActionResult Index(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 0 ) //, int? oParentId = null, int? id = null, int pageIndex = 1) //, int? oChuCategId = null, bool oShowAllCong = true) //, int? currFilterVal = null) //, ChurchBodyConfigMDL oCurrCBConfig = null)
//        {  // int subSetIndex = 0, 
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                // check permission 
//                if (!this.userAuthorized) return View(new OffertoryModel()); //retain view                  
//                var oOffertoryTrans = oUserLogIn_Priv[0].UserProfile;  // if (oCurrChuBodyLogOn == null) return View(oCurrMdl);
//                if (oOffertoryTrans == null) return View(new OffertoryModel());
//                //
//                var oCurrMdl = new OffertoryModel(); //TempData.Keep();  
//                                                    // int? oAppGloOwnId = null;
//                var oChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                //
//                int? oAppGloOwnId_Logged = null;
//                int? oChuBodyId_Logged = null;
//                int? oUserId_Logged = null;
//                if (oChuBodyLogOn != null)
//                {
//                    oAppGloOwnId_Logged = oChuBodyLogOn.AppGlobalOwnerId;
//                    oChuBodyId_Logged = oChuBodyLogOn.Id;
//                    oUserId_Logged = oOffertoryTrans.Id;

//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBodyLogOn.Id; }
//                    if (oAppGloOwnId == null) { oAppGloOwnId = oChuBodyLogOn.AppGlobalOwnerId; }
//                    //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
//                    //
//                    // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                }


//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;
                 
//                var currChurchMemberLogged = _context.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oOffertoryTrans.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCurrMdl);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }


//                var offertoryMdl = (
//                   from t_ot in _context.OffertoryTrans.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_ot.ChurchBodyId && c.AppGlobalOwnerId == t_ot.AppGlobalOwnerId)   //c.Id == oChurchBodyId && 
//                   from t_ap in _context.AccountPeriod.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_ot.AccountPeriodId)   //c.Id == oChurchBodyId && 
//                   from t_an in _context.AppUtilityNVP.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_ot.OffertoryTypeId).DefaultIfEmpty()
//                   from t_cce in _context.ChurchCalendarEvent.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_ot.RelatedEventId).DefaultIfEmpty()
//                   from t_curr in _context.Currency.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_ot.CurrencyId).DefaultIfEmpty()

//                   select new OffertoryModel()
//                   {
//                       oOffertoryTrans = t_ot,
//                       oAppGlolOwnId = t_ot.AppGlobalOwnerId,
//                       oAppGlolOwn = t_ot.AppGlobalOwner,
//                       oChurchBodyId = t_ot.ChurchBodyId,
//                       oChurchBody = t_ot.ChurchBody,
//                       //                       
//                       strOffertoryType = t_an != null ? t_an.NvpValue : "",
//                       strRelatedEvent = t_cce != null ? t_cce.Subject : "",
//                       strAccountPeriod = t_ap != null ? t_ap.PeriodDesc : "",
//                       strCurrency = t_curr != null ? t_curr.Acronym : "",
//                       //
//                       strAmount = String.Format("{0:N2}", t_ot.AmtPaid),
//                       dt_OffertoryDate = t_ot.OffertoryDate,
//                       strOffertoryDate = t_ot.OffertoryDate != null ? DateTime.Parse(t_ot.OffertoryDate.ToString()).ToString("ddd, dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",
//                       strPostDate = t_ot.PostedDate != null ? DateTime.Parse(t_ot.PostedDate.ToString()).ToString("ddd, dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",
//                       strPostStatus = GetPostStatusDesc(t_ot.PostStatus)
//                   }
//                   )   
//                   .OrderByDescending(c=>c.dt_OffertoryDate)
//                   .ToList();

                                                
//                oCurrMdl.lsOffertoryModel = offertoryMdl;

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
//                oCurrMdl.strCurrTask = "Church Offertory";
                  
//                // TempData.Put("oVmCB_CNFG", oCurrMdl);
//                TempData.Keep();
//                return View(oCurrMdl);
//            }
//        }


//        [HttpGet]
//        public IActionResult AddOrEdit_OFFT(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0,  int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) //, int pageIndex = 1)
//        {   //int subSetIndex = 0,

//            var oCurrMdl = new OffertoryModel(); TempData.Keep();
//            if (setIndex == 0) return PartialView("_AddOrEdit_OFFT", oCurrMdl);

//            var curr = _context.Currency.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.IsBaseCurrency == true).FirstOrDefault();
//            if (id == 0)
//            {   
//                var oTransOFFT = new OffertoryTrans(); 

//                oTransOFFT.AppGlobalOwnerId = oAppGloOwnId;
//                oTransOFFT.ChurchBodyId = oCurrChuBodyId;
//                var accPer = _context.AccountPeriod.Where(c => c.AppGlobalOwnerId==oAppGloOwnId && c.ChurchBodyId== oCurrChuBodyId && c.PeriodStatus == "O" && c.LongevityStatus == "C").FirstOrDefault();
//                oTransOFFT.AccountPeriodId = accPer != null ? accPer.Id : (int?)null; 
//                oTransOFFT.CurrencyId = curr != null ? curr.Id : (int?)null; 
//                oTransOFFT.PostStatus = "O";     // O-Open, P-Posted to GL, R-Reversed            
//                oTransOFFT.Status = "A";
//                oTransOFFT.OffertoryDate = DateTime.Now;
//                oTransOFFT.ReceivedByUserId = oUserId_Logged;

//                oCurrMdl.strCurrency = curr != null ? curr.Acronym : "";
//                oCurrMdl.strPostStatus = GetPostStatusDesc(oTransOFFT.PostStatus);
//                if (oTransOFFT != null)
//                {
//                    var oUser = _MSTRContext.UserProfile.Where(c => c.Id == oTransOFFT.ReceivedByUserId).FirstOrDefault();
//                    if (oUser != null) oCurrMdl.strReceivedBy = oUser.UserDesc;
//                }

//                oCurrMdl.oOffertoryTrans = oTransOFFT;
//            }

//            else
//            {
//                var offertoryMdl = (
//                   from t_ot in _context.OffertoryTrans.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId )
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_ot.ChurchBodyId && c.AppGlobalOwnerId == t_ot.AppGlobalOwnerId)   //c.Id == oChurchBodyId && 
//                   from t_ap in _context.AccountPeriod.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_ot.AccountPeriodId)   //c.Id == oChurchBodyId && 
//                   from t_an in _context.AppUtilityNVP.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_ot.OffertoryTypeId).DefaultIfEmpty()
//                   from t_cce in _context.ChurchCalendarEvent.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_ot.RelatedEventId).DefaultIfEmpty()
//                   from t_curr in _context.Currency.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_ot.CurrencyId).DefaultIfEmpty()

//                   select new OffertoryModel()
//                   {
//                       oOffertoryTrans = t_ot,
//                       oAppGlolOwnId = t_ot.AppGlobalOwnerId,
//                       oAppGlolOwn = t_ot.AppGlobalOwner,
//                       oChurchBodyId = t_ot.ChurchBodyId,
//                       oChurchBody = t_ot.ChurchBody, 
//                       //                      
//                       strOffertoryType = t_an != null ? t_an.NvpValue : "",
//                       strRelatedEvent = t_cce != null ? t_cce.Subject : "",
//                       strAccountPeriod = t_ap != null ? t_ap.PeriodDesc : "",
//                       strCurrency = t_curr != null ? t_curr.Acronym : "",
//                       //
//                       strAmount = String.Format("{0:N2}", t_ot.AmtPaid),
//                       strOffertoryDate = t_ot.OffertoryDate != null ? DateTime.Parse(t_ot.OffertoryDate.ToString()).ToString("ddd, dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",
//                       strPostDate = t_ot.PostedDate != null ? DateTime.Parse(t_ot.PostedDate.ToString()).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "",
//                       strPostStatus = GetPostStatusDesc(t_ot.PostStatus)
//                   }
//                   ).FirstOrDefault();

//                if (offertoryMdl != null)
//                    if (offertoryMdl.oOffertoryTrans != null)
//                    {
//                        var oUser = _MSTRContext.UserProfile.Where(c => c.Id == offertoryMdl.oOffertoryTrans.ReceivedByUserId).FirstOrDefault();
//                        if (oUser != null) offertoryMdl.strReceivedBy = oUser.UserDesc;
//                    }

//                oCurrMdl = offertoryMdl;
//            }
             
//            oCurrMdl.setIndex = setIndex;
//            oCurrMdl.oUserId_Logged = oUserId_Logged;
//            oCurrMdl.oAppGlolOwnId_Logged = oAGOId_Logged;
//            oCurrMdl.oChurchBodyId_Logged = oCBId_Logged;
//            //
//            oCurrMdl.oAppGlolOwnId = oAppGloOwnId;
//            oCurrMdl.oChurchBodyId = oCurrChuBodyId;
//            var oCurrChuBody = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
//            oCurrMdl.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null ;
            
           

//            ChurchBody oChurchBody = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
//            oCurrMdl = this.populateLookups_OFFT(oCurrMdl, oChurchBody);
//           // oCurrMdl.strCurrTask = "Church Offertory";

//            var _oCurrMdl = Newtonsoft.Json.JsonConvert.SerializeObject(oCurrMdl);
//            TempData["oVmCurrMod"] = _oCurrMdl; TempData.Keep();

//            return PartialView("_AddOrEdit_OFFT", oCurrMdl);
//        }

//        private OffertoryModel populateLookups_OFFT(OffertoryModel vmLkp, ChurchBody oCurrChuBody)
//        {
//            if (vmLkp == null || oCurrChuBody == null) return vmLkp;
//            //
//            vmLkp.lkpStatuses = new List<SelectListItem>();
//            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpPaymentModes = new List<SelectListItem>();
//            foreach (var dl in dlPmntModes) { vmLkp.lkpPaymentModes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }
//           // vmLkp.lkpPaymentModes.Add(new SelectListItem { Value = "O", Text = "Other" });

//            vmLkp.lkpOffertoryTypes = _context.AppUtilityNVP.Where(c => c.NvpCode == "OFFT_TYP")
//                       .OrderBy(c => c.RequireUserCustom).ThenBy(c => c.OrderIndex).ThenBy(c => c.NvpValue)
//                       .ToList()
//                       .Select(c => new SelectListItem()
//                       {
//                           Value = c.Id.ToString(),
//                           Text = c.NvpValue
//                       })
//                       // .OrderBy(c => c.Text)
//                       .ToList();
//            vmLkp.lkpOffertoryTypes.Insert(0, new SelectListItem { Value = "", Text = "Select" });


//            //var chMemList = (from t_cm in _context.ChurchMember.Where(c => c.ChurchBodyId == oChurchBodyId && c.MemberClass == "C" && c.IsActivated == true)
//            //                 from t_ms in _context.MemberStatus.Where(c => c.ChurchBodyId == oChurchBodyId && c.ChurchMemberId == t_cm.Id &&  //.Include(t => t.ChurchMemStatus)
//            //                                                            c.IsCurrent == true && c.ChurchMemStatus.Available == true && c.ChurchMemStatus.Deceased == false)
//            //                 select new ChurchMember
//            //                 {
//            //                     Id = t_cm.Id,
//            //                     //FirstName = t_cm.FirstName,
//            //                     //MiddleName = t_cm.MiddleName ,
//            //                     //LastName = t_cm.LastName,
//            //                     MemberGlobalId = t_cm.MemberGlobalId,
//            //                     strMemberName = GetConcatMemberName(t_cm.Title, t_cm.FirstName, t_cm.MiddleName, t_cm.LastName, false)
//            //                     //(((t_cm.FirstName + ' ' + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() + " " + (!string.IsNullOrEmpty(t_cm.Title) ? "(" + t_cm.Title + ")" : "")).Trim()
//            //                 })

//            //                 .OrderBy(c => c.strMemberName) //((c.FirstName + ' ' + c.MiddleName).Trim() + " " + c.LastName).Trim()) //strMemberName
//            //                 .ToList();
//            //vmLkp.lkpChurchMembers_Local = chMemList
//            //                               .Select(c => new SelectListItem()
//            //                               {
//            //                                   Value = c.Id.ToString(),
//            //                                   Text = (c.MemberGlobalId + ' ' + c.strMemberName).Trim()
//            //                               })
//            //                               // .OrderBy(c => c.Text)
//            //                               .ToList();
//            //vmLkp.lkpChurchMembers_Local.Insert(0, new SelectListItem { Value = "", Text = "Select" });


//            //var attnDate = DateTime.Now;
//            //var dtv = attnDate != null ? ((DateTime)attnDate).Date : (DateTime?)null;
//            vmLkp.lkpChurchEvents = _context.ChurchCalendarEvent.Include(t => t.ChurchBody)  //.Include(t => t.RelatedChurchLifeActivity)
//                .Where(c => !string.IsNullOrEmpty(c.Subject) && c.EventActive == true && c.ChurchBody.AppGlobalOwnerId == oCurrChuBody.AppGlobalOwnerId //&&   //c.ChurchBody.CountryId == oCurrChuBody.CountryId && 
//                       //((DateTime)c.EventFrom.Value).Date == dtv &&

//                        //(c.EventFrom.HasValue ? ((DateTime)c.EventFrom.Value).Date==dts : c.EventFrom==dts) &&
//                        //((oAttendVM.m_DateAttended.HasValue==false && c.EventFrom == null) || 
//                        //   (oAttendVM.m_DateAttended.HasValue==true && ((DateTime)c.EventFrom).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) == dt)) &&
//                        //(c.OwnedByChurchBodyId == oCurrChuBody.Id ||
//                        //(c.OwnedByChurchBodyId != oCurrChuBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oCurrChuBody.ParentChurchBodyId) ||
//                        //(c.OwnedByChurchBodyId != oCurrChuBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oCurrChuBody)))
//                    )
//                     .OrderByDescending(c => c.EventFrom).ThenByDescending(c => c.EventTo)
//                     .ToList()
//                             .Select(c => new SelectListItem()
//                             {
//                                 Value = c.Id.ToString(),
//                                 Text = c.Subject + ":- " +
//                                                     (c.IsFullDay == true ?
//                                                         (c.EventFrom != null ? DateTime.Parse(c.EventFrom.ToString()).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "").Trim() :
//                                                         (c.EventFrom != null ? DateTime.Parse(c.EventFrom.ToString()).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "").Trim() +
//                                                         (c.EventFrom != null && c.EventTo != null ? " -- " : "") +
//                                                         (c.EventTo != null ? DateTime.Parse(c.EventTo.ToString()).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) : "").Trim()
//                                                      )
//                             })
//                             //.OrderBy(c => c.Text)
//                             .ToList();

//            vmLkp.lkpChurchEvents.Insert(0, new SelectListItem { Value = "", Text = "Select" });


//            vmLkp.lkpCurrencies = _context.Currency.OrderBy(c => c.ChurchBodyId == oCurrChuBody.Id && c.Status=="A") .ToList()
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
//                                            .OrderBy(c => c.PeriodIndex)
//                                            .ToList()
//                                          .Select(c => new SelectListItem()
//                                          {
//                                              Value = c.Id.ToString(),
//                                              Text = c.PeriodDesc,
//                                              Selected = c.LongevityStatus == "C"  //Current
//                                          })
//                                         // .OrderBy(c => c.Text)
//                                          .ToList();
//            //  vmLkp.lkpAccountPeriods.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//            return vmLkp;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_OFFT(OffertoryModel vmMod)
//        {

//            OffertoryTrans _oChanges = vmMod.oOffertoryTrans;
//            vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as OffertoryModel : vmMod; TempData.Keep();

//            var oMdlData = vmMod.oOffertoryTrans;
//            oMdlData.ChurchBody = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oOffertoryTrans.AppGlobalOwnerId");
//                ModelState.Remove("oOffertoryTrans.ChurchBodyId");
//                ModelState.Remove("oOffertoryTrans.OffertoryTypeId");
//                ModelState.Remove("oOffertoryTrans.RelatedEventId");
//                ModelState.Remove("oOffertoryTrans.AccountPeriodId");
//                ModelState.Remove("oOffertoryTrans.CurrencyId");

//                ModelState.Remove("oOffertoryTrans.CreatedByUserId");
//                ModelState.Remove("oOffertoryTrans.LastModByUserId"); 
//                ModelState.Remove("oUserId_Logged"); 

//                // ChurchBody == null 

//                //finally check error state...
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again." });
                                            
//                if (_oChanges.AmtPaid == null || _oChanges.AmtPaid == 0) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Enter the offertory amount" });
//                }
//                if (_oChanges.CurrencyId == null) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please select currency." });
//                }
//                if (_oChanges.AccountPeriodId == null) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please select account period."  });
//                }
                 

//                if (_oChanges.OffertoryDate == null)  //Congregant... ChurchCodes required
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the date." });
//                }

//                if (_oChanges.OffertoryDate != null)   
//                {
//                    if ( _oChanges.OffertoryDate.Value > DateTime.Now.Date)
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Select the correct date. Date cannot be later than today."  });

//                    if (_oChanges.PostedDate != null)
//                        if (_oChanges.OffertoryDate.Value > _oChanges.PostedDate.Value)
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

//                    ViewBag.UserMsg = "Saved offertory data successfully.";
//                }
//                else
//                {
//                    //retain the pwd details... hidden fields 
//                    _context.Update(_oChanges);
//                    ViewBag.UserMsg = "User offertory data updated successfully.";
//                }

//                //save user profile first... 
//                await _context.SaveChangesAsync();
                 

//                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
//                TempData["oVmCurrMod"] = _vmMod; TempData.Keep();

               
//                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg });
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving offertory details. Err: " + ex.Message });
//            }

//        }

//    }
//}
