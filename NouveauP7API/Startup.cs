using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Jose;
using NouveauP7API.Data;
using NouveauP7API.Repositories;
using Microsoft.AspNetCore.Builder;

namespace NouveauP7API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration de la base de données
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(connectionString));

            // ... (autres configurations)

            // Configuration de l'authentification JWT
            var jwtSettings = new JwtSettings(); // Définir la classe JwtSettings
            Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);
            services.AddScoped<IJwtFactory, JwtFactory>();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    // ... (autres paramètres)
                };
            });

            // ... (autres configurations)
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UsePathBase("/swagger"); // Configurez la base du chemin ici

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
}