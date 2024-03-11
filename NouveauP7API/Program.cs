using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NouveauP7API.Data;

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

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            if (connectionString != null)
            {
                builder.Services.AddDbContext<LocalDbContext>(options => options.UseSqlServer(connectionString));
            }
            else
            {

                throw new InvalidOperationException("La chaîne de connexion à la base de données est nulle.");
            }

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