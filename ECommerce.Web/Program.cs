
using ECommerce.Domin.Contract;
using ECommerce.Domin.Contract.DataSeeding;
using ECommerce.Domin.Models.IdentityModule;
using ECommerce.Persistence.Data.DataSeed;
using ECommerce.Persistence.Data.DbContexts;
using ECommerce.Persistence.IdentityData.DbContexts;
using ECommerce.Persistence.Repository;
using ECommerce.Service;
using ECommerce.Service.MappingFolder;
using ECommerce.ServiceAbstractions;
using ECommerce.Web.CustomMiddleWares;
using ECommerce.Web.Extensions;
using ECommerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddKeyedScoped<IDataini, DataIni>("Default");
            builder.Services.AddKeyedScoped<IDataini, IdentityDataSeed>("Identity");




            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());
            builder.Services.AddAutoMapper(X => X.AddProfile<BasketProfile>());

            builder.Services.AddSingleton<IConnectionMultiplexer>(o =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });

            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResonseFactory.GenerateApiValidationResponse;
            });

            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
                // Add-Migration "IdentityTableCreate" -OutDir "IdentityData/Migrations" -Context "StoreIdentityDbContext"
            });


            builder.Services.AddIdentityCore<ApplicationUser>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.Services.AddScoped<IAuthService , AuthService>();

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Auth
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // UnAuth
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:secretKey"]!))
                };
            });


            var app = builder.Build();


            #region Data Seed

            await app.MigrateDbAsync();
            await app.SeedDataAsync();
            await app.SeedIdentityDataAsync();

            #endregion

            // Exception Middleware
            app.UseMiddleware<ExceptionHandlerMiddleWare>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
