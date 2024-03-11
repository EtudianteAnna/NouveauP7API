using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NouveauP7API.Data;
using NouveauP7API.Domain;
using NouveauP7API.Repositories;
using System.Text;

namespace NouveauP7API
{
    public class Startup
    {
        private  readonly IConfiguration _configuration;
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)

        {

            // Configuration de la base de données
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (connectionString != null)
            {
                services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(connectionString));
            }
            else
            {
                                
                throw new InvalidOperationException("La chaîne de connexion à la base de données est nulle.");
            }
            // Configuration pour Identity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            });


            // Configuration de l'authentication
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

            // Ajout du service de génération de jeton
            /* services.AddScoped<IJwtFactory, JwtFactory>();*/
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddLogging();

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

            // Configuration des services de repository 
            services.AddScoped<IBidListRepository, BidListRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITradeRepository, TradeRepository>();
            services.AddScoped<IRuleNameRepository, RuleNameRepository>();
            services.AddScoped<ICurvePointRepository, CurvePointsRepository>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddLogging();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
                    c.RoutePrefix = "";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Ajoutez vos configurations de routage supplémentaires ici si nécessaire
            });
        }
    }
}








