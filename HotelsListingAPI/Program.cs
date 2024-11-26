
using HotelsListingAPI.Core.Configurations;
using HotelsListingAPI.Core.Contracts;
using HotelsListingAPI.Controllers;
using HotelsListingAPI.Data;
using HotelsListingAPI.Core.Middleware;
using HotelsListingAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using HotelsListingAPI.Core.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace HotelsListingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
            builder.Services.AddDbContext<HotelListingDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddIdentityCore<ApiUser>()
                            .AddRoles<IdentityRole>()
                            .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingApi")
                            .AddEntityFrameworkStores<HotelListingDbContext>()
                            .AddDefaultTokenProviders();

            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Listing API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Autorization header using the Bearer scheme.
                                     Enter 'Bearer' [space] and then your token in the text input below.
                                     Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "0auth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            // this alow our to app to be used from not local only request.
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyHeader());
            });

            // add versioning to the app
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver")
                    );
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            // add loger to the builder, use logger to the console.
            builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console()
                                        .ReadFrom.Configuration(ctx.Configuration));
            builder.Services.AddAutoMapper(typeof(MapperConfig));


            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
            builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration
                    ["JwtSettings:Key"]))
                };
            });

            builder.Services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024;
                options.UseCaseSensitivePaths = true;
            });

            builder.Services.AddHealthChecks()
                .AddCheck<CustomeHealthCheck>("Custom health check", failureStatus: HealthStatus.Degraded, 
                tags: new[] {"custom"});

            builder.Services.AddControllers().AddOData(options =>
            {
                options.Select().Filter().OrderBy();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = healthcheck => healthcheck.Tags.Contains("custom"),
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                }
            });

            app.MapHealthChecks("/health");

            // add global handling ex
            app.UseMiddleware<ExceptionMiddleware>();

            // add http request to be log
            app.UseSerilogRequestLogging();


            app.UseHttpsRedirection();

            // add the service to the app
            app.UseCors("AllowAll");

            // add cashing to the app - must go after cors.
            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(10)
                };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };
                await next();
            });

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        

        class CustomeHealthCheck : IHealthCheck
        {
            public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
                CancellationToken cancellationToken = default)
            {
                var isHealthy = true;

                /*cstom check. logic etc etc. */

                if (isHealthy)
                {
                    return Task.FromResult(HealthCheckResult.Healthy("All system looking good."));
                }
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "System unhealthy."));
            }
        }
    }
}
