using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NouveauP7API.Data;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<LocalDbContext>()
    .AddDefaultTokenProviders();

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JwtSettings:SecretKey"))
    };
});

// Configuration de l'authentification JWT
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

// Ajoutez l'injection de dépendances pour les repositories
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IBidListRepository, BidListRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
builder.Services.AddScoped<ICurvePointRepository, CurvePointsRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IJwtFactory, JwtFactory>();
builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
builder.Services.AddLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "auth",
        pattern: "api/auth/{action=Login}");
});

app.Run();