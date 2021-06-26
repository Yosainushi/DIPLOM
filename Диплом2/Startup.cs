using Диплом2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Диплом2.Controllers;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;

namespace Диплом2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        

        public void ConfigureServices(IServiceCollection services)
        {
           
                string connection = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connection));

            services.AddSignalR();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => //CookieAuthenticationOptions
                {
                        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    });
                services.AddControllersWithViews();
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseDefaultFiles();
            app.UseRouting();

            app.UseAuthentication();    // àóòåíòèôèêàöèÿ
            app.UseAuthorization();     // àâòîðèçàöèÿ

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
           
        }
    }
}