using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NouveauP7API.Data;
using NouveauP7API.Domain;
using NouveauP7API.Repositories;

namespace NouveauP7API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITradeRepository, TradeRepository>();
            builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
            builder.Services.AddScoped<ICurvePointRepository, CurvePointsRepository>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IJwtFactory, JwtFactory>();
            builder.Services.AddLogging();
           


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (connectionString != null)
            {
                builder.Services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(connectionString));
            }
            else
            {

                throw new InvalidOperationException("La chaîne de connexion à la base de données est nulle.");
            }

            // Configuration pour Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<LocalDbContext>()
            .AddDefaultTokenProviders();


            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}