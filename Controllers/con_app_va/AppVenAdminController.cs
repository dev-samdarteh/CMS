
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RhemaCMS.Controllers.con_adhc;
using RhemaCMS.Models;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
// using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using RhemaCMS.Models.ViewModels;
using static RhemaCMS.Models.ViewModels.AppVenAdminVM;
//using Microsoft.Extensions.Hosting;

namespace RhemaCMS.Controllers.con_app_va
{
    public class AppVenAdminController : Controller
    {
        private readonly MSTR_DbContext _context;
       // private readonly MSTR_DbContext _masterContextLog;
      //  private readonly ChurchModelContext _clientContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private bool isCurrValid = false;
        private List<UserSessionPrivilege> oUserLogIn_Priv = null;
        
        private List<DiscreteLookup> dlCBDivOrgTypes = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlShareStatus = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlOwnerStatus = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlGenStatuses = new List<DiscreteLookup>();
      // private List<DiscreteLookup> dlChurchType = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlChuWorkStat = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlUserRoleTypes = new List<DiscreteLookup>();
        private List<DiscreteLookup> dlUserAuthTypes = new List<DiscreteLookup>();

        public AppVenAdminController(MSTR_DbContext context, IWebHostEnvironment hostingEnvironment) //ChurchModelContext ctx, 
        {
            _context = context;

          //  _masterContextLog = context; // new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString);

           // _clientContext = ctx ;
            _hostingEnvironment = hostingEnvironment;

            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "A", Desc = "Active" });
            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "D", Desc = "Deactive" });
            dlGenStatuses.Add(new DiscreteLookup() { Category = "GenStatus", Val = "E", Desc = "Expired" }); 

            //SharingStatus { get; set; }  // A - Share with all sub-congregations, C - Share with child congregations only, N - Do not share
            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "N", Desc = "Do not roll-down (share)" });
            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "C", Desc = "Roll-down (share) for direct child congregations" });
            dlShareStatus.Add(new DiscreteLookup() { Category = "ShrStat", Val = "A", Desc = "Roll-down (share) for all sub-congregations" });

            // OwnershipStatus { get; set; }  // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody
            dlOwnerStatus.Add(new DiscreteLookup() { Category = "OwnStat", Val = "O", Desc = "Originated" });
            dlOwnerStatus.Add(new DiscreteLookup() { Category = "OwnStat", Val = "I", Desc = "Inherited" });


            // dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CR", Desc = "Church Root (Denomination)" });  --  CB [church body]
            // dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "GB", Desc = "Governing Body" });              --  CB 
            //dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CO", Desc = "Church Office" });                --   CB
            //dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "DP", Desc = "Department" });  //Ministry        -- CSU [church sector unit]
            //dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CG", Desc = "Church Grouping" });                -- CSU
            //dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "SC", Desc = "Standing Committee" }); // Working Committee   -- CSU
            //dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CE", Desc = "Church Enterprise" });              -- CB
            //dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "TM", Desc = "Team" });   // Working Team .. group of roles/pos   -- CR  [church roles]
            ////dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CP", Desc = "Church Position" });              -- CR
            //dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "IB", Desc = "Independent Unit" });               -- CB
            ///
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CH", Desc = "Congregation Head-unit" });  // oversight directly on congregations   -- CB
            dlCBDivOrgTypes.Add(new DiscreteLookup() { Category = "CBDivOrgType", Val = "CN", Desc = "Congregation" });  //to look up congregation by church code [short or full path]  -- CB
            /// 
            //dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "", Desc = "N/A" });
            //dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CH", Desc = "Congregation Head-unit" });
            //dlChurchType.Add(new DiscreteLookup() { Category = "ChurchType", Val = "CF", Desc = "Congregation" });

            dlChuWorkStat.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "S", Desc = "Structure Only" });
            dlChuWorkStat.Add(new DiscreteLookup() { Category = "ChuWorkStat", Val = "O", Desc = "Operationalized" });

            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "SYS", Desc = "System" }); // 0
            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "SUP_ADMN", Desc = "Super Admin" }); // 1
            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "SYS_ADMN", Desc = "System Admin" });  // 2

            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CH_ADMN", Desc = "Church Admin" }); // 3
            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CH_RGSTR", Desc = "Church Registrar" }); // 4
            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CH_ACCT", Desc = "Church Accountant" });// 5
            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CH_CUST", Desc = "Church Custom" }); // 6

            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CF_ADMN", Desc = "Congregation Admin" }); //  7             
            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CF_RGSTR", Desc = "Congregation Registrar" }); // 8          
            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CF_ACCT", Desc = "Congregation Accountant" });  // 9          
            dlUserRoleTypes.Add(new DiscreteLookup() { Category = "UserRolType", Val = "CF_CUST", Desc = "Congregation Custom" }); // 10  

            dlUserAuthTypes.Add(new DiscreteLookup() { Category = "UserAuthType", Val = "1", Desc = "Two-way Authentication" });
            dlUserAuthTypes.Add(new DiscreteLookup() { Category = "UserAuthType", Val = "2", Desc = "Security Question Validation" });


            // The example displays the following output:
            //    Pattern                                  Result String
            //
            //    MM/dd/yyyy                               08/28/2014
            //    yyyy-MM-dd                               2014-08-28
            //    dddd, dd MMMM yyyy                       Thursday, 28 August 2014
            //    dddd, dd MMMM yyyy HH:mm                 Thursday, 28 August 2014 12:28
            //    dddd, dd MMMM yyyy hh:mm tt              Thursday, 28 August 2014 12:28 PM
            //    dddd, dd MMMM yyyy H:mm                  Thursday, 28 August 2014 12:28
            //    dddd, dd MMMM yyyy h:mm tt               Thursday, 28 August 2014 12:28 PM
            //    dddd, dd MMMM yyyy HH:mm:ss              Thursday, 28 August 2014 12:28:30
            //    MM/dd/yyyy HH:mm                         08/28/2014 12:28
            //    MM/dd/yyyy hh:mm tt                      08/28/2014 12:28 PM
            //    MM/dd/yyyy H:mm                          08/28/2014 12:28
            //    MM/dd/yyyy h:mm tt                       08/28/2014 12:28 PM
            //    yyyy-MM-dd HH:mm                         2014-08-28 12:28
            //    yyyy-MM-dd hh:mm tt                      2014-08-28 12:28 PM
            //    yyyy-MM-dd H:mm                          2014-08-28 12:28
            //    yyyy-MM-dd h:mm tt                       2014-08-28 12:28 PM
            //    MM/dd/yyyy HH:mm:ss                      08/28/2014 12:28:30
            //    yyyy-MM-dd HH:mm:ss                      2014-08-28 12:28:30
            //    MMMM dd                                  August 28
            //    MMMM dd                                  August 28
            //    yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK   2014-08-28T12:28:30.0000000
            //    yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK   2014-08-28T12:28:30.0000000
            //    ddd, dd MMM yyyy HH':'mm':'ss 'GMT'      Thu, 28 Aug 2014 12:28:30 GMT
            //    ddd, dd MMM yyyy HH':'mm':'ss 'GMT'      Thu, 28 Aug 2014 12:28:30 GMT
            //    yyyy'-'MM'-'dd'T'HH':'mm':'ss            2014-08-28T12:28:30
            //    HH:mm                                    12:28
            //    hh:mm tt                                 12:28 PM
            //    H:mm                                     12:28
            //    h:mm tt                                  12:28 PM
            //    HH:mm:ss                                 12:28:30
            //    yyyy'-'MM'-'dd HH':'mm':'ss'Z'           2014-08-28 12:28:30Z
            //    dddd, dd MMMM yyyy HH:mm:ss              Thursday, 28 August 2014 12:28:30
            //    yyyy MMMM                                2014 August
            //    yyyy MMMM                                2014 August

        }



        public int GetRoleTypeLevel(string oCode)
        {
            switch (oCode)
            {
                case "SYS": return 1;
                case "SUP_ADMN": return 2;
                case "SYS_ADMN": return 3;
                case "SYS_CUST": return 4;
                // case "SYS_CUST2": return 5;
                //
                case "CH_ADMN": return 6;
                case "CH_RGSTR": return 7;
                case "CH_ACCT": return 8;
                case "CH_CUST": return 9;
                // case "CH_CUST2": return 10;
                //
                case "CF_ADMN": return 11;
                case "CF_RGSTR": return 12;
                case "CF_ACCT": return 13;
                case "CF_CUST": return 14;
                // case "CF_CUST2": return 15; 
                //
                default: return 0;
            }
        }


        public static string GetStatusDesc(string oCode)
        { 
            switch (oCode)
            {
                case "A": return "Active";
                case "D": return "Deactive";
                case "P": return "Pending";
                case "E": return "Expired";


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
                //case "GB": return "Governing Body";
                //case "CO": return "Church Office";
                //case "DP": return "Department";                
                //case "CG": return "Church Grouping";
                //case "SC": return "Standing Committee";
                //case "CE": return "Church Enterprise";
                //case "TM": return "Team";
                //// case "CP": return "Church Position";
                //case "IB": return "Independent Unit";
                case "CH": return "Congregation Head-unit";
                case "CN": return "Congregation";

                default: return oCode;
            }
        }

        public object GetChuOrgTypeDetail(string oCode, bool returnSetIndex)
        {
            switch (oCode)
            {
                case "CR": if (returnSetIndex) return 0; else return "Church Root (Denomination)";
                //case "GB": if (returnSetIndex) return 1; else return "Governing Body";
                //case "CO": if (returnSetIndex) return 2; else return "Church Office";
                //case "DP": if (returnSetIndex) return 3; else return "Church Department";
                //case "CG": if (returnSetIndex) return 4; else return "Church Grouping";
                //case "SC": if (returnSetIndex) return 5; else return "Standing Committee";
                //case "CE": if (returnSetIndex) return 6; else return "Church Enterprise";
                //case "TM": if (returnSetIndex) return 7; else return "Team";
                ////case "CP": if (returnSetIndex) return 8; else return "Church Position";
               // case "IB": if (returnSetIndex) return 8; else return "Independent Unit";  // Independent Body e.g. Boards, Trustees
                case "CH": if (returnSetIndex) return 9; else return "Congregation Head-unit";
                case "CN": if (returnSetIndex) return 10; else return "Congregation";

                default: return oCode;
            }
        }

        public string GetChuOrgTypeCode(int setIndex)
        {
            switch (setIndex)
            {
                case 0: return "CR";
                //case 1: return "GB";
                //case 2: return "CO";
                //case 3: return "DP";
                //case 4: return "CG";
                //case 5: return "SC";
                //case 6: return "CE";
                //case 7: return "TM";
                //// case ?: return "CP";
                //case 8: return "IB";
                case 9: return "CH";
                case 10: return "CN";

                default: return "";
            }
        }


        public string GetConcatMemberName(string title, string fn, string mn, string ln, bool displayName = false)
        {
            if (displayName)
                return ((((!string.IsNullOrEmpty(title) ? title : "") + ' ' + fn).Trim() + " " + mn).Trim() + " " + ln).Trim();
            else
                return (((fn + ' ' + mn).Trim() + " " + ln).Trim() + " " + (!string.IsNullOrEmpty(title) ? "(" + title + ")" : "")).Trim();
        }

        //private async Task LogAction_UserAuditTrail(UserAuditTrail oUserTrail)
        //{ // var oUserTrail = _masterContext.UserAuditTrail.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null && churchCode=="000000") || (c.AppGlobalOwnerId== oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId))
        //    if (oUserTrail != null)
        //    {
        //        using (var logCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
        //        {
        //            logCtx.UserAuditTrail.Add(oUserTrail);
        //            await logCtx.SaveChangesAsync();

        //            logCtx.Entry(oUserTrail).State = EntityState.Detached;
        //            ///
        //            //DetachAllEntities(logCtx);
        //            //logCtx.Dispose();
        //        }
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


        //private async Task LoadDashboardValues()
        //{

        //    ViewData["TotalSubsDenom"] = String.Format("{0:N0}", 0);
        //    ViewData["TotalSubsCong"] = String.Format("{0:N0}", 0);
        //    ViewData["TotalSysPriv"] = String.Format("{0:N0}", 0);
        //    ViewData["TotalSysRoles"] = String.Format("{0:N0}", 0);
        //    ViewData["TotSysProfiles"] = String.Format("{0:N0}", 0);
        //    ViewData["TotSubscribers"] = String.Format("{0:N0}", 0);
        //    ViewData["TotDbaseCount"] = String.Format("{0:N0}", 0);
        //    ViewData["TodaysAuditCount"] = String.Format("{0:N0}", 0);
        //    ViewData["TotClientProfiles"] = String.Format("{0:N0}", 0);
        //    ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}", 0);


        //    //using (var dashContext = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
        //    //{
        //    //    var res = await (from dummyRes in new List<string> { "X" }
        //    //                     join tago in dashContext.AppGlobalOwner.Where(c => c.Status == "A") on 1 equals 1 into _tago
        //    //                     join tcb in dashContext.MSTRChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN")) on 1 equals 1 into _tcb
        //    //                     join tsr in dashContext.UserRole.Where(c => c.RoleStatus == "A" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null) on 1 equals 1 into _tsr
        //    //                     join tsp in dashContext.UserPermission.Where(c => c.PermStatus == "A") on 1 equals 1 into _tsp
        //    //                     join tms in dashContext.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A") on 1 equals 1 into _tms
        //    //                     join tsubs in dashContext.AppSubscription.Where(c => c.Slastatus == "A") on 1 equals 1 into _tsubs
        //    //                     join ttc in dashContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date) on 1 equals 1 into _ttc
        //    //                     join tdb in dashContext.ClientAppServerConfig.Select(c => c.DbaseName).Distinct() on 1 equals 1 into _tdb
        //    //                     join tcln_a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A") on 1 equals 1 into _tcln_a
        //    //                     join tcln_d in (from a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A")
        //    //                                     from b in dashContext.UserProfileRole.Where(c => c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN")
        //    //                                     select a) on 1 equals 1 into _tcln_d
        //    //                     select new
        //    //                     {
        //    //                         cnt_tago = _tago.Count(),
        //    //                         cnt_tcb = _tcb.Count(),
        //    //                         cnt_tsr = _tsr.Count(),
        //    //                         cnt_tsp = _tsp.Count(),
        //    //                         cnt_tms = _tms.Count(),
        //    //                         cnt_tsubs = _tsubs.Count(),
        //    //                         cnt_tdb = _tdb.Count(),
        //    //                         cnt_ttc = _ttc.Count(),
        //    //                         cnt_tcln_d = _tcln_d.Count(),
        //    //                         cnt_tcln_a = _tcln_a.Count()
        //    //                     })
        //    //                .ToList().ToListAsync();

        //    //    ///
        //    //    ViewBag.TotalSubsDenom = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tago : 0));
        //    //    ViewBag.TotalSubsCong = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcb : 0));
        //    //    ViewBag.TotalSysPriv = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsp : 0));
        //    //    ViewBag.TotalSysRoles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsr : 0));
        //    //    ViewBag.TotSysProfiles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tms : 0));
        //    //    ViewBag.TotSubscribers = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsubs : 0));
        //    //    ViewBag.TotDbaseCount = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tdb : 0));
        //    //    ViewBag.TodaysAuditCount = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_ttc : 0));
        //    //    ViewBag.TotClientProfiles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcln_a : 0));
        //    //    ViewBag.TotClientProfiles_Admins = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcln_d : 0));
        //    //}




        //    //using (var dashContext = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
        //    //{
        //    //    var res = await (from dummyRes in new List<string> { "X" }
        //    //                     join tago in dashContext.AppGlobalOwner.Where(c => c.Status == "A") on 1 equals 1 into _tago
        //    //                     join tcb in dashContext.MSTRChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN")) on 1 equals 1 into _tcb
        //    //                     join tsr in dashContext.UserRole.Where(c => c.RoleStatus == "A" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null) on 1 equals 1 into _tsr
        //    //                     join tsp in dashContext.UserPermission.Where(c => c.PermStatus == "A") on 1 equals 1 into _tsp
        //    //                     join tms in dashContext.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A") on 1 equals 1 into _tms
        //    //                     join tsubs in dashContext.AppSubscription.Where(c => c.Slastatus == "A") on 1 equals 1 into _tsubs
        //    //                     join ttc in dashContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date) on 1 equals 1 into _ttc
        //    //                     join tdb in dashContext.ClientAppServerConfig.Select(c => c.DbaseName).Distinct() on 1 equals 1 into _tdb
        //    //                     join tcln_a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A") on 1 equals 1 into _tcln_a
        //    //                     join tcln_d in (from a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A")
        //    //                                     from b in dashContext.UserProfileRole.Where(c => c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN")
        //    //                                     select a) on 1 equals 1 into _tcln_d
        //    //                     select new
        //    //                     {
        //    //                         cnt_tago = _tago.Count(),
        //    //                         cnt_tcb = _tcb.Count(),
        //    //                         cnt_tsr = _tsr.Count(),
        //    //                         cnt_tsp = _tsp.Count(),
        //    //                         cnt_tms = _tms.Count(),
        //    //                         cnt_tsubs = _tsubs.Count(),
        //    //                         cnt_tdb = _tdb.Count(),
        //    //                         cnt_ttc = _ttc.Count(),
        //    //                         cnt_tcln_d = _tcln_d.Count(),
        //    //                         cnt_tcln_a = _tcln_a.Count()
        //    //                     })
        //    //                .ToList().ToListAsync();

        //    //    ///
        //    //    if (res.Count > 0)
        //    //    {
        //    //        ViewData["TotalSubsDenom"] = String.Format("{0:N0}", res[0].cnt_tago);
        //    //        ViewData["TotalSubsCong"] = String.Format("{0:N0}", res[0].cnt_tcb);
        //    //        ViewData["TotalSysPriv"] = String.Format("{0:N0}", res[0].cnt_tsp);
        //    //        ViewData["TotalSysRoles"] = String.Format("{0:N0}", res[0].cnt_tsr);
        //    //        ViewData["TotSysProfiles"] = String.Format("{0:N0}", res[0].cnt_tms);
        //    //        ViewData["TotSubscribers"] = String.Format("{0:N0}", res[0].cnt_tsubs);
        //    //        ViewData["TotDbaseCount"] = String.Format("{0:N0}", res[0].cnt_tdb);
        //    //        ViewData["TodaysAuditCount"] = String.Format("{0:N0}", res[0].cnt_ttc);
        //    //        ViewData["TotClientProfiles"] = String.Format("{0:N0}", res[0].cnt_tcln_a);
        //    //        ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}", res[0].cnt_tcln_d);
        //    //    }

        //    //    else
        //    //    {
        //    //        ViewData["TotalSubsDenom"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TotalSubsCong"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TotalSysPriv"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TotalSysRoles"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TotSysProfiles"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TotSubscribers"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TotDbaseCount"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TodaysAuditCount"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TotClientProfiles"] = String.Format("{0:N0}", 0);
        //    //        ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}", 0);
        //    //    }
        //    //}

        //}



        private async Task LoadDashboardValues()
        {
            using (var dashContext = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
            {
                var res = await (from dummyRes in new List<string> { "X" }
                                 join tago in dashContext.MSTRAppGlobalOwner.Where(c => c.Status == "A") on 1 equals 1 into _tago
                                 join tcb in dashContext.MSTRChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN")) on 1 equals 1 into _tcb
                                 join tsr in dashContext.UserRole.Where(c => c.RoleStatus == "A" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null) on 1 equals 1 into _tsr
                                 join tsp in dashContext.UserPermission.Where(c => c.PermStatus == "A") on 1 equals 1 into _tsp
                                 join tms in dashContext.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A") on 1 equals 1 into _tms
                                 join tsubs in dashContext.AppSubscription.Where(c => c.Slastatus == "A") on 1 equals 1 into _tsubs
                                 join ttc in dashContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date) on 1 equals 1 into _ttc
                                 join tdb in dashContext.ClientAppServerConfig.Select(c => c.DbaseName).Distinct() on 1 equals 1 into _tdb
                                 join tcln_a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A") on 1 equals 1 into _tcln_a
                                 join tcln_d in (from a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A")
                                                 from b in dashContext.UserProfileRole.Where(c => c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN")
                                                 select a) on 1 equals 1 into _tcln_d

                                 select new
                                 {
                                     cnt_tago = _tago.Count(),
                                     cnt_tcb = _tcb.Count(),
                                     cnt_tsr = _tsr.Count(),
                                     cnt_tsp = _tsp.Count(),
                                     cnt_tms = _tms.Count(),
                                     cnt_tsubs = _tsubs.Count(),
                                     cnt_tdb = _tdb.Count(),
                                     cnt_ttc = _ttc.Count(),
                                     cnt_tcln_d = _tcln_d.Count(),
                                     cnt_tcln_a = _tcln_a.Count()
                                 })
                            .ToList().ToListAsync();

                ///
                if (res.Count > 0)
                {
                    ViewData["TotalSubsDenom"] = String.Format("{0:N0}", res[0].cnt_tago);
                    ViewData["TotalSubsCong"] = String.Format("{0:N0}", res[0].cnt_tcb);
                    ViewData["TotalSysPriv"] = String.Format("{0:N0}", res[0].cnt_tsp);
                    ViewData["TotalSysRoles"] = String.Format("{0:N0}", res[0].cnt_tsr);
                    ViewData["TotSysProfiles"] = String.Format("{0:N0}", res[0].cnt_tms);
                    ViewData["TotSubscribers"] = String.Format("{0:N0}", res[0].cnt_tsubs);
                    ViewData["TotDbaseCount"] = String.Format("{0:N0}", res[0].cnt_tdb);
                    ViewData["TodaysAuditCount"] = String.Format("{0:N0}", res[0].cnt_ttc);
                    ViewData["TotClientProfiles"] = String.Format("{0:N0}", res[0].cnt_tcln_a);
                    ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}", res[0].cnt_tcln_d);
                }

                else
                {
                    ViewData["TotalSubsDenom"] = String.Format("{0:N0}", 0);
                    ViewData["TotalSubsCong"] = String.Format("{0:N0}", 0);
                    ViewData["TotalSysPriv"] = String.Format("{0:N0}", 0);
                    ViewData["TotalSysRoles"] = String.Format("{0:N0}", 0);
                    ViewData["TotSysProfiles"] = String.Format("{0:N0}", 0);
                    ViewData["TotSubscribers"] = String.Format("{0:N0}", 0);
                    ViewData["TotDbaseCount"] = String.Format("{0:N0}", 0);
                    ViewData["TodaysAuditCount"] = String.Format("{0:N0}", 0);
                    ViewData["TotClientProfiles"] = String.Format("{0:N0}", 0);
                    ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}", 0);
                }
            }
             
        }



        private static bool IsAncestor_ChurchBody(MSTRChurchBody oAncestorChurchBody, MSTRChurchBody oCurrChurchBody)
        {
            if (oAncestorChurchBody == null || oCurrChurchBody == null) return false;
            //string ChurchCodeFullPath { get; set; }  //R0000-0000-0000-0000-0000-0000 
            if (oAncestorChurchBody.Id == oCurrChurchBody.ParentChurchBodyId) return true;
            if (string.IsNullOrEmpty(oCurrChurchBody.RootChurchCode)) return false;

            string[] arr = new string[] { oCurrChurchBody.RootChurchCode };
            if (oCurrChurchBody.RootChurchCode.Contains("--")) arr = oCurrChurchBody.RootChurchCode.Split("--");

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


        private bool userAuthorized = false;
        private void SetUserLogged()
        {
            ////  oUserLogIn_Priv = TempData.Get<List<UserSessionPrivilege>>("UserLogIn_oUserPrivCol");

            //List<UserSessionPrivilege> oUserLogIn_Priv = TempData.ContainsKey("UserLogIn_oUserPrivCol") ?
            //                                                TempData["UserLogIn_oUserPrivCol"] as List<UserSessionPrivilege> : null;

            if (TempData.ContainsKey("UserLogIn_oUserPrivCol"))
            {
                var tempPrivList = TempData["UserLogIn_oUserPrivCol"] as string;
                if (string.IsNullOrEmpty(tempPrivList)) RedirectToAction("LoginUserAcc", "UserLogin");
                // De serialize the string to object
                oUserLogIn_Priv = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSessionPrivilege>>(tempPrivList);

                isCurrValid = oUserLogIn_Priv?.Count > 0;
                if (isCurrValid)
                {
                    ViewBag.oAppGloOwnLogged = oUserLogIn_Priv[0].AppGlobalOwner;
                    ViewBag.oChuBodyLogged = oUserLogIn_Priv[0].ChurchBody;
                    ViewBag.oUserLogged = oUserLogIn_Priv[0].UserProfile;

                    // check permission for Core life...  given the sets of permissions
                    userAuthorized = oUserLogIn_Priv.Count > 0; //(oUserLogIn_Priv.Find(x => x.PermissionName == "_A0__System_Administration" || x.PermissionName == "xxx") != null);
                }
            }
            else RedirectToAction("LoginUserAcc", "UserLogin");

            
        }


        private List<ChurchFaithTypeModel> GetChurchFaithTypes(int categIndex)  
        {  //FS == Faith Sect like Catholism, Protestantism, Pentecostalism/Charismatism, FC == Faith Class like Presbyterian, Methodist, Catholic, Charismatic
            
            var dc = categIndex == 1  ? "FS" : categIndex == 2 ? "FC" : null;             
            var ls = (
                   from t_cft in _context.ChurchFaithType //.Include(t => t.FaithTypeClass).Include(t => t.SubFaithTypes)
                        .Where(c => c.Category == dc || dc == null)
                   from t_cft_c in _context.ChurchFaithType.AsNoTracking().Where(c => c.Id == t_cft.FaithTypeClassId).DefaultIfEmpty()
                   select new ChurchFaithTypeModel()
                   {
                       oChurchFaithType = t_cft,
                       strFaithTypeClass = t_cft_c != null ? t_cft_c.FaithDescription : ""
                   })
                   .OrderBy(c => c.oChurchFaithType.FaithTypeClassId).ThenBy(c => c.oChurchFaithType.FaithDescription)
                   .ToList();
            return ls;
        }

        private List<DenominationVM> GetDenominations()
        {   //return _context.MSTRAppGlobalOwner.ToList();
           return  (
                   from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Include(t => t.FaithTypeCategory).Include(t => t.ChurchLevels) 
                        //.Where(c=> oAppOwnId==null || (oAppOwnId != null && c.Id == oAppOwnId))                                                                                                                                       // from t_cl in _context.MSTRChurchLevel.AsNoTracking().Where(c=> c.AppGlobalOwnerId==t_ago.Id ).DefaultIfEmpty()
                   from t_ft in _context.ChurchFaithType.AsNoTracking().Where(c => c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()
                   from t_ctr in _context.MSTRCountry.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()
                   select new DenominationVM()
                   {
                       oDenomination = t_ago,
                       lsChurchLevels = t_ago.ChurchLevels.ToList(),
                       strAppGloOwn = t_ago != null ? t_ago.OwnerName : "",
                       strCountry = t_ctr != null ? t_ctr.EngName : "",
                       strFaithTypeCategory = t_ft != null ? t_ft.FaithDescription : "",
                   }
                   )
                   .OrderBy(c => c.strCountry).ThenBy(c => c.strFaithTypeCategory).ThenBy(c => c.strAppGloOwn).ToList(); 
        }

        private DenominationVM GetDenomination(int oAppOwnId)
        {   //return _context.MSTRAppGlobalOwner.ToList();
            return (
                    from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Include(t => t.FaithTypeCategory).Include(t => t.ChurchLevels)
                         .Where(c => c.Id == oAppOwnId)                                                                                                                                       // from t_cl in _context.MSTRChurchLevel.AsNoTracking().Where(c=> c.AppGlobalOwnerId==t_ago.Id ).DefaultIfEmpty()
                    from t_ft in _context.ChurchFaithType.AsNoTracking().Where(c => c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()
                    from t_ctr in _context.MSTRCountry.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()
                    select new DenominationVM()
                    {
                        oDenomination = t_ago,
                        lsChurchLevels = t_ago.ChurchLevels.ToList(),
                        strAppGloOwn = t_ago != null ? t_ago.OwnerName : "",
                        strCountry = t_ctr != null ? t_ctr.EngName : "",
                        strFaithTypeCategory = t_ft != null ? t_ft.FaithDescription : "",
                    }
                    ).FirstOrDefault();
        }

        private List<MSTRChurchLevel> GetChurchLevels(int? oAppOwnId)
        {
            return (
                   from t_cl in _context.MSTRChurchLevel.AsNoTracking().Include(t => t.AppGlobalOwner)
                        .Where(c => c.AppGlobalOwnerId == oAppOwnId)
                  // from t_cb_c in _context.MSTRChurchBody.AsNoTracking().Where(c => c.ParentChurchBodyId == t_cl.Id).DefaultIfEmpty()
                   select new MSTRChurchLevel()
                   {
                       Id = t_cl.Id,
                       AppGlobalOwnerId = t_cl.AppGlobalOwnerId,
                       Name = t_cl.Name,
                       CustomName = t_cl.CustomName,
                       LevelIndex = t_cl.LevelIndex,
                       Created = t_cl.Created,
                       LastMod = t_cl.LastMod,
                      // OwnedByChurchBodyId = t_cl.OwnedByChurchBodyId,
                       SharingStatus = t_cl.SharingStatus,
                       //
                       strAppGlobalOwner = t_cl.AppGlobalOwner != null ? t_cl.AppGlobalOwner.OwnerName : ""
                   }
                   ).OrderBy(c => c.AppGlobalOwnerId).ThenBy(c => c.LevelIndex)
                   .ToList();
        }

        private List<ChurchBodyVM> GetCongregations(int? oAppOwnId)  // , int? oParCongId, bool oShowAllCong)
        {
            return (
                   from t_cb in _context.MSTRChurchBody.AsNoTracking()
                       // .Include(t => t.AppGlobalOwner).Include(t => t.ParentChurchBody)
                        .Include(t => t.Country).Include(t => t.SubChurchUnits)
                        .Where(c => c.AppGlobalOwnerId == oAppOwnId) // && (c.OrganisationType=="GB" || c.OrganisationType == "CN" && c.ChurchWorkStatus== "O"))  //church unit must be (O) operationalized...
                     //   ((oParCongId == null && oShowAllCong) || (oShowAllCong == false && c.ParentChurchBodyId == null) || c.ParentChurchBodyId == oParCongId))
                   from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Where(c => c.Id == t_cb.AppGlobalOwnerId).DefaultIfEmpty()
                   from t_cl in _context.MSTRChurchLevel.AsNoTracking().Where(c => c.Id == t_cb.ChurchLevelId).DefaultIfEmpty()
                   from t_cb_c in _context.MSTRChurchBody.AsNoTracking().Where(c => c.Id == t_cb.ParentChurchBodyId).DefaultIfEmpty()
                   select new ChurchBodyVM()
                   {
                       oChurchBody = t_cb,
                       strAppGloOwn = t_ago != null ? t_ago.OwnerName : "",
                       lsSubChurchBodies = t_cb.SubChurchUnits.ToList(),
                       strParentChurchBody = t_cb_c != null ? t_cb_c.Name : "",
                       strChurchLevel = t_cl != null ? !string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name : "",
                       strCountry = t_cb.Country != null ? t_cb.Country.EngName : "",
                       strCountryRegion = t_cb_c != null ? t_cb_c.Name : "",
                     //  strChurchType = t_cb.ChurchType == "CH" ? "Hierarchy" : "Congregation",
                       /// strAssociationType = t_cb.AssociationType == "N" ? "Networked" : "Freelance"
                   }
                   ).OrderBy(c => c.oChurchBody.ParentChurchBodyId).ThenBy(c => c.oChurchBody.Name).ToList();
        }

        private List<MSTRCountry> GetCountries()
        {
            //return _context.MSTRAppGlobalOwner.ToList();
            return (
                   from t_ctr in _context.MSTRCountry.AsNoTracking().Include(t => t.CountryRegions)
                   from t_rgn in _context.MSTRCountryRegion.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ctr.CtryAlpha3Code).DefaultIfEmpty()
                   select t_ctr 

                   //select new Country()
                   //{
                   //    oCountry = t_ctr,
                   //    lsCountryRegions = t_ctr.CountryRegions,
                   //    strCountry = t_ctr != null ? t_ctr.EngName : ""
                   //}

                   ).OrderBy(c => c.EngName).ToList();
        }

        private List<MSTRCountryRegion> GetCountryRegions(string oCtryId)  //(int? oCtryId)
        {  //  return _context.MSTRAppGlobalOwner.ToList();
            return (
                   from t_rgn in _context.MSTRCountryRegion.AsNoTracking().Include(t => t.Country).Where(c => c.CtryAlpha3Code == oCtryId)
                   select new MSTRCountryRegion()
                   {
                       Id = t_rgn.Id,
                       Name = t_rgn.Name,
                       CtryAlpha3Code = t_rgn.CtryAlpha3Code,
                       Created = t_rgn.Created,
                       LastMod = t_rgn.LastMod,
                       RegCode = t_rgn.RegCode,
                     //  OwnedByChurchBodyId = t_rgn.OwnedByChurchBodyId,
                       SharingStatus = t_rgn.SharingStatus,
                                              
                       oCountry = t_rgn != null ? t_rgn.Country : null,
                       strCountry = t_rgn != null ? t_rgn.Country != null ? t_rgn.Country.EngName : "" : ""
                   }
                   ).OrderBy(c => c.oCountry.EngName).ToList();
        }

        public JsonResult GetCountryRegionsByCountry(string ctryId, bool addEmpty = false) //(int? ctryId, bool addEmpty = false)
        {
            var countryList = _context.MSTRCountryRegion.Include(t => t.Country)
                .Where(c =>  c.CtryAlpha3Code == ctryId)  //c.Country.Display == true &&
                .OrderBy(c => c.Name)
                .ToList()
            .Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .OrderBy(c => c.Text)
            .ToList();

            /// if (addEmpty) countryList.Insert(0, new CountryRegion { Id = "", Name = "Select" });             
            //return Json(new SelectList(countryList, "Id", "Name"));  

            if (addEmpty) countryList.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            return Json(countryList);
        }


        private static List<UserPermission> CombineCollection(List<UserPermission> list1, List<UserPermission> list2,
           List<UserPermission> list3 = null, List<UserPermission> list4 = null, List<UserPermission> list5 = null)
        {
            if (list1 != null)
            {
                if (list2 != null) list1.AddRange(list2);
                if (list3 != null) list1.AddRange(list3);
                if (list4 != null) list1.AddRange(list4);
                if (list5 != null) list1.AddRange(list5);
            }
            
            //
            return list1;
        }

        private List<UserProfileModel> _GetUserProfileList(int? oAppGloOwnId = null, int ? oChurchBodyId = null, string proScope = "V", string subScope = "")//, int? roleLevel = -1)    profileCode : -1 = V /SUP_ADMN | 0 = V /other users  || 1 = C /??_ADMN || 2 = C / other users
        {
           // var p = _context.UserProfile.AsNoTracking().Where(c => c.ChurchBodyId == null && c.ProfileScope == "V");

            //var profiles = new List<UserProfileVM>();

            // null CB means ... SUPER USER .. get all accounts at toplevel  
            // null CB means ... SUPER USER .. get all accounts at toplevel  
            var profiles = (
               from t_up in _context.UserProfile.Include(t => t.ContactInfo).AsNoTracking().Where(c => c.ProfileScope == proScope && ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null) ||
                                                                              (c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == null && c.AppGlobalOwnerId != null) ||
                                                                              (c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId && c.AppGlobalOwnerId != null && c.ChurchBodyId != null)))  //.Include(t => t.ChurchMember)   "V"  proScope == "C" && subScope == "D"
                   from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                   from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                                .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id &&
                                ((proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5)) ||
                                 ((proScope == "C" && subScope == "D" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                                 ((proScope == "C" && subScope == "A" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && (c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15))
                                )).DefaultIfEmpty()

                       // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
                       //from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
                       //                  (c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5))
                       //             //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
                       //from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                       //             .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                       //from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                       //             .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                       //from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                       //             .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

                   select new UserProfileModel()
               {
                       // oUserProfile = t_up,

                       oUserProfile = new UserProfile()
                   {
                       Id = t_up.Id,
                       AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                       ChurchBodyId = t_up.ChurchBodyId,
                       ChurchMemberId = t_up.ChurchMemberId,
                       ChurchBody = t_up.ChurchBody,
                       // ChurchMember = t_up.ChurchMember,
                       OwnerUser = t_up.OwnerUser,
                       strChurchCode_AGO = t_cb != null ? (t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.GlobalChurchCode : "") : "",
                       strChurchCode_CB = t_cb != null ? t_cb.GlobalChurchCode : "",

                       Username = t_up.Username,
                       UserDesc = t_up.UserDesc,
                       Email = t_up.Email,
                       ContactInfo = t_up.ContactInfo,
                           // PhoneNum = t_up.ContactInfo != null ? t_up.ContactInfo.MobilePhone1 : "", //t_up.PhoneNum,
                           Pwd = t_up.Pwd,
                       PwdExpr = t_up.PwdExpr,
                       PwdSecurityQue = t_up.PwdSecurityQue,
                       PwdSecurityAns = t_up.PwdSecurityAns,
                       ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                       Strt = t_up.Strt,
                       strStrt = t_up.strStrt,
                       Expr = t_up.Expr,
                       strExpr = t_up.strExpr != null ? DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                       OwnerUserId = t_up.OwnerUserId,
                       // UserId = t_up.UserId,
                       UserScope = t_up.UserScope,
                       UserPhoto = t_up.UserPhoto,
                       ProfileScope = t_up.ProfileScope,
                       Created = t_up.Created,
                       CreatedByUserId = t_up.CreatedByUserId,
                       LastMod = t_up.LastMod,
                       LastModByUserId = t_up.LastModByUserId,
                       UserStatus = t_up.UserStatus,
                       strUserStatus = GetStatusDesc(t_up.UserStatus)

                   },

                       //  lsUserGroups = t_upg.UserGroups,
                       // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
                       // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

                       strUserProfile = t_up.UserDesc,
                       strChurchBody = t_cb != null ? t_cb.Name : "",
                       strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",

                       //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                       // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
                   }
               )
               //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
               .Distinct()
               .ToList(); 

            return profiles;
        }

        private List<UserProfileModel> GetUserProfileList_SysAdmin() //int? oAppGloOwnId = null, int? oChurchBodyId = null, string proScope = "V", string subScope = "" ... , int? roleLevel = -1)    profileCode : -1 = V /SUP_ADMN | 0 = V /other users  || 1 = C /??_ADMN || 2 = C / other users
        {
           // int? oAppGloOwnId = null; int? oChurchBodyId = null; 
            string proScope = "V"; 
            // null CB means ... SUPER USER .. get all accounts at toplevel...

            var profiles = (
               from t_up in _context.UserProfile.Include(t => t.ContactInfo).AsNoTracking().Where(c => c.ProfileScope == proScope && c.AppGlobalOwnerId == null && c.ChurchBodyId == null)
                                                                    // ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null) ||
                                                                    //  (c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == null && c.AppGlobalOwnerId != null) ||
                                                                    //  (c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId && c.AppGlobalOwnerId != null && c.ChurchBodyId != null)))  //.Include(t => t.ChurchMember)   "V"  proScope == "C" && subScope == "D"
               from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
               from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                            .Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.UserProfileId == t_up.Id &&
                                        (proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5))                          
                            ).DefaultIfEmpty()

                   // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
                   //from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
                   //                  (c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5))
                   //             //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
                   //from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                   //             .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                   //from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                   //             .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                   //from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                   //             .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

               select new UserProfileModel()
               {
                   // oUserProfile = t_up,

                   oUserProfile = new UserProfile()
                   {
                       Id = t_up.Id,
                       AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                       ChurchBodyId = t_up.ChurchBodyId,
                       ChurchMemberId = t_up.ChurchMemberId,
                       ChurchBody = t_up.ChurchBody,
                       // ChurchMember = t_up.ChurchMember,
                       OwnerUser = t_up.OwnerUser,
                       oUserRole = t_upr.UserRole,
                       strChurchCode_AGO = t_cb != null ? (t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.GlobalChurchCode : "") : "",
                       strChurchCode_CB = t_cb != null ? t_cb.GlobalChurchCode : "",

                       Username = t_up.Username,
                       UserDesc = t_up.UserDesc,
                       Email = t_up.Email,
                       ContactInfo = t_up.ContactInfo,
                       // PhoneNum = t_up.ContactInfo != null ? t_up.ContactInfo.MobilePhone1 : "", //t_up.PhoneNum,
                       Pwd = t_up.Pwd,
                       PwdExpr = t_up.PwdExpr,
                       PwdSecurityQue = t_up.PwdSecurityQue,
                       PwdSecurityAns = t_up.PwdSecurityAns,
                       ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                       Strt = t_up.Strt,
                       strStrt = t_up.strStrt,
                       Expr = t_up.Expr,
                       strExpr = t_up.strExpr != null ? DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                       OwnerUserId = t_up.OwnerUserId,
                    //   UserId = t_up.UserId,
                       UserScope = t_up.UserScope,
                       UserPhoto = t_up.UserPhoto,
                       ProfileScope = t_up.ProfileScope,
                       ProfileLevel = t_up.ProfileLevel,
                       Created = t_up.Created,
                       CreatedByUserId = t_up.CreatedByUserId,
                       LastMod = t_up.LastMod,
                       LastModByUserId = t_up.LastModByUserId,
                       UserStatus = t_up.UserStatus,
                       strUserStatus = GetStatusDesc(t_up.UserStatus)

                   },

                   //  lsUserGroups = t_upg.UserGroups,
                   // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
                   // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

                   strUserProfile = t_up.UserDesc,
                   strChurchBody = t_cb != null ? t_cb.Name : "",
                   strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",

                   //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                   // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
               }
               )
               //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
               .Distinct()
               .ToList();

            return profiles;
        }

        private List<UserProfileModel> GetUserProfileList_ChuAdmin(string subScope = "")//, int? roleLevel = -1)    profileCode : -1 = V /SUP_ADMN | 0 = V /other users  || 1 = C /??_ADMN || 2 = C / other users
        {
            var proScope = "C";

            // null CB means ... SUPER USER .. get all accounts at toplevel  
            // null CB means ... SUPER USER .. get all accounts at toplevel  
            var profiles = (
               from t_up in _context.UserProfile.Include(t => t.ContactInfo).AsNoTracking().Where(c => c.ProfileScope == proScope) // && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId )
                                                                             // (c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == null && c.AppGlobalOwnerId != null) ||
                                                                             // (c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId && c.AppGlobalOwnerId != null && c.ChurchBodyId != null)))  //.Include(t => t.ChurchMember)   "V"  proScope == "C" && subScope == "D"
               from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
               from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                            .Where(c => c.AppGlobalOwnerId == t_up.AppGlobalOwnerId && c.ChurchBodyId == t_up.ChurchBodyId && c.UserProfileId == t_up.Id &&
                           // ((proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5)) ||
                             ((proScope == "C" && subScope == "D" && (c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN")) && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                             ((proScope == "C" && subScope == "A" && (c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST")) && (c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15))
                            ).DefaultIfEmpty()

                   // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
                   //from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
                   //                  (c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5))
                   //             //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
                   //from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                   //             .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                   //from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                   //             .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                   //from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                   //             .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

               select new UserProfileModel()
               {
                   // oUserProfile = t_up,

                   oUserProfile = new UserProfile()
                   {
                       Id = t_up.Id,
                       AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                       ChurchBodyId = t_up.ChurchBodyId,
                       ChurchMemberId = t_up.ChurchMemberId,
                       ChurchBody = t_up.ChurchBody,
                       // ChurchMember = t_up.ChurchMember,
                       OwnerUser = t_up.OwnerUser,
                       oUserRole = t_upr.UserRole,
                       strChurchCode_AGO = t_cb != null ? (t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.GlobalChurchCode : "") : "",
                       strChurchCode_CB = t_cb != null ? t_cb.GlobalChurchCode : "",

                       Username = t_up.Username,
                       UserDesc = t_up.UserDesc,
                       Email = t_up.Email,
                       ContactInfo = t_up.ContactInfo,
                       // PhoneNum = t_up.ContactInfo != null ? t_up.ContactInfo.MobilePhone1 : "", //t_up.PhoneNum,
                       Pwd = t_up.Pwd,
                       PwdExpr = t_up.PwdExpr,
                       PwdSecurityQue = t_up.PwdSecurityQue,
                       PwdSecurityAns = t_up.PwdSecurityAns,
                       ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                       Strt = t_up.Strt,
                       strStrt = t_up.strStrt,
                       Expr = t_up.Expr,
                       strExpr = t_up.strExpr != null ? DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                       OwnerUserId = t_up.OwnerUserId,
                      // UserId = t_up.UserId,
                       UserScope = t_up.UserScope,
                       UserPhoto = t_up.UserPhoto,
                       ProfileScope = t_up.ProfileScope,
                       ProfileLevel = t_up.ProfileLevel,
                       Created = t_up.Created,
                       CreatedByUserId = t_up.CreatedByUserId,
                       LastMod = t_up.LastMod,
                       LastModByUserId = t_up.LastModByUserId,
                       UserStatus = t_up.UserStatus,
                       strUserStatus = GetStatusDesc(t_up.UserStatus)

                   },

                   //  lsUserGroups = t_upg.UserGroups,
                   // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
                   // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

                   strUserProfile = t_up.UserDesc,
                   strChurchBody = t_cb != null ? t_cb.Name : "",
                   strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",

                   //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                   // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
               }
               )
               //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
               .Distinct()
               .ToList();

            return profiles;
        }

        private List<UserProfileModel> GetUserProfileList_ClientChuAdmin(int? oAppGloOwnId, int? oChurchBodyId)//, string subScope = "", int? roleLevel = -1)    profileCode : -1 = V /SUP_ADMN | 0 = V /other users  || 1 = C /??_ADMN || 2 = C / other users
        {
            //checking users at the level of the subscriber...

            var proScope = "C"; var subScope = "A";

            // null CB means ... SUPER USER .. get all accounts at toplevel  
            // null CB means ... SUPER USER .. get all accounts at toplevel  
            var profiles = (
               from t_up in _context.UserProfile.Include(t => t.ContactInfo).AsNoTracking().Where(c => c.ProfileScope == proScope && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId)
                   // (c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == null && c.AppGlobalOwnerId != null) ||
                   // (c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oChurchBodyId && c.AppGlobalOwnerId != null && c.ChurchBodyId != null)))  //.Include(t => t.ChurchMember)   "V"  proScope == "C" && subScope == "D"
               from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
               from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                            .Where(c => c.AppGlobalOwnerId == t_up.AppGlobalOwnerId && c.ChurchBodyId == t_up.ChurchBodyId && c.UserProfileId == t_up.Id &&
                             // ((proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5)) ||
                             // ((proScope == "C" && subScope == "D" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                            
                            ((proScope == "C" && subScope == "A" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && (c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15))
                            ).DefaultIfEmpty()

                   // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
                   //from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
                   //                  (c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5))
                   //             //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
                   //from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                   //             .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                   //from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                   //             .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                   //from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                   //             .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

               select new UserProfileModel()
               {
                   // oUserProfile = t_up,

                   oUserProfile = new UserProfile()
                   {
                       Id = t_up.Id,
                       AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                       ChurchBodyId = t_up.ChurchBodyId,
                       ChurchMemberId = t_up.ChurchMemberId,
                       ChurchBody = t_up.ChurchBody,
                       // ChurchMember = t_up.ChurchMember,
                       OwnerUser = t_up.OwnerUser,

                       Username = t_up.Username,
                       UserDesc = t_up.UserDesc,
                       Email = t_up.Email,
                       ContactInfo = t_up.ContactInfo,
                       // PhoneNum = t_up.ContactInfo != null ? t_up.ContactInfo.MobilePhone1 : "", //t_up.PhoneNum,
                       Pwd = t_up.Pwd,
                       PwdExpr = t_up.PwdExpr,
                       PwdSecurityQue = t_up.PwdSecurityQue,
                       PwdSecurityAns = t_up.PwdSecurityAns,
                       ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                       Strt = t_up.Strt,
                       strStrt = t_up.strStrt,
                       Expr = t_up.Expr,
                       strExpr = t_up.strExpr != null ? DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                       OwnerUserId = t_up.OwnerUserId,
                     //  UserId = t_up.UserUserId,
                       UserScope = t_up.UserScope,
                       UserPhoto = t_up.UserPhoto,
                       ProfileScope = t_up.ProfileScope,
                       Created = t_up.Created,
                       CreatedByUserId = t_up.CreatedByUserId,
                       LastMod = t_up.LastMod,
                       LastModByUserId = t_up.LastModByUserId,
                       UserStatus = t_up.UserStatus,
                       strUserStatus = GetStatusDesc(t_up.UserStatus)

                   },

                   //  lsUserGroups = t_upg.UserGroups,
                   // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
                   // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

                   strUserProfile = t_up.UserDesc,
                   strChurchBody = t_cb != null ? t_cb.Name : "",
                   strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",

                   //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                   // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
               }
               )
               //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
               .Distinct()
               .ToList();

            return profiles;
        }

        private List<UserProfileVM> GetUserProfiles(int? oDenomId = null, int ? oChurchBodyId = null, string proScope = "V", string subScope = "")//, int? roleLevel = -1)    profileCode : -1 = V /SUP_ADMN | 0 = V /other users  || 1 = C /??_ADMN || 2 = C / other users
        {
           // var p = _context.UserProfile.AsNoTracking().Where(c => c.ChurchBodyId == null && c.ProfileScope == "V");

            var profiles = new List<UserProfileVM>();

            // null CB means ... SUPER USER .. get all accounts at toplevel  
            profiles = (
                   from t_up in _context.UserProfile.Include(t=>t.ContactInfo).AsNoTracking().Where(c =>c.ProfileScope == proScope && ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null) || 
                                                                                (c.AppGlobalOwnerId == oDenomId && c.ChurchBodyId == null && c.AppGlobalOwnerId != null) || 
                                                                                (c.AppGlobalOwnerId == oDenomId && c.ChurchBodyId == oChurchBodyId && c.AppGlobalOwnerId != null && c.ChurchBodyId != null)))  //.Include(t => t.ChurchMember)   "V"  proScope == "C" && subScope == "D"
                   from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                   from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                                    .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id &&
                                    ((proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5)) ||
                                     ((proScope == "C" && subScope == "D" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                                     ((proScope == "C" && subScope == "A" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && (c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15))
                                    )).DefaultIfEmpty()

                       // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
                       //from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
                       //                  (c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5))
                       //             //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
                       //from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                       //             .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                       //from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                       //             .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                       //from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                       //             .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

                   select new UserProfileVM()
                   {
                       // oUserProfile = t_up,

                       oUserProfile = new UserProfile()
                       {
                           Id = t_up.Id,
                           AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                           ChurchBodyId = t_up.ChurchBodyId,
                           ChurchMemberId = t_up.ChurchMemberId,
                           ChurchBody = t_up.ChurchBody,
                           // ChurchMember = t_up.ChurchMember,
                           OwnerUser = t_up.OwnerUser,

                           Username = t_up.Username,
                           UserDesc = t_up.UserDesc,
                           Email = t_up.Email,
                           ContactInfo = t_up.ContactInfo,
                          // PhoneNum = t_up.ContactInfo != null ? t_up.ContactInfo.MobilePhone1 : "", //t_up.PhoneNum,
                           Pwd = t_up.Pwd,
                           PwdExpr = t_up.PwdExpr,
                           PwdSecurityQue = t_up.PwdSecurityQue,
                           PwdSecurityAns = t_up.PwdSecurityAns,
                           ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                           Strt = t_up.Strt,
                           strStrt = t_up.strStrt,
                           Expr = t_up.Expr,
                           strExpr = t_up.strExpr != null ?
                                                                DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                           OwnerUserId = t_up.OwnerUserId,
                         //  UserId = t_up.UserId,
                           UserScope = t_up.UserScope,
                           UserPhoto = t_up.UserPhoto,
                           ProfileScope = t_up.ProfileScope,
                           Created = t_up.Created,
                           CreatedByUserId = t_up.CreatedByUserId,
                           LastMod = t_up.LastMod,
                           LastModByUserId = t_up.LastModByUserId,
                           UserStatus = t_up.UserStatus,
                           strUserStatus = GetStatusDesc(t_up.UserStatus)

                       },

                       //  lsUserGroups = t_upg.UserGroups,
                       // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
                       // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

                       strUserProfile = t_up.UserDesc,
                       strChurchBody = t_cb != null ? t_cb.Name : "",
                       strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",

                       //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                       // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
                   }
                   )
                   //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
                   .Distinct()
                   .ToList();



            //if (proScope == "V")  // // null CB means ... SUPER USER .. get all accounts at toplevel  
            //{ 
            //    profiles = (
            //       from t_up in _context.UserProfile.AsNoTracking().Where(c => c.AppGlobalOwnerId==null && c.ChurchBodyId == null && c.ProfileScope == "V")  //.Include(t => t.ChurchMember)
            //       from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
            //       from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
            //                        .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id  &&
            //                        ((c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5))                                    
            //                        ).DefaultIfEmpty()

            //      // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
            //       //from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
            //       //                  (c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5))
            //       //             //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
            //       //from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
            //       //             .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
            //       //from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
            //       //             .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
            //       //from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
            //       //             .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

            //       select new UserProfileVM()
            //       {
            //          // oUserProfile = t_up,

            //           oUserProfile = new UserProfile()
            //           {
            //               Id = t_up.Id,
            //               AppGlobalOwnerId = t_up.AppGlobalOwnerId,
            //               ChurchBodyId = t_up.ChurchBodyId,
            //               ChurchMemberId = t_up.ChurchMemberId,
            //               ChurchBody = t_up.ChurchBody,
            //               ChurchMember = t_up.ChurchMember,
            //               Owner = t_up.Owner,

            //               Username = t_up.Username,
            //               UserDesc = t_up.UserDesc,
            //               Email = t_up.Email,
            //               PhoneNum = t_up.PhoneNum,
            //               Pwd = t_up.Pwd,
            //               PwdExpr = t_up.PwdExpr,
            //               PwdSecurityQue = t_up.PwdSecurityQue,
            //               PwdSecurityAns = t_up.PwdSecurityAns,
            //               ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
            //               Strt = t_up.Strt,
            //               strStrt = t_up.strStrt,
            //               Expr = t_up.Expr,
            //               strExpr = t_up.strExpr != null ?
            //                                                    DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
            //               OwnerId = t_up.OwnerId,
            //               UserId = t_up.UserId,
            //               UserScope = t_up.UserScope,
            //               UserPhoto = t_up.UserPhoto,
            //               ProfileScope = t_up.ProfileScope,
            //               Created = t_up.Created,
            //               CreatedByUserId = t_up.CreatedByUserId,
            //               LastMod = t_up.LastMod,
            //               LastModByUserId = t_up.LastModByUserId,
            //               UserStatus = t_up.UserStatus,
            //               strUserStatus = GetStatusDesc(t_up.UserStatus)

            //           },

            //         //  lsUserGroups = t_upg.UserGroups,
            //         // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
            //         // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

            //             strUserProfile = t_up.UserDesc,
            //             strChurchBody = t_cb != null ? t_cb.Name : "",
            //             strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",

            //           //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
            //           // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
            //       }
            //       )
            //       //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
            //       .Distinct()
            //       .ToList() ;
            //}

            //else if (proScope == "C" && subScope == "D")  //Administrative account
            //{
            //    profiles = (
            //      from t_up in _context.UserProfile.AsNoTracking().Where(c => c.AppGlobalOwnerId==oDenomId &&  c.ProfileScope == "C")  //.Include(t => t.ChurchMember)
            //       from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
            //       from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole).Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
            //           // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
            //       from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
            //                        (c.RoleType == "CH_ADMN" || c.RoleType == "CF_ADMN") && (c.RoleLevel >= 6 && c.RoleLevel <= 10))
            //           //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
            //       from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
            //                   .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
            //      from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
            //                   .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
            //      from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
            //                   .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

            //      select new UserProfileVM()
            //      {
            //           // oUserProfile = t_up,

            //           oUserProfile = new UserProfile()
            //          {
            //              Id = t_up.Id,
            //              AppGlobalOwnerId = t_up.AppGlobalOwnerId,
            //              ChurchBodyId = t_up.ChurchBodyId,
            //              ChurchMemberId = t_up.ChurchMemberId,
            //              ChurchBody = t_up.ChurchBody,
            //              ChurchMember = t_up.ChurchMember,
            //              Owner = t_up.Owner,

            //              Username = t_up.Username,
            //              UserDesc = t_up.UserDesc,
            //              Email = t_up.Email,
            //              PhoneNum = t_up.PhoneNum,
            //              Pwd = t_up.Pwd,
            //              PwdExpr = t_up.PwdExpr,
            //              PwdSecurityQue = t_up.PwdSecurityQue,
            //              PwdSecurityAns = t_up.PwdSecurityAns,
            //              ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
            //              Strt = t_up.Strt,
            //              strStrt = t_up.strStrt,
            //              Expr = t_up.Expr,
            //              strExpr = t_up.strExpr != null ?
            //                                                   DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
            //              OwnerId = t_up.OwnerId,
            //              UserId = t_up.UserId,
            //              UserScope = t_up.UserScope,
            //              UserPhoto = t_up.UserPhoto,
            //              ProfileScope = t_up.ProfileScope,
            //              Created = t_up.Created,
            //              CreatedByUserId = t_up.CreatedByUserId,
            //              LastMod = t_up.LastMod,
            //              LastModByUserId = t_up.LastModByUserId,
            //              UserStatus = t_up.UserStatus,
            //              strUserStatus = GetStatusDesc(t_up.UserStatus)

            //          },

            //           //  lsUserGroups = t_upg.UserGroups,
            //           // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
            //           // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

            //           strUserProfile = t_up.UserDesc,

            //          strChurchBody = t_cb != null ? t_cb.Name : "",
            //          strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
            //           //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
            //           // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
            //       }
            //      )
            //      //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
            //      .ToList();
            //}

            //else if (proScope == "C" && subScope == "A")   //all clients accounts... custom created
            //{
            //    profiles = (
            //     from t_up in _context.UserProfile.AsNoTracking().Where(c => c.AppGlobalOwnerId == oDenomId && c.ProfileScope == "C")  //.Include(t => t.ChurchMember)
            //      from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
            //      from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole).Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
            //          // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
            //      from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
            //                       (c.RoleType == "CH_ADMN" || c.RoleType == "CH_RGSTR" || c.RoleType == "CH_ACCT" || c.RoleType == "CH_CUST" || c.RoleType == "CF_ADMN" || c.RoleType == "CF_RGSTR" || c.RoleType == "CF_ACCT" || c.RoleType == "CF_CUST") && (c.RoleLevel >= 6 && c.RoleLevel <= 10))
            //          //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
            //      from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
            //                  .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
            //     from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
            //                  .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
            //     from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
            //                  .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

            //     select new UserProfileVM()
            //     {
            //          // oUserProfile = t_up,

            //          oUserProfile = new UserProfile()
            //         {
            //             Id = t_up.Id,
            //             AppGlobalOwnerId = t_up.AppGlobalOwnerId,
            //             ChurchBodyId = t_up.ChurchBodyId,
            //             ChurchMemberId = t_up.ChurchMemberId,
            //             ChurchBody = t_up.ChurchBody,
            //             ChurchMember = t_up.ChurchMember,
            //             Owner = t_up.Owner,

            //             Username = t_up.Username,
            //             UserDesc = t_up.UserDesc,
            //             Email = t_up.Email,
            //             PhoneNum = t_up.PhoneNum,
            //             Pwd = t_up.Pwd,
            //             PwdExpr = t_up.PwdExpr,
            //             PwdSecurityQue = t_up.PwdSecurityQue,
            //             PwdSecurityAns = t_up.PwdSecurityAns,
            //             ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
            //             Strt = t_up.Strt,
            //             strStrt = t_up.strStrt,
            //             Expr = t_up.Expr,
            //             strExpr = t_up.strExpr != null ?
            //                                                  DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
            //             OwnerId = t_up.OwnerId,
            //             UserId = t_up.UserId,
            //             UserScope = t_up.UserScope,
            //             UserPhoto = t_up.UserPhoto,
            //             ProfileScope = t_up.ProfileScope,
            //             Created = t_up.Created,
            //             CreatedByUserId = t_up.CreatedByUserId,
            //             LastMod = t_up.LastMod,
            //             LastModByUserId = t_up.LastModByUserId,
            //             UserStatus = t_up.UserStatus,
            //             strUserStatus = GetStatusDesc(t_up.UserStatus)

            //         },

            //          //  lsUserGroups = t_upg.UserGroups,
            //          // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
            //          // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

            //          strUserProfile = t_up.UserDesc,

            //         strChurchBody = t_cb != null ? t_cb.Name : "",
            //         strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
            //          //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
            //          // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
            //      }
            //     )
            //     //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
            //     .ToList();
            //}

            //    //else
            //    //    profiles =  new List<UserProfileVM>();  //jux an empty list


                return profiles;
        }

       

        public ActionResult Index_CFT(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0) //, int? oParentId = null, int? id = null, int pageIndex = 1)             
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
                if (!this.userAuthorized) return View(new ChurchFaithTypeModel()); //retain view    
                if (oUserLogIn_Priv[0] == null) return View(new ChurchFaithTypeModel());
                if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner != null || oUserLogIn_Priv[0].ChurchBody != null) return View(new ChurchFaithTypeModel());
                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                var oLoggedRole = oUserLogIn_Priv[0].UserRole;

                //
                var strDesc = subSetIndex == 1 ? "Faith stream" : (subSetIndex == 2 ? "Faith category" : "All Faith type");
                var _userTask = "Viewed " + strDesc + " list";
                var oCFTModel = new ChurchFaithTypeModel(); //TempData.Keep();  
                                                            // int? oAppGloOwnId = null;
                var oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody;
                //
                int? oAppGloOwnId_Logged = null;
                int? oChuBodyId_Logged = null;
                if (oChuBody_Logged != null)
                {
                    oAppGloOwnId_Logged = oChuBody_Logged.AppGlobalOwnerId;
                    oChuBodyId_Logged = oChuBody_Logged.Id;

                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBody_Logged.Id; }
                    if (oAppGloOwnId == null) { oAppGloOwnId = oChuBody_Logged.AppGlobalOwnerId; }
                    //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
                    //
                    // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
                }

                //int? oCurrChuMemberId_LogOn = null;
                //ChurchMember oCurrChuMember_LogOn = null;

                //var currChurchMemberLogged = _clientContext.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
                //if (currChurchMemberLogged != null) //return View(oCFTModel);
                //{
                //    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
                //    oCurrChuMember_LogOn = currChurchMemberLogged;
                //}

                var oUserId_Logged = oLoggedUser.Id;

                oCFTModel.lsChurchFaithTypeModels = GetChurchFaithTypes(subSetIndex);
                oCFTModel.strCurrTask = strDesc;

                //                
                oCFTModel.oAppGloOwnId = oAppGloOwnId;
                oCFTModel.oChurchBodyId = oCurrChuBodyId;
                //
                oCFTModel.oUserId_Logged = oUserId_Logged;
                oCFTModel.oChurchBody_Logged = oChuBody_Logged;
                oCFTModel.oAppGloOwnId_Logged = oAppGloOwnId_Logged;

                // oCFTModel.oMemberId_Logged = oCurrChuMemberId_LogOn;
                //
                oCFTModel.setIndex = setIndex;
                oCFTModel.subSetIndex = subSetIndex;

                //
                ///
                ViewData["strAppName"] = "RhemaCMS";
                ViewData["strAppNameMod"] = "Admin Palette";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(oLoggedUser.UserDesc) ? oLoggedUser.UserDesc : "[Current user]";
                ///
                ViewData["oAppGloOwnId_Logged"] = oAppGloOwnId_Logged;
                ViewData["oChuBodyId_Logged"] = oChuBodyId_Logged;
                ViewData["strAppCurrUser_ChRole"] = oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUser_PhotoFilename"] = oLoggedUser.UserPhoto;
                ///

                _ = LoadDashboardValues();

                var tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id));

                return View(oCFTModel);
            }
        }

        [HttpGet]
        public IActionResult AddOrEdit_CFT(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0,
                                                 int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {

                var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                // int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                //int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                // int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                var strDesc = subSetIndex == 1 ? "Faith stream" : (subSetIndex == 2 ? "Faith category" : "All Faith type");
                var _userTask = "Attempted accessing/modifying "  + strDesc; // _userTask = "Attempted creating new Faith stream"; // _userTask = "Opened Faith stream-" + oCFT_MDL.oChurchFaithType.FaithDescription;
                if (StackAppUtilties.IsAjaxRequest(HttpContext.Request))
                {
                    var oCFT_MDL = new ChurchFaithTypeModel();
                    if (id == 0)
                    {
                        //create user and init... 
                        oCFT_MDL.oChurchFaithType = new ChurchFaithType();
                        oCFT_MDL.oChurchFaithType.Level = subSetIndex;  //subSetIndex == 2 ? 1 : 2; // 1;
                        oCFT_MDL.oChurchFaithType.Category = subSetIndex == 1 ? "FS" : "FC";

                        //if (setIndex > 0) oCFT_MDL.oChurchFaithType.Category = setIndex == 1 ? "FS" : "FC";
                        _userTask = "Attempted creating new " + strDesc + ", " + oCFT_MDL.oChurchFaithType.FaithDescription;
                    }

                    else
                    {
                        oCFT_MDL = (
                             from t_cft in _context.ChurchFaithType.AsNoTracking() //.Include(t => t.FaithTypeClass)
                                 .Where(x => x.Id == id)
                             from t_cft_c in _context.ChurchFaithType.AsNoTracking().Where(c => c.Id == t_cft.FaithTypeClassId).DefaultIfEmpty()
                             select new ChurchFaithTypeModel()
                             {
                                 oChurchFaithType = t_cft,
                                 strFaithTypeClass = t_cft_c != null ? t_cft_c.FaithDescription : ""                                 
                             }) 
                             .FirstOrDefault();
                         

                        if (oCFT_MDL == null)
                        {
                            Response.StatusCode = 500;
                            return PartialView("ErrorPage");
                        }

                        _userTask = "Opened " + strDesc + ", " + oCFT_MDL.oChurchFaithType.FaithDescription;
                    }
                                       
                    if (oCFT_MDL.oChurchFaithType == null) return null;

                    oCFT_MDL.setIndex = setIndex;
                    oCFT_MDL.subSetIndex = subSetIndex;
                    oCFT_MDL.oUserId_Logged = oUserId_Logged;   
                    oCFT_MDL.oAppGloOwnId_Logged = oAGOId_Logged;
                    oCFT_MDL.oChurchBodyId_Logged = oCBId_Logged;
                    //
                    oCFT_MDL.oAppGloOwnId = oAppGloOwnId;
                    oCFT_MDL.oChurchBodyId = oCurrChuBodyId;
                    var oCurrChuBody = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
                    oCFT_MDL.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null;

                    if (oCFT_MDL.subSetIndex == 2) // Denomination classes av church sects
                        oCFT_MDL = this.popLookups_CFT(oCFT_MDL, oCFT_MDL.oChurchFaithType);

                    var tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oUserId_Logged, tm, tm, oUserId_Logged, oUserId_Logged));

                    var _oCFT_MDL = Newtonsoft.Json.JsonConvert.SerializeObject(oCFT_MDL);
                    TempData["oVmCurrMod"] = _oCFT_MDL; TempData.Keep();
                      
                    return PartialView("_AddOrEdit_CFT", oCFT_MDL);                     
                }

                //page not found error
                Response.StatusCode = 500;
                return PartialView("ErrorPage");
            }
        }

        public ChurchFaithTypeModel popLookups_CFT(ChurchFaithTypeModel vm, ChurchFaithType oCurrCFT)
        {
            if (vm != null)
            {
                vm.lkpFaithTypeClasses = _context.ChurchFaithType.Where(c => c.Id != oCurrCFT.Id && c.Category == "FS" && !string.IsNullOrEmpty(c.FaithDescription))
                                              .OrderBy(c => c.FaithDescription).ToList()
                                              .Select(c => new SelectListItem()
                                              {
                                                  Value = c.Id.ToString(),
                                                  Text = c.FaithDescription
                                              })
                                              .ToList();

                vm.lkpFaithTypeClasses.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }

            return vm;
        }

        public ChurchFaithTypeModel Get_AddOrEdit_CFT(int? oDenomId = null, int? oCurrChuBodyId=null, int id = 0, int setIndex = 0, int subSetIndex = 0)
        {
            if (setIndex == 0) return null;  //oCFT_MDL; oCFT_MDL.setIndex = setIndex;                
                                             //
            //var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
            //var oUserProfile = oUserLogIn_Priv[0].UserProfile;
            //if (oCurrChuBodyLogOn == null) return null;   //prompt!
            //if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
            //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

            //// check permission for Core life...
            //if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
            //    return null;

            //int? oCurrChuMemberId_LogOn = null;
            //ChurchMember oCurrChuMember_LogOn = null;
            //if (oUserProfile == null) return null;
            //if (oUserProfile.ChurchMember == null) return null;

            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

            var oCFT_MDL = new ChurchFaithTypeModel();
            if (id == 0)
            {
                //create user and init... 
                oCFT_MDL.oChurchFaithType = new ChurchFaithType();
                oCFT_MDL.oChurchFaithType.Level = subSetIndex;  //subSetIndex == 2 ? 1 : 2; // 1;
                oCFT_MDL.oChurchFaithType.Category = subSetIndex == 1 ? "FS" : "FC"; 

                //if (setIndex > 0) oCFT_MDL.oChurchFaithType.Category = setIndex == 1 ? "FS" : "FC";
            }

            else
            {
                oCFT_MDL = (
                     from t_cft in _context.ChurchFaithType.AsNoTracking().Include(t => t.FaithTypeClass)
                         .Where(x => x.Id == id)
                     select new ChurchFaithTypeModel()
                     {
                         oChurchFaithType = t_cft,
                         strFaithTypeClass = t_cft.FaithTypeClass == null ? t_cft.FaithTypeClass.FaithDescription : ""
                     }
                    ).FirstOrDefault();
            }

            if (oCFT_MDL.oChurchFaithType == null) return null;

            oCFT_MDL.setIndex = setIndex;
          //  oCFT_MDL.oCurrAppGlobalOwner = oCurrChuBodyLogOn.AppGlobalOwner;
          //  oCFT_MDL.oChurchBody = oCurrChuBodyLogOn;
          //  oCFT_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
          //  oCFT_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;

            if (oCFT_MDL.setIndex == 2) // Denomination classes av church sects
                oCFT_MDL = this.popLookups_CFT(oCFT_MDL, oCFT_MDL.oChurchFaithType);


            var _oCFT_MDL = Newtonsoft.Json.JsonConvert.SerializeObject(oCFT_MDL);
            TempData["oVmCurrMod"] = _oCFT_MDL; TempData.Keep();


            //TempData["oVmCurr"] = oCFT_MDL;
            //TempData.Keep();


           // return PartialView("_AddOrEdit_CFT", oCFT_MDL);
             return oCFT_MDL;

        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_CFT(ChurchFaithTypeModel vmMod)
        {

            ChurchFaithType _oChanges = vmMod.oChurchFaithType;
            //   vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();

            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ChurchFaithTypeModel>(arrData) : vmMod;

            var oCFT = vmMod.oChurchFaithType;
           // oCFT.ChurchBody = vmMod.oChurchBody;

            try
            {
               // ModelState.Remove("oChurchFaithType.AppGlobalOwnerId");
               // ModelState.Remove("oChurchFaithType.ChurchBodyId");
                ModelState.Remove("oChurchFaithType.FaithTypeClassId");
                ModelState.Remove("oChurchFaithType.CreatedByUserId");
                ModelState.Remove("oChurchFaithType.LastModByUserId");
               // ModelState.Remove("oChurchFaithType.OwnerId");
               // ModelState.Remove("oChurchFaithType.UserId");

                // ChurchBody == null 

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", signOutToLogIn = false });

                var strDesc = _oChanges.Category == "FS" ? "Faith stream" : "Faith category";
                if (string.IsNullOrEmpty(_oChanges.FaithDescription)) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide description for " + strDesc.ToLower(), signOutToLogIn = false });
                }
                if (_oChanges.Category == "FC" && _oChanges.FaithTypeClassId==null)  // you can create 'Others' to cater for non-category
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the church faith stream.", signOutToLogIn = false });
                }
                 
                var tm = DateTime.Now;
                _oChanges.LastMod = tm;
                _oChanges.LastModByUserId = vmMod.oUserId_Logged;

                var _reset = _oChanges.Id == 0;

                //validate...
                var _userTask = "Attempted saving " + strDesc.ToLower() + ", " + _oChanges.FaithDescription.ToUpper();  // _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.FaithDescription.ToUpper() + " successfully";   //  _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.FaithDescription.ToUpper() + " successfully";

                using (var _cftCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    if (_oChanges.Id == 0)
                    {
                        var existCFT = _context.ChurchFaithType.Where(c => c.FaithDescription.ToLower() == _oChanges.FaithDescription.ToLower() && c.Level == _oChanges.Level).ToList();
                        if (existCFT.Count() > 0)
                        {
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " [" + _oChanges.FaithDescription + "] already exist.", signOutToLogIn = false });
                        }

                        _oChanges.Created = tm;
                        _oChanges.CreatedByUserId = vmMod.oUserId_Logged;

                        _cftCtx.Add(_oChanges);

                        ViewBag.UserMsg = "Saved " + strDesc.ToLower() + ", " + (!string.IsNullOrEmpty(_oChanges.FaithDescription) ? " [" + _oChanges.FaithDescription + "]" : "") + " successfully.";
                        _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.FaithDescription.ToUpper() + " successfully";  
                    }

                    else
                    {
                        var existCFT = _context.ChurchFaithType.Where(c => c.Id != _oChanges.Id && c.FaithDescription.ToLower() == _oChanges.FaithDescription.ToLower() && c.Level == _oChanges.Level).ToList();
                        if (existCFT.Count() > 0)
                        {
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " [" + _oChanges.FaithDescription + "] already exist.", signOutToLogIn = false });
                        }

                        //retain the pwd details... hidden fields
                        _cftCtx.Update(_oChanges);

                        //var _strDesc = strDesc.Length > 0 ? strDesc.Substring(0, 1).ToUpper() + strDesc.Substring(1) : "Church faith type ";

                        ViewBag.UserMsg = strDesc + " updated successfully.";
                        _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.FaithDescription.ToUpper() + " successfully";
                    }

                    //save church faith type first... 
                    await _cftCtx.SaveChangesAsync();
                     

                    DetachAllEntities(_cftCtx);
                }

               

                //audit...
                var _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: Faith stream", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vmMod.oCurrUserId_Logged, _tm, _tm, vmMod.oCurrUserId_Logged, vmMod.oCurrUserId_Logged));


                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep(); 
                 
                return Json(new { taskSuccess = true,  oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg, signOutToLogIn = false });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving church faith type details. Err: " + ex.Message, signOutToLogIn = false });
            }
        }

        public IActionResult Delete_CFT(int? loggedUserId, int id, int setIndex, int subSetIndex, bool forceDeleteConfirm = false)
        {
            var strDesc = subSetIndex == 1 ? "Faith stream" : "Faith category"; 
            // 
            var tm = DateTime.Now; var _tm = DateTime.Now; var _userTask = "Attempted saving " + strDesc;

            try
            {                
                var oCFT = _context.ChurchFaithType   .Where(c => c.Id == id).FirstOrDefault(); // .Include(c => c.ChurchUnits)
                if (oCFT == null)
                {
                    _userTask = "Attempted deleting " + strDesc.ToLower() + "," + oCFT.FaithDescription;  // var _userTask = "Attempted saving " + strDesc;
                    _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " to delete could not be retrieved." });
                }

                    var saveDelete = true;
                    // ensuring cascade delete where there's none!

                    //check CFC for the CFS
                    var oFaithCategories = _context.ChurchFaithType.Where(c => c.FaithTypeClassId == oCFT.Id).ToList();


                using (var _cftCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                { 

                    if (oFaithCategories.Count() > 0) //+ oCFT.ChurchLevels.Count + oCFT.AppSubscriptions.Count + oCFT.ChurchMembers.Count )
                    {
                        if (forceDeleteConfirm == false)
                        {
                        var strConnTabs = "Faith category";
                            saveDelete = false;
                        // check user privileges to determine... administrator rights
                        // log
                        _userTask = "Attempted deleting " + strDesc.ToLower() + "," + oCFT.FaithDescription;  // var _userTask = "Attempted saving " + strDesc;
                        _tm = DateTime.Now;
                        _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                        return Json(new
                            {
                                taskSuccess = false,
                                tryForceDelete = false,
                                oCurrId = id,
                                userMess = "Specified " + strDesc.ToLower() +
                                                            " to delete has been used elsewhere in the system [" + strConnTabs + "]. Delete cannot be done unless dependent-references are removed."
                            });

                                //super_admin task
                                //return Json(new { taskSuccess = false, tryForceDelete = true, oCurrId = id, userMess = "Specified " + strDesc.ToLower() + 
                                //       " has dependencies or links with other external data [Faith category]. Delete cannot be done unless child refeneces are removed. DELETE (cascade) anyway?" });
                        }


                        //to be executed only for higher privileges...
                        try
                        {
                            //check AGO... for each CFC 
                          foreach (var child in oFaithCategories.ToList())
                          {
                            // AGO cannot be DELETED indirectly...  do it directly:: has too many dependencies
                              var oAppGLoOwns = _context.MSTRAppGlobalOwner.Where(c => c.FaithTypeCategoryId == child.Id).ToList();
                              if (oAppGLoOwns.Count() > 0)
                              {
                                  foreach (var grandchild in oAppGLoOwns.ToList())
                                  {
                                      grandchild.FaithTypeCategoryId = null;
                                      grandchild.LastMod = tm;
                                      grandchild.LastModByUserId = loggedUserId;
                                  }
                              }

                            //grandchild dependencies done... delete child
                            _cftCtx.ChurchFaithType.Remove(child); //counter check this too... before delete
                        }                              
                    }

                    catch (Exception ex)
                    {
                        _userTask = "Attempted deleting " + strDesc.ToLower() + "," + oCFT.FaithDescription + " but FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                        _tm = DateTime.Now;
                        _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                        saveDelete = false; 
                        return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Error occured while deleting specified " + strDesc.ToLower() + ": " + ex.Message + ". Reload and try to delete again." });
                    }  
                }

                    //successful...
                    if (saveDelete)
                    {
                        _cftCtx.ChurchFaithType.Remove(oCFT);
                        _cftCtx.SaveChanges();  
                        
                        DetachAllEntities(_cftCtx);

                        _userTask = "Deleted " + strDesc.ToLower() + "," + oCFT.FaithDescription + " successfully";  // var _userTask = "Attempted saving " + strDesc;
                        _tm = DateTime.Now;
                        _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                        return Json(new { taskSuccess = true, tryForceDelete = false, oCurrId = oCFT.Id, userMess = strDesc + " successfully deleted." });                       
                    } 
                }
                 
                    _userTask = "Attempted deleting " + strDesc.ToLower() + "," + oCFT.FaithDescription + " but FAILED. Data unavailable.";  // var _userTask = "Attempted saving " + strDesc;
                    _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));
                    //
                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "No " + strDesc.ToLower() + " data available to delete. Try again" });
                }

            catch (Exception ex)
            {
                _userTask = "Attempted deleting " + strDesc + " [ID=" + id + "] but FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Failed deleting " + strDesc.ToLower() + ". Err: " + ex.Message });
            }
        }
 


        // AGO
        public ActionResult Index_AGO()  // int? setIndex = 1, int? subSetIndex = 0  int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0) //, int? oParentId = null, int? id = null, int pageIndex = 1)             
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                //try
                //{
                    // check permission 
                    var _oUserPrivilegeCol = oUserLogIn_Priv;
                    var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                    TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();
                    //
                    if (!this.userAuthorized) return View(new MSTRAppGlobalOwnerModel()); //retain view    
                    if (oUserLogIn_Priv[0] == null) return View(new MSTRAppGlobalOwnerModel());
                    if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner != null || oUserLogIn_Priv[0].ChurchBody != null) return View(new MSTRAppGlobalOwnerModel());
                    var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                    var oLoggedRole = oUserLogIn_Priv[0].UserRole;

                    // 
                    var strDesc = "Denomination (Church)";
                    var _userTask = "Viewed " + strDesc.ToLower() + " list";
                    var oAGOModel = new MSTRAppGlobalOwnerModel(); //TempData.Keep();  
                                                               // int? oAppGloOwnId = null;
                    var oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody;
                    //
                    int? oAppGloOwnId_Logged = null;
                    int? oChuBodyId_Logged = null;
                    if (oChuBody_Logged != null)
                    {
                        oAppGloOwnId_Logged = oChuBody_Logged.AppGlobalOwnerId;
                        oChuBodyId_Logged = oChuBody_Logged.Id;

                        // if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBody_Logged.Id; }
                        //if (oAppGloOwnId == null) { oAppGloOwnId = oChuBody_Logged.AppGlobalOwnerId; }
                        //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
                        //
                        // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
                    }

                    //int? oCurrChuMemberId_LogOn = null;
                    //ChurchMember oCurrChuMember_LogOn = null;

                    //var currChurchMemberLogged = _clientContext.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oUserProfile.ChurchMemberId).FirstOrDefault();
                    //if (currChurchMemberLogged != null) //return View(oAGOModel);
                    //{
                    //    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
                    //    oCurrChuMember_LogOn = currChurchMemberLogged;
                    //}

                    var oUserId_Logged = oLoggedUser.Id;
                    var lsAGOMdl = (
                        from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking() //.Include(t => t.ChurchLevels)
                        from t_cft in _context.ChurchFaithType.AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()  //.Include(t => t.FaithTypeClass)
                        from t_ctry in _context.MSTRCountry.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()

                        select new MSTRAppGlobalOwnerModel()
                        {
                            oAppGlobalOwn = t_ago,
                        // lsChurchLevels = t_ago.ChurchLevels,
                        //       
                            TotalChurchLevels = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == t_ago.Id),
                            TotalCongregations = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == t_ago.Id && c.Status == "A"),
                            // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 
                            strAppGloOwn = t_ago.OwnerName,
                            strFaithCategory = t_cft != null ? t_cft.FaithDescription : "",
                            strCountry = t_ctry != null ? t_ctry.EngName : "",
                            strSlogan = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(0, t_ago.Slogan.IndexOf("|"))).Replace("|", "") : t_ago.Slogan,
                            strSloganResponse = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(t_ago.Slogan.IndexOf("|"))).Replace("|", "") : "",
                        //strChurchStream = t_cft.FaithTypeClass != null ? t_cft.FaithTypeClass.FaithDescription : "",
                        //   
                            blStatusActivated = t_ago.Status == "A",
                            strStatus = GetStatusDesc(t_ago.Status)
                        })
                        .OrderBy(c => c.strCountry).OrderBy(c => c.strAppGloOwn)
                        .ToList();

                    oAGOModel.lsAppGlobalOwnModels = lsAGOMdl;


                    oAGOModel.strCurrTask = strDesc;

                    //                
                    //oAGOModel.oAppGloOwnId = oAppGloOwnId;
                    //oAGOModel.oChurchBodyId = oCurrChuBodyId;
                    //
                    oAGOModel.oUserId_Logged = oUserId_Logged;
                    oAGOModel.oChurchBody_Logged = oChuBody_Logged;
                    oAGOModel.oAppGloOwnId_Logged = oAppGloOwnId_Logged;

                // 
                ///
                ViewData["strAppName"] = "RhemaCMS";
                ViewData["strAppNameMod"] = "Admin Palette";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(oLoggedUser.UserDesc) ? oLoggedUser.UserDesc : "[Current user]";
                ///
                ViewData["oAppGloOwnId_Logged"] = oAppGloOwnId_Logged;
                ViewData["oChuBodyId_Logged"] = oChuBodyId_Logged;
                ViewData["strAppCurrUser_ChRole"] = oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUser_PhotoFilename"] = oLoggedUser.UserPhoto;
                ///
                 

                //refresh Dash values
                _ = LoadDashboardValues();

                var tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id));

                    return View("Index_AGO", oAGOModel);

                //}

                //catch (Exception ex)
                //{
                //    //page not found error
                //    Response.StatusCode = 500;
                //    return PartialView("ErrorPage");
                //} 
            }
        }

        [HttpGet]
        public IActionResult AddOrEdit_AGO(int id = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) // (int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            { 
                try
                {
                    if (StackAppUtilties.IsAjaxRequest(HttpContext.Request))
                    {
                        var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                        var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                        // int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                        //int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                        // int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                        oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                        oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                        oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                        var strDesc = "Denomination (Church)";
                        var _userTask = "Attempted accessing/modifying " + strDesc.ToLower();  // _userTask = "Attempted creating new denomination (church)"; // _userTask = "Opened denomination (church)-" + oCFT_MDL.oChurchFaithType.FaithDescription;
                        var oAGO_MDL = new MSTRAppGlobalOwnerModel();
                        if (id == 0)
                        {
                            //create user and init... 
                            oAGO_MDL.oAppGlobalOwn = new MSTRAppGlobalOwner();
                            oAGO_MDL.oAppGlobalOwn.TotalLevels = 1;
                            //oAGO_MDL.oAppGlobalOwn.Status = "A";
                            oAGO_MDL.blStatusActivated = true;

                            _userTask = "Attempted creating new " + strDesc.ToLower();
                        }

                        else
                        {
                            oAGO_MDL = (
                                 from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking() //.Include(t => t.ChurchLevels) 
                                     .Where(x => x.Id == id)
                                 from t_cft in _context.ChurchFaithType.Include(t => t.FaithTypeClass).AsNoTracking().Where(c => c.Category == "FC" && c.Id == t_ago.FaithTypeCategoryId).DefaultIfEmpty()
                                 from t_ctry in _context.MSTRCountry.AsNoTracking().Where(c => c.CtryAlpha3Code == t_ago.CtryAlpha3Code).DefaultIfEmpty()

                                 select new MSTRAppGlobalOwnerModel()
                                 {
                                     oAppGlobalOwn = t_ago,
                                     lsChurchLevels = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == t_ago.Id).ToList(),
                                 //       
                                    TotalChurchLevels = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == t_ago.Id),
                                     TotalCongregations = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == t_ago.Id && c.Status == "A"),
                                 // && c.IsActivated==true && c.ChurchWorkStatus=="O" &&    c.OrganisationType=="CN"),  //c.OrganisationType=="CH" && 
                                    strAppGloOwn = t_ago.OwnerName,
                                     strFaithCategory = t_cft != null ? t_cft.FaithDescription : "",
                                     strCountry = t_ctry != null ? t_ctry.EngName : "",
                                     strSlogan = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(0, t_ago.Slogan.IndexOf("|"))).Replace("|", "") : t_ago.Slogan,
                                     strSloganResponse = t_ago.Slogan.Contains("|") ? (t_ago.Slogan.Substring(t_ago.Slogan.IndexOf("|"))).Replace("|", "") : "",
                                     strChurchStream = t_cft.FaithTypeClass != null ? t_cft.FaithTypeClass.FaithDescription : "",
                                    //   
                                    blStatusActivated = t_ago.Status == "A",
                                    strStatus = GetStatusDesc(t_ago.Status)
                                 })
                                 .FirstOrDefault();

                            if (oAGO_MDL.oAppGlobalOwn == null)
                            {
                                Response.StatusCode = 500;
                                return PartialView("ErrorPage");
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


                            if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.PrefixKey))
                                oAGO_MDL.oAppGlobalOwn.PrefixKey = GetNextCodePrefixByAcronym_jsonString(oAGO_MDL.oAppGlobalOwn.Acronym);

                            //church code  
                            if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.GlobalChurchCode) && !string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.PrefixKey))
                            {
                                oAGO_MDL.oAppGlobalOwn.GlobalChurchCode = oAGO_MDL.oAppGlobalOwn.PrefixKey + string.Format("{0:D3}", 0);
                                oAGO_MDL.oAppGlobalOwn.RootChurchCode = oAGO_MDL.oAppGlobalOwn.GlobalChurchCode;
                            }

                            //root church code  
                            if (string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.RootChurchCode) && !string.IsNullOrEmpty(oAGO_MDL.oAppGlobalOwn.GlobalChurchCode))
                            oAGO_MDL.oAppGlobalOwn.RootChurchCode = oAGO_MDL.oAppGlobalOwn.GlobalChurchCode; 


                            _userTask = "Opened " + strDesc.ToLower() + ", " + oAGO_MDL.oAppGlobalOwn.OwnerName;
                        }


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
                        _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oUserId_Logged, tm, tm, oUserId_Logged, oUserId_Logged));

                        var _oAGO_MDL = Newtonsoft.Json.JsonConvert.SerializeObject(oAGO_MDL);
                        TempData["oVmCurrMod"] = _oAGO_MDL; TempData.Keep();


                        return PartialView("_AddOrEdit_AGO", oAGO_MDL);
                    }

                    //page not found error
                    Response.StatusCode = 500;
                    return PartialView("ErrorPage");
                }

                catch (Exception ex)
                {
                    //page not found error
                    Response.StatusCode = 500;
                    return PartialView("ErrorPage");
                }
            }
        }

        public MSTRAppGlobalOwnerModel popLookups_AGO(MSTRAppGlobalOwnerModel vm, MSTRAppGlobalOwner oCurrAGO)
        {
            if (vm == null || oCurrAGO == null) return vm;
            //
            vm.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            vm.lkpFaithCategories = _context.ChurchFaithType.Where(c => c.Category == "FC" && !string.IsNullOrEmpty(c.FaithDescription))
                                          .OrderBy(c => c.FaithDescription).ToList()
                                          .Select(c => new SelectListItem()
                                          {
                                              Value = c.Id.ToString(),
                                              Text = c.FaithDescription
                                          })
                                          .ToList();            
          //  vm.lkpFaithCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            vm.lkpCountries = _context.MSTRCountry.ToList()  //.Where(c => c.Display == true)
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

        public JsonResult GetFaithStreamByCategory(int? categoryId )
        {
            var fs = _context.ChurchFaithType.Include(t=>t.FaithTypeClass)
                .Where(c => c.Category == "FC" && c.Id == categoryId).FirstOrDefault();

            var res = fs != null;
            var _strRes = fs != null ? (fs.FaithTypeClass != null ? fs.FaithTypeClass.FaithDescription : "") : "";
            return Json(new { taskSuccess = res, strRes = _strRes });

            //if (addEmpty) countryList.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            //return Json(countryList);
        }

        public JsonResult GetNextCodePrefixByAcronym(string strAcronym )
        {
            var tempCode = strAcronym.ToUpper(); // + tempCnt; //+ string.Format("{0:N0}", tempCnt);
            var fsCount = _context.MSTRAppGlobalOwner.Count(c => c.RootChurchCode == tempCode);
            if (fsCount == 0) return Json(new { taskSuccess = true, strRes = tempCode });
            else
            {
                var tempCnt = 1; tempCode = strAcronym.ToUpper() + tempCnt; //+ string.Format("{0:N0}", tempCnt);
                fsCount = _context.MSTRAppGlobalOwner.Count(c => c.RootChurchCode == tempCode);
                var res = false;
                while (fsCount > 0 && fsCount < 10)
                {
                    tempCnt++; tempCode = strAcronym.ToUpper() + tempCnt; //+ string.Format("{0:N0}", tempCnt);
                    fsCount = _context.MSTRAppGlobalOwner.Count(c => c.RootChurchCode == tempCode);
                    //
                    res = fsCount == 0;
                }

                return Json(new { taskSuccess = res, strRes = tempCode });
            }             
        }

        public string GetNextCodePrefixByAcronym_jsonString(string strAcronym)
        {
             var tempCode = strAcronym.ToUpper(); // + tempCnt; // string.Format("{0:N0}", tempCnt);
            var fsCount = _context.MSTRAppGlobalOwner.Count(c => c.RootChurchCode == tempCode);
            if (fsCount == 0) return tempCode; // @"{'taskSuccess' : " + true + ", strRes :'" + tempCode + "'}";
            else
            {
                var tempCnt = 1; tempCode = strAcronym.ToUpper() + tempCnt; // + string.Format("{0:N0}", tempCnt);
                fsCount = _context.MSTRAppGlobalOwner.Count(c => c.RootChurchCode == tempCode);
                var res = false;
                while (fsCount > 0 && fsCount < 10)
                {
                    tempCnt++; tempCode = strAcronym.ToUpper() + tempCnt; //+ string.Format("{0:N0}", tempCnt);
                    fsCount = _context.MSTRAppGlobalOwner.Count(c => c.RootChurchCode == tempCode);
                    //
                    res = fsCount == 0;
                }

                return tempCode;  // @"{'taskSuccess' : " + res + ", strRes :'" + tempCode + "'}";
            }
        }

        public JsonResult GetNextGlobalChurchCodeByAcronym(string prefixCode, int? oAppGloOwnId)
        {
            var fsCount = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == oAppGloOwnId && c.OrganisationType != "CR");
            var tempCnt = fsCount + 1; var tempCode = prefixCode.ToUpper() + string.Format("{0:D3}", tempCnt);
            fsCount = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == oAppGloOwnId && c.GlobalChurchCode == tempCode);
            if (fsCount == 0) return Json(new { taskSuccess = true, strRes = tempCode });
            else
            {
                tempCnt++; tempCode = prefixCode.ToUpper() + string.Format("{0:D3}", tempCnt);
                fsCount = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == oAppGloOwnId && c.GlobalChurchCode == tempCode);
                var res = false;
                while (fsCount > 0 && fsCount < 10)
                {
                    tempCnt++; tempCode = prefixCode.ToUpper() + string.Format("{0:D3}", tempCnt);
                    fsCount = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == oAppGloOwnId && c.GlobalChurchCode == tempCode);
                    //
                    res = fsCount == 0;
                }

                return Json(new { taskSuccess = res, strRes = tempCode });
            }
        }

        public string GetNextGlobalChurchCodeByAcronym_jsonString(string prefixCode, int? oAppGloOwnId)
        {
            var fsCount = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == oAppGloOwnId && c.OrganisationType != "CR");
            var tempCnt = fsCount + 1; var tempCode = prefixCode.ToUpper() + string.Format("{0:D3}", tempCnt);
            fsCount = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == oAppGloOwnId && c.GlobalChurchCode == tempCode);
            if (fsCount == 0) return tempCode; // @"{'taskSuccess' : " + true + ", strRes :'" + tempCode + "'}"; 
            else
            {
                tempCnt++; tempCode = prefixCode.ToUpper() + string.Format("{0:D3}", tempCnt);
                fsCount = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == oAppGloOwnId && c.GlobalChurchCode == tempCode);
                var res = false;
                while (fsCount > 0 && fsCount < 10)
                {
                    tempCnt++; tempCode = prefixCode.ToUpper() + string.Format("{0:D3}", tempCnt);
                    fsCount = _context.MSTRChurchBody.Count(c => c.AppGlobalOwnerId == oAppGloOwnId && c.GlobalChurchCode == tempCode);
                    //
                    res = fsCount == 0;
                }

                return tempCode; // @"{'taskSuccess' : " + res + ", strRes :'" + tempCode + "'}";
            }
        }

        public JsonResult GetNextRootChurchCodeByParentCB(string prefixCode, int? oAppGloOwnId, int? oParChurchBodyId, string strCBChurchCode )
        {
            //get the church code
            //get the church code
            if (string.IsNullOrEmpty(strCBChurchCode))
            {
                    var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                    var jsCBChurchCode = GetNextGlobalChurchCodeByAcronym_jsonString(prefixCode, oAppGloOwnId);  // string json1 = @"{'Name':'James'}";
                    var jsOut = JsonConvert.DeserializeAnonymousType(jsCBChurchCode, template); 
             
                    if (jsOut != null)
                        if (bool.Parse(jsOut.taskSuccess) == true)
                            strCBChurchCode = jsOut.strRes; 
            }         

            var oParCB = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id==oParChurchBodyId && c.Status == "A" ).FirstOrDefault();
            if (oParCB != null && !string.IsNullOrEmpty(strCBChurchCode))
            { 
                if (!string.IsNullOrEmpty(oParCB.RootChurchCode))
                    return Json(new { taskSuccess = true, strRes = oParCB.RootChurchCode + "--" + strCBChurchCode });
            }

            return Json(new { taskSuccess = false, strRes = "" });
        }

        public string GetNextRootChurchCodeByParentCB_jsonString(string prefixCode, int? oAppGloOwnId, int? oParChurchBodyId, string strCBChurchCode )
        {
            //get the church code
            if (string.IsNullOrEmpty(strCBChurchCode))
                strCBChurchCode = GetNextGlobalChurchCodeByAcronym_jsonString(prefixCode, oAppGloOwnId);

            // var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
            //  var jsCBChurchCode = GetNextGlobalChurchCodeByAcronym_jsonString(prefixCode, oAppGloOwnId);  // string json1 = @"{'Name':'James'}";
            //var jsOut = JsonConvert.DeserializeAnonymousType(jsCBChurchCode, template);

            //if (jsOut != null)
            //    if (bool.Parse(jsOut.taskSuccess) == true)
            //        strCBChurchCode = jsOut.strRes;

            var oParCB = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oParChurchBodyId && c.Status == "A").FirstOrDefault();
            if (oParCB != null && !string.IsNullOrEmpty(strCBChurchCode))
            {
                if (!string.IsNullOrEmpty(oParCB.RootChurchCode))
                    return oParCB.RootChurchCode + "--" + strCBChurchCode; // @"{'taskSuccess' : " + true + ", strRes :'" + oParCB.RootChurchCode + "--" + strCBChurchCode + "'}"; 
            }

            return ""; // @"{'taskSuccess' : " + false + ", strRes :''}";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_AGO(MSTRAppGlobalOwnerModel vm)
        {
            var strDesc = "Denomination (Church)";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });
            if (vm.oAppGlobalOwn == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });

            MSTRAppGlobalOwner _oChanges = vm.oAppGlobalOwn;  // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();

            //check if the configured levels <= total levels under AppGloOwn
            var lsCL = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == _oChanges.Id).ToList();
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
            var lsCBs = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == _oChanges.Id).ToList();  

            if ((_oChanges.Id == 0 || (_oChanges.Id > 0 && lsCBs.Count() == 0)) && string.IsNullOrEmpty(_oChanges.PrefixKey) && string.IsNullOrEmpty(_oChanges.Acronym)) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide acronym or church prefix for " + strDesc.ToLower() });
            }
            if (_oChanges.FaithTypeCategoryId == null)  // you can create 'Others' to cater for non-category
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the church faith stream or category." });
            }

            if (_oChanges.CtryAlpha3Code == null)  // you can create 'Others' to cater for non-category
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the base country." });
            }


            //validations done!
            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<MSTRAppGlobalOwnerModel>(arrData) : vm;

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
                if (vm.ChurchLogoFile != null ) //&& _oChanges.ChurchLogo != null
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
                _oChanges.Status = vm.blStatusActivated ? "A" : "D";

                //
                _oChanges.Slogan = (!string.IsNullOrEmpty(vm.strSlogan) ? vm.strSlogan : "")  + 
                                                    (!string.IsNullOrEmpty(vm.strSlogan) && !string.IsNullOrEmpty(vm.strSloganResponse) ? "|" : "") + 
                                                                                (!string.IsNullOrEmpty(vm.strSloganResponse) ? vm.strSloganResponse : "");
                //
                //get the prefix, church code, root code from acronym
                //get the prefix code  
                if (string.IsNullOrEmpty(_oChanges.PrefixKey))
                {
                    //var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                    //var jsCode = GetNextCodePrefixByAcronym_jsonString(_oChanges.Acronym);  // string json1 = @"{'Name':'James'}";
                    //var jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                    //if (jsOut != null)
                    //    if (bool.Parse(jsOut.taskSuccess) == true)
                    //        _oChanges.PrefixKey = jsOut.strRes;

                    _oChanges.PrefixKey = GetNextCodePrefixByAcronym_jsonString(_oChanges.Acronym);
                }

                //church code  
                if (string.IsNullOrEmpty(_oChanges.GlobalChurchCode) && !string.IsNullOrEmpty(_oChanges.PrefixKey))
                {
                    _oChanges.GlobalChurchCode = _oChanges.PrefixKey + string.Format("{0:D3}", 0);
                    _oChanges.RootChurchCode = _oChanges.GlobalChurchCode;
                }

                //var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                //jsCode = GetNextGlobalChurchCodeByAcronym_jsonString(_oChanges.PrefixKey, _oChanges.Id);  // string json1 = @"{'Name':'James'}";
                //jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                //if (jsOut != null)
                //    if (bool.Parse(jsOut.taskSuccess) == true)
                //        _oChanges.GlobalChurchCode = jsOut.strRes;



                //root church code  
                if (string.IsNullOrEmpty(_oChanges.RootChurchCode) && !string.IsNullOrEmpty(_oChanges.GlobalChurchCode))
                {
                    _oChanges.RootChurchCode = _oChanges.GlobalChurchCode;
                }

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


                using (var _agoCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    if (_oChanges.Id == 0)
                    {
                        var existAGO = _context.MSTRAppGlobalOwner.Where(c => c.OwnerName.ToLower() == _oChanges.OwnerName.ToLower()).ToList();
                        if (existAGO.Count() > 0)
                            { return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " -- " + _oChanges.OwnerName + " already exist." }); }

                        _oChanges.Created = tm;
                        _oChanges.CreatedByUserId = vm.oUserId_Logged;

                        _agoCtx.Add(_oChanges);

                        ViewBag.UserMsg = "Saved " + strDesc.ToLower() + " " + (!string.IsNullOrEmpty(_oChanges.OwnerName) ? " -- " + _oChanges.OwnerName : "") + " successfully.";
                        _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                    }

                    else
                    {
                        var existAGO = _context.MSTRAppGlobalOwner.Where(c => c.Id != _oChanges.Id && c.OwnerName.ToLower() == _oChanges.OwnerName.ToLower()).ToList();
                        if (existAGO.Count() > 0)
                        {
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = strDesc + " -- " + _oChanges.OwnerName + " already exist."  });
                        }

                        //retain the pwd details... hidden fields
                        _agoCtx.Update(_oChanges);
                        //var _strDesc = strDesc.Length > 0 ? strDesc.Substring(0, 1).ToUpper() + strDesc.Substring(1) : "Denomination ";

                        ViewBag.UserMsg = strDesc + " updated successfully.";
                        _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                    }

                    //save denomination first... 
                    await _agoCtx.SaveChangesAsync();
                     

                    DetachAllEntities(_agoCtx);

                }
                 

                var _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));




                //auto-update the church levels
                using (var _clCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    var oChLevelCntAdd = 0; var oChLevelCntUpd = 0;
                    //  _userTask = "Attempted saving church level, " + _oChanges.ToUpper();  // _userTask = "Added new church level, " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated church level, " + _oChanges.OwnerName.ToUpper() + " successfully";
                    if (vmMod.oAppGlobalOwn.Id == 0)
                    {
                        for (int i = 1; i <= _oChanges.TotalLevels; i++) // oAGO.TotalLevels; i++)
                        {
                            MSTRChurchLevel oCL = new MSTRChurchLevel()
                            {
                                Name = "Level_" + i,
                                CustomName = "Level " + i,
                                LevelIndex = i,
                                AppGlobalOwnerId = _oChanges.Id,
                                SharingStatus = "N",
                                Created = DateTime.Now,
                                LastMod = DateTime.Now,
                            };
                            //
                            oChLevelCntAdd++;
                            _clCtx.Add(oCL);
                        }

                        if (oChLevelCntAdd > 0)
                        {
                            _userTask = "Added new " + oChLevelCntAdd + " church levels for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                            ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oChLevelCntAdd + " church levels. Customization may be necessary";
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= _oChanges.TotalLevels; i++)
                        {
                            var oExistCL = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == _oChanges.Id && c.Name == "Level_" + i).FirstOrDefault();
                            if (oExistCL == null && (countCL + oChLevelCntAdd ) < _oChanges.TotalLevels)  //add new ... the missing levels
                            {
                                MSTRChurchLevel oCL = new MSTRChurchLevel()
                                {
                                    Name = "Level_" + i,
                                    CustomName = "Level " + i,
                                    LevelIndex = i,
                                    AppGlobalOwnerId = _oChanges.Id,
                                    SharingStatus = "N",
                                    Created = DateTime.Now,
                                    LastMod = DateTime.Now,
                                };
                               
                                //
                                oChLevelCntAdd++;
                                _clCtx.Add(oCL);
                            }

                            // UPDATE unecessary!
                            //else if (oExistCL != null && (countCL + oChLevelCntAdd ) <= _oChanges.TotalLevels)  // upd
                            //{
                            //    oExistCL.Name = "Level_" + i;
                            //    oExistCL.CustomName = "Level " + i;
                            //    oExistCL.LevelIndex = i;
                            //    oExistCL.AppGlobalOwnerId = _oChanges.Id;
                            //    oExistCL.SharingStatus = "N";
                            //    oExistCL.LastMod = DateTime.Now;
                            //    //
                            //    oChLevelCntUpd++;
                            //    ctx.Update(oExistCL);
                            //}
                        }

                        if ((oChLevelCntAdd + oChLevelCntUpd) > 0)
                        {
                            if (oChLevelCntAdd > 0)
                            {
                                _userTask = "Added new " + oChLevelCntAdd + " church levels for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                                ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oChLevelCntAdd + " church levels. Customization may be necessary";
                            }

                            if (oChLevelCntUpd > 0)
                            {
                                _userTask = !string.IsNullOrEmpty(_userTask) ? _userTask + ". " : "" + "Updated " + oChLevelCntUpd + " church levels for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                                ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Denomination's " + oChLevelCntUpd + " church levels updated. Customization may be necessary.";
                            }
                        }
                    }

                    if ((oChLevelCntAdd + oChLevelCntUpd) > 0)
                    {
                        await _clCtx.SaveChangesAsync();

                        
                        DetachAllEntities(_clCtx);
                        
                        _tm = DateTime.Now;
                        _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: Church Level", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));
                    }

                }


                //auto-update the church root - church body : RCM000
                using (var _cbCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    var oCBCntAdd = 0; var oCBCntUpd = 0;
                    //  _userTask = "Attempted saving church level, " + _oChanges.ToUpper();  // _userTask = "Added new church level, " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated church level, " + _oChanges.OwnerName.ToUpper() + " successfully";

                    var oCL_1 = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == _oChanges.Id && c.LevelIndex == 1).FirstOrDefault();
                    if (vmMod.oAppGlobalOwn.Id == 0)
                    {
                        MSTRChurchBody oCB = new MSTRChurchBody()
                        {
                            Name = _oChanges.OwnerName,
                            OrganisationType = "CR",
                            AppGlobalOwnerId = _oChanges.Id,
                            ChurchLevelId = oCL_1 != null ? oCL_1.Id : (int?)null,
                            // CountryId = _oChanges.CountryId,
                            CtryAlpha3Code = _oChanges.CtryAlpha3Code,
                            CountryRegionId = null,
                            GlobalChurchCode = _oChanges.GlobalChurchCode,
                            RootChurchCode = _oChanges.RootChurchCode,
                            //ChurchUnitLogo = _oChanges.ChurchLogo,
                            ParentChurchBodyId = null,
                            Status = "A", 
                            Created = DateTime.Now,
                            LastMod = DateTime.Now,
                            CreatedByUserId = _oChanges.CreatedByUserId,
                            LastModByUserId = _oChanges.LastModByUserId
                        };                        

                        oCBCntAdd++;
                        _cbCtx.Add(oCB);

                        if (oCBCntAdd > 0)
                        {
                            _userTask = "Added Church Root unit for " +  strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                            ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oCBCntAdd + " church root unit";
                        }
                    }
                    else
                    {
                        var oCBList = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == _oChanges.Id && c.OrganisationType == "CR" && c.Status == "A").ToList();
                        if (oCBList.Count() == 0)
                        {
                            MSTRChurchBody oCB = new MSTRChurchBody()
                            {
                                Name = _oChanges.OwnerName,
                                OrganisationType = "CR",
                                AppGlobalOwnerId = _oChanges.Id,
                                ChurchLevelId = oCL_1 != null ? oCL_1.Id : (int?)null,
                                CtryAlpha3Code = _oChanges.CtryAlpha3Code,
                                CountryRegionId = null,
                                GlobalChurchCode = _oChanges.GlobalChurchCode,
                                RootChurchCode = _oChanges.RootChurchCode,
                                //ChurchUnitLogo = _oChanges.ChurchLogo,
                                ParentChurchBodyId = null,
                                Status = "A",
                                Created = DateTime.Now,
                                LastMod = DateTime.Now,
                                CreatedByUserId = _oChanges.CreatedByUserId,
                                LastModByUserId = _oChanges.LastModByUserId
                            };

                            oCBCntAdd++;
                            _cbCtx.Add(oCB);
                        }
                        else
                        {
                            var recUpdated = false;
                            if (string.Compare(oCBList[0].Name, _oChanges.OwnerName, true) != 0) { oCBList[0].Name = _oChanges.OwnerName; recUpdated = true; }
                            if (oCBList[0].AppGlobalOwnerId != _oChanges.Id) { oCBList[0].AppGlobalOwnerId = _oChanges.Id; recUpdated = true; }
                            if (oCBList[0].ChurchLevelId != (oCL_1 != null ? oCL_1.Id : (int?)null)) { oCBList[0].ChurchLevelId = (oCL_1 != null ? oCL_1.Id : (int?)null); recUpdated = true; }
                            if (oCBList[0].ParentChurchBodyId != null) { oCBList[0].ParentChurchBodyId = null; recUpdated = true; }
                            if (oCBList[0].CtryAlpha3Code != _oChanges.CtryAlpha3Code) { oCBList[0].CtryAlpha3Code = _oChanges.CtryAlpha3Code; recUpdated = true; }
                            if (string.Compare(oCBList[0].GlobalChurchCode, _oChanges.GlobalChurchCode, true) != 0) { oCBList[0].GlobalChurchCode = _oChanges.GlobalChurchCode; recUpdated = true; }
                            if (string.Compare(oCBList[0].RootChurchCode, _oChanges.RootChurchCode, true) != 0) { oCBList[0].RootChurchCode = _oChanges.RootChurchCode; recUpdated = true; }

                            if (recUpdated)
                            {
                                oCBList[0].LastMod = DateTime.Now;
                                oCBList[0].LastModByUserId = _oChanges.LastModByUserId;
                                //
                                oCBCntUpd++;
                                _cbCtx.Update(oCBList[0]);
                            }
                        }

                        if ((oCBCntAdd + oCBCntUpd) > 0)
                        {
                            if (oCBCntAdd > 0)
                            {
                                _userTask = "Added Church Root unit for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                                ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oCBCntAdd + " church root unit";
                            }

                            if (oCBCntUpd > 0)
                            {
                                _userTask = !string.IsNullOrEmpty(_userTask) ? _userTask + ". " : "" + "Updated Church Root unit for " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
                                ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Denomination's " + oCBCntUpd + " Church Root unit updated.";
                            }
                        }
                    }

                    if ((oCBCntAdd + oCBCntUpd) > 0)
                    {
                        await _cbCtx.SaveChangesAsync();

                        using (var agoCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                        
                         DetachAllEntities(_cbCtx); 

                        _tm = DateTime.Now;
                        _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: Church Unit", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));
                    }

                }

                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving denomination (church) details. Err: " + ex.Message  });
            }
        }

        public IActionResult Delete_AGO(int? loggedUserId, int id, bool forceDeleteConfirm = false)  // (int? loggedUserId, int id, int setIndex, int subSetIndex, bool forceDeleteConfirm = false)
        {
            var strDesc =  "Denomination (Church)"; var _tm = DateTime.Now; var _userTask = "Attempted saving denomination (church)";
            //
            try
            {
                var tm = DateTime.Now;
                //
                var oAGO = _context.MSTRAppGlobalOwner .Where(c => c.Id == id).FirstOrDefault(); // .Include(c => c.ChurchUnits)
                if (oAGO == null)
                {
                    _userTask = "Attempted deleting " + strDesc.ToLower() +  ", "  + oAGO.OwnerName;  // var _userTask = "Attempted saving denomination (church)";
                    _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " to delete could not be retrieved." });
                }

                var saveDelete = true;
                // ensuring cascade delete where there's none!

                //check for the CL, CB, UP, CSC and others
                var oChurchLevels = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oAGO.Id).ToList();
                var oChurchBodies = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAGO.Id).ToList();
                var oUserProfiles = _context.UserProfile.Where(c => c.AppGlobalOwnerId == oAGO.Id).ToList();
                var oClientServerConfigs = _context.ClientAppServerConfig.Where(c => c.AppGlobalOwnerId == oAGO.Id).ToList();


                using (var _agoCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                { 
                    if ((oChurchLevels.Count() + oChurchBodies.Count() + oUserProfiles.Count() + oClientServerConfigs.Count()) > 0)
                    {
                        var strConnTabs = oChurchLevels.Count() > 0 ? "Church level" : "";
                        strConnTabs = strConnTabs.Length > 0 ? strConnTabs + ", " : strConnTabs;
                        strConnTabs = oChurchLevels.Count() > 0 ? strConnTabs + "Church unit" : strConnTabs;
                        strConnTabs = oUserProfiles.Count() > 0 ? strConnTabs + ", User profile" : strConnTabs;
                        strConnTabs = oClientServerConfigs.Count() > 0 ? strConnTabs + ", Client server config" : strConnTabs;

                        if (forceDeleteConfirm == false)
                        {
                            saveDelete = false;
                            // check user privileges to determine... administrator rights
                            // log
                            _userTask = "Attempted deleting " + strDesc.ToLower() + ", " + oAGO.OwnerName;  // var _userTask = "Attempted saving denomination (church)";
                            _tm = DateTime.Now;
                            _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                             "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                            return Json(new
                            {
                                taskSuccess = false,
                                tryForceDelete = false,
                                oCurrId = id,
                                userMess = "Specified " + strDesc.ToLower() + " to delete has been used elsewhere in the system [" + strConnTabs + "]. Delete cannot be done unless dependent-references are removed."
                            });

                            //super_admin task
                            //return Json(new { taskSuccess = false, tryForceDelete = true, oCurrId = id, userMess = "Specified " + strDesc.ToLower() + 
                            //       " has dependencies or links with other external data [Faith category]. Delete cannot be done unless child refeneces are removed. DELETE (cascade) anyway?" });
                        }


                        //to be executed only for higher privileges...
                        try
                        {
                            //check AGO... for each CFC 
                            foreach (var child in oChurchLevels.ToList())
                            {
                                // CB cannot be DELETED indirectly...  do it directly:: has too many dependencies
                                var oCBs = _context.MSTRChurchBody.Where(c => c.ChurchLevelId == child.Id).ToList();
                                if (oCBs.Count() > 0)
                                {
                                    foreach (var grandchild in oCBs.ToList())
                                    {
                                        grandchild.ChurchLevelId = null;
                                        grandchild.LastMod = tm;
                                        grandchild.LastModByUserId = loggedUserId;
                                    }
                                }

                                //grandchild dependencies done... delete child
                                _agoCtx.MSTRChurchLevel.Remove(child); //counter check this too... before delete
                            }
                        }

                        catch (Exception ex)
                        {
                            _userTask = "Attempted deleting " + strDesc.ToLower() + ", " + oAGO.OwnerName + " but FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                            _tm = DateTime.Now;
                            _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                             "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));


                            saveDelete = false;
                            return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Error occured while deleting specified " + strDesc.ToLower() + ": " + ex.Message + ". Reload and try to delete again." });
                        }
                    }

                    //successful...
                    if (saveDelete)
                    {
                        _agoCtx.MSTRAppGlobalOwner.Remove(oAGO);
                        _agoCtx.SaveChanges();


                        DetachAllEntities(_agoCtx); 

                        _userTask = "Deleted " + strDesc.ToLower() + ", " + oAGO.OwnerName + " successfully";  // var _userTask = "Attempted saving " + strDesc;
                        _tm = DateTime.Now;
                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                        return Json(new { taskSuccess = true, tryForceDelete = false, oCurrId = oAGO.Id, userMess = strDesc + " successfully deleted." });
                    }

                    else
                    { DetachAllEntities(_agoCtx); return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "No " + strDesc.ToLower() + " data available to delete. Try again" }); }
                      
                }
               
            }

            catch (Exception ex)
            {
                _userTask = "Attempted deleting " + strDesc.ToLower() + ", [ID=" + id + "] but FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc , AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Failed deleting " + strDesc.ToLower() + ". Err: " + ex.Message });
            }
        }



        // CL
        public ActionResult Index_CL(int? oAppGloOwnId = null, int pageIndex = 1)  // , int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0) //, int? oParentId = null, int? id = null, int pageIndex = 1)             
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                var strDesc = "Church Level"; 
                var _userTask = "Viewed " + strDesc.ToLower() + " list";

                // check permission 
                var _oUserPrivilegeCol = oUserLogIn_Priv;
                var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();
                //
                if (!this.userAuthorized) return View(new MSTRChurchLevelModel()); //retain view    
                if (oUserLogIn_Priv[0] == null) return View(new MSTRChurchLevelModel());
                if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner != null || oUserLogIn_Priv[0].ChurchBody != null) return View(new MSTRChurchLevelModel());
                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                var oLoggedRole = oUserLogIn_Priv[0].UserRole;

                //
                var oUserId_Logged = oLoggedUser.Id;
                var oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody; 
                int? oAppGloOwnId_Logged = null; int? oChuBodyId_Logged = null;
                if (oChuBody_Logged != null)
                {
                    oAppGloOwnId_Logged = oChuBody_Logged.AppGlobalOwnerId;
                    oChuBodyId_Logged = oChuBody_Logged.Id;
                }
                                

                //
                var oCLModel = new MSTRChurchLevelModel(); //TempData.Keep();     // int? oAppGloOwnId = null;
                var lsCLModel = (
                        from t_cl in _context.MSTRChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == oAppGloOwnId) //.Include(t => t.AppGlobalOwner)
                        from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Where(c => c.Id == t_cl.AppGlobalOwnerId).DefaultIfEmpty()
                        from t_ci_ago in _context.MSTRContactInfo.Include(t => t.Country).AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cl.AppGlobalOwnerId && c.ChurchBodyId == null && c.Id == t_ago.ContactInfoId).DefaultIfEmpty()  

                        select new MSTRChurchLevelModel()
                        {
                            oChurchLevel = t_cl,
                            strAppGloOwn = t_ago.OwnerName + (!string.IsNullOrEmpty(t_ago.OwnerName) && t_ci_ago.Country != null ? ", " + t_ci_ago.Country.EngName : (t_ci_ago.Country != null ? t_ci_ago.Country.EngName : "")),
                        }
                       )
                       .OrderBy(c => c.oChurchLevel.AppGlobalOwnerId).ThenBy(c => c.oChurchLevel.LevelIndex)
                       .ToList();

                oCLModel.lsChurchLevelModels = lsCLModel;
                oCLModel.strCurrTask = strDesc;

                //                
                oCLModel.oAppGloOwnId = oAppGloOwnId;
                oCLModel.PageIndex = pageIndex;
                //oCLModel.oChurchBodyId = oCurrChuBodyId;
                //
                oCLModel.oUserId_Logged = oUserId_Logged;
                oCLModel.oChurchBody_Logged = oChuBody_Logged;
                oCLModel.oAppGloOwnId_Logged = oAppGloOwnId_Logged;


                // dashboard lookups...
                oCLModel.strAppName = "RhemaCMS"; ViewBag.strAppName = oCLModel.strAppName;
                oCLModel.strAppNameMod = "Admin Palette"; ViewBag.strAppNameMod = oCLModel.strAppNameMod;
                oCLModel.strAppCurrUser = oLoggedUser.UserDesc; ViewBag.strAppCurrUser = oCLModel.strAppCurrUser;  // "Dan Abrokwa"
                                                                                                                   //oUPModel.strChurchType = "CH"; ViewBag.strChurchType = oUPModel.strChurchType;
                                                                                                                   //oUPModel.strChuBodyDenomLogged = "Rhema Global Church"; ViewBag.strChuBodyDenomLogged = oUPModel.strChuBodyDenomLogged;
                                                                                                                   //oUPModel.strChuBodyLogged = "Rhema Comm Chapel"; ViewBag.strChuBodyLogged = oUPModel.strChuBodyLogged;

                oCLModel.lkpAppGlobalOwns = _context.MSTRAppGlobalOwner.Where(c => c.Status == "A")
                                             .OrderBy(c => c.OwnerName).ToList()
                                             .Select(c => new SelectListItem()
                                             {
                                                 Value = c.Id.ToString(),
                                                 Text = c.OwnerName
                                             })
                                             .ToList();

                //           
                ///
                ViewData["strAppName"] = "RhemaCMS";
                ViewData["strAppNameMod"] = "Admin Palette";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(oLoggedUser.UserDesc) ? oLoggedUser.UserDesc : "[Current user]";
                ///
                ViewData["oAppGloOwnId_Logged"] = oAppGloOwnId_Logged;
                ViewData["oChuBodyId_Logged"] = oChuBodyId_Logged;
                ViewData["strAppCurrUser_ChRole"] = oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUser_PhotoFilename"] = oLoggedUser.UserPhoto;
                ///


                //refresh Dash values
                _ = LoadDashboardValues();

                var tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                 "RCMS-Admin: " + strDesc , AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id));

                return View(oCLModel);
            }
        }

        [HttpGet]
        public IActionResult AddOrEdit_CL(int id = 0, int? oAppGloOwnId = null, int pageIndex = 1, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) // (int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0, int? oCLId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                if (StackAppUtilties.IsAjaxRequest(HttpContext.Request))
                {    
                    var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                    var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                    // int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                    //int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                    // int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                    oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                    oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                    oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                    var strDesc = "Church level"; 
                    var _userTask = "Attempted accessing/modifying " + strDesc.ToLower(); // _userTask = "Attempted creating new denomination (church)"; // _userTask = "Opened denomination (church)-" + oCFT_MDL.oChurchFaithType.FaithDescription;
                    var oCLModel = new MSTRChurchLevelModel();
                    if (id == 0)
                    {
                        oCLModel.oChurchLevel = new MSTRChurchLevel();
                        var oCLIndex = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oAppGloOwnId).Count() + 1;
                        var oAppOwn = _context.MSTRAppGlobalOwner.Find(oAppGloOwnId);
                        if (oAppOwn == null)
                        {
                            Response.StatusCode = 500;
                            return PartialView("ErrorPage");
                        }

                        oCLModel.oChurchLevel.Name = "Level_" + oCLIndex;
                        oCLModel.oChurchLevel.CustomName = "Level " + oCLIndex;
                        oCLModel.oChurchLevel.LevelIndex = oCLIndex;
                        oCLModel.oChurchLevel.AppGlobalOwnerId = oAppGloOwnId;
                        oCLModel.oChurchLevel.SharingStatus = "N"; 

                        oCLModel.oChurchLevel.Created = DateTime.Now;
                        oCLModel.oChurchLevel.LastMod = DateTime.Now;
                        //
                        oCLModel.strAppGloOwn = oAppOwn.OwnerName;

                        _userTask = "Attempted creating new " + strDesc .ToLower() + ", " + (oCLModel.oChurchLevel.CustomName != null ? oCLModel.oChurchLevel.CustomName : oCLModel.oChurchLevel.Name);
                    }

                    else
                    {
                        oCLModel = (
                             from t_cl in _context.MSTRChurchLevel.AsNoTracking().Where(x => x.Id == id)
                             from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Where(c => c.Id == t_cl.AppGlobalOwnerId).DefaultIfEmpty()
                             from t_ci_ago in _context.MSTRContactInfo.Include(t => t.Country).AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cl.AppGlobalOwnerId && c.ChurchBodyId == null && c.Id == t_ago.ContactInfoId).DefaultIfEmpty()

                             select new MSTRChurchLevelModel()
                             {
                                 oChurchLevel = t_cl,
                                 strAppGloOwn = t_ago.OwnerName + (!string.IsNullOrEmpty(t_ago.OwnerName) && t_ci_ago.Country != null ? ", " + t_ci_ago.Country.EngName : (t_ci_ago.Country != null ? t_ci_ago.Country.EngName : "")),
                             }
                       )
                         .FirstOrDefault();

                        if (oCLModel == null)
                        {
                            Response.StatusCode = 500;
                            return PartialView("ErrorPage");
                        }

                        _userTask = "Opened " + strDesc .ToLower() + ", " + (oCLModel.oChurchLevel.CustomName != null ? oCLModel.oChurchLevel.CustomName : oCLModel.oChurchLevel.Name);
                    }


                    // oCLModel.setIndex = setIndex;
                    // oCLModel.subSetIndex = subSetIndex;
                    oCLModel.oUserId_Logged = oUserId_Logged;
                    oCLModel.oAppGloOwnId_Logged = oAGOId_Logged;
                    oCLModel.oChurchBodyId_Logged = oCBId_Logged;
                    //
                     oCLModel.oAppGloOwnId = oAppGloOwnId;
                    // oCLModel.oChurchBodyId = oCurrChuBodyId;
                    oCLModel.PageIndex = pageIndex;

                    //  var oCurrChuBody = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
                    // oCLModel.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null;

                    if (oCLModel.subSetIndex == 2) // Church level classes av church sects
                        oCLModel = this.popLookups_CL(oCLModel, oCLModel.oChurchLevel);
                     
                    var tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oUserId_Logged, tm, tm, oUserId_Logged, oUserId_Logged));

                    var _oCLModel = Newtonsoft.Json.JsonConvert.SerializeObject(oCLModel);
                    TempData["oVmCurrMod"] = _oCLModel; TempData.Keep();

                    return PartialView("_AddOrEdit_CL", oCLModel);
                }

                //page not found error
                Response.StatusCode = 500;
                return PartialView("ErrorPage");
            }
        }

        public MSTRChurchLevelModel popLookups_CL(MSTRChurchLevelModel vm, MSTRChurchLevel oCurrCL)
        {
            if (vm == null || oCurrCL == null) return vm;
            //
            //vm.lkpStatuses = new List<SelectListItem>();
            //foreach (var dl in dlGenStatuses) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            vm.lkpAppGlobalOwns = _context.MSTRAppGlobalOwner.Where(c => c.Status == "A")
                                              .OrderBy(c => c.OwnerName).ToList()
                                              .Select(c => new SelectListItem()
                                              {
                                                  Value = c.Id.ToString(),
                                                  Text = c.OwnerName
                                              })
                                              .ToList();

           // vm.lkpAppGlobalOwns.Insert(0, new SelectListItem { Value = "", Text = "Select" });


            return vm;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_CL(MSTRChurchLevelModel vm)
        {
            var strDesc = "Church level";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = vm.PageIndex });
            if (vm.oChurchLevel == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = vm.PageIndex });

            MSTRChurchLevel _oChanges = vm.oChurchLevel;  // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();

            //check if the configured levels <= total levels under AppGloOwn
            var countCL = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId);
            var oAGO = _context.MSTRAppGlobalOwner.Find(_oChanges.AppGlobalOwnerId);
            if ( oAGO == null)  
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify denomination (church)", pageIndex = vm.PageIndex });

            if ((_oChanges.Id == 0 && countCL >= oAGO.TotalLevels) || (_oChanges.Id > 0 && countCL > oAGO.TotalLevels))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Total " + strDesc.ToLower() + "s allowed for denomination, " + oAGO.OwnerName + " [" + oAGO.TotalLevels + "] reached.", pageIndex = vm.PageIndex });
                       
            if (_oChanges.LevelIndex <= 0 || _oChanges.LevelIndex > oAGO.TotalLevels)  
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please indicate correct level index. Hint: Must be within total church levels [" + oAGO.TotalLevels + "]", pageIndex = vm.PageIndex });

            if (string.IsNullOrEmpty(_oChanges.Name))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the " + strDesc.ToLower() + " name", pageIndex = vm.PageIndex });


            // validations done!
            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<MSTRChurchLevelModel>(arrData) : vm;

            var oCL = vmMod.oChurchLevel;
            // oCL.ChurchBody = vmMod.oChurchBody; 

            try
            {
                ModelState.Remove("oChurchLevel.AppGlobalOwnerId");
                ModelState.Remove("oChurchLevel.Name");
                //
                //ModelState.Remove("oChurchBody.Id");
                //ModelState.Remove("oChurchBody.Name");
                //ModelState.Remove("oChurchBody.ChurchType");
                //ModelState.Remove("oChurchBody.OrganisationType");

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", pageIndex = vm.PageIndex });
                 
                 
                //
                var tm = DateTime.Now;
                _oChanges.LastMod = tm;
                _oChanges.LastModByUserId = vm.oUserId_Logged;

                var _reset = _oChanges.Id == 0;
                var oCLDesc = strDesc + ", " + (_oChanges.CustomName != null ? _oChanges.CustomName : _oChanges.Name);

                //validate...
                var _userTask = "Attempted saving "  + oCLDesc;  // _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";
               

                using (var _clCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {

                    if (_oChanges.Id == 0)
                        {
                            var existCL = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId &&  
                                                     (c.CustomName.ToLower() == _oChanges.CustomName.ToLower() || c.Name.ToLower() == _oChanges.Name.ToLower()));
                            if (existCL > 0)
                                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = oCLDesc + " already exist.", pageIndex = vm.PageIndex });

                            _oChanges.Created = tm;
                            _oChanges.CreatedByUserId = vm.oUserId_Logged;

                            _clCtx.Add(_oChanges);

                            ViewBag.UserMsg = "Saved " + oCLDesc + " successfully.";
                            _userTask = "Added new " + oCLDesc + " successfully";
                        }

                        else
                        {
                            var existCL = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId==_oChanges.AppGlobalOwnerId && c.Id != _oChanges.Id && 
                                                    (c.CustomName.ToLower() == _oChanges.CustomName.ToLower() || c.Name.ToLower() == _oChanges.Name.ToLower())) ;
                            if (existCL > 0) 
                                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = oCLDesc + " already exist.", pageIndex = vm.PageIndex });
                
                            //retain the pwd details... hidden fields


                            _clCtx.Update(_oChanges);
                            //var _strDesc = strDesc.Length > 0 ? strDesc.Substring(0, 1).ToUpper() + strDesc.Substring(1) : "Church level ";

                            ViewBag.UserMsg = oCLDesc + " updated successfully.";
                            _userTask = "Updated " + oCLDesc + " successfully";
                        }

                        //save denomination first... 
                        await _clCtx.SaveChangesAsync();


                    DetachAllEntities(_clCtx);
                }

                 

                var _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));


                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vm);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg, pageIndex = vm.PageIndex });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving denomination (church) details. Err: " + ex.Message, pageIndex = vm.PageIndex });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditBLK_CL(MSTRChurchLevelModel vm, IFormCollection f) //ChurchMemAttendList oList)      
                                                                                                      // public IActionResult Index_Attendees(ChurchMemAttendList oList) //List<ChurchMember> oList)  //, int? reqChurchBodyId = null, string strAttendee="M", string strLongevity="C" ) //, char? filterIndex = null, int? filterVal = null)
        { 
            var strDesc = "Church level";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = 2 });
            if (vm.lsChurchLevelModels == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = "No changes made to " + strDesc + " data.", pageIndex = vm.PageIndex });
            if (vm.lsChurchLevelModels.Count == 0) return Json(new { taskSuccess = false, oCurrId = "", userMess = "No changes made to " + strDesc + " data.", pageIndex = vm.PageIndex });
            //    if (vm.oChurchLevel == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again." });

            //  MSTRChurchLevel _oChanges = vm.oChurchLevel;  // vm = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vm; TempData.Keep();

            //check if the configured levels <= total levels under AppGloOwn 
            var oVal = f["oAppGloOwnId"].ToString();
            var oAGOId = !string.IsNullOrEmpty(oVal) ? int.Parse(oVal) : (int?)null;
            var countCL = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == oAGOId);
            var oAGO = _context.MSTRAppGlobalOwner.Find(oAGOId);
            if (oAGO == null)
                return Json(new { taskSuccess = false, oCurrId = -1, userMess = "Specify denomination (church)" });

            if (countCL > oAGO.TotalLevels)
                return Json(new { taskSuccess = false, oCurrId = -1, userMess = "Total " + strDesc.ToLower() + "s allowed for denomination, " + oAGO.OwnerName + " [" + oAGO.TotalLevels + "] exceeded.", pageIndex = vm.PageIndex });


            // return View(vm);
            if (ModelState.IsValid == false)
                return Json(new { taskSuccess = false, oCurrId = oAGOId, userMess = "Saving data failed. Please refresh and try again", pageIndex = vm.PageIndex });

            //if (vm == null)
            //    return Json(new { taskSuccess = false, userMess = "Data to update not found. Please refresh and try again", pageIndex = vm.PageIndex });

            //if (vm.lsChurchLevelModels == null)
            //    return Json(new { taskSuccess = false, userMess = "No changes made to attendance data.", pageIndex = vm.PageIndex });

            //if (vm.lsChurchLevelModels.Count == 0)
            //    return Json(new { taskSuccess = false, userMess = "No changes made to attendance data", pageIndex = vm.PageIndex });


            


            //if ((_oChanges.Id == 0 && countCL >= oAGO.TotalLevels) || (_oChanges.Id > 0 && countCL > oAGO.TotalLevels))
            //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Total " + strDesc.ToLower() + "s allowed for denomination, " + oAGO.OwnerName + " [" + oAGO.TotalLevels + "] reached." });

            //if (_oChanges.LevelIndex <= 0 || _oChanges.LevelIndex > oAGO.TotalLevels)
            //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please indicate correct level index. Hint: Must be within total church levels [" + oAGO.TotalLevels + "]" });

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

            var numErrAddExceedCnt = 0;
            var numErrUpdExceedCnt = 0;
            var numErrAddExistCnt = 0;
            var numErrUpdExistCnt = 0;


            using (var _clBulkCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
            {
                var oChLevelCntAdd = 0; var oChLevelCntUpd = 0;
                foreach (var d in vm.lsChurchLevelModels)
                {
                    if (d.oChurchLevel != null)
                    {
                        if (d.oChurchLevel.Id > 0)  // update
                        {
                            var err = false;
                            if ((countCL + oChLevelCntAdd) > oAGO.TotalLevels)   // + oChLevelCntUpd
                            { numErrUpdExceedCnt++; err = true;   }

                            if (err == false)
                            {
                                var existCL = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == d.oChurchLevel.AppGlobalOwnerId && c.Id != d.oChurchLevel.Id &&
                                            (c.CustomName.ToLower() == d.oChurchLevel.CustomName.ToLower() || c.Name.ToLower() == d.oChurchLevel.Name.ToLower()));

                                if (existCL > 0) { numErrUpdExistCnt++; err = true; }
                            }

                            if (err==false )
                            {
                                var oCL = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == d.oChurchLevel.AppGlobalOwnerId && c.Id == d.oChurchLevel.Id).FirstOrDefault(); // c.Name == "Level_" + i).FirstOrDefault();
                                if (oCL != null)  // && (countCL + oChLevelCntAdd + oChLevelCntUpd ) < oAGO.TotalLevels
                                {
                                    oCL.Name = d.oChurchLevel.Name; // "Level_" + i;
                                    oCL.CustomName = d.oChurchLevel.CustomName;  //"Level " + i;
                                    oCL.LevelIndex = d.oChurchLevel.LevelIndex; //i;
                                    oCL.Acronym = d.oChurchLevel.Acronym;
                                    oCL.AppGlobalOwnerId = d.oChurchLevel.AppGlobalOwnerId;
                                    oCL.SharingStatus = "N";
                                    oCL.LastMod = DateTime.Now;
                                    //
                                    oChLevelCntUpd++;
                                    _clBulkCtx.Update(oCL);
                                }
                            }                            
                        }

                        else if (d.oChurchLevel.Id == 0)  //add
                        {
                            var err = false;
                            if ((countCL + oChLevelCntAdd) >= oAGO.TotalLevels)  // + oChLevelCntUpd
                            { numErrAddExceedCnt++; err = true;  }

                            if (err == false)
                            { 
                                var existCL = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == d.oChurchLevel.AppGlobalOwnerId && 
                                            (c.CustomName.ToLower() == d.oChurchLevel.CustomName.ToLower() || c.Name.ToLower() == d.oChurchLevel.Name.ToLower()));

                                if (existCL > 0) { numErrAddExistCnt++; err = true; }
                            }
                           
                            if (err==false)
                            {
                                MSTRChurchLevel oCL = new MSTRChurchLevel()
                                {
                                    Name = d.oChurchLevel.Name,
                                    CustomName = d.oChurchLevel.CustomName,
                                    LevelIndex = d.oChurchLevel.LevelIndex,
                                    Acronym = d.oChurchLevel.Acronym,
                                    AppGlobalOwnerId = d.oChurchLevel.AppGlobalOwnerId, // oAGO.Id,
                                    SharingStatus = "N",
                                    Created = DateTime.Now,
                                    LastMod = DateTime.Now,
                                };

                                //
                                oChLevelCntAdd++;
                                _clBulkCtx.Add(oCL);
                            }                           
                        }                        
                    }
                }


                var _userTask = "";
                if ((oChLevelCntAdd + oChLevelCntUpd) > 0)
                {
                    if (oChLevelCntAdd > 0)
                    {
                        var strErrAdd = numErrAddExceedCnt > 0 ? numErrAddExceedCnt + " church levels could not be added. Total levels [" + oAGO.TotalLevels + "] reached." : "";
                        strErrAdd += numErrAddExistCnt > 0 ? numErrAddExistCnt + " church levels could not be added. Church level name already exist" : "";
                           
                        _userTask = "Added new " + oChLevelCntAdd + " church levels for " + strDesc.ToLower() + ", " + oAGO.OwnerName.ToUpper() + " successfully. " + strErrAdd;
                        ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oChLevelCntAdd + " church levels. Customization may be necessary. " + strErrAdd;
                    }

                    if (oChLevelCntUpd > 0)
                    {
                        var strErrUpd = numErrUpdExceedCnt > 0 ? numErrUpdExceedCnt + " church levels could not be updated. Total levels [" + oAGO.TotalLevels + "] exceeded." : "";
                        strErrUpd += numErrUpdExistCnt > 0 ? numErrUpdExistCnt + " church levels could not be updated. Church level name already exist" : "";

                        _userTask = !string.IsNullOrEmpty(_userTask) ? _userTask + ". " : "" + "Updated " + oChLevelCntUpd + " church levels for " + strDesc.ToLower() + ", " + oAGO.OwnerName.ToUpper() + " successfully. " + strErrUpd;
                        ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Denomination's " + oChLevelCntUpd + " church levels updated. Customization may be necessary. " + strErrUpd;
                    }

                    //save all...
                    await _clBulkCtx.SaveChangesAsync();

 

                    DetachAllEntities(_clBulkCtx); 

                    var _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: Church Level", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));


                    return Json(new { taskSuccess = true, userMess = ViewBag.UserMsg, pageIndex = vm.PageIndex });
                }                 

            }
                         
            return Json(new { taskSuccess = false, userMess = "Saving data failed. Please refresh and try again.", pageIndex = vm.PageIndex });
        }

        public IActionResult Delete_CL(int? loggedUserId, int id, bool forceDeleteConfirm = false)  // (int? loggedUserId, int id, int setIndex, int subSetIndex, bool forceDeleteConfirm = false)
        {
            var strDesc = "Church level "; var _tm = DateTime.Now; var _userTask = "Attempted deleting " + strDesc.ToLower();
            
            //
            try
            {
                var tm = DateTime.Now;
                //
                var oCL = _context.MSTRChurchLevel.Where(c => c.Id == id).FirstOrDefault(); // .Include(c => c.ChurchUnits)
                if (oCL == null)
                { 
                    _userTask = "Attempted deleting " + strDesc.ToLower();
                    _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " to delete could not be retrieved." });
                }

                var saveDelete = true;
                // ensuring cascade delete where there's none!

                //check for the AGO and CB
                var oChurchBodies = _context.MSTRChurchBody.Where(c => c.ChurchLevelId == oCL.Id).ToList();
                // var oMTs = _clientContext.TransferTypeChurchLevel.Where(c => c.ChurchLevelId == oCL.Id).ToList(); 


                using (var _clBulkCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {                    
                    if (oChurchBodies.Count() > 0)
                    {
                        var strConnTabs = oChurchBodies.Count() > 0 ? "Church unit" : "";
                        //strConnTabs = strConnTabs.Length > 0 ? strConnTabs + ", " : strConnTabs;
                        //strConnTabs = oMTs.Count() > 0 ? strConnTabs + "Church transfer" : strConnTabs;

                        if (forceDeleteConfirm == false)
                        {
                            saveDelete = false;
                            // check user privileges to determine... administrator rights
                            // log
                            _userTask = "Attempted deleting " + strDesc.ToLower() + ", " + (oCL.CustomName != null ? oCL.CustomName : oCL.Name);  // var _userTask = "Attempted saving denomination (church)";
                            _tm = DateTime.Now;
                            _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                             "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                            return Json(new
                            {
                                taskSuccess = false,
                                tryForceDelete = false,
                                oCurrId = id,
                                userMess = "Specified " + strDesc.ToLower() + " to delete has been used elsewhere in the system [" + strConnTabs + "]. Delete cannot be done unless dependent-references are removed."
                            });

                            //super_admin task
                            //return Json(new { taskSuccess = false, tryForceDelete = true, oCurrId = id, userMess = "Specified " + strDesc.ToLower() + 
                            //       " has dependencies or links with other external data [Faith category]. Delete cannot be done unless child refeneces are removed. DELETE (cascade) anyway?" });
                        }
                    }

                    //successful...
                    if (saveDelete)
                    {
                        _clBulkCtx.MSTRChurchLevel.Remove(oCL);
                        _clBulkCtx.SaveChanges(); 
                        
                       
                        DetachAllEntities(_clBulkCtx);

                        _userTask = "Deleted " + strDesc.ToLower() + ", " + (oCL.CustomName != null ? oCL.CustomName : oCL.Name) + " successfully";  // var _userTask = "Attempted saving " + strDesc;
                        _tm = DateTime.Now;
                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                        return Json(new { taskSuccess = true, tryForceDelete = false, oCurrId = oCL.Id, userMess = strDesc + " successfully deleted." });
                    }

                    else
                    {  DetachAllEntities(_clBulkCtx); return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "No " + strDesc.ToLower() + " data available to delete. Try again" }); }
            
                }

            }

            catch (Exception ex)
            {
                _userTask = "Attempted deleting " + strDesc.ToLower() + ", [ID=" + id + "] but FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Failed deleting " + strDesc.ToLower() + ". Err: " + ex.Message });
            }
        }



        public ActionResult Index_CB(int? oAppGloOwnId = null, int pageIndex = 1)  // int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0) //, int? oParentId = null, int? id = null, int pageIndex = 1)             
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
                if (!this.userAuthorized) return View(new MSTRChurchBodyModel()); //retain view    
                if (oUserLogIn_Priv[0] == null) return View(new MSTRChurchBodyModel());
                if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner != null || oUserLogIn_Priv[0].ChurchBody != null) return View(new MSTRChurchBodyModel());
                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                var oLoggedRole = oUserLogIn_Priv[0].UserRole;

                //
                var strDesc = "Church Unit (Congregation)";
                var _userTask = "Viewed " + strDesc.ToLower() + " list";
                var oCBModel = new MSTRChurchBodyModel(); //TempData.Keep();    // int? oAppGloOwnId = null;
                var oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody;
                //
                int? oAppGloOwnId_Logged = null;
                int? oChuBodyId_Logged = null;
                if (oChuBody_Logged != null)
                {
                    oAppGloOwnId_Logged = oChuBody_Logged.AppGlobalOwnerId;
                    oChuBodyId_Logged = oChuBody_Logged.Id; 
                }
                 
                var oUserId_Logged = oLoggedUser.Id;

                var lsCBMdl = (
                    from t_cb in _context.MSTRChurchBody.Include(t => t.Country).Include(t => t.CountryRegion).AsNoTracking()
                            .Where(c=> c.AppGlobalOwnerId == oAppGloOwnId && (c.OrganisationType=="CH" || c.OrganisationType == "CN")) //c.OrganisationType == "CR" ||  // jux for structure
                    from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Where(c => c.Id == t_cb.AppGlobalOwnerId)                   
                    from t_cl in _context.MSTRChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb.ChurchLevelId).DefaultIfEmpty()
                    from t_cb_p in _context.MSTRChurchBody.AsNoTracking()
                            .Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && (c.OrganisationType == "CR" || c.OrganisationType == "CH" || c.OrganisationType == "CN") && c.Id==t_cb.ParentChurchBodyId).DefaultIfEmpty()
                    from t_cl_p in _context.MSTRChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb_p.ChurchLevelId).DefaultIfEmpty()
                    from t_ci in _context.MSTRContactInfo.AsNoTracking().Where(c => c.AppGlobalOwnerId==t_cb.AppGlobalOwnerId && c.ChurchBodyId==t_cb.Id && c.Id == t_cb.ContactInfoId).DefaultIfEmpty() 
                    from t_ci_ago in _context.MSTRContactInfo.Include(t => t.Country).AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cl.AppGlobalOwnerId && c.ChurchBodyId == null && c.Id == t_ago.ContactInfoId).DefaultIfEmpty()
                         
                    select new MSTRChurchBodyModel()
                    {
                        oAppGlobalOwn = t_ago,
                        oChurchBody = t_cb, 
                        strOrgType = GetChuOrgTypeDesc(t_cb.OrganisationType),
                        strParentChurchBody = t_cb_p.Name,
                        //
                        strCountry = t_cb.Country != null ? t_cb.Country.EngName : "",
                        strCountryRegion = t_cb.CountryRegion != null ? t_cb.CountryRegion.Name : "", 
                        strAppGloOwn = t_ago.OwnerName + (!string.IsNullOrEmpty(t_ago.OwnerName) && t_ci_ago.Country != null ? ", " + t_ci_ago.Country.EngName : (t_ci_ago.Country != null ? t_ci_ago.Country.EngName : "")),
                        strChurchBody = t_cb.Name,
                        strParentCB_HeaderDesc = !string.IsNullOrEmpty(t_cl.CustomName) ? t_cl_p.CustomName : "Parent Unit", 
                        strChurchLevel = (t_cb.ChurchLevelId == null && t_cb.OrganisationType == "CR") ? "Church Root" : (!string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name),
                        numCLIndex = t_cl.LevelIndex, 
                       // strCongLoc = t_ci.Location + (!string.IsNullOrEmpty(t_ci.Location) && !string.IsNullOrEmpty(t_ci.City) ? ", " + t_ci.City : t_ci_ago.City),
                        strCongLoc = (!string.IsNullOrEmpty(t_ci.Location) && !string.IsNullOrEmpty(t_ci.City) ? t_ci.Location + ", " + t_ci.City : t_ci.Location + t_ci.City).Trim(),
                        strCongLoc2 = (t_cb.CountryRegion != null && t_cb.Country != null ? t_cb.CountryRegion.Name + ", " + t_cb.Country.EngName : t_cb.CountryRegion.Name + t_cb.Country.EngName).Trim(),
                        blStatusActivated = t_cb.Status == "A", 
                        dtCreated = t_cb.Created,
                        //   
                        strStatus = GetStatusDesc(t_ago.Status)
                    })
                    .OrderByDescending(c=>c.dtCreated) //.OrderBy(c => c.strCountry).OrderBy(c => c.numCLIndex).OrderBy(c => c.strChurchBody)
                    .ToList();
                 


                oCBModel.lsChurchBodyModels = lsCBMdl;
                oCBModel.strCurrTask = strDesc;

                //                
                oCBModel.oAppGloOwnId = oAppGloOwnId;
                oCBModel.PageIndex = pageIndex;
                //oCBModel.oChurchBodyId = oCurrChuBodyId;
                //
                oCBModel.oUserId_Logged = oUserId_Logged;
                oCBModel.oChurchBody_Logged = oChuBody_Logged;
                oCBModel.oAppGloOwnId_Logged = oAppGloOwnId_Logged;

                oCBModel.lkpAppGlobalOwns = _context.MSTRAppGlobalOwner.Where(c => c.Status == "A")
                                             .OrderBy(c => c.OwnerName).ToList()
                                             .Select(c => new SelectListItem()
                                             {
                                                 Value = c.Id.ToString(),
                                                 Text = c.OwnerName
                                             })
                                             .ToList();

                //  
                // dashboard lookups
                ///
                ViewData["strAppName"] = "RhemaCMS";
                ViewData["strAppNameMod"] = "Admin Palette";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(oLoggedUser.UserDesc) ? oLoggedUser.UserDesc : "[Current user]";
                ///
                ViewData["oAppGloOwnId_Logged"] = oAppGloOwnId_Logged;
                ViewData["oChuBodyId_Logged"] = oChuBodyId_Logged;
                ViewData["strAppCurrUser_ChRole"] = oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUser_PhotoFilename"] = oLoggedUser.UserPhoto;
                ///

                //refresh Dash values
                _ = LoadDashboardValues();

                var tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id));

                return View(oCBModel);
            }
        }

        [HttpGet]
        // public IActionResult AddOrEdit_CB(int id = 0, int? oAppOwnId = null, int? oParentCBId = null, int? oUserId_Logged = null) // int? oAGOId_Logged = null, int? oCBId_Logged = null, (int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        public IActionResult AddOrEdit_CB(int id = 0, int? oAppGloOwnId = null, int pageIndex = 1, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                try
                {
                 if (StackAppUtilties.IsAjaxRequest(HttpContext.Request)) 
                    {
                        var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                        var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                        // int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                        //int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                        // int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                        oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                        oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                        oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                        //var strDesc = "Church level";
                        //var _userTask = "Attempted accessing/modifying " + strDesc.ToLower(); // _userTask = "Attempted creating new denomination (church)"; // _userTask = "Opened denomination (church)-" + oCFT_MDL.oChurchFaithType.FaithDescription;
                        //var oCLModel = new MSTRChurchLevelModel();

                    //    var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                    //var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                    //// int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                    ////int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                    //// int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                    //oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                    //int? oCBId_Logged = null; // = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                    //int? oAGOId_Logged = null; // oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                   if (oAppGloOwnId == null)
                    {
                                Response.StatusCode = 500;
                                return PartialView("ErrorPage");
                            }

                            //create user and init... 
                    var oAGO = _context.MSTRAppGlobalOwner.Find(oAppGloOwnId);
                    if (oAGO == null)
                     {
                                Response.StatusCode = 500;
                                return PartialView("ErrorPage");
                            }


                        var strDesc = "Church unit";
                        var _userTask = "Attempted accessing/modifying " + strDesc.ToLower();
                        var oCBModel = new MSTRChurchBodyModel();

                        // var oCBLevelCount = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == oAGO.Id);

                        if (id == 0)
                        {  
                            //get CB parent
                            // var oCBPar = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oParentCBId).FirstOrDefault();
                            // oCBModel.oChurchBody = oCBPar;

                            //if (oCBPar != null) oCBModel.strParentChurchBody = oCBPar.Name + " (" + GetChuOrgTypeDetail(oCBPar.OrganisationType, false).ToString() + ")";
                            //string parCBCode = oCBPar.RootChurchCode;  // get the parent church body ... CB to be created first by Vendor... and picked up by the subscribers at the ChurchStructure ... congregation
                            //var oCLParCB = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCBPar.ChurchLevelId).FirstOrDefault();
                            //var oCLCB = oCLParCB != null ? _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.LevelIndex == oCLParCB.LevelIndex + 1).FirstOrDefault() : (MSTRChurchLevel)null;


                            //create user and init... 
                            oCBModel.oChurchBody = new MSTRChurchBody();
                            oCBModel.oChurchBody.AppGlobalOwnerId = oAppGloOwnId;
                            oCBModel.numCLIndex = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == oAppGloOwnId);  //use what's configured... not digit at AGO

                            //  oCBLevelCount = oCBLevelCount >= 2 ? oCBLevelCount - 2 : 0;  // start at the lowest CB level ... CN will need [Max_Lev - 1] head-units ie. less HQ .. requesting CB itself

                            //
                            //var currCnt = _context.MSTRChurchBody.AsNoTracking().Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Status == "A").Count() + 1; // (c.OrganisationType == "GB" || c.OrganisationType == "CN")  && (c.ChurchType=="CH" || c.ChurchType == "CF"))

                            //// GlobalChurchCode ::>>  [ RootChurchCode /Acronym ] - [7-digit] ... RCM-0000000, RCM-0000001, PCG-1234567, COP-1000000, ICGC-9999999
                            //var strGloChuCode = (!string.IsNullOrEmpty(oAppOwn.Acronym) ? oAppOwn.Acronym.ToUpper() + "-" : "") + currCnt.ToString();  //add preceding zero's

                            //// RootChurchCode ::>> RCM-000001--RCM-000001--RCM-000001--RCM-000001                                               
                            //var strCBFullCode = !string.IsNullOrEmpty(parCBCode) ? parCBCode.ToUpper() + "--" : "" + strGloChuCode;                    // var strLocalChuCode = (!string.IsNullOrEmpty(oAppOwn.Acronym) ? oAppOwn.Acronym.ToUpper() : "") + strCBCode;  //add preceding zero's ... 


                            //oCBModel.oChurchBody.CountryId = oCurrCtryId; 

                            oCBModel.oAppGlobalOwn = oAGO;

                            //church code  
                            if (!string.IsNullOrEmpty(oCBModel.oAppGlobalOwn.PrefixKey))
                            { 
                                var template = new { taskSuccess = String.Empty, strRes = String.Empty };   
                                var jsCode = GetNextGlobalChurchCodeByAcronym_jsonString(oCBModel.oAppGlobalOwn.PrefixKey, oCBModel.oChurchBody.AppGlobalOwnerId);  // string json1 = @"{'Name':'James'}";
                                oCBModel.oChurchBody.GlobalChurchCode = jsCode;

                                //var jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                                //if (jsOut != null)
                                //    if (bool.Parse(jsOut.taskSuccess) == true)
                                //        oCBModel.oChurchBody.GlobalChurchCode = jsOut.strRes;
                            }

                            // oCBModel.oChurchBody.RootChurchCode = strCBFullCode;
                            // oCBModel.oChurchBody.ChurchLevelId = oCLCB != null ? oCLCB.Id : (int?)null;
                            // oCBModel.oChurchBody.OrganisationType = // "CH", "CN";
                            //  oCBModel.oChurchBody.ParentChurchBodyId = oCBPar != null ? oCBPar.Id : (int?)null;
                             
                            oCBModel.oChurchBody.CtryAlpha3Code = oCBModel.oAppGlobalOwn.CtryAlpha3Code;
                            // oCBModel.oChurchBody.CountryRegionId = oCBPar != null ? oCBPar.CountryRegionId : (int?)null;
                            
                            oCBModel.oChurchBody.Status = "A";
                            oCBModel.blStatusActivated = true;
                            //
                            oCBModel.oChurchBody.Created = DateTime.Now;
                            oCBModel.oChurchBody.LastMod = DateTime.Now;
                            oCBModel.strAppGloOwn = oCBModel.oAppGlobalOwn.OwnerName;

                            _userTask = "Attempted creating new " + strDesc.ToLower() + ", " + oCBModel.oChurchBody.Name;
                        }

                        else
                        {
                            oCBModel = (
                                         from t_cb in _context.MSTRChurchBody.Include(t => t.Country).Include(t => t.CountryRegion).AsNoTracking()
                                            .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id==id && (c.OrganisationType == "CR" || c.OrganisationType == "CH" || c.OrganisationType == "CN"))
                                         from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Where(c => c.Id == t_cb.AppGlobalOwnerId)
                                         from t_cl in _context.MSTRChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb.ChurchLevelId).DefaultIfEmpty()
                                         
                                         from t_cb_p in _context.MSTRChurchBody.AsNoTracking()
                                                 .Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && (c.OrganisationType == "CR" || c.OrganisationType == "CH" || c.OrganisationType == "CN") && c.Id == t_cb.ParentChurchBodyId).DefaultIfEmpty()
                                         from t_cl_p in _context.MSTRChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.Id == t_cb_p.ChurchLevelId).DefaultIfEmpty()
                                         from t_ci in _context.MSTRContactInfo.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cb.AppGlobalOwnerId && c.ChurchBodyId == t_cb.Id && c.Id == t_cb.ContactInfoId).DefaultIfEmpty()
                                         from t_ci_ago in _context.MSTRContactInfo.Include(t => t.Country).AsNoTracking().Where(c => c.AppGlobalOwnerId == t_cl.AppGlobalOwnerId && c.ChurchBodyId == null && c.Id == t_ago.ContactInfoId).DefaultIfEmpty()

                                         select new MSTRChurchBodyModel()
                                         {
                                             oAppGlobalOwn = t_ago,
                                             oChurchBody = t_cb,
                                             strOrgType = GetChuOrgTypeDesc(t_cb.OrganisationType),
                                             strParentChurchBody = t_cb_p.Name,
                                             //
                                             strCountry = t_cb.Country != null ? t_cb.Country.EngName : "",
                                             strCountryRegion = t_cb.CountryRegion != null ? t_cb.CountryRegion.Name : "",
                                             strAppGloOwn = t_ago.OwnerName + (!string.IsNullOrEmpty(t_ago.OwnerName) && t_ci_ago.Country != null ? ", " + t_ci_ago.Country.EngName : (t_ci_ago.Country != null ? t_ci_ago.Country.EngName : "")),
                                             strChurchBody = t_cb.Name,
                                             strParentCB_HeaderDesc = t_cl_p.CustomName,
                                             strChurchLevel = !string.IsNullOrEmpty(t_cl.CustomName) ? t_cl.CustomName : t_cl.Name,
                                             numCLIndex = t_cl.LevelIndex,
                                            // strCongLoc = t_ci.Location + (!string.IsNullOrEmpty(t_ci.Location) && !string.IsNullOrEmpty(t_ci.City) ? ", " + t_ci.City : t_ci_ago.City),
                                             strCongLoc = (!string.IsNullOrEmpty(t_ci.Location) && !string.IsNullOrEmpty(t_ci.City) ? t_ci.Location + ", " + t_ci.City : t_ci.Location + t_ci.City).Trim(),
                                             strCongLoc2 = (t_cb.CountryRegion != null && t_cb.Country != null ? t_cb.CountryRegion.Name + ", " + t_cb.Country.EngName : t_cb.CountryRegion.Name + t_cb.Country.EngName).Trim(),
                                             blStatusActivated = t_cb.Status == "A", 
                                             //   
                                             strStatus = GetStatusDesc(t_ago.Status)
                                         })
                                         .FirstOrDefault();

                            if (oCBModel == null)
                            { Response.StatusCode = 500; return PartialView("ErrorPage"); }

                            //if (oCBModel.oAppGlobalOwn == null)
                            //{ Response.StatusCode = 500; return PartialView("ErrorPage"); }

                            if (oCBModel.oChurchBody == null)
                            { Response.StatusCode = 500; return PartialView("ErrorPage"); }


                            //church code  
                            if (string.IsNullOrEmpty(oCBModel.oChurchBody.GlobalChurchCode) && !string.IsNullOrEmpty(oCBModel.oAppGlobalOwn.PrefixKey))
                            {
                                //_oChanges.GlobalChurchCode = _oChanges.PrefixKey + string.Format("{0:D3}", 0);
                                //_oChanges.RootChurchCode = _oChanges.GlobalChurchCode;

                                //var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                                var jsCode = GetNextGlobalChurchCodeByAcronym_jsonString(oCBModel.oAppGlobalOwn.PrefixKey, oCBModel.oChurchBody.AppGlobalOwnerId);  // string json1 = @"{'Name':'James'}";
                                oCBModel.oChurchBody.GlobalChurchCode = jsCode;
                            }

                            //root church code  
                            if (string.IsNullOrEmpty(oCBModel.oChurchBody.RootChurchCode) && !string.IsNullOrEmpty(oCBModel.oChurchBody.GlobalChurchCode))
                            {
                                // _oChanges.RootChurchCode = _oChanges.GlobalChurchCode;

                                //var template = new { taskSuccess = String.Empty, strRes = String.Empty };
                                var jsCode = GetNextRootChurchCodeByParentCB_jsonString(oCBModel.oAppGlobalOwn.PrefixKey, oCBModel.oChurchBody.AppGlobalOwnerId, oCBModel.oChurchBody.ParentChurchBodyId, oCBModel.oChurchBody.GlobalChurchCode);
                                oCBModel.oChurchBody.RootChurchCode = jsCode;

                                //var jsOut = JsonConvert.DeserializeAnonymousType(jsCode, template);

                                //if (jsOut != null)
                                //    if (bool.Parse(jsOut.taskSuccess) == true)
                                //        oCBModel.oChurchBody.RootChurchCode = jsOut.strRes;
                            }

                           // oCBLevelCount = oCBModel.numCLIndex;  // oCBModel.numCLIndex != 0 ? oCBModel.numCLIndex - 1 : 0;
                            _userTask = "Opened " + strDesc.ToLower() + ", " + oCBModel.oChurchBody.Name;
                        }

                        
                        // oCBModel.setIndex = setIndex;

                        oCBModel.PageIndex = pageIndex ;
                        oCBModel.oUserId_Logged = oUserId_Logged;
                        oCBModel.oAppGloOwnId_Logged = oAGOId_Logged;
                        oCBModel.oChurchBodyId_Logged = oCBId_Logged;


                        /// set the lookups for the church bodies
                        ///  
                        //if (oCBLevels.Count > 0)
                        //{
                        //    ViewBag.Filter_fr_fn = ViewBag.Filter_fn = !string.IsNullOrEmpty(oCBLevels[0].CustomName) ? oCBLevels[0].CustomName : oCBLevels[6].Name;
                        //   



                        oCBModel.oCBLevelCount = oCBModel.numCLIndex - 1;        // oCBLevelCount -= 2;  // less requesting CB
                        List<MSTRChurchLevel> oCBLevelList = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.LevelIndex > 0 && c.LevelIndex < oCBModel.numCLIndex).ToList().OrderBy(c=>c.LevelIndex).ToList();
                        ///
                        if (oCBModel.oCBLevelCount > 0 && oCBLevelList.Count > 0)
                        {
                            oCBModel.strChurchLevel_1 = !string.IsNullOrEmpty(oCBLevelList[0].CustomName) ? oCBLevelList[0].CustomName : oCBLevelList[0].Name;
                            ViewBag.strChurchLevel_1 = oCBModel.strChurchLevel_1;
                            ///
                            var oCB_1 = _context.MSTRChurchBody.Include(t => t.ChurchLevel)
                                              .Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && // c.Status == "A" && 
                                                    c.ChurchLevel.LevelIndex == 1 && c.OrganisationType == "CR") //c.ChurchLevelId == oCBLevelList[0].Id &&
                                              .FirstOrDefault();

                            if (oCB_1 != null)
                                { oCBModel.ChurchBodyId_1 = oCB_1.Id;  oCBModel.strChurchBody_1 = oCB_1.Name + " [Church Root]";  }

                            ViewBag.ChurchBodyId_1 = oCBModel.ChurchBodyId_1; 
                            ViewBag.strChurchBody_1 = oCBModel.strChurchBody_1;

                            ///
                            if (oCBModel.oCBLevelCount > 1)
                            {
                                oCBModel.strChurchLevel_2 = !string.IsNullOrEmpty(oCBLevelList[1].CustomName) ? oCBLevelList[1].CustomName : oCBLevelList[1].Name;
                                ViewBag.strChurchLevel_2 = oCBModel.strChurchLevel_2;
                                ///
                                var lsCB_2 = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[1].Id).ToList();
                                var oCB_2 = lsCB_2.Where(c=> IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                if (oCB_2 .Count() != 0)
                                { oCBModel.ChurchBodyId_2 = oCB_2[0].Id; }
                                ViewBag.ChurchBodyId_2 = oCBModel.ChurchBodyId_2; 

                                /// 
                                //oCBModel.lkp_ChurchBodies_2 = _context.MSTRChurchBody.Include(t=>t.ChurchLevel)
                                //                    .Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.Status == "A" && c.ChurchLevel.LevelIndex == 2 && //c.ChurchLevelId == oCBLevelList[0].Id &&
                                //                    (c.OrganisationType == "CH" || c.OrganisationType == "CN"))
                                //              .Select(c => new SelectListItem()
                                //              {
                                //                  Value = c.Id.ToString(),
                                //                  Text = c.Name
                                //              })
                                //              .OrderBy(c => c.Text)
                                //              .ToList();
                                // oCBModel.lkp_ChurchBodies_1.Insert(0, new SelectListItem { Value = "", Text = "Select" });   

                                if (oCBModel.oCBLevelCount > 2)
                                {
                                    oCBModel.strChurchLevel_3 = !string.IsNullOrEmpty(oCBLevelList[2].CustomName) ? oCBLevelList[2].CustomName : oCBLevelList[2].Name;
                                    ViewBag.strChurchLevel_3 = oCBModel.strChurchLevel_3;

                                    var lsCB_3 = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[2].Id).ToList();
                                    var oCB_3 = lsCB_3.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                    if (oCB_3.Count() != 0)
                                    { oCBModel.ChurchBodyId_3 = oCB_3[0].Id; }
                                    ViewBag.ChurchBodyId_3 = oCBModel.ChurchBodyId_3;
                                     

                                    if (oCBModel.oCBLevelCount > 3)
                                    {
                                        oCBModel.strChurchLevel_4 = !string.IsNullOrEmpty(oCBLevelList[3].CustomName) ? oCBLevelList[3].CustomName : oCBLevelList[3].Name;
                                        ViewBag.strChurchLevel_4 = oCBModel.strChurchLevel_4;

                                        var lsCB_4 = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[3].Id).ToList();
                                        var oCB_4 = lsCB_4.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                        if (oCB_4.Count() != 0)
                                        { oCBModel.ChurchBodyId_4 = oCB_4[0].Id; }
                                        ViewBag.ChurchBodyId_4 = oCBModel.ChurchBodyId_4;


                                        if (oCBModel.oCBLevelCount > 4)
                                        {
                                            oCBModel.strChurchLevel_5 = !string.IsNullOrEmpty(oCBLevelList[4].CustomName) ? oCBLevelList[4].CustomName : oCBLevelList[4].Name;
                                            ViewBag.strChurchLevel_5 = oCBModel.strChurchLevel_4;

                                            var lsCB_5 = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oCBModel.oChurchBody.AppGlobalOwnerId && c.ChurchLevelId == oCBLevelList[4].Id).ToList();
                                            var oCB_5 = lsCB_5.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oCBModel.oChurchBody.RootChurchCode, c.Id, oCBModel.oChurchBody.ParentChurchBodyId)).ToList();
                                            if (oCB_5.Count() != 0)
                                            { oCBModel.ChurchBodyId_5 = oCB_5[0].Id; }
                                            ViewBag.ChurchBodyId_5 = oCBModel.ChurchBodyId_5;
                                        }
                                    }
                                }
                            }
                        } 
                         


                        //
                        // oCBModel.oAppGloOwnId = oAppGloOwnId;
                        // oCBModel.oChurchBodyId = oCurrChuBodyId;
                        //  var oCurrChuBody = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
                        // oCBModel.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null;

                       oCBModel = this.popLookups_CB(oCBModel, oCBModel.oChurchBody);

                        var tm = DateTime.Now;
                        _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oUserId_Logged, tm, tm, oUserId_Logged, oUserId_Logged));


                        var _oCBModel = Newtonsoft.Json.JsonConvert.SerializeObject(oCBModel);
                        TempData["oVmCurrMod"] = _oCBModel; TempData.Keep();

                        return PartialView("_AddOrEdit_CB", oCBModel);

                    }

                    //page not found error
                    Response.StatusCode = 500;
                    return PartialView("ErrorPage");

                }

                catch (Exception ex)
                {
                    //page not found error
                    Response.StatusCode = 500;
                    return PartialView("ErrorPage");
                } 
            }
        }
        public MSTRChurchBodyModel popLookups_CB(MSTRChurchBodyModel vm, MSTRChurchBody oCurrChurchBody )
        {
            if (vm == null || oCurrChurchBody == null) return vm;
            //
            vm.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            vm.lkpOrgTypes = new List<SelectListItem>();
            foreach (var dl in dlCBDivOrgTypes) { vm.lkpOrgTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            //vm.lkpAppGlobalOwns = _context.MSTRAppGlobalOwner.Where(c => c.Status == "A")
            //                                   .OrderBy(c => c.OwnerName).ToList()
            //                                   .Select(c => new SelectListItem()
            //                                   {
            //                                       Value = c.Id.ToString(),
            //                                       Text = c.OwnerName
            //                                   })
            //                                   .ToList();

            //vm.lkpAppGlobalOwns.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            vm.lkpChurchLevels = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oCurrChurchBody.AppGlobalOwnerId)
                                              .OrderByDescending(c => c.LevelIndex)
                                              .Select(c => new SelectListItem()
                                              {
                                                  Value = c.Id.ToString(),
                                                  Text = !string.IsNullOrEmpty(c.CustomName) ? c.CustomName : c.Name
                                              }) 
                                              .ToList();
            //vm.lkpChurchLevels.Insert(0, new SelectListItem { Value = "", Text = "Select" });


            //vm.lkpChurchCategories = _context.MSTRChurchBody.Include(t => t.ChurchLevel)
            //                .Where(c => c.Id != oCurrChurchBody.Id && //c.Id != oCurrChurchBody.ParentChurchBodyId &&
            //                                 c.AppGlobalOwnerId == oCurrChurchBody.AppGlobalOwnerId) // &&  c.ChurchType=="CH"  )
            //                                                                  //  (c.ChurchLevel.LevelIndex == oCurrChurchBody.ChurchLevel.LevelIndex + 1 || c.ChurchLevel.LevelIndex == oCurrChurchBody.ChurchLevel.LevelIndex - 1))
            //                                      .OrderBy(c => c.ChurchLevel.LevelIndex).ThenBy(c => c.Name).ToList()
            //                                      .Select(c => new SelectListItem()
            //                                      {
            //                                          Value = c.Id.ToString(),
            //                                          Text = c.Name
            //                                      })
            //                                      .ToList();

            //vm.lkpChurchCategories.Insert(0, new SelectListItem { Value = "", Text = "Select" });


            vm.lkpCountries = _context.MSTRCountry.ToList()  //.Where(c => c.Display == true)
                                          .Select(c => new SelectListItem()
                                          {
                                              Value = c.CtryAlpha3Code, // .ToString(),
                                              Text = c.EngName
                                          })
                                          .OrderBy(c => c.Text)
                                          .ToList();
           // vm.lkpCountries.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            //vm.lkpCountryRegions = _context.MSTRCountryRegion.Include(t => t.Country).ToList()  //.Where(c => c.Country.Display == true)
            //                                   .Select(c => new SelectListItem()
            //                                   {
            //                                       Value = c.Id.ToString(),
            //                                       Text = c.Name
            //                                   })
            //                                   .OrderBy(c => c.Text)
            //                                   .ToList();
            //vm.lkpCountryRegions.Insert(0, new SelectListItem { Value = null, Text = "Select" });
             

            return vm;
        }
        public JsonResult GetCountryByParentChurchBody(int? oParentCBId, int? oAppGloOwnId)
        {
            var cb = _context.MSTRChurchBody  // .Include(t => t.FaithTypeClass)
                .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oParentCBId).FirstOrDefault();

            var res = cb != null;
            var _strResId = cb != null ? cb.Id : (int?)null;
            var _strRes = cb != null ? cb.Name : "";
            return Json(new { taskSuccess = res, strResId = _strResId, strRes = _strRes });

            //if (addEmpty) countryList.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            //return Json(countryList);
        }
        public JsonResult GetInitChurchBodyListByAppGloOwn(int? oAppGloOwnId,  bool addEmpty = false)
        {
            var oCBList = new List<SelectListItem>();
            ///
            oCBList = _context.MSTRChurchBody.Include(t => t.ChurchLevel)
                       .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchLevel.LevelIndex == 2 && // c.Status == "A" && 
                       c.OrganisationType == "CH")
                   .OrderBy(c => c.Name)
                   .ToList()
                   .Select(c => new SelectListItem()
                   {
                       Value = c.Id.ToString(),
                       Text = c.Name
                   })
                   .OrderBy(c => c.Text)
                   .ToList();
            ///
            if (addEmpty) oCBList.Insert(0, new SelectListItem { Value = "", Text = "Select..." });
            return Json(oCBList);
        } 
        public JsonResult GetChurchLevelIndexByChurchLevel(int? oChurchLevelId, int? oAppGloOwnId, bool addEmpty = false)
        {
            var oCBList = new List<SelectListItem>();
            ///
            var oCL = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oChurchLevelId).FirstOrDefault();
            var res = oCL != null;
            var _numResLev = oCL != null ? oCL.LevelIndex : (int?)null;
            // !string.IsNullOrEmpty(oCBLevelList[0].CustomName) ? oCBLevelList[0].CustomName : oCBLevelList[0].Name
            var _strRes = oCL != null ? (!string.IsNullOrEmpty(oCL.CustomName) ? oCL.CustomName : oCL.Name) : "";
            ///
            return Json(new { taskSuccess = res, numResLev = _numResLev, strRes = _strRes }); 
        }
        public JsonResult GetChurchLevelIndexesByChurchLevel(int? oChurchLevelId, int? oAppGloOwnId, bool addEmpty = false)
        {
            var oCBList = new List<SelectListItem>();
            ///
            var oCL = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oChurchLevelId).FirstOrDefault();
            var res = oCL != null;
            var _numResLev = oCL != null ? oCL.LevelIndex : (int?)null;
            /// 

            if (oCL != null)
            {
                var oCLs = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.LevelIndex <= oCL.LevelIndex).OrderBy(c=>c.LevelIndex).ToList();
                var _strRes = "";
                foreach (var oChuLev in  oCLs)
                {
                    var strRes = oChuLev != null ? (!string.IsNullOrEmpty(oChuLev.CustomName) ? oChuLev.CustomName : oChuLev.Name) : "";
                    _strRes += strRes + ",";
                }

                _strRes = _strRes.Contains(",") ? _strRes.Remove(_strRes.LastIndexOf(",")) : _strRes;

                //  get the first CB
                var oCB_1 = _context.MSTRChurchBody.Include(t => t.ChurchLevel)
                                 .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && // c.Status == "A" && 
                                       c.ChurchLevel.LevelIndex == 1 && c.OrganisationType == "CR") //c.ChurchLevelId == oCBLevelList[0].Id &&
                                 .FirstOrDefault();

                var _numChurchBodyId_1 = (int?)null; var _strChurchBody_1 = "";
                if (oCB_1 != null)
                {  _numChurchBodyId_1 = oCB_1.Id;  _strChurchBody_1 = oCB_1.Name + " [Church Root]"; } 

                ///
                return Json(new { taskSuccess = res, numResLev = _numResLev, strResList = _strRes, numChurchBodyId_1 = _numChurchBodyId_1, strChurchBody_1 = _strChurchBody_1 });
            }


              return Json(new { taskSuccess = res, numResLev = _numResLev, strResList = "" }); 
        }
        public JsonResult GetChurchBodyListByParentBody(int? oParentCBId, int? oAppGloOwnId, string strOrgType = null, bool addEmpty = false)
        {
            var oCBList = _context.MSTRChurchBody  //.Include(t => t.ChurchLevel)
                .Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.ParentChurchBodyId == oParentCBId && // c.Status == "A" && 
                        (c.OrganisationType == "CH" || c.OrganisationType == "CN"))
                       // (c.OrganisationType == strOrgType || (strOrgType == null && (c.OrganisationType == "CH" || c.OrganisationType == "CN"))))
                .OrderBy(c => c.Name)
                .ToList()
            .Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .OrderBy(c => c.Text)
            .ToList(); 

            if (addEmpty) oCBList.Insert(0, new SelectListItem { Value = "", Text = "Select..." });
            return Json(oCBList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_CB(MSTRChurchBodyModel vm)
        {
            var strDesc = "Church unit";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = vm.PageIndex });
            if (vm.oChurchBody == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = vm.PageIndex });

            MSTRChurchBody _oChanges = vm.oChurchBody;

            
            /// server validations
            ///   
            if (string.IsNullOrEmpty(_oChanges.OrganisationType))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church unit organisation type [Congregation or Congregation Head-unit] is not specified", pageIndex = vm.PageIndex });
            strDesc = _oChanges.OrganisationType == "CH" ? "Congregation Head-unit" : _oChanges.OrganisationType == "CN" ? "Congregation" : "Church unit";

            if (_oChanges.AppGlobalOwnerId == null)
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify the denomination (church).", pageIndex = vm.PageIndex });

            var oAGO = _context.MSTRAppGlobalOwner.Find(_oChanges.AppGlobalOwnerId);
            if (oAGO == null)  // let's know the denomination... for prefic code
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Denomination (church) for " + strDesc.ToLower() + " could not be found. Please refresh and try again", pageIndex = vm.PageIndex });

            // check...
            if (string.IsNullOrEmpty(oAGO.PrefixKey))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church prefix code has not been specified. Hint: configure via Denominations" });

            if (string.IsNullOrEmpty(_oChanges.Name))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the " + strDesc.ToLower() + " name", pageIndex = vm.PageIndex }); 

            if (_oChanges.ChurchLevelId == null)  
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify the church level.", pageIndex = vm.PageIndex });

            var oCBLevel = _context.MSTRChurchLevel.Find(_oChanges.ChurchLevelId);
            if (oCBLevel == null)  // ... parent church level > church unit level
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church unit level could not be found. Please refresh and try again", pageIndex = vm.PageIndex });

            /// get the parent id
            /// 
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
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church structure is networked. Provide the parent " + parDesc.ToLower(), pageIndex = vm.PageIndex });

            var oCBParent = _context.MSTRChurchBody.Include(t=>t.ChurchLevel).Where(c=>c.Id == _oChanges.ParentChurchBodyId).FirstOrDefault();
            if (oCBParent == null)  // let's know the parent church unit... parent church level > church unit level
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Parent church unit could not be found. Please refresh and try again", pageIndex = vm.PageIndex });

            if (oCBLevel.LevelIndex <= oCBParent.ChurchLevel.LevelIndex)  // ... parent church level > church unit level
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church unit level cannot be higher or same as parent church unit. Please select the correct parent unit or change church unit level", pageIndex = vm.PageIndex });

            if (_oChanges.CtryAlpha3Code == null)   // auto-fill the country and regions using the parent details...
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the base country.", pageIndex = vm.PageIndex }); 
              

            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<MSTRChurchBodyModel>(arrData) : vm;
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
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", pageIndex = vm.PageIndex });                 
                  

                //church code  
                if (string.IsNullOrEmpty(_oChanges.GlobalChurchCode) && !string.IsNullOrEmpty(oAGO.PrefixKey))
                { 
                    //var template = new { taskSuccess = String.Empty, strRes = String.Empty };   // var definition = new { Name = "" };
                    var jsCode = GetNextGlobalChurchCodeByAcronym_jsonString(oAGO.PrefixKey, _oChanges.AppGlobalOwnerId);  // string json1 = @"{'Name':'James'}";
                    _oChanges.GlobalChurchCode = jsCode;
                }
                 

                //root church code  
                if (string.IsNullOrEmpty(_oChanges.RootChurchCode) && !string.IsNullOrEmpty(_oChanges.GlobalChurchCode))
                { 
                   // var template = new { taskSuccess = String.Empty, strRes = String.Empty };
                    var jsCode = GetNextRootChurchCodeByParentCB_jsonString(oAGO.PrefixKey, _oChanges.AppGlobalOwnerId, _oChanges.ParentChurchBodyId, _oChanges.GlobalChurchCode);
                    _oChanges.RootChurchCode = jsCode;
                }

                // check...
                if ( string.IsNullOrEmpty(_oChanges.GlobalChurchCode) || string.IsNullOrEmpty(_oChanges.RootChurchCode))  
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church code and Root church code for " + strDesc.ToLower() + " must be specified", pageIndex = vm.PageIndex }); 
                                  

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
                using (var _cbCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                { 
                    if (_oChanges.Id == 0)
                    {
                        var oCBVal = _context.MSTRChurchBody  //.Include(t=>t.ParentChurchBody)
                            .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ParentChurchBodyId == oCB.ParentChurchBodyId && c.Name == oCB.Name).FirstOrDefault();
                        if (oCBVal != null) return Json(new  {  taskSuccess = false, oCurrId = oCB.Id,  userMess = strDesc + ", " +  oCBVal.Name +  " already exists.", pageIndex = vm.PageIndex });

                        oCBVal = _context.MSTRChurchBody  //.Include(t => t.ParentChurchBody)
                            .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId &&
                                    (c.GlobalChurchCode == oCB.GlobalChurchCode //||  c.RootChurchCode == oCB.RootChurchCode || 
                                                                                // (oCB.ChurchCodeCustom != null && c.ChurchCodeCustom == oCB.ChurchCodeCustom)
                                   )).FirstOrDefault();

                        if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id,  userMess = "Church codes must be unique." + Environment.NewLine +  oCBVal.Name +  " has same church code.", pageIndex = vm.PageIndex });
                      
                        _oChanges.Created = tm;
                        _oChanges.CreatedByUserId = vm.oUserId_Logged;
                        _cbCtx.Add(_oChanges);

                        ViewBag.UserMsg = "Saved " + strDesc.ToLower() + (!string.IsNullOrEmpty(_oChanges.Name) ? ", " + _oChanges.Name : "") + " successfully.";
                        _userTask = "Added new " + strDesc.ToLower() + (!string.IsNullOrEmpty(_oChanges.Name) ? ", " + _oChanges.Name : "") + " successfully";
                    }

                    else
                    {
                        var oCBVal = _context.MSTRChurchBody  //.Include(t => t.ParentChurchBody)
                            .Where(c => c.Id != oCB.Id && c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ParentChurchBodyId == oCB.ParentChurchBodyId && c.Name == oCB.Name).FirstOrDefault();
                        if (oCBVal != null) return Json(new { taskSuccess = false,  oCurrId = oCB.Id, userMess = strDesc + ", " + oCBVal.Name +  " already exists.", pageIndex = vm.PageIndex });

                        // oCBVal = _context.MSTRChurchBody.Include(t => t.ParentChurchBody).Where(c => c.Id != oCB.Id && c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ChurchCode == oCB.ChurchCode ).FirstOrDefault();
                        //if (oCBVal != null) return Json(new { taskSuccess = false, oCurrId = oCB.Id, userMess = "Church code must be unique." + Environment.NewLine + 
                        //        oCBVal.Name + (oCBVal.ParentChurchBody != null ? " of " + oCBVal.ParentChurchBody.Name : "") + " has  same code."});
                    
                    
                        oCBVal = _context.MSTRChurchBody   //.Include(t => t.ParentChurchBody)
                            .Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.Id != oCB.Id &&
                                   (c.GlobalChurchCode == oCB.GlobalChurchCode //|| c.RootChurchCode == oCB.RootChurchCode ||
                                                                               // (oCB.ChurchCodeCustom != null && c.ChurchCodeCustom == oCB.ChurchCodeCustom)
                                   )).FirstOrDefault();

                        if (oCBVal != null) return Json(new {  taskSuccess = false,  oCurrId = oCB.Id,  userMess = "Church codes must be unique." + Environment.NewLine + oCBVal.Name +   " has same church code.", pageIndex = vm.PageIndex });
                    

                        //retain the pwd details... hidden fields


                        _cbCtx.Update(_oChanges);
                        //var _strDesc = strDesc.Length > 0 ? strDesc.Substring(0, 1).ToUpper() + strDesc.Substring(1) : "Denomination ";

                        ViewBag.UserMsg = strDesc + (!string.IsNullOrEmpty(_oChanges.Name) ? ", " + _oChanges.Name : "") + " updated successfully.";
                        _userTask = "Updated " + strDesc.ToLower() + (!string.IsNullOrEmpty(_oChanges.Name) ? ", " + _oChanges.Name : "") + " successfully";
                    }

                    //save denomination first... 
                    await _cbCtx.SaveChangesAsync();

                     
                    DetachAllEntities(_cbCtx);
                }

                 

                var _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oCurrUserId_Logged, _tm, _tm, vm.oCurrUserId_Logged, vm.oCurrUserId_Logged));
                                 

                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg, pageIndex = vm.PageIndex });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving church unit details. Err: " + ex.Message, pageIndex = vm.PageIndex });
            }
        }

        public IActionResult Delete_CB(int? loggedUserId, int id, bool forceDeleteConfirm = false)  // attach oAppGloOwnId ... (int? loggedUserId, int id, int setIndex, int subSetIndex, bool forceDeleteConfirm = false)
        {
            var strDesc = "Church unit"; var _tm = DateTime.Now; var _userTask = "Attempted saving church unit";
            //
            try
            {
                var tm = DateTime.Now;
                //
                var oCB = _context.MSTRChurchBody.Where(c => c.Id == id).FirstOrDefault(); // .Include(c => c.ChurchUnits)
                strDesc = oCB.OrganisationType == "CH" ? "Congregation Head-unit" : oCB.OrganisationType == "CN" ? "Congregation" : "Church Unit";
                if (oCB == null)
                {
                    _userTask = "Attempted deleting " + strDesc.ToLower() + ", " + oCB.Name;  // var _userTask = "Attempted saving church unit";
                    _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " to delete could not be retrieved." });
                }

                var saveDelete = true;
                // ensuring cascade delete where there's none!

                //check for the UP and CSC, parent CB ... and many more! almost every table is connectedto CB
                var oUserProfiles = _context.UserProfile.Where(c => c.ChurchBodyId == oCB.Id).ToList();
                // var oClientServerConfigs = _context.ClientAppServerConfig.Where(c => c.ChurchBodyId == oCB.Id).ToList();
                var oParentCBs = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oCB.AppGlobalOwnerId && c.ParentChurchBodyId == oCB.Id).ToList();

                using (var _cbCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                { 
                    if ((oUserProfiles.Count() + oParentCBs.Count()) > 0)
                    {
                        var strConnTabs = oUserProfiles.Count() > 0 ? "User profile" : "";
                        strConnTabs = strConnTabs.Length > 0 ? strConnTabs + ", " : strConnTabs;
                       // strConnTabs = oClientServerConfigs.Count() > 0 ? strConnTabs + "Client server configuration" : strConnTabs;
                        strConnTabs = oParentCBs.Count() > 0 ? strConnTabs + "Church unit (parented other church units) " : strConnTabs;

                        if (forceDeleteConfirm == false)
                        {
                            saveDelete = false;
                            // check user privileges to determine... administrator rights
                            // log
                            _userTask = "Attempted deleting " + strDesc.ToLower() + ", " + oCB.Name;  // var _userTask = "Attempted saving church unit";
                            _tm = DateTime.Now;
                            _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                             "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                            return Json(new
                            {
                                taskSuccess = false,
                                tryForceDelete = false,
                                oCurrId = id,
                                userMess = "Specified " + strDesc.ToLower() + " to delete has been used elsewhere in the system [" + strConnTabs + "]. Delete cannot be done unless dependent-references are removed."
                            });

                            //super_admin task
                            //return Json(new { taskSuccess = false, tryForceDelete = true, oCurrId = id, userMess = "Specified " + strDesc.ToLower() + 
                            //       " has dependencies or links with other external data [Faith category]. Delete cannot be done unless child refeneces are removed. DELETE (cascade) anyway?" });
                        } 

                        ///Else....
                    }

                    //successful...
                    if (saveDelete)
                    {
                        _cbCtx.MSTRChurchBody.Remove(oCB);
                        _cbCtx.SaveChanges();

                         
                        DetachAllEntities(_cbCtx);

                        _userTask = "Deleted " + strDesc.ToLower() + ", " + oCB.Name + " successfully";  // var _userTask = "Attempted saving " + strDesc;
                        _tm = DateTime.Now;
                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                        return Json(new { taskSuccess = true, tryForceDelete = false, oCurrId = oCB.Id, userMess = strDesc + " successfully deleted." });
                    }

                    else
                    {   DetachAllEntities(_cbCtx); return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "No " + strDesc.ToLower() + " data available to delete. Try again" }); }
                     
                }
            }

            catch (Exception ex)
            {
                _userTask = "Attempted deleting " + strDesc.ToLower() + ", [ID=" + id + "] but FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Failed deleting " + strDesc.ToLower() + ". Err: " + ex.Message });
            }
        }


        // CSC
        public ActionResult Index_CASC(int? oAppGloOwnId = null, int pageIndex = 1)  // , int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0) //, int? oParentId = null, int? id = null, int pageIndex = 1)             
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                var strDesc = "Client server configuration";
                var _userTask = "Viewed " + strDesc.ToLower() + " list";

                // check permission 
                var _oUserPrivilegeCol = oUserLogIn_Priv;
                var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();
                //
                if (!this.userAuthorized) return View(new ClientAppServerConfigModel()); //retain view    
                if (oUserLogIn_Priv[0] == null) return View(new ClientAppServerConfigModel());
                if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner != null || oUserLogIn_Priv[0].ChurchBody != null) return View(new ClientAppServerConfigModel());
                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                var oLoggedRole = oUserLogIn_Priv[0].UserRole;

                //
                var oCASCModel = new ClientAppServerConfigModel(); //TempData.Keep();  
                                                           // int? oAppGloOwnId = null;
                var oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody;
                //
                int? oAppGloOwnId_Logged = null;
                int? oChuBodyId_Logged = null;
                if (oChuBody_Logged != null)
                {
                    oAppGloOwnId_Logged = oChuBody_Logged.ChurchLevelId;
                    oChuBodyId_Logged = oChuBody_Logged.Id;
                }

                var oUserId_Logged = oLoggedUser.Id;

                //
                var lsCLModel = (
                        from t_casc in _context.ClientAppServerConfig.AsNoTracking() //.Where(c => c.AppGlobalOwnerId == oAppGloOwnId) //.Include(t => t.AppGlobalOwner)
                        from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Where(c => c.Id == t_casc.AppGlobalOwnerId)  

                        select new ClientAppServerConfigModel()
                        {
                            oCASConfig = t_casc,
                            strAppGloOwn = t_ago != null ? t_ago.OwnerName : "", 
                            strConfigDate = t_casc.ConfigDate != null ? DateTime.Parse(t_casc.ConfigDate.ToString()).ToString("d-MMM-yyyy", CultureInfo.InvariantCulture) : "",
                        }
                       ).OrderByDescending(c => c.oCASConfig.ConfigDate).ThenBy(c => c.strAppGloOwn) 
                       .ToList();

                oCASCModel.lsCASConfigModels = lsCLModel;
                oCASCModel.strCurrTask = strDesc;

                //                
                oCASCModel.oAppGloOwnId = oAppGloOwnId;
                //oCASCModel.oChurchBodyId = oCurrChuBodyId;
                oCASCModel.PageIndex = pageIndex;
                //
                oCASCModel.oUserId_Logged = oUserId_Logged;
                oCASCModel.oChurchBody_Logged = oChuBody_Logged;
                oCASCModel.oAppGloOwnId_Logged = oAppGloOwnId_Logged;



                if (oCASCModel.PageIndex == 2) // Client server configuration classes av church sects
                    oCASCModel = this.popLookups_CASC(oCASCModel, oCASCModel.oCASConfig);



                //  dashboard stuff
                ///
                ViewData["strAppName"] = "RhemaCMS";
                ViewData["strAppNameMod"] = "Admin Palette";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(oLoggedUser.UserDesc) ? oLoggedUser.UserDesc : "[Current user]";
                ///
                ViewData["oAppGloOwnId_Logged"] = oAppGloOwnId_Logged;
                ViewData["oChuBodyId_Logged"] = oChuBodyId_Logged;
                ViewData["strAppCurrUser_ChRole"] = oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUser_PhotoFilename"] = oLoggedUser.UserPhoto;
                ///


                //refresh Dash values
                _ = LoadDashboardValues();

                var tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id));

                return View("Index_CASC", oCASCModel);
            }
        }


        [HttpGet]
        public IActionResult AddOrEdit_CASC(int id = 0, int? oAppGloOwnId = null, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) // (int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0, int? oCASCId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                if (StackAppUtilties.IsAjaxRequest(HttpContext.Request))
                {
                    var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                    var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                    // int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                    //int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                    // int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                    oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                    oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                    oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                    var strDesc = "Client server configuration";
                    var _userTask = "Attempted accessing/modifying " + strDesc.ToLower(); // _userTask = "Attempted creating new denomination (church)"; // _userTask = "Opened denomination (church)-" + oCFT_MDL.oChurchFaithType.FaithDescription;
                    var oCASCModel = new ClientAppServerConfigModel();
                    if (id == 0)
                    { 
                        //var oAppOwn = _context.MSTRAppGlobalOwner.Find(oAppGloOwnId);
                        //if (oAppOwn == null)
                        //{
                        //    Response.StatusCode = 500;
                        //    return PartialView("ErrorPage");
                        //}

                        oCASCModel.oCASConfig = new ClientAppServerConfig();
                          
                       // oCASCModel.oCASConfig.AppGlobalOwnerId = (int)oAppGloOwnId;

                        oCASCModel.oCASConfig.ConfigDate = DateTime.Now;
                        oCASCModel.oCASConfig.DbaseName = "DBRCMS_CL_"; // + oAppOwn.Acronym.ToUpper(); //check uniqueness
                                               
                        oCASCModel.oCASConfig.Status = "A";

                        oCASCModel.oCASConfig.Created = DateTime.Now;
                        oCASCModel.oCASConfig.LastMod = DateTime.Now;
                        //
                        //oCASCModel.oCASConfig.AppGlobalOwner = oAppOwn;
                        //oCASCModel.strAppGloOwn = oAppOwn.OwnerName;

                        _userTask = "Attempted creating new " + strDesc.ToLower(); //, " + oAppOwn.OwnerName;
                    }

                    else
                    {
                        oCASCModel = (
                             from t_casc in _context.ClientAppServerConfig.AsNoTracking().Include(t=>t.AppGlobalOwner).Where(x => x.Id == id)
                             from t_ago in _context.MSTRAppGlobalOwner.AsNoTracking().Where(c => c.Id == t_casc.AppGlobalOwnerId)

                             select new ClientAppServerConfigModel()
                             {
                                 oCASConfig = t_casc,
                                 strAppGloOwn = t_ago != null ? t_ago.OwnerName : "",
                                 strConfigDate = t_casc.ConfigDate != null ? DateTime.Parse(t_casc.ConfigDate.ToString()).ToString("d-MMM-yyyy", CultureInfo.InvariantCulture) : "",
                             }
                            )
                             .FirstOrDefault();

                        if (oCASCModel == null)
                        {
                            Response.StatusCode = 500;
                            return PartialView("ErrorPage");
                        }

                        _userTask = "Opened " + strDesc.ToLower() + " -- [#Id: " + oCASCModel.oCASConfig.Id + "] for denomination, " + oCASCModel.strAppGloOwn;
                    }


                    // oCASCModel.setIndex = setIndex;
                    // oCASCModel.subSetIndex = subSetIndex;

                    oCASCModel.PageIndex = 2;

                    oCASCModel.oUserId_Logged = oUserId_Logged;
                    oCASCModel.oAppGloOwnId_Logged = oAGOId_Logged;
                    oCASCModel.oChurchBodyId_Logged = oCBId_Logged;
                    //
                    // oCASCModel.oAppGloOwnId = oAppGloOwnId;
                    // oCASCModel.oChurchBodyId = oCurrChuBodyId;
                    //  var oCurrChuBody = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
                    // oCASCModel.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null;

                  //  if (oCASCModel.subSetIndex == 2) // Client server configuration classes av church sects
                   
                    oCASCModel = this.popLookups_CASC(oCASCModel, oCASCModel.oCASConfig);

                    var tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oUserId_Logged, tm, tm, oUserId_Logged, oUserId_Logged));

                    var _oCASCModel = Newtonsoft.Json.JsonConvert.SerializeObject(oCASCModel);
                    TempData["oVmCurrMod"] = _oCASCModel; TempData.Keep();

                    return PartialView("_AddOrEdit_CASC", oCASCModel);
                }

                //page not found error
                Response.StatusCode = 500;
                return PartialView("ErrorPage");
            }
        }

        public ClientAppServerConfigModel popLookups_CASC(ClientAppServerConfigModel vm, ClientAppServerConfig oCurrCL)
        {
            if (vm == null ) return vm;   // || oCurrCL == null
            //
            vm.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vm.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            vm.lkpAppGlobalOwns = _context.MSTRAppGlobalOwner.Where(c => c.Status == "A")
                                              .OrderBy(c => c.OwnerName).ToList()
                                              .Select(c => new SelectListItem()
                                              {
                                                  Value = c.Id.ToString(),
                                                  Text = c.OwnerName
                                              })
                                              .ToList();

            vm.lkpAppGlobalOwns.Insert(0, new SelectListItem { Value = "", Text = "Select" });


            return vm;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_CASC(ClientAppServerConfigModel vm)
        {
            var strDesc = "Client server configuration";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = vm.PageIndex });
            if (vm.oCASConfig == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = vm.PageIndex });

            ClientAppServerConfig _oChanges = vm.oCASConfig;  // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();
             
             
            if (_oChanges.AppGlobalOwnerId == null)
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify denomination (church) to configure", pageIndex = vm.PageIndex });

            if (string.IsNullOrEmpty(_oChanges.ServerName))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the database server", pageIndex = vm.PageIndex });

            if (string.IsNullOrEmpty(_oChanges.DbaseName))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the database name", pageIndex = vm.PageIndex });

           // oCASCModel.oCASConfig.DbaseName = "DBRCMS_CL_" + oAppOwn.Acronym.ToUpper(); //check uniqueness
            if (_oChanges.DbaseName.StartsWith("DBRCMS_CL_") == false)
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name must begin with 'DBRCMS_CL_'", pageIndex = vm.PageIndex });

            // database must not have been assigned...
            var oCASCExist = _context.ClientAppServerConfig.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.DbaseName.ToLower() == _oChanges.DbaseName.ToLower()).FirstOrDefault();
            if (oCASCExist != null)
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name specified is not available. It's used by another denomination/church.", pageIndex = vm.PageIndex });

            if (string.IsNullOrEmpty(_oChanges.SvrUserId))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the server user", pageIndex = vm.PageIndex });

            if (string.IsNullOrEmpty(_oChanges.SvrPassword))
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the server password", pageIndex = vm.PageIndex });

            // check connection successful...  ??? 
            // get and mod the conn
            var _clientDBConnString = "";
            var conn = new SqlConnectionStringBuilder(_context.Database.GetDbConnection().ConnectionString);
            conn.DataSource = _oChanges.ServerName; conn.InitialCatalog = _oChanges.DbaseName; conn.UserID = _oChanges.SvrUserId; conn.Password = _oChanges.SvrPassword; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
            _clientDBConnString = conn.ConnectionString;

            // test the NEW DB conn
            var _clientContext = new ChurchModelContext(_clientDBConnString);
            if (!_clientContext.Database.CanConnect()) 
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to connect client database. Please try again or contact System Admin", pageIndex = vm.PageIndex }); 
             

            // validations done!
            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ClientAppServerConfigModel>(arrData) : vm;

            var oCASC = vmMod.oCASConfig;
            // oCASC.ChurchBody = vmMod.oChurchBody; 

            try
            {
                ModelState.Remove("oCASConfig.AppGlobalOwnerId"); 
                //
                ModelState.Remove("oCASConfig.CreatedByUserId");
                ModelState.Remove("oCASConfig.LastModByUserId");


                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", pageIndex = vm.PageIndex });


                //
                var tm = DateTime.Now;
                _oChanges.LastMod = tm;
                _oChanges.LastModByUserId = vm.oUserId_Logged;

                var _reset = _oChanges.Id == 0;
                var oAGO = _context.MSTRAppGlobalOwner.Find(_oChanges.AppGlobalOwnerId);
                var oCASCDesc = strDesc.ToLower(); // + " -- [#Id: " + _oChanges.Id + "] for denomination" + (oAGO != null ? ", " + oAGO.OwnerName : "");

                //validate...
                var _userTask = "Attempted saving " + oCASCDesc;  // _userTask = "Added new " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";   //  _userTask = "Updated " + strDesc.ToLower() + ", " + _oChanges.OwnerName.ToUpper() + " successfully";

                using (var _cascCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                { 
                    if (_oChanges.Id == 0)
                    {
                        var existCASC = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.Status == "A").FirstOrDefault();
                        if (existCASC != null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Client server configuration already exist" + (existCASC.AppGlobalOwner != null ? ". Denomination (Church): " + existCASC.AppGlobalOwner.OwnerName : ""), pageIndex = vm.PageIndex });


                        var oCASC_Db = _context.ClientAppServerConfig.Include(t=>t.AppGlobalOwner).Where(c => c.DbaseName.ToLower().Equals(_oChanges.DbaseName.ToLower())).FirstOrDefault();
                        if (oCASC_Db != null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess =  "Database name already used for another denomination" + (oCASC_Db.AppGlobalOwner != null ? ", " + oCASC_Db.AppGlobalOwner.OwnerName : ""), pageIndex = vm.PageIndex });

                        _oChanges.Created = tm;
                        _oChanges.CreatedByUserId = vm.oUserId_Logged;

                        _cascCtx.Add(_oChanges);

                   
                        ViewBag.UserMsg = "Saved " + oCASCDesc + " successfully.";
                        _userTask = "Added new " + oCASCDesc + " successfully";
                    }

                    else
                    {
                        var existCASC = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.Id != _oChanges.Id && c.Status == "A").FirstOrDefault(); 
                        if (existCASC != null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Client server configuration already exist" + (existCASC.AppGlobalOwner != null ? ". Denomination (Church): " + existCASC.AppGlobalOwner.OwnerName : ""), pageIndex = vm.PageIndex });

                        var oCASC_Db = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.Id != _oChanges.Id && c.DbaseName.ToLower().Equals(_oChanges.DbaseName.ToLower())).FirstOrDefault();
                        if (oCASC_Db != null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name already used for another denomination" + (oCASC_Db.AppGlobalOwner != null ? ", " + oCASC_Db.AppGlobalOwner.OwnerName : ""), pageIndex = vm.PageIndex });
                     
                        //retain the pwd details... hidden fields
                        _cascCtx.Update(_oChanges);
                        //var _strDesc = strDesc.Length > 0 ? strDesc.Substring(0, 1).ToUpper() + strDesc.Substring(1) : "Client server configuration ";

                        oCASCDesc += " -- [#Id: " + _oChanges.Id + "] for denomination" + (oAGO != null ? ", " + oAGO.OwnerName : "");
                        ViewBag.UserMsg = oCASCDesc + " updated successfully.";
                        _userTask = "Updated " + oCASCDesc + " successfully";
                    }

                    //save denomination first... 
                    await _cascCtx.SaveChangesAsync();

                    DetachAllEntities(_cascCtx);
                }


                var _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oCurrUserId_Logged, _tm, _tm, vm.oCurrUserId_Logged, vm.oCurrUserId_Logged));


                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, resetNew = _reset, userMess = ViewBag.UserMsg, pageIndex = vm.PageIndex });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving denomination (church) details. Err: " + ex.Message, pageIndex = vm.PageIndex });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditBLK_CASC(ClientAppServerConfigModel vm, IFormCollection f) //ChurchMemAttendList oList)      
                                                                                                     // public IActionResult Index_Attendees(ChurchMemAttendList oList) //List<ChurchMember> oList)  //, int? reqChurchBodyId = null, string strAttendee="M", string strLongevity="C" ) //, char? filterIndex = null, int? filterVal = null)
        {
            var strDesc = "Client server configuration";
            if (vm == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = strDesc + " data to update unavailable. Please refresh and try again.", pageIndex = 2 });
            if (vm.lsCASConfigModels == null) return Json(new { taskSuccess = false, oCurrId = "", userMess = "No changes made to " + strDesc + " data.", pageIndex = vm.PageIndex });
            if (vm.lsCASConfigModels.Count == 0) return Json(new { taskSuccess = false, oCurrId = "", userMess = "No changes made to " + strDesc + " data.", pageIndex = vm.PageIndex });
           
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
            //    return Json(new { taskSuccess = false, oCurrId = -1, userMess = "Total " + strDesc.ToLower() + "s allowed for denomination, " + oAGO.OwnerName + " [" + oAGO.TotalLevels + "] exceeded.", pageIndex = vm.PageIndex });


            // return View(vm);
            if (ModelState.IsValid == false)
                return Json(new { taskSuccess = false, oCurrId = "", userMess = "Saving data failed. Please refresh and try again", pageIndex = vm.PageIndex  });

            //if (vm == null)
            //    return Json(new { taskSuccess = false, userMess = "Data to update not found. Please refresh and try again", pageIndex = vm.PageIndex });

            //if (vm.lsCASConfigModels == null)
            //    return Json(new { taskSuccess = false, userMess = "No changes made to attendance data.", pageIndex = vm.PageIndex });

            //if (vm.lsCASConfigModels.Count == 0)
            //    return Json(new { taskSuccess = false, userMess = "No changes made to attendance data", pageIndex = vm.PageIndex });

             

            //if ((_oChanges.Id == 0 && countCL >= oAGO.TotalLevels) || (_oChanges.Id > 0 && countCL > oAGO.TotalLevels))
            //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Total " + strDesc.ToLower() + "s allowed for denomination, " + oAGO.OwnerName + " [" + oAGO.TotalLevels + "] reached." });

            //if (_oChanges.LevelIndex <= 0 || _oChanges.LevelIndex > oAGO.TotalLevels)
            //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please indicate correct level index. Hint: Must be within total Client server configurations [" + oAGO.TotalLevels + "]" });

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

             


            foreach (var d in vm.lsCASConfigModels)
            {
                if (d.oCASConfig != null)
                {
                    var _oChanges = d.oCASConfig;
                    if (_oChanges.AppGlobalOwnerId == null)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify denomination (church) to configure" , pageIndex = vm.PageIndex });

                    var oAGO = _context.MSTRAppGlobalOwner.Find(_oChanges.AppGlobalOwnerId);
                    var strAGO = oAGO != null ? oAGO.OwnerName : "";

                    if (string.IsNullOrEmpty(_oChanges.ServerName))
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the database server" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : "") , pageIndex = vm.PageIndex });

                    if (string.IsNullOrEmpty(_oChanges.DbaseName))
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the database name" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : ""), pageIndex = vm.PageIndex });

                    // oCASCModel.oCASConfig.DbaseName = "DBRCMS_CL_" + oAppOwn.Acronym.ToUpper(); //check uniqueness
                    if (_oChanges.DbaseName.StartsWith("DBRCMS_CL_") == false)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name must begin with 'DBRCMS_CL_'" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : ""), pageIndex = vm.PageIndex });

                    if (string.IsNullOrEmpty(_oChanges.SvrUserId))
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the server user" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : ""), pageIndex = vm.PageIndex });

                    if (string.IsNullOrEmpty(_oChanges.SvrPassword))
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Provide the server password" + (!string.IsNullOrEmpty(strAGO) ? ". Denomination (Church): " + strAGO : ""), pageIndex = vm.PageIndex });


                    if (d.oCASConfig.Id > 0)  // update
                    {
                        var existCASC = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.Id != _oChanges.Id && c.Status == "A").FirstOrDefault();
                        if (existCASC != null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Client server configuration already exist" + (existCASC.AppGlobalOwner != null ? ". Denomination (Church): " + existCASC.AppGlobalOwner.OwnerName : ""), pageIndex = vm.PageIndex });


                        var oCASC_Db = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.Id != _oChanges.Id && c.DbaseName.ToLower().Equals(_oChanges.DbaseName.ToLower())).FirstOrDefault();
                        if (oCASC_Db != null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name already used for another denomination" + (oCASC_Db.AppGlobalOwner != null ? ", " + oCASC_Db.AppGlobalOwner.OwnerName : ""), pageIndex = vm.PageIndex });

                    }

                    else if (d.oCASConfig.Id == 0)  //add
                    {
                        var existCASC = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.Status == "A").FirstOrDefault();
                        if (existCASC != null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Client server configuration already exist" + (existCASC.AppGlobalOwner != null ? ". Denomination (Church): " + existCASC.AppGlobalOwner.OwnerName : ""), pageIndex = vm.PageIndex });
                         
                        var oCASC_Db = _context.ClientAppServerConfig.Include(t => t.AppGlobalOwner).Where(c => c.DbaseName.ToLower().Equals(_oChanges.DbaseName.ToLower())).FirstOrDefault();
                        if (oCASC_Db != null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Database name already used for another denomination" + (oCASC_Db.AppGlobalOwner != null ? ", " + oCASC_Db.AppGlobalOwner.OwnerName : ""), pageIndex = vm.PageIndex });
                   
                    }
                }
            }


            // all clear.....
             
            using (var _cascCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
            {
                var oCASC_CntAdd = 0; var oCASC_CntUpd = 0;
                foreach (var d in vm.lsCASConfigModels)
                {
                    if (d.oCASConfig != null)
                    {
                        if (d.oCASConfig.Id > 0)  // update
                        {
                            var oCASC = _context.ClientAppServerConfig.Where(c => c.AppGlobalOwnerId == d.oCASConfig.AppGlobalOwnerId && c.Id == d.oCASConfig.Id).FirstOrDefault();  
                            if (oCASC != null)  
                            {
                                oCASC.AppGlobalOwnerId = d.oCASConfig.AppGlobalOwnerId;  
                                oCASC.ServerName = d.oCASConfig.ServerName;  
                                oCASC.DbaseName  = d.oCASConfig.DbaseName;  
                                oCASC.SvrUserId = d.oCASConfig.SvrUserId;
                                oCASC.SvrPassword = d.oCASConfig.SvrPassword;
                                oCASC.ConfigDate = d.oCASConfig.ConfigDate;
                                oCASC.LastMod = DateTime.Now;
                                //
                                oCASC_CntUpd++;
                                _cascCtx.Update(oCASC);
                            }
                        }

                        else if (d.oCASConfig.Id == 0)  //add
                        {
                            ClientAppServerConfig oCASC = new ClientAppServerConfig()
                            {
                                AppGlobalOwnerId = d.oCASConfig.AppGlobalOwnerId,
                                ServerName = d.oCASConfig.ServerName,
                                DbaseName = d.oCASConfig.DbaseName,
                                SvrUserId = d.oCASConfig.SvrUserId,
                                SvrPassword = d.oCASConfig.SvrPassword,  
                                ConfigDate = d.oCASConfig.ConfigDate,
                                Created = DateTime.Now,
                                LastMod = DateTime.Now,
                            };

                            //
                            oCASC_CntAdd++;
                            _cascCtx.Add(oCASC);
                        }
                    }
                }


                var _userTask = "";
                if ((oCASC_CntAdd + oCASC_CntUpd) > 0)
                {
                    if (oCASC_CntAdd > 0)
                    { 
                        _userTask = "Added new " + oCASC_CntAdd + " Client server configurations for " + strDesc.ToLower() + " successfully.";
                        ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + "Created " + oCASC_CntAdd + " client server configurations." ;
                    }

                    if (oCASC_CntUpd > 0)
                    { 
                        _userTask = !string.IsNullOrEmpty(_userTask) ? _userTask + ". " : "" + "Updated " + oCASC_CntUpd + " Client server configurations for " + strDesc.ToLower() + " successfully.";
                        ViewBag.UserMsg = (!string.IsNullOrEmpty(ViewBag.UserMsg) ? ViewBag.UserMsg + ". " : "") + Environment.NewLine + Environment.NewLine + oCASC_CntUpd + " client server configurations updated.";
                    }

                    //save all...
                    await _cascCtx.SaveChangesAsync();


                    var _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: Client Server Configuration", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vm.oUserId_Logged, _tm, _tm, vm.oUserId_Logged, vm.oUserId_Logged));


                    return Json(new { taskSuccess = true, userMess = ViewBag.UserMsg, pageIndex = vm.PageIndex });
                }

            }

            return Json(new { taskSuccess = false, userMess = "Saving data failed. Please refresh and try again.", pageIndex = vm.PageIndex });
        }
         

        public IActionResult Delete_CASC(int? loggedUserId, int id, bool forceDeleteConfirm = false)  // (int? loggedUserId, int id, int setIndex, int subSetIndex, bool forceDeleteConfirm = false)
        {
            var strDesc = "Client server configuration" ; var _tm = DateTime.Now; var _userTask = "Attempted deleting " + strDesc.ToLower(); 
            //
            try
            {
                var tm = DateTime.Now;
                //
                var oCASC = _context.ClientAppServerConfig.Include(t=>t.AppGlobalOwner).Where(c => c.Id == id).FirstOrDefault(); // .Include(c => c.ChurchUnits)
                if (oCASC == null)
                {
                    _userTask = "Attempted deleting " + strDesc.ToLower() + " -- [#Id: " + id + "]";
                    _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " to delete could not be retrieved." });
                }

                var saveDelete = true;
                // ensuring cascade delete where there's none!

                //check for the AGO and CB
                // var oChurchBodies = _context.MSTRAppGlobalOwner.Where(c => c.Id == oCASC.AppGlobalOwnerId).ToList();
                // var oMTs = _CASCientContext.TransferTypeChurchLevel.Where(c => c.ChurchLevelId == oCASC.Id).ToList(); 

                using (var _cascCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                { 
                    //if (oChurchBodies.Count() > 0)
                    //{
                    //    var strConnTabs = oChurchBodies.Count() > 0 ? "Denomination (church)" : "";
                    //    //strConnTabs = strConnTabs.Length > 0 ? strConnTabs + ", " : strConnTabs;
                    //    //strConnTabs = oMTs.Count() > 0 ? strConnTabs + "Church transfer" : strConnTabs;

                    //    if (forceDeleteConfirm == false)
                    //    {
                    //        saveDelete = false;
                    //        // check user privileges to determine... administrator rights
                    //        // log 

                    //        _userTask = "Attempted deleting " + strDesc.ToLower() + " -- [#Id: " + id + "]" + (oCASC.AppGlobalOwner != null ? ". Denomination: " + oCASC.AppGlobalOwner.OwnerName : "");
                    //        _tm = DateTime.Now;
                    //        _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                    //                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                    //        return Json(new
                    //        {
                    //            taskSuccess = false,
                    //            tryForceDelete = false,
                    //            oCurrId = id,
                    //            userMess = "Specified " + strDesc.ToLower() + " to delete has been used elsewhere in the system [" + strConnTabs + "]. Delete cannot be done unless dependent-references are removed."
                    //        });

                    //        //super_admin task
                    //        //return Json(new { taskSuccess = false, tryForceDelete = true, oCurrId = id, userMess = "Specified " + strDesc.ToLower() + 
                    //        //       " has dependencies or links with other external data [Faith category]. Delete cannot be done unless child refeneces are removed. DELETE (cascade) anyway?" });
                    //    }
                    //}

                    //successful...
                    if (saveDelete)
                    {
                        _cascCtx.ClientAppServerConfig.Remove(oCASC);
                        _cascCtx.SaveChanges(); 
                        
                        DetachAllEntities(_cascCtx);

                        _userTask = "Deleted " + strDesc.ToLower() + " -- [#Id: " + oCASC.Id + "]" + (oCASC.AppGlobalOwner != null ? ". Denomination: " + oCASC.AppGlobalOwner.OwnerName : "") + " successfully";  // var _userTask = "Attempted saving " + strDesc;
                        _tm = DateTime.Now;
                        _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                         "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                        return Json(new { taskSuccess = true, tryForceDelete = false, oCurrId = oCASC.Id, userMess = strDesc + " successfully deleted." });
                    }

                    else
                    {   DetachAllEntities(_cascCtx); return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "No " + strDesc.ToLower() + " data available to delete. Try again" }); }
                   
                }

            }

            catch (Exception ex)
            {
                _userTask = "Attempted deleting " + strDesc.ToLower() + " -- [#Id: " + id + "] but FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Failed deleting " + strDesc.ToLower() + ". Err: " + ex.Message });
            }
        }


        /// <summary>
        /// CONTACT DETAILS --- for AGO, CB [cong amd cong unit levels]
        /// </summary>
        /// <returns></returns>






        ////////////////////////////
        private List<UserRole> GetUserRoles()  //(int? userRoleId = null, int? oCurrChuBodyId = null)
        {  //System roles ... oCurrChuBodyId == null
           // if (oCurrChuBodyId == null) return new List<UserPermission>();

            var userRoles = (
                        //from t_upr in _context.UserRolePermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userRoleId == null || (userRoleId != null && c.UserRoleId == userRoleId)))
                        from t_up in _context.UserRole
                        select new UserRole()
                        {
                            Id = t_up.Id,
                            ChurchBodyId = t_up.ChurchBodyId,
                            RoleType = t_up.RoleType,
                            RoleDesc = t_up.RoleDesc,
                            RoleLevel = t_up.RoleLevel,
                            RoleStatus = t_up.RoleStatus,
                            RoleName = t_up.RoleName,
                            Created = t_up.Created,
                            CreatedByUserId = t_up.CreatedByUserId,
                            LastMod = t_up.LastMod,
                            LastModByUserId = t_up.LastModByUserId

                        })
                           .OrderBy(c => c.RoleLevel).ThenBy(c => c.RoleName)
                           .ToList();

            return userRoles;
        }
        private List<UserPermission> GetUserPermissions()  //(int? userRoleId = null, int? oCurrChuBodyId = null)
        {  //System roles ... oCurrChuBodyId == null
           // if (oCurrChuBodyId == null) return new List<UserPermission>();

            var userPerms = (
                        //from t_upr in _context.UserRolePermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userRoleId == null || (userRoleId != null && c.UserRoleId == userRoleId)))
                        from t_up in _context.UserPermission
                        from t_up_c in _context.UserPermission.Where(c => c.Id == t_up.UserPermCategoryId).DefaultIfEmpty()
                        select new UserPermission()
                        {
                            Id = t_up.Id,
                            // ChurchBodyId = t_up.ChurchBodyId,
                            PermissionCode = t_up.PermissionCode,
                            PermissionName = t_up.PermissionName,
                            strPermDesc = AppUtilties.GetPermissionDesc_FromName(t_up.PermissionName),
                            PermStatus = t_up.PermStatus,
                            UserPermCategoryId = t_up.UserPermCategoryId,
                            strPermCategory = t_up_c != null ? AppUtilties.GetPermissionDesc_FromName(t_up_c.PermissionName) : "",
                            // Crud = t_up.Crud,
                            Created = t_up.Created,
                            CreatedByUserId = t_up.CreatedByUserId,
                            LastMod = t_up.LastMod,
                            LastModByUserId = t_up.LastModByUserId

                        }
                               )
                               .OrderBy(c => c.PermissionCode).ToList();

            return userPerms;
        }

        private List<UserAuditTrail> GetUserAuditTasks()  //(int? userRoleId = null, int? oCurrChuBodyId = null)
        {  //System roles ... oCurrChuBodyId == null
           // if (oCurrChuBodyId == null) return new List<UserPermission>();

            //  var userAudits = _context.UserAuditTrail.ToList().OrderByDescending(c => c.Created).ToList();

            var userAudits = (from t_uat in _context.UserAuditTrail.Include(t => t.AppGlobalOwner).Include(t => t.ChurchBody)
                              from t_up in _context.UserProfile.Where(c => c.AppGlobalOwnerId == t_uat.AppGlobalOwnerId && c.ChurchBodyId == t_uat.ChurchBodyId && c.Id == t_uat.UserProfileId)
                              select new UserAuditTrail()
                              {
                                  Id = t_uat.Id,
                                  AppGlobalOwnerId = t_uat.AppGlobalOwnerId,
                                  ChurchBodyId = t_uat.ChurchBodyId,
                                  UserProfileId = t_uat.UserProfileId,
                                  EventDetail = t_uat.EventDetail,
                                  EventDate = t_uat.EventDate,
                                  AuditType = t_uat.AuditType,
                                  UI_Desc = t_uat.UI_Desc,
                                  Url = t_uat.Url,
                                  //
                                  Created = t_uat.Created,
                                  CreatedByUserId = t_uat.CreatedByUserId,
                                  LastMod = t_uat.LastMod,
                                  LastModByUserId = t_uat.LastModByUserId,
                                  //
                                  strEventDate = t_uat.EventDate != null ? DateTime.Parse(t_uat.EventDate.ToString()).ToString("ddd, dd-MMM-yyyy h:mm tt", CultureInfo.InvariantCulture) : "",
                                  strAuditType = GetAuditTypeDesc(t_uat.AuditType),
                                  strEventUser = t_up != null ? t_up.UserDesc : "",
                                  strSubscriber = (t_uat.AppGlobalOwner != null ? t_uat.AppGlobalOwner.OwnerName + " - " : "") + (t_uat.ChurchBody != null ? t_uat.ChurchBody.Name : ""),
                              })
                                 .OrderByDescending(c => c.Created).ToList();

            return userAudits;
        }

        private List<UserPermission> GetPermissionsByRole(int? userRoleId = null, int? oCurrChuBodyId = null)
        {  //System roles ... oCurrChuBodyId == null
           // if (oCurrChuBodyId == null) return new List<UserPermission>();

            var userPerms = (
                        from t_upr in _context.UserRolePermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userRoleId == null || (userRoleId != null && c.UserRoleId == userRoleId)))
                        from t_up in _context.UserPermission.Where(c => c.PermStatus == "A" && c.Id == t_upr.UserRoleId)
                        select t_up
                               )
                               .OrderBy(c => c.PermissionCode).ToList();

            return userPerms;
        }

        public JsonResult GetPermissionsListByRole(int? userRoleId, int? oCurrChuBodyId = null)  //, bool addEmpty = false)
        {
            var userPerms = (
                        from t_upr in _context.UserRolePermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userRoleId == null || (userRoleId != null && c.UserRoleId == userRoleId)))
                        from t_up in _context.UserPermission.Where(c => c.PermStatus == "A" && c.Id == t_upr.UserRoleId)
                        select t_up
                               ).OrderBy(c => c.PermissionCode).ToList()
                                .Select(c => new SelectListItem()
                                {
                                    Value = c.Id.ToString(),
                                    Text = c.PermissionName
                                })
                                .OrderBy(c => c.Text)
                                .ToList();

            // if (addEmpty) userPerms.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            return Json(userPerms);
        }

        private List<UserPermission> GetPermissionsByGroup(int? userGroupId = null, int? oCurrChuBodyId = null)
        {
            if (oCurrChuBodyId == null) return new List<UserPermission>();

            var userPerms = (
                        from t_upr in _context.UserGroupPermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userGroupId == null || (userGroupId != null && c.UserGroupId == userGroupId)))
                        from t_up in _context.UserPermission.Where(c => c.PermStatus == "A" && c.Id == t_upr.UserGroupId)
                               .OrderBy(c => c.PermissionCode)
                        select t_up
                               )
                               .ToList();

            return userPerms;
        }

        public JsonResult GetPermissionsListByGroup(int? userGroupId, int? oCurrChuBodyId = null)  //, bool addEmpty = false)
        {
            var userPerms = (
                        from t_upr in _context.UserGroupPermission.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Status == "A" && (userGroupId == null || (userGroupId != null && c.UserGroupId == userGroupId)))
                        from t_up in _context.UserPermission.Where(c => c.PermStatus == "A" && c.Id == t_upr.UserGroupId)
                        select t_up
                               ).OrderBy(c => c.PermissionCode).ToList()
                                .Select(c => new SelectListItem()
                                {
                                    Value = c.Id.ToString(),
                                    Text = c.PermissionName
                                })
                                .OrderBy(c => c.Text)
                                .ToList();

            // if (addEmpty) userPerms.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            return Json(userPerms);
        }


        // GET: ChurchBodyConfig 
        public ActionResult Index(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0, int? oParentId = null, int? id = null, int pageIndex = 1) //, int? oChuCategId = null, bool oShowAllCong = true) //, int? currFilterVal = null) //, ChurchBodyConfigMDL oCurrCBConfig = null)
        {
            //Request.Headers.Add("entityId", "COPDatabase");
            //Request.Headers.TryGetValue("entityId", out var entityVal);
            //var entityValue =  entityVal;

            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                // check permission 
                if (!this.userAuthorized) return View(new AppVenAdminVM()); //retain view                  
                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;  // if (oCurrChuBodyLogOn == null) return View(oCBConVM);
                if (oLoggedUser == null) return View(new AppVenAdminVM());
                //
                var oCBConVM = new AppVenAdminVM(); //TempData.Keep();  
                                                    // int? oAppGloOwnId = null;
                var oChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
                //
                int? oAppGloOwnId_Logged = null;
                int? oChuBodyId_Logged = null;
                int? oUserId_Logged = null;
                if (oChuBodyLogOn != null)
                {
                    oAppGloOwnId_Logged = oChuBodyLogOn.AppGlobalOwnerId;
                    oChuBodyId_Logged = oChuBodyLogOn.Id;
                    if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBodyLogOn.Id; }
                    if (oAppGloOwnId == null) { oAppGloOwnId = oChuBodyLogOn.AppGlobalOwnerId; }
                    //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
                    //
                    // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
                }


                int? oCurrChuMemberId_LogOn = null;
                Models.CLNTModels.ChurchMember oCurrChuMember_LogOn = null;


                oUserId_Logged = oLoggedUser.Id;
                //var currChurchMemberLogged = _clientContext.ChurchMember.Where(c => c.ChurchBodyId == oCurrChuBodyId && c.Id == oLoggedUser.ChurchMemberId).FirstOrDefault();
                //if (currChurchMemberLogged != null) //return View(oCBConVM);
                //{
                //    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
                //    oCurrChuMember_LogOn = currChurchMemberLogged;
                //}

                //                
                oCBConVM.oAppGloOwnId = oAppGloOwnId;
                oCBConVM.oChurchBodyId = oCurrChuBodyId;
                //
                oCBConVM.oUserId_Logged = oUserId_Logged;
                oCBConVM.oChurchBody_Logged = oChuBodyLogOn;
                oCBConVM.oAppGloOwnId_Logged = oAppGloOwnId_Logged;
                oCBConVM.oMemberId_Logged = oCurrChuMemberId_LogOn;
                //
                oCBConVM.setIndex = setIndex;
                oCBConVM.subSetIndex = subSetIndex;
                oCBConVM.pageIndex = pageIndex;
                //      

                // var oHomeDash1 = new HomeDashboardVM();

                // oCBConVM.strChurchLevelDown = "Assemblies";
                oCBConVM.strAppName = "RhemaCMS"; ViewBag.strAppName = oCBConVM.strAppName;
                oCBConVM.strAppNameMod = "Admin Palette"; ViewBag.strAppNameMod = oCBConVM.strAppNameMod;
                oCBConVM.strAppCurrUser = "Dan Abrokwa"; ViewBag.strAppCurrUser = oCBConVM.strAppCurrUser;
                // oHomeDash.strChurchType = "CH"; ViewBag.strChurchType = oHomeDash.strChurchType;
                // oHomeDash.strChuBodyDenomLogged = "Rhema Global Church"; ViewBag.strChuBodyDenomLogged = oHomeDash.strChuBodyDenomLogged;
                //  oHomeDash.strChuBodyLogged = "Rhema Comm Chapel"; ViewBag.strChuBodyLogged = oHomeDash.strChuBodyLogged;

                //           
                ViewBag.strAppCurrUser_ChRole = "System Adminitrator";
                ViewBag.strAppCurrUser_RoleCateg = "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewBag.strAppCurrUser_PhotoFilename = "2020_dev_sam.jpg";
                // ViewBag.strAppCurrChu_LogoFilename = "14dc86a7-81ae-462c-b73e-4581bd4ee2b2_church-of-pentecost.png";
                ViewBag.strUserSessionDura = "Logged: 10 minutes ago";

                //

                if (setIndex == 1) // user profiles
                {
                    var _strCurrTask = ""; var _proScope = ""; var _subScope = "";
                    if (subSetIndex >= 1 && subSetIndex <= 5)  // sys=1   sup_admin=2  sys_admin=3 sys_cust=4
                    {
                        _proScope = "V"; _subScope = "";
                        _strCurrTask = "System Admin Profiles";
                    }
                    else if (subSetIndex == 6 || subSetIndex == 11)  // CH_admin=6,CH_rgstr=7,CH_acct=8,CH_cust=9... CF_admin=11
                    {
                        _proScope = "C"; _subScope = "D";
                        _strCurrTask = "Church Admin Profiles";
                    }
                    else if (subSetIndex >= 6 && subSetIndex <= 15)  // CH_admin, CF_admin
                    {
                        _proScope = "C"; _subScope = "A";
                        _strCurrTask = "Church User Profiles";
                    }

                    if (pageIndex == 1) //read
                    {
                        //if (subSetIndex >= 1 && subSetIndex <= 5)  // sys   sup_admin  sys_admin sys_cust
                        //{
                        //    oCBConVM.lsUserProfiles = GetUserProfiles(null, null, "V", ""); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                        //    oCBConVM.strCurrTask = "System Admin Profiles";
                        //}
                        //else if (subSetIndex == 6 || subSetIndex == 11)  // CH_admin, CF_admin
                        //{
                        //    oCBConVM.lsUserProfiles = GetUserProfiles(null, null, "C", "D"); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                        //    oCBConVM.strCurrTask = "Church Admin Profiles";
                        //}
                        //else if (subSetIndex >= 6 && subSetIndex <= 15)  // CH_admin, CF_admin
                        //{
                        //    oCBConVM.lsUserProfiles = GetUserProfiles(null, null, "C", "A"); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                        //    oCBConVM.strCurrTask = "Church User Profiles";
                        //}

                        if (subSetIndex >= 1 && subSetIndex <= 15)
                        {
                            oCBConVM.lsUserProfiles = GetUserProfiles(oAppGloOwnId, oCurrChuBodyId, _proScope, _subScope);
                            oCBConVM.strCurrTask = _strCurrTask;
                        }
                        else if (subSetIndex == 21) //enlist all roles
                        {
                            oCBConVM.lsRoles = GetUserRoles(); // (null, null, "V", ""); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                            oCBConVM.strCurrTask = "System Roles";
                        }
                        else if (subSetIndex == 21) //enlist all privileges
                        {
                            oCBConVM.lsPermissions = GetUserPermissions(); // (null, null, "V", ""); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                            oCBConVM.strCurrTask = "System Privileges";
                        }
                    }

                    else if (pageIndex == 2 || pageIndex == 3)  //edit
                    {  //   
                        var oUserModel = AddOrEdit_UPR(oAppGloOwnId, oCurrChuBodyId, id, setIndex, subSetIndex, oAppGloOwnId_Logged, oChuBodyId_Logged, oUserId_Logged);
                        oCBConVM.oUserProfileModel = oUserModel;

                        // var oUserModel = new UserProfileModel();
                        // oUserModel.oUserProfile = new UserProfile(); //_context.UserProfile.FirstOrDefault();
                        // oUserModel.oCurrUserId_Logged = oUserProfile_Logged.Id; oUserModel.oAppGloOwnId_Logged = ; oUserModel.oChurchBodyId_Logged = oAppGloOwnId_Logged;
                        if (oUserModel != null)
                        {
                            var cc = "000000";
                            //get the roles, groups and privileges    .... _clientContext, 
                            oCBConVM.lsPermissions = AppUtilties.GetUserAssignedPermissions(_context, _proScope == "V" ? cc : oLoggedUser.ChurchBody?.GlobalChurchCode, oLoggedUser);
                            oCBConVM.lsProfileRoles = (from upr in _context.UserProfileRole.Where(c => c.UserProfileId == oLoggedUser.Id && (c.ChurchBodyId == null || c.ChurchBodyId == oLoggedUser.ChurchBodyId)) //&& c.ProfileRoleStatus == "A" && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))
                                                       from ur in _context.UserRole.Where(c => c.Id == upr.UserRoleId && (c.ChurchBodyId == null || c.ChurchBodyId == oLoggedUser.ChurchBodyId))
                                                       select upr).ToList();
                            oCBConVM.lsProfileGroups = (from upg in _context.UserProfileGroup.Where(c => c.UserProfileId == oLoggedUser.Id && (c.ChurchBodyId == null || c.ChurchBodyId == oLoggedUser.ChurchBodyId)) //&& c.ProfileRoleStatus == "A" && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))
                                                        from ur in _context.UserGroup.Where(c => c.Id == upg.UserGroupId && (c.ChurchBodyId == null || c.ChurchBodyId == oLoggedUser.ChurchBodyId))
                                                        select upg).ToList();

                            // the admin profiles
                            oCBConVM.lsUserProfiles = GetUserProfiles(oLoggedUser.AppGlobalOwnerId, oLoggedUser.ChurchBodyId, _proScope, _subScope);
                            //oCBConVM.strCurrTask = _strCurrTask;
                        }

                        else
                        {
                            Response.StatusCode = 403;  //obj null
                            return PartialView("ErrorPage");
                        }
                    }
                }

                else if (setIndex == 2) // church faith types
                {
                    //oCBConVM.lsFaithCategories = GetChurchFaithTypes(setIndex);

                    //if (subSetIndex == 0)
                    //{
                    //    oCBConVM.strCurrTask = "Faith Streams";
                    //}
                    //else if (subSetIndex == 1)
                    //{
                    //    oCBConVM.strCurrTask = "Faith Categories";
                    //}
                    //else
                    //{
                    //    oCBConVM.strCurrTask = "All Faith Types";
                    //}
                    //   // oCBConVM.strCurrTask = subSetIndex == 1 ? "Faith Category" : subSetIndex == 2 ? "Faith Sub-Category" : "All Faith Category";                  
                }

                else if (setIndex == 3)  //denominations
                {
                    if (oCBConVM.oCurrDenomVM != null)
                    {
                        oCBConVM.oCurrDenomVM.oAppGloOwnId = (int)oAppGloOwnId;

                        if (subSetIndex == 0)
                        {
                            oCBConVM.oCurrDenomVM = GetDenomination((int)oAppGloOwnId);
                            oCBConVM.strCurrTask = "Denominations";
                        }
                        else if (subSetIndex == 1)
                        {
                            oCBConVM.oCurrDenomVM.lsChurchLevels = GetChurchLevels((int)oAppGloOwnId);
                            oCBConVM.strCurrTask = "Church Levels";
                        }
                        else if (subSetIndex == 2)
                        {
                            oCBConVM.oCurrDenomVM.lsChurchBodies = GetCongregations((int)oAppGloOwnId);
                            oCBConVM.strCurrTask = "Congregations";
                        }

                        else if (subSetIndex == 3)
                        {
                            oCBConVM.lsUserProfiles = GetUserProfiles((int)oAppGloOwnId, (int)oCurrChuBodyId, "C", "D"); // -- GET ALL CH ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                            oCBConVM.strCurrTask = "Church Admin Profiles";
                        }

                        else if (subSetIndex == 4)
                        {
                            // oCBConVM.oCurrDenomVM.lsSubscriptions = Subscriptions((int)oAppGloOwnId, "C", "D", 6);
                            oCBConVM.strCurrTask = "Subcriptions";
                        }

                        //else if (subSetIndex == 5)
                        //{
                        //    oCBConVM.oCurrDenomVM.ChurchAdminProfiles = GetUserProfiles(); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                        //    oCBConVM.strCurrTask = "Vendor User Profiles";
                        //}

                        else   // subSetIndex == 0
                        {
                            oCBConVM.lsDenominations = GetDenominations();
                            oCBConVM.strCurrTask = "Denominations";
                        }
                    }

                    else   // subSetIndex == 0
                    {
                        oCBConVM.lsDenominations = GetDenominations();
                        oCBConVM.strCurrTask = "Denominations";
                    }
                }

                else if (setIndex == 4)  //all subsriptions
                {

                }

                else if (setIndex == 5)  // other app parameters
                {
                    oCBConVM.lsCountries = GetCountries();
                    oCBConVM.strCurrTask = "Countries";
                }


                //  if (setIndex == 6) { oCBConVM.lsAppSubscriptions = GetAppSubscriptions(); oCBConVM.strCurrTask = "App Subscriptions"; }
                //if (setIndex == 7) { oCBConVM.lsUserProfiles = GetUserProfiles(oCBConVM.oChurchBody.Id); oCBConVM.strCurrTask = "User Profiles"; }
                //if (setIndex == 10) { oCBConVM.lsCountries = GetCountries(); oCBConVM.strCurrTask = "Countries & Regions"; }
                //if (setIndex == 11) { oCBConVM.lsCountryRegions = GetCountryRegions(oParentId); oCBConVM.strCurrTask = "Country Regions"; }

                //
                // TempData.Put("oVmCB_CNFG", oCBConVM);
                TempData.Keep();
                return View(oCBConVM);
            }
        }

        public IActionResult AddOrEdit_CNFG(int? oDenomId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0,
                                                 int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)

        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                // int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                //int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                // int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                if (StackAppUtilties.IsAjaxRequest(HttpContext.Request))
                {
                    if (setIndex == 1) // vendor admins
                    {
                        var oUserModel = AddOrEdit_UPR(oDenomId, oCurrChuBodyId, id, setIndex, subSetIndex, oAGOId_Logged, oCBId_Logged, oUserId_Logged);
                        // var oUserModel = new UserProfileModel();
                        // oUserModel.oUserProfile = new UserProfile(); //_context.UserProfile.FirstOrDefault();
                        // oUserModel.oCurrUserId_Logged = oUserProfile_Logged.Id; oUserModel.oAppGloOwnId_Logged = ; oUserModel.oChurchBodyId_Logged = oAppGloOwnId_Logged;
                        if (oUserModel != null)
                            if (oUserModel.oUserProfile != null)
                                return PartialView("_AddOrEdit_UPR", oUserModel); //PartialView("_AddOrEdit_UPR", oMdl1); // break;
                            else
                            {
                                Response.StatusCode = 500;
                                return PartialView("ErrorPage");
                            }
                        else
                        {
                            Response.StatusCode = 403;  //obj null
                            return PartialView("ErrorPage");
                        }
                    }

                    //else if (setIndex == 2) // church faith types
                    //{
                    //    //var oCFTModel = AddOrEdit_CFT(oDenomId, oCurrChuBodyId, id, setIndex, subSetIndex); 
                    //    //if (oCFTModel != null)
                    //    //    if (oCFTModel.oChurchFaithType != null)
                    //    //        return PartialView("_AddOrEdit_CFT", oCFTModel); //PartialView("_AddOrEdit_UPR", oMdl1); // break;
                    //    //    else
                    //    //    {
                    //    //        Response.StatusCode = 500;
                    //    //        return PartialView("ErrorPage");
                    //    //    }
                    //    //else
                    //    //{
                    //    //    Response.StatusCode = 403;  //obj null
                    //    //    return PartialView("ErrorPage");
                    //    //}




                    //    //var oMdl1 = AddOrEdit_CFT(oDenomId, oCurrChuBodyId, id, setIndex, subSetIndex);
                    //    //if (oMdl1 != null) return PartialView("_AddOrEdit_CFT", oMdl1); // break;
                    //    //else
                    //    //{
                    //    //    Response.StatusCode = 403;  //obj null
                    //    //    return PartialView("ErrorPage");
                    //    //}

                    //    //// oCBConVM.lsFaithCategories = GetChurchFaithTypes(setIndex);

                    //    //if (subSetIndex == 1)
                    //    //{
                    //    //     oCBConVM.strCurrTask = "Faith Types";
                    //    //}
                    //    //else if (subSetIndex == 2)
                    //    //{
                    //    //    oCBConVM.strCurrTask = "Faith Sub-Types";
                    //    //}
                    //    //else
                    //    //{
                    //    //    oCBConVM.strCurrTask = "All Faith Types";
                    //    //}
                    //    // oCBConVM.strCurrTask = subSetIndex == 1 ? "Faith Category" : subSetIndex == 2 ? "Faith Sub-Category" : "All Faith Category";                  
                    //}

                    //else if (setIndex == 3)  //denominations
                    //{
                    //    if (oDenomId != null)
                    //    {
                    //       // oCBConVM.oCurrDenomVM.oAppGloOwnId = (int)oDenomId;

                    //        if (subSetIndex == 0)
                    //        {
                    //            //oCBConVM.oCurrDenomVM = GetDenomination((int)oDenomId);
                    //            //oCBConVM.strCurrTask = "Denominations";
                    //        }
                    //        else if (subSetIndex == 1)
                    //        {
                    //            //oCBConVM.oCurrDenomVM.lsChurchLevels = GetChurchLevels((int)oDenomId);
                    //            //oCBConVM.strCurrTask = "Church Levels";
                    //        }
                    //        else if (subSetIndex == 2)
                    //        {
                    //           // oCBConVM.oCurrDenomVM.lsChurchBodies = GetCongregations((int)oDenomId);
                    //            //oCBConVM.strCurrTask = "Congregations";
                    //        }
                    //        else if (subSetIndex == 3)
                    //        {
                    //            //oCBConVM.oCurrDenomVM.lsChurchAdminProfiles = GetUserProfiles((int)oDenomId, "C", "D", 6);
                    //           // oCBConVM.strCurrTask = "Church Admin Profiles";
                    //        }

                    //        else if (subSetIndex == 4)
                    //        {
                    //            // oCBConVM.oCurrDenomVM.lsSubscriptions = Subscriptions((int)oDenomId, "C", "D", 6);
                    //            //oCBConVM.strCurrTask = "Subcriptions";
                    //        }

                    //        //else if (subSetIndex == 5)
                    //        //{
                    //        //    oCBConVM.oCurrDenomVM.ChurchAdminProfiles = GetUserProfiles(); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                    //        //    oCBConVM.strCurrTask = "Vendor User Profiles";
                    //        //}

                    //        else   // subSetIndex == 0
                    //        {
                    //           // oCBConVM.lsDenominations = GetDenominations();
                    //            //oCBConVM.strCurrTask = "Denominations";
                    //        }
                    //    }

                    //    else   // subSetIndex == 0
                    //    {
                    //       // oCBConVM.lsDenominations = GetDenominations();
                    //       // oCBConVM.strCurrTask = "Denominations";
                    //    }
                    //}

                    //else if (setIndex == 4)  //all subsriptions
                    //{

                    //}

                    //else if (setIndex == 5)  // other app parameters
                    //{
                    //    //oCBConVM.lsCountries = GetCountries();
                    //    //oCBConVM.strCurrTask = "Countries";
                    //    return View(); //clear line later
                    //} 

                    else
                        return View(); //clear line later
                                       //}
                }

                //page not found error
                Response.StatusCode = 500;
                return PartialView("ErrorPage");
            }
        }

        public UserProfileModel AddOrEdit_UPR(int? oDenomId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int subSetIndex = 0,
                                                                            int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) //, int pageIndex = 1)
        {

            var oUserModel = new UserProfileModel(); TempData.Keep();
            if (setIndex == 0) return oUserModel;

            // 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
            var proScope = "V"; var subScope = "";
            if (subSetIndex >= 1 && subSetIndex <= 5) { proScope = "V"; subScope = ""; }
            else if (subSetIndex == 6 || subSetIndex == 11) { proScope = "C"; subScope = "D"; }
            else if (subSetIndex >= 6 && subSetIndex <= 15) { proScope = "C"; subScope = "A"; }

            if (id == 0)
            {   //create user and init... 
                //var existSUP_ADMNs = (
                //   from t_up in _context.UserProfile.AsNoTracking() //.Include(t => t.ChurchMember)
                //                .Where(c => c.Id == id &&
                //                (c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileScope == "V") ||
                //                (c.AppGlobalOwnerId == oDenomId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "C"))
                //   from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId && c.AppGlobalOwnerId == t_up.AppGlobalOwnerId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                //   from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                //                    .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id &&
                //                    ((proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5)) ||
                //                     ((proScope == "C" && subScope == "D" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                //                     ((proScope == "C" && subScope == "A" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && (c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15))
                //                    )).DefaultIfEmpty()
                //   select t_up
                //   ).ToList();

                ////supadmin <creation> task.... but must have logged in as SYS
                //if (setIndex==1 && subSetIndex==2 && existSUP_ADMNs.Count > 0)
                //{ //prompt user sup_admin == 1 only
                //    oUserModel.oUserProfile = null;
                //    return oUserModel;
                //}

                var oUser = new UserProfile();
                oUser.ChurchBodyId = oCurrChuBodyId;
                oUser.Strt = DateTime.Now;
                oUser.ResetPwdOnNextLogOn = true;

                //oUPR_MDL.oUserProfile.CountryId = oCurrCtryId;

                oUser.UserStatus = "A";   // A-ctive...D-eactive   
                oUser.ProfileScope = proScope;

                if (subSetIndex >= 1 && subSetIndex <= 5) // 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
                {
                    oUser.UserScope = "E";  // I-internal, E-external
                    if (subSetIndex == 2) { oUser.Username = "supadmin"; oUser.UserDesc = "Super Admin"; }
                }
                else // I-internal, E-external [manually config]
                { oUser.UserScope = "I"; }

                oUserModel.oUserProfile = oUser;
            }

            else
            {
                var oUser = (
                   from t_up in _context.UserProfile.Include(t => t.ContactInfo).AsNoTracking() //.Include(t => t.ChurchMember)
                                .Where(c => c.Id == id &&
                                (c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileScope == "V") ||
                                (c.AppGlobalOwnerId == oDenomId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "C"))
                   from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId && c.AppGlobalOwnerId == t_up.AppGlobalOwnerId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                   from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                                    .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id &&
                                    ((proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5)) ||
                                     ((proScope == "C" && subScope == "D" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                                     ((proScope == "C" && subScope == "A" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && (c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15))
                                    )).DefaultIfEmpty()

                       //// from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole).Where(c => c.UserProfileId == t_up.Id).DefaultIfEmpty()
                       // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == t_up.ChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()
                       // from t_ur in _context.UserRole.AsNoTracking().Where(c => c.Id == t_upr.UserRoleId &&
                       //              ((c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5)) ||
                       //              ((c.RoleType == "CH_ADMN" || c.RoleType == "CF_ADMN") && (c.RoleLevel >= 6 && c.RoleLevel <= 10))).DefaultIfEmpty()
                       // from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                       //              .Where(c => c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                       // from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                       //              .Where(c => c.UserProfileId == t_up.Id).DefaultIfEmpty()
                       // from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                       //              .Where(c => c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

                   select new UserProfile()
                   {
                       Id = t_up.Id,
                       AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                       ChurchBodyId = t_up.ChurchBodyId,
                       ChurchMemberId = t_up.ChurchMemberId,
                       ChurchBody = t_up.ChurchBody,
                       //  ChurchMember = t_up.ChurchMember,
                       OwnerUser = t_up.OwnerUser,
                       //
                       Username = t_up.Username,
                       UserDesc = t_up.UserDesc,
                       Email = t_up.Email,
                       ContactInfo = t_up.ContactInfo,
                       // PhoneNum = t_up.PhoneNum,
                       Pwd = t_up.Pwd,
                       PwdExpr = t_up.PwdExpr,
                       PwdSecurityQue = t_up.PwdSecurityQue,
                       PwdSecurityAns = t_up.PwdSecurityAns,
                       ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                       Strt = t_up.Strt,
                       strStrt = t_up.strStrt,
                       Expr = t_up.Expr,
                       strExpr = t_up.strExpr != null ? DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                       //
                       OwnerUserId = t_up.OwnerUserId,
                     //  UserId = t_up.UserId,
                       UserScope = t_up.UserScope,
                       UserPhoto = t_up.UserPhoto,
                       ProfileScope = t_up.ProfileScope,
                       Created = t_up.Created,
                       CreatedByUserId = t_up.CreatedByUserId,
                       LastMod = t_up.LastMod,
                       LastModByUserId = t_up.LastModByUserId,
                       UserStatus = t_up.UserStatus,
                       strUserStatus = GetStatusDesc(t_up.UserStatus)
                   }
                   ).FirstOrDefault();

                oUserModel.oUserProfile = oUser;
                if (oUser != null)
                {
                    oUserModel.strUserProfile = oUser.UserDesc;
                    oUserModel.strChurchBody = oUser.ChurchBody != null ? oUser.ChurchBody.Name : "";
                    oUserModel.strAppGloOwn = oUser.AppGlobalOwner != null ? oUser.AppGlobalOwner.OwnerName : "";

                    //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                    // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc

                }
            }

            if (oUserModel.oUserProfile != null)
            {
                if (oUserModel.oUserProfile.AppGlobalOwnerId != null)
                {
                    List<MSTRChurchLevel> oCBLevels = _context.MSTRChurchLevel
                        .Where(c => c.AppGlobalOwnerId == oUserModel.oUserProfile.AppGlobalOwnerId).ToList().OrderBy(c => c.LevelIndex).ToList();

                    if (oCBLevels.Count() > 0)
                    {
                        ViewBag.Filter_ln = !string.IsNullOrEmpty(oCBLevels[oCBLevels.Count - 1].CustomName) ? oCBLevels[oCBLevels.Count - 1].CustomName : oCBLevels[6].Name;
                        ViewBag.Filter_1 = !string.IsNullOrEmpty(oCBLevels[0].CustomName) ? oCBLevels[0].CustomName : oCBLevels[0].Name;

                        if (oCBLevels.Count() > 1)
                        {
                            ViewBag.Filter_2 = ViewBag.Filter_2 = !string.IsNullOrEmpty(oCBLevels[1].CustomName) ? oCBLevels[1].CustomName : oCBLevels[1].Name;
                            if (oCBLevels.Count() > 2)
                            {
                                ViewBag.Filter_3 = ViewBag.Filter_3 = !string.IsNullOrEmpty(oCBLevels[2].CustomName) ? oCBLevels[2].CustomName : oCBLevels[2].Name;
                                if (oCBLevels.Count() > 3)
                                {
                                    ViewBag.Filter_4 = ViewBag.Filter_4 = !string.IsNullOrEmpty(oCBLevels[3].CustomName) ? oCBLevels[3].CustomName : oCBLevels[3].Name;
                                    if (oCBLevels.Count() > 4)
                                    {
                                        ViewBag.Filter_5 = ViewBag.Filter_5 = !string.IsNullOrEmpty(oCBLevels[4].CustomName) ? oCBLevels[43].CustomName : oCBLevels[4].Name;
                                        if (oCBLevels.Count() > 5)
                                        {
                                            ViewBag.Filter_6 = ViewBag.Filter_6 = !string.IsNullOrEmpty(oCBLevels[5].CustomName) ? oCBLevels[5].CustomName : oCBLevels[5].Name;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            oUserModel.setIndex = setIndex;
            oUserModel.subSetIndex = subSetIndex;
            oUserModel.oCurrUserId_Logged = oUserId_Logged;
            oUserModel.oAppGloOwnId_Logged = oAGOId_Logged;
            oUserModel.oChurchBodyId_Logged = oCBId_Logged;
            //
            oUserModel.oAppGloOwnId = oDenomId;
            oUserModel.oChurchBodyId = oCurrChuBodyId;
            //  oUserModel.oCurrUserId_Logged = oCurrUserId_Logged; 

            // ChurchBody oCB = null;
            // if (oCurrChuBodyId != null)  oCB = _context.MSTRChurchBody.Where(c=>c.Id == oCurrChuBodyId && c.AppGlobalOwnerId==oDenomId).FirstOrDefault();

            if (subSetIndex >= 1 && subSetIndex <= 5) // no SUP_ADMN or SYS as option
                oUserModel = this.populateLookups_UPR_MS(oUserModel, oDenomId, subSetIndex);

            else if ((oUserModel.profileScope == "V" && (subSetIndex == 6 || subSetIndex == 11)) || (oUserModel.profileScope == "C" && subSetIndex >= 6 && subSetIndex <= 15))
            {
                // var oCB = _context.MSTRChurchBody.Where(c => c.Id == oCurrChuBodyId && c.AppGlobalOwnerId == oDenomId).FirstOrDefault();
                oUserModel = this.populateLookups_UPR_CL(oUserModel, oCurrChuBodyId);
            }

            //oUPR_MDL.lkpStatuses = new List<SelectListItem>();
            //foreach (var dl in dlGenStatuses) { oUPR_MDL.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            //TempData["oVmCurr"] = oUserModel;
            //TempData.Keep();

            // var _oUserModel = Newtonsoft.Json.JsonConvert.SerializeObject(oUserModel);
            // TempData["oVmCurr"] = _oUserModel; TempData.Keep();

            var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(oUserModel);
            TempData["oVmCurrMod"] = _vmMod; TempData.Keep();

            return oUserModel;
        }


        //public UserProfileVM AddOrEdit_UPR2(int? oDenomId = null, int? oCurrChuBodyId = null, int id = 0, int setIndex = 0, int subSetIndex = 0)
        public UserProfileModel AddOrEdit_UPR2(int? oDenomId = null, int? oCurrChuBodyId = null, int id = 0, int setIndex = 0, int subSetIndex = 0)
        {
            var oUPR_MDL = new UserProfileModel(); TempData.Keep();
            if (setIndex == 0) return oUPR_MDL;
            oUPR_MDL.setIndex = setIndex;
            oUPR_MDL.subSetIndex = subSetIndex;

            ////
            //var oCurrChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
            //var oUserProfile = oUserLogIn_Priv[0].UserProfile;
            //if (oCurrChuBodyLogOn == null) return oUPR_MDL;

            //if (oCurrChuBodyId == null) oCurrChuBodyId = oCurrChuBodyLogOn.Id;
            //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...

            //// check permission for Core life...
            //if (oUserLogIn_Priv.Find(x => x.PermissionName == "Manage_SuperAdmin_Priv" || x.PermissionName == "xxx") == null) //prompt!
            //    return oUPR_MDL;

            //int? oCurrChuMemberId_LogOn = null;
            //ChurchMember oCurrChuMember_LogOn = null;
            //if (oUserProfile == null) //prompt!
            //    return oUPR_MDL;
            //if (oUserProfile.ChurchMember == null) //prompt!
            //    return oUPR_MDL;

            //oCurrChuMemberId_LogOn = oUserProfile.ChurchMember.Id;
            //oCurrChuMember_LogOn = oUserProfile.ChurchMember;

            if (id == 0)
            {         //create user and init... 
                oUPR_MDL.oUserProfile = new UserProfile();
                oUPR_MDL.oUserProfile.ChurchBodyId = oCurrChuBodyId;
                //oUPR_MDL.oUserProfile.CountryId = oCurrCtryId;
                oUPR_MDL.oUserProfile.UserScope = "I"; // I-internal, E-external
                oUPR_MDL.oUserProfile.UserStatus = "A";   // A-ctive...D-eactive   

                if (setIndex == 1) // sys admin
                {
                    oUPR_MDL.oUserProfile.ProfileScope = "V"; // V-Vendor, C-Client
                }

                else if (setIndex == 3) // && subSetIndex==3) //church admin acc
                {
                    oUPR_MDL.oUserProfile.ProfileScope = "C";
                }
            }

            else
            {
                if (setIndex == 1) // sys admin
                {
                    oUPR_MDL = (
                    from t_up in _context.UserProfile.Include(t => t.ContactInfo).AsNoTracking().Where(c => c.Id == id && c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileScope == "V")  //.Include(t => t.ChurchMember)
                    from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                    from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole).Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                        // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
                    from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
                                       (c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5))
                        //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
                    from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                                  .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                    from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                                 .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                    from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                                 .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

                    select new UserProfileModel()
                    {
                        // oUserProfile = t_up,

                        oUserProfile = new UserProfile()
                        {
                            Id = t_up.Id,
                            AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                            ChurchBodyId = t_up.ChurchBodyId,
                            ChurchMemberId = t_up.ChurchMemberId,
                            ChurchBody = t_up.ChurchBody,
                            // ChurchMember = t_up.ChurchMember,
                            OwnerUser = t_up.OwnerUser,

                            Username = t_up.Username,
                            UserDesc = t_up.UserDesc,
                            Email = t_up.Email,
                            ContactInfo = t_up.ContactInfo,
                            //PhoneNum = t_up.PhoneNum,
                            Pwd = t_up.Pwd,
                            PwdExpr = t_up.PwdExpr,
                            PwdSecurityQue = t_up.PwdSecurityQue,
                            PwdSecurityAns = t_up.PwdSecurityAns,
                            ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                            Strt = t_up.Strt,
                            strStrt = t_up.strStrt,
                            Expr = t_up.Expr,
                            strExpr = t_up.strExpr != null ?
                                                                 DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                            OwnerUserId = t_up.OwnerUserId,
                          //  UserId = t_up.UserId,
                            UserScope = t_up.UserScope,
                            UserPhoto = t_up.UserPhoto,
                            ProfileScope = t_up.ProfileScope,
                            Created = t_up.Created,
                            CreatedByUserId = t_up.CreatedByUserId,
                            LastMod = t_up.LastMod,
                            LastModByUserId = t_up.LastModByUserId,
                            UserStatus = t_up.UserStatus,
                            strUserStatus = GetStatusDesc(t_up.UserStatus)

                        },

                        //  lsUserGroups = t_upg.UserGroups,
                        // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
                        // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

                        strUserProfile = t_up.UserDesc,

                        strChurchBody = t_cb != null ? t_cb.Name : "",
                        strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                        //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                        // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
                    }
                    )
                    //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
                    .FirstOrDefault();
                }

                else if (setIndex == 3) // && subSetIndex==3) //church admin acc
                {

                    oUPR_MDL = (
                      from t_up in _context.UserProfile.Include(t => t.ContactInfo).AsNoTracking().Where(c => c.Id == id && c.AppGlobalOwnerId == oDenomId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "C")  //.Include(t => t.ChurchMember)
                      from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                      from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole).Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                          // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == oChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()                   
                      from t_ur in _context.UserRole.AsNoTracking().Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId &&
                                       (c.RoleType == "CH_ADMN" || c.RoleType == "CF_ADMN") && (c.RoleLevel >= 6 && c.RoleLevel <= 10))
                          //   (c.RoleType != null && c.RoleLevel == roleLevel) || (roleLevel == null && c.RoleLevel > 0 && c.RoleLevel <= 5)))  //.DefaultIfEmpty()
                      from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                              .Where(c => c.ChurchBodyId == null && c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                      from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                                   .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id).DefaultIfEmpty()
                      from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                                   .Where(c => c.ChurchBodyId == null && c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

                      select new UserProfileModel()
                      {
                          // oUserProfile = t_up,

                          oUserProfile = new UserProfile()
                          {
                              Id = t_up.Id,
                              AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                              ChurchBodyId = t_up.ChurchBodyId,
                              ChurchMemberId = t_up.ChurchMemberId,
                              ChurchBody = t_up.ChurchBody,
                              // ChurchMember = t_up.ChurchMember,
                              OwnerUser = t_up.OwnerUser,

                              Username = t_up.Username,
                              UserDesc = t_up.UserDesc,
                              Email = t_up.Email,
                              ContactInfo = t_up.ContactInfo,
                              // PhoneNum = t_up.PhoneNum,
                              Pwd = t_up.Pwd,
                              PwdExpr = t_up.PwdExpr,
                              PwdSecurityQue = t_up.PwdSecurityQue,
                              PwdSecurityAns = t_up.PwdSecurityAns,
                              ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                              Strt = t_up.Strt,
                              strStrt = t_up.strStrt,
                              Expr = t_up.Expr,
                              strExpr = t_up.strExpr != null ?
                                                                   DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                              OwnerUserId = t_up.OwnerUserId,
                            //  UserId = t_up.UserId,
                              UserScope = t_up.UserScope,
                              UserPhoto = t_up.UserPhoto,
                              ProfileScope = t_up.ProfileScope,
                              Created = t_up.Created,
                              CreatedByUserId = t_up.CreatedByUserId,
                              LastMod = t_up.LastMod,
                              LastModByUserId = t_up.LastModByUserId,
                              UserStatus = t_up.UserStatus,
                              strUserStatus = GetStatusDesc(t_up.UserStatus)

                          },

                          //  lsUserGroups = t_upg.UserGroups,
                          // lsUserRoles = t_upr != null ? t_upr.UserRoles : null,
                          // lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),

                          strUserProfile = t_up.UserDesc,

                          strChurchBody = t_cb != null ? t_cb.Name : "",
                          strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                          //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                          // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
                      }
                      )
                      //.OrderBy(c => c.oUserRole.RoleDesc).ThenBy(c => c.strUserProfile)
                      .FirstOrDefault();
                }


                //oUPR_MDL = (
                //      from t_up in _context.UserProfile.AsNoTracking().Include(t => t.ChurchMember)
                //      .Where(x => x.ChurchBodyId == oCurrChuBodyId && x.Id == id)
                //      from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(x => x.Id == oCurrChuBodyId && x.Id == t_up.ChurchBodyId)
                //      from t_cm in _context.ChurchMember.AsNoTracking().Where(x => x.Id == oCurrChuBodyId && x.Id == t_up.ChurchMemberId)
                //      from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRoles).Where(x => x.ChurchBodyId == oCurrChuBodyId && x.UserProfileId == t_up.Id).DefaultIfEmpty()
                //      from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermissions).Where(x => x.ChurchBodyId == oCurrChuBodyId && x.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                //      from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroups).Where(x => x.ChurchBodyId == oCurrChuBodyId && x.UserProfileId == t_up.Id).DefaultIfEmpty()
                //      from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermissions).Where(x => x.ChurchBodyId == oCurrChuBodyId && x.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()
                //      select new UserProfileVM()
                //      {
                //          oUserProfile = t_up,
                //          lsUserGroups = t_upg.UserGroups,
                //          lsUserRoles = t_upr.UserRoles,
                //          lsUserPermissions = CombineCollection(t_urp.UserPermissions, t_ugp.UserPermissions, null, null, null),
                //          strCongregation = t_cb != null ? t_cb.Name : "",
                //          strAppGloOwn = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                //          strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                //          strUserProfile = ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim()
                //      }
                //    ).FirstOrDefault();
            }


            oUPR_MDL.oAppGloOwnId = oDenomId;
            oUPR_MDL.oChurchBodyId = oCurrChuBodyId;
            //oUPR_MDL.oCurrLoggedMember = oCurrChuMember_LogOn;
            //oUPR_MDL.oCurrLoggedMemberId = oCurrChuMemberId_LogOn;

            // ChurchBody oCB = null;
            // if (oCurrChuBodyId != null)  oCB = _context.MSTRChurchBody.Where(c=>c.Id == oCurrChuBodyId && c.AppGlobalOwnerId==oDenomId).FirstOrDefault();

            if (setIndex == 1 || (setIndex == 3 && subSetIndex == 3))
                oUPR_MDL = this.populateLookups_UPR_MS(oUPR_MDL, oDenomId, setIndex);

            else
            {
                // var oCB = _context.MSTRChurchBody.Where(c => c.Id == oCurrChuBodyId && c.AppGlobalOwnerId == oDenomId).FirstOrDefault();
                oUPR_MDL = this.populateLookups_UPR_CL(oUPR_MDL, oCurrChuBodyId);
            }


            //oUPR_MDL.lkpStatuses = new List<SelectListItem>();
            //foreach (var dl in dlGenStatuses) { oUPR_MDL.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            TempData["oVmCurr"] = oUPR_MDL;
            TempData.Keep();

            return oUPR_MDL;

        }


        /// ////////////////// 

        private UserProfileModel populateLookups_UPR_CL(UserProfileModel vmLkp, int? oChurchBodyId)  //AppGloOwnId   ChurchBody oCurrChuBody)
        {
            if (vmLkp == null || oChurchBodyId == null) return vmLkp;
            //
            vmLkp.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            vmLkp.lkpUserRoles = _context.UserRole.Where(c => c.RoleStatus == "A" && c.RoleLevel >= 6 && c.ChurchBodyId == oChurchBodyId)
                               .OrderBy(c => c.RoleLevel)
                               .Select(c => new SelectListItem()
                               {
                                   Value = c.Id.ToString(),
                                   Text = c.RoleName.Trim()
                               })
                               // .OrderBy(c => c.Text)
                               .ToList();
            //  vmLkp.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            vmLkp.lkpUserGroups = _context.UserGroup.Where(c => c.Status == "A" && c.ChurchBodyId == oChurchBodyId)
                               .OrderBy(c => c.UserGroupCategoryId).ThenBy(c => c.GroupName)
                               .Select(c => new SelectListItem()
                               {
                                   Value = c.Id.ToString(),
                                   Text = c.GroupName.Trim()
                               })
                               // .OrderBy(c => c.Text)
                               .ToList();


            vmLkp.lkpPwdSecQueList = _context.AppUtilityNVP.Where(c => c.NvpCode == "PWD_SEC_QUE")
                      .OrderBy(c => c.RequireUserCustom).ThenBy(c => c.OrderIndex).ThenBy(c => c.NvpValue)
                      .ToList()
                      .Select(c => new SelectListItem()
                      {
                          Value = c.Id.ToString(),
                          Text = c.NvpValue
                      })
                      // .OrderBy(c => c.Text)
                      .ToList();
            vmLkp.lkpPwdSecQueList.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            //vmLkp.lkpPwdSecAnsList = _context.AppUtilityNVP.Where(c => c.NvpCode == "PWD_SEC_ANS")
            //         .OrderBy(c => c.RequireUserCustom).ThenBy(c => c.OrderIndex).ThenBy(c => c.NvpValue)
            //         .ToList()
            //         .Select(c => new SelectListItem()
            //         {
            //             Value = c.Id.ToString(),
            //             Text = c.NvpValue
            //         })
            //         // .OrderBy(c => c.Text)
            //         .ToList();
            //vmLkp.lkpPwdSecAnsList.Insert(0, new SelectListItem { Value = "", Text = "Select" });


            return vmLkp;
        }

        private UserProfileModel populateLookups_UPR_MS(UserProfileModel vmLkp, int? AppGloOwnId, int subSetIndex)
        {
            //if (vmLkp == null || oDenom == null) return vmLkp;
            // 
            vmLkp.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            //vmLkp.lkpUserRoles = _context.UserRole.Where(c => AppGloOwnId == null && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 0 && c.RoleType=="SUP_ADMN" )
            if (subSetIndex >= 2 && subSetIndex <= 3)  //   SYS .. 1-SUP_ADMN, 2-SYS_ADMN, 3-SYS_CUST
            {
                vmLkp.lkpUserRoles = _context.UserRole.Where(c => AppGloOwnId == null && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel >= 2 && c.RoleLevel <= 5)
                    .OrderBy(c => c.RoleLevel)
                    .Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.RoleName.Trim()
                    })
                    // .OrderBy(c => c.Text)
                    .ToList();
                //  vmLkp.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });

                vmLkp.lkpUserGroups = _context.UserGroup.Where(c => AppGloOwnId == null && c.ChurchBodyId == null && c.Status == "A")
                                   .OrderBy(c => c.UserGroupCategoryId).ThenBy(c => c.GroupName)
                                   .Select(c => new SelectListItem()
                                   {
                                       Value = c.Id.ToString(),
                                       Text = c.GroupName.Trim()
                                   })
                                   // .OrderBy(c => c.Text)
                                   .ToList();
            }

            else if (subSetIndex == 6 || subSetIndex == 7)  //  6-CH_ADMN, 7-CF_ADMN
            {
                vmLkp.lkpUserRoles = _context.UserRole.Where(c => c.RoleStatus == "A" && c.RoleLevel >= 6 && c.RoleLevel <= 10 && c.ChurchBody.AppGlobalOwnerId == AppGloOwnId)
                    .OrderBy(c => c.RoleLevel)
                    .Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.RoleName.Trim()
                    })
                    // .OrderBy(c => c.Text)
                    .ToList();
                //  vmLkp.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });

                vmLkp.lkpUserGroups = _context.UserGroup.Where(c => c.Status == "A" && c.ChurchBody.AppGlobalOwnerId == AppGloOwnId)
                                   .OrderBy(c => c.UserGroupCategoryId).ThenBy(c => c.GroupName)
                                   .Select(c => new SelectListItem()
                                   {
                                       Value = c.Id.ToString(),
                                       Text = c.GroupName.Trim()
                                   })
                                   // .OrderBy(c => c.Text)
                                   .ToList();
            }

            return vmLkp;
        }
         

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_UPR(UserProfileModel vmMod)
        {

            UserProfile _oChanges = vmMod.oUserProfile;
            //   vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();

            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfileModel>(arrData) : vmMod;

            var oCV = vmMod.oUserProfile;
            oCV.ChurchBody = vmMod.oChurchBody;

            try
            {
                ModelState.Remove("oUserProfile.AppGlobalOwnerId");
                ModelState.Remove("oUserProfile.ChurchBodyId");
                ModelState.Remove("oUserProfile.ChurchMemberId");
                ModelState.Remove("oUserProfile.CreatedByUserId");
                ModelState.Remove("oUserProfile.LastModByUserId");
                ModelState.Remove("oUserProfile.OwnerId");
                ModelState.Remove("oUserProfile.UserId");

                // ChurchBody == null 

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", signOutToLogIn = false });

                if (string.IsNullOrEmpty(_oChanges.Username)) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide username and password.", signOutToLogIn = false });
                }
                //if (_oChanges.PwdSecurityQue != null && string.IsNullOrEmpty(_oChanges.PwdSecurityAns))  //Congregant... ChurcCodes required
                //{
                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the response to the security question specified.", signOutToLogIn = false });
                //}



                //confirm this is SYS acc   //check for the SYS acc
                var currLogUserInfo = (from up in _context.UserProfile.Where(c => c.ChurchBodyId == null && c.Id == vmMod.oCurrUserId_Logged)
                                       from upr in _context.UserProfileRole.Where(c => c.UserProfileId == up.Id && c.ChurchBodyId == null && c.ProfileRoleStatus == "A" && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))
                                       from ur in _context.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SYS")
                                       select new
                                       {
                                           UserId = up.Id,
                                           UserRoleId = ur.Id,
                                           UserType = ur.RoleType,
                                           UserRoleLevel = ur.RoleLevel,
                                           UserStatus = up.strUserStatus == "A" && upr.ProfileRoleStatus == "A" && ur.RoleStatus == "A"
                                       }
                                 ).FirstOrDefault();

                if (currLogUserInfo == null)
                { return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Current user not found! Please refresh and try again.", signOutToLogIn = false }); }

                if (_oChanges.ProfileScope == "V")  //vendor admins ... SYS, SUP_ADMN, SYS_ADMN etc.
                {
                    if (currLogUserInfo.UserType == "SYS" && string.Compare(_oChanges.Username, "supadmin", true) != 0 && string.Compare(_oChanges.Username, "sys", true) == 0)
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "SYS account can ONLY manage the [sys] or [supadmin] profile. Hint: Sign in with [supadmin] or other Admin account.", signOutToLogIn = false });
                    }

                    if (currLogUserInfo.UserType == "SUP_ADMN" && string.Compare(_oChanges.Username, "sys", true) == 0)
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Current user does not have SYS role. SYS role required to manage SYS account.", signOutToLogIn = false });
                    }

                    if (currLogUserInfo.UserType != "SUP_ADMN" && currLogUserInfo.UserType != "SYS" && string.Compare(_oChanges.Username, "supadmin", true) == 0)
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Current user does not have SUP_ADMN role. SUP_ADMN role required to manage SUP_ADMN account.", signOutToLogIn = false });
                    }

                    if (_oChanges.Id == 0)
                    {
                        if (string.Compare(_oChanges.Username, "sys", true) == 0)
                        {
                            var existUserRoles = (from upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == null && c.ProfileRoleStatus == "A" && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))
                                                  from ur in _context.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SYS")
                                                  select upr
                                     );
                            if (existUserRoles.Count() > 0)
                            {
                                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "SYS account is not available. Only one (1) SYS role allowed.", signOutToLogIn = false });
                            }
                        }

                        if (string.Compare(_oChanges.Username, "supadmin", true) == 0)
                        {
                            var existUserRoles = (from upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == null && c.ProfileRoleStatus == "A" && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))
                                                  from ur in _context.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN")
                                                  select upr
                                     );
                            if (existUserRoles.Count() > 0)
                            {
                                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Super Admin profile is not available. Only one (1) SUP_ADMN role allowed.", signOutToLogIn = false });
                            }
                        }
                    }
                }

                else  //CLIENT ADMINs ... creating users for their churches /congregations
                {
                    //check availability of username... SYS /SUP_ADMN reserved
                    if (string.Compare(_oChanges.Username, "sys", true) == 0 || string.Compare(_oChanges.Username, "supadmin", true) == 0)
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Username 'supadmin' not available. Try different username.", signOutToLogIn = false });
                    }
                }


                //check that username is unique in all instances
                var existUserProfiles = _context.UserProfile.Where(c => (c.ChurchBodyId == null || c.ChurchBodyId == _oChanges.ChurchBodyId) && c.Id != _oChanges.Id && c.Username.Trim().ToLower() == _oChanges.Username.Trim().ToLower()).ToList();
                if (existUserProfiles.Count() > 0)
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Username '" + _oChanges.Username + "' not available. Try different username.", signOutToLogIn = false });
                }

                if (string.IsNullOrEmpty(_oChanges.UserDesc))  //Congregant... ChurchCodes required
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the user description or name of user.", signOutToLogIn = false });
                }

                if (_oChanges.Expr != null)  //allow historic
                {
                    if (_oChanges.UserStatus == "A" && _oChanges.Expr.Value <= DateTime.Now.Date)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please user account has expired. Activate account first.", signOutToLogIn = false });

                    if (_oChanges.Strt != null)
                        if (_oChanges.Strt.Value > _oChanges.Expr.Value)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Start date cannot be later than expiry date", signOutToLogIn = false });
                }

                //check email availability and validity
                if (_oChanges.Email != null) //_oChanges.ChurchMemberId != null && 
                {
                    var oExistUser = _context.MSTRContactInfo.Where(c => c.RefUserId != _oChanges.Id && c.Email == _oChanges.Email).FirstOrDefault();
                    if (oExistUser != null)  // ModelState.AddModelError(_oChanges.Id.ToString(), "Email of member must be unique. >> Hint: Already used by another member: "  + GetConcatMemberName(_oChanges.Title, _oChanges.FirstName, _oChanges.MiddleName, _oChanges.LastName) + "[" + oCM.ChurchBody.Name + "]");
                        return Json(new
                        {
                            taskSuccess = false,
                            oCurrId = _oChanges.Id,
                            userMess = "User email must be unique. >> Hint: Already used by another: [User: " + _oChanges.UserDesc + "]", //  GetConcatMemberName(_oChanges.ChurchMember.Title, _oChanges.ChurchMember.FirstName, _oChanges.ChurchMember.MiddleName, _oChanges.ChurchMember.LastName) + "[" + oCV.ChurchBody.Name + "]",
                            signOutToLogIn = false
                        });

                    //if (_oChanges == null)
                    //{
                    //    return Json(new
                    //    {
                    //        taskSuccess = false,
                    //        oCurrId = _oChanges.Id,
                    //        userMess = "Member status [ current state of the person - active, dormant, invalid, deceased etc. ] is required"
                    //    });
                    //}

                    ////member must be active, NOT deceased
                    //if (_oChanges_MS.ChurchMemStatusId == null)
                    //{
                    //    return Json(new
                    //    {
                    //        taskSuccess = false,
                    //        oCurrId = _oChanges.Id,
                    //        userMess = "Select the Member status [current state of the person - active, dormant, invalid, deceased etc.] as applied"
                    //    });
                    //}
                }


                _oChanges.LastMod = DateTime.Now;
                _oChanges.LastModByUserId = vmMod.oCurrUserId_Logged;
                string uniqueFileName = null;

                var oFormFile = vmMod.UserPhotoFile;
                if (oFormFile != null && oFormFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img_db");  //~/frontend/dist/img_db
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + oFormFile.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    oFormFile.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                else
                    if (_oChanges.Id != 0) uniqueFileName = _oChanges.UserPhoto;

                _oChanges.UserPhoto = uniqueFileName;

                //_oChanges.PwdSecurityQue = "What account is this?"; _oChanges.PwdSecurityAns = "Rhema-SYS";
                if (!string.IsNullOrEmpty((_oChanges.PwdSecurityQue + _oChanges.PwdSecurityAns).Trim()))
                    _oChanges.PwdSecurityAns = AppUtilties.ComputeSha256Hash(_oChanges.PwdSecurityQue + _oChanges.PwdSecurityAns);

                var tm = DateTime.Now;
                _oChanges.LastMod = tm;
                _oChanges.CreatedByUserId = vmMod.oCurrUserId_Logged;

                //validate...
                if (_oChanges.Id == 0)
                {
                    _oChanges.Pwd = "123456";  //temp pwd... to reset @ next login   
                    if (_oChanges.ProfileScope == "V")
                    {
                        var cc = "000000";    //var churchCode = "000000"; _oChanges.Username = "SysAdmin"; _oChanges.Pwd = "$ys@dmin1";                                        
                        _oChanges.Pwd = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username + _oChanges.Pwd);
                    }
                    else
                        _oChanges.Pwd = AppUtilties.ComputeSha256Hash(_oChanges.Username + _oChanges.Pwd);

                    _oChanges.Strt = tm;
                    _oChanges.Expr = tm.AddDays(90);  //default to 90 days
                    _oChanges.ResetPwdOnNextLogOn = true;
                    _oChanges.PwdExpr = tm.AddDays(30);  //default to 30 days 

                    _oChanges.Created = tm;
                    _oChanges.CreatedByUserId = vmMod.oCurrUserId_Logged;
                    _context.Add(_oChanges);

                    ViewBag.UserMsg = "Saved user profile " + (!string.IsNullOrEmpty(_oChanges.UserDesc) ? "[" + _oChanges.UserDesc + "]" : "") + " successfully. Password must be changed on next logon";
                }
                else
                {
                    //retain the pwd details... hidden fields
                    _context.Update(_oChanges);
                    ViewBag.UserMsg = "User profile updated successfully.";
                }

                //save user profile first... 
                await _context.SaveChangesAsync();

                //check if role assigned... SUP_ADMN -- auto, others -- warn!

                if (vmMod.subSetIndex == 2) // SUP_ADMN role
                {
                    var oSupAdminRole = _context.UserRole.Where(c => c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN").FirstOrDefault();
                    if (oSupAdminRole != null)
                    {
                        var existUserRoles = (from upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == null && c.UserRoleId == oSupAdminRole.Id && c.ProfileRoleStatus == "A") // && 
                                                                                                                                                                                        // ((c.Strt == null || c.Expr == null) || (c.Strt != null && c.Expr != null && c.Strt <= DateTime.Now && c.Expr >= DateTime.Now && c.Strt <= c.Expr)))
                                                                                                                                                                                        // from up in _context.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN")
                                              select upr
                                     );

                        //add SUP_ADMN role to SUP_ADMN user ... assign all privileges to the SUP_ADMN role
                        if (existUserRoles.Count() == 0)
                        {
                            //var oSupAdminRole = _context.UserRole.Where(c => c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN").FirstOrDefault();
                            //if (oSupAdminRole != null)
                            //{                             
                            var oUserRole = new UserProfileRole
                            {
                                ChurchBodyId = null,
                                UserRoleId = oSupAdminRole.Id,
                                UserProfileId = _oChanges.Id,
                                Strt = tm,
                                Expr = tm,
                                ProfileRoleStatus = "A",
                                Created = tm,
                                LastMod = tm,
                                CreatedByUserId = vmMod.oCurrUserId_Logged,
                                LastModByUserId = vmMod.oCurrUserId_Logged
                            };

                            _context.Add(oUserRole);
                            //save user role...
                            await _context.SaveChangesAsync();
                            ViewBag.UserMsg += Environment.NewLine + " ~ SUP_ADMN role added.";
                            // }

                            if (oSupAdminRole != null)
                            {
                                // assign all privileges to the SUP_ADMN role 
                                var existUserRolePerms = (from upr in _context.UserRolePermission.Where(c => c.ChurchBodyId == null && c.Status == "A" && c.UserRoleId == oSupAdminRole.Id && c.UserRole.RoleStatus == "A") // && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))     // from up in _context.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SUP_ADMN")
                                                          select upr);
                                //if (existUserRolePerms.Count() > 0)
                                //{
                                var oUserPerms = (from upr in _context.UserPermission.Where(c => c.PermStatus == "A")                                                                                                                                                      // from up in _context.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SUP_ADMN")
                                                  select upr);

                                if (oUserPerms.Count() > 0) //(existUserRolePerms.Count() < oUserPerms.Count())
                                {
                                    var rowUpdated = false; var rowsUpdated = 0; var rowsAdded = 0;
                                    foreach (var oURP in oUserPerms)
                                    {
                                        var existUserRolePerm = existUserRolePerms.Where(c => c.UserPermissionId == oURP.Id).FirstOrDefault();
                                        if (existUserRolePerm == null)
                                        {
                                            var oUserRolePerm = new UserRolePermission
                                            {
                                                ChurchBodyId = null,
                                                UserRoleId = oSupAdminRole.Id,
                                                UserPermissionId = oURP.Id,
                                                ViewPerm = true,
                                                CreatePerm = true,
                                                EditPerm = true,
                                                DeletePerm = true,
                                                ManagePerm = true,
                                                Status = "A",
                                                Created = tm,
                                                LastMod = tm,
                                                CreatedByUserId = vmMod.oCurrUserId_Logged,
                                                LastModByUserId = vmMod.oCurrUserId_Logged
                                            };

                                            _context.Add(oUserRolePerm);
                                            rowsAdded++;
                                        }
                                        else
                                        {
                                            rowUpdated = false;
                                            if (!existUserRolePerm.ViewPerm) { existUserRolePerm.ViewPerm = true; rowUpdated = true; }
                                            if (!existUserRolePerm.CreatePerm) { existUserRolePerm.CreatePerm = true; rowUpdated = true; }
                                            if (!existUserRolePerm.EditPerm) { existUserRolePerm.EditPerm = true; rowUpdated = true; }
                                            if (!existUserRolePerm.DeletePerm) { existUserRolePerm.DeletePerm = true; rowUpdated = true; }
                                            if (!existUserRolePerm.ManagePerm) { existUserRolePerm.ManagePerm = true; rowUpdated = true; }

                                            if (rowUpdated)
                                            {
                                                existUserRolePerm.Created = tm;
                                                existUserRolePerm.LastMod = tm;
                                                existUserRolePerm.CreatedByUserId = vmMod.oCurrUserId_Logged;
                                                existUserRolePerm.LastModByUserId = vmMod.oCurrUserId_Logged;

                                                _context.Add(existUserRolePerm);
                                                rowsUpdated++;
                                            }
                                        }
                                    }

                                    // prompt users
                                    if (rowsAdded > 0) ViewBag.UserMsg += Environment.NewLine + " ~ " + rowsAdded + " user permissions added.";
                                    if (rowsUpdated > 0) ViewBag.UserMsg += ". " + rowsUpdated + " user permissions updated.";
                                }
                                //}
                            }
                        }
                    }


                }

                // oCM_NewConvert.Created = DateTime.Now;
                // _context.Add(_oChanges);


                //save changes... 
                await _context.SaveChangesAsync();


                //user roles / user groups and/or user permissions [ tick ... pick from the attendance concept]
                //var oMemRoles = currCtx.MemberChurchRole.Include(t => t.LeaderRole).ThenInclude(t => t.LeaderRoleCategory)
                //    .Where(c => c.LeaderRole.ChurchBodyId == oChuTransf.FromChurchBodyId && c.ChurchMemberId == oCurrChuMember.Id && c.IsCurrServing == true).ToList();
                //foreach (var oMR in oMemRoles)
                //{
                //    oMR.IsCurrServing = false;
                //    oMR.Completed = oChuTransf.TransferDate;
                //    oMR.CompletionReason = oChuTransf.TransferType;
                //    oMR.LastMod = DateTime.Now;
                //    //
                //    currCtx.Update(oMR);
                //}
                //ViewBag.UserMsg += " Church visitor added to church as New Convert successfully. Update of member details may however be required."  


                //save everything
                // await _context.SaveChangesAsync();

                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();


                //if (_oChanges.PwdExpr != null)
                //{
                //    if (_oChanges.PwdExpr.Value >= DateTime.Now.Date)
                //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please user password has expired. Check email/phone for confirm code to activate password.", signOutToLogIn = true  });
                //}

                // return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = false });
                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = false });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving user profile details. Err: " + ex.Message, signOutToLogIn = false });
            }
        }

        public async Task<IActionResult> Delete_UPR(int? id)
        {
            var res = false;
            var UserProfile = await _context.UserProfile.FindAsync(id);
            if (UserProfile != null)
            {
                //check all member related modules for references to deny deletion

                res = true;
                if (res)
                {
                    _context.UserProfile.Remove(UserProfile);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError(UserProfile.Username, "Delete failed. User Profile data is referenced elsewhere in the Application.");
                    ViewBag.UserDelMsg = "Delete failed. User Profile data is referenced elsewhere in the Application.";
                }

                return Json(res);
            }

            else
                return Json(false);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit_SYS(UserProfileVM vmMod, string churchCode)
        {
            try
            {
                //UserProfile _oChanges = vmMod.oUserProfile;
                // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileVM : vmMod; TempData.Keep();

                var _churchCode = churchCode;

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _churchCode, userMess = "Failed to create requested user. Please refresh and try again.", signOutToLogIn = false });


                //var tm = DateTime.Now;
                //_oChanges.LastMod = tm;
                //_oChanges.LastModByUserId = vmMod.oCurrLoggedUserId;

                if (!string.IsNullOrEmpty(_churchCode))
                {
                    var userProList = (from t_upx in _context.UserProfile.Where(c => _churchCode == "000000" && c.ChurchBodyId == null && c.ProfileScope == "V" && c.UserStatus == "A")
                                       from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == null && c.UserProfileId == t_upx.Id && c.ProfileRoleStatus == "A").DefaultIfEmpty()
                                       from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == null && c.Id == t_upr.UserRoleId && c.RoleStatus == "A" && c.RoleLevel == 0 && c.RoleType == "SYS").DefaultIfEmpty()
                                       select t_upx
                                 ).OrderBy(c => c.UserDesc).ToList();

                    if (userProList.Count > 0)
                        return Json(new { taskSuccess = false, oCurrId = _churchCode, userMess = "SYS account profile already created. There could only be one SYS account.", signOutToLogIn = false });

                }


                //create user and init...
                var _oChanges = new UserProfile();

                //_oChanges.AppGlobalOwnerId = null; // oCV.ChurchBody != null ? oCV.ChurchBody.AppGlobalOwnerId : null;
                //_oChanges.ChurchBodyId = null; //(int)oCV.ChurchBody.Id;
                //_oChanges.OwnerId =null; // (int)vmMod.oCurrLoggedUserId;

                var tm = DateTime.Now;
                _oChanges.Strt = tm;
                // ChurchBody == null

                //_oChanges.Expr = null; // tm.AddDays(90);  //default to 30 days
                //  oCurrVmMod.oUserProfile.UserId = oCurrChuMemberId_LogOn;
                //_oChanges.ChurchMemberId = null; // vmMod.oCurrLoggedMemberId;

                _oChanges.UserScope = "E"; // I-internal, E-external
                _oChanges.ProfileScope = "V"; // V-Vendor, C-Client
                _oChanges.ResetPwdOnNextLogOn = true;
                _oChanges.PwdSecurityQue = "What account is this?";
                _oChanges.PwdSecurityAns = "Rhema-SYS";
                _oChanges.Email = "samuel@rhema-systems.com";
                // _oChanges.PhoneNum = "233242188212";
                _oChanges.UserDesc = "Sys Profile";

                var cc = "000000"; _oChanges.Username = "Sys"; _oChanges.Pwd = "654321"; // [ get the raw data instead ]
                _oChanges.Pwd = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username + _oChanges.Pwd);

                _oChanges.PwdExpr = tm.AddDays(30);  //default to 90 days 
                _oChanges.UserStatus = "A"; // A-ctive...D-eactive

                _oChanges.Created = tm;
                _oChanges.LastMod = tm;
                _oChanges.CreatedByUserId = null; // (int)vmMod.oCurrLoggedUserId;
                _oChanges.LastModByUserId = null; // (int)vmMod.oCurrLoggedUserId;

                //_oChanges.UserPhoto = null;
                //_oChanges.UserId = null;
                //_oChanges.PhoneNum = null;
                //_oChanges.Email = null; 

                // 
                ViewBag.UserMsg = "Saved SYS account profile successfully. Sign-out and then sign-in to create Super Admin profile to perform the required settings /client configurations.";

                _context.Add(_oChanges);
                //save everything
                _context.SaveChanges();

                // TempData["oVmCurrMod"] = vmMod;
                // TempData.Keep();
                // return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = true });


                //succesful...  login required
                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = true });
                // return RedirectToAction("LoginUserAcc", "UserLogin");
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = churchCode, userMess = "Failed saving SYS account profile. Err: " + ex.Message, signOutToLogIn = false });
            }

        }




        // UP
        public ActionResult Index_UP(int setIndex, int pageIndex= 1) //, string proScope, string subScope = "")  // , string subScope="" int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int setIndex = 0, int subSetIndex = 0) //, int? oParentId = null, int? id = null, int pageIndex = 1)             
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
                if (!this.userAuthorized) return View(new UserProfileModel()); //retain view    
                if (oUserLogIn_Priv[0] == null) return View(new UserProfileModel());
                if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner != null || oUserLogIn_Priv[0].ChurchBody != null) return View(new UserProfileModel());
                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                var oLoggedRole = oUserLogIn_Priv[0].UserRole;

                //

                var oUPModel = new UserProfileModel(); //TempData.Keep();   // int? oAppGloOwnId = null;
                var oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody;
                //
                int? oAppGloOwnId_Logged = null;
                int? oChuBodyId_Logged = null;
                if (oChuBody_Logged != null)
                {
                    oAppGloOwnId_Logged = oChuBody_Logged.AppGlobalOwnerId;
                    oChuBodyId_Logged = oChuBody_Logged.Id;
                }

                var oUserId_Logged = oLoggedUser.Id;

                // 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
                var proScope = setIndex == 1 ? "V" : "C";
                var subScope = setIndex == 2 ? "D" : setIndex == 3 ? "A" : "";

                //var _subSetIndex = 0;
                //switch (oLoggedRole.RoleName.ToUpper())
                //{
                //    case "SYS": _subSetIndex = 1; break;
                //    case "SUP_ADMN": _subSetIndex = 2; break;
                //    case "SYS_ADMN": _subSetIndex = 3; break;
                //    case "SYS_CUST": _subSetIndex = 4; break;
                //    // case "SYS_???": _subSetIndex = 5; break;
                //    case "CH_ADMN": _subSetIndex = 6; break;
                //    case "CF_ADMN": _subSetIndex = 11; break;
                //}

                // var _strCurrTask = ""; var _proScope = ""; var _subScope = "";

                if (proScope == "V" && setIndex == 1) // && subSetIndex >= 1 && subSetIndex <= 5)  // subSetIndex >= 1 && subSetIndex <= 5)  // sys=1   sup_admin=2  sys_admin=3 sys_cust=4
                {   //_proScope = "V"; _subScope = "";
                    oUPModel.strCurrTask = "System Admin Profile";
                    oUPModel.lsUserProfileModels = GetUserProfileList_SysAdmin();
                }

                else if (proScope == "C" && (setIndex == 2 || setIndex == 3)) // ((subScope == "D" && setIndex == 2) || (subScope == "A" && setIndex == 3))) //  (proScope == "C" && ((subScope == "D" && (subSetIndex == 6 || subSetIndex == 11)) || (subScope == "A" && (subSetIndex >= 6 || subSetIndex <= 15))))  // && subScope == "D" ... D - aDmin users, A-All users, subSetIndex == 6 || subSetIndex == 11)  // CH_admin=6, CH_rgstr=7, CH_acct=8, CH_cust=9... CH_???=10
                {  //_proScope = "C"; _subScope = "D";   // 
                    oUPModel.strCurrTask = subScope == "D" ? "Church Admin Profile" : "Church User Profile";
                    oUPModel.lsUserProfileModels = GetUserProfileList_ChuAdmin(subScope);
                }

                //else if (proScope == "C" )  // subSetIndex >= 6 && subSetIndex <= 15)  // CF_admin=11, CF_rgstr=12, CF_acct=13, CF_cust=14... CF_???=15
                //{ // _proScope = "C"; _subScope = "A";
                //    oUPModel.strCurrTask = subScope == "A" ? "Church User Profiles" : "User Profiles"; 
                //    oUPModel.lsUserProfileModels = GetUserProfileList_ChuAdmin(subScope);
                //}

                //else if (proScope == "C" && subScope == "A")  // subSetIndex >= 6 && subSetIndex <= 15)  // CF_admin=11, CF_rgstr=12, CF_acct=13, CF_cust=14... CF_???=15
                //{ // _proScope = "C"; _subScope = "A";
                //    oUPModel.strCurrTask = "User Profiles";
                //    oUPModel.lsUserProfileModels = GetUserProfileList_ClientChuAdmin(oAppGloOwnId, oChurchBodyId);
                //}

                //if (subSetIndex >= 1 && subSetIndex <= 15)
                //{
                //    oUPModel.lsUserProfileModels = GetUserProfileList(oAppGloOwnId, oChurchBodyId, _proScope, _subScope);
                //    oUPModel.strCurrTask = _strCurrTask;
                //}

                else if (setIndex == 6) //enlist all roles
                {
                    oUPModel.lsUserRoles = GetUserRoles(); // (null, null, "V", ""); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                    oUPModel.strCurrTask = "System Role";
                }

                else if (setIndex == 7) //enlist all privileges
                {
                    oUPModel.lsUserPermissions = GetUserPermissions(); // (null, null, "V", ""); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                    oUPModel.strCurrTask = "System Privilege";
                }

                else if (setIndex == 8) //enlist all privileges
                {
                    oUPModel.lsUserAuditTrails = GetUserAuditTasks(); // (null, null, "V", ""); // -- GET ALL VENDOR ADMINS...  (null, "V", "", -1);  // Vendor Admin may need a seperate API for cross databases :: since every clients db differ by DBNAME
                    oUPModel.strCurrTask = "UserAudit Trail";
                }

                // oUPModel.strCurrTask = "Denominations (Churches)";

                //                
                //oUPModel.oAppGloOwnId = oAppGloOwnId;
                //oUPModel.oChurchBodyId = oCurrChuBodyId;
                //
                oUPModel.oUserId_Logged = oUserId_Logged;
                oUPModel.oChurchBody_Logged = oChuBody_Logged;
                oUPModel.oAppGloOwnId_Logged = oAppGloOwnId_Logged;
                oUPModel.oUserProfileLevel_Logged = oLoggedUser.ProfileLevel;   // sync this with the User_roles ... roles can be multiple

                // oUPModel.oMemberId_Logged = oCurrChuMemberId_LogOn;
                //

                oUPModel.setIndex = setIndex;
               // oUPModel.subSetIndex = subSetIndex;   // unknown... yet
                oUPModel.pageIndex = pageIndex; // 1;
                oUPModel.profileScope = proScope;
                oUPModel.subScope = subScope;

                // oUPModel.pageIndex = pageIndex;

                ///
                ViewData["strAppName"] = "RhemaCMS";
                ViewData["strAppNameMod"] = "Admin Palette";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(oLoggedUser.UserDesc) ? oLoggedUser.UserDesc : "[Current user]";
                ///
                ViewData["oAppGloOwnId_Logged"] = oAppGloOwnId_Logged;
                ViewData["oChuBodyId_Logged"] = oChuBodyId_Logged;
                ViewData["strAppCurrUser_ChRole"] = oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUser_PhotoFilename"] = oLoggedUser.UserPhoto;
                ///


                //refresh Dash values
                _ = LoadDashboardValues();



                var _userTask = "Viewed " + oUPModel.strCurrTask.ToLower() + " list";
                var tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                 "RCMS-Admin: " + oUPModel.strCurrTask, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id));

                return View(oUPModel);
            }
        }
         

        [HttpGet]
        public IActionResult AddOrEdit_UP1(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int id = 0, int? oParentId = null, int setIndex = 0, int subSetIndex = 0,
                                                int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {
                var oCurrChuBodyLogOn_Logged = oUserLogIn_Priv[0].ChurchBody;
                var oUserProfile_Logged = oUserLogIn_Priv[0].UserProfile;
                // int? oAppGloOwnId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : (int?)null;
                //int? oChurchBodyId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : (int?)null;
                // int? oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : (int?)null;
                oUserId_Logged = oUserProfile_Logged != null ? oUserProfile_Logged.Id : oUserId_Logged;
                oCBId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.Id : oCBId_Logged;
                oAGOId_Logged = oCurrChuBodyLogOn_Logged != null ? oCurrChuBodyLogOn_Logged.AppGlobalOwnerId : oAGOId_Logged;

                if (StackAppUtilties.IsAjaxRequest(HttpContext.Request))
                {
                    var oUP_MDL = new ChurchFaithTypeModel();
                    if (id == 0)
                    {
                        //create user and init... 
                        oUP_MDL.oChurchFaithType = new ChurchFaithType();
                        oUP_MDL.oChurchFaithType.Level = subSetIndex;  //subSetIndex == 2 ? 1 : 2; // 1;
                        oUP_MDL.oChurchFaithType.Category = subSetIndex == 1 ? "FS" : "FC";

                        //if (setIndex > 0) oUP_MDL.oChurchFaithType.Category = setIndex == 1 ? "FS" : "FC";
                    }

                    else
                    {
                        oUP_MDL = (
                             from t_cft in _context.ChurchFaithType.AsNoTracking() //.Include(t => t.FaithTypeClass)
                                 .Where(x => x.Id == id)
                             from t_cft_c in _context.ChurchFaithType.AsNoTracking().Where(c => c.Id == t_cft.FaithTypeClassId).DefaultIfEmpty()
                             select new ChurchFaithTypeModel()
                             {
                                 oChurchFaithType = t_cft,
                                 strFaithTypeClass = t_cft_c != null ? t_cft_c.FaithDescription : ""
                             })
                             .FirstOrDefault();
                    }

                    if (oUP_MDL.oChurchFaithType == null) return null;

                    oUP_MDL.setIndex = setIndex;
                    oUP_MDL.subSetIndex = subSetIndex;
                    oUP_MDL.oUserId_Logged = oUserId_Logged;
                    oUP_MDL.oAppGloOwnId_Logged = oAGOId_Logged;
                    oUP_MDL.oChurchBodyId_Logged = oCBId_Logged;
                    //
                    oUP_MDL.oAppGloOwnId = oAppGloOwnId;
                    oUP_MDL.oChurchBodyId = oCurrChuBodyId;
                    var oCurrChuBody = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();
                    oUP_MDL.oChurchBody = oCurrChuBody != null ? oCurrChuBody : null;

                    if (oUP_MDL.subSetIndex == 2) // Denomination classes av church sects
                        oUP_MDL = this.popLookups_CFT(oUP_MDL, oUP_MDL.oChurchFaithType);

                    var _oUP_MDL = Newtonsoft.Json.JsonConvert.SerializeObject(oUP_MDL);
                    TempData["oVmCurrMod"] = _oUP_MDL; TempData.Keep();

                    //TempData["oVmCurr"] = oUP_MDL;
                    //TempData.Keep(); 

                    return PartialView("_AddOrEdit_UP", oUP_MDL);
                }

                //page not found error
                Response.StatusCode = 500;
                return PartialView("ErrorPage");
            }
        }

        public ChurchFaithTypeModel popLookups_UP1(ChurchFaithTypeModel vm, ChurchFaithType oCurrUP)
        {
            if (vm != null)
            {
                vm.lkpFaithTypeClasses = _context.ChurchFaithType.Where(c => c.Id != oCurrUP.Id && c.Category == "FS" && !string.IsNullOrEmpty(c.FaithDescription))
                                              .OrderBy(c => c.FaithDescription).ToList()
                                              .Select(c => new SelectListItem()
                                              {
                                                  Value = c.Id.ToString(),
                                                  Text = c.FaithDescription
                                              })
                                              .ToList();

                vm.lkpFaithTypeClasses.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }

            return vm;
        }


        [HttpGet]
        public IActionResult AddOrEdit_UP(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int subSetIndex = 0,
                                                                           int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) //, int pageIndex = 1)
        {
            //// check permission 
            //var _oUserPrivilegeCol = oUserLogIn_Priv;
            //var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
            //TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();
            //
            var oUserModel = new UserProfileModel(); //TempData.Keep();
                                                     //  if (setIndex == 0) return oUserModel;

            //subSetIndex: 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
            // setIndex: 1-Vendor-any , 2-Client - CH/CF, 3- Client - any/all
            var proScope = setIndex == 1 ? "V" : "C";               // V - vendor, C - Client
            var subScope = setIndex == 2 ? "D" : setIndex == 3 ? "A" : "";      // D - denomination, A - all

            //if (subSetIndex >= 1 && subSetIndex <= 5) { proScope = "V"; subScope = ""; }
            //else if (subSetIndex == 6 || subSetIndex == 11) { proScope = "C"; subScope = "D"; }
            //else if (subSetIndex >= 6 && subSetIndex <= 15) { proScope = "C"; subScope = "A"; }

            var strDesc = "User Profile";
            var _userTask = "Attempted accessing/modifying " + strDesc.ToLower() ;
            if (id == 0)
            {   //create user and init... 
                //var existSUP_ADMNs = (
                //   from t_up in _context.UserProfile.AsNoTracking() //.Include(t => t.ChurchMember)
                //                .Where(c => c.Id == id &&
                //                (c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileScope == "V") ||
                //                (c.AppGlobalOwnerId == oDenomId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "C"))
                //   from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId && c.AppGlobalOwnerId == t_up.AppGlobalOwnerId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                //   from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                //                    .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id &&
                //                    ((proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5)) ||
                //                     ((proScope == "C" && subScope == "D" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                //                     ((proScope == "C" && subScope == "A" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && (c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15))
                //                    )).DefaultIfEmpty()
                //   select t_up
                //   ).ToList();

                ////supadmin <creation> task.... but must have logged in as SYS
                //if (setIndex==1 && subSetIndex==2 && existSUP_ADMNs.Count > 0)
                //{ //prompt user sup_admin == 1 only
                //    oUserModel.oUserProfile = null;
                //    return oUserModel;
                //}

                var oUser = new UserProfile();
                oUser.AppGlobalOwnerId = oAppGloOwnId;
                oUser.ChurchBodyId = oCurrChuBodyId;
                oUser.Strt = DateTime.Now;
                oUser.ResetPwdOnNextLogOn = true;

                //oUP_MDL.oUserProfile.CountryId = oCurrCtryId;

                oUser.UserStatus = "A";   // A-ctive...D-eactive   
                oUser.ProfileScope = proScope;                 
                oUser.ProfileLevel = subSetIndex;  //remember to change the profile level when the ROLE type is changed                

                if (oUser.ProfileLevel >= 1 && oUser.ProfileLevel <= 5) // 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
                {
                    oUser.UserScope = "E";  // I-internal, E-external
                    if (oUser.ProfileLevel == 2) { oUser.Username = "supadmin"; oUser.UserDesc = "Super Admin"; }
                }
                else // I-internal, E-external [manually config]
                {
                    if (oAppGloOwnId == null || oCurrChuBodyId == null)
                    { Response.StatusCode = 500; return PartialView("ErrorPage"); }

                    //var oAGO = _context.MSTRAppGlobalOwner.Find(oAppGloOwnId);
                    //var oCB = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();

                    //if (oAppGloOwnId == null || oCurrChuBodyId == null)
                    //{ Response.StatusCode = 500; return PartialView("ErrorPage"); }

                    oUser.UserScope = "I";
                    oUserModel.oUserProfile.numCLIndex = _context.MSTRChurchLevel.Count(c => c.AppGlobalOwnerId == oAppGloOwnId);  //use what's configured... not digit at AGO
                }

                _userTask = "Attempted creating new " + strDesc.ToLower(); // + ", " + oUserModel.oUserProfile.UserDesc;  
                oUserModel.oUserProfile = oUser; 
            }

            else
            {
                var oUser = (
                   from t_up in _context.UserProfile.AsNoTracking().Include(t => t.ContactInfo) //.Include(t => t.ChurchBody) //.Include(t => t.ChurchMember)
                                .Where(c => c.Id == id &&
                                (c.AppGlobalOwnerId == null && c.ChurchBodyId == null) ||  //&& c.ProfileScope == "V"   
                                (oAppGloOwnId != null && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "C"))
                   from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId && c.AppGlobalOwnerId == t_up.AppGlobalOwnerId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                   from t_cl in _context.MSTRChurchLevel.AsNoTracking().Where(c => c.AppGlobalOwnerId == t_up.AppGlobalOwnerId && c.Id == t_cb.ChurchLevelId).DefaultIfEmpty()
                   from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                                 .Where(c => c.AppGlobalOwnerId == t_up.AppGlobalOwnerId && c.ChurchBodyId == t_up.ChurchBodyId && c.UserProfileId == t_up.Id &&
                                     (proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5) ||
                                     (proScope == "C" && subScope == "D" && (c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                                     (proScope == "C" && subScope == "A" && (c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15)
                                  ).DefaultIfEmpty()

                       //from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole).Where(c => c.UserProfileId == t_up.Id).DefaultIfEmpty()
                       // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == t_up.ChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()
                       // from t_ur in _context.UserRole.AsNoTracking().Where(c => c.Id == t_upr.UserRoleId &&
                       //              ((c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5)) ||
                       //              ((c.RoleType == "CH_ADMN" || c.RoleType == "CF_ADMN") && (c.RoleLevel >= 6 && c.RoleLevel <= 10))).DefaultIfEmpty()
                       // from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                       //              .Where(c => c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                       // from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                       //              .Where(c => c.UserProfileId == t_up.Id).DefaultIfEmpty()
                       // from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                       //              .Where(c => c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

                   select new UserProfile()
                   {
                       Id = t_up.Id,
                       AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                       ChurchBodyId = t_up.ChurchBodyId,
                       ChurchMemberId = t_up.ChurchMemberId,
                       ChurchBody = t_cb, // t_up.ChurchBody,
                       oUserRole = t_upr.UserRole,
                       strChurchCode_AGO = t_cb != null ? (t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.GlobalChurchCode : "") : "",
                       strChurchCode_CB = t_cb != null ? t_cb.GlobalChurchCode : "",
                       // 
                       numCLIndex = t_cl.LevelIndex,
                       oCBChurchLevelId = t_cb.ChurchLevelId,
                       //  ChurchMember = t_up.ChurchMember,
                       OwnerUser = t_up.OwnerUser,
                       //
                       Username = t_up.Username,
                       UserDesc = t_up.UserDesc, 
                       Email = t_up.Email,
                       ContactInfo = t_up.ContactInfo,
                       // PhoneNum = t_up.PhoneNum,
                       Pwd = t_up.Pwd,
                       PwdExpr = t_up.PwdExpr,
                       PwdSecurityQue = t_up.PwdSecurityQue,
                       PwdSecurityAns = t_up.PwdSecurityAns,
                       ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                       Strt = t_up.Strt,
                       strStrt = t_up.strStrt,
                       Expr = t_up.Expr,
                       strExpr = t_up.strExpr != null ? DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                       //
                       OwnerUserId = t_up.OwnerUserId,
                      // UserId = t_up.UserId,
                       UserScope = t_up.UserScope,
                       ProfileLevel = t_up.ProfileLevel,
                       UserPhoto = t_up.UserPhoto,
                       ProfileScope = t_up.ProfileScope,
                       Created = t_up.Created,
                       CreatedByUserId = t_up.CreatedByUserId,
                       LastMod = t_up.LastMod,
                       LastModByUserId = t_up.LastModByUserId,
                       UserStatus = t_up.UserStatus,
                       strUserStatus = GetStatusDesc(t_up.UserStatus)

                   }
                   ).FirstOrDefault();

                oUserModel.oUserProfile = oUser;
                if (oUser == null)
                {
                    Response.StatusCode = 500;
                    return PartialView("ErrorPage");
                }

                    oUserModel.strUserProfile = oUser.UserDesc;
                    subSetIndex = oUser.ProfileLevel != null ? (int)oUser.ProfileLevel : 0;
                    oUserModel.strChurchBody = oUser.ChurchBody != null ? oUser.ChurchBody.Name : "";
                    oUserModel.strAppGloOwn = oUser.AppGlobalOwner != null ? oUser.AppGlobalOwner.OwnerName : "";

                    //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                    // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc
                    _userTask = "Opened " + strDesc.ToLower() + ", " + oUserModel.oUserProfile.UserDesc;               
            }
                         
            oUserModel.setIndex = setIndex;
            oUserModel.subSetIndex = subSetIndex;
            oUserModel.profileScope = proScope;
            oUserModel.subScope = subScope;
            //
            oUserModel.oCurrUserId_Logged = oUserId_Logged;
            oUserModel.oAppGloOwnId_Logged = oAGOId_Logged;
            oUserModel.oChurchBodyId_Logged = oCBId_Logged;
            //
            oUserModel.oAppGloOwnId = oAppGloOwnId;
            oUserModel.oChurchBodyId = oCurrChuBodyId;

            //  oUserModel.oCurrUserId_Logged = oCurrUserId_Logged; 

            // ChurchBody oCB = null;
            // if (oCurrChuBodyId != null)  oCB = _context.MSTRChurchBody.Where(c=>c.Id == oCurrChuBodyId && c.AppGlobalOwnerId==oDenomId).FirstOrDefault();

            //if (setIndex == 1) // (subSetIndex >= 1 && subSetIndex <= 5) // no SUP_ADMN or SYS as option
            //    oUserModel = this.populateLookups_UP_MS(oUserModel, null, null, setIndex);

            //else if (setIndex == 2 || setIndex == 3) // ((oUserModel.profileScope == "V" && (subSetIndex == 6 || subSetIndex == 11)) || (oUserModel.profileScope == "C" && subSetIndex >= 6 && subSetIndex <= 15))
            //{
            //    // var oCB = _context.MSTRChurchBody.Where(c => c.Id == oCurrChuBodyId && c.AppGlobalOwnerId == oDenomId).FirstOrDefault();
            //    oUserModel = this.populateLookups_UP_CL(oUserModel, oCurrChuBodyId);
            //}


            if (setIndex == 2 && (oUserModel.oUserProfile.ProfileLevel == 6 || oUserModel.oUserProfile.ProfileLevel == 11)) //CH admin, CF admin (oUserModel.oChurchBody != null)
            {
                if (oUserModel.oAppGloOwnId != null)
                {
                    var oUserCB = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oUserModel.oAppGloOwnId && c.Id == oUserModel.oChurchBodyId).FirstOrDefault();
                    ///
                    oUserModel.oCBLevelCount = oUserModel.oUserProfile.numCLIndex; // - 1;        // oCBLevelCount -= 2;   
                    List<MSTRChurchLevel> oCBLevelList = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oUserModel.oAppGloOwnId && c.LevelIndex > 0 &&
                                                                                                c.LevelIndex <= oUserModel.oUserProfile.numCLIndex).ToList().OrderBy(c => c.LevelIndex).ToList();
                    ///
                    if (oUserModel.oCBLevelCount > 0 && oCBLevelList.Count > 0)
                    {
                        oUserModel.strChurchLevel_1 = !string.IsNullOrEmpty(oCBLevelList[0].CustomName) ? oCBLevelList[0].CustomName : oCBLevelList[0].Name;
                        ViewData["strChurchLevel_1"] = oUserModel.strChurchLevel_1;
                        ///

                        if (oUserModel.oCBLevelCount == 1 && oUserCB.ChurchLevelId == oCBLevelList[0].Id)  // at the last index [no parent]
                        { oUserModel.ChurchBodyId_1 = oUserCB.Id; oUserModel.strChurchBody_1 = oUserCB.Name + " [Church Root]"; }
                        else
                        {
                            var oCB_1 = _context.MSTRChurchBody.Include(t => t.ChurchLevel)
                                          .Where(c => c.AppGlobalOwnerId == oUserModel.oAppGloOwnId && // c.Status == "A" && 
                                                c.ChurchLevel.LevelIndex == 1 && c.OrganisationType == "CR") //c.ChurchLevelId == oCBLevelList[0].Id &&
                                          .FirstOrDefault();

                            if (oCB_1 != null)
                            { oUserModel.ChurchBodyId_1 = oCB_1.Id; oUserModel.strChurchBody_1 = oCB_1.Name + " [Church Root]"; }
                        }
                          
                        ViewData["ChurchBodyId_1"] = oUserModel.ChurchBodyId_1;
                        ViewData["strChurchBody_1"] = oUserModel.strChurchBody_1;

                        ///
                        if (oUserModel.oCBLevelCount > 1)
                        {
                            oUserModel.strChurchLevel_2 = !string.IsNullOrEmpty(oCBLevelList[1].CustomName) ? oCBLevelList[1].CustomName : oCBLevelList[1].Name;
                            ViewData["strChurchLevel_2"] = oUserModel.strChurchLevel_2;
                            ///                            
                            if (oUserCB != null)
                            {
                                if (oUserModel.oCBLevelCount == 2 && oUserCB.ChurchLevelId == oCBLevelList[1].Id)  // at the last index [no parent]
                                { oUserModel.ChurchBodyId_2 = oUserCB.Id; }
                                else
                                {
                                    var lsCB_2 = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oUserModel.oAppGloOwnId && c.ChurchLevelId == oCBLevelList[1].Id).ToList();
                                    var oCB_2 = lsCB_2.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oUserCB.RootChurchCode, c.Id, oUserCB.ParentChurchBodyId)).ToList();
                                    if (oCB_2.Count() > 0)
                                    { oUserModel.ChurchBodyId_2 = oCB_2[0].Id; }
                                } 
                                ViewData["ChurchBodyId_2"] = oUserModel.ChurchBodyId_2;
                            }
                            else
                            {
                                oUserModel.ChurchBodyId_2 = null;
                                ViewData["ChurchBodyId_2"] = 0;
                            }
                             

                            if (oUserModel.oCBLevelCount > 2)
                            {
                                oUserModel.strChurchLevel_3 = !string.IsNullOrEmpty(oCBLevelList[2].CustomName) ? oCBLevelList[2].CustomName : oCBLevelList[2].Name;
                                ViewData["strChurchLevel_3"] = oUserModel.strChurchLevel_3;
                                 
                                if (oUserCB != null)
                                {
                                    if (oUserModel.oCBLevelCount == 3 && oUserCB.ChurchLevelId == oCBLevelList[2].Id)  // at the last index [no parent]
                                    { oUserModel.ChurchBodyId_3 = oUserCB.Id; }
                                    else
                                    {
                                        var lsCB_3 = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oUserModel.oAppGloOwnId && c.ChurchLevelId == oCBLevelList[2].Id).ToList();
                                        var oCB_3 = lsCB_3.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oUserCB.RootChurchCode, c.Id, oUserCB.ParentChurchBodyId)).ToList();
                                        if (oCB_3.Count() > 0)
                                        { oUserModel.ChurchBodyId_3 = oCB_3[0].Id; }
                                    }                                    
                                    ViewData["ChurchBodyId_3"] = oUserModel.ChurchBodyId_3;
                                }
                                else
                                {
                                    oUserModel.ChurchBodyId_3 = null;
                                    ViewData["ChurchBodyId_3"] = 0;
                                }

                                if (oUserModel.oCBLevelCount > 3)
                                {
                                    oUserModel.strChurchLevel_4 = !string.IsNullOrEmpty(oCBLevelList[3].CustomName) ? oCBLevelList[3].CustomName : oCBLevelList[3].Name;
                                    ViewData["strChurchLevel_4"] = oUserModel.strChurchLevel_4;
                                     
                                    if (oUserCB != null)
                                    {
                                        if (oUserModel.oCBLevelCount == 4 && oUserCB.ChurchLevelId == oCBLevelList[3].Id)  // at the last index [no parent]
                                        { oUserModel.ChurchBodyId_4 = oUserCB.Id; }
                                        else
                                        {
                                            var lsCB_4 = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oUserModel.oAppGloOwnId && c.ChurchLevelId == oCBLevelList[3].Id).ToList();
                                            var oCB_4 = lsCB_4.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oUserCB.RootChurchCode, c.Id, oUserCB.ParentChurchBodyId)).ToList();
                                            if (oCB_4.Count() > 0)
                                            { oUserModel.ChurchBodyId_4 = oCB_4[0].Id; } 
                                        }                                        
                                        ViewData["ChurchBodyId_4"] = oUserModel.ChurchBodyId_4;
                                    }
                                    else
                                    {
                                        oUserModel.ChurchBodyId_4 = null;
                                        ViewData["ChurchBodyId_4"] = 0;
                                    }

                                    if (oUserModel.oCBLevelCount > 4)
                                    {
                                        oUserModel.strChurchLevel_5 = !string.IsNullOrEmpty(oCBLevelList[4].CustomName) ? oCBLevelList[4].CustomName : oCBLevelList[4].Name;
                                        ViewData["strChurchLevel_5"] = oUserModel.strChurchLevel_5;
                                         
                                        if (oUserCB != null)
                                        {
                                            if (oUserModel.oCBLevelCount == 5 && oUserCB.ChurchLevelId == oCBLevelList[4].Id)  // at the last index [no parent]
                                            { oUserModel.ChurchBodyId_5 = oUserCB.Id; }
                                            else
                                            {
                                                var lsCB_5 = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oUserModel.oAppGloOwnId && c.ChurchLevelId == oCBLevelList[4].Id).ToList();
                                                var oCB_5 = lsCB_5.Where(c => IsAncestor_ChurchBody(c.RootChurchCode, oUserCB.RootChurchCode, c.Id, oUserCB.ParentChurchBodyId)).ToList();
                                                if (oCB_5.Count() > 0)
                                                { oUserModel.ChurchBodyId_5 = oCB_5[0].Id; }
                                            } 
                                            ViewData["ChurchBodyId_5"] = oUserModel.ChurchBodyId_5;
                                        }
                                        else
                                        {
                                            oUserModel.ChurchBodyId_5 = null;
                                            ViewData["ChurchBodyId_5"] = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                } 
            }

              
            oUserModel = this.populateLookups_UP_MS(oUserModel, setIndex, oUserModel.oUserProfile.ChurchBody);

            //oUP_MDL.lkpStatuses = new List<SelectListItem>();
            //foreach (var dl in dlGenStatuses) { oUP_MDL.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            //TempData["oVmCurr"] = oUserModel;
            //TempData.Keep();

            // var _oUserModel = Newtonsoft.Json.JsonConvert.SerializeObject(oUserModel);
            // TempData["oVmCurr"] = _oUserModel; TempData.Keep();

            var _oUserModel = Newtonsoft.Json.JsonConvert.SerializeObject(oUserModel);
            TempData["oVmCurrMod"] = _oUserModel; TempData.Keep();
            
            var tm = DateTime.Now;
            _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                             "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, tm, oUserId_Logged, tm, tm, oUserId_Logged, oUserId_Logged));

            return PartialView("_AddOrEdit_UP", oUserModel);             
        }

        [HttpGet]
        public UserProfileModel AddOrEdit_UP_CL(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int subSetIndex = 0,
                                                int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null) //, int pageIndex = 1)
        {
            var oUserModel = new UserProfileModel(); TempData.Keep();
            if (setIndex == 0) return oUserModel;

            // 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
            var proScope = setIndex == 1 ? "V" : "C";
            var subScope = setIndex == 2 ? "D" : setIndex == 3 ? "A" : "";

            //if (subSetIndex >= 1 && subSetIndex <= 5) { proScope = "V"; subScope = ""; }
            //else if (subSetIndex == 6 || subSetIndex == 11) { proScope = "C"; subScope = "D"; }
            //else if (subSetIndex >= 6 && subSetIndex <= 15) { proScope = "C"; subScope = "A"; }

            if (id == 0)
            {   //create user and init... 
                //var existSUP_ADMNs = (
                //   from t_up in _context.UserProfile.AsNoTracking() //.Include(t => t.ChurchMember)
                //                .Where(c => c.Id == id &&
                //                (c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileScope == "V") ||
                //                (c.AppGlobalOwnerId == oDenomId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "C"))
                //   from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId && c.AppGlobalOwnerId == t_up.AppGlobalOwnerId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                //   from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                //                    .Where(c => c.ChurchBodyId == null && c.UserProfileId == t_up.Id &&
                //                    ((proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && (c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5)) ||
                //                     ((proScope == "C" && subScope == "D" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                //                     ((proScope == "C" && subScope == "A" && c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && (c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15))
                //                    )).DefaultIfEmpty()
                //   select t_up
                //   ).ToList();

                ////supadmin <creation> task.... but must have logged in as SYS
                //if (setIndex==1 && subSetIndex==2 && existSUP_ADMNs.Count > 0)
                //{ //prompt user sup_admin == 1 only
                //    oUserModel.oUserProfile = null;
                //    return oUserModel;
                //}

                var oUser = new UserProfile();
                oUser.ChurchBodyId = oCurrChuBodyId;
                oUser.Strt = DateTime.Now;
                oUser.ResetPwdOnNextLogOn = true;

                //oUP_MDL.oUserProfile.CountryId = oCurrCtryId;

                oUser.UserStatus = "A";   // A-ctive...D-eactive   
                oUser.ProfileScope = proScope;

                if (subSetIndex >= 1 && subSetIndex <= 5) // 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
                {
                    oUser.UserScope = "E";  // I-internal, E-external
                    if (subSetIndex == 2) { oUser.Username = "supadmin"; oUser.UserDesc = "Super Admin"; }
                }
                else // I-internal, E-external [manually config]
                { oUser.UserScope = "I"; }

                oUserModel.oUserProfile = oUser;
            }

            else
            {
                var oUser = (
                   from t_up in _context.UserProfile.Include(t => t.ContactInfo).AsNoTracking() //.Include(t => t.ChurchMember)
                                .Where(c => c.Id == id &&
                                (c.AppGlobalOwnerId == null && c.ChurchBodyId == null) ||  //&& c.ProfileScope == "V"
                                (oAppGloOwnId != null && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == "C"))
                   from t_cb in _context.MSTRChurchBody.AsNoTracking().Include(t => t.AppGlobalOwner).Where(c => c.Id == t_up.ChurchBodyId && c.AppGlobalOwnerId == t_up.AppGlobalOwnerId).DefaultIfEmpty()  //c.Id == oChurchBodyId && 
                   from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole)
                                 .Where(c => c.AppGlobalOwnerId == t_up.AppGlobalOwnerId && c.ChurchBodyId == t_up.ChurchBodyId && c.UserProfileId == t_up.Id &&
                                     (proScope == "V" && (c.UserRole.RoleType == "SYS" || c.UserRole.RoleType == "SUP_ADMN" || c.UserRole.RoleType == "SYS_ADMN" || c.UserRole.RoleType == "SYS_CUST") && c.UserRole.RoleLevel >= 1 && c.UserRole.RoleLevel <= 5) ||
                                     (proScope == "C" && subScope == "D" && (c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN") && (c.UserRole.RoleLevel == 6 || c.UserRole.RoleLevel == 11)) ||
                                     (proScope == "C" && subScope == "A" && (c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CH_RGSTR" || c.UserRole.RoleType == "CH_ACCT" || c.UserRole.RoleType == "CH_CUST" || c.UserRole.RoleType == "CF_ADMN" || c.UserRole.RoleType == "CF_RGSTR" || c.UserRole.RoleType == "CF_ACCT" || c.UserRole.RoleType == "CF_CUST") && c.UserRole.RoleLevel >= 6 && c.UserRole.RoleLevel <= 15)
                                  ).DefaultIfEmpty()

                       //// from t_upr in _context.UserProfileRole.AsNoTracking().Include(t => t.UserRole).Where(c => c.UserProfileId == t_up.Id).DefaultIfEmpty()
                       // from t_cm in _context.ChurchMember.AsNoTracking().Where(c => c.Id == t_up.ChurchBodyId && c.Id == t_up.ChurchMemberId).DefaultIfEmpty()
                       // from t_ur in _context.UserRole.AsNoTracking().Where(c => c.Id == t_upr.UserRoleId &&
                       //              ((c.RoleType == "SYS" || c.RoleType == "SUP_ADMN" || c.RoleType == "SYS_ADMN" || c.RoleType == "SYS_CUST") && (c.RoleLevel > 0 && c.RoleLevel <= 5)) ||
                       //              ((c.RoleType == "CH_ADMN" || c.RoleType == "CF_ADMN") && (c.RoleLevel >= 6 && c.RoleLevel <= 10))).DefaultIfEmpty()
                       // from t_urp in _context.UserRolePermission.AsNoTracking().Include(t => t.UserPermission)
                       //              .Where(c => c.UserRoleId == t_upr.UserRoleId).DefaultIfEmpty()
                       // from t_upg in _context.UserProfileGroup.AsNoTracking().Include(t => t.UserGroup)
                       //              .Where(c => c.UserProfileId == t_up.Id).DefaultIfEmpty()
                       // from t_ugp in _context.UserGroupPermission.AsNoTracking().Include(t => t.UserPermission)
                       //              .Where(c => c.UserGroupId == t_upg.UserGroupId).DefaultIfEmpty()

                   select new UserProfile()
                   {
                       Id = t_up.Id,
                       AppGlobalOwnerId = t_up.AppGlobalOwnerId,
                       ChurchBodyId = t_up.ChurchBodyId,
                       ChurchMemberId = t_up.ChurchMemberId,
                       ChurchBody = t_up.ChurchBody,
                       //  ChurchMember = t_up.ChurchMember,
                       OwnerUser = t_up.OwnerUser,
                       //
                       Username = t_up.Username,
                       UserDesc = t_up.UserDesc,
                       Email = t_up.Email,
                       ContactInfo = t_up.ContactInfo,
                       // PhoneNum = t_up.PhoneNum,
                       Pwd = t_up.Pwd,
                       PwdExpr = t_up.PwdExpr,
                       PwdSecurityQue = t_up.PwdSecurityQue,
                       PwdSecurityAns = t_up.PwdSecurityAns,
                       ResetPwdOnNextLogOn = t_up.ResetPwdOnNextLogOn,
                       Strt = t_up.Strt,
                       strStrt = t_up.strStrt,
                       Expr = t_up.Expr,
                       strExpr = t_up.strExpr != null ? DateTime.Parse(t_up.Expr.ToString()).ToString("d MMM, yyyy", CultureInfo.InvariantCulture) : "",
                       //
                       OwnerUserId = t_up.OwnerUserId,
                      // UserId = t_up.UserId,
                       UserScope = t_up.UserScope,
                       UserPhoto = t_up.UserPhoto,
                       ProfileScope = t_up.ProfileScope,
                       Created = t_up.Created,
                       CreatedByUserId = t_up.CreatedByUserId,
                       LastMod = t_up.LastMod,
                       LastModByUserId = t_up.LastModByUserId,
                       UserStatus = t_up.UserStatus,
                       strUserStatus = GetStatusDesc(t_up.UserStatus)
                   }
                   ).FirstOrDefault();

                oUserModel.oUserProfile = oUser;
                if (oUser != null)
                {
                    oUserModel.strUserProfile = oUser.UserDesc;
                    oUserModel.strChurchBody = oUser.ChurchBody != null ? oUser.ChurchBody.Name : "";
                    oUserModel.strAppGloOwn = oUser.AppGlobalOwner != null ? oUser.AppGlobalOwner.OwnerName : "";

                    //  strChurchMember = t_cb.AppGlobalOwner != null ? t_cb.AppGlobalOwner.OwnerName : "",
                    // strUserProfile = t_cm != null ? ((((!string.IsNullOrEmpty(t_cm.Title) ? t_cm.Title : "") + ' ' + t_cm.FirstName).Trim() + " " + t_cm.MiddleName).Trim() + " " + t_cm.LastName).Trim() : t_up.UserDesc

                }
            }

            //if (oUserModel.oUserProfile != null)
            //{
            //    if (oUserModel.oUserProfile.AppGlobalOwnerId != null)
            //    {
            //        List<MSTRChurchLevel> oCBLevels = _context.MSTRChurchLevel
            //            .Where(c => c.AppGlobalOwnerId == oUserModel.oUserProfile.AppGlobalOwnerId).ToList().OrderBy(c => c.LevelIndex).ToList();

            //        if (oCBLevels.Count() > 0)
            //        {
            //            ViewBag.Filter_ln = !string.IsNullOrEmpty(oCBLevels[oCBLevels.Count - 1].CustomName) ? oCBLevels[oCBLevels.Count - 1].CustomName : oCBLevels[6].Name;
            //            ViewBag.Filter_1 = !string.IsNullOrEmpty(oCBLevels[0].CustomName) ? oCBLevels[0].CustomName : oCBLevels[0].Name;

            //            if (oCBLevels.Count() > 1)
            //            {
            //                ViewBag.Filter_2 = ViewBag.Filter_2 = !string.IsNullOrEmpty(oCBLevels[1].CustomName) ? oCBLevels[1].CustomName : oCBLevels[1].Name;
            //                if (oCBLevels.Count() > 2)
            //                {
            //                    ViewBag.Filter_3 = ViewBag.Filter_3 = !string.IsNullOrEmpty(oCBLevels[2].CustomName) ? oCBLevels[2].CustomName : oCBLevels[2].Name;
            //                    if (oCBLevels.Count() > 3)
            //                    {
            //                        ViewBag.Filter_4 = ViewBag.Filter_4 = !string.IsNullOrEmpty(oCBLevels[3].CustomName) ? oCBLevels[3].CustomName : oCBLevels[3].Name;
            //                        if (oCBLevels.Count() > 4)
            //                        {
            //                            ViewBag.Filter_5 = ViewBag.Filter_5 = !string.IsNullOrEmpty(oCBLevels[4].CustomName) ? oCBLevels[43].CustomName : oCBLevels[4].Name;
            //                            if (oCBLevels.Count() > 5)
            //                            {
            //                                ViewBag.Filter_6 = ViewBag.Filter_6 = !string.IsNullOrEmpty(oCBLevels[5].CustomName) ? oCBLevels[5].CustomName : oCBLevels[5].Name;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            oUserModel.setIndex = setIndex;
            oUserModel.subSetIndex = subSetIndex;
            oUserModel.oCurrUserId_Logged = oUserId_Logged;
            oUserModel.oAppGloOwnId_Logged = oAGOId_Logged;
            oUserModel.oChurchBodyId_Logged = oCBId_Logged;
            //
            oUserModel.oAppGloOwnId = oAppGloOwnId;
            oUserModel.oChurchBodyId = oCurrChuBodyId;
            //  oUserModel.oCurrUserId_Logged = oCurrUserId_Logged; 

            // ChurchBody oCB = null;
            // if (oCurrChuBodyId != null)  oCB = _context.MSTRChurchBody.Where(c=>c.Id == oCurrChuBodyId && c.AppGlobalOwnerId==oDenomId).FirstOrDefault();

            //if (setIndex == 1) // (subSetIndex >= 1 && subSetIndex <= 5) // no SUP_ADMN or SYS as option
            //    oUserModel = this.populateLookups_UP_MS(oUserModel, null, null, setIndex);

            //else if (setIndex == 2 || setIndex == 3) // ((oUserModel.profileScope == "V" && (subSetIndex == 6 || subSetIndex == 11)) || (oUserModel.profileScope == "C" && subSetIndex >= 6 && subSetIndex <= 15))
            //{
            //    // var oCB = _context.MSTRChurchBody.Where(c => c.Id == oCurrChuBodyId && c.AppGlobalOwnerId == oDenomId).FirstOrDefault();
            //    oUserModel = this.populateLookups_UP_CL(oUserModel, oCurrChuBodyId);
            //}

            oUserModel = this.populateLookups_UP_CL(oUserModel, oAppGloOwnId, oCurrChuBodyId);

            //oUP_MDL.lkpStatuses = new List<SelectListItem>();
            //foreach (var dl in dlGenStatuses) { oUP_MDL.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            //TempData["oVmCurr"] = oUserModel;
            //TempData.Keep();

            // var _oUserModel = Newtonsoft.Json.JsonConvert.SerializeObject(oUserModel);
            // TempData["oVmCurr"] = _oUserModel; TempData.Keep();


            var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(oUserModel);
            TempData["oVmCurrMod"] = _vmMod; TempData.Keep();

            return oUserModel;
        }
        public JsonResult GetChurchBodyLevelsByAppGloOwn(int? oAppGloOwnId, bool addEmpty = false)
        {
            var oCBList = new List<SelectListItem>();
            ///
            oCBList = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oAppGloOwnId)
                                              .OrderByDescending(c => c.LevelIndex).ToList()

                   .Select(c => new SelectListItem()
                   {
                       Value = c.Id.ToString(),
                       Text = !string.IsNullOrEmpty(c.CustomName) ? c.CustomName : c.Name
                   })
                   //.OrderBy(c => c.Text)
                   .ToList();
            ///
            if (addEmpty) oCBList.Insert(0, new SelectListItem { Value = "", Text = "Select Church level" });
            return Json(oCBList);
        }
        private UserProfileModel populateLookups_UP_CL(UserProfileModel vmLkp, int? AppGloOwnId, int? oChurchBodyId)  //AppGloOwnId   ChurchBody oCurrChuBody)
        {
            if (vmLkp == null || oChurchBodyId == null) return vmLkp;
            //
            vmLkp.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            vmLkp.lkpUserRoles = _context.UserRole.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null) || (c.AppGlobalOwnerId == AppGloOwnId &&  c.ChurchBodyId == oChurchBodyId)) && 
                                                                c.RoleStatus == "A" && c.RoleLevel >= 6 && c.RoleLevel <= 15)
                               .OrderBy(c => c.RoleLevel)
                               .Select(c => new SelectListItem()
                               {
                                   Value = c.Id.ToString(),
                                   Text = c.RoleName.Trim()
                               })
                               // .OrderBy(c => c.Text)
                               .ToList();
            //  vmLkp.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            vmLkp.lkpUserGroups = _context.UserGroup.Where(c => c.Status == "A" && c.ChurchBodyId == oChurchBodyId)
                               .OrderBy(c => c.UserGroupCategoryId).ThenBy(c => c.GroupName)
                               .Select(c => new SelectListItem()
                               {
                                   Value = c.Id.ToString(),
                                   Text = c.GroupName.Trim()
                               })
                               // .OrderBy(c => c.Text)
                               .ToList();


            vmLkp.lkpPwdSecQueList = _context.AppUtilityNVP.Where(c => c.NvpCode == "PWD_SEC_QUE")
                      .OrderBy(c => c.RequireUserCustom).ThenBy(c => c.OrderIndex).ThenBy(c => c.NvpValue)
                      .ToList()
                      .Select(c => new SelectListItem()
                      {
                          Value = c.Id.ToString(),
                          Text = c.NvpValue
                      })
                      // .OrderBy(c => c.Text)
                      .ToList();

            vmLkp.lkpPwdSecQueList.Insert(0, new SelectListItem { Value = "", Text = "Select" });

            //vmLkp.lkpPwdSecAnsList = _context.AppUtilityNVP.Where(c => c.NvpCode == "PWD_SEC_ANS")
            //         .OrderBy(c => c.RequireUserCustom).ThenBy(c => c.OrderIndex).ThenBy(c => c.NvpValue)
            //         .ToList()
            //         .Select(c => new SelectListItem()
            //         {
            //             Value = c.Id.ToString(),
            //             Text = c.NvpValue
            //         })
            //         // .OrderBy(c => c.Text)
            //         .ToList();
            //vmLkp.lkpPwdSecAnsList.Insert(0, new SelectListItem { Value = "", Text = "Select" });


            return vmLkp;
        }

        private UserProfileModel populateLookups_UP_MS(UserProfileModel vmLkp, int setIndex, MSTRChurchBody oChurchBody = null)
        {
            //if (vmLkp == null || oDenom == null) return vmLkp;
            // 
            vmLkp.lkpStatuses = new List<SelectListItem>();
            foreach (var dl in dlGenStatuses) { vmLkp.lkpStatuses.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

            //vmLkp.lkpUserRoles = _context.UserRole.Where(c => AppGloOwnId == null && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 0 && c.RoleType=="SUP_ADMN" )
            if (setIndex == 1 ) //>= 2 && setIndex <= 3)  //   SYS .. 1-SUP_ADMN, 2-SYS_ADMN, 3-SYS_CUST
            {
                vmLkp.lkpUserRoles = _context.UserRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel > 2 && c.RoleLevel <= 5)
                    .OrderBy(c => c.RoleLevel)
                    .Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.RoleName.Trim()
                    })
                    // .OrderBy(c => c.Text)
                    .ToList();
                //  vmLkp.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });

                vmLkp.lkpUserGroups = _context.UserGroup.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.Status == "A")
                                   .OrderBy(c => c.UserGroupCategoryId).ThenBy(c => c.GroupName)
                                   .Select(c => new SelectListItem()
                                   {
                                       Value = c.Id.ToString(),
                                       Text = c.GroupName.Trim()
                                   })
                                   // .OrderBy(c => c.Text)
                                   .ToList();
            }

            else if (setIndex == 2 || setIndex == 3) // == 6 || subSetIndex == 7)  //  6-CH_ADMN, 7-CF_ADMN
            {
                vmLkp.lkpAppGlobalOwns = _context.MSTRAppGlobalOwner.Where(c => c.Status == "A")
                                             .OrderBy(c => c.OwnerName).ToList()
                                             .Select(c => new SelectListItem()
                                             {
                                                 Value = c.Id.ToString(),
                                                 Text = c.OwnerName
                                             })
                                             .ToList();


                if (oChurchBody != null)
                {
                    vmLkp.lkpChurchLevels = _context.MSTRChurchLevel.Where(c => c.AppGlobalOwnerId == oChurchBody.AppGlobalOwnerId)
                                              .OrderByDescending(c => c.LevelIndex)
                                              .Select(c => new SelectListItem()
                                              {
                                                  Value = c.Id.ToString(),
                                                  Text = !string.IsNullOrEmpty(c.CustomName) ? c.CustomName : c.Name
                                              })
                                              .ToList();
                    //vm.lkpChurchLevels.Insert(0, new SelectListItem { Value = "", Text = "Select" });
                }


                if (setIndex == 2)
                {
                    vmLkp.lkpUserRoles = _context.UserRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null &&
                                                        c.RoleStatus == "A" && (c.RoleLevel == 6 || c.RoleLevel == 11))
                        .OrderBy(c => c.RoleLevel)
                        .Select(c => new SelectListItem()
                        {
                            Value = c.Id.ToString(),
                            Text = c.RoleName.Trim()
                        })
                        // .OrderBy(c => c.Text)
                        .ToList(); 
                }

                else if (setIndex == 3)
                {
                    vmLkp.lkpUserRoles = _context.UserRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null &&
                                                        c.RoleStatus == "A" && (c.RoleLevel >= 6 && c.RoleLevel <= 15))
                        .OrderBy(c => c.RoleLevel)
                        .Select(c => new SelectListItem()
                        {
                            Value = c.Id.ToString(),
                            Text = c.RoleName.Trim()
                        })
                        // .OrderBy(c => c.Text)
                        .ToList(); 
                }

                vmLkp.lkpUserRoles.Insert(0, new SelectListItem { Value = "", Text = "Select" });

                //
                vmLkp.lkpUserGroups = _context.UserGroup.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.Status == "A")
                                   .OrderBy(c => c.UserGroupCategoryId).ThenBy(c => c.GroupName)
                                   .Select(c => new SelectListItem()
                                   {
                                       Value = c.Id.ToString(),
                                       Text = c.GroupName.Trim()
                                   })
                                   // .OrderBy(c => c.Text)
                                   .ToList();
            }

            return vmLkp;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_UP(UserProfileModel vm)
        {
            UserProfile _oChanges = vm.oUserProfile;
            //   vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as UserProfileModel : vmMod; TempData.Keep();

            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            var vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfileModel>(arrData) : vm;

            var oUP = vmMod.oUserProfile;
            oUP.ChurchBody = vmMod.oChurchBody;

            try
            {
                ModelState.Remove("oUserProfile.AppGlobalOwnerId");
                ModelState.Remove("oUserProfile.ChurchBodyId");
                ModelState.Remove("oUserProfile.ChurchMemberId");
                ModelState.Remove("oUserProfile.CreatedByUserId");
                ModelState.Remove("oUserProfile.LastModByUserId");
                ModelState.Remove("oUserProfile.OwnerId");
                ModelState.Remove("oUserProfile.UserId");

                // ChurchBody == null 

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed to load the data to save. Please refresh and try again.", signOutToLogIn = false });

                if (string.IsNullOrEmpty(_oChanges.Username)) // || string.IsNullOrEmpty(_oChanges.Pwd))  //Congregant... ChurcCodes required
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide username and password.", signOutToLogIn = false });
                }
                //if (_oChanges.PwdSecurityQue != null && string.IsNullOrEmpty(_oChanges.PwdSecurityAns))  //Congregant... ChurcCodes required
                //{
                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the response to the security question specified.", signOutToLogIn = false });
                //}


                //confirm this is SYS acc   //check for the SYS acc
                //string strPwdHashedData = AppUtilties.ComputeSha256Hash(_oChanges.ChurchCode + _oChanges.Username.Trim().ToLower() + _oChanges.Password);
                //string strUserKeyHashedData = AppUtilties.ComputeSha256Hash(vm.ChurchCode + _oChanges.Username.Trim().ToLower());
                // c.Username.Trim().ToLower() == model.Username.Trim().ToLower() && c.UserKey == strUserKeyHashedData && c.Pwd == strPwdHashedData
                ///
                var currLogUserInfo = (from up in _context.UserProfile.Where(c => c.AppGlobalOwnerId==null && c.ChurchBodyId == null && c.Id == vmMod.oCurrUserId_Logged)
                                       from upr in _context.UserProfileRole.Where(c => c.AppGlobalOwnerId==null && c.ChurchBodyId == null &&  c.UserProfileId == up.Id && c.ProfileRoleStatus == "A" && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))
                                       from ur in _context.UserRole.Where(c => c.AppGlobalOwnerId==null && c.ChurchBodyId == null && c.Id == upr.UserRoleId && c.RoleStatus == "A") // && c.RoleLevel == 1 && c.RoleType == "SYS")
                                       select new
                                       {
                                           UserId = up.Id,
                                           UserRoleId = ur.Id,
                                           UserType = ur.RoleType,
                                           UserRoleLevel = ur.RoleLevel,
                                           UserStatus = up.strUserStatus == "A" && upr.ProfileRoleStatus == "A" && ur.RoleStatus == "A"
                                       }
                                 ).FirstOrDefault();


                if (currLogUserInfo == null)
                { return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Current user not found! Please refresh and try again.", signOutToLogIn = false }); }

                if (_oChanges.ProfileScope == "V")  //vendor admins ... SYS, SUP_ADMN, SYS_ADMN etc.
                {
                    if (currLogUserInfo.UserType == "SYS" && string.Compare(_oChanges.Username, "supadmin", true) != 0 && string.Compare(_oChanges.Username, "sys", true) != 0)
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "SYS account can ONLY manage the [sys] or [supadmin] profile. Hint: Sign in with [supadmin] or other Admin account.", signOutToLogIn = false });
                    }

                    if (currLogUserInfo.UserType == "SUP_ADMN" && string.Compare(_oChanges.Username, "sys", true) == 0)
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Current user does not have SYS role. SYS role required to manage SYS account.", signOutToLogIn = false });
                    }

                    if (currLogUserInfo.UserType != "SYS" && string.Compare(_oChanges.Username, "supadmin", true) == 0) // currLogUserInfo.UserType != "SUP_ADMN" && 
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Current user does not have SYS role. SYS role required to manage SUP_ADMN account.", signOutToLogIn = false });
                    }

                    if (_oChanges.Id == 0)
                    {
                        if (string.Compare(_oChanges.Username, "sys", true) == 0)
                        {
                            var existUserRoles = (from upr in _context.UserProfileRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileRoleStatus == "A" && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))
                                                  from ur in _context.UserRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.Id == upr.UserRoleId && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SYS")
                                                  select upr
                                     );
                            if (existUserRoles.Count() > 0)
                            {
                                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "SYS account cannot be created. Only one (1) SYS role allowed.", signOutToLogIn = false });
                            }
                        }

                        if (string.Compare(_oChanges.Username, "supadmin", true) == 0)
                        {
                            var existUserRoles = (from upr in _context.UserProfileRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileRoleStatus == "A" && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))
                                                  from ur in _context.UserRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.Id == upr.UserRoleId && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN")
                                                  select upr
                                     );
                            if (existUserRoles.Count() > 0)
                            {
                                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Super Admin profile cannot be created. Only one (1) SUP_ADMN role allowed.", signOutToLogIn = false });
                            }
                        }
                    }
                     

                    // check if client database has been created or can connect... thus if task is to create/manage a client admin profile
                    // 1 - vendor admins, 2 - client admins, 3 - client users
                    ///
                    if (vm.setIndex == 2 || vm.setIndex == 3)
                    {
                        // Get the client database details.... db connection string                        
                        var oClientConfig = _context.ClientAppServerConfig.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.Status == "A").FirstOrDefault();
                        if (oClientConfig == null) 
                        { ViewData["strUserLoginFailMess"] = "Client database details not found. Please try again or contact System Admin";
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = ViewData["strUserLoginFailMess"], signOutToLogIn = false }); }

                        // get and mod the conn
                        var _clientDBConnString = "";
                        var conn = new SqlConnectionStringBuilder(_context.Database.GetDbConnection().ConnectionString);
                        conn.DataSource = oClientConfig.ServerName; conn.InitialCatalog = oClientConfig.DbaseName; conn.UserID = oClientConfig.SvrUserId; conn.Password = oClientConfig.SvrPassword; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
                        _clientDBConnString = conn.ConnectionString;

                        // test the NEW DB conn
                        var _clientContext = new ChurchModelContext(_clientDBConnString);
                        if (!_clientContext.Database.CanConnect()) 
                        { ViewData["strUserLoginFailMess"] = "Failed to connect client database. Please try again or contact System Admin"; 
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = ViewData["strUserLoginFailMess"], signOutToLogIn = false }); }  // give appropriate user prompts

                    }
                }

                else  //CLIENT ADMINs ... creating users for their churches /congregations
                {
                    if (_oChanges.AppGlobalOwnerId == null)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify the denomination/church.", signOutToLogIn = false });

                    if (_oChanges.oCBChurchLevelId == null)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify the church level for the user profile.", signOutToLogIn = false });

                    //var oCBLevel = _context.MSTRChurchLevel.Find(_oChanges.oCBChurchLevelId);
                    //if (oCBLevel == null)  // ... parent church level > church unit level
                    //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church unit level could not be found. Please refresh and try again", signOutToLogIn = false });

                    /// get the parent id
                    /// 
                    var parDesc = "church unit";
                    switch (vm.oCBLevelCount)
                    {
                        case 1: _oChanges.ChurchBodyId = vm.ChurchBodyId_1; parDesc = vm.strChurchLevel_1; break;
                        case 2: _oChanges.ChurchBodyId = vm.ChurchBodyId_2; parDesc = vm.strChurchLevel_2; break;
                        case 3: _oChanges.ChurchBodyId = vm.ChurchBodyId_3; parDesc = vm.strChurchLevel_3; break;
                        case 4: _oChanges.ChurchBodyId = vm.ChurchBodyId_4; parDesc = vm.strChurchLevel_4; break;
                        case 5: _oChanges.ChurchBodyId = vm.ChurchBodyId_5; parDesc = vm.strChurchLevel_5; break;
                    }
                     

                    //check availability of username... SYS /SUP_ADMN reserved
                    if (string.Compare(_oChanges.Username, "sys", true) == 0 || string.Compare(_oChanges.Username, "supadmin", true) == 0)
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Username 'supadmin' not available. Try different username.", signOutToLogIn = false });
                    }

                    // Denomination and ChurchBody cannot be null
                    if (_oChanges.AppGlobalOwnerId==null || _oChanges.ChurchBodyId==null)
                    {
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specify the denomination and church unit", signOutToLogIn = false });
                    }
                }


                //check that username is unique in all instances
                var existUserProfiles = _context.UserProfile.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null) || (c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId )) && // && c.ChurchBodyId == _oChanges.ChurchBodyId //... restrict within denomination as dbase is per denomination
                                                                        c.Id != _oChanges.Id && c.Username.Trim().ToLower() == _oChanges.Username.Trim().ToLower()).ToList();
                if (existUserProfiles.Count() > 0)
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Username '" + _oChanges.Username + "' not available. Try different username. [Hint: User's email is unique.]", signOutToLogIn = false });
                }

                if (string.IsNullOrEmpty(_oChanges.UserDesc))  //Congregant... ChurchCodes required
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please provide the user description or name of user.", signOutToLogIn = false });
                }

                if (_oChanges.Expr != null)  //allow historic
                {
                    if (_oChanges.UserStatus == "A" && _oChanges.Expr.Value <= DateTime.Now.Date)
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please user account has expired. Activate account first.", signOutToLogIn = false });

                    if (_oChanges.Strt != null)
                        if (_oChanges.Strt.Value > _oChanges.Expr.Value)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Start date cannot be later than expiry date", signOutToLogIn = false });
                }

                //// Check password ... should be done @LOGIN instead. jux update:- keep as it is.
                //if (_oChanges.PwdExpr == null)
                //{
                //    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "User profile password not set. Reset password and try again.", signOutToLogIn = false });
                //}
                //else
                //{
                //    if (_oChanges.PwdExpr.Value <= DateTime.Now.Date)
                //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "User profile password has expired. Reset password.", signOutToLogIn = false });
                //}

                //Email... must be REQUIRED -- for password reset!
                if (_oChanges.Email == null)
                {
                    return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please specify email of user. Email is needed for password reset and login user verifications.", signOutToLogIn = false });
                }
                            //check email availability and validity
                else // (_oChanges.Email != null) //_oChanges.ChurchMemberId != null && 
                {
                    // ??? ... check validity... REGEX
                    ///
                    // if (AppUtil)

                    var oUserEmailExist = _context.UserProfile.Where(c => c.Id != _oChanges.Id && c.Email == _oChanges.Email).FirstOrDefault();
                    if (oUserEmailExist != null)  // ModelState.AddModelError(_oChanges.Id.ToString(), "Email of member must be unique. >> Hint: Already used by another member: "  + GetConcatMemberName(_oChanges.Title, _oChanges.FirstName, _oChanges.MiddleName, _oChanges.LastName) + "[" + oCM.ChurchBody.Name + "]");
                        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "User email must be unique. Email already used by another: [User: " + _oChanges.UserDesc + "]", signOutToLogIn = false });


                    var oUserCIEmailExist = _context.MSTRContactInfo.Where(c => c.RefUserId != _oChanges.Id && c.Email == _oChanges.Email).FirstOrDefault();
                    if (oUserCIEmailExist != null)  // ModelState.AddModelError(_oChanges.Id.ToString(), "Email of member must be unique. >> Hint: Already used by another member: "  + GetConcatMemberName(_oChanges.Title, _oChanges.FirstName, _oChanges.MiddleName, _oChanges.LastName) + "[" + oCM.ChurchBody.Name + "]");
                        return Json(new { taskSuccess = false,  oCurrId = _oChanges.Id,  userMess = "User email must be unique. Email already used by another: [User: " + _oChanges.UserDesc + "]",  signOutToLogIn = false  });
                     

                    //if (_oChanges == null)
                    //{
                    //    return Json(new
                    //    {
                    //        taskSuccess = false,
                    //        oCurrId = _oChanges.Id,
                    //        userMess = "Member status [ current state of the person - active, dormant, invalid, deceased etc. ] is required"
                    //    });
                    //}

                    ////member must be active, NOT deceased
                    //if (_oChanges_MS.ChurchMemStatusId == null)
                    //{
                    //    return Json(new
                    //    {
                    //        taskSuccess = false,
                    //        oCurrId = _oChanges.Id,
                    //        userMess = "Select the Member status [current state of the person - active, dormant, invalid, deceased etc.] as applied"
                    //    });
                    //}

                }


                // SYS control account check...
                var oAdminsCount = _context.UserProfile.Count();
                if (string.Compare(_oChanges.Username, "sys", true) == 0 && oAdminsCount > 1)  // other users have been created....
                {
                    //check the SYS account...
                    var oSYSAcc = _context.UserProfile.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.Username.ToLower() == "SYS".ToLower() && c.UserStatus == "A").FirstOrDefault();
                    if (oSYSAcc == null)
                        return Json(new { taskSuccess = false, oCurrId = oSYSAcc.Id, userMess = "SYS profile not found. SYS profile is a control account. Contact System Admin for help.", signOutToLogIn = false });
                    ///

                    if (oSYSAcc.PwdExpr == null)  
                    {
                        return Json(new { taskSuccess = false, oCurrId = oSYSAcc.Id, userMess = "SYS profile password not set. Reset password and try again.", signOutToLogIn = false });
                    }
                    else
                    {
                        if (oSYSAcc.PwdExpr.Value <= DateTime.Now.Date)
                            return Json(new { taskSuccess = false, oCurrId = oSYSAcc.Id, userMess = "SYS profile password has expired. Reset password.", signOutToLogIn = false }); 
                    }

                    if (string.IsNullOrEmpty(oSYSAcc.Email))
                        return Json(new { taskSuccess = false, oCurrId = oSYSAcc.Id, userMess = "SYS profile email not configured. Email is needed for password reset and login user verifications.", signOutToLogIn = false });

                }



                //if (_oChanges.ProfileScope == "V" && string.Compare(_oChanges.Username, "sys", true) == 0)
                //{
                //    var oAdminsCount = _context.UserProfile.Count(); // (c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.ProfileScope == "V");
                //    if (oAdminsCount > 0 ) 
                //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "There are other users. SYS profile must have assigned email. Email is needed for password reset and login user verifications.", signOutToLogIn = false });
                //}

                //else
                //{
                //    if (_oChanges.Email != null)
                //    {
                //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please specify email of user. Email is needed for password reset and login user verifications.", signOutToLogIn = false });
                //    }
                //}
                 

                _oChanges.LastMod = DateTime.Now;
                _oChanges.LastModByUserId = vmMod.oCurrUserId_Logged;
                string uniqueFileName = null;

                var oFormFile = vm.UserPhotoFile;
                if (oFormFile != null && oFormFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img_db");  //~/frontend/dist/img_db
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + oFormFile.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    oFormFile.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                else
                    if (_oChanges.Id != 0) uniqueFileName = _oChanges.UserPhoto;

                _oChanges.UserPhoto = uniqueFileName;

                //_oChanges.PwdSecurityQue = "What account is this?"; _oChanges.PwdSecurityAns = "Rhema-SYS";
                if (!string.IsNullOrEmpty((_oChanges.PwdSecurityQue + _oChanges.PwdSecurityAns).Trim()))
                    _oChanges.PwdSecurityAns = AppUtilties.ComputeSha256Hash(_oChanges.PwdSecurityQue + _oChanges.PwdSecurityAns);

                var tm = DateTime.Now;
                _oChanges.LastMod = tm;
                _oChanges.CreatedByUserId = vmMod.oCurrUserId_Logged;
               // _oChanges.AppGlobalOwnerId = 
               // _oChanges.ChurchBodyId = 

                //validate...
                var _userTask = "Attempted saving user profile, " + (!string.IsNullOrEmpty(_oChanges.UserDesc) ? "[" + _oChanges.UserDesc + "]" : "");  //    _userTask = "Added new user profile, " + (!string.IsNullOrEmpty(_oChanges.UserDesc) ? "[" + _oChanges.UserDesc + "]" : "") + " successfully";  // _userTask = "Updated user profile, " + (!string.IsNullOrEmpty(_oChanges.UserDesc) ? "[" + _oChanges.UserDesc + "]" : "") + " successfully";

                using (var _userCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    if (_oChanges.Id == 0)
                    {                         
                        var cc = "";
                        if (_oChanges.AppGlobalOwnerId == null && _oChanges.ChurchBodyId == null && _oChanges.ProfileScope == "V")
                        {
                            cc = "000000";    //var churchCode = "000000"; _oChanges.Username = "SysAdmin"; _oChanges.Pwd = "$ys@dmin1";  
                        }
                        else  //client admins
                        {
                            var oAGO = _context.MSTRAppGlobalOwner.Find(_oChanges.AppGlobalOwnerId);
                            var oCB = _context.MSTRChurchBody.Where(c=> c.AppGlobalOwnerId==_oChanges.AppGlobalOwnerId && c.Id==_oChanges.ChurchBodyId).FirstOrDefault();

                            if (oAGO == null || oCB==null)
                                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specified denomination and church unit could not be retrieved. Please refresh and try again.", signOutToLogIn = false });
                            if (string.IsNullOrEmpty(oCB.GlobalChurchCode))
                                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church code not specified. The unique global church code of church unit is required. Please verify with System Admin and try again.", signOutToLogIn = false });

                            cc = oCB.GlobalChurchCode;
                            
                            // _oChanges.Pwd = AppUtilties.ComputeSha256Hash(_oChanges.Username + _oChanges.Pwd);
                        }

                        _oChanges.UserKey = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username.ToLower());
                        _oChanges.Pwd = "123456";  //temp pwd... to reset @ next login  
                        _oChanges.Pwd = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username.ToLower() + _oChanges.Pwd);
                        _oChanges.Strt = tm;
                        _oChanges.Expr = (string.Compare(_oChanges.Username, "sys", true) == 0 || string.Compare(_oChanges.Username, "supadmin", true) == 0) ? (DateTime?)null : tm.AddDays(90);  //default to 90 days
                        _oChanges.ResetPwdOnNextLogOn = true;
                        _oChanges.PwdExpr = tm.AddDays(30);  //default to 30 days 
                        ///
                        _oChanges.Created = tm;
                        _oChanges.CreatedByUserId = vmMod.oCurrUserId_Logged;


                        _userCtx.Add(_oChanges);

                        _userTask = "Added new user profile, " + (!string.IsNullOrEmpty(_oChanges.UserDesc) ? "[" + _oChanges.UserDesc + "]" : "") + " successfully";
                        ViewBag.UserMsg = "Saved user profile, " + (!string.IsNullOrEmpty(_oChanges.UserDesc) ? "[" + _oChanges.UserDesc + "]" : "") + " successfully. Password must be changed on next logon";
                    }
                    else
                    {
                        if (string.Compare(_oChanges.UserKey, AppUtilties.ComputeSha256Hash(_oChanges.strChurchCode_CB + _oChanges.Username.ToLower()), true ) != 0) 
                            _oChanges.UserKey = AppUtilties.ComputeSha256Hash(_oChanges.strChurchCode_CB + _oChanges.Username.ToLower());

                        //retain the pwd details... hidden fields
                        _userCtx.Update(_oChanges);

                        _userTask = "Updated user profile, " + (!string.IsNullOrEmpty(_oChanges.UserDesc) ? "[" + _oChanges.UserDesc + "]" : "") + " successfully";
                        ViewBag.UserMsg = "User profile, " + (!string.IsNullOrEmpty(_oChanges.UserDesc) ? "[" + _oChanges.UserDesc + "]" : "") + "updated successfully.";
                    }
                                       

                    //save user profile first... 
                    await _userCtx.SaveChangesAsync();
                    ///
                    DetachAllEntities(_userCtx);
                }
                

                //audit...
                var _tm = DateTime.Now;
                await this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vmMod.oCurrUserId_Logged, _tm, _tm, vmMod.oCurrUserId_Logged, vmMod.oCurrUserId_Logged));


             //check if role assigned... SUP_ADMN -- auto, others -- warn!
                using (var _roleCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {         
                    if (_oChanges.ProfileLevel == 2) //subSetIndex == 2) // SUP_ADMN role
                    {
                        var oSupAdminRole = _context.UserRole.Where(c => c.AppGlobalOwnerId== null && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN").FirstOrDefault();
                        if (oSupAdminRole != null)
                        {
                            var existUserRoles = (from upr in _context.UserProfileRole.Where(c => c.AppGlobalOwnerId==null && c.ChurchBodyId == null && c.UserRoleId == oSupAdminRole.Id && c.ProfileRoleStatus == "A") // && 
                                                                                                                                                                                           // ((c.Strt == null || c.Expr == null) || (c.Strt != null && c.Expr != null && c.Strt <= DateTime.Now && c.Expr >= DateTime.Now && c.Strt <= c.Expr)))
                                                                                                                                                                                           // from up in roleCtx.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN")
                                                  select upr
                                         ).ToList();

                            //add SUP_ADMN role to SUP_ADMN user ... assign all privileges to the SUP_ADMN role
                            if (existUserRoles.Count() == 0)
                            {
                                //var oSupAdminRole = roleCtx.UserRole.Where(c => c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN").FirstOrDefault();
                                //if (oSupAdminRole != null)
                                //{        


                                var oUserRole = new UserProfileRole
                                {
                                    AppGlobalOwnerId = null,
                                    ChurchBodyId = null,
                                    UserRoleId = oSupAdminRole.Id,
                                    UserProfileId = _oChanges.Id,
                                    Strt = tm,
                                    // Expr = tm,
                                    ProfileRoleStatus = "A",
                                    Created = tm,
                                    LastMod = tm,
                                    CreatedByUserId = vmMod.oCurrUserId_Logged,
                                    LastModByUserId = vmMod.oCurrUserId_Logged
                                };

                                _roleCtx.Add(oUserRole);
                                //save user role...
                                await _roleCtx.SaveChangesAsync();

                                DetachAllEntities(_roleCtx);


                                _userTask = "Added SUP_ADMN role to user, " + _oChanges.Username;
                                ViewBag.UserMsg += Environment.NewLine + " ~ SUP_ADMN role added.";


                                _tm = DateTime.Now;
                                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vmMod.oCurrUserId_Logged, _tm, _tm, vmMod.oCurrUserId_Logged, vmMod.oCurrUserId_Logged));
                                // }


                                if (oSupAdminRole != null)
                                {
                                    using (var _permCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                                    {
                                        // assign all privileges [vendor domain only] to the SUP_ADMN role 
                                        var existUserRolePerms = (from upr in _context.UserRolePermission.Where(c => c.AppGlobalOwnerId==null &&  c.ChurchBodyId == null && 
                                                                  c.Status == "A" && c.UserRoleId == oSupAdminRole.Id && c.UserRole.RoleStatus == "A") // && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))     // from up in permCtx.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SUP_ADMN")
                                                                  select upr);
                                        //if (existUserRolePerms.Count() > 0)
                                        //{

                                        // get only [A0_] System Admin permissions ... 
                                        var oUserPerms = (from upr in _context.UserPermission.Where(c => c.PermStatus == "A" && c.PermissionCode.StartsWith("A0"))                                                                                                                                                      // from up in permCtx.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SUP_ADMN")
                                                          select upr);

                                        if (oUserPerms.Count() > 0) //(existUserRolePerms.Count() < oUserPerms.Count())
                                        {
                                            var rowUpdated = false; var rowsUpdated = 0; var rowsAdded = 0;
                                            foreach (var oURP in oUserPerms)
                                            {
                                                var existUserRolePerm = existUserRolePerms.Where(c => c.UserPermissionId == oURP.Id).FirstOrDefault();
                                                if (existUserRolePerm == null)
                                                {
                                                    var oUserRolePerm = new UserRolePermission
                                                    {
                                                        AppGlobalOwnerId = null,
                                                        ChurchBodyId = null,
                                                        UserRoleId = oSupAdminRole.Id,
                                                        UserPermissionId = oURP.Id,
                                                        ViewPerm = true,
                                                        CreatePerm = true,
                                                        EditPerm = true,
                                                        DeletePerm = true,
                                                        ManagePerm = true,
                                                        Status = "A",
                                                        Created = tm,
                                                        LastMod = tm,
                                                        CreatedByUserId = vmMod.oCurrUserId_Logged,
                                                        LastModByUserId = vmMod.oCurrUserId_Logged
                                                    };

                                                    _permCtx.Add(oUserRolePerm);
                                                    rowsAdded++;
                                                }
                                                else
                                                {
                                                    rowUpdated = false;
                                                    if (!existUserRolePerm.ViewPerm) { existUserRolePerm.ViewPerm = true; rowUpdated = true; }
                                                    if (!existUserRolePerm.CreatePerm) { existUserRolePerm.CreatePerm = true; rowUpdated = true; }
                                                    if (!existUserRolePerm.EditPerm) { existUserRolePerm.EditPerm = true; rowUpdated = true; }
                                                    if (!existUserRolePerm.DeletePerm) { existUserRolePerm.DeletePerm = true; rowUpdated = true; }
                                                    if (!existUserRolePerm.ManagePerm) { existUserRolePerm.ManagePerm = true; rowUpdated = true; }

                                                    if (rowUpdated)
                                                    {
                                                        existUserRolePerm.Created = tm;
                                                        existUserRolePerm.LastMod = tm;
                                                        existUserRolePerm.CreatedByUserId = vmMod.oCurrUserId_Logged;
                                                        existUserRolePerm.LastModByUserId = vmMod.oCurrUserId_Logged;

                                                        _permCtx.Add(existUserRolePerm);
                                                        rowsUpdated++;
                                                    }
                                                }
                                            }

                                            // prompt users
                                            if (rowsAdded > 0)
                                            {
                                                _userTask = "Added " + rowsAdded + " user permissions to SUP_ADMN role";
                                                ViewBag.UserMsg += Environment.NewLine + " ~ " + rowsAdded + " user permissions added.";
                                            }
                                            if (rowsUpdated > 0)
                                            {
                                                _userTask = "Updated " + rowsUpdated + " user permissions on SUP_ADMN role";
                                                ViewBag.UserMsg += ". " + rowsUpdated + " user permissions updated.";
                                            }

                                            if ((rowsAdded + rowsUpdated) > 0)
                                            {
                                                //save changes... 
                                                await _permCtx.SaveChangesAsync();

                                                DetachAllEntities(_permCtx);

                                                _tm = DateTime.Now;
                                                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                                                  "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vmMod.oCurrUserId_Logged, _tm, _tm, vmMod.oCurrUserId_Logged, vmMod.oCurrUserId_Logged));

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    else if (_oChanges.ProfileLevel == 6 || _oChanges.ProfileLevel == 11)  //(vmMod.subSetIndex == 6 || vmMod.subSetIndex == 11) // SUP_ADMN role
                    {
                        var oChuAdminRole = _context.UserRole.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.RoleStatus == "A" && 
                                                                        (c.RoleLevel == 6 || c.RoleLevel == 11) && (c.RoleType == "CH_ADMN" || c.RoleType == "CF_ADMN")).FirstOrDefault();
                        if (oChuAdminRole != null)
                        {
                            var existUserRoles = (from upr in _context.UserProfileRole.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.ChurchBodyId == _oChanges.ChurchBodyId && 
                                                  c.UserRoleId == oChuAdminRole.Id && c.ProfileRoleStatus == "A") // &&                                                                                                                                                                                                                           // ((c.Strt == null || c.Expr == null) || (c.Strt != null && c.Expr != null && c.Strt <= DateTime.Now && c.Expr >= DateTime.Now && c.Strt <= c.Expr)))
                                                                                                                                                                                                                          // from up in roleCtx.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN")
                                                  select upr
                                         ).ToList();

                            var oUserRole = new UserProfileRole();
                            //add SUP_ADMN role to SUP_ADMN user ... assign all privileges to the SUP_ADMN role
                            if (existUserRoles.Count() == 0)
                            {
                                //var oSupAdminRole = roleCtx.UserRole.Where(c => c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN").FirstOrDefault();
                                //if (oSupAdminRole != null)
                                //{        

                                oUserRole = new UserProfileRole
                                {
                                    AppGlobalOwnerId = _oChanges.AppGlobalOwnerId,
                                    ChurchBodyId = _oChanges.ChurchBodyId,
                                    UserRoleId = oChuAdminRole.Id,
                                    UserProfileId = _oChanges.Id,
                                    Strt = tm,
                                    // Expr = tm,
                                    ProfileRoleStatus = "A",
                                    Created = tm,
                                    LastMod = tm,
                                    CreatedByUserId = vmMod.oCurrUserId_Logged,
                                    LastModByUserId = vmMod.oCurrUserId_Logged
                                };

                                _roleCtx.Add(oUserRole);
                                //save user role...
                                await _roleCtx.SaveChangesAsync();

                                DetachAllEntities(_roleCtx);

                                _userTask = "Added [" + oChuAdminRole.RoleType + "] role to user, " + _oChanges.Username;
                                ViewBag.UserMsg += Environment.NewLine + " ~ [" + oChuAdminRole.RoleType + "] role added.";


                                _tm = DateTime.Now;
                                _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vmMod.oCurrUserId_Logged, _tm, _tm, vmMod.oCurrUserId_Logged, vmMod.oCurrUserId_Logged));
                                // } 
                            }
                            else
                            {
                                oUserRole = existUserRoles[0];  // user may have multiple roles .. CHECK
                            }

                            if (oChuAdminRole != null && oUserRole != null)
                            {
                                using (var _permCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                                {
                                    // assign all privileges to the SUP_ADMN role 
                                    var existUserRolePerms = (from upr in _context.UserRolePermission.Where(c => c.AppGlobalOwnerId == null && c.ChurchBodyId == null && c.Status == "A" && c.UserRoleId == oUserRole.UserRoleId && c.UserRole.RoleStatus == "A") // && (c.Strt == null || c.Strt <= DateTime.Now) && (c.Expr == null || c.Expr >= DateTime.Now))     // from up in permCtx.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SUP_ADMN")
                                                              select upr);
                                    //if (existUserRolePerms.Count() > 0)
                                    //{

                                    // get only [00_] Church level Admin permissions ... 
                                    var oUserPerms = (from upr in _context.UserPermission.Where(c => c.PermStatus == "A" && c.PermissionCode.StartsWith("01"))                                                                                                                                                      // from up in permCtx.UserRole.Where(c => c.Id == upr.UserRoleId && c.ChurchBodyId == null && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SUP_ADMN")
                                                      select upr);

                                    if (oUserPerms.Count() > 0) //(existUserRolePerms.Count() < oUserPerms.Count())
                                    {
                                        var rowUpdated = false; var rowsUpdated = 0; var rowsAdded = 0;
                                        foreach (var oURP in oUserPerms)
                                        {
                                            var existUserRolePerm = existUserRolePerms.Where(c => c.UserPermissionId == oURP.Id).FirstOrDefault();
                                            if (existUserRolePerm == null)
                                            {
                                                var oUserRolePerm = new UserRolePermission
                                                {
                                                    AppGlobalOwnerId = _oChanges.AppGlobalOwnerId,
                                                    ChurchBodyId = _oChanges.ChurchBodyId,
                                                    UserRoleId = oUserRole.UserRoleId,
                                                    UserPermissionId = oURP.Id,
                                                    ViewPerm = true,
                                                    CreatePerm = true,
                                                    EditPerm = true,
                                                    DeletePerm = true,
                                                    ManagePerm = true,
                                                    Status = "A",
                                                    Created = tm,
                                                    LastMod = tm,
                                                    CreatedByUserId = vmMod.oCurrUserId_Logged,
                                                    LastModByUserId = vmMod.oCurrUserId_Logged
                                                };

                                                _permCtx.Add(oUserRolePerm);
                                                rowsAdded++;
                                            }
                                            else
                                            {
                                                rowUpdated = false;
                                                if (!existUserRolePerm.ViewPerm) { existUserRolePerm.ViewPerm = true; rowUpdated = true; }
                                                if (!existUserRolePerm.CreatePerm) { existUserRolePerm.CreatePerm = true; rowUpdated = true; }
                                                if (!existUserRolePerm.EditPerm) { existUserRolePerm.EditPerm = true; rowUpdated = true; }
                                                if (!existUserRolePerm.DeletePerm) { existUserRolePerm.DeletePerm = true; rowUpdated = true; }
                                                if (!existUserRolePerm.ManagePerm) { existUserRolePerm.ManagePerm = true; rowUpdated = true; }

                                                if (rowUpdated)
                                                {
                                                    existUserRolePerm.Created = tm;
                                                    existUserRolePerm.LastMod = tm;
                                                    existUserRolePerm.CreatedByUserId = vmMod.oCurrUserId_Logged;
                                                    existUserRolePerm.LastModByUserId = vmMod.oCurrUserId_Logged;

                                                    _permCtx.Add(existUserRolePerm);
                                                    rowsUpdated++;
                                                }
                                            }
                                        }

                                        // prompt users
                                        if (rowsAdded > 0)
                                        {
                                            _userTask = "Added " + rowsAdded + " user permissions to [" + oChuAdminRole.RoleType + "] role";
                                            ViewBag.UserMsg += Environment.NewLine + " ~ " + rowsAdded + " user permissions added.";
                                        }
                                        if (rowsUpdated > 0)
                                        {
                                            _userTask = "Updated " + rowsUpdated + " user permissions on [" + oChuAdminRole.RoleType + "] role";
                                            ViewBag.UserMsg += ". " + rowsUpdated + " user permissions updated.";
                                        }

                                        if ((rowsAdded + rowsUpdated) > 0)
                                        {
                                            //save changes... 
                                            await _permCtx.SaveChangesAsync();

                                            DetachAllEntities(_permCtx);

                                            _tm = DateTime.Now;
                                            _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                                              "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vmMod.oCurrUserId_Logged, _tm, _tm, vmMod.oCurrUserId_Logged, vmMod.oCurrUserId_Logged));

                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                          

         

                // oCM_NewConvert.Created = DateTime.Now;
                // _context.Add(_oChanges);

               

                //user roles / user groups and/or user permissions [ tick ... pick from the attendance concept]
                //var oMemRoles = currCtx.MemberChurchRole.Include(t => t.LeaderRole).ThenInclude(t => t.LeaderRoleCategory)
                //    .Where(c => c.LeaderRole.ChurchBodyId == oChuTransf.FromChurchBodyId && c.ChurchMemberId == oCurrChuMember.Id && c.IsCurrServing == true).ToList();
                //foreach (var oMR in oMemRoles)
                //{
                //    oMR.IsCurrServing = false;
                //    oMR.Completed = oChuTransf.TransferDate;
                //    oMR.CompletionReason = oChuTransf.TransferType;
                //    oMR.LastMod = DateTime.Now;
                //    //
                //    currCtx.Update(oMR);
                //}
                //ViewBag.UserMsg += " Church visitor added to church as New Convert successfully. Update of member details may however be required."  


                //save everything
                // await _context.SaveChangesAsync();

                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                

                //if (_oChanges.PwdExpr != null)
                //{
                //    if (_oChanges.PwdExpr.Value >= DateTime.Now.Date)
                //        return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Please user password has expired. Check email/phone for confirm code to activate password.", signOutToLogIn = true  });
                //}

                // return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = false });
                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = false });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Failed saving user profile details. Err: " + ex.Message, signOutToLogIn = false });
            }
        }

        public IActionResult Delete_UP(int? oAppGloOwnId, int? oCurrChuBodyId, int ? loggedUserId, int id, int setIndex, int subSetIndex, bool forceDeleteConfirm = false)
        {
            // var strDesc = setIndex == 1 ? "System profile" : setIndex == 2 ? "Church admin profile" : "Church user profile";
            var strDesc = (setIndex == 1 ? "System admin profile" : (setIndex == 2 ? "Church admin profile" : "Church user profile"));
            var tm = DateTime.Now; var _tm = DateTime.Now; var _userTask = "Attempted saving  " + strDesc;
            //
            try
            {
                var strUserDenom = "Vendor Admin";
                if (setIndex != 1 )
                {
                    if (oAppGloOwnId == null || oCurrChuBodyId == null) 
                        return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Denomination/church of " + strDesc + " unknown. Please refesh and try again." });

                    var oAGO = _context.MSTRAppGlobalOwner.Find(oAppGloOwnId);
                    var oCB = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == oAppGloOwnId && c.Id == oCurrChuBodyId).FirstOrDefault();

                    if (oAGO == null || oCB == null)
                        return Json(new { taskSuccess = false, oCurrId = "", userMess = "Specified denomination and church unit could not be retrieved. Please refresh and try again.", signOutToLogIn = false });

                    strUserDenom = oCB.Name + (!string.IsNullOrEmpty(oAGO.Acronym) ? ", " + oAGO.Acronym : oAGO.OwnerName);
                    strUserDenom = "--" + (string.IsNullOrEmpty(strUserDenom) ? "Denomination: " + strUserDenom : strUserDenom);
                }
                    

                var oUser = _context.UserProfile.Where(c => c.Id == id && (setIndex==1 && oAppGloOwnId == null && oCurrChuBodyId == null) || 
                                            (setIndex != 1 && c.AppGlobalOwnerId==oAppGloOwnId && c.ChurchBodyId==oCurrChuBodyId)).FirstOrDefault();// .Include(c => c.ChurchUnits)
                if (oUser == null)
                {
                    _userTask = "Attempted deleting " + strDesc.ToLower() + ", " + oUser.UserDesc + " [" + oUser.Username + "]" + strUserDenom;  // var _userTask = "Attempted saving  " + strDesc;
                    _tm = DateTime.Now;
                    _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                     "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " to delete could not be retrieved." });
                }

                if (string.Compare(oUser.Username, "sys", true) == 0)
                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " [SYS] cannot be deleted: it's a system profile." });

                if (string.Compare(oUser.Username, "sys", true) == 0)
                    return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = strDesc + " [SYS] cannot be deleted: it's a system profile." });

                var saveDelete = true;
                // ensuring cascade delete where there's none!

                //check oUPRs, UPGs, UATs for this UP to delete
                //var oUPRs = _context.UserProfileRole.Where(c => c.UserProfileId == oUser.Id).ToList();    // .... cascade delete together with the roles, groups / permissions assigned
                //var UPGs = _context.UserProfileGroup.Where(c => c.UserProfileId == oUser.Id).ToList();

                var UATs = _context.UserAuditTrail.Where(c => c.UserProfileId == oUser.Id).ToList();

                using (var _userCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    if (UATs.Count()  > 0) // + UPGs.Count() + oUPRs.Count() //+oUser.ChurchMembers.Count )
                        {
                            if (forceDeleteConfirm == false)
                            {
                                var strConnTabs = "User audit trail";  //User profile role, User profile group and 
                                saveDelete = false;

                                // check user privileges to determine... administrator rights
                                //log...
                                _userTask = "Attempted deleting " + strDesc.ToLower() + ", " + oUser.UserDesc + " [" + oUser.Username + "]" + strUserDenom;
                                _tm = DateTime.Now;
                                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                                 "RCMS-Admin:  " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

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
                            _userCtx.UserProfile.Remove(oUser);
                            _userCtx.SaveChanges();
                                                
                            DetachAllEntities(_userCtx);

                            //audit...
                            _userTask = "Deleted " + strDesc.ToLower() + ", " + oUser.UserDesc + " [" + oUser.Username + "]" + strUserDenom;
                            _tm = DateTime.Now;
                            _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                             "RCMS-Admin:  " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                            return Json(new { taskSuccess = true, tryForceDelete = false, oCurrId = oUser.Id, userMess = strDesc + " successfully deleted." });
                        }

                }

                
                _userTask = "Attempted deleting " + strDesc.ToLower() + "," + oUser.UserDesc + "[" + oUser.Username + "]" + strUserDenom + " -- but FAILED. Data unavailable.";   // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId));

                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "No " + strDesc.ToLower() + " data available to delete. Try again" });
            }

            catch (Exception ex)
            {
                _userTask = "Attempted deleting " + strDesc.ToLower() + ", [ ID= " + id + "] FAILED. ERR: " + ex.Message;  // var _userTask = "Attempted saving " + strDesc;
                _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: " + strDesc, AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, loggedUserId, _tm, _tm, loggedUserId, loggedUserId)); 
                //
                return Json(new { taskSuccess = false, tryForceDelete = false, oCurrId = id, userMess = "Failed deleting " + strDesc.ToLower() + ". Err: " + ex.Message });
            }
        }
         
        [HttpGet]   // public IActionResult AddOrEdit_UP_ChangePwd(int? oAppGloOwnId = null, int? oCurrChuBodyId = null, int? id = 0, int setIndex = 0, int subSetIndex = 0, int? oAGOId_Logged = null, int? oCBId_Logged = null, int? oUserId_Logged = null)  //(int userId = 0, int setIndex = 0) // int? oCurrChuBodyId = null, string profileScope = "C", int setIndex = 0)   // setIndex = 0 (SYS), setIndex = 1 (SUP_ADMN), = 2 (Create/update user), = 3 (reset Pwd) 
        public IActionResult AddOrEdit_UP_ChangePwd(int? id = 0, int setIndex = 0, int? oUserId_Logged = null, int pageIndex = 1)  //(int userId = 0, int setIndex = 0) // int? oCurrChuBodyId = null, string profileScope = "C", int setIndex = 0)   // setIndex = 0 (SYS), setIndex = 1 (SUP_ADMN), = 2 (Create/update user), = 3 (reset Pwd) 
        {
            SetUserLogged();
            if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
            else
            {  
                //
                var oUserResetModel = new ResetUserPasswordModel(); 
                // 1-SYS .. 2-SUP_ADMN, 3-SYS_ADMN, 4-SYS_CUST | 6-CH_ADMN, 7-CF_ADMN
                var proScope = setIndex == 1 ? "V" : "C";
                var subScope = setIndex == 2 ? "D" : setIndex == 3 ? "A" : ""; 
                var _userTask = "Attempted changing user password";
                int? oAppGloOwnId = null; int? oCurrChuBodyId = null;
                int? oAGOId_Logged = null; int? oCBId_Logged = null;
                // 
                var oUser = _context.UserProfile
                         .Where(c => c.Id == id && c.AppGlobalOwnerId == oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId && c.ProfileScope == proScope).FirstOrDefault();

                if (oUser == null) return PartialView("_AddOrEdit_UP_ChangePwd", oUserResetModel);

                // 
                //oCurrVmMod.oChurchBody = oCurrChuBodyLogOn; 
               // oUserResetModel.setIndex = setIndex;
                oUserResetModel.oUserProfile = oUser;
                //
                oUserResetModel.Username = oUser.Username;
                oUserResetModel.CurrentPassword = null;
                oUserResetModel.NewPassword = null;
                oUserResetModel.RepeatPassword = null;
               // oCurrVmMod.SecurityQue = oUser.PwdSecurityQue;
               // oCurrVmMod.SecurityAns = null;
                oUserResetModel.VerificationCode = null; // via email, sms
                oUserResetModel.strLogUserDesc = oUser.UserDesc;
                oUserResetModel.AuthTypeUsed = null;
                  
                //var _oCurrVmMod = oCurrVmMod;
                //TempData["oVmCurrMod"] = _vmMod;
                //TempData.Keep();

                //var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(_oCurrVmMod);
                //TempData["oVmCurrMod"] = _vmMod; TempData.Keep();


                oUserResetModel.setIndex = setIndex;
                oUserResetModel.pageIndex = pageIndex;

               // oUserResetModel.subSetIndex = subSetIndex;
                oUserResetModel.profileScope = proScope;
                oUserResetModel.subScope = subScope;
                //
                oUserResetModel.oCurrUserId_Logged = oUserId_Logged;
                oUserResetModel.oAppGloOwnId_Logged = oAGOId_Logged;
                oUserResetModel.oChurchBodyId_Logged = oCBId_Logged;
                //
                oUserResetModel.oAppGloOwnId = oAppGloOwnId;
                oUserResetModel.oChurchBodyId = oCurrChuBodyId;


                // oUserModel = this.populateLookups_UP_MS(oUserModel, setIndex); 
                oUserResetModel.lkpAuthTypes = new List<SelectListItem>();
                foreach (var dl in dlUserAuthTypes) { oUserResetModel.lkpAuthTypes.Add(new SelectListItem { Value = dl.Val, Text = dl.Desc }); }

                var _oUserResetModel = Newtonsoft.Json.JsonConvert.SerializeObject(oUserResetModel);
                TempData["oVmCurrMod"] = _oUserResetModel; TempData.Keep();

                _userTask = "Modified user profile--change password";
                var _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, oUserId_Logged, _tm, _tm, oUserId_Logged, oUserId_Logged));
                  

                return PartialView("_AddOrEdit_UP_ChangePwd", oUserResetModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit_ChangePwd(ResetUserPasswordModel vmMod)
        {
            UserProfile _oChanges;  // = vmMod .oUserProfile;
            // vmMod = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as ResetUserProfilePwdVM : vmMod; TempData.Keep();

            var arrData = "";
            arrData = TempData.ContainsKey("oVmCurrMod") ? TempData["oVmCurrMod"] as string : arrData;
            vmMod = (!string.IsNullOrEmpty(arrData)) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ResetUserPasswordModel>(arrData) : vmMod;

            var oUP = _context.UserProfile.Where(c => c.Username == vmMod.Username).FirstOrDefault();  // vmMod.oUserProfile;
            oUP.ChurchBody = vmMod.oChurchBody;
             
            try
            {
                ModelState.Remove("oUserProfile.AppGlobalOwnerId");
                ModelState.Remove("oUserProfile.ChurchBodyId");
                ModelState.Remove("oUserProfile.ChurchMemberId");
                ModelState.Remove("oUserProfile.CreatedByUserId");
                ModelState.Remove("oUserProfile.LastModByUserId");
                ModelState.Remove("oUserProfile.OwnerId");

                //finally check error state...
                if (ModelState.IsValid == false)
                    return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "Failed to load the data to save. Please refresh and try again.", signOutToLogIn = false });

                //var tm = DateTime.Now;
                //_oChanges.LastMod = tm;
                //_oChanges.LastModByUserId = vmMod.oCurrLoggedUserId;

                if (vmMod.profileScope != "V" && vmMod.ChurchCode != "000000") // Denomination and ChurchBody cannot be null
                {
                    if (vmMod.oAppGloOwnId == null || vmMod.oChurchBodyId == null)                    
                        return Json(new { taskSuccess = false, oCurrId = "", userMess = "Specify the denomination and church unit", signOutToLogIn = false });

                }

                // get the user profile...
                var userProList = (from t_upx in _context.UserProfile.Where(c => c.AppGlobalOwnerId == vmMod.oAppGloOwnId && c.ChurchBodyId == vmMod.oChurchBodyId &&
                                                                                     c.ProfileScope == vmMod.profileScope && c.Id == oUP.Id)
                                       //  from t_upr in _context.UserProfileRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.UserProfileId == t_upx.Id).DefaultIfEmpty()
                                       //  from t_ur in _context.UserRole.Where(c => c.ChurchBodyId == t_upx.ChurchBodyId && c.Id == t_upr.UserRoleId && c.RoleLevel == 2 && c.RoleType == "SUP_ADMN").DefaultIfEmpty()
                                   select t_upx
                                  ).OrderBy(c => c.UserDesc).ToList();

                if (userProList.Count == 0)
                    return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "User account was not found. Please rfresh and try again.", signOutToLogIn = false });

                if (oUP.Expr != null)
                {
                    if (oUP.Expr.Value >= DateTime.Now.Date)
                        return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "Please user account has expired. Activate account first.", signOutToLogIn = false });
                }
                if (vmMod.NewPassword != null)
                    return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "Please provide user password (minimum 6-digit; use strong passwords:- UPPER and lower cases, digits (0-9) and $pecial characters)", signOutToLogIn = false });

                if (vmMod.NewPassword != vmMod.RepeatPassword)
                    return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "Password mismatch. Provide user password again.", signOutToLogIn = false });

                if (vmMod.AuthTypeUsed == null)
                    return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "Please indicate authentication type to confirm user profile.", signOutToLogIn = false });

                if (vmMod.AuthTypeUsed == 1)  //2-way  ... Compulsory for VENDOR  ... optional for clients
                {
                    if (vmMod.VerificationCode != "12345678") // to be sent to user's email, sms
                        return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "Enter correct verification code.", signOutToLogIn = false });
                }
                else
                {
                    var _secAns = AppUtilties.ComputeSha256Hash(vmMod.SecurityQue + vmMod.SecurityAns);
                    if (vmMod.SecurityQue.ToLower().Equals(vmMod.SecurityQue.ToLower()) && vmMod.SecurityAns.Equals(_secAns))
                        return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "Security answer provided is not correct.", signOutToLogIn = false });
                }


                var _userTask = "Attempted to changed user password.";
                using (var _pwdCtx = new MSTR_DbContext(_context.Database.GetDbConnection().ConnectionString))
                {
                    //create user and init...
                    _oChanges = new UserProfile();
                    //_oChanges.AppGlobalOwnerId = null; // oUP.ChurchBody != null ? oUP.ChurchBody.AppGlobalOwnerId : null;
                    //_oChanges.ChurchBodyId = null; //(int)oUP.ChurchBody.Id;
                    //_oChanges.OwnerId =null; // (int)vmMod.oCurrLoggedUserId;
                     
                    var cc = "";
                    if (_oChanges.AppGlobalOwnerId == null && _oChanges.ChurchBodyId == null && _oChanges.ProfileScope == "V")
                    {
                        cc = "000000";    //var churchCode = "000000"; _oChanges.Username = "SysAdmin"; _oChanges.Pwd = "$ys@dmin1";  
                    }
                    else
                    {
                        var oAGO = _context.MSTRAppGlobalOwner.Find(_oChanges.AppGlobalOwnerId);
                        var oCB = _context.MSTRChurchBody.Where(c => c.AppGlobalOwnerId == _oChanges.AppGlobalOwnerId && c.Id == _oChanges.ChurchBodyId).FirstOrDefault();

                        if (oAGO == null || oCB == null)
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Specified denomination and church unit could not be retrieved. Please refresh and try again.", signOutToLogIn = false });
                        if (string.IsNullOrEmpty(oCB.GlobalChurchCode))
                            return Json(new { taskSuccess = false, oCurrId = _oChanges.Id, userMess = "Church code not specified. The unique global church code of church unit is required. Please verify with System Admin and try again.", signOutToLogIn = false });


                        cc = oCB.GlobalChurchCode;

                        // _oChanges.Pwd = AppUtilties.ComputeSha256Hash(_oChanges.Username + _oChanges.Pwd);
                    }

                    _oChanges.UserKey = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username);
                    _oChanges.Pwd = "123456";  //temp pwd... to reset @ next login  
                    _oChanges.Pwd = AppUtilties.ComputeSha256Hash(cc + _oChanges.Username + _oChanges.Pwd);


                    var tm = DateTime.Now;
                    _oChanges.Strt = tm;
                    //_oChanges.Expr = null; // tm.AddDays(90);  //default to 30 days
                    //  oCurrVmMod.oUserProfile.UserId = oCurrChuMemberId_LogOn;
                    //_oChanges.ChurchMemberId = null; // vmMod.oCurrLoggedMemberId;
                    // _oChanges.UserScope = "E"; // I-internal, E-external
                    //_oChanges.ProfileScope = "V"; // V-Vendor, C-Client

                    _oChanges.ResetPwdOnNextLogOn = false;
                    _oChanges.PwdExpr = tm.AddDays(30);  //default to 90 days 
                    _oChanges.UserStatus = "A"; // A-ctive...D-eactive

                    // _oChanges.Created = tm;
                    _oChanges.LastMod = tm;
                    //  _oChanges.CreatedByUserId = null; // (int)vmMod.oCurrLoggedUserId;
                    _oChanges.LastModByUserId = null; // (int)vmMod.oCurrLoggedUserId;

                  //  _oChanges.Pwd = AppUtilties.ComputeSha256Hash(vmMod.Username + vmMod.NewPassword);
                    //_oChanges.UserDesc = "Super Admin";
                    //_oChanges.UserPhoto = null;
                    //_oChanges.UserId = null;
                    //_oChanges.PhoneNum = null;
                    //_oChanges.Email = null; 

                    // 
                   
                    _userTask = "Changed user password successfully.";
                    ViewBag.UserMsg = "Password changed successfully.";

                    //save everything
                    await _pwdCtx.SaveChangesAsync();


                    DetachAllEntities(_pwdCtx);
                }

               

                //audit...
                var _tm = DateTime.Now;
                _ =  this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "T",
                                 "RCMS-Admin: User Profile", AppUtilties.GetRawTarget(HttpContext.Request), _userTask, _tm, vmMod.oCurrUserId_Logged, _tm, _tm, vmMod.oCurrUserId_Logged, vmMod.oCurrUserId_Logged));


                var _vmMod = Newtonsoft.Json.JsonConvert.SerializeObject(vmMod);
                TempData["oVmCurr"] = _vmMod; TempData.Keep();

                return Json(new { taskSuccess = true, oCurrId = _oChanges.Id, userMess = ViewBag.UserMsg, signOutToLogIn = true });
            }

            catch (Exception ex)
            {
                return Json(new { taskSuccess = false, oCurrId = oUP.Id, userMess = "Failed saving user profile details. Err: " + ex.Message, signOutToLogIn = false });
            }
        }
        
        
    }
}



