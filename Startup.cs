using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MonetaAPI.Data;
using MonetaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;

namespace MonetaAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(opt => {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 5, 0);
            });

            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddDbContext<UserContext>(opt => { opt.UseSqlServer(Configuration.GetConnectionString("AzureDatabase")); });
            services.AddDbContext<CountriesContext>(opt => { opt.UseSqlServer(Configuration.GetConnectionString("AzureDatabase")); });
            services.AddDbContext<TransactionContext>(opt => { opt.UseSqlServer(Configuration.GetConnectionString("AzureDatabase")); });
            services.AddDbContext<BlogContext>(opt => { opt.UseSqlServer(Configuration.GetConnectionString("AzureDatabase")); });

            services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<UserContext>();

            services.AddMvc();
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins( new string[] { Configuration["ApplicationSettings:Local_URL"].ToString(), Configuration["ApplicationSettings:Client_URL"].ToString() });
                        builder.WithHeaders("Access-Control-Allow-Methods", "GET,HEAD,OPTIONS,POST,PUT");
                        builder.AllowAnyHeader();
                    });
            });

            var secret = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x => 
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
                //x.Authority = "https://monetastudio.us.auth0.com/api/v2/";
                //x.Audience = "https://monetaapi.azurewebsites.net";
                //x.Audience = "http://localhost:49755";
            });
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
                app.UseExceptionHandler("/Home");
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
