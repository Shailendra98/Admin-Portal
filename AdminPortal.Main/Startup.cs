using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TKW.AdminPortal.Constants;
using TKW.AdminPortal.Security.Authentication;
using TKW.AdminPortal.Utils;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.MaterialContext.Services;
using TKW.ApplicationCore.Contexts.PaymentContext.Services;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Services;
using TKW.ApplicationCore.Contexts.InventoryContext.Services;
using TKW.ApplicationCore.Contexts.AreaContext.Services;
using TKW.ApplicationCore.Enums;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Settings;
using TKW.Infrastructure;
using TKW.ApplicationCore.Contexts.SellContext.Services;
using TKW.ApplicationCore.Contexts.ExpenseContext.Services;
using TKW.ApplicationCore.Contexts.FranchiseContext.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using TKW.ApplicationCore.Contexts.IncentiveContext.Services;
using TKW.Queries;

namespace AdminPortal
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
            services.AddInfrastructureServices(Configuration);
            services.AddQueries(Configuration);
            services.AddDistributedMemoryCache();
            services.AddSession();
            services
                .AddAuthorization(options =>
                     {
                         options.AddPolicy(AuthorizationPolicies.GlobalAdminRoles, policy => policy.RequireRole(UserRole.Admin.ToString(), UserRole.TeleCaller.ToString()));
                         options.AddPolicy(AuthorizationPolicies.AdminRoles, policy => policy.RequireRole(UserRole.Admin.ToString(), UserRole.FranchiseAdmin.ToString(), UserRole.TeleCaller.ToString()));
                         options.DefaultPolicy = options.GetPolicy(AuthorizationPolicies.AdminRoles);
                     }).AddAuthentication(options =>
                     {
                         options.DefaultAuthenticateScheme = AuthScheme.SessionToken_Cookie;
                         options.DefaultChallengeScheme = AuthScheme.SessionToken_Cookie;
                     })
                     .AddScheme<SessionTokenAuthenticationOptions,SessionToken_CookieAuthenticationHandler>(AuthScheme.SessionToken_Cookie, options => { });

            services.Configure<PhotoSetting>(Configuration.GetSection("PhotoSetting"));
            services.Configure<PaytmSetting>(Configuration.GetSection("PaytmSetting"));
            services.Configure<MSG91Setting>(Configuration.GetSection("MSG91Setting"));
            services.AddSingleton(provider => Configuration.GetSection(nameof(GeneralSetting)).Get<GeneralSetting>());
            services.AddScoped<PhotoSetting>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<UrlGenerator>();
            services.AddScoped<SessionData>();
            services.AddScoped<IAppUserService, AppUserService>();

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBuyerService, BuyerService>();
            services.AddTransient<ILocalityService, LocalityService>();
            services.AddTransient<IPincodeService, PincodeService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IMaterialService, MaterialService>();
            services.AddTransient<IWarehouseService, WarehouseService>();
            services.AddTransient<IPaymentTransactionService, PaymentTransactionService>();
            services.AddTransient<IPickupSessionService, PickupSessionService>();
            services.AddTransient<ISellService, SellService>();
            services.AddTransient<IExpenseService, ExpenseService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IIncentiveService, IncentiveService>();
            
            TypeDescriptor.AddAttributes(typeof(DateTime), new TypeConverterAttribute(typeof(TKW.AdminPortal.TypeConverters.DateTimeConverter)));
            TypeDescriptor.AddAttributes(typeof(DateTime?), new TypeConverterAttribute(typeof(TKW.AdminPortal.TypeConverters.DateTimeConverter)));

            services.AddHttpClient();

            services.AddRazorPages()
                .AddRazorPagesOptions(options => {
                    options.Conventions.AuthorizeAreaFolder("Request", "/");
                    options.Conventions.AuthorizeAreaFolder("Employee", "/");
                    options.Conventions.AuthorizeAreaFolder("Warehouse", "/");
                    options.Conventions.AuthorizeAreaFolder("Material", "/");
                    options.Conventions.AuthorizeAreaFolder("Sell", "/");
                    options.Conventions.AuthorizeAreaFolder("Expense", "/");
                }).AddMvcOptions(options=> {
                    options.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>();
                });
            
            services.AddControllers(options=> {
                options.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseSession();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            var cultureInfo = new CultureInfo("en-IN");
            cultureInfo.NumberFormat.CurrencySymbol = "₹";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
