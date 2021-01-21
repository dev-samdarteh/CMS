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
//    public class AccountMasterController : Controller
//    {
//        private readonly ChurchModelContext _context;
//        private readonly MSTR_DbContext _MSTRContext;
//        private readonly IWebHostEnvironment _hostingEnvironment;

//        private bool isCurrValid = false;
//        private List<UserSessionPrivilege> oUserLogIn_Priv = null;
         
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

//        public AccountMasterController(ChurchModelContext context, MSTR_DbContext ctx,
//             IWebHostEnvironment hostingEnvironment)
//        {
//            _context = context;
//            _MSTRContext = ctx;
//            _hostingEnvironment = hostingEnvironment;


//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
//            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "B", Desc = "Blocked" });
//            // dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "E", Desc = "Deactive" });

//            // INC, EXP, ELB, CAS, CLB, ELB
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "INC", Desc = "Income" });
//            dlMainAccTypes.Add(new DiscreteLookup() { Category = "MainAccType", Val = "EXP", Desc = "Expenditure" });
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


//            //        [ASSET = EQUITY + LIABILITY]
//            //        INCOME... [SALES/Receipts, COST OF SALES/Receipts, OTHER INCOME/Receipts], 
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC_Type", Val = "INC1", Desc = "Mainline Receipts" });
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC_Type", Val = "INC2", Desc = "Other Receipts" });
//            dlINC_Types.Add(new DiscreteLookup() { Category = "INC_Type", Val = "INC3", Desc = "Cost of Receipts" });

//            //        EXPENSE... [EXPENSES, TAX, DIVIDENDS], 
//            dlEXP_Types.Add(new DiscreteLookup() { Category = "EXP_Type", Val = "EXP1", Desc = "Expenses" });
//            //  dlEXP_Types.Add(new DiscreteLookup() { Category = "EXP_Type", Val = "EXP2", Desc = "Tax" });

//            //        FIXED ASSETS... [INVESTMENTS, OTHER FIXED ASSETS]
//            dlFAS_Types.Add(new DiscreteLookup() { Category = "FAS_Type", Val = "FAS1", Desc = "Investments" });
//            dlFAS_Types.Add(new DiscreteLookup() { Category = "FAS_Type", Val = "FAS2", Desc = "Other Fixed Assets" });

//            //        CURRENT ASSETS... [ACC RECEIVABLE, INVENTORY [Asset Register/Properties], CASH]
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS_Type", Val = "CAS1", Desc = "Cash and Bank" });
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS_Type", Val = "CAS2", Desc = "Account Receivable" });
//            dlCAS_Types.Add(new DiscreteLookup() { Category = "CAS_Type", Val = "CAS3", Desc = "Assets & Properties" });

//            //        CURRENT LIABILITIES... [ACC PAYABLE, OTHER CURR LIABILITIES]
//            dlCLB_Types.Add(new DiscreteLookup() { Category = "CLB_Type", Val = "CLB1", Desc = "Account Payable" });
//            dlCLB_Types.Add(new DiscreteLookup() { Category = "CLB_Type", Val = "CLB2", Desc = "Other Liabilities" });

//            //        EQUITY... [RETAINED EARNINGS(OTHER {STARTING CAPITAL}, INC.SURPLUS), LONG-TERM BORROWINGS, OTHER LONG-TERM LIABILITIES]'
//            dlELB_Types.Add(new DiscreteLookup() { Category = "ELB_Type", Val = "ELB1", Desc = "Retained Earnings" });  //Surplus or Deficits
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
//                if (!this.userAuthorized) return View(new AccountMasterModel()); //retain view                  
//                var oAccountMaster = oUserLogIn_Priv[0].UserProfile;  // if (oCurrChuBodyLogOn == null) return View(oCurrMdl);
//                if (oAccountMaster == null) return View(new AccountMasterModel());
//                //
//                var oCurrMdl = new AccountMasterModel(); //TempData.Keep();  
//                                                     // int? oAppGloOwnId = null;
//                var oChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
//                //
//                int? oAppGloOwnId_Logged = null;
//                int? oChuBodyId_Logged = null;
//                int? oUserId_Logged = null;
//                if (oChuBodyLogOn != null)
//                {
//                    oAppGloOwnId_Logged = oChuBodyLogOn.AppGlobalOwnerId;
//                    oChuBodyId_Logged = oChuBodyLogOn.Id;
//                    oUserId_Logged = oAccountMaster.Id;

//                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBodyLogOn.Id; }
//                    if (oAppGloOwnId == null) { oAppGloOwnId = oChuBodyLogOn.AppGlobalOwnerId; }
//                    //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
//                    //
//                    // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
//                }


//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//                var currChurchMemberLogged = _context.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oAccountMaster.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCurrMdl);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }


//                var AccountMasterMdl = (
//                   from t_am in _context.AccountMaster.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_am.ChurchBodyId && c.AppGlobalOwnerId == t_am.AppGlobalOwnerId)   //c.Id == oChurchBodyId && 
//                   from t_am_p in _context.AccountMaster.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_am.ParentAccountId).DefaultIfEmpty()
//                   from t_ac in _context.AccountCategory.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_am.AccountCategoryId)   //c.Id == oChurchBodyId && 
//                   //
//                   //from t_mat in this.dlMainAccTypes.ToList().Where(c => c.Val == t_at.AccountType)   //c.Id == oChurchBodyId && 
//                   //from t_ra in this.dlRptAreas.ToList().Where(c => c.Val == t_at.ReportArea)
//                   //from t_bt in this.dlBalTypes.ToList().Where(c => c.Val == t_at.BalanceType)

//                   select new AccountMasterModel()
//                   {
//                       oAccountMaster = t_am,
//                       oAppGlolOwnId = t_am.AppGlobalOwnerId,
//                       oAppGlolOwn = t_am.AppGlobalOwner,
//                       oChurchBodyId = t_am.ChurchBodyId,
//                       oChurchBody = t_am.ChurchBody,
//                       //     
//                       strAccountName = (!string.IsNullOrEmpty(t_am.AccountCode) ? t_am.AccountCode + " - " : "") + t_am.AccountName, //(t_am != null ? t_am.AccountNo + "-" : "") + (t_am != null ? t_am.AccountName : ""),
//                       strAccountCategory = t_am_p != null ? t_am_p.AccountName : "",
//                       strAccountTypeCategory = t_ac != null ? t_ac.CategoryDesc : "",
//                       // 
//                       strAccountType = GetMainAccTypeDesc(t_ac.AccountType),  // t_mat != null ? t_mat.Desc : "",  // strAccountCategory = t_am_cat != null ? t_am_cat.AccountName : "",
//                       strBalanceType = GetBalTypeDesc(GetAccBalCodeByAccountTypeCode(t_ac.AccountType)),  //t_bt != null ? t_bt.Desc  : "",   //strAccountTypeCategory = t_at != null ? t_at.CategoryDesc : "",
//                       strReportArea = GetRptTypeDesc(t_ac.ReportArea),  //t_ra != null ? t_ra.Desc : "",   // strAccountType = t_mat != null ? t_mat.Desc : "",
//                                                                          //
//                       strAccBalance = String.Format("{0:N2}", 0), // get the balance from current year transaction + bal b/f
//                       strAccBudget = String.Format("{0:N2}", t_am.BudgetEst), 
//                       strAccVariance = String.Format("{0:N2}", t_am.BudgetEst-0), 
//                       //
//                       strBalanceCode = t_ac != null ? t_ac.BalanceType : "", 
//                       //
//                       strStatus = GetStatusDesc(t_am.Status)
//                   }).ToList()
//                   .OrderByDescending(c => c.strAccountType).ThenBy(c=>c.strAccountCategory).ThenBy(c=>c.strAccountName)
//                   .ToList();


//                oCurrMdl.lsAccountMasterModel = AccountMasterMdl;

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
//                oCurrMdl.strCurrTask = "Chart of Accounts";

//                // TempData.Put("oVmCB_CNFG", oCurrMdl);
//                TempData.Keep();
//                return View(oCurrMdl);
//            }
//        }


//        [HttpGet]
//        public IActionResult AddOrEdit_AM(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) //, int pageIndex = 1)
//        {   //int subSetIndex = 0,

//            var oCurrMdl = new AccountMasterModel(); TempData.Keep();
//            if (setIndex == 0) return PartialView("_AddOrEdit_AM", oCurrMdl);
                        
//            if (id == 0)
//            {
//                var oTransACCM = new AccountMaster();

//                oTransACCM.AppGlobalOwnerId = oAppGloOwnId;
//                oTransACCM.ChurchBodyId = oCurrChuBodyId;
//               // var accPer = _context.AccountPeriod.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.PeriodStatus == "O" && c.LongevityStatus == "C").FirstOrDefault();
//               //
//                var nextCount = _context.AccountMaster.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId).Count() + 1;
//                oTransACCM.strMainAccountNo = String.Format("{0:0000.#}", nextCount);  // String.Format("{0:N6}", nextCount); 
//                oTransACCM.strSubAccountNo = "000"; 
//                oTransACCM.AccountCode = oTransACCM.strMainAccountNo + "_" + oTransACCM.strSubAccountNo; 
//                oTransACCM.Status = "A"; 
//                oTransACCM.CreatedByUserId = oUserId_Logged;
//                oTransACCM.LastModByUserId = oUserId_Logged;
                 
//                oCurrMdl.strStatus = GetStatusDesc(oTransACCM.Status);   
//                //
//                oCurrMdl.oAccountMaster = oTransACCM;
//            }

//            else
//            {
//                var AccountMasterMdl = (
//                   from t_am in _context.AccountMaster.AsNoTracking() //.Include(t => t.ChurchMember)
//                                .Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId)
//                   from t_cb in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_am.ChurchBodyId && c.AppGlobalOwnerId == t_am.AppGlobalOwnerId)   //c.Id == oChurchBodyId && 
//                   from t_am_p in _context.AccountMaster.AsNoTracking().Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_am.ParentAccountId).DefaultIfEmpty()
//                   from t_ac in _context.AccountCategory.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.ChurchBodyId == t_cb.Id && c.Id == t_am.AccountCategoryId)   //c.Id == oChurchBodyId && 
//                                                                                                                                                                                 //
//                                                                                                                                                                                 //from t_mat in this.dlMainAccTypes.ToList().Where(c => c.Val == t_at.AccountType)   //c.Id == oChurchBodyId && 
//                                                                                                                                                                                 //from t_ra in this.dlRptAreas.ToList().Where(c => c.Val == t_at.ReportArea)
//                                                                                                                                                                                //from t_bt in this.dlBalTypes.ToList().Where(c => c.Val == t_at.BalanceType)
//                   select new AccountMasterModel()
//                   {
//                       oAccountMaster = t_am,
//                       oAppGlolOwnId = t_am.AppGlobalOwnerId,
//                       oAppGlolOwn = t_am.AppGlobalOwner,
//                       oChurchBodyId = t_am.ChurchBodyId,
//                       oChurchBody = t_am.ChurchBody,
//                       //     
//                       strAccountName = (!string.IsNullOrEmpty(t_am.AccountCode) ? t_am.AccountCode + " - " : "") + t_am.AccountName, //(t_am != null ? t_am.AccountNo + "-" : "") + (t_am != null ? t_am.AccountName : ""),
//                       strAccountCategory = t_am_p != null ? t_am_p.AccountName : "",
//                       strAccountTypeCategory = t_ac != null ? t_ac.CategoryDesc : "",
//                       // 
//                       strAccountType = GetMainAccTypeDesc(t_ac.AccountType),  // t_mat != null ? t_mat.Desc : "",  // strAccountCategory = t_am_cat != null ? t_am_cat.AccountName : "",
//                       strBalanceType = GetBalTypeDesc(GetAccBalCodeByAccountTypeCode(t_ac.AccountType)),  //t_bt != null ? t_bt.Desc  : "",   //strAccountTypeCategory = t_at != null ? t_at.CategoryDesc : "",
//                       strReportArea = GetRptTypeDesc(t_ac.ReportArea),  //t_ra != null ? t_ra.Desc : "",   // strAccountType = t_mat != null ? t_mat.Desc : "",
//                                                                         //
//                       strAccBalance = String.Format("{0:N2}", 0), // get the balance from current year transaction + bal b/f
//                       strAccBudget = String.Format("{0:N2}", t_am.BudgetEst),
//                       strAccVariance = String.Format("{0:N2}", t_am.BudgetEst - 0),
//                       //
//                       strBalanceCode = t_ac != null ? t_ac.BalanceType : "",
//                       //
//                       strStatus = GetStatusDesc(t_am.Status)
//                   }).ToList()
//                   .FirstOrDefault(); 

//                oCurrMdl = AccountMasterMdl;
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

//            var curr = _context.Currency.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.IsBaseCurrency == true).FirstOrDefault();
//            oCurrMdl.strCurrency = curr != null ? curr.Acronym : "";

//            ChurchBody oChurchBody = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
//            oCurrMdl = this.populateLookups_AM(oCurrMdl, oChurchBody);
//            // oCurrMdl.strCurrTask = "Church AccountMaster";

//            var _oCurrMdl = Newtonsoft.Json.JsonConvert.SerializeObject(oCurrMdl);
//            TempData["oVmCurrMod"] = _oCurrMdl; TempData.Keep();

//            return PartialView("_AddOrEdit_AM", oCurrMdl);
//        }

//        private AccountMasterModel populateLookups_AM(AccountMasterModel vmLkp, ChurchBody oCurrChuBody, int currAccId = 0)
//        {
//            if (vmLkp == null || oCurrChuBody == null) return vmLkp;
//            //
//            //vmLkp.lkpAccountTypes = new List<SelectListItem>();
//            //foreach (var dl in dlMainAccTypes) { vmLkp.lkpAccountTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            //vmLkp.lkpBalanceTypes = new List<SelectListItem>();
//            //foreach (var dl in dlBalTypes) { vmLkp.lkpBalanceTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpReportAreas = new List<SelectListItem>();
//            foreach (var dl in dlRptAreas) { vmLkp.lkpReportAreas.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            vmLkp.lkpStatuses = new List<SelectListItem>();
//            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//            //vmLkp.lkpAccountTypeCategories = _context.AccountTypeCategory.Where(c => c.Status == "A")
//            //           .OrderBy(c => c.AccountType).ThenBy(c => c.CategoryCode) //.ToList()
//            //           .Select(c => new SelectListItem()
//            //           {
//            //               Value = c.Id.ToString(),
//            //               Text = c.CategoryDesc
//            //           })
//            //           // .OrderBy(c => c.Text)
//            //           .ToList();
//            //vmLkp.lkpAccountTypeCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//            //vmLkp.lkpGLAccounts = _context.AccountMaster.Where(c => c.Status == "A" && c.Id != currAccId)
//            //          .OrderBy(c => c.AccountCategoryId).ThenBy(c => c.AccountName) //.ToList()
//            //          .Select(c => new SelectListItem()
//            //          {
//            //              Value = c.Id.ToString(),
//            //              Text = (!string.IsNullOrEmpty(c.AccountCode) ? c.AccountCode + " - " : "") + c.AccountName
//            //          })
//            //          // .OrderBy(c => c.Text)
//            //          .ToList();
//            //vmLkp.lkpGLAccounts.Insert(0, new SelectListItem { Value = "", Text = "Select" });

//            vmLkp.lkpCurrencies = _context.Currency.OrderBy(c => c.ChurchBodyId == oCurrChuBody.Id && c.Status == "A").ToList()
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.Acronym
//                                           })
//                                           .OrderBy(c => c.Text)
//                                           .ToList();


//            return vmLkp;
//        }


//        public JsonResult GetAccountTypeByReportAreaCode(string oReportAreaCode) //, int? oCurrChuBodyId = null, int? oAppGlolOwnId = null)   
//        {
//            var oList = new List<SelectListItem>();
            
//            if (oReportAreaCode == "IS")
//            {
//                foreach (var dl in dlMainAccTypes)
//                {
//                    if (dl.Val.StartsWith("INC") || dl.Val.StartsWith("EXP")) 
//                    oList.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc });
//                }
//            }

//            else if (oReportAreaCode == "BS")
//            {
//                foreach (var dl in dlMainAccTypes)
//                {
//                    if (dl.Val.StartsWith("FAS") || dl.Val.StartsWith("CAS") || dl.Val.StartsWith("CLB") || dl.Val.StartsWith("ELB")) 
//                        oList.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc });
//                }
//            }            

//            oList.OrderBy(c => c.Value).OrderBy(c => c.Text);
//            // if (addEmpty) userPerms.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json(oList);
//        }

    

//        public JsonResult GetAccountTypeCategoryByAccountTypeCode(string oAccountTypeCode, int? oCurrChuBodyId = null, int? oAppGlolOwnId = null)   
//        {
//            var oList = new List<SelectListItem>();

//            oList = (
//                         from t_atc in _context.AccountTypeCategory.Where(c => c.AppGlobalOwnerId== oAppGlolOwnId && c.ChurchBodyId == oCurrChuBodyId && 
//                                        c.Status == "A" && c.AccountType== oAccountTypeCode)
//                         select t_atc
//                                ).OrderBy(c => c.AccountType).ThenBy(c => c.CategoryCode).ToList()
//                                 .Select(c => new SelectListItem()
//                                 {
//                                     Value = c.Id.ToString(),
//                                     Text = c.CategoryDesc
//                                 })
//                                 .OrderBy(c => c.Text)
//                                 .ToList();

//           // oList.OrderBy(c => c.Value).OrderBy(c => c.Text);
//            // if (addEmpty) userPerms.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json(oList);
//        }

//        public JsonResult GetAccountCategoryByFinCategoryCode(int? oFinCategoryId, int? currAccId = null, int? oCurrChuBodyId = null, int? oAppGlolOwnId = null)
//        {
//            var oList = new List<SelectListItem>();

//            oList = (
//                         from t_ac in _context.AccountMaster.Include(t=>t.AccountCategory)
//                                    .Where(c => c.AppGlobalOwnerId == oAppGlolOwnId && c.ChurchBodyId == oCurrChuBodyId &&
//                                        c.Status == "A" && c.Id != currAccId && c.AccountCategoryId == oFinCategoryId)
//                         select t_ac
//                                ).OrderBy(c => c.AccountCategory.CategoryCode).ThenBy(c => c.AccountName).ToList()
//                                 .Select(c => new SelectListItem()
//                                 {
//                                     Value = c.Id.ToString(),
//                                     Text = (!string.IsNullOrEmpty(c.AccountCode) ? c.AccountCode + " - " : "") + c.AccountName
//                                 })
//                                 .OrderBy(c => c.Text)
//                                 .ToList();
             

//            // oList.OrderBy(c => c.Value).OrderBy(c => c.Text);
//            // if (addEmpty) userPerms.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json(oList);
//        }


//        public JsonResult GetAccountBalByAccountTypeCode(string AccountTypeCode) //, int? oCurrChuBodyId = null, int? oAppGlolOwnId = null)   
//        {
//            var strBal = "";
//            if (AccountTypeCode.StartsWith("EXP") || AccountTypeCode.StartsWith("CAS") || AccountTypeCode.StartsWith("FAS"))
//                strBal = "DB";

//            else if (AccountTypeCode.StartsWith("INC") || AccountTypeCode.StartsWith("CLB") || AccountTypeCode.StartsWith("ELB") )
//                strBal = "CR";
             
//            // if (addEmpty) userPerms.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json (strBal);
//        }


//        public static string GetAccBalCodeByAccountTypeCode(string AccountTypeCode) //, int? oCurrChuBodyId = null, int? oAppGlolOwnId = null)   
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
//        public async Task<IActionResult> AddOrEdit_AM(AccountMasterModel vmMod)
//        {

//            AccountMaster _oChanges = vmMod.oAccountMaster;
//            var tempList = ""; // TempData["oVmCurrMod"] as string;
//            tempList = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : tempList;
//            if (!string.IsNullOrEmpty(tempList))
//                vmMod = Newtonsoft.Json.JsonConvert.DeserializeObject<AccountMasterModel>(tempList); //vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as AccountMasterModel : vmMod; TempData.Keep();

//            var oMdlData = vmMod.oAccountMaster;
//            oMdlData.ChurchBody = vmMod.oChurchBody;

//            try
//            {
//                ModelState.Remove("oAccountMaster.AppGlobalOwnerId");
//                ModelState.Remove("oAccountMaster.ChurchBodyId");
//                ModelState.Remove("oAccountMaster.AccountCategoryId");
//                ModelState.Remove("oAccountMaster.AccountTypeId"); 

//                ModelState.Remove("oAccountMaster.CreatedByUserId");
//                ModelState.Remove("oAccountMaster.LastModByUserId");
//                ModelState.Remove("oUserId_Logged");

//                // ChurchBody == null 

//                //finally check error state...  // 100000-000   == Main, // 100000-001   == Sub
//                if (ModelState.IsValid == false)
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again." });

//                _oChanges.AccountCode = _oChanges.strMainAccountNo + "_" + _oChanges.strSubAccountNo;
//                if (string.IsNullOrEmpty(_oChanges.AccountCode))
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify account code/number" });
//                } 

//                if (string.IsNullOrEmpty(_oChanges.AccountName)) 
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify account name" });
//                }
                
//                if (_oChanges.AccountCategoryId == null)  
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify the financial category." });
//                }
//                if (_oChanges.IsSubAccount == true  && _oChanges.ParentAccountId == null)
//                {
//                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify parent account for sub-account." });
//                }

//                if (_oChanges.IsSubAccount==true)  
//                {
//                    if (_oChanges.ParentAccountId != null)
//                    {
//                        if (oMdlData.ParentAccount == null) 
//                            oMdlData.ParentAccount = _context.AccountMaster.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId && c.Id == _oChanges.ParentAccountId).FirstOrDefault();
                        
//                        if (oMdlData.AccountCategory == null) 
//                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parent account specified could not be found. Please refresh and try again." });
//                        //
//                        //if (oMdlData.AccountCategory == null)
//                        //    oMdlData.AccountCategory = _context.AccountCategory.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId && c.Id == _oChanges.AccountCategoryId).FirstOrDefault();

//                        //if (oMdlData.AccountCategory == null)
//                        //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Account category specified could not be found. Please refresh and try again." });
//                        //
//                        if (string.IsNullOrEmpty(oMdlData.ParentAccount.AccountCode)) return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parent account code cannot be empty." });
//                        if (oMdlData.ParentAccount.AccountCode == _oChanges.AccountCode) return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Sub account code cannot be same as the main or parent account code." });

//                        var mainAccNo = oMdlData.AccountCode.Substring(0, oMdlData.ParentAccount.AccountCode.IndexOf("_"));  // 100000-000   == Main, // 100000-001   == Sub
//                        if (oMdlData.ParentAccount.AccountCode != mainAccNo) 
//                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Sub account code must be a suffix of the main or parent account code." });
//                        //
//                        if (oMdlData.ParentAccount.AccountCategoryId != _oChanges.AccountCategoryId) 
//                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Main account and sub account must belong to same account category." });
//                    } 
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
//                    //acc num must be unique
//                    var oAcc = _context.AccountMaster.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId && 
//                                        c.AccountCode.ToLower() == _oChanges.AccountCode.ToLower()).FirstOrDefault();
//                    if (oAcc != null)
//                    {
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Another account, " + oAcc.AccountName + " has been assigned with account # " + _oChanges.AccountCode + ". Specify unique account number." });
//                    }

//                    oAcc = _context.AccountMaster.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId &&
//                                            c.AccountCategoryId== _oChanges.AccountCategoryId && c.AccountName.ToLower() == _oChanges.AccountName.ToLower()).FirstOrDefault();
//                    if (oAcc != null)
//                    {
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Account name specified, " + _oChanges.AccountName + " already exists. Check the category and specify the correct account name." });
//                    }

//                    _oChanges.Created = tm;
//                    if (_oChanges.CreatedByUserId == null) _oChanges.CreatedByUserId = vmMod.oUserId_Logged;
//                    _context.Add(_oChanges);

//                    ViewBag.UserMsg = "Created GL account successfully.";
//                }
//                else
//                {
//                    //acc num must be unique
//                    var oAcc = _context.AccountMaster.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId &&
//                                        c.Id != _oChanges.Id && c.AccountCode.ToLower() == _oChanges.AccountCode.ToLower()).FirstOrDefault();
//                    if (oAcc != null)
//                    {
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Another account, " + oAcc.AccountName + " has been assigned with account # " + _oChanges.AccountCode + ". Specify unique account number." });
//                    }

//                    oAcc = _context.AccountMaster.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId &&
//                                            c.Id != _oChanges.Id && c.AccountCategoryId == _oChanges.AccountCategoryId && c.AccountName.ToLower() == _oChanges.AccountName.ToLower()).FirstOrDefault();
//                    if (oAcc != null)
//                    {
//                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Account name specified, " + _oChanges.AccountName + " already exists. Check the category and specify the correct account name." });
//                    }

//                    //retain the pwd details... hidden fields 
//                    _context.Update(_oChanges);
//                    ViewBag.UserMsg = "GL account details updated successfully.";
//                }

//                //save user profile first... 
//                await _context.SaveChangesAsync();


//                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
//                TempData["oVmCurr"] = _vmMod; TempData.Keep();


//                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg });
//            }

//            catch (Exception ex)
//            {
//                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving GL account details. Err: " + ex.Message });
//            }
//        }
//    }
//}
