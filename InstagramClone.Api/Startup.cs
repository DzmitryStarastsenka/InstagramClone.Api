using FluentValidation;
using FluentValidation.AspNetCore;
using InstagramClone.Api.Extensions;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Services.Post;
using InstagramClone.Application.Services.Post.Interfaces;
using InstagramClone.Application.Services.User;
using InstagramClone.Application.Services.User.Interfaces;
using InstagramClone.Application.Services.User.Providers;
using InstagramClone.Application.Sieve;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.Jwt;
using InstagramClone.Domain.Jwt.Impl;
using InstagramClone.Domain.Jwt.Interfaces;
using InstagramClone.Domain.UserProviders;
using InstagramClone.Infrastructure.DAL;
using InstagramClone.Infrastructure.DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace InstagramClone.Api
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
            services.AddDbContext<InstagramCloneDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvcCore().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ValidateTokenRequestValidator>());
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-Us");

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => JWTHandler.ConfigureJwtBearerOptions(options, Configuration.GetSection("JwtToken")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InstagramClone", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                  {
                    new OpenApiSecurityScheme
                    {
                      Reference = new OpenApiReference
                        {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                      },
                      new List<string>()
                    }
                  });
            });

            services.AddScoped<IUserJwtTokenGenerator, AuthJwtTokenGenerator>();

            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilters>();
            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

            services.AddScoped<IRepository<UserProfile>, EntityRepository<UserProfile>>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRepository<UserPost>, EntityRepository<UserPost>>();
            services.AddScoped<IRepository<PostComment>, EntityRepository<PostComment>>();
            services.AddScoped<IRepository<PostLike>, EntityRepository<PostLike>>();
            services.AddScoped<IPostService, PostService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthenticatedCurrentUserInfoProvider, AuthenticatedCurrentUserInfoProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InstagramClone v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureCustomExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
