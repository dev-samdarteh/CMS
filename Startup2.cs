using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RhemaCMS.Models;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;

namespace RhemaCMS
{
    public class Startup2
    {
        public Startup2(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider();

            services.AddControllersWithViews();

            services.AddDistributedMemoryCache();

            services.AddDbContext<MSTR_DbContext>(options
               => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            /////  multiple dbcontext connections

            //services.AddDbContext<ChurchModelContext>((serviceProvider, dbContextBuilder) =>
            //{
            //    var connStringPlaceHolder = Configuration.GetConnectionString("DefaultConnection");
            //    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            //    var dbName = httpContextAccessor.HttpContext.Request.Headers["entityId"].First();
            //    var connString = connStringPlaceHolder.Replace("{dbName}", dbName);
            //    dbContextBuilder.UseSqlServer(connString);
            //});

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSession(x =>
            {
                x.Cookie.Name = ".RhemaCMS.Session";
                x.IdleTimeout = TimeSpan.FromSeconds(100 * 5 * 60);  //5 m
                x.Cookie.IsEssential = true;
            });
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
