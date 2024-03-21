using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NouveauP7API.Data;
using NouveauP7API.Domain;
using NouveauP7API.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "your_issuer_here",
        ValidAudience = "your_audience_here",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_here"))
    };
});

// Configuration de l'authentification JWT
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddScoped<IJwtFactory, JwtFactory>();

// Add interfaces
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
builder.Services.AddScoped<ICurvePointRepository, CurvePointsRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UsePathBase("/"); // Configurez la base du chemin ici
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting(); // Add routing middleware

app.MapControllers();

app.Run();
