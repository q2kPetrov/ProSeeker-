﻿namespace ProSeeker.Web
{
    using System.Reflection;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using ProSeeker.Data;
    using ProSeeker.Data.Common;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Data.Seeding;
    using ProSeeker.Services.Data;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Data.BaseJobCategories;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Services.Data.Cloud;
    using ProSeeker.Services.Data.Home;
    using ProSeeker.Services.Data.Inquiries;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Services.Data.Opinions;
    using ProSeeker.Services.Data.PrivateChat;
    using ProSeeker.Services.Data.Quizz;
    using ProSeeker.Services.Data.Raitings;
    using ProSeeker.Services.Data.ServicesService;
    using ProSeeker.Services.Data.Specialists;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Services.Data.Votes;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Services.Messaging;
    using ProSeeker.Web.Hubs;
    using ProSeeker.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            // TODO: Add Privacy page and add implement facebook/google APIs
            services.AddAuthentication();

            // .AddFacebook(facebookOptions =>
            // {
            //    facebookOptions.AppId = this.configuration["Authentication:Facebook:AppId"];
            //    facebookOptions.AppSecret = this.configuration["Authentication:Facebook:AppSecret"];
            // })
            // .AddGoogle(googleOptions =>
            // {
            //    googleOptions.ClientId = this.configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = this.configuration["Authentication:Google:ClientSecret"];
            // });

            // Cloudinary Authentication
            var clodAccount = new CloudinaryDotNet.Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:Secret"]);
            var cloudinary = new Cloudinary(clodAccount);
            services.AddSingleton(cloudinary);

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();

            services.AddSignalR(options =>
               {
                   options.EnableDetailedErrors = true;
               });

            // CSRF
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Register all application services (Dependency container)
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<ICloudinaryApplicationService, CloudinaryApplicationService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IServicesService, ServicesService>();
            services.AddTransient<IRatingsService, RatingsService>();
            services.AddTransient<IAdsService, AdsService>();
            services.AddTransient<ICitiesService, CitiesService>();
            services.AddTransient<IOffersService, OffersService>();
            services.AddTransient<IOpinionsService, OpinionsService>();
            services.AddTransient<IVotesService, VotesService>();
            services.AddTransient<IInquiriesService, InquiriesService>();
            services.AddTransient<ISpecialistsService, SpecialistsService>();
            services.AddTransient<IPrivateChatService, PrivateChatService>();
            services.AddTransient<ISurveysService, SurveysService>();
            services.AddTransient<IBaseJobCategoriesService, BaseJobCategoriesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<PrivateChatHub>("/privateChatHub");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("JobCategories", "jobcategories/{name:minlength(2)}", new { controller = "JobCategories", action = "ByName" });
                        endpoints.MapRazorPages();
                    });
        }
    }
}
