using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebAppEs.Data;
using WebAppEs.Handlers;
using WebAppEs.Models;
using WebAppEs.Services;

namespace WebAppEs
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
			services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

			services.AddDistributedMemoryCache();

			

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromHours(5);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;

			});

			services.AddAuthentication()
			.Services.ConfigureApplicationCookie(options =>
			{
				options.SlidingExpiration = true;
				options.ExpireTimeSpan = TimeSpan.FromHours(5);
			});

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
			});
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login";

			});

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

		

			services.AddDefaultIdentity<ApplicationUser>(options =>
				{
					options.SignIn.RequireConfirmedAccount = false;
					options.Password.RequireDigit = false;
					options.Password.RequiredLength = 4;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;
					options.Password.RequireLowercase = false;
				})
				
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();



			services.AddControllersWithViews().AddRazorRuntimeCompilation();
			services.AddRazorPages();
			services.AddMemoryCache();

			services.AddScoped<IDataAccessService, DataAccessService>();
			services.AddScoped<IAuthorizationHandler, PermissionHandler>();
			services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

			//when we use single policy it needs to register like this
			//services.AddAuthorization(options =>
			//{
			//	options.AddPolicy("Authorization", policyCorrectUser =>
			//	{
			//		policyCorrectUser.Requirements.Add(new AuthorizationRequirement());
			//	});
			//});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();

				DbInitializer.Initialize(app);
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseSession();

			app.UseEndpoints(endpoints =>
			{
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapRazorPages();
			});
		}
	}
}