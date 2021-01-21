//using System;
//using System.Collections.Generic; 
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
//    public class AccountTypeCategoryController : Controller
//    {
//        private readonly ChurchModelContext _context;
//        private readonly MSTR_DbContext _MSTRContext;
//        private readonly IWebHostEnvironment _hostingEnvironment;

//        private bool isCurrValid = false;
//        private List<UserSessionPrivilege> oUserLogIn_Priv = null;

//        //  private List<DiscreteLookup> dlShareStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlGenStatuses = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlMainAccTypes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlRptAreas = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlBalTypes = new List<DiscreteLookup>();

//        private List<DiscreteLookup> dlINC_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlEXP_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlFAS_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlCAS_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlCLB_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlELB_Types = new List<DiscreteLookup>();
//        public AccountTypeCategoryController(ChurchModelContext context, MSTR_DbContext ctx,
//             IWebHostEnvironment hostingEnvironment)
//        {
//            _context = context;
//            _MSTRContext = ctx;
//            _hostingEnvironment = hostingEnvironment;


//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "B", Desc = "Blocked" });
//           // dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "E", Desc = "Deactive" });

//            ////SharingStatus { get; set; }  // A - Share with all sub-congregations, C - Share with child congregations only, N - Do not share
//            //dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "N", Desc = "Do not roll-down (share)" });
//            //dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "C", Desc = "Roll-down (share) for direct child congregations" });
//            //dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "A", Desc = "Roll-down (share) for all sub-congregations" });

//            // Accout Types  ... INC, EXP, FAS, CAS, CLB, ELB
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "INC", Desc = "Income" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "EXP", Desc = "Expense" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "FAS", Desc = "Fixed Asset" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "CAS", Desc = "Current Asset" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "CLB", Desc = "Current Liability" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "ELB", Desc = "Equity/Long-term Liability" });

//            // IS - Income Statement  BS - Balance Sheet
//            dlRptAreas.Add(new DiscreteLookup() { Category = "RptArea", Val = "IS", Desc = "Income Statement" });
//            dlRptAreas.Add(new DiscreteLookup() { Category = "RptArea", Val = "BS", Desc = "Balance Sheet" });

//            // DB - Debit   CR - Credit
//            dlBalTypes.Add(new DiscreteLookup() { Category = "BalType", Val = "DB", Desc = "Debit" });
//            dlBalTypes.Add(new DiscreteLookup() { Category = "BalType", Val = "CR", Desc = "Credit" });



//            // Account Type Category  -- Financial Category        [ASSET = EQUITY + LIABILITY]
//            //        INCOME... [SALES/Receipts, COST OF SALES/Receipts, OTHER INCOME/Receipts], 
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC1", Val = "INC1", Desc = "Receipts" });
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC2", Val = "INC2", Desc = "Cost of Receipts" });
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC3", Val = "INC3", Desc = "Other Receipts" });

//            //        EXPENSE... [EXPENSES, TAX, DIVIDENDS], 
//            dlEXP_Types.Add(new DiscreteLookup() { Category = "EXP1", Val = "EXP1", Desc = "Expenses" });
//          //  dlEXP_Types.Add(new DiscreteLookup() { Category = "EXP_Type", Val = "EXP2", Desc = "Tax" });

//            //        FIXED ASSETS... [INVESTMENTS, OTHER FIXED ASSETS]
//            dlFAS_Types.Add(new DiscreteLookup() { Category = "FAS1", Val = "FAS1", Desc = "Investments" });
//            dlFAS_Types.Add(new DiscreteLookup() { Category = "FAS2", Val = "FAS2", Desc = "Other Fixed Assets" });

//            //        CURRENT ASSETS... [ACC RECEIVABLE, INVENTORY [Asset Register/Properties], CASH]
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS1", Val = "CAS1", Desc = "Account Receivable" });
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS2", Val = "CAS2", Desc = "Assets & Properties" });
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS3", Val = "CAS3", Desc = "Cash" });

//            //        CURRENT LIABILITIES... [ACC PAYABLE, OTHER CURR LIABILITIES]
//            dlCLB_Types.Add(new DiscreteLookup() { Category = "CLB1", Val = "CLB1", Desc = "Account Payable" });
//            dlCLB_Types.Add(new DiscreteLookup() { Category = "CLB2", Val = "CLB2", Desc = "Other Liabilities" });

//            //        EQUITY... [RETAINED EARNINGS(OTHER {STARTING CAPITAL}, INC.SURPLUS), LONG-TERM BORROWINGS, OTHER LONG-TERM LIABILITIES]'
//            dlELB_Types.Add(new DiscreteLookup() { Category = "ELB1", Val = "ELB1", Desc = "Retained Receipts" });
//            dlELB_Types.Add(new DiscreteLookup() { Category = "ELB2", Val = "ELB2", Desc = "Long-term Borrowings" });
//            dlELB_Types.Add(new DiscreteLookup() { Category = "ELB3", Val = "ELB3", Desc = "Other Long-term Liabilities" });

//        }


//        public static string GetStatusDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "A": return "Active";
//                case "B": return "Blocked";
//                //case "D": return "Deactive";
//                //case "P": return "Pending";
//                //case "E": return "Expired";

//                default: return oCode;
//            }
//        }

//        public static string GetMainAccTypeDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "INC": return "Income";
//                case "EXP": return "Expense";
//                case "FAS": return "Fixed Asset";
//                case "CAS": return "Current Asset";
//                case "CLB": return "Current Liability";
//                case "ELB": return "Equity/Long-term Liability"; 

//                default: return oCode;
//            }
//        }

//        public static string GetRptTypeDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "IS": return "Income Statement";
//                case "BS": return "Balance Sheet";  

//                default: return oCode;
//            }
//        }
//        public static string GetBalTypeDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "DB": return "Debit";
//                case "CR": return "Credit";

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


//        public ActionResult Index(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 1) //, int? oParentId = null, int? id = null, int pageIndex = 1) //, int? oChuCategId = null, bool oShowAllCong = true) //, int? currFilterVal = null) //, ChurchBodyConfigMDL oCurrCBConfig = null)
//        {  // int subSetIndex = 0, 
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                // check permission 
//                if (!this.userAuthorized) return View(new AccountCategoryModel()); //retain view                  
//                var oAccountCategory = oUserLogIn_Priv[0].UserProfile;  // if (oCurrChuBodyLogOn == null) return View(oCurrMdl);
//                if (oAccountCategory == null) return View(new AccountCategoryModel());
//                //
//                var oCurrMdl = new AccountCategoryModel(); //TempData.Keep();  
//                                                         // int? oAppGloOwnId = null;
//                var oChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                //
//                int? oAppGloOwnId_Logged = null;
//                int? oChuBodyId_Logged = null;
//                int? oUserId_Logged = null;
//                if (oChuBodyLogOn != null)
//                {
//                    oAppGloOwnId_Logged = oChuBodyLogOn.AppGlobalOwnerId;
//                    oChuBodyId_Logged = oChuBodyLogOn.Id;
//                    oUserId_Logged = oAccountCategory.Id;

//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBodyLogOn.Id; }
//                    if (oAppGloOwnId == null) { oAppGloOwnId = oChuBodyLogOn.AppGlobalOwnerId; }
//                    //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
//                    //
//                    // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                }


//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//                var currChurchMemberLogged = _context.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oAccountCategory.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCurrMdl);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }

//                ////var lsRptArea = this.dlRptAreas;
//                ////var lsBalTypes = this.dlBalTypes;
//              //  var a = _context.AccountCategory.ToList();

//                var AccountTypeMdl = (
//                   from t_ac in _context.AccountCategory.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_ac.ChurchBodyId && c.AppGlobalOwnerId == t_ac.AppGlobalOwnerId)   //c.Id == oChurchBodyId && 
//                   from t_ac_p in _context.AccountCategory.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_ac.ChurchBodyId && c.AppGlobalOwnerId == t_ac.AppGlobalOwnerId && c.Id == t_ac.ParentCategoryId)
//                       //from t_mat in this.dlMainAccTypes.ToList().Where(c => c.Val== t_AC.AccountType)   //c.Id == oChurchBodyId && 
//                       //from t_ra in lsRptArea.Where(c => c.Val == t_AC.ReportArea)
//                       //from t_bt in lsBalTypes.Where(c => c.Val == t_AC.BalanceType)

//                   select new AccountCategoryModel()
//                   {
//                       oAccountCategory = t_ac,
//                       oAppGlolOwnId = t_ac.AppGlobalOwnerId,
//                       oAppGlolOwn = t_ac.AppGlobalOwner,
//                       oChurchBodyId = t_ac.ChurchBodyId,
//                       oChurchBody = t_ac.ChurchBody,
//                       //                       
//                       strAccountCategory = t_ac != null ? t_ac.CategoryDesc : "",
//                       strParentCategory = t_ac_p != null ? t_ac_p.CategoryDesc : "",
//                       strAccountType = GetMainAccTypeDesc(t_ac.AccountType),  // t_mat != null ? t_mat.Desc : "",
//                       strBalanceType = GetBalTypeDesc(GetAccBalCodeByAccountTypeCode(t_ac.AccountType)),  //t_bt != null ? t_bt.Desc  : "",
//                       strReportArea = GetRptTypeDesc(t_ac.ReportArea),  //t_ra != null ? t_ra.Desc : "",
//                       //
//                       strStatus = GetStatusDesc(t_ac.Status)
//                   }).ToList()
//                   .OrderBy(c => c.strAccountType).ThenBy(c => c.strAccountCategory)
//                   .ToList();


//                oCurrMdl.lsAccountCategoryModel = AccountTypeMdl;

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
//                oCurrMdl.strCurrTask = "Church AccountType";

//                // TempData.Put("oVmCB_CNFG", oCurrMdl);
//                var _oCurrMdl = Newtonsoft.Json.JsonConvert.SerializeObject(oCurrMdl);
//                TempData["oVmCurrMod"] = _oCurrMdl; TempData.Keep();

//                TempData.Keep();
//                return View(oCurrMdl);
//            }
//        }


//        [HttpGet]
//        public IActionResult AddOrEdit_AC(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) //, int pageIndex = 1)
//        {   //int subSetIndex = 0,

//            var oCurrMdl = new AccountCategoryModel(); TempData.Keep();
//            if (setIndex == 0) return PartialView("_AddOrEdit_AC", oCurrMdl);

//          //  var curr = _context.Currency.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.IsBaseCurrency == true).FirstOrDefault();
//            if (id == 0)
//            {
//                var oTransACCM = new AccountCategory();

//                oTransACCM.AppGlobalOwnerId = oAppGloOwnId;
//                oTransACCM.ChurchBodyId = oCurrChuBodyId;
//             //   var accPer = _context.AccountPeriod.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.PeriodStatus == "O" && c.LongevityStatus == "C").FirstOrDefault();

//                //oTransACCM.AccountType = "INC";
//                //oTransACCM.ReportArea = "IS";
//                //oTransACCM.BalanceType = "CR";
 
//                oTransACCM.Status = "A";
//                oTransACCM.CreatedByUserId = oUserId_Logged;
//                oTransACCM.LastModByUserId = oUserId_Logged;

//                oCurrMdl.strStatus = GetStatusDesc(oTransACCM.Status);
//                //
//                oCurrMdl.oAccountCategory = oTransACCM;
//                oCurrMdl.strAccountCategory = "";
//                oCurrMdl.strAccountType = "";
//                oCurrMdl.strBalanceType = "";
//                oCurrMdl.strReportArea = "";
//            }

//            else
//            {
//                var AccountTypeMdl = (
//                   from t_ac in _context.AccountCategory.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_ac.ChurchBodyId && c.AppGlobalOwnerId == t_ac.AppGlobalOwnerId)   //c.Id == oChurchBodyId && 
//                   from t_ac_p in _context.AccountCategory.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_ac.ChurchBodyId && c.AppGlobalOwnerId == t_ac.AppGlobalOwnerId && c.Id == t_ac.ParentCategoryId)
//                       //from t_mat in this.dlMainAccTypes.ToList().Where(c => c.Val== t_AC.AccountType)   //c.Id == oChurchBodyId && 
//                       //from t_ra in lsRptArea.Where(c => c.Val == t_AC.ReportArea)
//                       //from t_bt in lsBalTypes.Where(c => c.Val == t_AC.BalanceType)

//                   select new AccountCategoryModel()
//                   {
//                       oAccountCategory = t_ac,
//                       oAppGlolOwnId = t_ac.AppGlobalOwnerId,
//                       oAppGlolOwn = t_ac.AppGlobalOwner,
//                       oChurchBodyId = t_ac.ChurchBodyId,
//                       oChurchBody = t_ac.ChurchBody,
//                       //                       
//                       strAccountCategory = t_ac != null ? t_ac.CategoryDesc : "",
//                       strParentCategory = t_ac_p != null ? t_ac_p.CategoryDesc : "",
//                       strAccountType = GetMainAccTypeDesc(t_ac.AccountType),  // t_mat != null ? t_mat.Desc : "",
//                       strBalanceType = GetBalTypeDesc(GetAccBalCodeByAccountTypeCode(t_ac.AccountType)),  //t_bt != null ? t_bt.Desc  : "",
//                       strReportArea = GetRptTypeDesc(t_ac.ReportArea),  //t_ra != null ? t_ra.Desc : "",
//                       //
//                       strStatus = GetStatusDesc(t_ac.Status)
//                   }).FirstOrDefault();

//                oCurrMdl = AccountTypeMdl;
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


//            ChurchBody oChurchBody = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
//            oCurrMdl = this.populateLookups_AC(oCurrMdl, oChurchBody, oCurrMdl.oAccountCategory.Id);
//            // oCurrMdl.strCurrTask = "Church AccountType";

//            var _oCurrMdl = Newtonsoft.Json.JsonConvert.SerializeObject(oCurrMdl);
//            TempData["oVmCurrMod"] = _oCurrMdl; TempData.Keep();

//            return PartialView("_AddOrEdit_AC", oCurrMdl);
//        }

//        private AccountCategoryModel populateLookups_AC(AccountCategoryModel vmLkp, ChurchBody oCurrChuBody, int currCategoryId = 0)
//        {
//            if (vmLkp == null || oCurrChuBody == null) return vmLkp;
//            //
//            vmLkp.lkpAccountTypes = new List<SelectListItem>();
//            foreach (var dl in dlMainAccTypes) { vmLkp.lkpAccountTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpBalanceTypes = new List<SelectListItem>();
//            foreach (var dl in dlBalTypes) { vmLkp.lkpBalanceTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpReportAreas = new List<SelectListItem>();
//            foreach (var dl in dlRptAreas) { vmLkp.lkpReportAreas.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpStatuses = new List<SelectListItem>();
//            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpParentCategories = _context.AccountCategory.Where(c => c.Status == "A" && c.Id != currCategoryId)
//                       .OrderBy(c => c.AccountType).ThenBy(c => c.CategoryCode) //.ToList()
//                       .Select(c => new SelectListItem()
//                       {
//                           Value = c.Id.ToString(),
//                           Text = c.CategoryDesc
//                       })
//                       // .OrderBy(c => c.Text)
//                       .ToList();
//            vmLkp.lkpParentCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });
             

//            return vmLkp;
//        }


//        //public string GetAccountTypeByReportAreaCode(string ReportAreaCode) //, int? oCurrChuBodyId = null, int? oAppGlolOwnId = null)   
//        //{
//        //    var strAccType = "";

//        //    if (ReportAreaCode == "IS")
//        //    {
//        //        foreach (var dl in dlMainAccTypes)
//        //        {
//        //            if (dl.Val.StartsWith("INC") || dl.Val.StartsWith("EXP"))
//        //                oList.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc });
//        //        }
//        //    }

//        //    else if (ReportAreaCode == "BS")
//        //    {
//        //        foreach (var dl in dlMainAccTypes)
//        //        {
//        //            if (dl.Val.StartsWith("FAS") || dl.Val.StartsWith("CAS") || dl.Val.StartsWith("CLB") || dl.Val.StartsWith("ELB"))
//        //                oList.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc });
//        //        }
//        //    }

//        //    return strAccType;
//        //}

//        public static string  GetAccBalCodeByAccountTypeCode(string AccountTypeCode) //, int? oCurrChuBodyId = null, int? oAppGlolOwnId = null)   
//        {
//            var strBal = "";

//            if (AccountTypeCode.StartsWith("EXP") || AccountTypeCode.StartsWith("CAS") || AccountTypeCode.StartsWith("FAS"))
//                strBal = "DB";

//            else if (AccountTypeCode.StartsWith("INC") || AccountTypeCode.StartsWith("CLB") || AccountTypeCode.StartsWith("ELB"))
//                strBal = "CR";

//            return strBal;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_AC(AccountCategoryModel vmMod)
//        {
//            AccountCategory _oChanges = vmMod.oAccountCategory;
//            var tempList = ""; // TempData["oVmCurrMod"] as string;
//            tempList = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : tempList;
//            if (!string.IsNullOrEmpty(tempList))
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountCategoryModel>(tempList); // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : vmMod;  

//            var oMdlData = vmMod.oAccountCategory;
//            oMdlData.ChurchBody = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oAccountCategory.AppGlobalOwnerId");
//                ModelState.Remove("oAccountCategory.ChurchBodyId");
//                ModelState.Remove("oAccountCategory.ParentCategoryId");
//                //ModelState.Remove("oAccountCategory.BalanceType");
//                //ModelState.Remove("oAccountCategory.ReportArea");
//                //ModelState.Remove("oAccountCategory.Status");

//                ModelState.Remove("oAccountCategory.CreatedByUserId");
//                ModelState.Remove("oAccountCategory.LastModByUserId");
//                ModelState.Remove("oUserId_Logged");

//                // ChurchBody == null 

//                //finally check error state...  // 100000-000   == Main, // 100000-001   == Sub
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

//                if (string.IsNullOrEmpty(_oChanges.CategoryDesc)) 
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify description" }); 

//                if (string.IsNullOrEmpty(_oChanges.AccountType)) 
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify account type" });

//                if (string.IsNullOrEmpty(_oChanges.ReportArea)) 
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify account report area" }); 

//                if (string.IsNullOrEmpty(_oChanges.BalanceType)) 
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify account balance type" }); 

//                if (_oChanges.ParentCategoryId==null) 
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify parent account category" }); 

//                if (oMdlData.ParentAccountCategory == null)
//                    oMdlData.ParentAccountCategory = _context.AccountCategory.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId && c.Id == _oChanges.ParentCategoryId).FirstOrDefault();

//                if (oMdlData.ParentAccountCategory == null)
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parent account specified could not be found. Please refresh and try again." });


//                //reset form for new item...
//                var _reset = _oChanges.Id == 0;

//                //
//                var tm = DateTime.Now;
//                _oChanges.LastMod = tm;
//                if (_oChanges.LastModByUserId == null) _oChanges.LastModByUserId = vmMod.oUserId_Logged;
                 
//                var parCategory = oMdlData.ParentAccountCategory;
//               // _oChanges.CategoryCode = (parCategory.CategoryCode.Contains("_") ? parCategory.CategoryCode.Substring(0, parCategory.CategoryCode.IndexOf("_")) : parCategory.CategoryCode) + "_" + String.Format("{0:0000.#}", _oChanges.str);
//                _oChanges.CategoryCode = String.Format("{0:0000.#}", _oChanges.strParentCategoryCode) + "_" + String.Format("{0:0000.#}", _oChanges.strSubCategoryCode);
//                _oChanges.LevelIndex = parCategory.LevelIndex + 1;

//                //validate...
//                if (_oChanges.Id == 0)
//                { 
//                    _oChanges.Created = tm;
//                    if (_oChanges.CreatedByUserId == null) _oChanges.CreatedByUserId = vmMod.oUserId_Logged;
//                    _context.Add(_oChanges);

//                    ViewBag.UserMsg = "Created financial category successfully.";
//                }
//                else
//                {
//                    //retain the pwd details... hidden fields 
//                    _context.Update(_oChanges);
//                    ViewBag.UserMsg = "Financial category details updated successfully.";
//                }

//                //save user profile first... 
//                await _context.SaveChangesAsync();


//                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
//                TempData["oVmCurr"] = _vmMod; TempData.Keep();


//                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg });
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving account type details. Err: " + ex.Message });
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> ResetToDefaults_AC( AccountCategoryModel vmMod)
//        {

//            //AccountCategory _oChanges = vmMod.oAccountCategory;
//            // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as AccountCategoryModel : new AccountCategoryModel(); TempData.Keep();

//            var tempList = ""; // TempData["oVmCurrMod"] as string;
//            tempList = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : tempList;
//            if (!string.IsNullOrEmpty(tempList))
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountCategoryModel>(tempList);

//           // var oMdlData = vmMod.oAccountCategory;
//            //oMdlData.ChurchBody = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oAccountCategory.AppGlobalOwnerId");
//                ModelState.Remove("oAccountCategory.ChurchBodyId");
//                ModelState.Remove("oAccountCategory.ParentCategoryId");
//                //ModelState.Remove("oAccountCategory.AccountType");
//                //ModelState.Remove("oAccountCategory.BalanceType");
//                //ModelState.Remove("oAccountCategory.ReportArea");
//                //ModelState.Remove("oAccountCategory.Status");

//                ModelState.Remove("oAccountCategory.CreatedByUserId");
//                ModelState.Remove("oAccountCategory.LastModByUserId");
//                ModelState.Remove("oUserId_Logged");

//                // ChurchBody == null 

//                //finally check error state...  // 100000-000   == Main, // 100000-001   == Sub
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = -1, userMess = "Failed to load the data to save. Please refresh and try again." });

//                if (vmMod==null)
//                {
//                    return Json(new { taskSuccess = false, oCurrId = -1, userMess = "User logging details could not be found" });
//                }

//                //if (string.IsNullOrEmpty(_oChanges.CategoryDesc))
//                //{
//                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify financial category description" });
//                //}
//                //if (string.IsNullOrEmpty(_oChanges.AccountType))
//                //{
//                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify account type" });
//                //}
//                //if (string.IsNullOrEmpty(_oChanges.ReportArea))
//                //{
//                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify account report area" });
//                //}
//                //if (string.IsNullOrEmpty(_oChanges.BalanceType))
//                //{
//                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify account balance type" });
//                //}

//                //reset form for new item...
//                // var _reset = _oChanges.Id == 0;

//                // AccountCategory _oChanges = null;



//                //clear existing categories
//                //
//                //

//                var tm = DateTime.Now;
//                var nextCodeCount = _context.AccountCategory.Where(c => c.AppGlobalOwnerId == vmMod.oAppGlolOwnId && c.ChurchBodyId == vmMod.oChurchBodyId).Count() ;

//                //get list of all main categories...   6 account types
//                //0000_0001-INC, 0000_0002-EXP, 0000_0003-FAS, 0000_0004-CAS, 0000_0005-CLB, 0000_0006-ELB, [+3]... 
//                foreach (var category in this.dlMainAccTypes)
//                {
//                    var accType = category.Val.Substring(0, 3).ToUpper();
//                    var _oChanges = _context.AccountCategory.Where(c => c.AppGlobalOwnerId == vmMod.oAppGlolOwnId && c.ChurchBodyId == vmMod.oChurchBodyId &&
//                                        c.CategoryCode == category.Val).FirstOrDefault();
//                    if (_oChanges == null)
//                    {
//                        nextCodeCount++;
//                        _oChanges = new AccountCategory()
//                        {
//                            //Id
//                            AppGlobalOwnerId = vmMod.oAppGlolOwnId,
//                            ChurchBodyId = vmMod.oChurchBodyId,
//                            CategoryDesc = category.Desc,
//                            OwnerKey = category.Val, //INC1, INC2, EXP1, ELB1, CAS1, FAS1, CLB1 etc.
//                            CategoryCode = "0000_" + String.Format("{0:0000.#}", nextCodeCount),
//                            // ParentCategoryId = category.Val,  .. set after saving...
//                            AccountType = accType, //category.Val.Substring(0, 3).ToUpper(),
//                            ReportArea = (accType == "INC" || accType == "EXP") ? "IS" : "BS",
//                            BalanceType = GetAccBalCodeByAccountTypeCode(accType),
//                            LevelIndex = 1,
//                            Status = "A",
//                            Created = tm,
//                            LastMod = tm,
//                            CreatedByUserId = vmMod.oUserId_Logged,
//                            LastModByUserId = vmMod.oUserId_Logged
//                        };


//                        _context.Add(_oChanges);
//                    }
//                    else
//                    {
//                        //_oChanges.AppGlobalOwnerId = vmMod.oAppGlolOwnId;
//                        //_oChanges.ChurchBodyId = vmMod.oChurchBodyId;
//                        _oChanges.CategoryDesc = category.Desc;                       
//                        _oChanges.OwnerKey = category.Val; //INC1, INC2, EXP1, ELB1, CAS1, FAS1, CLB1 etc.
//                       // _oChanges.CategoryCode = "0000_" + String.Format("{0:0000.#}", nextCodeCount);  // _oChanges.CategoryCode = category.Val;
//                        // ParentCategoryId = category.Val,  .. set after saving...                        
//                        _oChanges.AccountType = accType;
//                        _oChanges.ReportArea = (accType == "INC" || accType == "EXP") ? "IS" : "BS";
//                        _oChanges.BalanceType = GetAccBalCodeByAccountTypeCode(accType);
//                        _oChanges.LevelIndex = 1;
//                        _oChanges.Status = "A";
//                        //_oChanges.Created = tm;
//                        _oChanges.LastMod = tm;
//                        //_oChanges.CreatedByUserId = vmMod.oUserId_Logged;
//                        _oChanges.LastModByUserId = vmMod.oUserId_Logged;


//                        _context.Update(_oChanges);
//                    }
//                }

//                //save first... account categories... 
//                await _context.SaveChangesAsync();



//                //get list of all fin categories... acctypecateg
//                List<DiscreteLookup> lsAccTypeCategories = new List<DiscreteLookup>();

//                lsAccTypeCategories.AddRange(this.dlINC_Types);
//                lsAccTypeCategories.AddRange(this.dlEXP_Types);
//                lsAccTypeCategories.AddRange(this.dlFAS_Types);
//                lsAccTypeCategories.AddRange(this.dlCAS_Types);
//                lsAccTypeCategories.AddRange(this.dlCLB_Types);
//                lsAccTypeCategories.AddRange(this.dlELB_Types);

                
//                //0000_0001-INC, 0000_0002-EXP, 0000_0003-FAS, 0000_0004-CAS, 0000_0005-CLB, 0000_0006-ELB, [+3]..., 000?_0010, 000?_0011...
//                nextCodeCount = _context.AccountCategory.Where(c => c.AppGlobalOwnerId == vmMod.oAppGlolOwnId && c.ChurchBodyId == vmMod.oChurchBodyId).Count() + 3;
//                //
//                foreach (var category in lsAccTypeCategories)
//                {
//                    var _oChanges = _context.AccountCategory.Where(c => c.AppGlobalOwnerId == vmMod.oAppGlolOwnId && c.ChurchBodyId == vmMod.oChurchBodyId &&
//                                        c.CategoryCode == category.Val).FirstOrDefault();
//                    if (_oChanges == null)
//                    {
//                        nextCodeCount++;
//                        _oChanges = new AccountCategory()
//                        {
//                            //Id
//                            AppGlobalOwnerId = vmMod.oAppGlolOwnId,
//                            ChurchBodyId = vmMod.oChurchBodyId,
//                            CategoryDesc = category.Desc,
//                            OwnerKey = category.Val, //INC1, INC2, EXP1, ELB1, CAS1, FAS1, CLB1 etc.
//                            CategoryCode = String.Format("{0:0000.#}", nextCodeCount),
//                           // ParentCategoryId = category.Val,  .. set after saving...
//                            AccountType = category.Val.Substring(0, 3).ToUpper(),
//                            ReportArea = (category.Val.Substring(0, 3).ToUpper() == "INC" || category.Val.Substring(0, 3).ToUpper() == "EXP") ? "IS" : "BS",
//                            BalanceType = GetAccBalCodeByAccountTypeCode(category.Val.Substring(0, 3).ToUpper()),
//                            Status = "A",
//                            Created = tm,
//                            LastMod = tm,
//                            CreatedByUserId = vmMod.oUserId_Logged,
//                            LastModByUserId = vmMod.oUserId_Logged
//                        };


//                        _context.Add(_oChanges);
//                    }
//                    else
//                    {
//                        //_oChanges.AppGlobalOwnerId = vmMod.oAppGlolOwnId;
//                        //_oChanges.ChurchBodyId = vmMod.oChurchBodyId;
//                        _oChanges.CategoryDesc = category.Desc;
//                        _oChanges.OwnerKey = category.Val; //INC1, INC2, EXP1, ELB1, CAS1, FAS1, CLB1 etc.
//                        _oChanges.CategoryCode = String.Format("{0:0000.#}", (_oChanges.CategoryCode.Contains("_") ? _oChanges.CategoryCode.Substring(_oChanges.CategoryCode.IndexOf("_") + 1) : _oChanges.CategoryCode));
//                        // ParentCategoryId = category.Val,  .. set after saving...
//                        var accType = category.Val.Substring(0, 3).ToUpper();
//                        _oChanges.AccountType = accType;
//                        _oChanges.ReportArea = (accType == "INC" || accType == "EXP") ? "IS" : "BS";
//                        _oChanges.BalanceType = GetAccBalCodeByAccountTypeCode(accType);
//                        _oChanges.Status = "A";
//                        //_oChanges.Created = tm;
//                        _oChanges.LastMod = tm;
//                        //_oChanges.CreatedByUserId = vmMod.oUserId_Logged;
//                        _oChanges.LastModByUserId = vmMod.oUserId_Logged;
                        

//                        _context.Update(_oChanges);
//                    }                     
//                }
                 
//                //save  
//                await _context.SaveChangesAsync();



//                // check the default 1st level parents
//                var lsDefaultCategories = _context.AccountCategory.Where(c => c.AppGlobalOwnerId == vmMod.oAppGlolOwnId && c.ChurchBodyId == vmMod.oChurchBodyId).ToList();
//                var _lsDefaultCategories = lsDefaultCategories;
//                foreach (var oCategory in lsDefaultCategories)
//                {
//                    var accType = oCategory.OwnerKey.Substring(0, 3).ToUpper();
//                    var parCategory = _lsDefaultCategories.Find(c => c.OwnerKey == accType);
//                    oCategory.ParentCategoryId = parCategory != null ? parCategory.Id : (int?)null;
//                   // var parAccCode = 
//                    oCategory.CategoryCode = (parCategory.CategoryCode.Contains("_") ? parCategory.CategoryCode.Substring(0, parCategory.CategoryCode.IndexOf("_")) : parCategory.CategoryCode) + "_" + String.Format("{0:0000.#}", oCategory.CategoryCode);
//                    oCategory.LevelIndex = parCategory.LevelIndex + 1;
//                }

//                //save user profile first... 
//                await _context.SaveChangesAsync();
//                ViewBag.UserMsg = "Financial category reset to defaults successfully";

//                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
//                TempData["oVmCurr"] = _vmMod; TempData.Keep();

//                return Json(new { taskSuccess = true, oCurrId = -1, resetNew = false, userMess = ViewBag.UserMsg });
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = -1, userMess = "Financial category reset to defaults failed. Err: " + ex.Message });
//            }
//        }

//    }
//}
