using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NouveauP7API.Data;
using NouveauP7API.Domain;
using NouveauP7API.Repositories;
using NouveauP7API.Repositories.JwtFactory;
using System.Text;


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
        // Configuration de la base de données
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("La chaîne de connexion à la base de données est nulle.");
        }

        services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(connectionString));

        // Configuration pour Identity
        services.AddIdentity<User, IdentityRole>(options =>
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

        // Configuration de l'authentification JWT
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JwtSettings:Issuer"],
                ValidAudience = Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"]))
            };
        });

        // Configuration du service de génération de token
        services.AddScoped<IJwtFactory, JwtFactory>();

        // Configuration des services de repository 
        services.AddScoped<IBidListRepository, BidListRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITradeRepository, TradeRepository>();
        services.AddScoped<IRuleNameRepository, RuleNameRepository>();
        services.AddScoped<ICurvePointRepository, CurvePointsRepository>();

        // Configuration des politiques d'autorisation
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            options.AddPolicy("RHPolicy", policy => policy.RequireRole("RH"));
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
        });

        // Configuration des contrôleurs
        services.AddControllers();

        // Configuration du swagger/open API
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Vous devez saisir un jeton valide",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions c)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NouveauP7API V1");
                c.RoutePrefix = "swagger"; // Utilisez un chemin de base approprié ici
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            // Ajoutez d'autres configurations de routage si nécessaire
        });
    }
}
