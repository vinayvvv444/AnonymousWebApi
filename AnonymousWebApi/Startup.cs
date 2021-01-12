using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AnonymousWebApi.ActionFilters;
using AnonymousWebApi.Data.Contracts;
using AnonymousWebApi.Data.Contracts.ContratModels;
using AnonymousWebApi.Data.Contracts.Master;
using AnonymousWebApi.Data.DomainModel;
using AnonymousWebApi.Data.DomainModel.Master;
using AnonymousWebApi.Data.EFCore;
using AnonymousWebApi.Data.EFCore.Repository;
using AnonymousWebApi.Data.EFCore.Repository.Master;
using AnonymousWebApi.Helpers.EmailService;
using AnonymousWebApi.Helpers.ExtensionMethods;
using AnonymousWebApi.MappingProfiles;
using AnonymousWebApi.Models;
using AnonymousWebApi.Services;
using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;

namespace AnonymousWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*--------------------------------------------------------------------------------------------------------------------*/
            /*                      Anti Forgery Token Validation Service                                                         */
            /* We use the option patterm to configure the Antiforgery feature through the AntiForgeryOptions Class                */
            /* The HeaderName property is used to specify the name of the header through which antiforgery token will be accepted */
            /*--------------------------------------------------------------------------------------------------------------------*/
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });
            services.ConfigureLoggerService();

            services.AddMemoryCache();
            services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");
            //services.AddProblemDetails();
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Anonymous API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      Array.Empty<string>()
    }
  });
            });

            //Inject AppSettings
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

            //services.AddDbContext<AuthenticationContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddDbContext<AnonymousDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddTransient<ICountryCommandText, CountryCommandText>();
            services.AddScoped<UserAddressRepository>();
            services.AddScoped<CountryRepository>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            services.ConfigureRepositoryServices();
            services.ConfigureQuartzServices();
            services.AddScoped<ValidateEntityExistsAttribute<Country>>();


            var emailConfig = Configuration
         .GetSection("EmailConfiguration")
         .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddScoped<IEmailSender, EmailSender>();

            // inject auto mapper
            //services.AddAutoMapper();
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AnonymousDBContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            }
            );

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("IdentityConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddCors();

            //Jwt Authentication

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddApplicationInsightsTelemetry();

            

            // profiling
            //services.AddMiniProfiler(options =>
            //   options.RouteBasePath = "/profiler"
            //);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IBackgroundJobClient backgroundJobs,
            IRecurringJobManager recurringJobManager,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            ILoggerManager logger,
            IAntiforgery antiforgery)
        {
            /* Configure the app to provide a token in a cookie called XSRF-TOKEN */
            /* Custom Middleware Component is required to Set the cookie which is named XSRF-TOKEN 
             * The Value for this cookie is obtained from IAntiForgery service
             * We must configure this cookie with HttpOnly option set to false so that browser will allow JS to read this cookie
             */
            app.Use(nextDelegate => context =>
            {
                string path = context.Request.Path.Value;
                //string[] directUrls = { "/admin", "/store", "/cart", "checkout", "/login" };
                // if (path.StartsWith("/swagger") || path.StartsWith("/api") || string.Equals("/", path))
                if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) || path.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
                {
                    //AntiforgeryTokenSet tokens = antiforgery.GetAndStoreTokens(context);
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions()
                    {
                        HttpOnly = false
                        //Path = "/",
                        //Secure = true,
                        //IsEssential = true,
                        //SameSite = SameSiteMode.Strict
                    });

                }

                return nextDelegate(context);
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseProblemDetails(); // Add the middleware

            app.UseHttpsRedirection();

            //app.ConfigureExceptionHandler(logger);
            app.ConfigureCustomExceptionMiddleware();

            app.UseCors(builder =>
            builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
            .AllowAnyHeader()
            .AllowAnyMethod()
            );

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireDashboard();
            // backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            //recurringJobManager.AddOrUpdate("Run every minute",
            //    () => serviceProvider.GetService<CountryRepository>().GetAll(),
            //    "* * * * *"); 

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //c.RoutePrefix = "api-doc";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Anonymous API V1");
                // this custom html has miniprofiler integration
                //c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("SwaggerIndex.html");
            });

         

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            // profiling, url to see last profile check: http://localhost:xxxxx/profiler/results
            app.UseMiniProfiler();
        }
    }
}
