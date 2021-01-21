//using System;
//using System.Collections.Generic;
//using System.Linq; 
//using Microsoft.AspNetCore.Mvc; 
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System.Globalization; 
//using RhemaCMS.Models.MSTRModels;
//using RhemaCMS.Models.CLNTModels;
//using RhemaCMS.Models.Adhoc; 
//using RhemaCMS.Models.ViewModels;

//namespace RhemaCMS.Controllers.con_ch_mem_reg
//{
//    public class MemberPaneController : Controller
//    {
//        private readonly MSTR_DbContext _masterDBContext;
//        private readonly ChurchModelContext _context;

//        private bool isCurrValid = false;
//        private List<UserSessionPrivilege> oUserLogIn_Priv = null;

//       // private UserProfileRole oUserProRoleLog = null;
//        private List<DiscreteLookup> dlStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlGenderType = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlMarriageType = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlMaritalStatus = new List<DiscreteLookup>(); 
//        private List<DiscreteLookup> dlCBDivOrgTypes = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlShareStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlOwnerStatus = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlGenStatuses = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlChurchType = new List<DiscreteLookup>();
//        private List<DiscreteLookup> dlChuWorkStat = new List<DiscreteLookup>();

//        public MemberPaneController(ChurchModelContext context)
//        {
//            _context = context;

//            dlStatus.Add(new DiscreteLookup() { Category = "EntityStatus", Val = "A", Desc = "Active" });
//            dlStatus.Add(new DiscreteLookup() { Category = "EntityStatus", Val = "D", Desc = "Deactive" });

//            dlGenderType.Add(new DiscreteLookup() { Category = "GenderType", Val = "M", Desc = "Male" });
//            dlGenderType.Add(new DiscreteLookup() { Category = "GenderType", Val = "F", Desc = "Female" });
//            dlGenderType.Add(new DiscreteLookup() { Category = "GenderType", Val = "O", Desc = "Other" });

//            ///Marriage registered Number may be required
//            dlMarriageType.Add(new DiscreteLookup() { Category = "MarrType", Val = "C", Desc = "Customary" });
//            dlMarriageType.Add(new DiscreteLookup() { Category = "MarrType", Val = "O", Desc = "Ordinance" });

//            dlMaritalStatus.Add(new DiscreteLookup() { Category = "MarrStatus", Val = "S", Desc = "Single" });
//            dlMaritalStatus.Add(new DiscreteLookup() { Category = "MarrStatus", Val = "M", Desc = "Married" });
//            dlMaritalStatus.Add(new DiscreteLookup() { Category = "MarrStatus", Val = "S", Desc = "Separated" });
//            dlMaritalStatus.Add(new DiscreteLookup() { Category = "MarrStatus", Val = "D", Desc = "Divorced" });
//            dlMaritalStatus.Add(new DiscreteLookup() { Category = "MarrStatus", Val = "W", Desc = "Widowed" });
//            dlMaritalStatus.Add(new DiscreteLookup() { Category = "MarrStatus", Val = "O", Desc = "Other" });


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
//            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CN", Desc = "Congregation" });

//            dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CU", Desc = "Church Unit" });
//            dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CH", Desc = "Congregation Hierarchy" });
//            dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CF", Desc = "Congregation" });

//            dlChuWorkStat.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "S", Desc = "Structure Only" });
//            dlChuWorkStat.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "O", Desc = "Operationalized" });

//        }



//        public static string GetConcatMemberName(string title, string fn, string mn, string ln, bool displayName = false)
//        {
//            if (displayName)
//                return ((((!string.IsNullOrEmpty(title) ? title : "") + ' ' + fn).Trim() + " " + mn).Trim() + " " + ln).Trim();
//            else
//                return (((fn + ' ' + mn).Trim() + " " + ln).Trim() + " " + (!string.IsNullOrEmpty(title) ? "(" + title + ")" : "")).Trim();
//        }

//        public static string GetStatusDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "A": return "Active";
//                case "D": return "Deactive";
//                case "P": return "Pending";
//                case "E": return "Expired";

//                case "M": return "Male";
//                case "F": return "Female";
//                case "X": return "Mixed";

//                case "O": return "Ordained";
//                case "L": return "Lay";



//                default: return oCode;
//            }
//        }

//        public static string GetWorkStatusDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "S": return "Structure Only";
//                case "O": return "Operationalized";

//                default: return oCode;
//            }
//        }

//        public static object GetChuOrgTypeDetail(string oCode, bool returnSetIndex)
//        {
//            switch (oCode)
//            {
//                case "CR": if (returnSetIndex) return 0; else return "Church Root";
//                case "GB": if (returnSetIndex) return 1; else return "Governing Body";
//                case "CO": if (returnSetIndex) return 2; else return "Church Office";
//                case "DP": if (returnSetIndex) return 3; else return "Church Department";
//                case "CG": if (returnSetIndex) return 4; else return "Church Grouping";
//                case "SC": if (returnSetIndex) return 5; else return "Standing Committee";
//                case "CE": if (returnSetIndex) return 6; else return "Church Enterprise";
//                case "TM": if (returnSetIndex) return 7; else return "Team";
//                //case "CP": if (returnSetIndex) return 8; else return "Church Position";
//                case "IB": if (returnSetIndex) return 9; else return "Independent Unit";
//                case "CN": if (returnSetIndex) return 10; else return "Congregation";

//                default: return oCode;
//            }
//        }

//        public static string GetVis_StatusDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "D": return "Dummy /Deactive";  
//                case "A": return "Active";   
//                case "P": return "Prospect";
//                case "N": return "New Convert";
//                case "M": return "Member";

//                default: return oCode;
//            }
//        }

//        public static string GetChuOrgTypeCode(int setIndex)
//        {
//            switch (setIndex)
//            {
//                case 0: return "CR";
//                case 1: return "GB";
//                case 2: return "CO";
//                case 3: return "DP";
//                case 4: return "CG";
//                case 5: return "SC";
//                case 6: return "CE";
//                case 7: return "TM";
//                // case 8: return "CP";
//                case 9: return "IB";
//                case 10: return "CN";
//                //
//                default: return "";
//            }
//        }

         

//        public static string  GetAdhocStatusDesc(string oCode)
//        {
//            switch (oCode)
//            {
//                case "N": return "Do not roll-down (share)";
//                case "C": return "Roll-down (share) for direct child congregation";
//                case "A": return "Roll-down (share) for all sub-congregations";
//                //case "S": return "Roll-down (share) for specific congregation"; //specify the church code

//                case "T": return "Tenure";
//                case "Y": return "Age (years)"; //years

//                case "O": return "Originated";
//                case "I": return "Inherited";

//                case "GA": return "General Activity";//GA-- General activ, ER-Event Role,  MC--Member Churchlife Activity related, EV-Church E-vent related
//                case "ER": return "Event Role";
//                case "MC": return "Member Church-life";
//                case "EV": return "Event";

//                default: return oCode;
//            }
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


//        public  IActionResult Index(int? oCurrChuBodyId = null, bool isCurrMember = true, char ? filterIndex = null, int? filterVal=null, string memClass = "C")
//        {
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                MemberProfileVM oMemProfiles = TempData.ContainsKey("oVmCurr") ? TempData["oVmCurr"] as MemberProfileVM : new MemberProfileVM();
//                // var oCBConVM = new MemberProfileVM(); TempData.Keep();

//                var oCBLogged = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                // if (oCurrChuBodyLogOn == null) return View(oCBConVM);
//                var oCurrChuBodyLogOn = _context.ChurchBody.Where(c => c.GlobalChurchCode == oCBLogged.GlobalChurchCode && c.SubscriptionKey == oCBLogged.SubscriptionKey).FirstOrDefault();
//                if (oCurrChuBodyLogOn == null) return View(oMemProfiles);
//                //
//                if (oCurrChuBodyId == null) { oCurrChuBodyId = oCurrChuBodyLogOn.Id; }
//                    else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...


//                // check permission for Core life...
//                if (!this.userAuthorized)
//                {
//                    //retain view
//                    return View(oMemProfiles);
//                }

//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//                // if (oUserProfile == null) return View(oUserProVM);

//                var currChurchMemberLogged = _context.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCBConVM);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }


//                oMemProfiles.CHCF_TotCong = 0;  //the requesting congregation is included... it's excluded when summing the subs
//                if (oCurrChuBodyLogOn.ChurchType == "CH")
//                {
//                    List<ChurchBody> congList = new List<ChurchBody>();
//                    congList = GetChurchUnits(oCurrChuBodyLogOn, oCurrChuBodyLogOn.Id, isCurrMember, memClass);
//                    //if (!(filterIndex == null || filterVal == null))
//                    //    congList = GetMemberProfiles(congList, filterIndex, filterVal);

//                    oMemProfiles.CHCF_TotMem = 0;
//                    oMemProfiles.CHCF_TotMaleMem = 0;
//                    oMemProfiles.CHCF_TotFemMem = 0;
//                    oMemProfiles.CHCF_TotOtherMem = 0;
//                    foreach (var cb in congList)
//                    {
//                        var qry = cb.CH_TotMemList; //GetCurrentMemberProfiles(cb); 
//                        var maleCount = qry.ToList().Where(c => c.Gender == "M").Count();   //oMemProfiles.Result.ToList().OfType<MemberProfile>()
//                        var femaleCount = qry.Where(c => c.Gender == "F").Count();
//                        var newCount = qry.Where(c => (DateTime)c.Created.Value.Date == DateTime.Now.Date).Count();

//                        cb.CH_TotMem = cb.CH_TotMemList.Count();
//                        cb.CH_TotNewMem = newCount;
//                        //
//                        oMemProfiles.CHCF_TotCong += cb.CH_TotSubUnits;
//                        oMemProfiles.CHCF_TotMem += qry.Count();
//                        oMemProfiles.CHCF_TotNewMem += newCount;
//                        oMemProfiles.CHCF_TotMaleMem += maleCount;
//                        oMemProfiles.CHCF_TotFemMem += femaleCount;
//                        oMemProfiles.CHCF_TotOtherMem += qry.Count - maleCount - femaleCount;
//                    }

//                    //oMemProfiles.oChurchBody = oCurrChuBodyLogOn; 
//                    oMemProfiles.CurrSubChurchUnits = congList;

//                }

//                else if (oCurrChuBodyLogOn.ChurchType == "CF")
//                {
//                    List<MemberProfileVM> qry = new List<MemberProfileVM>();
//                    //qry = ReturnMemberProfileList();
//                    // var qry = ReturnMemberProfileList(); 

//                    qry = ReturnMemberProfileList(oCurrChuBodyLogOn, isCurrMember, memClass);

//                    if (!(filterIndex == null || filterVal == null))
//                        qry = GetMemberProfiles(qry, filterIndex, filterVal);

//                    //var oMemProfiles = AppUtilties_Static.ToListAsync<MemberProfileVM>(qry);

//                    //set the counts... oMemProfiles.Result.ToList().OfType<MemberProfile>()
//                    var maleCount = qry.ToList().Where(c => c.oPersonalData.Gender == "M").Count();   //oMemProfiles.Result.ToList().OfType<MemberProfile>()
//                    var femaleCount = qry.Where(c => c.oPersonalData.Gender == "F").Count();
//                    var newCount = qry.Where(c => (DateTime)c.oPersonalData.Created.Value.Date == DateTime.Now.Date).Count();
//                    //ViewBag.MaleCount = maleCount;
//                    //ViewBag.FemaleCount = femaleCount;
//                    //ViewBag.OtherCount = qry.Count - maleCount - femaleCount;    //oMemProfiles.Result
//                    //
//                    oMemProfiles.CHCF_TotMem = qry.Count();
//                    oMemProfiles.CHCF_TotNewMem = newCount;
//                    oMemProfiles.CHCF_TotMaleMem = maleCount;
//                    oMemProfiles.CHCF_TotFemMem = femaleCount;
//                    oMemProfiles.CHCF_TotOtherMem = qry.Count - maleCount - femaleCount;

//                    // var oMemProfiles = AppUtilties_Static.ToListAsync<MemberProfileVM>(qry);

//                    // oMemProfiles.oMemberPaneFilters = LoadMemberListFilters(oCurrChuBody); //((int)oCurrChuBody.Id);
//                    // ViewBag.oMemProfiles_Init = qry;

//                    oMemProfiles.MemberProfiles = qry;
//                }

//                oMemProfiles.oChurchBody = oCurrChuBodyLogOn;  //current working CB
//                oMemProfiles.oLoggedChurchBody = oCurrChuBodyLogOn;    //logged by user;

                 

//               // oMemProfiles.lsTitheModel = TitheMdl;

//                //
//             //   oMemProfiles.oAppGlolOwnId = oAppGloOwnId;
//                oMemProfiles.oChurchBodyId = oCurrChuBodyId;
//                //
//              //  oMemProfiles.oUserId_Logged = oUserId_Logged;
//              //  oMemProfiles.oChurchBodyId_Logged = oChuBodyId_Logged;
//              //  oMemProfiles.oAppGloOwnId_Logged = oAppGloOwnId_Logged;
//                oMemProfiles.oMemberId_Logged = oCurrChuMemberId_LogOn;
//                //
//               // oMemProfiles.setIndex = setIndex;
//                //       

//                // oCurrMdl.strChurchLevelDown = "Assemblies";
//                oMemProfiles.strAppName = "RhemaCMS"; ViewBag.strAppName = oMemProfiles.strAppName;
//                oMemProfiles.strAppNameMod = "Admin Palette"; ViewBag.strAppNameMod = oMemProfiles.strAppNameMod;
//                oMemProfiles.strAppCurrUser = "Dan Abrokwa"; ViewBag.strAppCurrUser = oMemProfiles.strAppCurrUser;
//                oMemProfiles.strChurchType =  oMemProfiles.oChurchBody.ChurchType.ToUpper(); ViewBag.strChurchType = oMemProfiles.strChurchType;
//                // oHomeDash.strChuBodyDenomLogged = "Rhema Global Church"; ViewBag.strChuBodyDenomLogged = oHomeDash.strChuBodyDenomLogged;
//                //  oHomeDash.strChuBodyLogged = "Rhema Comm Chapel"; ViewBag.strChuBodyLogged = oHomeDash.strChuBodyLogged;

//                //           
//                ViewBag.strAppCurrUser_ChRole = "System Adminitrator";
//                ViewBag.strAppCurrUser_RoleCateg = "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
//                ViewBag.strAppCurrUser_PhotoFilename = "2020_dev_sam.jpg";
//                ViewBag.strAppCurrChu_LogoFilename = "14dc86a7-81ae-462c-b73e-4581bd4ee2b2_church-of-pentecost.png";
//                ViewBag.strUserSessionDura = "Logged: 10 minutes ago"; 
//                //
//                // oMemProfiles.strCurrTask = "Member Explorer"; 
//                oMemProfiles.strCurrTask = oMemProfiles.isCurrMemberQry ? "Members (Active)" : "Member History";


//                //
//                // TempData.Put("oVmCB_CNFG", oCBConVM);
//                TempData.Keep();
//                return View(oMemProfiles);
//            }
//        }

//        public IActionResult Index_Vis( string vStatus, int? reqChurchBodyId = null) //, char? filterIndex = null, int? filterVal = null)
//        {
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                MemberProfileVM oMemProfiles = TempData.ContainsKey("oVmCurr") ? TempData["oVmCurr"] as MemberProfileVM : new MemberProfileVM();
//                // var oCBConVM = new MemberProfileVM(); TempData.Keep();

//                var oCBLogged = oUserLogIn_Priv[0].ChurchBody;
//                var oUserProfile = oUserLogIn_Priv[0].UserProfile;
//                // if (oCurrChuBodyLogOn == null) return View(oCBConVM);
//                var oCurrChuBodyLogOn = _context.ChurchBody.Where(c => c.GlobalChurchCode == oCBLogged.GlobalChurchCode && c.SubscriptionKey == oCBLogged.SubscriptionKey).FirstOrDefault();
//                if (oCurrChuBodyLogOn == null) return View(oMemProfiles);
//                //
//                if (reqChurchBodyId == null) { reqChurchBodyId = oCurrChuBodyLogOn.Id; }
//                else if (reqChurchBodyId != oCurrChuBodyLogOn.Id) reqChurchBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

//                // check permission for Core life...
//                if (!this.userAuthorized)
//                {
//                    //retain view
//                    return View(oMemProfiles);
//                }

//                int? oCurrChuMemberId_LogOn = null;
//                ChurchMember oCurrChuMember_LogOn = null;

//                // if (oUserProfile == null) return View(oUserProVM);

//                var currChurchMemberLogged = _context.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyLogOn.Id && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
//                if (currChurchMemberLogged != null) //return View(oCBConVM);
//                {
//                    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
//                    oCurrChuMember_LogOn = currChurchMemberLogged;
//                }


//                //var UserLogIn_Priv = TempData.Get<List<UserSessionPrivilege>>("UserLogIn_oUserPrivCol");
//                //var checkUser = UserLogIn_Priv?.Count > 0;

//               // var oCurrChuBody = oUserLogIn_Priv[0].ChurchBody;
//               // if (oCurrChuBody == null) return View();
//              //  ViewBag.strChuBodyLogged = oCurrChuBody.AppGlobalOwner.OwnerName.ToUpper() + " - " + oCurrChuBody.Name;

//                ChurchBody oRequestedChurchBody;
//                if (reqChurchBodyId != null)
//                    oRequestedChurchBody = _context.ChurchBody.Where(c => c.AppGlobalOwnerId == oCurrChuBodyLogOn.AppGlobalOwnerId && c.Id == reqChurchBodyId).FirstOrDefault();
//                else
//                { reqChurchBodyId = oCurrChuBodyLogOn.Id; oRequestedChurchBody = oCurrChuBodyLogOn; }

//                // check permission for Core life...
//                if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null)
//                    return View();



//                ViewBag.strChuBodyLogged = oCurrChuBodyLogOn.AppGlobalOwner.OwnerName.ToUpper() + " - " + oCurrChuBodyLogOn.Name;
//                var oVisitors = new ChurchVisitorVM();

//                oVisitors.strCurrTaskStat = vStatus;
//                oVisitors.isActiveVisQry = oVisitors.strCurrTaskStat == "A";  //else history
//                oVisitors.strCurrTask = oVisitors.isActiveVisQry ? "Visitors Log (Active)" : "Visitors History";  // isMigrated == true ? "New Converts" : "New Coverts (Historic)" : isMigrated == true ? "Visitors" : "Visitors (Historic)";
                

//             //   oVisitors.isCurrMigrateQry = isMigrated;

//                oVisitors.CHCF_TotCong = 0;  //the requesting congregation is included... it's excluded when summing the subs
//                if (oRequestedChurchBody.ChurchType == "CH")
//                {
//                    List<ChurchBody> congList = new List<ChurchBody>();
//                    congList = GetChurchUnits_Vis(oRequestedChurchBody, oRequestedChurchBody.Id, vStatus);
//                    //if (!(filterIndex == null || filterVal == null))
//                    //    congList = GetMemberProfiles(congList, filterIndex, filterVal);

//                    oVisitors.CHCF_TotMem = 0;
//                    oVisitors.CHCF_TotMaleMem = 0;
//                    oVisitors.CHCF_TotFemMem = 0;
//                    oVisitors.CHCF_TotOtherMem = 0;
//                    foreach (var cb in congList)
//                    {
//                        var qry = cb.CH_TotMemList; //GetCurrentMemberProfiles(cb); 
//                        var maleCount = qry.ToList().Where(c => c.Gender == "M").Count();   //oVisitors.Result.ToList().OfType<MemberProfile>()
//                        var femaleCount = qry.Where(c => c.Gender == "F").Count();
//                        var newCount = qry.Where(c => (DateTime)c.Created.Value.Date == DateTime.Now.Date).Count();

//                        cb.CH_TotMem = cb.CH_TotMemList.Count();
//                        cb.CH_TotNewMem = newCount;
//                        //
//                        oVisitors.CHCF_TotCong += cb.CH_TotSubUnits;
//                        oVisitors.CHCF_TotMem += qry.Count();
//                        oVisitors.CHCF_TotNewMem += newCount;
//                        oVisitors.CHCF_TotMaleMem += maleCount;
//                        oVisitors.CHCF_TotFemMem += femaleCount;
//                        oVisitors.CHCF_TotOtherMem += qry.Count - maleCount - femaleCount;
//                    }

//                    //oVisitors.oChurchBody = oRequestedChurchBody; 
//                    oVisitors.CurrSubChurchUnits = congList;

//                }

//                else if (oRequestedChurchBody.ChurchType == "CF")
//                {
//                    List<ChurchVisitorVM> qry = new List<ChurchVisitorVM>();
//                    //qry = ReturnMemberProfileList();
//                    // var qry = ReturnMemberProfileList(); 

//                    qry = ReturnMemberProfileList_Vis(oRequestedChurchBody, vStatus );

//                    //if (!(filterIndex == null || filterVal == null))
//                    //    qry = GetMemberProfiles(qry, filterIndex, filterVal);

//                    //var oVisitors = AppUtilties_Static.ToListAsync<MemberProfileVM>(qry);

//                    //set the counts... oVisitors.Result.ToList().OfType<MemberProfile>()
//                    var maleCount = qry.ToList().Where(c => c.oChurchVisitor.Gender == "M").Count();   //oVisitors.Result.ToList().OfType<MemberProfile>()
//                    var femaleCount = qry.Where(c => c.oChurchVisitor.Gender == "F").Count();
//                    var newCount = qry.Where(c => (DateTime)c.oChurchVisitor.Created.Value.Date == DateTime.Now.Date).Count();

//                    //ViewBag.MaleCount = maleCount;
//                    //ViewBag.FemaleCount = femaleCount;
//                    //ViewBag.OtherCount = qry.Count - maleCount - femaleCount;    //oVisitors.Result

//                    //
//                    oVisitors.CHCF_TotMem = qry.Count();
//                    oVisitors.CHCF_TotNewMem = newCount;
//                    oVisitors.CHCF_TotMaleMem = maleCount;
//                    oVisitors.CHCF_TotFemMem = femaleCount;
//                    oVisitors.CHCF_TotOtherMem = qry.Count - maleCount - femaleCount;

//                    // var oVisitors = AppUtilties_Static.ToListAsync<MemberProfileVM>(qry);

//                    // oVisitors.oMemberPaneFilters = LoadMemberListFilters(oCurrChuBody); //((int)oCurrChuBody.Id);
//                    // ViewBag.oVisitors_Init = qry;

//                    oVisitors.ChurchVisitorList = qry;
//                }

//                oVisitors.oChurchBody = oRequestedChurchBody;  //current working CB
//                oVisitors.oLoggedChurchBody = oCurrChuBodyLogOn;    //logged by user
//              //  oVisitors.strCurrTaskStat = vStatus;    //logged by user
//                                                                  //logged by user

//                //oVisitors.lkpChurchLevels = _context.ChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == oCurrChuBody.AppGlobalOwnerId && c.LevelIndex >= oCurrChuBody.ChurchLevel.LevelIndex
//                //                           ).OrderBy(c => c.LevelIndex).ThenBy(c => c.CustomName).ThenBy(c => c.Name)
//                //                          .Select(c => new SelectListItem()
//                //                          {
//                //                              Value = c.Id.ToString(),
//                //                              Text = !string.IsNullOrEmpty(c.CustomName) ? c.CustomName : c.Name
//                //                          })
//                //                          // .OrderBy(c => c.Text)
//                //                          .ToList();


//                //oVisitors.lkpCongregations = _context.ChurchBody.AsNoTracking().Where(c => c.AppGlobalOwnerId == oCurrChuBody.AppGlobalOwnerId && c.ChurchType=="CF" || c.ChurchType == "CH" &&
//                //                                c.ParentChurchBodyId== oCurrChuBody.Id 
//                //                          ).OrderBy(c => c.Name)//.ThenBy(c => c.CustomName).ThenBy(c => c.Name)
//                //                         .Select(c => new SelectListItem()
//                //                         {
//                //                             Value = c.Id.ToString(),
//                //                             Text = !string.IsNullOrEmpty(c.Name) ? c.Name : c.Name
//                //                         })
//                //                         // .OrderBy(c => c.Text)
//                //                         .ToList();


//                TempData.Keep();
//                return View(oVisitors);

//                //ViewBag.oMemberList = GetMemberList((int)oUserProRoleLog.UserProfile.ChurchBodyId);
//                //AddOrEditPersData();

//                //TempData.Keep();
//                //return View();
//            }
//        }

//        public JsonResult GetChurchUnitTypeByOrgType(int? oCurrChuBodyId, string orgType, bool addEmpty = false)
//        {
//            var oChurchBody = _context.ChurchBody.Include(t => t.AppGlobalOwner).Include(t => t.ChurchLevel).Where(c => c.Id == oCurrChuBodyId).FirstOrDefault();
//            if (oChurchBody == null) return null;

//            var ChurchUnitTypes = _context.ChurchDivisionType.Include(t => t.OwnedByChurchBody)
//                            .Where(c => c.OwnedByChurchBody.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId && !string.IsNullOrEmpty(c.Description) &&
//                                            c.OrganisationType == orgType &&
//                             (c.OwnedByChurchBodyId == oChurchBody.Id ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody)))
//                    ).OrderBy(c => c.OrganisationType).ThenBy(c => c.Description).ToList()
//                 .Select(c => new SelectListItem()
//                 {
//                     Value = c.Id.ToString(),
//                     Text = c.Description
//                 })
//                 .ToList();

//            if (addEmpty) ChurchUnitTypes.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            return Json(ChurchUnitTypes);
//        }


//        //public JsonResult GetChurchUnitTypeByChurchLevel(int? oCurrChuBodyId, int? chuLevelId, bool addEmpty = false)
//        //{
//        //var oCurrChuBody = _context.ChurchBody.Include(t => t.AppGlobalOwner).Include(t => t.ChurchLevel).Where(c => c.Id == oCurrChuBodyId).FirstOrDefault();
//        //if (oCurrChuBody == null) return null;

//        //oMemProfiles.lkpCongregations = _context.ChurchBody.AsNoTracking().Where(c => c.AppGlobalOwnerId == oCurrChuBody.AppGlobalOwnerId && c.ChurchType == "CF" || c.ChurchType == "CH" &&
//        //                                            c.        c.ParentChurchBodyId == oCurrChuBody.Id
//        //                              ).OrderBy(c => c.Name)//.ThenBy(c => c.CustomName).ThenBy(c => c.Name)
//        //                             .Select(c => new SelectListItem()
//        //                             {
//        //                                 Value = c.Id.ToString(),
//        //                                 Text = !string.IsNullOrEmpty(c.Name) ? c.Name : c.Name
//        //                             })
//        //                             // .OrderBy(c => c.Text)
//        //                             .ToList();


//        //var oChurchBody = _context.ChurchBody.Include(t => t.AppGlobalOwner).Include(t => t.ChurchLevel).Where(c => c.Id == oCurrChuBodyId).FirstOrDefault();
//        //if (oChurchBody == null) return null;

//        //var ChurchUnitTypes = _context.ChurchDivisionType.Include(t => t.OwnedByChurchBody)
//        //                .Where(c => c.OwnedByChurchBody.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId && !string.IsNullOrEmpty(c.Description) &&
//        //                                c.OrganisationType == orgType &&
//        //                 (c.OwnedByChurchBodyId == oChurchBody.Id ||
//        //                 (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//        //                 (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody)))
//        //        ).OrderBy(c => c.OrganisationType).ThenBy(c => c.Description).ToList()
//        //     .Select(c => new SelectListItem()
//        //     {
//        //         Value = c.Id.ToString(),
//        //         Text = c.Description
//        //     })
//        //     .ToList();

//        //if (addEmpty) ChurchUnitTypes.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//        //return Json(ChurchUnitTypes);
//        //}



//        public IActionResult FilterMemberProfiles(List<MemberProfileVM> memProList, char filter, int? filterVal)
//        {
//            SetUserLogged();
//            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//            else
//            {
//                var oCBLogged = oUserLogIn_Priv[0].ChurchBody;
//                if (oCBLogged == null) return View();

//                // check permission for Core life...
//                if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null)
//                    return View();

//                var oCurrChuBodyLogOn = _context.ChurchBody.Where(c => c.GlobalChurchCode == oCBLogged.GlobalChurchCode).FirstOrDefault();
//                if (oCurrChuBodyLogOn == null) return View();

//                var qry1 = ReturnMemberProfileList(oCurrChuBodyLogOn);
//                var qry = GetMemberProfiles(qry1, filter, filterVal);

//                //var oMemProfiles = AppUtilties_Static.ToListAsync<MemberProfileVM>(qry);

//                //set the counts... oMemProfiles.Result.ToList().OfType<MemberProfile>()
//                var maleCount = qry.ToList().Where(c => c.oPersonalData.Gender == "M").Count();   //oMemProfiles.Result.ToList().OfType<MemberProfile>()
//                var femaleCount = qry.Where(c => c.oPersonalData.Gender == "F").Count();

//                ViewBag.MaleCount = maleCount;
//                ViewBag.FemaleCount = femaleCount;
//                ViewBag.OtherCount = qry.Count - maleCount - femaleCount;    //oMemProfiles.Result

//                // var oMemProfiles = AppUtilties_Static.ToListAsync<MemberProfileVM>(qry);
//                var oMemProfiles = new MemberProfileVM();
//                oMemProfiles.MemberProfiles = qry;

//                ViewBag.oMemberPaneFilters = LoadMemberListFilters(oCurrChuBodyLogOn); // (int)oCurrChuBody.Id);
//               // ViewBag.oMemProfiles_Init = qry;

//                TempData.Keep();
//              //  return View(await oMemProfiles);
//                return View(oMemProfiles);

//            }
//        }
      
        
        
//        public List<ChurchBody> GetChurchUnits(ChurchBody parentChurchBody, int? oParentId, bool isCurrMem = true, string memClass = "C")  //, string organisationType 
//        {  //U-Church Unit, D-Church Department, G-Church Grouping, O-Office, C-Standing Committee [permanent, others to go to the specific church unit]
//           //Where(x =>  (x.Id == oChurchBody.Id || x.ParentChurchBodyId == oChurchBody.Id || x.RootChurchCode.Contains(oChurchBody.RootChurchCode)))
//            if (parentChurchBody == null) parentChurchBody = _context.ChurchBody.Include(t => t.AppGlobalOwner).Where(c => c.Id == oParentId).FirstOrDefault();
//            if (parentChurchBody == null) return new List<ChurchBody>();

//           var churchUnits = (
//                   from t_cb in _context.ChurchBody.AsNoTracking()//.Include(t => t.AppGlobalOwner).Include(t => t.ChurchLevel)//.Include(t => t.ChurchDivisionType)
//                   //.Include(t => t.ParentChurchBody).Include(t => t.SubChurchUnits)
//                        .Where(c => c.AppGlobalOwnerId == parentChurchBody.AppGlobalOwnerId && (c.OrganisationType == "GB" || c.OrganisationType == "CN") && (c.ChurchType == "CH" || c.ChurchType == "CF") &&
//                                    ((oParentId == null && c.ParentChurchBodyId == null) || (oParentId != null && c.ParentChurchBodyId == oParentId)) //&&
//                                                                                                                                                      //(c.OwnedByChurchBodyId == churchBody.Id ||
//                                                                                                                                                      //(c.OwnedByChurchBodyId != churchBody.Id && c.SharingStatus == "C" && c.ParentChurchBodyId == oParentId) ||
//                                                                                                                                                      //(c.OwnedByChurchBodyId != churchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, churchBody)))
//                        )
//                   //join t_pcdx in _context.ChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner)
//                   //   .Where(c => c.AppGlobalOwnerId == churchBody.AppGlobalOwnerId) on t_cb.ParentChurchBodyId equals t_pcdx.Id into ab
//                   //from t_pcd in ab.DefaultIfEmpty()
//                   //join t_cdtx in _context.ChurchUnitType.AsNoTracking().Include(t => t.OwnedByChurchBody)
//                   //  .Where(c => c.OwnedByChurchBody.AppGlobalOwnerId == churchBody.AppGlobalOwnerId) on t_cb.ChurchUnitTypeId equals t_cdtx.Id into abc
//                   //from t_cdt in abc.DefaultIfEmpty()
//                   //join t_clx in _context.ChurchLevel.AsNoTracking()
//                   //  .Where(c => c.AppGlobalOwnerId == churchBody.AppGlobalOwnerId) on t_cb.ChurchLevelId equals t_clx.Id into abcd
//                   //from t_cl in abcd.DefaultIfEmpty()
//                   select new ChurchBody()
//                   {
//                       Id = t_cb.Id,
//                       Name = t_cb.Name,
//                       AppGlobalOwnerId = t_cb.AppGlobalOwnerId,
//                       AppGlobalOwner = t_cb.AppGlobalOwner,
//                       ParentChurchBodyId = t_cb.ParentChurchBodyId,
//                       ParentChurchBody = t_cb.ParentChurchBody,
//                       OrganisationType = t_cb.OrganisationType,
//                      // ChurchUnitTypeId = t_cb.ChurchUnitTypeId,
//                       OwnedByChurchBodyId = t_cb.OwnedByChurchBodyId,

//                      // CH_TotSubUnits = _context.ChurchBody.Where(y => y.ParentChurchBodyId == t_cb.Id && (y.OrganisationType == "GB" || y.OrganisationType == "CN") && (y.ChurchType=="CH" || y.ChurchType == "CF") && y.Status=="A").Count(), // Is_Activated is for COMMIT tasks  t_cb.SubChurchUnits,
//                       CH_TotSubUnits = _context.ChurchBody
//                                                    .Where(y => y.AppGlobalOwnerId== t_cb.AppGlobalOwnerId && (y.OrganisationType == "GB" || y.OrganisationType == "CN") && (y.ChurchType=="CH" || y.ChurchType == "CF") && y.Status=="A" && y.Id != t_cb.Id &&
//                                                           (  y.ParentChurchBodyId == t_cb.Id || y.RootChurchCode.Contains(t_cb.RootChurchCode))
//                                                            ).Count(),  
                        
//                       //CH_TotMem = _context.ChurchMember//.Include(t => t.ChurchBody)
//                       //    .Where(y => y.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && y.IsActivated == true && y.AffiliateStatus == "C" &&
//                       //            y.ChurchBody.OrganisationType == "CN" && y.ChurchBody.ChurchType == "CF" &&
//                       //           (y.ChurchBodyId == t_cb.Id || y.ChurchBody.ParentChurchBodyId == t_cb.Id || y.ChurchBody.RootChurchCode.Contains(t_cb.RootChurchCode)))
//                       //    .Count(),

//                       CH_TotMemList = (
//                                            from t_cb_1 in _context.ChurchBody.AsNoTracking().Where(x => x.AppGlobalOwnerId == t_cb.AppGlobalOwnerId &&
//                                                   x.OrganisationType == "CN" && x.ChurchType == "CF" &&  //(x.OrganisationType == "GB" || x.OrganisationType == "CN") && (x.ChurchType == "CH" || x.ChurchType == "CF") &&
//                                                   (x.Id == t_cb.Id || x.ParentChurchBodyId == t_cb.Id || x.RootChurchCode.Contains(t_cb.RootChurchCode)))
//                                            from t_cm in _context.ChurchMember.AsNoTracking().Where(x => x.AppGlobalOwnerId == t_cb_1.AppGlobalOwnerId && x.ChurchBodyId == t_cb_1.Id &&
//                                                           x.IsActivated == true && x.MemberClass == memClass)

//                                                //select * from MemberStatus where ChurchBodyId=1029  and iscurrent=1 and ChurchMemStatusId in (1,2, 3) 
//                                            from t_ms in _context.MemberStatus.AsNoTracking().Include(t => t.ChurchMemStatus).Where(x => x.ChurchBodyId == t_cm.ChurchBodyId && x.ChurchMemberId == t_cm.Id && x.IsCurrent == true) //.DefaultIfEmpty()
//                                            from t_cms in _context.ChurchMemStatus.AsNoTracking().Where(x => x.Id == t_ms.ChurchMemStatusId && x.Available == isCurrMem)
//                                            select t_cm
//                                        ).ToList(),

//                       //CH_TotMemList = (from t_cb_1 in _context.ChurchBody.AsNoTracking().Where(y => y.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && (y.OrganisationType == "GB" || y.OrganisationType == "CN") && (y.ChurchType == "CH" || y.ChurchType == "CF") &&
//                       //                           (y.Id == t_cb.Id || y.ParentChurchBodyId == t_cb.Id || (!string.IsNullOrEmpty(t_cb.RootChurchCode) && y.RootChurchCode.Contains(t_cb.RootChurchCode))))
//                       //                join t_cmx in _context.ChurchMember.AsNoTracking() 
//                       //                                     .Where(y => y.IsActivated == true && y.MemberClass == "C") on t_cb_1.Id equals t_cmx.ChurchBodyId into _tcmx 
//                       //                 from t_cm in _tcmx //.DefaultIfEmpty()
//                       //                 join t_msx in _context.MemberStatus.AsNoTracking()
//                       //                                    .Where(y => y.IsCurrent == true && y.ChurchMemStatus.Available == isCurrMem) on t_cm.Id equals t_msx.ChurchMemberId into _tmsx

//                       //                 from t_ms in _tmsx //.DefaultIfEmpty()

//                       //               //  where(t_cm.ChurchBodyId == t_cb_1.Id && t_ms.ChurchBodyId == t_cb_1.Id) 
//                       //                 select t_cm).Distinct().ToList (),


//                       //CH_TotMemList = (
//                       //                      from t_cb_1 in _context.ChurchBody.AsNoTracking().Where(z => z.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && (z.OrganisationType == "GB" || z.OrganisationType == "CN") && (z.ChurchType == "CH" || z.ChurchType == "CF")) &&
//                       //                           (z.Id == t_cb.Id || z.ParentChurchBodyId == t_cb.Id || z.RootChurchCode.Contains(t_cb.RootChurchCode)))
//                       //                      from t_cm in _context.ChurchMember.AsNoTracking()
//                       //                                     .Where(z => z.AppGlobalOwnerId == t_cb_1.AppGlobalOwnerId && z.ChurchBodyId == t_cb_1.Id &&
//                       //                                     z.IsActivated == true && z.MemberClass == "C")
//                       //                      from t_ms in _context.MemberStatus.AsNoTracking().Where(z => z.ChurchMemberId == t_cm.Id && z.ChurchBodyId == t_cb_1.Id &&
//                       //                                     z.IsCurrent == true && z.ChurchMemStatus.Available == true && z.ChurchMemStatus.Deceased == false)
//                       //                                     .DefaultIfEmpty()

//                       //                      select t_cm)
//                       //                                    //.OrderBy(x => z.strMemberFullName) 
//                       //                                    .ToList(),

//                       // CH_TotMemList = GetCurrentMemberProfiles(t_cb, _context),
//                       CH_TotMaleMem = 0,
//                       CH_TotFemMem = 0,
//                       CH_TotOtherMem = 0,
//                       oCH_InChargeMLR = null,
//                       strCH_InCharge = "",

//                       ////
//                       //strOrgType = GetChuOrgTypeDetail(t_cb.OrganisationType, false).ToString(),
//                       //strChurchUnitType = t_cdt != null ? t_cdt.Description : "",
//                       //strParentUnit = t_pcd != null ? t_pcd.Name : "",
//                       //strChurchLevel = t_cl != null ? (!string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name) : "",
//                       //intChurchLevel_Index = t_cl != null ? t_cl.LevelIndex : 0
//                   }
//                   )
//                   .OrderBy(c => c.intChurchLevel_Index).ThenBy(c => c.OrganisationType).ThenBy(c => c.strChurchUnitType).ThenBy(c => c.strParentUnit).ThenBy(c => c.Name)
//                   .ToList();

//            return churchUnits;
//        }

//        public List<ChurchBody> GetChurchUnits_Vis(ChurchBody parentChurchBody, int? oParentId, string vStatus)  //, string organisationType 
//        {  //U-Church Unit, D-Church Department, G-Church Grouping, O-Office, C-Standing Committee [permanent, others to go to the specific church unit]
//           //Where(x =>  (x.Id == oChurchBody.Id || x.ParentChurchBodyId == oChurchBody.Id || x.RootChurchCode.Contains(oChurchBody.RootChurchCode)))
//            if (parentChurchBody == null) parentChurchBody = _context.ChurchBody.Include(t => t.AppGlobalOwner).Where(c => c.Id == oParentId).FirstOrDefault();
//            if (parentChurchBody == null) return new List<ChurchBody>();

//            var churchUnits = (
//                    from t_cb in _context.ChurchBody.AsNoTracking()//.Include(t => t.AppGlobalOwner).Include(t => t.ChurchLevel)//.Include(t => t.ChurchDivisionType)
//                                                                   //.Include(t => t.ParentChurchBody).Include(t => t.SubChurchUnits)
//                         .Where(c => c.AppGlobalOwnerId == parentChurchBody.AppGlobalOwnerId && (c.OrganisationType == "GB" || c.OrganisationType == "CN") && (c.ChurchType == "CH" || c.ChurchType == "CF") &&
//                                     ((oParentId == null && c.ParentChurchBodyId == null) || (oParentId != null && c.ParentChurchBodyId == oParentId)) //&&
//                                                                                                                                                       //(c.OwnedByChurchBodyId == churchBody.Id ||
//                                                                                                                                                       //(c.OwnedByChurchBodyId != churchBody.Id && c.SharingStatus == "C" && c.ParentChurchBodyId == oParentId) ||
//                                                                                                                                                       //(c.OwnedByChurchBodyId != churchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, churchBody)))
//                         ) 
//                   select new ChurchBody()
//                    {
//                        Id = t_cb.Id,
//                        Name = t_cb.Name,
//                        AppGlobalOwnerId = t_cb.AppGlobalOwnerId,
//                        AppGlobalOwner = t_cb.AppGlobalOwner,
//                        ParentChurchBodyId = t_cb.ParentChurchBodyId,
//                        ParentChurchBody = t_cb.ParentChurchBody,
//                        OrganisationType = t_cb.OrganisationType,
//                     //   ChurchUnitTypeId = t_cb.ChurchUnitTypeId,
//                        OwnedByChurchBodyId = t_cb.OwnedByChurchBodyId,

//                       // CH_TotSubUnits = _context.ChurchBody.Where(y => y.ParentChurchBodyId == t_cb.Id && (y.OrganisationType == "GB" || y.OrganisationType == "CN") && (y.ChurchType=="CH" || y.ChurchType == "CF") && y.Status=="A").Count(), // Is_Activated is for COMMIT tasks  t_cb.SubChurchUnits,
//                       CH_TotSubUnits = _context.ChurchBody
//                                                     .Where(y => y.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && (y.OrganisationType == "GB" || y.OrganisationType == "CN") && (y.ChurchType == "CH" || y.ChurchType == "CF") && y.Status == "A" && y.Id != t_cb.Id &&
//                                                            (y.ParentChurchBodyId == t_cb.Id || y.RootChurchCode.Contains(t_cb.RootChurchCode))
//                                                             ).Count(),
                        
//                       CH_TotMemList_Visitor = (
//                                             from t_cb_1 in _context.ChurchBody.AsNoTracking().Where(x => x.AppGlobalOwnerId == t_cb.AppGlobalOwnerId &&
//                                                    x.OrganisationType == "CN" && x.ChurchType == "CF" &&  //(x.OrganisationType == "GB" || x.OrganisationType == "CN") && (x.ChurchType == "CH" || x.ChurchType == "CF") &&
//                                                    (x.Id == t_cb.Id || x.ParentChurchBodyId == t_cb.Id || x.RootChurchCode.Contains(t_cb.RootChurchCode)))
//                                             from t_cm in _context.ChurchVisitor.AsNoTracking().Where(x => x.ChurchBody.AppGlobalOwnerId == t_cb_1.AppGlobalOwnerId && x.ChurchBodyId == t_cb_1.Id &&
//                                                    ((vStatus != null && x.Vstatus == vStatus) || vStatus == null))   //         x.IsActivated == true &&

//                                                 //select * from MemberStatus where ChurchBodyId=1029  and iscurrent=1 and ChurchMemStatusId in (1,2, 3) 
//                                                 //from t_ms in _context.MemberStatus.AsNoTracking().Include(t => t.ChurchMemStatus).Where(x => x.ChurchBodyId == t_cm.ChurchBodyId && x.ChurchMemberId == t_cm.Id && x.IsCurrent == true) //.DefaultIfEmpty()
//                                                 //from t_cms in _context.ChurchMemStatus.AsNoTracking().Where(x => x.Id == t_ms.ChurchMemStatusId && x.Available == isCurrMem)
//                                             select t_cm
//                                         ).ToList(), 
//                       CH_TotMaleMem = 0,
//                        CH_TotFemMem = 0,
//                        CH_TotOtherMem = 0,
//                        oCH_InChargeMLR = null,
//                        strCH_InCharge = "",
                         
//                   }
//                    )
//                    .OrderBy(c => c.intChurchLevel_Index).ThenBy(c => c.OrganisationType).ThenBy(c => c.strChurchUnitType).ThenBy(c => c.strParentUnit).ThenBy(c => c.Name)
//                    .ToList();

//            return churchUnits;
//        }

//        public List<MemberProfileVM> ReturnMemberProfileList(ChurchBody oChurchBody, bool? isCurrMem = true, string memClass = "C")
//        {
//            //Get member profile list      ... current members ... available

//            var oMemProfileVMList = (
//                             from t_cb in _context.ChurchBody.AsNoTracking().Where(x => x.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId && 
//                                                   (x.OrganisationType == "GB" || x.OrganisationType == "CN") && (x.ChurchType == "CH" || x.ChurchType == "CF") &&
//                                                   (x.Id == oChurchBody.Id || x.ParentChurchBodyId == oChurchBody.Id || x.RootChurchCode.Contains(oChurchBody.RootChurchCode)))
//                             from t_cm in _context.ChurchMember.AsNoTracking().Where(x => x.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && x.ChurchBodyId == t_cb.Id &&
//                                            x.IsActivated == true && x.MemberClass == memClass)

//                                 //select * from MemberStatus where ChurchBodyId=1029  and iscurrent=1 and ChurchMemStatusId in (1,2, 3) 
//                             from t_ms in _context.MemberStatus.AsNoTracking().Include(t=>t.ChurchMemStatus).Where(x => x.ChurchBodyId == t_cm.ChurchBodyId && x.ChurchMemberId == t_cm.Id && x.IsCurrent == true) //.DefaultIfEmpty()
//                             from t_cms in _context.ChurchMemStatus.AsNoTracking().Where(x => x.Id == t_ms.ChurchMemStatusId && x.Available == isCurrMem) // == true .DefaultIfEmpty() //on t_msx. equals cms.Id into _tms   // .Where(x => x.ChurchMemberId == t_cm.Id && x.ChurchBodyId == t_cb.Id &&

//                                 //x.ChurchBodyId == t_cm.ChurchBodyId && 
//                             from t_mr in _context.MemberRank.AsNoTracking().Include(t=>t.ChurchRank).Where(x => x.ChurchMemberId == t_cm.Id &&
//                                            x.IsCurrentRank == true).DefaultIfEmpty()
//                                 //from t_mp in _context.MemberPosition.AsNoTracking().Where(x => x.ChurchBodyId == t_cm.ChurchBodyId && x.ChurchMemberId == t_cm.Id &&  
//                                 //                x.CurrentPos == true).DefaultIfEmpty()
//                             from t_mcug in _context.MemberChurchUnit.AsNoTracking().Where(x => x.ChurchMemberId == t_cm.Id &&  
//                                            x.ChurchUnit.IsUnitGenerational == true && x.IsCurrUnit == true).DefaultIfEmpty()
                                             

//                                     //from t_cm in _context.ChurchMember.AsNoTracking()
//                                     //                   //.Include(t => t.ChurchBody).ThenInclude(t=>t.AppGlobalOwner)
//                                     //                  // .Include(t => t.MemberType).Include(t => t.ContactInfo).Include(t => t.Nationality).Include(t => t.HometownRegion).Include(t => t.MotherTongue).Include(t => t.IDType)
//                                     //                       .Where (x=>x.ChurchBodyId== oChurchBody.Id && x.IsActivated == true) //on t_ms.ChurchMemberId equals t_cmx.Id into abc
//                                     // join t_cbx in _context.ChurchBody.AsNoTracking() on t_cm.ChurchBodyId equals t_cbx.Id into _cb
//                                     // from t_cb in _cb   //.DefaultIfEmpty()
//                                     // join t_msx in _context.MemberStatus.AsNoTracking().Include(t => t.ChurchMemStatus)
//                                     //                                   .Where(x => x.IsCurrent == true) on t_cm.Id equals t_msx.ChurchMemberId into _tms //on g.ChurchBodyId equals b.Id into ab  a.Id equals g.ChurchMemberId into abcdefg  //  from t6 in abcdefg.DefaultIfEmpty()
//                                     // from t_ms in _tms.DefaultIfEmpty() //  c.ChurchBodyId == oChurchBodyId && c.IsCurrent == true 
//                                     //                   //join t_cbx in _context.ChurchBody on t_ms.ChurchBodyId equals t_cbx.Id into a
//                                     //                   //from t_cb in a
//                                     //                   //join t_agox in _context.AppGlobalOwner on t_cb.AppGlobalOwnerId equals t_agox.Id into ab
//                                     //                   //from t_ago in ab    //.DefaultIfEmpty()
//                                     //                   //join t_cmx in _context.ChurchMember.Include(t => t.ContactInfo).Include(t => t.Nationality).Include(t => t.HometownRegion).Include(t => t.MotherTongue).Include(t => t.IDType) on t_ms.ChurchMemberId equals t_cmx.Id into abc
//                                     //                   //from t_cm in abc   //.DefaultIfEmpty()  //.Where(c => c.Id == id)  

//                                     //     //join t_mclx in _context.MemberChurchLife.AsNoTracking() on t_cm.Id equals t_mclx.ChurchMemberId into _mcl
//                                     //     //from t_mcl in _mcl   //.DefaultIfEmpty()
//                                     // join t_mrx in _context.MemberRank.AsNoTracking().Where(x => x.IsCurrentRank == true ) on t_cm.Id equals t_mrx.ChurchMemberId into _tmr
//                                     // from t_mr in _tmr.DefaultIfEmpty()
//                                     // join t_mpx in _context.MemberPosition.AsNoTracking().Where(x => x.CurrentPos == true ) on t_cm.Id equals t_mpx.ChurchMemberId into _tmp
//                                     // from t_mp in _tmp.DefaultIfEmpty()
//                                     // join t_mcugx in _context.MemberChurchUnit.AsNoTracking().Include(t => t.ChurchUnit).Where(x => x.ChurchUnit.IsUnitGenerational == true && x.IsCurrUnit == true)
//                                     //                   .Take(1).DefaultIfEmpty() on t_cm.Id equals t_mcugx.ChurchMemberId into _tmcug //  age bracket/group: children, youth, adults as defined in config. OR auto-assign
//                                     // from t_mcug in _tmcug.DefaultIfEmpty() 

//                                     //    // && _context.ChurchGroup.Where(y => y.Id==x.Id && y.Generational == true).Count() > 0
//                                     //    //join f in _context.ChurchGroup.Where(x => x.Generational == true) on t4.SectorId equals f.Id into abcdef  //&& _context.ChurchGroup.Find(x.Id) !=null
//                                     //    //from t5 in abcde.DefaultIfEmpty()

//                                     //join t_mcumx in _context.MemberChurchUnit.AsNoTracking()  //.Include(t => t.ChurchUnit)
//                                     //          .Where(x => x.IsMainArea == true).Take(1).DefaultIfEmpty() on t_cm.Id equals t_mcumx.ChurchMemberId into _tmcum  // main service or department... can be service /gen
//                                     //from t_mcum in _tmcum.DefaultIfEmpty()

//                                     //join t_mlrx in _context.MemberChurchRole.AsNoTracking()   //.Include(t => t.LeaderRole)  .Include(t => t.ChurchUnit)
//                                     //                                   .Where(x => x.IsCoreRole == true) on t_cm.Id equals t_mlrx.ChurchMemberId into _tmlr   //&& x.CurrServing && x.LeaderRole.AuthorityIndex == _context.MemberChurchRole.Where(y => y.ChurchMemberId == x.ChurchMemberId).Min(y => y.LeaderRole.AuthorityIndex)
//                                     //from t_mlr in _tmlr.DefaultIfEmpty()
//                                     //join t_rcux in _context.ChurchBody.AsNoTracking().Take(1)   // .Include(t => t.ChurchUnit).Where(x => x.IsCoreRole == true).Take(1) 
//                                     //                                      on t_mlr.ChurchUnitId equals t_rcux.Id into _trcu   //&& x.CurrServing && x.LeaderRole.AuthorityIndex == _context.MemberChurchRole.Where(y => y.ChurchMemberId == x.ChurchMemberId).Min(y => y.LeaderRole.AuthorityIndex)
//                                     //from t_rcu in _trcu.DefaultIfEmpty()

//                                     //join t_mcx in _context.MemberContact.AsNoTracking().Include(t => t.InternalContact).ThenInclude(t => t.ContactInfo).Include(t => t.ExternalContact).ThenInclude(t => t.ContactInfo).Where(x => x.CurrentContact == true).Take(1) on t_cm.Id equals t_mcx.ChurchMemberId into _tmc
//                                     //from t_mc in _tmc.DefaultIfEmpty()
//                                     //join t_mlsx in _context.MemberLanguageSpoken.AsNoTracking().Where(x => x.PrimaryLanguage == true) on t_cm.Id equals t_mlsx.ChurchMemberId into _tmls
//                                     //from t_mls in _tmls.DefaultIfEmpty()
//                                     //join t_mehx in _context.MemberEducHistory.AsNoTracking().Where(x => x.InstitutionType.EduLevelIndex == _context.MemberEducHistory.Where(y => y.ChurchMemberId == x.ChurchMemberId).Min(y => y.InstitutionType.EduLevelIndex)).Take(1) on t_cm.Id equals t_mehx.ChurchMemberId into _tmeh
//                                     //from t_meh in _tmeh.DefaultIfEmpty()

//                                 select new MemberProfileVM
//                             {
//                                     //cong of member's membership... not his work or role     // //core dept or sector == group, cttee or the local church (e.g. Min) ...  
//                                     //oAppGlobalOwner = t_cm.ChurchBody != null ? t_cm.ChurchBody.AppGlobalOwner != null ? t_cm.ChurchBody.AppGlobalOwner : null : null,
//                                     oChurchBody = t_cb != null ? t_cb : null,
//                                 strCongregation = t_cb != null ? t_cb.Name : "",

//                                     //peers stuff of mem
//                                     oPersonalData = t_cm,
//                                     //  intMemberAge = t_cm.DateOfBirth != null ? (new ChurchMembersController(_context, null)).CalcAge((DateTime)t_cm.DateOfBirth, DateTime.Now) : 0,
//                                     //  strMemberAge = t_cm.DateOfBirth != null ? (new ChurchMembersController(_context, null)).GetAgeString((DateTime)t_cm.DateOfBirth, DateTime.Now) : "",
//                                     // strJoined = GetMemberAvailability((DateTime)t_mcl.Joined, (DateTime)t_mcl.Departed, t_ms.ChurchMemStatus),

//                                     //strJoined = t_cm.Enrolled != null ? String.Format("{0:d-MMM-yyyy}", (DateTime)t_cm.Enrolled) : "N/A",
//                                     //strLongevity = (new ChurchMembersController(_context, null)).GetMemberLongevity(t_mcl.Joined, t_mcl.Departed, t_ms.ChurchMemStatus),

//                                     //strLongevity =(new ChurchMembersController(_context, null)).GetMemberAvailability(t_cm.Enrolled, t_cm.Departed, null),
//                                     // strLongevity = (new ChurchMembersController(_context, null)).GetMemberAvailability(t_mcl.Joined, t_mcl.Departed, t_ms.ChurchMemStatus),
//                                     //(t_mcl.Joined != null ? (new ChurchMembersController(_context, null)).CalcAge((DateTime)t_mcl.Joined, DateTime.Now) : 0).ToString() + " years",

//                                     // strBirthday = t_cm.DateOfBirth == null ? "" : (new DateTime(t_cm.DateOfBirth.Value.Year, t_cm.DateOfBirth.Value.Month, t_cm.DateOfBirth.Value.Day)).ToString("m", CultureInfo.CreateSpecificCulture("en-us")),
//                                     // strBirthdayTag = (new ChurchMembersController(_context, null)).GetBirthdayTag(t_cm.DateOfBirth, t_ms.ChurchMemStatus),

//                                     strMemberFullName = (((t_cm.FirstName + ' ' + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() + " " + (!string.IsNullOrEmpty(t_cm.Title) ? "(" + t_cm.Title + ")" : "")).Trim(),
//                                     // strMemberDisplayName = ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim(),
                                   
//                                     // strMemberType = t_cm.ChurchMemberType != null ? t_cm.ChurchMemberType.Description : "",
//                                     strGender = GetStatusDesc(t_cm.Gender),  // t_cm.Gender == "M" ? "Male" : t_cm.Gender == "F" ? "Female" : "Other",
//                                                                          // strMaritalStat = (new MemberProfileVM()).ReturnMaritalStatusDesc(t_cm.MaritalStatus),
//                                                                          //  strNativity = (t_cm.NationalityId != null ? t_cm.Nationality.Name : "N/A") + ". Hails from " + (t_cm.Hometown != null ? t_cm.Hometown : "") + (t_cm.HometownRegion != null ? ", " + (!string.IsNullOrEmpty(t_cm.HometownRegion.RegCode) ? t_cm.HometownRegion.RegCode.ToUpper() : t_cm.HometownRegion.Name).Trim() : ""),     ///(t_cm.HometownRegion != null ? (t_cm.HometownRegion.RegCode.Length > 0 ? t_cm.HometownRegion.RegCode.ToUpper() : t_cm.HometownRegion.Name.ToUpper()) : "")
//                                                                          // strLangProfiency = t_mls.LanguageSpoken == null ? "None" : "Speaks " + t_mls.LanguageSpoken.Name + " (" + (new MemberProfileVM()).ReturnLangProficiency((int)t_mls.ProficiencyLevel) + ")",
//                                                                          // oPrimaryLanguage = t_mls.LanguageSpoken != null ? t_mls.LanguageSpoken : null,
//                                                                          // PrimaryLanguageId = t_mls.LanguageSpokenId != null ? t_mls.LanguageSpokenId : null,

//                                     //oMemberChurchlife = t_mcl,  // != null ? t_mcl : null, //  t9 ?? new MemberChurchLife(),
//                                     //oMemberLocAddr = t_cm.ContactInfo != null ? t_cm.ContactInfo : null,  //t_cm.ContactInfo ?? new ContactInfo(),
//                                     //oMemberContact = t_mc, //!= null ? t_mc : null,  // t8 ?? new MemberContact(),
//                                     //oEducationLevel = t_meh != null ? t_meh.InstitutionType : null,
//                                     //EducationLevel = (from a in _context.InstitutionType.Where(x => x.EduLevelIndex == 
//                                     //                  _context.MemberEducHistory.Where(y => y.ChurchMemberId == t_cm.Id).Min(y => y.InstitutionType.EduLevelIndex)) select a ).Take(1).ToList(),
//                                     //  EducationLevelId = t_meh != null ? t_meh.InstitutionTypeId : null,
//                                     // PhotoPath = t_cm.PhotoUrl,

//                                     ////member's identity in cong
//                                     //MemberId = t_cm.Id,
//                                     //MemberCode = t_cm.MemberCode,

//                                     // //oMemberRank = t_mr ?? new MemberRank(),
//                                     //strChurchRank = t_mr.ChurchRank != null ? t_mr.ChurchRank.RankName : "None",
//                                     //oChurchRank = t_mr != null ? t_mr.ChurchRank : new ChurchRank(),
//                                     //RankId = t_mr != null ? t_mr.ChurchRankId : 0,

//                                     ////oChurchPosition = t_mp != null ? t_mp.ChurchPosition : new ChurchPosition(),
//                                     // strChurchPos = t_mp.ChurchPosition != null ? t_mp.ChurchPosition.PositionName : "None",
//                                     //oMemberPos = t_mp ?? new MemberPosition(), 
//                                     //PositionId = t_mp != null ? t_mp.ChurchPositionId : 0,

//                                     strMemberStat = t_ms.ChurchMemStatus != null ? t_ms.ChurchMemStatus.Name : "Unassigned",
                                    
//                                     oMemberStatus =  t_ms , //?? new MemberStatus(),

//                                                         //oChurchMemStatus = t_ms != null ? t_ms.ChurchMemStatus : new ChurchMemStatus(),
//                                                         //ChurchMemStatusId = t_ms != null ? t_ms.ChurchMemStatusId : 0,

//                                                         //what the member does in church
//                                                         //   strCoreArea = t_mcum.ChurchUnit != null ? t_mcum.ChurchUnit.Name : "None",  //service group
//                                                         //oMemberChurchUnit = t_mcug != null ? t_mcug : null,
//                                     strAgeGroup = t_mcug.ChurchUnit != null ? t_mcug.ChurchUnit.Name : "None",   /// age group
//                                     //strCoreRole = t_mlr.LeaderRole != null ? t_mlr.LeaderRole.RoleName : "None",   // core role
//                                     //strMemChuRoleDesc = (t_mlr.LeaderRole != null ? t_mlr.LeaderRole.RoleName : "None") != "None" && t_rcu != null ? t_mlr.LeaderRole.RoleName + ", " + t_rcu.Name : t_mlr.LeaderRole.RoleName


//                                     //&& x.Departed != null      
//                                     //String.Format("{0:dd-MMM-yyyy}", dt)
//                                     //DateTime thisDate = new DateTime(2008, 3, 15);
//                                     //CultureInfo culture = new CultureInfo("pt-BR");
//                                     //Console.WriteLine(thisDate.ToString("d", culture));  // Displays 15/3/2008

//                                     //DateTime date1 = new DateTime(2008, 4, 10);
//                                     //Console.WriteLine(date1.ToString("D",
//                                     //                 CultureInfo.CreateSpecificCulture("en-US")));
//                                     // Displays Thursday, April 10, 2008    

//                                     //intMemberAge = a.DateOfBirth != null ? (int)((DateTime.Now.Date - a.DateOfBirth.Value.Date).TotalDays / 365) : -1, 

//                                     //DateTime date1 = new DateTime(2008, 4, 10, 6, 30, 0);
//                                     //Console.WriteLine(date1.ToString("m",  CultureInfo.CreateSpecificCulture("en-us")));
//                                     // Displays April 10  ....  10 April .. "ms-MY"

//                                 })
//                                //.Distinct()
//                                .OrderBy(x => x.strMemberFullName)
//                                .ToList();


//            return oMemProfileVMList;
//        }

//        public List<ChurchVisitorVM> ReturnMemberProfileList_Vis(ChurchBody oChurchBody, string vStatus)
//        {
//            //Get member profile list      ... current members ... available

//            var oVisitorVMList = (
//                             from t_cb in _context.ChurchBody.AsNoTracking().Where(x => x.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId &&
//                                                   (x.OrganisationType == "GB" || x.OrganisationType == "CN") && (x.ChurchType == "CH" || x.ChurchType == "CF") &&
//                                                   (x.Id == oChurchBody.Id || x.ParentChurchBodyId == oChurchBody.Id || x.RootChurchCode.Contains(oChurchBody.RootChurchCode)))
//                             from t_cm in _context.ChurchVisitor.AsNoTracking().Where(x => x.ChurchBody.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && x.ChurchBodyId == t_cb.Id &&
//                                                    ((vStatus!=null && x.Vstatus==vStatus) || vStatus==null))   //         x.IsActivated == true &&

//                             select new ChurchVisitorVM
//                             { 
//                                 oChurchBody = t_cb != null ? t_cb : null,
//                                 strCongregation = t_cb != null ? t_cb.Name : "", 
//                                 oChurchVisitor = t_cm, 

//                                 //peers stuff of mem                                
//                                 strMemberFullName = GetConcatMemberName(t_cm.Title, t_cm.FirstName, t_cm.MiddleName, t_cm.LastName, false), //(((t_cm.FirstName + ' ' + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() + " " + (!string.IsNullOrEmpty(t_cm.Title) ? "(" + t_cm.Title + ")" : "")).Trim(),
//                                 strGender = GetStatusDesc(t_cm.Gender) ,
//                                 strCtryLocation = t_cm.Location + (!string.IsNullOrEmpty(t_cm.Location) && t_cm.Nationality != null ? ", " : "") + (t_cm.Nationality != null ? t_cm.Nationality.Name : ""),
//                                 strVisitorType = t_cm.ChurchVisitorType != null ? t_cm.ChurchVisitorType.Description : "",
//                                 strFirstVisitDate = t_cm.FirstVisitDate != null ? DateTime.Parse(t_cm.FirstVisitDate.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
//                                 strVStatus = GetVis_StatusDesc(t_cm.Vstatus)
//                             })
//                                //.Distinct()
//                                .OrderBy(x => x.oChurchVisitor.FirstVisitDate).ThenBy(x => x.strMemberFullName)
//                                .ToList();

//            return oVisitorVMList;
//        }

//        //public List<MemberProfileVM> ReturnUnavailableMemberProfileList(ChurchBody oChurchBody)
//        //{
//        //    //Get member profile list      ... current members ... available

//        //    var oMemProfileVMList = (
//        //                     from t_cb in _context.ChurchBody.AsNoTracking().Where(x => x.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId &&
//        //                                           (x.OrganisationType == "GB" || x.OrganisationType == "CN") && (x.ChurchType == "CH" || x.ChurchType == "CF") &&
//        //                                           (x.Id == oChurchBody.Id || x.ParentChurchBodyId == oChurchBody.Id || x.RootChurchCode.Contains(oChurchBody.RootChurchCode)))
//        //                     from t_cm in _context.ChurchMember.AsNoTracking().Where(x => x.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && x.ChurchBodyId == t_cb.Id &&
//        //                                    x.IsActivated == true && x.MemberClass == "C")

//        //                         //select * from MemberStatus where ChurchBodyId=1029  and iscurrent=1 and ChurchMemStatusId in (1,2, 3) 
//        //                     from t_ms in _context.MemberStatus.AsNoTracking().Include(t => t.ChurchMemStatus).Where(x => x.ChurchMemberId == t_cm.Id && x.IsCurrent == true) //.DefaultIfEmpty()
//        //                     from t_cms in _context.ChurchMemStatus.AsNoTracking().Where(x => x.Id == t_ms.ChurchMemStatusId && x.Available==false) // == true .DefaultIfEmpty() //on t_msx. equals cms.Id into _tms   // .Where(x => x.ChurchMemberId == t_cm.Id && x.ChurchBodyId == t_cb.Id &&

//        //                         //x.ChurchBodyId == t_cm.ChurchBodyId && 
//        //                     from t_mr in _context.MemberRank.AsNoTracking().Include(t => t.ChurchRank).Where(x => x.ChurchMemberId == t_cm.Id &&
//        //                                      x.IsCurrentRank == true).DefaultIfEmpty()
//        //                         //from t_mp in _context.MemberPosition.AsNoTracking().Where(x => x.ChurchBodyId == t_cm.ChurchBodyId && x.ChurchMemberId == t_cm.Id &&  
//        //                         //                x.CurrentPos == true).DefaultIfEmpty()
//        //                     from t_mcug in _context.MemberChurchUnit.AsNoTracking().Where(x => x.ChurchMemberId == t_cm.Id &&
//        //                                    x.ChurchUnit.IsUnitGenerational == true && x.IsCurrUnit == true).DefaultIfEmpty()


//        //                     select new MemberProfileVM
//        //                     {
//        //                         //cong of member's membership... not his work or role     // //core dept or sector == group, cttee or the local church (e.g. Min) ...  
//        //                         //oAppGlobalOwner = t_cm.ChurchBody != null ? t_cm.ChurchBody.AppGlobalOwner != null ? t_cm.ChurchBody.AppGlobalOwner : null : null,
//        //                         oChurchBody = t_cb != null ? t_cb : null,
//        //                         strCongregation = t_cb != null ? t_cb.Name : "",

//        //                         //peers stuff of mem
//        //                         oPersonalData = t_cm,
//        //                         //  intMemberAge = t_cm.DateOfBirth != null ? (new ChurchMembersController(_context, null)).CalcAge((DateTime)t_cm.DateOfBirth, DateTime.Now) : 0,
//        //                         //  strMemberAge = t_cm.DateOfBirth != null ? (new ChurchMembersController(_context, null)).GetAgeString((DateTime)t_cm.DateOfBirth, DateTime.Now) : "",
//        //                         // strJoined = GetMemberAvailability((DateTime)t_mcl.Joined, (DateTime)t_mcl.Departed, t_ms.ChurchMemStatus),

//        //                         //strJoined = t_cm.Enrolled != null ? String.Format("{0:d-MMM-yyyy}", (DateTime)t_cm.Enrolled) : "N/A",
//        //                         //strLongevity = (new ChurchMembersController(_context, null)).GetMemberLongevity(t_mcl.Joined, t_mcl.Departed, t_ms.ChurchMemStatus),

//        //                         //strLongevity =(new ChurchMembersController(_context, null)).GetMemberAvailability(t_cm.Enrolled, t_cm.Departed, null),
//        //                         // strLongevity = (new ChurchMembersController(_context, null)).GetMemberAvailability(t_mcl.Joined, t_mcl.Departed, t_ms.ChurchMemStatus),
//        //                         //(t_mcl.Joined != null ? (new ChurchMembersController(_context, null)).CalcAge((DateTime)t_mcl.Joined, DateTime.Now) : 0).ToString() + " years",

//        //                         // strBirthday = t_cm.DateOfBirth == null ? "" : (new DateTime(t_cm.DateOfBirth.Value.Year, t_cm.DateOfBirth.Value.Month, t_cm.DateOfBirth.Value.Day)).ToString("m", CultureInfo.CreateSpecificCulture("en-us")),
//        //                         // strBirthdayTag = (new ChurchMembersController(_context, null)).GetBirthdayTag(t_cm.DateOfBirth, t_ms.ChurchMemStatus),

//        //                         strMemberFullName = (((t_cm.FirstName + ' ' + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() + " " + (!string.IsNullOrEmpty(t_cm.Title) ? "(" + t_cm.Title + ")" : "")).Trim(),
//        //                         // strMemberDisplayName = ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim(),
//        //                         strMemberType = t_cm.MemberType != null ? t_cm.MemberType.Description : "",
//        //                         strGender = GetStatusDesc(t_cm.Gender),  // t_cm.Gender == "M" ? "Male" : t_cm.Gender == "F" ? "Female" : "Other",
//        //                                                                  // strMaritalStat = (new MemberProfileVM()).ReturnMaritalStatusDesc(t_cm.MaritalStatus),
//        //                                                                  //  strNativity = (t_cm.NationalityId != null ? t_cm.Nationality.Name : "N/A") + ". Hails from " + (t_cm.Hometown != null ? t_cm.Hometown : "") + (t_cm.HometownRegion != null ? ", " + (!string.IsNullOrEmpty(t_cm.HometownRegion.RegCode) ? t_cm.HometownRegion.RegCode.ToUpper() : t_cm.HometownRegion.Name).Trim() : ""),     ///(t_cm.HometownRegion != null ? (t_cm.HometownRegion.RegCode.Length > 0 ? t_cm.HometownRegion.RegCode.ToUpper() : t_cm.HometownRegion.Name.ToUpper()) : "")
//        //                                                                  // strLangProfiency = t_mls.LanguageSpoken == null ? "None" : "Speaks " + t_mls.LanguageSpoken.Name + " (" + (new MemberProfileVM()).ReturnLangProficiency((int)t_mls.ProficiencyLevel) + ")",
//        //                                                                  // oPrimaryLanguage = t_mls.LanguageSpoken != null ? t_mls.LanguageSpoken : null,
//        //                                                                  // PrimaryLanguageId = t_mls.LanguageSpokenId != null ? t_mls.LanguageSpokenId : null,

//        //                         //oMemberChurchlife = t_mcl,  // != null ? t_mcl : null, //  t9 ?? new MemberChurchLife(),
//        //                         //oMemberLocAddr = t_cm.ContactInfo != null ? t_cm.ContactInfo : null,  //t_cm.ContactInfo ?? new ContactInfo(),
//        //                         //oMemberContact = t_mc, //!= null ? t_mc : null,  // t8 ?? new MemberContact(),
//        //                         //oEducationLevel = t_meh != null ? t_meh.InstitutionType : null,
//        //                         //EducationLevel = (from a in _context.InstitutionType.Where(x => x.EduLevelIndex == 
//        //                         //                  _context.MemberEducHistory.Where(y => y.ChurchMemberId == t_cm.Id).Min(y => y.InstitutionType.EduLevelIndex)) select a ).Take(1).ToList(),
//        //                         //  EducationLevelId = t_meh != null ? t_meh.InstitutionTypeId : null,
//        //                         // PhotoPath = t_cm.PhotoUrl,

//        //                         ////member's identity in cong
//        //                         //MemberId = t_cm.Id,
//        //                         //MemberCode = t_cm.MemberCode,

//        //                         // //oMemberRank = t_mr ?? new MemberRank(),
//        //                         //strChurchRank = t_mr.ChurchRank != null ? t_mr.ChurchRank.RankName : "None",
//        //                         //oChurchRank = t_mr != null ? t_mr.ChurchRank : new ChurchRank(),
//        //                         //RankId = t_mr != null ? t_mr.ChurchRankId : 0,

//        //                         ////oChurchPosition = t_mp != null ? t_mp.ChurchPosition : new ChurchPosition(),
//        //                         // strChurchPos = t_mp.ChurchPosition != null ? t_mp.ChurchPosition.PositionName : "None",
//        //                         //oMemberPos = t_mp ?? new MemberPosition(), 
//        //                         //PositionId = t_mp != null ? t_mp.ChurchPositionId : 0,

//        //                         strMemberStat = t_ms.ChurchMemStatus != null ? t_ms.ChurchMemStatus.Name : "Unassigned",

//        //                         oMemberStatus = t_ms, //?? new MemberStatus(),

//        //                         //oChurchMemStatus = t_ms != null ? t_ms.ChurchMemStatus : new ChurchMemStatus(),
//        //                         //ChurchMemStatusId = t_ms != null ? t_ms.ChurchMemStatusId : 0,

//        //                         //what the member does in church
//        //                         //   strCoreArea = t_mcum.ChurchUnit != null ? t_mcum.ChurchUnit.Name : "None",  //service group
//        //                         //oMemberChurchUnit = t_mcug != null ? t_mcug : null,
//        //                         strAgeGroup = t_mcug.ChurchUnit != null ? t_mcug.ChurchUnit.Name : "None",   /// age group
//        //                         //strCoreRole = t_mlr.LeaderRole != null ? t_mlr.LeaderRole.RoleName : "None",   // core role
//        //                         //strMemChuRoleDesc = (t_mlr.LeaderRole != null ? t_mlr.LeaderRole.RoleName : "None") != "None" && t_rcu != null ? t_mlr.LeaderRole.RoleName + ", " + t_rcu.Name : t_mlr.LeaderRole.RoleName


//        //                         //&& x.Departed != null      
//        //                         //String.Format("{0:dd-MMM-yyyy}", dt)
//        //                         //DateTime thisDate = new DateTime(2008, 3, 15);
//        //                         //CultureInfo culture = new CultureInfo("pt-BR");
//        //                         //Console.WriteLine(thisDate.ToString("d", culture));  // Displays 15/3/2008

//        //                         //DateTime date1 = new DateTime(2008, 4, 10);
//        //                         //Console.WriteLine(date1.ToString("D",
//        //                         //                 CultureInfo.CreateSpecificCulture("en-US")));
//        //                         // Displays Thursday, April 10, 2008    

//        //                         //intMemberAge = a.DateOfBirth != null ? (int)((DateTime.Now.Date - a.DateOfBirth.Value.Date).TotalDays / 365) : -1, 

//        //                         //DateTime date1 = new DateTime(2008, 4, 10, 6, 30, 0);
//        //                         //Console.WriteLine(date1.ToString("m",  CultureInfo.CreateSpecificCulture("en-us")));
//        //                         // Displays April 10  ....  10 April .. "ms-MY"

//        //                     })
//        //                        //.Distinct()
//        //                        .OrderBy(x => x.strMemberFullName)
//        //                        .ToList();


//        //    return oMemProfileVMList;
//        //}

//        public  List<ChurchMember> GetCurrentMemberProfiles(ChurchBody oChurchBody, ChurchModelContext _context = null)
//        {
//            //Get member profile list      
//            if (_context == null) _context = this._context;
//                var  oMemProfileVMList = (                                 
//                                 from t_cb in _context.ChurchBody.AsNoTracking().Where(x => x.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId && (x.OrganisationType == "GB" || x.OrganisationType == "CN") && (x.ChurchType == "CH" || x.ChurchType == "CF") &&
//                                               (x.Id == oChurchBody.Id || x.ParentChurchBodyId == oChurchBody.Id || x.RootChurchCode.Contains(oChurchBody.RootChurchCode)))
//                                 from t_cm in _context.ChurchMember.AsNoTracking()
//                                                .Where(x => x.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && x.ChurchBodyId == t_cb.Id &&
//                                                x.IsActivated == true && x.MemberClass == "C")
//                                 from t_ms in _context.MemberStatus.AsNoTracking().Where(x => x.ChurchMemberId == t_cm.Id && x.ChurchBodyId== t_cb.Id &&
//                                                x.IsCurrent == true && x.ChurchMemStatus.Available == true && x.ChurchMemStatus.Deceased==false)
//                                                .DefaultIfEmpty()

//                                 //from t_mr in _context.MemberRank.AsNoTracking().Where(x => x.ChurchMemberId == t_cm.Id && x.ChurchBodyId == t_cb.Id && 
//                                 //               x.IsCurrentRank==true).DefaultIfEmpty()
//                                 //from t_mp in _context.MemberPosition.AsNoTracking().Where(x => x.ChurchMemberId == t_cm.Id && x.ChurchBodyId == t_cb.Id &&
//                                 //                x.CurrentPos == true).DefaultIfEmpty()
//                                 //from t_mcug in _context.MemberChurchUnit.AsNoTracking().Where(x => x.ChurchMemberId == t_cm.Id && x.ChurchBodyId == t_cb.Id &&
//                                 //               x.ChurchUnit.IsUnitGenerational == true && x.IsCurrUnit == true).DefaultIfEmpty()
 
//                                  select   t_cm) 
//                                    //.OrderBy(x => x.strMemberFullName) 
//                                    .ToList();
                                  

//            return  oMemProfileVMList;   
//        }

//        // public List<MemberProfileVM> FilterMemberProfiles(List<MemberProfileVM> memProList, char filter, object filterVal)
//        public List<MemberProfileVM> GetMemberProfiles(List<MemberProfileVM> memProList, char? filterItem, int? filterVal)
//        {
//            //filter :  G-church gen groups, S-church status, P-church position
//            var oMemProVMList = new List<MemberProfileVM>();

//            switch ((char)filterItem)
//            {
//                case 'G':   //geneneration grouping
//                    return memProList.Where(c => c.oMemberChurchUnit?.ChurchUnitId == (int)filterVal).ToList(); // break;

//                case 'S':
//                    return memProList.Where(c => c.oMemberStatus?.ChurchMemStatusId == (int)filterVal).ToList(); //break;

//                case 'P':
//                    return memProList.Where(c => c.oMemberPos?.ChurchPositionId == (int)filterVal).ToList(); //break;

//                default:
//                    return memProList; //break;
//            }

//            //var _oMemProVMList = AppUtilties_Static.ToListAsync<MemberProfileVM>(oMemProVMList);
//            //return View(await _oMemProVMList);
//        }

//        public MemberProfileVM LoadMemberListFilters(ChurchBody oChurchBody) //int currChuBodyId)
//        {
//            var oMemPaneVM = new MemberProfileVM();

//            //var ChurchUnitTypes = _context.ChurchDivisionType.Include(t => t.OwnedByChurchBody)
//            //                .Where(c => c.OwnedByChurchBody.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId && !string.IsNullOrEmpty(c.Description) &&
//            //                                c.OrganisationType == orgType &&
//            //                 (c.OwnedByChurchBodyId == oChurchBody.Id ||
//            //                 (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//            //                 (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody)))
//            //        ).OrderBy(c => c.OrganisationType).ThenBy(c => c.Description).ToList()
//            //     .Select(c => new SelectListItem()
//            //     {
//            //         Value = c.Id.ToString(),
//            //         Text = c.Description
//            //     })
//            //     .ToList();

//            //if (addEmpty) ChurchUnitTypes.Insert(0, new SelectListItem { Value = "", Text = "Select" });
//            //return Json(ChurchUnitTypes);


//            oMemPaneVM.lkpChurchMemStatuses = _context.ChurchMemStatus.Where(c => !string.IsNullOrEmpty(c.Name) && //c.ChurchBodyId == oChurchBody.Id && 
//                                                                                                                        //&& c.Available==false && c.Deceased == false &&
//                             (c.OwnedByChurchBodyId == oChurchBody.Id ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody))))
//                .OrderBy(c => c.OrderIndex).ThenBy(c => c.Name).ToList()
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.Name
//                                           })
//                                           // .OrderBy(c => c.Text)
//                                           .ToList();

//            oMemPaneVM.lkpChuMemTypes = _context.ChurchMemType.Where(c => !string.IsNullOrEmpty(c.Description) && //c.ChurchBodyId == oChurchBody.Id &&
//                            (c.OwnedByChurchBodyId == oChurchBody.Id ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody))))
//                .OrderBy(c => c.Description).ToList()
//                                           .Select(c => new SelectListItem()
//                                           {
//                                               Value = c.Id.ToString(),
//                                               Text = c.Description
//                                           })
//                                           // .OrderBy(c => c.Text)
//                                           .ToList();

//            oMemPaneVM.lkpChuMemRanks = _context.ChurchRank.Where(c => !string.IsNullOrEmpty(c.RankName) && //c.ChurchBodyId == oChurchBody.Id &&
//                             (c.OwnedByChurchBodyId == oChurchBody.Id ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody))))
//                .OrderBy(c => c.GradeLevel).ThenBy(c => c.RankName).ToList()
//                                         .Select(c => new SelectListItem()
//                                         {
//                                             Value = c.Id.ToString(),
//                                             Text = c.RankName
//                                         })
//                                         // .OrderBy(c => c.Text)
//                                         .ToList();

//            oMemPaneVM.lkpChuPositions = _context.ChurchPosition.Where(c => !string.IsNullOrEmpty(c.PositionName) && // c.ChurchBodyId == oChurchBody.Id &&
//                            (c.OwnedByChurchBodyId == oChurchBody.Id ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//                             (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody))))
//                .OrderBy(c => c.GradeLevel).ToList()
//                                          .Select(c => new SelectListItem()
//                                          {
//                                              Value = c.Id.ToString(),
//                                              Text = c.PositionName
//                                          })
//                                          // .OrderBy(c => c.Text)
//                                          .ToList();

//            oMemPaneVM.lkpChurchGroupingTypes = _context.ChurchBody.Where(c => c.OrganisationType=="CG" && !string.IsNullOrEmpty(c.Name) && //c.ChurchBodyId == oChurchBody.Id &&
//                (c.OwnedByChurchBodyId == oChurchBody.Id ||
//                           (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//                           (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody))))
//                           .OrderBy(c => c.Name).ToList()
//                                         .Select(c => new SelectListItem()
//                                         {
//                                             Value = c.Id.ToString(),
//                                             Text = c.Name
//                                         })
//                                         // .OrderBy(c => c.Text)
//                                         .ToList();

//            oMemPaneVM.lkpChurchUnits = _context.ChurchBody.Where(c => !string.IsNullOrEmpty(c.Name) && //c.ChurchBodyId == oChurchBody.Id &&
//                 (c.OwnedByChurchBodyId == oChurchBody.Id ||
//                            (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "C" && c.OwnedByChurchBodyId == oChurchBody.ParentChurchBodyId) ||
//                            (c.OwnedByChurchBodyId != oChurchBody.Id && c.SharingStatus == "A" && IsAncestor_ChurchBody(c.OwnedByChurchBody, oChurchBody))))
//                            .OrderBy(c => c.ParentChurchBodyId).ThenBy(c => c.Name).ToList()
//                                          .Select(c => new SelectListItem()
//                                          {
//                                              Value = c.Id.ToString(),
//                                              Text = c.Name
//                                          })
//                                          // .OrderBy(c => c.Text)
//                                          .ToList();


//            //oMemPaneVM.ChuSectors = _context.ChurchSector.Where(c => c.ChurchBodyId == oChurchBody.Id)
//            //    .OrderBy(c => c.ChurchSectorCategory.Name).ThenBy(c => c.Name).ToList()
//            //                               .Select(c => new SelectListItem()
//            //                               {
//            //                                   Value = c.Id.ToString(),
//            //                                   Text = c.Name
//            //                               })
//            //                               // .OrderBy(c => c.Text)
//            //                               .ToList();


//            return oMemPaneVM;
//        }





//        //public string ReturnLangProficiency(decimal lvl)
//        //{
//        //    switch (lvl)
//        //    {
//        //        case 1 : return "Basics"; //1-3
//        //        case 2: return "Intermediate"; //4-6
//        //        case 3: return "Fluent"; //7-8
//        //        case 4: return "Proficient"; // 9-10

//        //        default: return string.Empty; 
//        //    }
//        //}


//        //public string ReturnMaritalStatusDesc(string mCode)
//        //{
//        //    switch (mCode)
//        //    {
//        //        case "S": return "Single"  ;
//        //        case "M": return "Married" ;
//        //        case "X": return "Separated";
//        //        case "D": return "Divorced" ;
//        //        case "W": return "Widowed" ;
//        //        case "O": return "Other";

//        //        default: return "N/A";
//        //    }
//        //}



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

//        private void SetUserLogged1()
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

//        private void SetUserLogged2()
//        {
//            //oUserLogIn_Priv = TempData.Get<List<UserSessionPrivilege>>("UserLogIn_oUserPrivCol");

//            var tempPrivList = TempData["UserLogIn_oUserPrivCol"] as string;
//            // De serialize the string to object
//            oUserLogIn_Priv = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);
//            //
//            isCurrValid = oUserLogIn_Priv?.Count > 0;
//            if (isCurrValid)
//            {
//                ViewBag.oChuBodyLogged = oUserLogIn_Priv[0].ChurchBody;
//                ViewBag.oUserLogged = oUserLogIn_Priv[0].UserProfile;
//            }
//        }


//        //private MemPersDataViewModel populateLookups(MemPersDataViewModel vmLkp, int? currChuBodyId)
//        //{
//        //    vmLkp.GenderTypes = new List<SelectListItem>();
//        //    foreach (var dl in dlGenderType) { vmLkp.GenderTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//        //    vmLkp.MaritalStatuses = new List<SelectListItem>();
//        //    foreach (var dl in dlMaritalStatus) { vmLkp.MaritalStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//        //    vmLkp.MaritalTypes = new List<SelectListItem>();
//        //    foreach (var dl in dlMarriageType) { vmLkp.MaritalTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

//        //    //get other lists
//        //    vmLkp.ChuPositions = _context.ChurchPosition.Where(c => c.ChurchBodyId == currChuBodyId).OrderByDescending(c => c.AuthorityIndex).ThenBy(c => c.PositionName).ToList()
//        //                                    .Select(c => new SelectListItem()
//        //                                    {
//        //                                        Value = c.Id.ToString(),
//        //                                        Text = c.PositionName
//        //                                    })
//        //                                    .ToList();

//        //    vmLkp.ChurchChurchMemStatuses = _context.ChurchMemStatus.Where(c => c.ChurchBodyId == currChuBodyId).OrderBy(c => c.OrdeIndex).ThenBy(c => c.Name).ToList()
//        //                                    .Select(c => new SelectListItem()
//        //                                    {
//        //                                        Value = c.Id.ToString(),
//        //                                        Text = c.Name
//        //                                    })
//        //                                    .ToList();

//        //    vmLkp.Countries = _context.Country.Where(c => c.Display == true).ToList()
//        //                                   .Select(c => new SelectListItem()
//        //                                   {
//        //                                       Value = c.Id.ToString(),
//        //                                       Text = c.Name
//        //                                   })
//        //                                   .OrderBy(c => c.Text)
//        //                                   .ToList();

//        //    vmLkp.CtryRegions = _context.CountryRegion
//        //                                    .Include(t => t.Country)
//        //                                    //.Where(c => c.CountryId == currChuBodyId)
//        //                                    .ToList()
//        //                                    .Select(c => new SelectListItem()
//        //                                    {
//        //                                        Value = c.Id.ToString(),
//        //                                        Text = c.Name
//        //                                    })
//        //                                    .OrderBy(c => c.Text)
//        //                                    .ToList();

//        //    vmLkp.EduLevels = _context.InstitutionType.Where(c => c.ChurchBodyId == currChuBodyId).OrderBy(c => c.Name).ToList()
//        //                .Select(c => new SelectListItem()
//        //                {
//        //                    Value = c.Id.ToString(),
//        //                    Text = c.Name
//        //                })
//        //                .OrderBy(c => c.Text)
//        //                .ToList();

//        //    vmLkp.Languages = _context.LanguageSpoken.Where(c => c.ChurchBodyId == currChuBodyId).ToList()
//        //                .Select(c => new SelectListItem()
//        //                {
//        //                    Value = c.Id.ToString(),
//        //                    Text = c.Name
//        //                })
//        //                .OrderBy(c => c.Text)
//        //                .ToList();

//        //    vmLkp.ID_Types = _context.National_IDType.ToList()
//        //                .Select(c => new SelectListItem()
//        //                {
//        //                    Value = c.Id.ToString(),
//        //                    Text = c.IDType
//        //                })
//        //                .OrderBy(c => c.Text)
//        //                .ToList();

//        //    //TempData.Keep();
//        //    return vmLkp;
//        //}


//        //public IActionResult AddOrEdit(int id = 0)
//        //{
//        //    SetUserLogged();
//        //    if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");

//        //    //continue....
//        //    var oUserProRoleLog = TempData.Get<UserProfileRole>("oUserProRoleLogIn");
//        //    int? currChuBodyId = null;
//        //    if (oUserProRoleLog != null)
//        //        currChuBodyId = oUserProRoleLog.UserProfile?.ChurchBodyId;

//        //    MemPersDataViewModel vmMod = new MemPersDataViewModel();
//        //    if (currChuBodyId == null)
//        //        return PartialView("_AddOrEditPersData", vmMod);


//        //    //vmMod.ChuPositions = new List<ChurchPosition>();
//        //    //// vmMod.MemContactInfo = new ContactInfo();
//        //    //vmMod.MemChurchPositions = new List<MemberPosition>();
//        //    //vmMod.stat = new List<MemberStatus>();

//        //    if (id == 0)
//        //    {
//        //        //create user and init...
//        //        vmMod.oPersonalData = new ChurchMember();
//        //        vmMod.oPersonalData.ChurchBodyId = currChuBodyId;
//        //        // vmMod.oPersonalData.ContactInfo = new ContactInfo();
//        //    }

//        //    else
//        //    {
//        //        vmMod.oPersonalData = _context.ChurchMember.Find(id);
//        //    }

//        //    var _vmMod = populateLookups(vmMod, currChuBodyId);
//        //    TempData.Put("oVmCurr", _vmMod);
//        //    TempData.Keep();

//        //    return PartialView("_AddOrEditPersData", _vmMod);
//        //}


//        //// POST: ChurchMembers/Create
//        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> AddOrEdit(MemPersDataViewModel vm)
//        ////([Bind("Id,ChurchBodyId,MemberCode,Title,FirstName,MiddleName,LastName,MaidenName,Gender,DateOfBirth,MaritalStatus,MarriageType,NationalityId,ContactInfoId,Hobbies,Hometown,HometownRegionId,Profession,Occupation,WorkPlace,EducationLevelId,Discipline,PhotoUrl,OtherInfo,Created,LastMod")] ChurchMember churchMember)

//        //{
//        //    var churchMember = vm.oPersonalData;

//        //    //get lists to persist...            
//        //    //ModelState.Remove("oPersonalData.OwnerId");
//        //    //ModelState.Remove("oPersonalData.ChurchBodyId");

//        //    //new accounts must be activated b/f assign role
//        //    //if (churchMember.Id == 0 && vm.oPersonalData != null)
//        //    //{

//        //    //dummy user... no role yet
//        //    ModelState.Remove("oPersonalData.ContactInfoId");
//        //    ModelState.Remove("oPersonalData.ChurchBodyId");
//        //    ModelState.Remove("oPersonalData.HometownRegionId");
//        //    ModelState.Remove("oPersonalData.EducationLevelId");
//        //    ModelState.Remove("oPersonalData.MotherTongueId");
//        //    ModelState.Remove("oPersonalData.NationalityId");
//        //    ModelState.Remove("oPersonalData.PrimaryLanguageId");
//        //    ModelState.Remove("oPersonalData.PositionId");
//        //    ModelState.Remove("oPersonalData.StatusId");
//        //    ModelState.Remove("oPersonalData.IDTypeId");
//        //    ModelState.Remove("oMemberLocAddr.RegionId");
//        //    ModelState.Remove("oMemberLocAddr.CountryId");
//        //    ModelState.Remove("oMemberLocAddr.ChurchBodyId");
//        //    // }

//        //    if (ModelState.IsValid)
//        //    {  //validations... unique name
//        //        var oExistCnt = _context.ChurchMember.Where(c => c.MemberCode == churchMember.MemberCode && c.ChurchBodyId == churchMember.ChurchBodyId && c.Id != churchMember.Id).Count();
//        //        if (oExistCnt > 0)
//        //            ModelState.AddModelError(churchMember.MemberCode, "Member Code must be unique for every member");
//        //        else
//        //        {
//        //            if (churchMember.DateOfBirth > DateTime.Now)
//        //                ModelState.AddModelError(churchMember.Id.ToString(), "Date of birth must be ahead of today member, " + churchMember.FirstName.ToUpper() + " " + churchMember.LastName.ToUpper());
//        //            //else
//        //            //{
//        //            //    var oExistUser = _context.ChurchMember.Where(c => c.Id != churchMember.Id && c. == churchMember.Email && c.ChurchBodyId == churchMember.ChurchBodyId).FirstOrDefault();
//        //            //    if (oExistUser != null)
//        //            //        ModelState.AddModelError(churchMember.Email, "Email of user must be unique. >> Hint: Already used by: " + oExistUser.UserDesc.ToUpper());
//        //            //}
//        //        }

//        //        //finally check error state...
//        //        if (ModelState.IsValid)
//        //        {
//        //            churchMember.LastMod = DateTime.Now;

//        //            if (churchMember.Id == 0)
//        //            {
//        //                //if (IsNumeric(vmPost.oCurrchurchMember.Pwd))
//        //                //reset on next logon... send temp password to email.

//        //                //var oExistUser = _context.ChurchMember.Where(c => c.Email == churchMember.Email && c.ChurchBodyId == churchMember.ChurchBodyId).FirstOrDefault();
//        //                //if (oExistUser != null) ModelState.AddModelError(churchMember.Email, "Email of user must be unique. >> Hint: Already used by: " + oExistUser.UserDesc.ToUpper());

//        //                //string strPwdHashedData = AppUtilties.ComputeSha256Hash(churchMember.Username + churchMember.Pwd);
//        //                //churchMember.Pwd = strPwdHashedData;

//        //                churchMember.Created = DateTime.Now;
//        //                _context.Add(churchMember);

//        //                //await _context.SaveChangesAsync();
//        //                ViewBag.UserMsg = "Saved member " + churchMember.FirstName.ToUpper() + " " + churchMember.LastName.ToUpper() + " Personal profile successfully.";
//        //            }

//        //            else
//        //            {
//        //                ViewBag.UserMsg = "Updated member " + churchMember.FirstName.ToUpper() + " " + churchMember.LastName.ToUpper() + " Personal profile successfully.";

//        //                _context.Update(churchMember);

//        //                //reset password to be done via another page                        
//        //                //await _context.SaveChangesAsync();
//        //            }


//        //            churchMember.LastMod = DateTime.Now;

//        //            try
//        //            {
//        //                await _context.SaveChangesAsync();
//        //            }

//        //            catch(Exception ex)
//        //            {
//        //                ModelState.AddModelError(string.Empty, ex.ToString());
//        //            }


//        //            ////get th contact set
//        //            //if (churchMember.ContactInfo == null)
//        //            //{
//        //            //    var oNewMem = _context.ChurchMember.Where(c => c.ChurchBodyId == churchMember.ChurchBodyId && c.LastMod == _context.ChurchMember.Max(p => p.LastMod)).FirstOrDefault();
//        //            //    var oContact = new ContactInfo();
//        //            //    oContact.TempMemRef = oNewMem.Id;
//        //            //    oNewMem.Created = DateTime.Now;
//        //            //    oNewMem.LastMod = DateTime.Now;
//        //            //    _context.Add(oNewMem);
//        //            //    await _context.SaveChangesAsync();
//        //            //}                   
//        //        }
//        //    }
//        //    else
//        //        ModelState.AddModelError(string.Empty, "Error! Please check data and re-submit.");


//        //    var _oVmCurr = TempData.Get<MemPersDataViewModel>("oVmCurr");
//        //    if (_oVmCurr != null) _oVmCurr.oPersonalData = churchMember;

//        //    // var _vmMod = populateLookups(vmMod, currChuBodyId);
//        //    TempData.Put("oVmCurr", _oVmCurr);
//        //    TempData.Keep();

//        //    return RedirectToAction(nameof(Index));
//        //    //  return PartialView("_AddOrEditPersData", _oVmCurr);
//        //}

//        //// GET: ChurchMember/Delete/5
//        //public async Task<IActionResult> Delete(int? id)
//        //{
//        //    var res = false;
//        //    var ChurchMember = await _context.ChurchMember.FindAsync(id);
//        //    if (ChurchMember != null)
//        //    {

//        //        //check all member related modules for references to deny deletion

//        //        if (res)
//        //        {
//        //            _context.ChurchMember.Remove(ChurchMember);
//        //            await _context.SaveChangesAsync();
//        //        }
//        //        else
//        //        {
//        //            ModelState.AddModelError(ChurchMember.FirstName, "Delete failed. Member data is referenced elsewhere in the Application.");
//        //            ViewBag.UserDelMsg = "Delete failed. Member data is referenced elsewhere in the Application.";
//        //        }
//        //    }

//        //    return RedirectToAction(nameof(Index));
//        //    //return View();
//        //}










//        //public IActionResult AddOrEditPersData(int memId = 0)
//        //{
//        //    SetUserLogged();
//        //    if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
//        //    else
//        //    {                
//        //        ChurchMember churchMember = null;
//        //        if (memId == 0)
//        //        {
//        //            churchMember = new ChurchMember();
//        //        }

//        //        else
//        //        {
//        //            churchMember = _context.ChurchMember.Find(memId);
//        //        }

//        //        TempData.Keep();
//        //        popLookups_MemberPersData();
//        //        return PartialView("_MemberPersData", churchMember);
//        //    }             
//        //}








//        //private void popLookups_MemberPersData(object oSelectNational = null, object oSelectCongre = null, object oSelectHomeReg = null, object oSelectEducLevel = null)
//        //{
//        //    var listGender = new List<SelectListItem>
//        //    {
//        //        new SelectListItem{ Text="Male", Value = "M" },
//        //        new SelectListItem{ Text="Female", Value = "F" },
//        //        new SelectListItem{ Text="Other", Value = "O" }
//        //    };

//        //    ViewBag.oGenderList = listGender;
//        //    ViewBag.oNational = new SelectList(_context.Country.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name", oSelectNational);
//        //    ViewBag.oHomeReg = new SelectList(_context.CountryRegion.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name", oSelectHomeReg);
//        //    ViewBag.oEducLevel = new SelectList(_context.InstitutionType.OrderBy(c => c.Name).AsNoTracking(), "Id", "Name", oSelectEducLevel);
//        //    ViewBag.oCongre = new SelectList(_context.ChurchBody.Where(c => c.ChurchType == "CF").OrderBy(c => c.Name).AsNoTracking(), "Id", "Name", oSelectCongre);
//        //}


//    }

//}