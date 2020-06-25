using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if (OrganizationalAuth || IndividualB2CAuth)
using Microsoft.AspNetCore.Authentication;
#endif
#if (OrganizationalAuth)
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
#if (MultiOrgAuth)
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
#endif
using Microsoft.AspNetCore.Authorization;
#endif
#if (IndividualB2CAuth)
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
#endif
using Microsoft.AspNetCore.Builder;
#if (IndividualLocalAuth)
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
#endif
using Microsoft.AspNetCore.Hosting;
#if (RequiresHttps)
using Microsoft.AspNetCore.HttpsPolicy;
#endif
#if (OrganizationalAuth)
using Microsoft.AspNetCore.Mvc.Authorization;
#endif
#if (IndividualLocalAuth)
using Microsoft.EntityFrameworkCore;
using Company.WebApplication1.Data;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if(MultiOrgAuth)
using Microsoft.IdentityModel.Tokens;
#endif
#if (GenerateApi)
using Company.WebApplication1.Services;
#endif

namespace Company.WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if (IndividualLocalAuth)
            services.AddDbContext<ApplicationDbContext>(options =>
#if (UseLocalDB)
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
#else
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
#endif
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
#elif (OrganizationalAuth)
            services.AddSignIn(Configuration, "AzureAd");
#if (GenerateApi)
            services.AddWebAppCallsProtectedWebApi(Configuration, 
                                                  "AzureAd")
                    .AddInMemoryTokenCaches();

            services.AddDownstreamWebApiService(Configuration);
#endif
#elif (IndividualB2CAuth)
            services.AddSignIn(Configuration, "AzureAdB2C");
#if (GenerateApi)
            services.AddWebAppCallsProtectedWebApi(Configuration, 
                                                  "AzureAdB2C")
                    .AddInMemoryTokenCaches();

            services.AddDownstreamWebApiService(Configuration);
#endif
#endif
#if (OrganizationalAuth)

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
#else
            services.AddControllersWithViews();
#endif
#if (OrganizationalAuth || IndividualAuth)
           services.AddRazorPages()
                   .AddMicrosoftIdentityUI();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
#if (IndividualLocalAuth)
                app.UseDatabaseErrorPage();
#endif
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
#if (RequiresHttps)
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
#else
            }
#endif
            app.UseStaticFiles();

            app.UseRouting();

#if (OrganizationalAuth || IndividualAuth)
            app.UseAuthentication();
#endif
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
#if (OrganizationalAuth || IndividualAuth)
                endpoints.MapRazorPages();
#endif
            });
        }
    }
}
