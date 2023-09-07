using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
// using api.Context;
using System.Reflection;
using AutoMapper;
// using api.CronJob;
using api.Infrastructure;
// using api.Middlewares;
// using api.Repositories.Abstraction;
// using api.Repositories.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog.Sinks.Elasticsearch;
using Serilog.Formatting.Elasticsearch;
//using api.Hubs;

namespace api
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
            AppSettings config = new AppSettings();
            Configuration.Bind("AppSettings", config);
            services.AddSingleton(config);
            services.AddHttpClient();
            string year = DateTime.Now.Year.ToString();

            services.AddControllers(); // or services.AddControllersWithViews();

            // services.AddCronJob<MyCronJob>(c =>
            // {
            //     c.TimeZoneInfo = TimeZoneInfo.Local;
            //     c.CronExpression = @"* * * * *";
            // });

            services.AddAutoMapper(typeof(Startup));
            // dependencies injection
            //services.AddTransient<IReportRepository, ReportRepository>();
            //services.AddTransient<IFoodCouponRepository, FoodCouponRepository>();
            //services.AddTransient<IRegisterRepository, RegisterRepository>();
            //services.AddTransient<IFactoryRepository, FactoryRepository>();
            //services.AddTransient<ILineService, LineService>();
            //services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IMasterDataRepository, MasterDataRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(config.AppConfig["Version"],
                    new OpenApiInfo { Title = $"Learning API by I,P,N and P", Version = config.AppConfig["Version"] });
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Please enter x-api-key",
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey
                //});
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        new string[] { }
                //    }
                //});
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                //c.IncludeXmlComments(xmlPath);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("all",
                    builder =>
                    {
                        builder.
                        AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            ;
                    });
            });

            //services.AddSignalR();

            // Authenticate to Central Payment Gateway
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            //string oauthUrl = config.AppConfig["oauthUrl"];
            //var lineOAuthUrl = config.AppConfig["LineOAuthUrl"];
            //var key = Encoding.UTF8.GetBytes(config.AppConfig["Token"]);

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    //x.Authority = oAuthServer;
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {

            // if (env.IsDevelopment())
            //     app.UseDeveloperExceptionPage();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Content")),
                RequestPath = "/Content"
            });

            if (env.IsDevelopment() || bool.Parse(Configuration["IsShowSwagger"]))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"./swagger/{Configuration["AppSettings:AppConfig:Version"]}/swagger.json", "API");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("all");

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseSession();

            app.UseAuthentication();

            //app.UseAuthorization();

            app.UseHsts();

            // TODO : implement serilog middleware
            //app.UseRequestLogMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<DataHub>("/chart");
            });
        }
    }
}