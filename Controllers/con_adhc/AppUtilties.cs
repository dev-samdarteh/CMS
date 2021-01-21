
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks; 


namespace RhemaCMS.Controllers.con_adhc
{

    public static class AppUtilties_Static
    {
        public static Task<List<T>> ToListAsync<T>(this List<T> list) //IQueryable
        {
            return Task.Run(() => list.ToList());
        }
    }

    public static class StackAppUtilties
    { 
        public static bool IsAjaxRequest(this HttpRequest request)  //(this HttpRequest request)
        {
            if (request == null)
                return false; // new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return !string.IsNullOrEmpty(request.Headers["X-Requested-With"]) &&
                    string.Equals(
                        request.Headers["X-Requested-With"],
                        "XmlHttpRequest",
                        StringComparison.OrdinalIgnoreCase);

            return false;
        }
    }
 

    public class AppUtilties
    {
        public AppUtilties() { }

         

        public static ChurchModelContext GetNewDBContext_CL(MSTR_DbContext dbContext, string dbName, string dbServer = null)
        {
            var conn = new SqlConnectionStringBuilder(dbContext.Database.GetDbConnection().ConnectionString);
            conn.InitialCatalog = dbName;
            if (dbServer != null)
                conn.DataSource = dbServer;
            // dbContext.Database.GetDbConnection().ConnectionString = conn.ConnectionString;
            ///
            var customConn = new ChurchModelContext(conn.ConnectionString);
            return customConn;
        }

        public static ChurchModelContext GetNewDBContext_CL(ChurchModelContext dbContext)
        {
            var customConn = new ChurchModelContext(dbContext.Database.GetDbConnection().ConnectionString);
            return new ChurchModelContext(dbContext.Database.GetDbConnection().ConnectionString);
        }

        public static MSTR_DbContext GetNewDBContext_MS(MSTR_DbContext dbContext, string dbName, string dbServer = null)
        {
            var conn = new SqlConnectionStringBuilder(dbContext.Database.GetDbConnection().ConnectionString);
            //  "DefaultConnection": "Server=RHEMA-SDARTEH;Database=DBRCMS_MS_DEV;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true"
            conn.InitialCatalog = dbName; // conn.DataSource = dbServer; conn.UserID = "sa"; conn.Password = "sa"; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
            
            ///
            if (dbServer != null)
                conn.DataSource = dbServer;
            // dbContext.Database.GetDbConnection().ConnectionString = conn.ConnectionString;
            ///
            var customConn = new MSTR_DbContext(conn.ConnectionString);
            //  "DefaultConnection": "Server=RHEMA-SDARTEH;Database=DBRCMS_MS_DEV;User Id=sa;Password=sadmin;Trusted_Connection=True;MultipleActiveResultSets=true"
            // conn.InitialCatalog = "DBRCMS_CL_TEST";  conn.DataSource = "RHEMA-SDARTEH"; conn.UserID = "sa"; conn.Password = "sadmin"; conn.MultipleActiveResultSets = true; conn.TrustServerCertificate = true;
            ///
            customConn.Database.GetDbConnection().ConnectionString = conn.ConnectionString;
            return customConn;
        }

        public static MSTR_DbContext GetNewDBContext_MS(MSTR_DbContext dbContext)
        { 
            var customConn = new MSTR_DbContext(dbContext.Database.GetDbConnection().ConnectionString);
            return new MSTR_DbContext(dbContext.Database.GetDbConnection().ConnectionString);
        }

        public  static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        ///// <summary>	/// Gets the raw target of an HTTP request.	/// </summary>	/// <returns>Raw target of an HTTP request</returns>	/// <remarks>	/// ASP.NET Core manipulates the HTTP request parameters exposed to pipeline	/// components via the HttpRequest class. This extension method delivers an untainted	/// request target. https://tools.ietf.org/html/rfc7230#section-5.3	/// </remarks>
        //public static string GetRawTarget(this HttpRequest request)
        //{
        //    var httpRequestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>(); return httpRequestFeature.RawTarget;
        //}

        public static string GetRawTarget(HttpRequest request)
        {
            var a = request.Path;
            var b = request.Path.Value; 

            return request.QueryString.Value;

            //var httpRequestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>(); 
            //return httpRequestFeature.RawTarget;
        }

        public static string Pluralize(string strWord)
        {
            var strPlu = strWord;
            if (strPlu.Substring(strPlu.Length - 1, 1).ToLower() != "s" && strPlu.Substring(strPlu.Length - 3, 3).ToLower() != "es" && strPlu.Substring(strPlu.Length - 3, 3).ToLower() != "ies")
            {
                if (strPlu.Substring(strPlu.Length - 1, 1).ToLower() == "y") { strPlu += "ies"; } else if (strPlu.Substring(strPlu.Length - 1, 1).ToLower() == "ch") { strPlu += "es"; } else { strPlu += "s"; }
            }

            return strPlu;
        }

        public static bool CheckUserPrivilege(UserProfileRole oUserProRoleLog)
        {
            //var oUserProRoleLog = TempData.Get<ChurchProData.Models.Authentication.UserProfileRole>("oUserProRoleLogIn");

            if (oUserProRoleLog == null)
                return false;
            else
            {
                if (oUserProRoleLog.UserProfile == null)
                    return false;

                else
                {
                    if (oUserProRoleLog.UserRole == null)
                        return false;

                    else
                    {
                        //ViewBag.oUserLogged = oUserProRoleLog.UserProfile;
                        //ViewBag.oUserProRoleLogged = oUserProRoleLog.UserRole;
                        return true;
                    }
                }
            }
        }


        public static List<UserSessionPrivilege> GetUserPrivilege( MSTR_DbContext _context,  ChurchModelContext _clientContext,  string churchCode, UserProfile oUserProfile)
        {
            //already authenticated... jux the use the user and churchCode... check Active though
            // var _oUserLog_RolePerm = new List<UserPermission>();

            var h_pwd = AppUtilties.ComputeSha256Hash(churchCode);
            // var oUserLog_RolePerm = (
            //                         from a in _context.UserProfile //.Include(u => u.ChurchBody)
            //                         .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode && x.ChurchBodyId == oUserProfile.ChurchBodyId)) && x.Username.Trim().ToLower() == oUserProfile.Username.Trim().ToLower() && x.UserStatus == "A")
            //                         from b in _context.UserProfileRole//.Include(u => u.ChurchBody)
            //                         .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserProfileId == a.Id && x.ProfileRoleStatus == "A")
            //                         from c in _context.UserRolePermission//.Include(u => u.ChurchBody)
            //                         .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserRoleId == b.UserRoleId && x.Status == "A")
            //                         from d in _context.UserPermission//.Include(u => u.ChurchBody)
            //                         .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.Id == c.UserPermissionId && x.PermStatus == "A")

            //                         select d  // c.UserPermissions; 
            //                         ).ToList();


            //// var _oUserLog_GroupPerm = new List<UserPermission>();
            //  var oUserLog_GroupPerm = (
            //                             from a in _context.UserProfile//.Include(u => u.ChurchBody)
            //                             .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode && x.ChurchBodyId==oUserProfile.ChurchBodyId)) && x.Username.Trim().ToLower() == oUserProfile.Username.Trim().ToLower() && x.UserStatus == "A")
            //                             from b in _context.UserProfileGroup//.Include(u => u.ChurchBody)
            //                             .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserProfileId==a.Id && x.Status == "A")   
            //                             from c in _context.UserGroupPermission//.Include(u => u.ChurchBody)
            //                             .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserGroupId== b.UserGroupId && x.Status == "A")
            //                             from d in _context.UserPermission//.Include(u => u.ChurchBody)
            //                             .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.Id == c.UserPermissionId && x.PermStatus == "A")

            //                             select d  // c.UserPermissions; 
            //                         ).ToList();


            // var oUserLogPermAll = new List<UserPermission>();
            // oUserLogPermAll.AddRange(oUserLog_RolePerm);
            // oUserLogPermAll.AddRange(oUserLog_GroupPerm);

         //   var oUserLogPermAll = GetUserAssignedPermissions(_context, _clientContext, churchCode, oUserProfile);
           

            var oUserPrivList = new List<UserSessionPrivilege>(); 

            if (h_pwd == ac1 && // "000000") && 
                oUserProfile.Username == AppUtilties.ComputeSha256Hash("000000") && oUserProfile.Pwd == AppUtilties.ComputeSha256Hash("000000" + "000000" + "$000000"))          
            {  //if (oUserProfile.RootProfileCode == AppUtilties.ComputeSha256Hash(oUserProfile.Username + "RHEMA_Sup_Admn1"))
               // var p = ((JProperty)JObject.FromObject((new UserPermissionLog())._A0__System_Administration).Properties()).Name;
               //  JObject jsonPerm = JObject.FromObject((new UserPermissionLog())._A0__System_Administration);

                //create permission on the fly... DO NOT STORE IN THE DB!           strAppCurrUser_RoleCateg
                oUserPrivList.Add(new UserSessionPrivilege
                { AppGlobalOwner = null, ChurchBody = null, UserProfile = oUserProfile, UserPermission=null, PermissionName = "_A0__System_Administration", PermissionValue = true });  //PermissionName = ((JProperty)jsonPerm.Properties()).Name
                return oUserPrivList;                
            }

            else
            {
                var oUserLogPermAll = GetUserAssigned_SessionPrivileges(_context, churchCode, oUserProfile);            //_clientContext, 
                return oUserLogPermAll;

                //if (oUserLogPermAll.Count > 0)   //(oChurchBody != null && 
                //{
                //    var mainCode = "";
                //    if (churchCode.Contains("_")) mainCode = churchCode.Substring(0, churchCode.IndexOf("-"));
                //    var oAppGloOwn = _context.AppGlobalOwner.Where(x => x.RootChurchCode == mainCode).FirstOrDefault(); //PCG, ICGC, RCM1, RCM2 etc  // ... RCM-0000000, RCM-0000001, PCG-1234567, COP-1000000, ICGC-9999999
                //    var oChurchBody = _context.MSTRChurchBody.Where(x => x.GlobalChurchCode == churchCode).FirstOrDefault(); //PCG-000000  .Include(t=>t.ChurchLevel)  //.Include(t => t.AppGlobalOwner)  //ChurchBody MUST also have been subscribed or renewed...

                //    foreach (var oPerm in oUserLogPermAll)
                //    {
                //        oUserPrivList.Add(new UserSessionPrivilege
                //        {
                //            AppGlobalOwner = oAppGloOwn,
                //            ChurchBody = oChurchBody,
                //            UserProfile = oUserProfile,
                //            UserPermission = oPerm,
                //            PermissionName = oPerm.PermissionName,
                //            PermissionValue = true
                //        });
                //    }

                //    return oUserPrivList;
                //}
            }                   
        
            //all else...
           // return null;

            //UserProfileRole oUserProRoleLog = _context.UserProfileRole
            //        .Include(u => u.ChurchBody)
            //       //.Include(u => u.UserProfile)
            //       //.Include(u => u.UserRole)
            //       //.Include(t => t.UserProfile.ChurchBody)
            //       .Where(c => c.ChurchBody.ChurchCode == churchCode && c.UserProfileId == oUserProfile.Id && c.ProfileRoleStatus == "A").FirstOrDefault();

            //// UserProfile oUserLogged = oUserLog[0].oUser; UserProfileRole oUserRoleLogged = oUserLog[0].oUserRole;

            //if (oUserProRoleLog != null)
            //{
            //    UserRole oUserRoleLog = _context.UserRole.Where(c => c.Id == oUserProRoleLog.UserRoleId && c.RoleStatus == "A").FirstOrDefault();
            //    //HttpContext.Session.SetObjectAsJson("oUserLogged", oUserLog);
            //    //HttpContext.Session.SetObjectAsJson("oUserProRoleLogged", oUserProRoleLog);

            //    oUserProRoleLog.UserProfile = oUserLog;
            //    oUserProRoleLog.UserRole = oUserRoleLog;                
            //} 
        }

        public static List<UserSessionPrivilege> GetUserPrivilege(MSTR_DbContext _context, string churchCode, UserProfile oUserProfile)
        {
            //already authenticated... jux the use the user and churchCode... check Active though
            // var _oUserLog_RolePerm = new List<UserPermission>();

            var h_pwd = AppUtilties.ComputeSha256Hash(churchCode);
            // var oUserLog_RolePerm = (
            //                         from a in _context.UserProfile //.Include(u => u.ChurchBody)
            //                         .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode && x.ChurchBodyId == oUserProfile.ChurchBodyId)) && x.Username.Trim().ToLower() == oUserProfile.Username.Trim().ToLower() && x.UserStatus == "A")
            //                         from b in _context.UserProfileRole//.Include(u => u.ChurchBody)
            //                         .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserProfileId == a.Id && x.ProfileRoleStatus == "A")
            //                         from c in _context.UserRolePermission//.Include(u => u.ChurchBody)
            //                         .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserRoleId == b.UserRoleId && x.Status == "A")
            //                         from d in _context.UserPermission//.Include(u => u.ChurchBody)
            //                         .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.Id == c.UserPermissionId && x.PermStatus == "A")

            //                         select d  // c.UserPermissions; 
            //                         ).ToList();


            //// var _oUserLog_GroupPerm = new List<UserPermission>();
            //  var oUserLog_GroupPerm = (
            //                             from a in _context.UserProfile//.Include(u => u.ChurchBody)
            //                             .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode && x.ChurchBodyId==oUserProfile.ChurchBodyId)) && x.Username.Trim().ToLower() == oUserProfile.Username.Trim().ToLower() && x.UserStatus == "A")
            //                             from b in _context.UserProfileGroup//.Include(u => u.ChurchBody)
            //                             .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserProfileId==a.Id && x.Status == "A")   
            //                             from c in _context.UserGroupPermission//.Include(u => u.ChurchBody)
            //                             .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserGroupId== b.UserGroupId && x.Status == "A")
            //                             from d in _context.UserPermission//.Include(u => u.ChurchBody)
            //                             .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.Id == c.UserPermissionId && x.PermStatus == "A")

            //                             select d  // c.UserPermissions; 
            //                         ).ToList();


            // var oUserLogPermAll = new List<UserPermission>();
            // oUserLogPermAll.AddRange(oUserLog_RolePerm);
            // oUserLogPermAll.AddRange(oUserLog_GroupPerm);

            //   var oUserLogPermAll = GetUserAssignedPermissions(_context, _clientContext, churchCode, oUserProfile);


            var oUserPrivList = new List<UserSessionPrivilege>();

            if (h_pwd == ac1 && // "000000") && 
                oUserProfile.Username == AppUtilties.ComputeSha256Hash("000000") && oUserProfile.Pwd == AppUtilties.ComputeSha256Hash("000000" + "000000" + "$000000"))
            {  //if (oUserProfile.RootProfileCode == AppUtilties.ComputeSha256Hash(oUserProfile.Username + "RHEMA_Sup_Admn1"))
               // var p = ((JProperty)JObject.FromObject((new UserPermissionLog())._A0__System_Administration).Properties()).Name;
               //  JObject jsonPerm = JObject.FromObject((new UserPermissionLog())._A0__System_Administration);

                //create permission on the fly... DO NOT STORE IN THE DB!           strAppCurrUser_RoleCateg
                oUserPrivList.Add(new UserSessionPrivilege
                { AppGlobalOwner = null, ChurchBody = null, UserProfile = oUserProfile, UserPermission = null, PermissionName = "_A0__System_Administration", PermissionValue = true });  //PermissionName = ((JProperty)jsonPerm.Properties()).Name
                return oUserPrivList;
            }

            else
            {
                var oUserLogPermAll = GetUserAssigned_SessionPrivileges(_context, churchCode, oUserProfile);

                return oUserLogPermAll;

                //if (oUserLogPermAll.Count > 0)   //(oChurchBody != null && 
                //{
                //    var mainCode = "";
                //    if (churchCode.Contains("_")) mainCode = churchCode.Substring(0, churchCode.IndexOf("-"));
                //    var oAppGloOwn = _context.AppGlobalOwner.Where(x => x.RootChurchCode == mainCode).FirstOrDefault(); //PCG, ICGC, RCM1, RCM2 etc  // ... RCM-0000000, RCM-0000001, PCG-1234567, COP-1000000, ICGC-9999999
                //    var oChurchBody = _context.MSTRChurchBody.Where(x => x.GlobalChurchCode == churchCode).FirstOrDefault(); //PCG-000000  .Include(t=>t.ChurchLevel)  //.Include(t => t.AppGlobalOwner)  //ChurchBody MUST also have been subscribed or renewed...

                //    foreach (var oPerm in oUserLogPermAll)
                //    {
                //        oUserPrivList.Add(new UserSessionPrivilege
                //        {
                //            AppGlobalOwner = oAppGloOwn,
                //            ChurchBody = oChurchBody,
                //            UserProfile = oUserProfile,
                //            UserPermission = oPerm,
                //            PermissionName = oPerm.PermissionName,
                //            PermissionValue = true
                //        });
                //    }

                //    return oUserPrivList;
                //}
            }

            //all else...
            // return null;

            //UserProfileRole oUserProRoleLog = _context.UserProfileRole
            //        .Include(u => u.ChurchBody)
            //       //.Include(u => u.UserProfile)
            //       //.Include(u => u.UserRole)
            //       //.Include(t => t.UserProfile.ChurchBody)
            //       .Where(c => c.ChurchBody.ChurchCode == churchCode && c.UserProfileId == oUserProfile.Id && c.ProfileRoleStatus == "A").FirstOrDefault();

            //// UserProfile oUserLogged = oUserLog[0].oUser; UserProfileRole oUserRoleLogged = oUserLog[0].oUserRole;

            //if (oUserProRoleLog != null)
            //{
            //    UserRole oUserRoleLog = _context.UserRole.Where(c => c.Id == oUserProRoleLog.UserRoleId && c.RoleStatus == "A").FirstOrDefault();
            //    //HttpContext.Session.SetObjectAsJson("oUserLogged", oUserLog);
            //    //HttpContext.Session.SetObjectAsJson("oUserProRoleLogged", oUserProRoleLog);

            //    oUserProRoleLog.UserProfile = oUserLog;
            //    oUserProRoleLog.UserRole = oUserRoleLog;                
            //} 
        }



        public static List<UserPermission> GetUserAssignedPermissions(MSTR_DbContext _context, string churchCode, UserProfile oUserProfile)   //ChurchModelContext _clientContext, 
        {
            //already authenticated... jux the use the user and churchCode... check Active though
            // var _oUserLog_RolePerm = new List<UserPermission>();

            var h_pwd = AppUtilties.ComputeSha256Hash(churchCode);
            var oUserLog_RolePerm = (
                                    from a in _context.UserProfile //.Include(u => u.ChurchBody)
                                    .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode && x.ChurchBodyId == oUserProfile.ChurchBodyId)) && x.Username.Trim().ToLower() == oUserProfile.Username.Trim().ToLower() && x.UserStatus == "A")
                                    from b in _context.UserProfileRole//.Include(u => u.ChurchBody)
                                    .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserProfileId == a.Id && x.ProfileRoleStatus == "A")
                                    from c in _context.UserRolePermission//.Include(u => u.ChurchBody)
                                    .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserRoleId == b.UserRoleId && x.Status == "A")
                                    from d in _context.UserPermission//.Include(u => u.ChurchBody)
                                    .Where(x => x.Id == c.UserPermissionId && x.PermStatus == "A")  //&& (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId 

                                    select d  // c.UserPermissions; 
                                    ).ToList();


            // var _oUserLog_GroupPerm = new List<UserPermission>();
            var oUserLog_GroupPerm = (
                                       from a in _context.UserProfile//.Include(u => u.ChurchBody)
                                       .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode && x.ChurchBodyId == oUserProfile.ChurchBodyId)) && x.Username.Trim().ToLower() == oUserProfile.Username.Trim().ToLower() && x.UserStatus == "A")
                                       from b in _context.UserProfileGroup//.Include(u => u.ChurchBody)
                                       .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserProfileId == a.Id && x.Status == "A")
                                       from c in _context.UserGroupPermission//.Include(u => u.ChurchBody)
                                       .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserGroupId == b.UserGroupId && x.Status == "A")
                                       from d in _context.UserPermission//.Include(u => u.ChurchBody)
                                       .Where(x => x.Id == c.UserPermissionId && x.PermStatus == "A")   // (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) &&  x.ChurchBodyId == a.ChurchBodyId && 

                                       select d  // c.UserPermissions; 
                                   ).ToList();


            var oUserLogPermAll = new List<UserPermission>();
            oUserLogPermAll.AddRange(oUserLog_RolePerm);
            oUserLogPermAll.AddRange(oUserLog_GroupPerm);
             
            return oUserLogPermAll; 
        }

        public static List<UserSessionPrivilege> GetUserAssigned_SessionPrivileges(MSTR_DbContext _context, string churchCode, UserProfile oUserProfile) // ChurchModelContext _clientContext, 
        {
            //already authenticated... jux the use the user and churchCode... check Active though
            // var _oUserLog_RolePerm = new List<UserPermission>();

            var h_pwd = AppUtilties.ComputeSha256Hash(churchCode);
            var oUserLog_RolePerm = (
                                    from a in _context.UserProfile.Include(u => u.AppGlobalOwner).Include(u => u.ChurchBody)
                                    .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode && x.ChurchBodyId == oUserProfile.ChurchBodyId)) && x.Username.Trim().ToLower() == oUserProfile.Username.Trim().ToLower() && x.UserStatus == "A")
                                    from b in _context.UserProfileRole.Include(u => u.UserRole)
                                    .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserProfileId == a.Id && x.ProfileRoleStatus == "A")
                                    from c in _context.UserRolePermission//.Include(u => u.ChurchBody)
                                    .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserRoleId == b.UserRoleId && x.Status == "A")
                                    from d in _context.UserPermission//.Include(u => u.ChurchBody)
                                    .Where(x => x.Id == c.UserPermissionId && x.PermStatus == "A")  //&& (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId 
                                     
                                    select new UserSessionPrivilege()
                                    {
                                        AppGlobalOwner = a.AppGlobalOwner,
                                        ChurchBody = a.ChurchBody,
                                        UserProfile = oUserProfile,
                                        UserRole = b.UserRole,
                                        RoleName = b.UserRole != null ? b.UserRole.RoleName : "",
                                        UserPermission = d,
                                        PermissionName = d.PermissionName,
                                        PermissionValue = true,
                                        strChurchCode_AGO = a.AppGlobalOwner != null ? a.AppGlobalOwner.GlobalChurchCode : "",
                                        strChurchCode_CB = a.ChurchBody != null ? a.ChurchBody.GlobalChurchCode : "",  // assumption that Unspecified CB ~~ Vendor [Church code is MANDATORY!]
                                        //
                                        ViewPerm = c.ViewPerm,                                        
                                        CreatePerm = c.CreatePerm,                                        
                                        EditPerm = c.EditPerm,                                        
                                        DeletePerm = c.DeletePerm,                                        
                                        ManagePerm = c.ManagePerm                                       
                                    })
                                    .ToList();


            // var _oUserLog_GroupPerm = new List<UserPermission>();
            var oUserLog_GroupPerm = (
                                       from a in _context.UserProfile.Include(u => u.AppGlobalOwner).Include(u => u.ChurchBody)
                                       .Where(x => (h_pwd == ac1 || (x.ChurchBody.GlobalChurchCode == churchCode && x.ChurchBodyId == oUserProfile.ChurchBodyId)) && x.Username.Trim().ToLower() == oUserProfile.Username.Trim().ToLower() && x.UserStatus == "A")
                                       from b in _context.UserProfileGroup.Include(u => u.UserGroup)
                                       .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserProfileId == a.Id && x.Status == "A")
                                       from c in _context.UserGroupPermission//.Include(u => u.ChurchBody)
                                       .Where(x => (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) && x.ChurchBodyId == a.ChurchBodyId && x.UserGroupId == b.UserGroupId && x.Status == "A")
                                       from d in _context.UserPermission//.Include(u => u.ChurchBody)
                                       .Where(x => x.Id == c.UserPermissionId && x.PermStatus == "A")   // (h_pwd == ac1 || x.ChurchBody.GlobalChurchCode == churchCode) &&  x.ChurchBodyId == a.ChurchBodyId && 

                                       select new UserSessionPrivilege()
                                       {
                                           AppGlobalOwner = a.AppGlobalOwner,
                                           ChurchBody = a.ChurchBody,
                                           UserProfile = oUserProfile,
                                           UserGroup = b.UserGroup,
                                           GroupName = b.UserGroup != null ? b.UserGroup.GroupName : "",
                                           UserPermission = d,
                                           PermissionName = d.PermissionName,
                                           PermissionValue = true,
                                           strChurchCode_AGO = a.AppGlobalOwner != null ? a.AppGlobalOwner.GlobalChurchCode : "",
                                           strChurchCode_CB = a.ChurchBody != null ? a.ChurchBody.GlobalChurchCode : "",  // assumption that Unspecified CB ~~ Vendor [Church code is MANDATORY!]
                                           //
                                           ViewPerm = c.ViewPerm,
                                           CreatePerm = c.CreatePerm,
                                           EditPerm = c.EditPerm,
                                           DeletePerm = c.DeletePerm,
                                           ManagePerm = c.ManagePerm
                                       })
                                        .ToList();


            var oUserLogPermAll = new List<UserSessionPrivilege>();
            oUserLogPermAll.AddRange(oUserLog_RolePerm);
            oUserLogPermAll.AddRange(oUserLog_GroupPerm);
             //
            return oUserLogPermAll; 
        }

        public static List<UserPermission> CombineCollection(List<UserPermission> list1, List<UserPermission> list2,
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

        public static string  GetPermissionDesc_FromName(string permName )
        {
            //[ ____ : (, ___ : ), ______ : - , _____ : &]
            if (permName.Contains("__"))
            {
                //var pn = ((((permName.Replace("______", " - ")).Replace("_____", " & ")).Replace("____", " (")).Replace("___", ") ")).Trim();  //   .Replace("__", "")).Replace("_", " ")).Trim();

                var pn1 = permName.Replace("______", "-");
                var pn2 = pn1.Replace("_____", " & ");
                var pn3 = pn2.Replace("____", " (");
                var pn4 = pn3.Replace("___", ") ");  //   .Replace("__", "")).Replace("_", " ")).Trim();
                var codeStr = pn4.Split("__"); 
                return ((codeStr[1].Replace("__", "")).Replace("_", " ")).Trim();
            }

            return permName; 
        }

        public static string GetPermissionCode_FromName(string permName)
        {
            if (permName.Contains("__"))
            {
                var codeStr = permName.Split("__");

                if (codeStr.Length > 1)
                    return codeStr[0].Substring(1).Trim();
                else
                    return codeStr[0].Trim();
            }

            return "";
        }

        

        public List<UserPermission> GetSystem_Administration_Permissions()
        {
            var lsPerms = new List<UserPermission>(); //10
            //
            lsPerms.Add(new UserPermission(0, null, "A0", "_A0__System_Administration", "A", null, null, null, null));   
            lsPerms.Add(new UserPermission(0, null, "A0_00", "_A0_00__Super_Admin_Account", "A", null, null, null, null));   // for SYS account only
            lsPerms.Add(new UserPermission(0, null, "A0_01", "_A0_01__Church_Faith_Types", "A", null, null, null, null));    
            lsPerms.Add(new UserPermission(0, null, "A0_02", "_A0_02__Denominations", "A", null, null, null, null));    
            lsPerms.Add(new UserPermission(0, null, "A0_03", "_A0_03__Church_Levels", "A", null, null, null, null));    
            lsPerms.Add(new UserPermission(0, null, "A0_04", "_A0_04__Congregations", "A", null, null, null, null));    
            lsPerms.Add(new UserPermission(0, null, "A0_05", "_A0_05__Subscribers_Unit_Church_Structure", "A", null, null, null, null));    
            lsPerms.Add(new UserPermission(0, null, "A0_06", "_A0_06__Church_Administrator_Accounts", "A", null, null, null, null));    
            lsPerms.Add(new UserPermission(0, null, "A0_07", "_A0_07__User_Subscriptions", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "A0_08", "_A0_08__Admin_Dashboard", "A", null, null, null, null));
            //
            return lsPerms;
        }
        public List<UserPermission> GetAppDashboard_Permissions()
        {
            var lsPerms = new List<UserPermission>(); //8
            //
            lsPerms.Add(new UserPermission(0, null, "00", "_00__Dashboard", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "00_01", "_00_01__Oversight_Congregations", "A", null, null, null, null));   // for CH account only
            lsPerms.Add(new UserPermission(0, null, "00_02", "_00_02__Members", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "00_03", "_00_03__New_Converts", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "00_04", "_00_04__Receipts____Income___", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "00_05", "_00_05__Payments____Expense___", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "00_06", "_00_06__Church_Attendance_Trend", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "00_07", "_00_07__To______Do_List", "A", null, null, null, null)); 
            //
            return lsPerms;
        }
        public List<UserPermission> GetAppConfigurations_Permissions()
        {
            var lsPerms = new List<UserPermission>();//9
            //
            lsPerms.Add(new UserPermission(0, null, "00", "_01__App_Configurations", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "01_01", "_01_01__Church_Parameters____Configurations___", "A", null, null, null, null));    
            lsPerms.Add(new UserPermission(0, null, "01_02", "_01_02__General_Parameters", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "01_03", "_01_03__Church_Units_Structure____Organisational_Chart___", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "01_04", "_01_04__Internal_User_Accounts", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "01_05", "_01_05__User_Preferences", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "01_06", "_01_06__Upload_Configuration", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "01_07", "_01_07__Custom_Import_Configuration", "A", null, null, null, null));
            //
            return lsPerms;
        }
        public List<UserPermission> GetMemberRegister_Permissions()
        {
            var lsPerms = new List<UserPermission>();//9
            //
            lsPerms.Add(new UserPermission(0, null, "02", "_02__Member_Register", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "02_01", "_02_01__Member_Explorer", "A", null, null, null, null));   // for SYS account only
            lsPerms.Add(new UserPermission(0, null, "02_01_01", "_02_01_01__Member_Profile", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "02_01_02", "_02_01_02__Member_Church______life", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "02_02", "_02_02__New_Members", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "02_03", "_02_03__Past_Membership", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "02_04", "_02_04__Profile_Card", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "02_05", "_02_05__Lookup_Member", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "02_06", "_02_06__Member_History", "A", null, null, null, null));
            //
            return lsPerms;
        }
        public List<UserPermission> GetChurchlifeAndEvents_Permissions()
        {
            var lsPerms = new List<UserPermission>();//12
            //
            lsPerms.Add(new UserPermission(0, null, "03", "_03__Church______life_____Events", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_01", "_03_01__Church_Calendar____Almanac___", "A", null, null, null, null));   // for SYS account only
            lsPerms.Add(new UserPermission(0, null, "03_02", "_03_02__Events_Countdown", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_03", "_03_03__Church_Service_Line______up", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_04", "_03_04__Order_of_Service", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_05", "_03_05__Preaching_Plan", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_06", "_03_06__Church_Activity_Roster", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_07", "_03_07__Minister_Schedule", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_08", "_03_08__Church_Core_Activities", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_09", "_03_09__Member______Activity_Checklist", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_10", "_03_10__My_Calendar", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "03_11", "_03_11__To______Do_List", "A", null, null, null, null));
            //
            return lsPerms;
        }
        public List<UserPermission> GetChurchAdministration_Permissions()
        {
            var lsPerms = new List<UserPermission>();   //19
            //
            lsPerms.Add(new UserPermission(0, null, "04", "_04__Church_Administration", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_01", "_04_01__Congregations", "A", null, null, null, null));   // for SYS account only
            lsPerms.Add(new UserPermission(0, null, "04_02", "_04_02__Church_Units", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_03", "_04_03__Leadership_Pool", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_04", "_04_04__Church_Projects", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_05", "_04_05__Church_Attendance", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_06", "_04_06__Church_Visitors", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_07", "_04_07__New_Converts", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_08", "_04_08__Church_Transfers", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_08_01", "_04_08_01__Member_Transfers", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_08_02", "_04_08_02__Clergy_Transfers", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_08_03", "_04_08_03__Role_Transfers", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_08_04", "_04_08_04__Transfer_Requests_Approval", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_09", "_04_09__Promotions_____Demotions", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_10", "_04_10__Notices_____Announcements", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_11", "_04_11__Internal______Communication", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_11_01", "_04_11_01__Broadcast_Notifications", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_11_02", "_04_11_02__Send_Birthday_Anniversary_Messages", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "04_12", "_04_12__Assets_Register", "A", null, null, null, null)); 
            //
            return lsPerms;
        }
        public List<UserPermission> GetFinanceManagement_Permissions()
        {
            var lsPerms = new List<UserPermission>();   //9
            //
            lsPerms.Add(new UserPermission(0, null, "05", "_05__Finance_Management", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "05_01", "_05_01__Receipts____Income___", "A", null, null, null, null));   // for SYS account only
            lsPerms.Add(new UserPermission(0, null, "05_02", "_05_02__Payments____Expense___", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "05_03", "_05_03__Offertory____Collection___", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "05_04", "_05_04__Tithes", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "05_05", "_05_05__Donations", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "05_06", "_05_06__Trial_Balance", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "05_07", "_05_07__Financial_Reports", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "05_08", "_05_08__Sync_Capitalized_Assets", "A", null, null, null, null));
             
            //
            return lsPerms;
        }
        public List<UserPermission> GetReportsAnalytics_Permissions()
        {
            var lsPerms = new List<UserPermission>();   //4
            //
            lsPerms.Add(new UserPermission(0, null, "06", "_06__Reports_____Analytics", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "06_01", "_06_01__Church_Statistics", "A", null, null, null, null));   // for SYS account only
            lsPerms.Add(new UserPermission(0, null, "06_02", "_06_02__Growth_Trends", "A", null, null, null, null));
            lsPerms.Add(new UserPermission(0, null, "06_03", "_06_03__Adhoc_Analysis", "A", null, null, null, null));
                          
            //
            return lsPerms;
        }
         


        public List<UserRole> GetSystemDefaultRoles()
        {
            var lsRoles = new List<UserRole>();
            //
            lsRoles.Add(new UserRole(0, null,null, "SYS", "System Account", "A", 1, "SYS", null, null, null, null));     
            lsRoles.Add(new UserRole(0, null,null, "SUP_ADMN", "Super Administrator", "A", 2, "SUP_ADMN", null, null, null, null));     
            lsRoles.Add(new UserRole(0, null,null, "SYS_ADMN", "System Administrator", "A", 3, "SYS_ADMN", null, null, null, null));    
            lsRoles.Add(new UserRole(0, null,null, "SYS_CUST", "System Custom", "A", 4, "SYS_CUST", null, null, null, null));    
           // lsRoles.Add(new UserRole(null, null,null, "SYS_CUST2", "System Custom", "A", 5, "SYS_CUST", null, null, null, null));    
            //                               null,
            lsRoles.Add(new UserRole(0, null,null, "CH_ADMN", "Church Administrator", "A", 6, "CH_ADMN", null, null, null, null));  // 1st created by Vendor   
            lsRoles.Add(new UserRole(0, null,null, "CH_RGSTR", "Church Registrar", "A", 7, "CH_RGSTR", null, null, null, null));     
            lsRoles.Add(new UserRole(0, null,null, "CH_ACCT", "Church Accountant", "A", 8, "CH_ACCT", null, null, null, null));     
            lsRoles.Add(new UserRole(0, null,null, "CH_CUST", "Church Custom", "A", 9, "CH_CUS", null, null, null, null));  
           // lsRoles.Add(new UserRole(null, null,null, "CH_CUST2", "Church Custom", "A", 10, "CH_CUS", null, null, null, null));  
            //                               null,
            lsRoles.Add(new UserRole(0, null,null, "CF_ADMN", "Congregation Administrator", "A", 11, "CF_ADMN", null, null, null, null));   // 1st created by Vendor  
            lsRoles.Add(new UserRole(0, null,null, "CF_RGSTR", "Congregation Registrar", "A", 12, "CF_RGSTR", null, null, null, null));     
            lsRoles.Add(new UserRole(0, null,null, "CF_ACCT", "Congregation Accountant", "A", 13, "CF_ACCT", null, null, null, null));     
            lsRoles.Add(new UserRole(0, null,null, "CF_CUST", "Congregation Custom", "A", 14, "CF_CUS", null, null, null, null));  
           // lsRoles.Add(new UserRole(null, null,null, "CF_CUST2", "Congregation Custom", "A", 15, "CF_CUS", null, null, null, null));  
            //
            return lsRoles;
        }


        //public static List<RelationshipType> GetCustomRelationTypes(ChurchMember oChurchMember, RhemaCMS.Models.CLNTModels.ChurchModelContext _context)
        //{
        //    //List<RelationshipType> exclRelConfig
        //   // List<RelationshipType> oRelConfAvail = new List<RelationshipType>();

        //    if (oChurchMember != null)
        //    {
        //        var exclRelConf = (from a in _context.RelationshipType
        //                           join
        //                           b in _context.MemberRelation.Where(c => c.ChurchMemberId == oChurchMember.Id) on a.Id equals b.RelationshipId
        //                           select a)
        //                      .ToList();

        //        //filter lists...            
        //        return _context.RelationshipType
        //                            .Where(tpl => !exclRelConf.Contains(tpl))
        //                            .OrderBy(tpl => tpl.RelationIndex).ThenBy(tpl => tpl.Name)
        //                            .ToList(); 
        //    }
        //    else
        //        return  _context.RelationshipType 
        //                            .OrderBy(c => c.RelationIndex).ThenBy(c=>c.Name)
        //                            .ToList(); ;


        //    ////config per member... member config with husband should not show up next time to be picked up
        //    //List < RelationshipType > oRelTypes = new List<RelationshipType>();

        //    ////get list saved already...
        //    //var oRelOptions = _context.RelationshipType                               
        //    //                    .OrderBy(c => c.RelationIndex)
        //    //                    .ToList();
        //    //foreach (var oRelItem in oRelOptions)
        //    //{
        //    //    var oRelFound = false;
        //    //    if (exclRelConfig != null)
        //    //    {
        //    //        foreach (var oExcl in exclRelConfig) // int i = 0; i < exclRelTypes.Count; i++)
        //    //        {
        //    //            if (oExcl.Name.Equals(oRelItem.Name) && oExcl.RelationIndex.Equals(oRelItem.RelationIndex))
        //    //                oRelFound = true; break;
        //    //        }
        //    //    }

        //    //    if (!oRelFound)
        //    //        oRelTypes.Add(oRelItem);
        //    //}

        //    //return oRelTypes;
        //}
        

        public static List<RelationshipType> GetGenericRelationTypes(List<RelationshipType> exclRelCustom)
        {
            //config per saved dbase... member config with list <> should not show up next time to be saved
            List<RelationshipType> oRelTypes = new List<RelationshipType>();
            var oRelOptions = GetRelationOptions();
            foreach (var oRelItem in oRelOptions)   //for(int i=0; i<oRelOptions.Count; i++) 
            {
               // var oRelItem = oRelOptions[i];

                var oRelFound = false;
                if (exclRelCustom != null)
                {
                    foreach (var oExcl in exclRelCustom) // int i = 0; i < exclRelTypes.Count; i++)
                    {
                        if (oExcl.Name.Equals(oRelItem[0]) && oExcl.RelationIndex.Equals(oRelItem[1]))
                            oRelFound = true; break;
                    }
                }

                if (!oRelFound)
                {
                    var newRelType = new RelationshipType();
                    newRelType.Name =  oRelItem[0].ToString();
                    newRelType.RelationIndex = int.Parse(oRelItem[1].ToString()); 
                    //
                    oRelTypes.Add(newRelType);
                }                    
            }
            
            return oRelTypes;
        }

               
        private static List<ArrayList> GetRelationOptions()
        {
            List<ArrayList> oRelations = new List<ArrayList>();
            var arr = new ArrayList();

            // Next-of-Kin = 1  [can be paired up with any other relation as add-on], 
            arr.Add("Next-of-kin"); arr.Add(1);
            oRelations.Add(arr);  
            
            // Spouse = Wife = Husband = 2, ... No Polygamy! (.:.)
            arr.Clear(); arr.Add("Spouse"); arr.Add(2);
            oRelations.Add(arr);  
            arr.Clear(); arr.Add("Wife"); arr.Add(2);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Husband"); arr.Add(2);
            oRelations.Add(arr);

            //Child = Son = Daughter = 3             
            arr.Clear(); arr.Add("Child"); arr.Add(3);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Son"); arr.Add(3);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Daughter"); arr.Add(3);
            oRelations.Add(arr);

            // Father = Mother = 4
            arr.Clear(); arr.Add("Father"); arr.Add(4);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Mother"); arr.Add(4);
            oRelations.Add(arr);            

            // Sister = Brother = 5
            arr.Clear(); arr.Add("Brother"); arr.Add(5);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Sister"); arr.Add(5);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Step Brother"); arr.Add(5);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Step Sister"); arr.Add(5);
            oRelations.Add(arr);

            // Great grandfather = Great grandmother = 6,
            arr.Clear(); arr.Add("Great grandfather"); arr.Add(6);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Great grandmother"); arr.Add(6);
            oRelations.Add(arr);

            // Grandfather = Grandmother = 7,
            arr.Clear(); arr.Add("Grandfather"); arr.Add(7);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Grandmother"); arr.Add(7);
            oRelations.Add(arr);

            // Uncle = Auntie = Step mother = Step father = 8
            arr.Clear(); arr.Add("Step father"); arr.Add(8);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Step mother"); arr.Add(8);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Uncle"); arr.Add(8);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Auntie"); arr.Add(8);
            oRelations.Add(arr);

            // Grandchild = Granddaughter = Granddaughter = 9,
            arr.Clear(); arr.Add("Grandchild"); arr.Add(9);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Granddaughter"); arr.Add(9);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Granddaughter"); arr.Add(9);
            oRelations.Add(arr);

            //Father-in-law = Mother-in-law = Godfather = Godmother = 10, 
            arr.Clear(); arr.Add("Godfather"); arr.Add(10);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Godmother"); arr.Add(10);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Father-in-law"); arr.Add(10);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Mother-in-law"); arr.Add(10);
            oRelations.Add(arr);

            // Son-in-law = Daughter-in-law = 11, 
            arr.Clear(); arr.Add("Son-in-law"); arr.Add(11);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Daughter-in-law"); arr.Add(11);
            oRelations.Add(arr);

            // Cousin = Fiance = Fiancee = Best friend = 12,
            arr.Clear(); arr.Add("Cousin"); arr.Add(12);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Fiance"); arr.Add(12);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Fiancee"); arr.Add(12);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Best friend"); arr.Add(12);
            oRelations.Add(arr);

            // Nephew = Niece = Step child = Step son = Step daughter = 13, 
            arr.Clear(); arr.Add("Nephew"); arr.Add(13);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Niece"); arr.Add(13);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Step child"); arr.Add(13);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Step son"); arr.Add(13);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Step daughter"); arr.Add(13);
            oRelations.Add(arr);

            // Great grandchild = Great grandson = Great ganddaughter = 14 
            arr.Clear(); arr.Add("Great grandchild"); arr.Add(14);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Great grandson"); arr.Add(14);
            oRelations.Add(arr);
            arr.Clear(); arr.Add("Great ganddaughter"); arr.Add(14);
            oRelations.Add(arr); 

            return oRelations;
        }
             

        public static List<MSTRCountry> GetMS_Countries()
        {
            List<MSTRCountry> oCountries = new List<MSTRCountry>();
            List<string> CountryList = new List<string>();

            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo oCtry = new RegionInfo(CInfo.LCID);
                if (!(CountryList.Contains(oCtry.EnglishName)))
                {
                    CountryList.Add(oCtry.EnglishName);

                    var ctry = new MSTRCountry();
                    ctry.EngName = oCtry.EnglishName;
                    ctry.CtryAlpha3Code = oCtry.ThreeLetterISORegionName;
                    ctry.CtryAlpha2Code = oCtry.TwoLetterISORegionName;
                    ctry.CurrEngName = oCtry.CurrencyEnglishName;
                    ctry.CurrLocName = oCtry.CurrencyNativeName;
                    ctry.CurrSymbol = oCtry.CurrencySymbol;
                    ctry.Curr3LISOSymbol = oCtry.ISOCurrencySymbol;

                    oCountries.Add(ctry);
                }
            }

          //  CountryList.Sort(); 
            return oCountries;
        }

        public static List<Country> GetClientCountries()
        {
            List<Country> oCountries = new List<Country>();
            List<string> CountryList = new List<string>();

            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo oCtry = new RegionInfo(CInfo.LCID);
                if (!(CountryList.Contains(oCtry.EnglishName)))
                {
                    CountryList.Add(oCtry.EnglishName);

                    var ctry = new Country();
                    ctry.EngName = oCtry.EnglishName;
                    ctry.CtryAlpha3Code = oCtry.ThreeLetterISORegionName;
                    ctry.CtryAlpha2Code = oCtry.TwoLetterISORegionName;
                    ctry.CurrEngName = oCtry.CurrencyEnglishName;
                    ctry.CurrLocName = oCtry.CurrencyNativeName;
                    ctry.CurrSymbol = oCtry.CurrencySymbol;
                    ctry.Curr3LISOSymbol = oCtry.ISOCurrencySymbol;
                    ///
                   // ctry.IsChurchCountry = true;
                   // ctry.IsDisplay = true;

                    oCountries.Add(ctry);
                }
            }

            //  CountryList.Sort(); 
            return oCountries;
        }


        public static bool SendSMSNotification(string strPhone, string strPostalCode, string strMsg)
        {
            if (strPhone == null || strPhone == "") return false;

            if (strPhone.Length <= 10 && !strPhone.StartsWith(strPostalCode))
                strPhone = strPostalCode + strPhone.Substring(1, strPhone.Length - 1);  

            var oSendSMS = new AppUtility_SMS();

            //pick SMS Sender ID from db
            bool resMsg = oSendSMS.sendMessage("RHEMAChurch", strPhone, strMsg, "sdarteh", "Sdgh?2020"); 
            return resMsg;

            //if (resMsg)
            //    MessageBox.Show("Message sent successfully")
            //    else
            //    MessageBox.Show("Failed sending message to some recipient(s).");
            //End If
        }





        private static List<char> GetUpperCaseChars(int count)
        {
            List<char> result = new List<char>();
            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Char.ToUpper(Convert.ToChar(random.Next(97, 122))));
            }

            return result;
        }

        private static List<char> GetLowerCaseChars(int count)
        {
            List<char> result = new List<char>();

            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Char.ToLower(Convert.ToChar(random.Next(97, 122))));
            }

            return result;
        }

        private static List<char> GetNumericChars(int count)
        {
            List<char> result = new List<char>();

            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Convert.ToChar(random.Next(0, 9).ToString()));
            }

            return result;
        }

        private static string GenerateCodeFromList(List<char> chars)
        {
            string result = string.Empty;

            Random random = new Random();

            while (chars.Count > 0)
            {
                int randomIndex = random.Next(0, chars.Count);
                result += chars[randomIndex];
                chars.RemoveAt(randomIndex);
            }

            return result;
        }



        /// <summary>
        /// ////////////////////
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="churchCode"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// 
        private const string ac1 = "91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203";
        //private const string ac2 = "f7b11509f4d675c3c44f0dd37ca830bb02e8cfa58f04c46283c4bfcbdce1ff45";
        //private const string ac3 = "78415a1535ca0ef885aa7c0278a4de274b85d0c139932cc138ba6ee5cac4a00b";

        public static List<UserSessionPrivilege> ValidateUser(MSTR_DbContext _context, ChurchModelContext _clientDBContext, string churchCode, string username, string password)
        {
            try
            {
                //  var temp = AppUtilties.ComputeSha256Hash("dabrokwah" + "$rhemacloud");
                string strPwdHashedData = AppUtilties.ComputeSha256Hash(churchCode + username + password);
                UserProfile oUser = null;
                //if (username == "test" || username == "test1")
                //{
                //    //oUser = _context.UserProfile //.Include(u => u.ChurchBody).Include(u => u.ChurchMember)
                //    //                .Where(c => c.ChurchBody.ChurchCode == churchCode && c.ChurchMember.IsCurrent && c.Username == username).FirstOrDefault(); // && c.Pwd.Trim() == strPwdHashedData.Trim() && c.UserStatus == "A").FirstOrDefault();

                //    oUser = (from t_up in _context.UserProfile.Include(t => t.ChurchBody) //.Include(t => t.ChurchMember)
                //                       .Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.Username == username)
                //             from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == t_up.ChurchMemberId)
                //             select t_up
                //                      ).FirstOrDefault();
                //}

                if (AppUtilties.ComputeSha256Hash(churchCode) == ac1) //&& AppUtilties.ComputeSha256Hash(churchCode + username) == ac2 && AppUtilties.ComputeSha256Hash(churchCode + username + password) == ac3)  //6-digit vendor code 6-digit code for churches ... [church code: 0000000000 + ?? userid + ?? pwd] + no existing SUPADMIN user ... pop up SUPADMIN for new SupAdmin()
                {
                    // string strRootCode = AppUtilties.ComputeSha256Hash(model.ChurchCode); //church code ...0x6... 91b4d142823f7d20c5f08df69122de43f35f057a988d9619f6d3138485c9a203
                    //  string _strRootCode = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username);  // user  ...0x6... f7b11509f4d675c3c44f0dd37ca830bb02e8cfa58f04c46283c4bfcbdce1ff45
                    //  string strRootCode0 = AppUtilties.ComputeSha256Hash(model.ChurchCode + model.Username + model.Password);  // pwd ...$0x6... 78415a1535ca0ef885aa7c0278a4de274b85d0c139932cc138ba6ee5cac4a00b

                    //var userList = (from t_up in _context.UserProfile.Where(c => c.ProfileScope == "V" && c.UserStatus == "A")
                    //                from t_upr in _context.UserProfileRole.Where(c => c.UserProfileId == t_up.Id && c.ProfileRoleStatus == "A")
                    //                from t_ur in _context.UserRole.Where(c => c.Id == t_upr.UserRoleId && c.RoleStatus == "A" && c.RoleLevel == 1 && c.RoleType == "SUP_ADMN")
                    //                select t_up
                    //                ).ToList();

                    ////if no SUP_ADMIN... create one and only 1... then create other users
                    //if (userList.Count == 0)
                    //{
                    //    //...
                    //}
                    //else
                    //{

                    oUser = (from t_up in _context.UserProfile.Include(t => t.ChurchBody)  //.Include(t => t.ChurchMember)
                                  .Where(c => c.ProfileScope == "V" && c.UserStatus == "A" && c.Username == username && c.Pwd.Trim() == strPwdHashedData.Trim())
                                 // from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == t_up.ChurchMemberId)
                             select t_up
                                 ).FirstOrDefault();

                    //}
                }

                else
                {
                    //  oUser = _context.UserProfile
                    //.Include(u => u.ChurchBody).Include(u => u.ChurchMember)
                    //.Where(c => c.ChurchBody.ChurchCode == churchCode && c.ChurchMember.IsCurrent && c.Username == username && c.Pwd.Trim() == strPwdHashedData.Trim() && c.UserStatus == "A").FirstOrDefault();


                    //oUser = (from t_up in _context.UserProfile.Include(t => t.ChurchBody)  //.Include(t => t.ChurchMember)
                    //                   .Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.ProfileScope == "C" && c.Username == username && c.Pwd.Trim() == strPwdHashedData.Trim() && c.UserStatus == "A")
                    //         from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == t_up.ChurchMemberId)
                    //         select t_up
                    //                  ).FirstOrDefault();

                    oUser = (from t_up in _context.UserProfile  //.Include(t => t.ChurchBody)  //.Include(t => t.ChurchMember)
                                       .Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.ProfileScope == "C" && c.UserStatus == "A" && c.Username == username && c.Pwd.Trim() == strPwdHashedData.Trim())
                             //from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == t_up.ChurchMemberId)
                             select t_up
                                      ).FirstOrDefault();

                    //if (oUser != null) {
                    //    var chkUserMemExist = (
                    //        from t_ms in _clientDBContext.MemberStatus.Where(c => c.ChurchBody.GlobalChurchCode == churchCode && c.IsCurrent == true && c.ChurchMemberId == oUser.ChurchMemberId)
                    //        select t_ms
                    //                 ).FirstOrDefault();

                    //    //making sure user is active member of the church  ... might not be compulsory anyway... cos church may employ persons from other faiths
                    //    oUser = chkUserMemExist != null ? oUser : null; 
                    //}
                    
                }


                //var oUser = new UserProfile();
                //var oUserRole = new UserProfileRole();           

                if (oUser != null)
                { 
                        var oLoggedPrivileges = AppUtilties.GetUserPrivilege(_context, _clientDBContext, churchCode, oUser);

                        if (oLoggedPrivileges?.Count > 0) //i.e. at least have a permission == from groups and roles
                        {
                            // TempData.Put("oUserProRoleLogIn", oUserProRoleLog);
                            return oLoggedPrivileges;
                        }                   
                }

                return null; //ViewBag.UserPromptMsg = "Invalid credentials provided. Enter right username and password.";



                //// This is a simple single-user system
                //if (username == "keyvan" && password == "pas$word")
                //    return true;
                //return false;

            }
            catch (Exception ex)
            {
                return null; // ex;
            }
        }


        public static string GetUserPhone(MSTR_DbContext _context, string username)
        {
            UserProfile oUserLog = _context.UserProfile.Include(t=>t.ContactInfo).Where(c => c.Username == username
            //&& c.Pwd.Trim() == strPwdHashedData.Trim() 
            && c.UserStatus == "A")
                .FirstOrDefault();

            if (oUserLog != null)
            {
                if (oUserLog.ContactInfo != null)
                {
                    if (!string.IsNullOrEmpty(oUserLog.ContactInfo.MobilePhone1))
                        return oUserLog.ContactInfo.MobilePhone1;
                    else
                        return oUserLog.ContactInfo.MobilePhone2;
                } 
            }
            // This is a simple single-user system
            // if (username == "keyvan") return "+1YOURPHONE";

            return string.Empty;
        }

        public static string ReadValidationCode(string username)
        {
            // string path = HttpContext.Current.Server.MapPath("~/App_Data/usercodes.xml");
            // string path = _context.Request.PathBase
            //XDocument doc = XDocument.Load(path);
            //string code = (from u in doc.Element("Users").Descendants("User")
            //               where u.Attribute("name").Value == username
            //               select u.Attribute("code").Value).SingleOrDefault();

            string code = "";

            return code;
        }

        public static void StoreValidationCode(string username, string code)
        {
            //string path = HttpContext.Current.Server.MapPath("~/App_Data/usercodes.xml");

            //XDocument doc = XDocument.Load(path);
            //XElement user = (from u in doc.Element("Users").Descendants("User")
            //                 where u.Attribute("name").Value == username
            //                 select u).SingleOrDefault();

            //if (user != null)
            //{
            //    user.Attribute("code").SetValue(code);
            //}
            //else
            //{
            //    XElement newUser = new XElement("User");
            //    newUser.SetAttributeValue("name", username);
            //    newUser.SetAttributeValue("code", code);
            //    doc.Element("Users").Add(newUser);
            //}
            //doc.Save(path); 
        }




        public static class TrustedClients
        {
            public static bool ValidateClient(string username, string ip,
                string useragent)
            {
                //string path = HttpContext.Current.
                //      Server.MapPath("~/App_Data/trustedclients.xml");

                //XDocument doc = XDocument.Load(path);
                //var client = (from c in doc.Element("Clients").Descendants("Client")
                //              where
                //              ((c.Attribute("username").Value == username) &&
                //              (c.Attribute("ip").Value == ip) &&
                //              (c.Attribute("useragent").Value == useragent))
                //              select c).SingleOrDefault();

                //if (client != null)
                //    return true;
                return false;
            }

            public static void AddClient(string username, string ip, string useragent)
            {
                //string path = HttpContext.Current.
                //    Server.MapPath("~/App_Data/trustedclients.xml");

                //XDocument doc = XDocument.Load(path);
                //XElement newClient = new XElement("Client");

                //newClient.SetAttributeValue("username", username);
                //newClient.SetAttributeValue("ip", ip);
                //newClient.SetAttributeValue("useragent", useragent);

                //doc.Element("Clients").Add(newClient);

                //doc.Save(path);
            }
 

        }





        //using Twilio;
        //public static class TwilioMessenger
        //{
        //    public static void SendTextMessage(string number, string message)
        //    {
        //        TwilioRestClient twilioClient =
        //            new TwilioRestClient("acccountSid", "authToken");
        //        twilioClient.SendSmsMessage("+12065696562", number, message);
        //    }
        //}



        public static bool SendEmailNotification(string senderId, string strSubject, string strMailMess,
            MailAddressCollection lsToAddr, MailAddressCollection lsCcAddr, MailAddressCollection lsBccAddr, string docAttachFilePath)
        //HttpPostedFileBase fileUploader) SendMailwithAttachment.Models.MailModel objModelMail
        {
            try
            {
                string strFrom = "samdartgroup@gmail.com"; //example:- sourabh9303@gmail.com  //  var fromAddr = new MailAddress(from, "RHEMACLOUD"); 

                using (MailMessage mail = new MailMessage()) // strFrom, senderId))
                {
                    mail.From = new MailAddress(strFrom, senderId);
                    mail.Subject = strSubject;
                    mail.Body = strMailMess;
                    foreach (var oTo in lsToAddr) mail.To.Add(oTo);
                    foreach (var oCc in lsCcAddr) mail.To.Add(oCc);
                    foreach (var oBcc in lsBccAddr) mail.To.Add(oBcc);

                    //if (fileUploader != null)
                    //{
                    //    string fileName = Path.GetFileName(fileUploader.FileName);
                    //    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                    //}

                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(strFrom, "Sdgh1284");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = networkCredential;
                    smtp.Port = 587;
                    smtp.Send(mail);
                    // ViewBag.Message = "Sent";

                    return true;  // View("Index", objModelMail);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public static class CodeGenerator
        {
            public static string GenerateCode()
            {
                List<char> chars = new List<char>();

                chars.AddRange(GetUpperCaseChars(4));
                chars.AddRange(GetNumericChars(4));

                return GenerateCodeFromList(chars);
            }

            private static List<char> GetUpperCaseChars(int count)
            {
                List<char> result = new List<char>();
                Random random = new Random();

                for (int index = 0; index < count; index++)
                {
                    result.Add(Char.ToUpper(Convert.ToChar(random.Next(97, 122))));
                }

                return result;
            }

            private static List<char> GetLowerCaseChars(int count)
            {
                List<char> result = new List<char>();

                Random random = new Random();

                for (int index = 0; index < count; index++)
                {
                    result.Add(Char.ToLower(Convert.ToChar(random.Next(97, 122))));
                }

                return result;
            }

            private static List<char> GetNumericChars(int count)
            {
                List<char> result = new List<char>();

                Random random = new Random();

                for (int index = 0; index < count; index++)
                {
                    result.Add(Convert.ToChar(random.Next(0, 9).ToString()));
                }

                return result;
            }

            private static string GenerateCodeFromList(List<char> chars)
            {
                string result = string.Empty;

                Random random = new Random();

                while (chars.Count > 0)
                {
                    int randomIndex = random.Next(0, chars.Count);
                    result += chars[randomIndex];
                    chars.RemoveAt(randomIndex);
                }

                return result;
            }
        }



    }
}
