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
using RhemaCMS.Models.ViewModels;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using RhemaCMS.Models.MSTRModels;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Controllers.con_adhc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace RhemaCMS.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        // private readonly ChurchModelContext _context;
        // private readonly string _clientDBConnString;

        private readonly MSTR_DbContext _masterContext;
        private readonly ChurchModelContext _clientDBContext;
       
       // private readonly MSTR_DbContext _masterContextLog;
         

        private bool isCurrValid = false;
        private List<UserSessionPrivilege> oUserLogIn_Priv = null;

        public HomeController( MSTR_DbContext masterContext) //ChurchModelContext context ,, ILogger<HomeController> logger )
        {
            // _context = context;
            _masterContext = masterContext;

            //  _masterContextLog = new MSTR_DbContext();
            //  _logger = logger;  
        }


        private bool userAuthorized = false;
        private void SetUserLogged()
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


        //private async Task LogUserActivity_AppMainUserAuditTrail (UserAuditTrail oUserTrail) 
        //{ // var oUserTrail = _masterContext.UserAuditTrail.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null && churchCode=="000000") || (c.AppGlobalOwnerId== oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId))
        //    if (oUserTrail!=null)
        //    { 
        //        _masterContextLog.UserAuditTrail.Add(oUserTrail);
        //        await _masterContextLog.SaveChangesAsync();
        //    }           
        //}


        //private async Task LogUserActivity_AppMainUserAuditTrail(UserAuditTrail oUserTrail)
        //{ // var oUserTrail = _masterContext.UserAuditTrail.Where(c => ((c.AppGlobalOwnerId == null && c.ChurchBodyId == null && churchCode=="000000") || (c.AppGlobalOwnerId== oAppGloOwnId && c.ChurchBodyId == oCurrChuBodyId))
        //    if (oUserTrail != null)
        //    {
        //        using (var logCtx = new MSTR_DbContext())
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

        //private async Task LoadDashboardValues()
        //{ 
        //    using (var dashContext = new MSTR_DbContext())
        //    {
        //        var res = await (from dummyRes in new List<string> { "X" }
        //                   join tago in dashContext.AppGlobalOwner.Where(c => c.Status == "A") on 1 equals 1 into _tago
        //                   join tcb in dashContext.MSTRChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN")) on 1 equals 1 into _tcb
        //                   join tsr in dashContext.UserRole.Where(c => c.RoleStatus == "A" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null) on 1 equals 1 into _tsr
        //                   join tsp in dashContext.UserPermission.Where(c => c.PermStatus == "A") on 1 equals 1 into _tsp
        //                   join tms in dashContext.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A") on 1 equals 1 into _tms
        //                   join tsubs in dashContext.AppSubscription.Where(c => c.Slastatus == "A") on 1 equals 1 into _tsubs
        //                   join ttc in dashContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date) on 1 equals 1 into _ttc
        //                   join tdb in dashContext.ClientAppServerConfig.Select(c => c.DbaseName).Distinct() on 1 equals 1 into _tdb
        //                   join tcln_a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A") on 1 equals 1 into _tcln_a
        //                   join tcln_d in (from a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A")
        //                                   from b in dashContext.UserProfileRole.Where(c => c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN")
        //                                   select a) on 1 equals 1 into _tcln_d
        //                   select new
        //                   {
        //                       cnt_tago = _tago.Count(),
        //                       cnt_tcb = _tcb.Count(),
        //                       cnt_tsr = _tsr.Count(),
        //                       cnt_tsp = _tsp.Count(),
        //                       cnt_tms = _tms.Count(),
        //                       cnt_tsubs = _tsubs.Count(),
        //                       cnt_tdb = _tdb.Count(),
        //                       cnt_ttc = _ttc.Count(),
        //                       cnt_tcln_d = _tcln_d.Count(),
        //                       cnt_tcln_a = _tcln_a.Count()
        //                   })
        //                    .ToList().ToListAsync();

        //        ///
        //        ViewBag.TotalSubsDenom = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tago : 0));  
        //        ViewBag.TotalSubsCong = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcb: 0));   
        //        ViewBag.TotalSysPriv = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsp: 0));   
        //        ViewBag.TotalSysRoles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsr: 0));   
        //        ViewBag.TotSysProfiles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tms: 0));  
        //        ViewBag.TotSubscribers = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsubs: 0));  
        //        ViewBag.TotDbaseCount = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tdb: 0));    
        //        ViewBag.TodaysAuditCount = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_ttc: 0));   
        //        ViewBag.TotClientProfiles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcln_a: 0));   
        //        ViewBag.TotClientProfiles_Admins = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcln_d: 0));   

        //        // Index value... main dashboard...
        //        //
        //        //oHomeDash.TotalSubsDenom = String.Format("{0:N0}", res.cnt_tago); ViewBag.TotalSubsDenom = oHomeDash.TotalSubsDenom;
        //        //oHomeDash.TotalSubsCong = String.Format("{0:N0}", res.cnt_tcb); ViewBag.TotalSubsCong = oHomeDash.TotalSubsCong;
        //        //oHomeDash.TotalSysPriv = String.Format("{0:N0}", res.cnt_tsp); ViewBag.TotalSysPriv = oHomeDash.TotalSysPriv;
        //        //oHomeDash.TotalSysRoles = String.Format("{0:N0}", res.cnt_tsr); ViewBag.TotalSysRoles = oHomeDash.TotalSysRoles;
        //        //oHomeDash.TotSysProfiles = String.Format("{0:N0}", res.cnt_tms); ViewBag.TotSysProfiles = oHomeDash.TotSysProfiles;
        //        //oHomeDash.TotSubscribers = String.Format("{0:N0}", res.cnt_tsubs); ViewBag.TotSubscribers = oHomeDash.TotSubscribers;
        //        //oHomeDash.TotDbaseCount = String.Format("{0:N0}", res.cnt_tdb); ViewBag.TotDbaseCount = oHomeDash.TotDbaseCount;
        //        //oHomeDash.TodaysAuditCount = String.Format("{0:N0}", res.cnt_ttc); ViewBag.TodaysAuditCount = oHomeDash.TodaysAuditCount;
        //        //oHomeDash.TotClientProfiles = String.Format("{0:N0}", res.cnt_tcln_a); ViewBag.TotClientProfiles = oHomeDash.TotClientProfiles;
        //        //oHomeDash.TotClientProfiles_Admins = String.Format("{0:N0}", res.cnt_tcln_d); ViewBag.TotClientProfiles_Admins = oHomeDash.TotClientProfiles_Admins;
        //    }

        //}


        private async Task LoadVendorDashboardValues()
        {
            // using (var dashContext = new MSTR_DbContext())
            using (var dashContext = new MSTR_DbContext(_masterContext.Database.GetDbConnection().ConnectionString)) // ("Server=RHEMA-SDARTEH;Database=DBRCMS_MS_TEST;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true") ) // AppUtilties.GetNewDBContext_MS(_context, "DBRCMS_CL_TEST"))  // MSTR_DbContext()) //
            {
                if (dashContext.Database.CanConnect() == false) dashContext.Database.OpenConnection();
                else if (dashContext.Database.GetDbConnection().State != System.Data.ConnectionState.Open) dashContext.Database.OpenConnection();

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



                // close connection
                dashContext.Database.CloseConnection();
            }



            //ViewData["TotalSubsDenom"] = String.Format("{0:N0}", 0);
            //ViewData["TotalSubsCong"] = String.Format("{0:N0}", 0);
            //ViewData["TotalSysPriv"] = String.Format("{0:N0}", 0);
            //ViewData["TotalSysRoles"] = String.Format("{0:N0}", 0);
            //ViewData["TotSysProfiles"] = String.Format("{0:N0}", 0);
            //ViewData["TotSubscribers"] = String.Format("{0:N0}", 0);
            //ViewData["TotDbaseCount"] = String.Format("{0:N0}", 0);
            //ViewData["TodaysAuditCount"] = String.Format("{0:N0}", 0);
            //ViewData["TotClientProfiles"] = String.Format("{0:N0}", 0);
            //ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}", 0);

            //using (var dashContext = new MSTR_DbContext())
            //{
            //    var res = await (from dummyRes in new List<string> { "X" }
            //                     join tago in dashContext.AppGlobalOwner.Where(c => c.Status == "A") on 1 equals 1 into _tago
            //                     join tcb in dashContext.MSTRChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN")) on 1 equals 1 into _tcb
            //                     join tsr in dashContext.UserRole.Where(c => c.RoleStatus == "A" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null) on 1 equals 1 into _tsr
            //                     join tsp in dashContext.UserPermission.Where(c => c.PermStatus == "A") on 1 equals 1 into _tsp
            //                     join tms in dashContext.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A") on 1 equals 1 into _tms
            //                     join tsubs in dashContext.AppSubscription.Where(c => c.Slastatus == "A") on 1 equals 1 into _tsubs
            //                     join ttc in dashContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date) on 1 equals 1 into _ttc
            //                     join tdb in dashContext.ClientAppServerConfig.Select(c => c.DbaseName).Distinct() on 1 equals 1 into _tdb
            //                     join tcln_a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A") on 1 equals 1 into _tcln_a
            //                     join tcln_d in (from a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A")
            //                                     from b in dashContext.UserProfileRole.Where(c => c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN")
            //                                     select a) on 1 equals 1 into _tcln_d
            //                     select new
            //                     {
            //                         cnt_tago = _tago.Count(),
            //                         cnt_tcb = _tcb.Count(),
            //                         cnt_tsr = _tsr.Count(),
            //                         cnt_tsp = _tsp.Count(),
            //                         cnt_tms = _tms.Count(),
            //                         cnt_tsubs = _tsubs.Count(),
            //                         cnt_tdb = _tdb.Count(),
            //                         cnt_ttc = _ttc.Count(),
            //                         cnt_tcln_d = _tcln_d.Count(),
            //                         cnt_tcln_a = _tcln_a.Count()
            //                     })
            //                .ToList().ToListAsync();

            //    ///
            //    if (res.Count > 0)
            //    {
            //        ViewData["TotalSubsDenom"] = String.Format("{0:N0}", res[0].cnt_tago );
            //        ViewData["TotalSubsCong"] = String.Format("{0:N0}", res[0].cnt_tcb );
            //        ViewData["TotalSysPriv"] = String.Format("{0:N0}", res[0].cnt_tsp );
            //        ViewData["TotalSysRoles"] = String.Format("{0:N0}", res[0].cnt_tsr );
            //        ViewData["TotSysProfiles"] = String.Format("{0:N0}", res[0].cnt_tms );
            //        ViewData["TotSubscribers"] = String.Format("{0:N0}", res[0].cnt_tsubs );
            //        ViewData["TotDbaseCount"] = String.Format("{0:N0}", res[0].cnt_tdb );
            //        ViewData["TodaysAuditCount"] = String.Format("{0:N0}", res[0].cnt_ttc );
            //        ViewData["TotClientProfiles"] = String.Format("{0:N0}", res[0].cnt_tcln_a );
            //        ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}",  res[0].cnt_tcln_d );
            //    }

            //    else
            //    {
            //        ViewData["TotalSubsDenom"] = String.Format("{0:N0}", 0);
            //        ViewData["TotalSubsCong"] = String.Format("{0:N0}", 0);
            //        ViewData["TotalSysPriv"] = String.Format("{0:N0}", 0);
            //        ViewData["TotalSysRoles"] = String.Format("{0:N0}", 0);
            //        ViewData["TotSysProfiles"] = String.Format("{0:N0}", 0);
            //        ViewData["TotSubscribers"] = String.Format("{0:N0}", 0);
            //        ViewData["TotDbaseCount"] = String.Format("{0:N0}", 0);
            //        ViewData["TodaysAuditCount"] = String.Format("{0:N0}", 0);
            //        ViewData["TotClientProfiles"] = String.Format("{0:N0}", 0);
            //        ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}", 0);
            //    } 
            //}
        }    
        
        private async Task LoadClientDashboardValues(string clientDBConnString, UserProfile oLoggedUser)
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
                var clientCB = clientContext.ChurchBody.Where(c => c.MSTR_AppGlobalOwnerId == oLoggedUser.AppGlobalOwnerId && c.MSTR_ChurchBodyId==oLoggedUser.ChurchBodyId && c.Status == "A").FirstOrDefault();
                ///
                var qrySuccess = false;
                if (clientAGO != null && clientCB != null)
                {
                    var res = await (from dummyRes in new List<string> { "X" }
                                     join tcb_sb in clientContext.ChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN") &&
                                                         c.AppGlobalOwnerId== clientAGO.Id && c.ParentChurchBodyId == clientCB.Id) on 1 equals 1 into _tcb_sb
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
                            .ToList().ToListAsync();

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

                if(!qrySuccess)
                {
                    ViewData["CB_SubCongCount"] = String.Format("{0:N0}", 0);
                    ViewData["CB_MemListCount"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_NewMemListCount"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_NewConvertsCount"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_VisitorsCount"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_ReceiptsAmt"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_PaymentsAmt"] = String.Format("{0:N0}", 0);
                    ///
                    ViewData["Today_AuditCount"] = String.Format("{0:N0}", 0);
                }

                // close connection
                clientContext.Database.CloseConnection();
            } 
        }


        private async Task<HomeDashboardVM> LoadClientDashboardValues(HomeDashboardVM oHomeDash, string clientDBConnString, UserProfile oLoggedUser)
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
                var clientCB = clientContext.ChurchBody.Where(c => c.MSTR_AppGlobalOwnerId == oLoggedUser.AppGlobalOwnerId && c.MSTR_ChurchBodyId==oLoggedUser.ChurchBodyId && c.Status == "A").FirstOrDefault();
                ///
                var qrySuccess = false;
                if (clientAGO != null && clientCB != null)
                {
                    var res = await (from dummyRes in new List<string> { "X" }
                                     join tcb_sb in clientContext.ChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN") &&
                                                         c.AppGlobalOwnerId== clientAGO.Id && c.ParentChurchBodyId == clientCB.Id) on 1 equals 1 into _tcb_sb
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
                            .ToList().ToListAsync();

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

                if(!qrySuccess)
                {
                    ViewData["CB_SubCongCount"] = String.Format("{0:N0}", 0);
                    ViewData["CB_MemListCount"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_NewMemListCount"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_NewConvertsCount"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_VisitorsCount"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_ReceiptsAmt"] = String.Format("{0:N0}", 0);
                    ViewData["CBWeek_PaymentsAmt"] = String.Format("{0:N0}", 0);
                    ///
                    ViewData["Today_AuditCount"] = String.Format("{0:N0}", 0);
                }

                /// initialize Model values
                oHomeDash.strCB_SubCongCount = ViewData["CB_SubCongCount"] as string;
                oHomeDash.strCB_MemListCount = ViewData["CB_MemListCount"] as string;
                oHomeDash.strCBWeek_NewMemListCount = ViewData["CBWeek_NewMemListCount"] as string;
                oHomeDash.strCBWeek_NewConvertsCount = ViewData["CBWeek_NewConvertsCount"] as string;
                oHomeDash.strCBWeek_VisitorsCount = ViewData["CBWeek_VisitorsCount"] as string;
                oHomeDash.strCBWeek_ReceiptsAmt = ViewData["CBWeek_ReceiptsAmt"] as string;
                oHomeDash.strCBWeek_PaymentsAmt = ViewData["CBWeek_PaymentsAmt"] as string;
                oHomeDash.strTodaysAuditCount = ViewData["TodaysAuditCount"] as string;
                oHomeDash.strCB_CurrUsed = ViewData["CB_CurrUsed"] as string;
                
                // close connection
                clientContext.Database.CloseConnection();
            }

            return oHomeDash;
        }



        public async Task<IActionResult> Index()
        {
            // init the client DB            
            SetUserLogged();
            if (!isCurrValid) { ViewData["strUserLoginFailMess"] = "Client user profile validation unsuccessful."; return RedirectToAction("LoginUserAcc", "UserLogin"); }
            else
            {
                if (oUserLogIn_Priv[0].UserProfile == null)
                { ViewData["strUserLoginFailMess"] = "Client user profile not found. Please try again or contact System Admin"; return RedirectToAction("LoginUserAcc", "UserLogin"); }


                // check permission 
                var _oUserPrivilegeCol = oUserLogIn_Priv;
                var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();

                //
                //if (!this.userAuthorized) return View(new HomeDashboardVM()); //retain view    
                //if (oUserLogIn_Priv[0] == null) return View(new HomeDashboardVM());

                //  if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner != null || oUserLogIn_Priv[0].ChurchBody != null) return View(new HomeDashboardVM());
                // var oLoggedUser = oUserLogIn_Priv[0].UserProfile;

                //var oLoggedRole = oUserLogIn_Priv[0].UserRole;
                ////
                //var oUserId_Logged = oLoggedUser.Id;
                //var oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody;
                //int? oAppGloOwnId_Logged = null; int? oChuBodyId_Logged = null;
                //if (oChuBody_Logged != null)
                //{
                //    oAppGloOwnId_Logged = oChuBody_Logged.AppGlobalOwnerId;
                //    oChuBodyId_Logged = oChuBody_Logged.Id;
                //}


                //// Get the client database details.... db connection string
                //var _clientDBConnString = "";
                //var conn = new SqlConnectionStringBuilder(_masterContext.Database.GetDbConnection().ConnectionString);
                ////  "DefaultConnection": "Server=RHEMA-SDARTEH;Database=DBRCMS_MS_DEV;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true"
                ////conn.DataSource = "RHEMA-SDARTEH"; conn.InitialCatalog = "DBRCMS_CL_TEST"; conn.UserID = "sa"; conn.Password = "sadmin"; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;

                //var oClientConfig = _masterContext.ClientAppServerConfig.Where(c => c.AppGlobalOwnerId == oLoggedUser.AppGlobalOwnerId && c.Status == "A").FirstOrDefault();
                //if (oClientConfig == null) { ViewData["strUserLoginFailMess"] = "Client database details not found. Please try again or contact System Admin"; return RedirectToAction("LoginUserAcc", "UserLogin"); }
                /////
                //conn.DataSource = oClientConfig.ServerName; conn.InitialCatalog = oClientConfig.DbaseName; conn.UserID = oClientConfig.SvrUserId; conn.Password = oClientConfig.SvrPassword; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
                //_clientDBConnString = conn.ConnectionString;

                //// test the NEW DB conn
                //var _context = new ChurchModelContext(_clientDBConnString = conn.ConnectionString);
                //if (!_context.Database.CanConnect()) { ViewData["strUserLoginFailMess"] = "Failed to connect client database. Please try again or contact System Admin"; return RedirectToAction("LoginUserAcc", "UserLogin"); }  // give appropriate user prompts

                //// connection SUCESS! initialize main context... this._context  -->>> to inject subsequent instances.  REM: not using the Setup otopn now. Setup only connects the Master DB
                //// this._context = testConn;



                //SetUserLogged();
                //if (!isCurrValid) return RedirectToAction("LoginUserAcc", "UserLogin");
                //else
                //{
                //    // check permission 
                //    var _oUserPrivilegeCol = oUserLogIn_Priv;
                //    var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                //    TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();
                //    //
                //    // check permission 
                //    if (!this.userAuthorized) return View(new HomeDashboardVM()); //retain view    
                //    if (oUserLogIn_Priv[0] == null) return View(new HomeDashboardVM());
                //    if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner == null || oUserLogIn_Priv[0].ChurchBody == null) return View(new HomeDashboardVM()); 
                //    var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                //    var oChuBodyLogOn = oUserLogIn_Priv[0].ChurchBody;
                //    var oAppGloOwn = oUserLogIn_Priv[0].AppGlobalOwner;


                ///                                
                //Actually... loggedUser has already been authenticated @Login -->> exists, subscribed, active etc.
                ///
                var oHomeDash = new HomeDashboardVM();
                var oLoggedRole = oUserLogIn_Priv[0].UserRole;
                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                var oLoggedCB = oUserLogIn_Priv[0].ChurchBody;
                var oAppGloOwn = _masterContext.MSTRAppGlobalOwner.Find(oLoggedUser.AppGlobalOwnerId);
                var oChuBodyLogOn = _masterContext.MSTRChurchBody.AsNoTracking().Include(t => t.ChurchLevel)
                                            .Where(c => c.AppGlobalOwnerId == oLoggedUser.AppGlobalOwnerId && c.Id == oLoggedUser.ChurchBodyId).FirstOrDefault();

                ///
                ViewData["strAppName"] = "RhemaCMS";
                ViewData["strAppNameMod"] = "Church Dashboard";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(oLoggedUser.UserDesc) ? oLoggedUser.UserDesc : "[Current user]";
                ///
                ViewData["oAppGloOwnId_Logged"] = oLoggedUser.AppGlobalOwnerId;
                ViewData["oChurchBodyId_Logged"] = oLoggedUser.ChurchBodyId;
                ViewData["oChurchBodyOrgType_Logged"] = oLoggedCB.OrganisationType;
                ViewData["strAppCurrUser_ChRole"] = oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUserPhoto_Filename"] = oLoggedUser.UserPhoto;
                ///
                ViewData["strClientLogo_Filename"] = oAppGloOwn?.ChurchLogo;
                ViewData["strAppLogo_Filename"] = "~/frontend/dist/img/rhema_logo.png"; // oAppGloOwn?.ChurchLogo;
                ViewData["strClientChurchName"] = oAppGloOwn.OwnerName;
                ViewData["strClientBranchName"] = oChuBodyLogOn.Name;
                ViewData["strClientChurchLevel"] = !string.IsNullOrEmpty(oChuBodyLogOn.ChurchLevel?.CustomName) ? oChuBodyLogOn.ChurchLevel?.CustomName : oChuBodyLogOn.ChurchLevel?.Name;  // Assembly, Presbytery etc



                var _clientCon = GetClientDBContext(oLoggedUser);
                if (_clientCon == null)
                {
                    ViewData["strUserLoginFailMess"] = "Client database connection unsuccessful. Please try again or contact System Admin";
                    // return RedirectToAction("LoginUserAcc", "UserLogin"); 
                    ModelState.AddModelError("", "Client database connection unsuccessful. Please try again or contact System Admin");
                    return View(oHomeDash);
                }


                var strClientConn = _clientCon.Database.GetDbConnection().ConnectionString;

                //successfull login... audit!
                var tm = DateTime.Now;
                //this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(oUserPrivilegeCol[0].AppGlobalOwner != null ? oUserPrivilegeCol[0].AppGlobalOwner.Id : (int?)null, oUserPrivilegeCol[0].ChurchBody != null ? oUserPrivilegeCol[0].ChurchBody.Id : (int?)null,
                //                "L", null, null, "Logged in successfully into RHEMA-CMS", tm, oUserPrivilegeCol[0].UserProfile.Id, tm, tm, oUserPrivilegeCol[0].UserProfile.Id, oUserPrivilegeCol[0].UserProfile.Id));

                // record... @vendor...
                _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, oLoggedUser.AppGlobalOwnerId, oLoggedUser.ChurchBodyId, "N",
                                 "RCMS-Client: Home-Dashboard", AppUtilties.GetRawTarget(HttpContext.Request), "Launched into Home-Dashboard", tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id));

                // record ... @client
                _ = this.LogUserActivity_ClientUserAuditTrail(new UserAuditTrail_CL(0, oLoggedUser.AppGlobalOwnerId, oLoggedUser.ChurchBodyId, "N",
                                 "RCMS-Client: Home-Dashboard", AppUtilties.GetRawTarget(HttpContext.Request), "Launched into Home-Dashboard", tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id)
                                , strClientConn);


                // refreshValues...
                oHomeDash = await LoadClientDashboardValues(oHomeDash, strClientConn, oLoggedUser);



                //int? oAppGloOwnId_Logged = null;
                //string strChuBodyDenom_Logged = "";
                //int? oChuBodyId_Logged = null;
                //int? oUserId_Logged = null;
                //string strChuBodyType_Logged = "";                
                //string strChuBodyCong_Logged = "";
                //string strUserDesc_Logged = "";
                //string strUserPhoto_Logged = ""; 

                //if (oChuBodyLogOn != null)
                //{
                //    oAppGloOwnId_Logged = oChuBodyLogOn.AppGlobalOwnerId;
                //    strChuBodyDenom_Logged =  oAppGloOwn?.OwnerName;// "The Church of Pentecost";
                //    oChuBodyId_Logged = oChuBodyLogOn.Id;
                //    strChuBodyType_Logged = oChuBodyLogOn.OrganisationType.ToUpper();  //ChurchType.ToUpper();
                //    strChuBodyCong_Logged =  oChuBodyLogOn.Name; //  "La Assembly, Accra";                 
                //    oUserId_Logged = oLoggedUser.Id;
                //    strUserDesc_Logged = oLoggedUser.UserDesc; // "Apostle Sam Darteh";
                //    strUserPhoto_Logged = oLoggedUser.UserPhoto;//"2020_dev_sam.jpg"; 

                //    //if (oCurrChuBodyId == null) { oCurrChuBodyId = oChuBodyLogOn.Id; }
                //    //if (oAppGloOwnId == null) { oAppGloOwnId = oChuBodyLogOn.AppGlobalOwnerId; }
                //    //else if (oCurrChuBodyId != oCurrChuBodyLogOn.Id) oCurrChuBodyId = oCurrChuBodyLogOn.Id;  //reset to logon...
                //    //
                //    // oAppGloOwnId = oCurrChuBodyLogOn.AppGlobalOwnerId;
                //}


                //int? oCurrChuMemberId_LogOn = null;
                //ChurchMember oCurrChuMember_LogOn = null;

                //var currChurchMemberLogged = _context.ChurchMember.Where(c => c.ChurchBodyId == oChuBodyId_Logged && c.Id == oLoggedUser.ChurchMemberId).FirstOrDefault();
                //if (currChurchMemberLogged != null) //return View(oCurrMdl);
                //{
                //    oCurrChuMemberId_LogOn = currChurchMemberLogged.Id;
                //    oCurrChuMember_LogOn = currChurchMemberLogged;
                //}



                //oHomeDash.oAppGlolOwnId = oAppGloOwnId_Logged; ViewBag.oAppGloOwnId_Logged = oHomeDash.oAppGloOwnId_Logged;
                //oHomeDash.oChurchBodyId = oChuBodyId_Logged; ViewBag.oChuBodyId_Logged = oHomeDash.oChurchBodyId;
                ////
                //oHomeDash.oUserId_Logged = oUserId_Logged; ViewBag.oUserId_Logged = oHomeDash.oUserId_Logged;
                //oHomeDash.oChurchBody_Logged = oChuBodyLogOn; ViewBag.oChurchBody_Logged = oHomeDash.oChurchBody_Logged;
                //oHomeDash.oAppGloOwnId_Logged = oAppGloOwnId_Logged; ViewBag.oAppGloOwnId_Logged = oHomeDash.oAppGloOwnId_Logged;
                //oHomeDash.oMemberId_Logged = oCurrChuMemberId_LogOn; ViewBag.oMemberId_Logged = oHomeDash.oMemberId_Logged;

                ////
                ////
                //oHomeDash.strChurchLevelDown = "Assemblies";
                //oHomeDash.strAppName = "RhemaCMS"; ViewBag.strAppName = oHomeDash.strAppName;
                //oHomeDash.strAppCurrUser = strUserDesc_Logged; ViewBag.strAppCurrUser = oHomeDash.strAppCurrUser;
                //oHomeDash.strChurchType = strChuBodyType_Logged; ViewBag.strChurchType = oHomeDash.strChurchType;//"CH"
                //oHomeDash.strChuBodyDenomLogged =  strChuBodyDenom_Logged; ViewBag.strChuBodyDenomLogged = oHomeDash.strChuBodyDenomLogged;//"Rhema Global Church"
                //oHomeDash.strChuBodyLogged =  strChuBodyCong_Logged; ViewBag.strChuBodyLogged = oHomeDash.strChuBodyLogged;//"Rhema Comm Chapel"

                ////           
                //ViewBag.strAppCurrUser_ChRole = "Pastor-in-Charge";
                //ViewBag.strAppCurrUser_RoleCateg = "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                //ViewBag.strAppCurrUser_PhotoFilename = strUserPhoto_Logged; //   "2020_dev_sam.jpg"; //
                //ViewBag.strAppCurrChu_LogoFilename = "14dc86a7-81ae-462c-b73e-4581bd4ee2b2_church-of-pentecost.png";
                //ViewBag.strUserSessionDura = "Logged: 1 hour ago";


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


                // var ls = _context.ChurchBody.ToList();
                //var _oUserPrivilegeCol = oUserLogIn_Priv;
                //var privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                //TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();

                return View(oHomeDash);
            }
            
        }
        

        public async Task<IActionResult> Index_sa()
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
                if (!this.userAuthorized) return View(new HomeDashboardVM()); //retain view    
                if (oUserLogIn_Priv[0] == null) return View(new HomeDashboardVM());
                if (oUserLogIn_Priv[0].UserProfile == null || oUserLogIn_Priv[0].AppGlobalOwner != null || oUserLogIn_Priv[0].ChurchBody != null) return View(new HomeDashboardVM());
                var oLoggedUser = oUserLogIn_Priv[0].UserProfile;
                var oLoggedRole = oUserLogIn_Priv[0].UserRole;
               
               // var oUserId_Logged = oLoggedUser.Id;
               // var oChuBody_Logged = oUserLogIn_Priv[0].ChurchBody;
               //// int? oAppGloOwnId_Logged = null; int? oChuBodyId_Logged = null;
               // if (oChuBody_Logged != null)
               // {
               //     oAppGloOwnId_Logged = oChuBody_Logged.AppGlobalOwnerId;
               //     oChuBodyId_Logged = oChuBody_Logged.Id;
               // }
                 
                
                //  successfull login... audit!
                var tm = DateTime.Now;

                _ = this.LogUserActivity_AppMainUserAuditTrail(new UserAuditTrail(0, null, null, "N",
                                 "RCMS-Admin: Home-Dashboard", AppUtilties.GetRawTarget(HttpContext.Request), "Launched into Admin Palette Home-Dashboard", tm, oLoggedUser.Id, tm, tm, oLoggedUser.Id, oLoggedUser.Id));


                var oHomeDashAdmin = new HomeDashboardVM();
                //  oHomeDashAdmin.strChurchLevelDown = "Assemblies"; 
                ///
                ViewData["strAppName"] = "RhemaCMS";
                ViewData["strAppNameMod"] = "Admin Palette";
                ViewData["strAppCurrUser"] = !string.IsNullOrEmpty(oLoggedUser.UserDesc) ? oLoggedUser.UserDesc : "[Current user]";
                ///
                ViewData["oAppGloOwnId_Logged"] = (int?)null;
                ViewData["oChuBodyId_Logged"] = (int?)null;
                ViewData["strAppCurrUser_ChRole"] = oLoggedRole.RoleDesc; // "System Adminitrator";
                ViewData["strAppCurrUser_RoleCateg"] = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                ViewData["strAppCurrUser_PhotoFilename"] = oLoggedUser.UserPhoto;
                ///
                //ViewData["strAppCurrChu_LogoFilename"] = oLoggedUser.UserPhoto;
                //ViewData["strChuBodyDenomLogged"] = oLoggedUser.UserPhoto;
                //ViewData["strChuBodyLogged"] = oLoggedUser.UserPhoto;
                ///

                //oHomeDashAdmin.strAppName = "RhemaCMS"; ViewBag.strAppName = oHomeDashAdmin.strAppName;
                //oHomeDashAdmin.strAppNameMod = "Admin Palette"; ViewBag.strAppNameMod = oHomeDashAdmin.strAppNameMod;
                //oHomeDashAdmin.strAppCurrUser = oLoggedUser.UserDesc; ViewBag.strAppCurrUser = oHomeDashAdmin.strAppCurrUser;  // "Dan Abrokwa"
                //                                                                                                     //oHomeDashAdmin.strChurchType = "CH"; ViewBag.strChurchType = oHomeDashAdmin.strChurchType;
                //oHomeDashAdmin.strChuBodyDenomLogged = "Rhema Global Church"; ViewBag.strChuBodyDenomLogged = oHomeDashAdmin.strChuBodyDenomLogged;
                //oHomeDashAdmin.strChuBodyLogged = "Rhema Comm Chapel"; ViewBag.strChuBodyLogged = oHomeDashAdmin.strChuBodyLogged;

                //           
                //ViewBag.oAppGloOwnId_Logged = oAppGloOwnId_Logged;
                //ViewBag.oChuBodyId_Logged = oChuBodyId_Logged;
                //ViewBag.strAppCurrUser_ChRole = oLoggedRole.RoleDesc; // "System Adminitrator";
                //ViewBag.strAppCurrUser_RoleCateg = oLoggedRole.RoleName; // "SUP_ADMN";  // CH_ADMN | CF_ADMN | CH_RGTR | CF_RGTR | CH_ACCT | CF_ACCT | CH_CUST | CH_CUST
                //ViewBag.strAppCurrUser_PhotoFilename = oLoggedUser.UserPhoto; // "2020_dev_sam.jpg";
                // ViewBag.strAppCurrChu_LogoFilename = "14dc86a7-81ae-462c-b73e-4581bd4ee2b2_church-of-pentecost.png";
                // ViewBag.strUserSessionDura = "Logged: 10 minutes ago";

              
                
                // refreshValues...
                await LoadVendorDashboardValues();


                //ViewData["TotalSubsDenom"] = String.Format("{0:N0}", 0);
                //ViewData["TotalSubsCong"] = String.Format("{0:N0}", 0);
                //ViewData["TotalSysPriv"] = String.Format("{0:N0}", 0);
                //ViewData["TotalSysRoles"] = String.Format("{0:N0}", 0);
                //ViewData["TotSysProfiles"] = String.Format("{0:N0}", 0);
                //ViewData["TotSubscribers"] = String.Format("{0:N0}", 0);
                //ViewData["TotDbaseCount"] = String.Format("{0:N0}", 0);
                //ViewData["TodaysAuditCount"] = String.Format("{0:N0}", 0);
                //ViewData["TotClientProfiles"] = String.Format("{0:N0}", 0);
                //ViewData["TotClientProfiles_Admins"] = String.Format("{0:N0}", 0);


                //using (var dashContext = new MSTR_DbContext())
                //{
                //    var res = await (from dummyRes in new List<string> { "X" }
                //                     join tago in dashContext.AppGlobalOwner.Where(c => c.Status == "A") on 1 equals 1 into _tago
                //                     join tcb in dashContext.MSTRChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN")) on 1 equals 1 into _tcb
                //                     join tsr in dashContext.UserRole.Where(c => c.RoleStatus == "A" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null) on 1 equals 1 into _tsr
                //                     join tsp in dashContext.UserPermission.Where(c => c.PermStatus == "A") on 1 equals 1 into _tsp
                //                     join tms in dashContext.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A") on 1 equals 1 into _tms
                //                     join tsubs in dashContext.AppSubscription.Where(c => c.Slastatus == "A") on 1 equals 1 into _tsubs
                //                     join ttc in dashContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date) on 1 equals 1 into _ttc
                //                     join tdb in dashContext.ClientAppServerConfig.Select(c => c.DbaseName).Distinct() on 1 equals 1 into _tdb
                //                     join tcln_a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A") on 1 equals 1 into _tcln_a
                //                     join tcln_d in (from a in dashContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A")
                //                                           from b in dashContext.UserProfileRole.Where(c => c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN")
                //                                           select a) on 1 equals 1 into _tcln_d
                //                     select new
                //                     {
                //                         cnt_tago = _tago.Count(),
                //                         cnt_tcb = _tcb.Count(),
                //                         cnt_tsr = _tsr.Count(),
                //                         cnt_tsp = _tsp.Count(),
                //                         cnt_tms = _tms.Count(),
                //                         cnt_tsubs = _tsubs.Count(),
                //                         cnt_tdb = _tdb.Count(),
                //                         cnt_ttc = _ttc.Count(),
                //                         cnt_tcln_d = _tcln_d.Count(),
                //                         cnt_tcln_a = _tcln_a.Count()
                //                     })
                //                .ToList().ToListAsync();

                //    // Index value... main dashboard included [model]...  
                //    ViewBag.TotalSubsDenom = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tago : 0));
                //    ViewBag.TotalSubsCong = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcb : 0));
                //    ViewBag.TotalSysPriv = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsp : 0));
                //    ViewBag.TotalSysRoles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsr : 0));
                //    ViewBag.TotSysProfiles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tms : 0));
                //    ViewBag.TotSubscribers = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsubs : 0));
                //    ViewBag.TotDbaseCount = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tdb : 0));
                //    ViewBag.TodaysAuditCount = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_ttc : 0));
                //    ViewBag.TotClientProfiles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcln_a : 0));
                //    ViewBag.TotClientProfiles_Admins = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcln_d : 0));

                //    oHomeDashAdmin.TotalSubsDenom = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tago : 0)); ViewBag.TotalSubsDenom = oHomeDashAdmin.TotalSubsDenom;
                //    oHomeDashAdmin.TotalSubsCong = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcb : 0)); ViewBag.TotalSubsCong = oHomeDashAdmin.TotalSubsCong;
                //    oHomeDashAdmin.TotalSysPriv = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsp : 0)); ViewBag.TotalSysPriv = oHomeDashAdmin.TotalSysPriv;
                //    oHomeDashAdmin.TotalSysRoles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsr : 0)); ViewBag.TotalSysRoles = oHomeDashAdmin.TotalSysRoles;
                //    oHomeDashAdmin.TotSysProfiles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tms : 0)); ViewBag.TotSysProfiles = oHomeDashAdmin.TotSysProfiles;
                //    oHomeDashAdmin.TotSubscribers = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tsubs : 0)); ViewBag.TotSubscribers = oHomeDashAdmin.TotSubscribers;
                //    oHomeDashAdmin.TotDbaseCount = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tdb : 0)); ViewBag.TotDbaseCount = oHomeDashAdmin.TotDbaseCount;
                //    oHomeDashAdmin.TodaysAuditCount = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_ttc : 0)); ViewBag.TodaysAuditCount = oHomeDashAdmin.TodaysAuditCount;
                //    oHomeDashAdmin.TotClientProfiles = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcln_a : 0)); ViewBag.TotClientProfiles = oHomeDashAdmin.TotClientProfiles;
                //    oHomeDashAdmin.TotClientProfiles_Admins = String.Format("{0:N0}", (res.Count > 0 ? res[0].cnt_tcln_d : 0)); ViewBag.TotClientProfiles_Admins = oHomeDashAdmin.TotClientProfiles_Admins;
                //}


                //oHomeDashAdmin.TotalSubsDenom = String.Format("{0:N0}", 0); ViewBag.TotalSubsDenom = oHomeDashAdmin.TotalSubsDenom;
                //oHomeDashAdmin.TotalSubsCong = String.Format("{0:N0}", 0); ViewBag.TotalSubsCong = oHomeDashAdmin.TotalSubsCong;
                //oHomeDashAdmin.TotalSysPriv = String.Format("{0:N0}", 0); ViewBag.TotalSysPriv = oHomeDashAdmin.TotalSysPriv;
                //oHomeDashAdmin.TotalSysRoles = String.Format("{0:N0}", 0); ViewBag.TotalSysRoles = oHomeDashAdmin.TotalSysRoles;
                //oHomeDashAdmin.TotSysProfiles = String.Format("{0:N0}", 0); ViewBag.TotSysProfiles = oHomeDashAdmin.TotSysProfiles;
                //oHomeDashAdmin.TotSubscribers = String.Format("{0:N0}", 0); ViewBag.TotSubscribers = oHomeDashAdmin.TotSubscribers;
                //oHomeDashAdmin.TotDbaseCount = String.Format("{0:N0}", 0); ViewBag.TotDbaseCount = oHomeDashAdmin.TotDbaseCount;
                //oHomeDashAdmin.TodaysAuditCount = String.Format("{0:N0}", 0); ViewBag.TodaysAuditCount = oHomeDashAdmin.TodaysAuditCount;
                //oHomeDashAdmin.TotClientProfiles = String.Format("{0:N0}", 0); ViewBag.TotClientProfiles = oHomeDashAdmin.TotClientProfiles;
                //oHomeDashAdmin.TotClientProfiles_Admins = String.Format("{0:N0}", 0); ViewBag.TotClientProfiles_Admins = oHomeDashAdmin.TotClientProfiles_Admins;


                // var ls = _context.ChurchBody.ToList();
                //_oUserPrivilegeCol = oUserLogIn_Priv;
                //privList = Newtonsoft.Json.JsonConvert.SerializeObject(_oUserPrivilegeCol);
                //TempData["UserLogIn_oUserPrivCol"] = privList; TempData.Keep();


                TempData.Keep();

                ///
                return View(oHomeDashAdmin);
            }                         
        }


        //public ActionResult ReRouteDbase()
        //{
        //    string[] args = new string[] { };
        //    Program.CreateHostBuilder2(args).Build().Run();
        //}


        //public  async Task<IActionResult> GetDashboardValues()
        //{
            //var res = new
            //{
            //    tago = await _masterContext.AppGlobalOwner.Where(c => c.Status == "A").ToListAsync(),
            //    tcb = await _masterContext.MSTRChurchBody.Where(c => c.Status == "A" && (c.OrganisationType == "CH" || c.OrganisationType == "CN")).ToListAsync(), //.Result.Count(),
            //    tsr = await _masterContext.UserRole.Where(c => c.RoleStatus == "A" && c.AppGlobalOwnerId == null && c.ChurchBodyId == null).ToListAsync(), //.Result.Count(),
            //    tsp = await _masterContext.UserPermission.Where(c => c.PermStatus == "A").ToListAsync(), //.Result.Count(), //,
            //    tms = await _masterContext.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A").ToListAsync(), //.Result.Count(), //,
            //    tcl_d = await (from a in _masterContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A")
            //             from b in _masterContext.UserProfileRole.Where(c => c.UserRole.RoleType == "CH_ADMN" || c.UserRole.RoleType == "CF_ADMN")
            //             select a).ToListAsync(), //.Result.Count(),
            //    tcl_a = await _masterContext.UserProfile.Where(c => c.ProfileScope == "C" && c.UserStatus == "A").ToListAsync(), //.Result.Count(),
            //    tsubs = await _masterContext.AppSubscription.Where(c => c.Slastatus == "A").ToListAsync(), //.Result.Count(),
            //    tdb = await _masterContext.ClientAppServerConfig.Select(c => c.DbaseName).Distinct().ToListAsync(), //.Result.Count(),
            //    ttc = await _masterContext.UserAuditTrail.Where(c => c.EventDate.Date == DateTime.Now.Date).ToListAsync() //.Result.Count()
            //};

        //    return ""; // != null;
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
