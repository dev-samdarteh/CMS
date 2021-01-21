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
//    public class TransControlAccountController : Controller
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
//        private List<DiscreteLookup> dlTransModules = new List<DiscreteLookup>();

//        private List<DiscreteLookup> dlINC_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlEXP_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlFAS_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlCAS_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlCLB_Types = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlELB_Types = new List<DiscreteLookup>();

//        public TransControlAccountController(ChurchModelContext context, MSTR_DbContext ctx,
//             IWebHostEnvironment hostingEnvironment)
//        {
//            _context = context;
//            _MSTRContext = ctx;
//            _hostingEnvironment = hostingEnvironment;


//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "B", Desc = "Blocked" });
//            // dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "E", Desc = "Deactive" }); 

//            // INC, EXP, FAS, CAS, CLB, ELB
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "INC", Desc = "Income" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "EXP", Desc = "Expense" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "FAS", Desc = "Fixed Asset" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "CAS", Desc = "Current Asset" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "CLB", Desc = "Current Liability" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "ELB", Desc = "Equity/Long-term Liability" });

//            // IS - Income Statement  BS - Balance Sheet
//            dlRptAreas.Add(new DiscreteLookup() { Category = "RptArea", Val = "IS", Desc = "Income Statement" });
//            dlRptAreas.Add(new DiscreteLookup() { Category = "RptArea", Val = "BS", Desc = "Balance Sheet" });

//            //// DB - Debit   CR - Credit
//            //dlBalTypes.Add(new DiscreteLookup() { Category = "BalType", Val = "DB", Desc = "Debit" });
//            //dlBalTypes.Add(new DiscreteLookup() { Category = "BalType", Val = "CR", Desc = "Credit" });


//            //control accounts .. OFFT, TITH, DONA, PLDG, DISB, WDIS, DEPR, LEVY, SPPL, PROP
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "OFFT", Desc = "Offertory" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "TITH", Desc = "Tithes" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "DONA", Desc = "Donations" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "PLDG", Desc = "Pledges" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "DISB", Desc = "Disbursements" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "WDIS", Desc = "Welfare Distributions" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "DEPR", Desc = "Depreciation" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "LEVY", Desc = "Levies & Dues" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "PROP", Desc = "Assets/Properties" });
//            dlTransModules.Add(new DiscreteLookup() { Category = "TransMod", Val = "SPLR", Desc = "Suppliers" });



//            //        [ASSET = EQUITY + LIABILITY]
//            //        INCOME... [SALES/Receipts, COST OF SALES/Receipts, OTHER INCOME/Receipts], 
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC_Type", Val = "INC1", Desc = "Receipts" });
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC_Type", Val = "INC2", Desc = "Cost of Receipts" });
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC_Type", Val = "INC3", Desc = "Other Receipts" });

//            //        EXPENSE... [EXPENSES, TAX, DIVIDENDS], 
//            dlEXP_Types.Add(new DiscreteLookup() { Category = "EXP_Type", Val = "EXP1", Desc = "Expenses" });
//            //  dlEXP_Types.Add(new DiscreteLookup() { Category = "EXP_Type", Val = "EXP2", Desc = "Tax" });

//            //        FIXED ASSETS... [INVESTMENTS, OTHER FIXED ASSETS]
//            dlFAS_Types.Add(new DiscreteLookup() { Category = "FAS_Type", Val = "FAS1", Desc = "Investments" });
//            dlFAS_Types.Add(new DiscreteLookup() { Category = "FAS_Type", Val = "FAS2", Desc = "Other Fixed Assets" });

//            //        CURRENT ASSETS... [ACC RECEIVABLE, INVENTORY [Asset Register/Properties], CASH]
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS_Type", Val = "CAS1", Desc = "Account Receivable" });
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS_Type", Val = "CAS2", Desc = "Assets & Properties" });
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS_Type", Val = "CAS3", Desc = "Cash" });

//            //        CURRENT LIABILITIES... [ACC PAYABLE, OTHER CURR LIABILITIES]
//            dlCLB_Types.Add(new DiscreteLookup() { Category = "CLB_Type", Val = "CLB1", Desc = "Account Payable" });
//            dlCLB_Types.Add(new DiscreteLookup() { Category = "CLB_Type", Val = "CLB2", Desc = "Other Liabilities" });

//            //        EQUITY... [RETAINED EARNINGS(OTHER {STARTING CAPITAL}, INC.SURPLUS), LONG-TERM BORROWINGS, OTHER LONG-TERM LIABILITIES]'
//            dlELB_Types.Add(new DiscreteLookup() { Category = "ELB_Type", Val = "ELB1", Desc = "Retained Receipts" });
//            dlELB_Types.Add(new DiscreteLookup() { Category = "ELB_Type", Val = "ELB2", Desc = "Long-term Borrowings" });
//            dlELB_Types.Add(new DiscreteLookup() { Category = "ELB_Type", Val = "ELB3", Desc = "Other Long-term Liabilities" });

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

//        public static string GetTransModuleDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "OFFT": return "Offertory";
//                case "TITH": return "Tithes";
//                case "DONA": return "Donations" ;
//                case "PLDG": return "Pledges";
//                case "DISB": return "Disbursements";
//                case "WDIS": return "Welfare Distributions";
//                case "DEPR": return "Depreciation";
//                case "LEVY": return "Levies & Dues";
//                case "PROP": return "Assets/Properties";
//                case "SPLR": return "Suppliers";

//       default: return oCode;
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
//            if (oCurrChurchBody.RootChurchCode.Contains("--")) arr = oCurrChurchBody.RootChurchCode.Split("--");

//            if (arr.Length > 0)
//            {
//                var ancestorCode = oAncestorChurchBody.RootChurchCode;
//                var tempCode = oCurrChurchBody.RootChurchCode;
//                var k = arr.Length - 1;
//                for (var i = arr.Length - 1; i >= 0; i--)
//                {
//                    if (tempCode.Contains("--" + arr[i])) tempCode = tempCode.Replace("--" + arr[i], "");
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
//                if (!this.userAuthorized) return View(new TransControlAccountModel()); //retain view                  
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;  // if (oCurrChuBodyLogOn == null) return View(oCurrMdl);
//                if (oUserProfile == null) return View(new TransControlAccountModel());
//                //
//                var oCurrMdl = new TransControlAccountModel(); //TempData.Keep();  
//                                                               // int? oAppGloOwnId = null;
//                var oChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                //
//                int? oAppGloOwnId_Logged = null;
//                int? oChuBodyId_Logged = null;
//                int? oUserId_Logged = null;
//                if (oChuBodyLogOn != null)
//                {
//                    oAppGloOwnId_Logged = oChuBodyLogOn.AppGlobalOwnerId;
//                    oChuBodyId_Logged = oChuBodyLogOn.Id;
//                    oUserId_Logged = oUserProfile.Id;

//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBodyLogOn.Id; }
//                    if (oAppGloOwnId == null) { oAppGloOwnId = oChuBodyLogOn.AppGlobalOwnerId; }
//                    //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
//                    //
//                    // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                }


//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//                var currChurchMemberLogged = _context.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCurrMdl);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }


//                var AccountTypeMdl = (
//                   from t_tca in _context.TransControlAccount.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId)
//                  // from t_tm in this.dlTransModules.Where(c => c.Val == t_tca.TransModuleCode)
//                   from t_am in _context.AccountMaster.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId && c.Id == t_tca.ControlAccountId)
//                   from t_ac in _context.AccountCategory.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId && c.Id == t_am.AccountCategoryId)
//                   from t_am_con in _context.AccountMaster.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId && c.Id == t_tca.ContraAccountId).DefaultIfEmpty()
//                   from t_ac_con in _context.AccountCategory.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId && c.Id == t_am_con.AccountCategoryId).DefaultIfEmpty()

//                   select new TransControlAccountModel()
//                   {
//                       oTransControlAccount = t_tca,
//                       oAppGlolOwnId = t_tca.AppGlobalOwnerId,
//                       oAppGlolOwn = t_tca.AppGlobalOwner,
//                       oChurchBodyId = t_tca.ChurchBodyId,
//                       oChurchBody = t_tca.ChurchBody,
//                       //                       
//                       strControlAccount = t_am != null ? t_am.AccountName : "",
//                       strContraAccount = t_am_con != null ? t_am_con.AccountName : "",
//                       strTransModule = GetTransModuleDesc(t_tca.TransModuleCode), // t_tm != null ? t_tm.Desc : "",
//                       //
//                       strAccountTypeCode = t_ac != null ? t_ac.AccountType : "",
//                       AccountCategoryId = t_ac != null ? t_ac.Id : (int?)null,
//                       strContraAccountTypeCode = t_ac_con != null ? t_ac_con.AccountType : "",
//                       ContraAccountCategoryId = t_ac_con != null ? t_ac_con.Id : (int?)null,
//                       //
//                       strStatus = GetStatusDesc(t_tca.Status)
//                   }).ToList()
//                   .OrderByDescending(c => c.strTransModule).ThenBy(c => c.strControlAccount)
//                   .ToList();


//                oCurrMdl.lsTransControlAccountModel = AccountTypeMdl;

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
//                TempData.Keep();
//                return View(oCurrMdl);
//            }
//        }


//        [HttpGet]
//        public IActionResult AddOrEdit_TCA(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) //, int pageIndex = 1)
//        {   //int subSetIndex = 0,

//            var oCurrMdl = new TransControlAccountModel(); TempData.Keep();
//            if (setIndex == 0) return PartialView("_AddOrEdit_TCA", oCurrMdl);

//            var curr = _context.Currency.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.IsBaseCurrency == true).FirstOrDefault();
//            if (id == 0)
//            {
//                var oTransTCA = new TransControlAccount();

//                oTransTCA.AppGlobalOwnerId = oAppGloOwnId;
//                oTransTCA.ChurchBodyId = oCurrChuBodyId;
//                var accPer = _context.AccountPeriod.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.PeriodStatus == "O" && c.LongevityStatus == "C").FirstOrDefault();

//                //oTransTCA.AccountType = "INC";
//                //oTransTCA.ReportArea = "IS";
//                //oTransTCA.BalanceType = "CR";

//                oTransTCA.Status = "A";
//                oTransTCA.CreatedByUserId = oUserId_Logged;
//                oTransTCA.LastModByUserId = oUserId_Logged;

//                oCurrMdl.strStatus = GetStatusDesc(oTransTCA.Status);
//                //
//                oCurrMdl.oTransControlAccount = oTransTCA;
//            }

//            else
//            {
//                var AccountTypeMdl = (
//                   from t_tca in _context.TransControlAccount.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId)
//                       // from t_tm in this.dlTransModules.Where(c => c.Val == t_tca.TransModuleCode)
//                   from t_am in _context.AccountMaster.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId && c.Id == t_tca.ControlAccountId)
//                   from t_ac in _context.AccountCategory.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId && c.Id == t_am.AccountCategoryId)
//                   from t_am_con in _context.AccountMaster.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId && c.Id == t_tca.ContraAccountId).DefaultIfEmpty()
//                   from t_ac_con in _context.AccountCategory.AsNoTracking().Include(t => t.AppGlobalOwner)
//                                .Where(c => c.Id == t_tca.ChurchBodyId && c.AppGlobalOwnerId == t_tca.AppGlobalOwnerId && c.Id == t_am_con.AccountCategoryId).DefaultIfEmpty()

//                   select new TransControlAccountModel()
//                   {
//                       oTransControlAccount = t_tca,
//                       oAppGlolOwnId = t_tca.AppGlobalOwnerId,
//                       oAppGlolOwn = t_tca.AppGlobalOwner,
//                       oChurchBodyId = t_tca.ChurchBodyId,
//                       oChurchBody = t_tca.ChurchBody,
//                       //                       
//                       strControlAccount = t_am != null ? t_am.AccountName : "",
//                       strContraAccount = t_am_con != null ? t_am_con.AccountName : "",
//                       strTransModule = GetTransModuleDesc(t_tca.TransModuleCode), // t_tm != null ? t_tm.Desc : "",
//                       //
//                       strAccountTypeCode = t_ac != null ? t_ac.AccountType : "",
//                       AccountCategoryId = t_ac != null ? t_ac.Id : (int?)null,
//                       strContraAccountTypeCode = t_ac_con != null ? t_ac_con.AccountType : "",
//                       ContraAccountCategoryId = t_ac_con != null ? t_ac_con.Id : (int?)null,
//                       //
//                       strStatus = GetStatusDesc(t_tca.Status)
//                   })
//                   .FirstOrDefault();

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
//            oCurrMdl = this.populateLookups_TCA(oCurrMdl, oChurchBody);
//            // oCurrMdl.strCurrTask = "Church AccountType";

//            var _oCurrMdl = Newtonsoft.Json.JsonConvert.SerializeObject(oCurrMdl);
//            TempData["oVmCurrMod"] = _oCurrMdl; TempData.Keep();

//            return PartialView("_AddOrEdit_TCA", oCurrMdl);
//        }

//        private TransControlAccountModel populateLookups_TCA(TransControlAccountModel vmLkp, ChurchBody oCurrChuBody, int currAccId = 0)
//        {
//            if (vmLkp == null || oCurrChuBody == null) return vmLkp;
//            //
//            vmLkp.lkpTransModules = new List<SelectListItem>();
//            foreach (var dl in dlTransModules) { vmLkp.lkpTransModules.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            //vmLkp.lkpMainAccounts = new List<SelectListItem>();
//            //foreach (var dl in dlMainAccTypes ) { vmLkp.lkpMainAccounts.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpReportAreas = new List<SelectListItem>();
//            foreach (var dl in dlRptAreas) { vmLkp.lkpReportAreas.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpStatuses = new List<SelectListItem>();
//            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }


//            return vmLkp;
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddOrEdit_TCA(TransControlAccountModel vmMod)
//        {
//            TransControlAccount _oChanges = vmMod.oTransControlAccount;
//            var tempList = ""; // TempData["oVmCurrMod"] as string;
//            tempList = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : tempList;
//            if (!string.IsNullOrEmpty(tempList))
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<TransControlAccountModel>(tempList); // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as TransControlAccountModel : vmMod; TempData.Keep();

//            var oMdlData = vmMod.oTransControlAccount;
//            oMdlData.ChurchBody = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oTransControlAccount.AppGlobalOwnerId");
//                ModelState.Remove("oTransControlAccount.ChurchBodyId");
//                //ModelState.Remove("oUserProfile.AccountType");
//                //ModelState.Remove("oUserProfile.BalanceType");
//                //ModelState.Remove("oUserProfile.ReportArea");
//                //ModelState.Remove("oUserProfile.Status");

//                ModelState.Remove("oUserProfile.CreatedByUserId");
//                ModelState.Remove("oUserProfile.LastModByUserId");
//                ModelState.Remove("oUserId_Logged");

//                // ChurchBody == null 

//                //finally check error state...  // 100000-000   == Main, // 100000-001   == Sub
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

//                if (string.IsNullOrEmpty(_oChanges.TransModuleCode))
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify transaction module" });
//                }
//                if (_oChanges.ControlAccountId==null)
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify control account" });
//                }
//                if (_oChanges.ContraAccountId == null)
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify [at least one] contra account" });
//                }
                

//                //reset form for new item...
//                var _reset = _oChanges.Id == 0;

//                //
//                var tm = DateTime.Now;
//                _oChanges.LastMod = tm;
//                if (_oChanges.LastModByUserId == null) _oChanges.LastModByUserId = vmMod.oUserId_Logged;

//                //validate...
//                if (_oChanges.Id == 0)
//                {
//                    _oChanges.Created = tm;
//                    if (_oChanges.CreatedByUserId == null) _oChanges.CreatedByUserId = vmMod.oUserId_Logged;
//                    _context.Add(_oChanges);

//                    ViewBag.UserMsg = "Created transaction control account successfully.";
//                }
//                else
//                {
//                    //retain the pwd details... hidden fields 
//                    _context.Update(_oChanges);
//                    ViewBag.UserMsg = "Transaction control account  details updated successfully.";
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

 
//    }
//}
