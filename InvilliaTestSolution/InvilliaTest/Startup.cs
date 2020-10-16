using AutoMapper;
using DataAccess;
using DataAccess.Repository;
using Domain.Interfaces;
using Domain.Service;
using Domain.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Security;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace InvilliaTest
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
            services.AddCors();
            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddLogging(c =>
            {
                c.ClearProviders();
                c.AddConsole();
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Borrowing Games API",
                    Description = "This API is used to manage games and friends who borrow the games",

                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT with Bearer Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services
                .AddDbContext<BorrowedGamesContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("BorrowedGamesContext")));

            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services
                .AddAuthentication(c =>
                {
                    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(c =>
                {
                    c.RequireHttpsMetadata = false;
                    c.SaveToken = true;
                    c.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,

                    };
                });

            services.AddScoped<IUnityOfWork, UnityOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IBorrowGameService, BorrowGameService>();
            services.AddScoped<IPasswordService, PasswordService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, BorrowedGamesContext borrowedGamesContext)
        {
            EnsureDbMigration(borrowedGamesContext, logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invillia API");
                c.RoutePrefix = string.Empty;
            });
        }

        private void EnsureDbMigration(BorrowedGamesContext dbContext, ILogger<Startup> logger, int tentatives = 1)
        {
            logger.LogInformation($"Attempting to Migrate Database. Tentative: {tentatives}");
            int count = tentatives;
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Thread.Sleep(30000);
                count += 1;
                if (count > 5)
                    throw ex;
                EnsureDbMigration(dbContext, logger, count);
            }
        }
    }
}
