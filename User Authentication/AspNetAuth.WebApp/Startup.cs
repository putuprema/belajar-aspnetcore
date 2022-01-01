using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetAuth.Shared.Classes;
using AspNetAuth.WebApp.Constants;
using AspNetAuth.WebApp.Interfaces;
using AspNetAuth.WebApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AspNetAuth.WebApp
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
            services.AddHttpContextAccessor();

            services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            // Configure Token Authentication & Authorization
            var jwtConfig = Configuration.GetSection("JWT").Get<JwtConfig>();
            services.AddSingleton(jwtConfig);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidIssuer = jwtConfig.ValidIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
                        ClockSkew = TimeSpan.FromSeconds(60)
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[Defaults.AccessTokenCookieKey];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddHttpClient(Defaults.DefaultHttpClientName, client =>
            {
                client.BaseAddress = new Uri(Configuration["ApiBaseAddress"]);
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBlogService, BlogService>();
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

            // Enable Authentication Middleware
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}