using AutoMapper;
using TemplateProject.Data;
using TemplateProject.EntityModels;
using TemplateProject.EnumsAndConstants;
using TemplateProject.Extensions;
using TemplateProject.Helpers;
using TemplateProject.Mappings;
using TemplateProject.Services;
using TemplateProject.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace TemplateProject
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

            services.AddSqlServerConnectionService(Configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TemplateProjectDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityOptionService();

            services.AddAuthorizationService();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "TemplateProject";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                //ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.Configure<PasswordHasherOptions>(option =>
            {
                option.IterationCount = 12000;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add application services.
            //Helper
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddHelperDependancyService();

            //Repository
            services.AddRepositoryDependancyService();

            //Manager
            services.AddManagerDependancyService();

            CreateDefaultAuthenticationAndAuthorization(services);

            SeedData(services);

            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());

            services.AddAutoMapper();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //creates default roles if not exist
        public async void CreateDefaultAuthenticationAndAuthorization(IServiceCollection services)
        {
            RoleManager<IdentityRole> roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();


            //Add Default Role
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            #region Add All Permission to Default Role Admin
            //Add Permission To Roles

            List<string> controllerListForAdmin = new List<string>()
            {
                "Account:","Dashboard:",
                "Employee:","Home:","Manage:",
                "Role:"
            };

            AddPermissionToRoleAsync(controllerListForAdmin, RoleConstants.Admin, services);
            #endregion



            UserManager<ApplicationUser> userManager =
                services.BuildServiceProvider().GetRequiredService<UserManager<ApplicationUser>>();

            //Add Default User With Default Role
            var existingUser = await userManager.FindByEmailAsync("refatmustaque@gmail.com");

            if (existingUser == null)
            {
                var user = new ApplicationUser { UserName = "refatmustaque@gmail.com", Email = "refatmustaque@gmail.com" };

                var result = await userManager.CreateAsync(user, "Ab@12345");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        private async void AddPermissionToRoleAsync(List<string> controllerList, string admin, IServiceCollection services)
        {
            RoleManager<IdentityRole> roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();

            var role = await roleManager.FindByNameAsync(admin);

            var permittedClaimList = roleManager.GetClaimsAsync(role);

            ClaimHelper claimHelper = new ClaimHelper();

            foreach(var controller in controllerList)
            {
                var claimList = claimHelper.GetAllAvailableClaims().Where(c => c.Value.Contains(controller));

                foreach (var claim in claimList)
                {
                    if (!permittedClaimList.Result.Any(x => x.Type == claim.Value))
                    {
                        await roleManager.AddClaimAsync(role, new Claim(claim.Value, role.Id));
                    }
                }
            }
        }

        public void SeedData(IServiceCollection services)
        {
            UnitOfWork _unitOfWork = services.BuildServiceProvider().GetRequiredService<UnitOfWork>();

            #region Seed branch
            if (!_unitOfWork.BranchManager.IsExistFirstOrDefault(c => c.Name == "Gulshan Branch"))
            {
                _unitOfWork.BranchManager.Save(new Branch() { Name = "Gulshan Branch" });
            }

            if (!_unitOfWork.BranchManager.IsExistFirstOrDefault(c => c.Name == "Mirpur Branch"))
            {
                _unitOfWork.BranchManager.Save(new Branch() { Name = "Mirpur Branch" });
            }

            if (!_unitOfWork.BranchManager.IsExistFirstOrDefault(c => c.Name == "Dhanmondi Branch"))
            {
                _unitOfWork.BranchManager.Save(new Branch() { Name = "Dhanmondi Branch" });
            }
            #endregion

            #region Seed Employee
            var gulshanBranch = _unitOfWork.BranchManager.GetFirstOrDefault(x => x.Name == "Gulshan Branch");
            List<Employee> Employees = new List<Employee>
            {
                new Employee{Id= _unitOfWork.EmployeeManager.GenerateId("EMP", 10), Name="Emoloyee One",Department="Doctor",BranchId=gulshanBranch.Id},
                new Employee{Id= _unitOfWork.EmployeeManager.GenerateId("EMP", 10), Name="Emoloyee Two",Department="Doctor",BranchId=gulshanBranch.Id},
                new Employee{Id= _unitOfWork.EmployeeManager.GenerateId("EMP", 10), Name="Emoloyee Three",Department="Doctor",BranchId=gulshanBranch.Id},
                new Employee{Id= _unitOfWork.EmployeeManager.GenerateId("EMP", 10), Name="Emoloyee Four",Department="Doctor",BranchId=gulshanBranch.Id},
                new Employee{Id= _unitOfWork.EmployeeManager.GenerateId("EMP", 10), Name="Emoloyee Five",Department="Doctor",BranchId=gulshanBranch.Id}
            };

            foreach (var item in Employees)
            {
                if (!_unitOfWork.EmployeeManager.IsExistFirstOrDefault(c => c.Name == item.Name))
                {
                    _unitOfWork.EmployeeManager.Save(item);
                }
            }
            #endregion
        }
    }
}

