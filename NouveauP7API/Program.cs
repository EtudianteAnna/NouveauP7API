using Jose;
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
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<LocalDbContext>();
                    // Initialisez la base de données si nécessaire
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Une erreur s'est produite lors de la migration de la base de données.");
                }
            }

            host.Run();
        }

        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
     .ConfigureWebHostDefaults(webBuilder =>
     {
         webBuilder.UseStartup<Startup>();
     })
     .ConfigureServices((hostContext, services) =>
     {
         // Add services to the container.
         services.AddControllers();
         services.AddEndpointsApiExplorer();
         services.AddSwaggerGen();
         services.AddScoped<IRatingRepository, RatingRepository>();
         services.AddScoped<IUserRepository, UserRepository>();
         services.AddScoped<ITradeRepository, TradeRepository>();
         services.AddScoped<IRuleNameRepository, RuleNameRepository>();
         services.AddScoped<ICurvePointRepository, CurvePointsRepository>();
         services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
         //services.AddScoped<IJwtFactory, JwtFactory>();
         services.AddLogging();

         var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
         if (string.IsNullOrEmpty(connectionString))
         {
             throw new InvalidOperationException("La chaîne de connexion à la base de données est nulle.");
         }

         services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(connectionString));

         // Configuration de JwtSettings
         var jwtSettings = new NouveauP7API.Domain.JwtSettings(); // Utilisez le chemin complet de NouveauP7API.Domain.JwtSettings
         hostContext.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
         services.AddSingleton(jwtSettings);

     });

    }

}
