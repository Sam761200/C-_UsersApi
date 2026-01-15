using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersApi.Data;
using UsersApi.Repositories;
using UsersApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // 🔥 Active les Controllers (MVC pattern)
builder.Services.AddEndpointsApiExplorer(); // Permet la génération d'OpenAPI/Swagger
builder.Services.AddSwaggerGen(); // 🔥 Génère la documentation Swagger

// 🔥 Configuration d'Entity Framework Core avec SQLite
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UsersDatabase")));

// 🔥 Configuration de l'authentification JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "UsersApi",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "UsersApiClient",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "super-secret-jwt-key-for-development-only"))
        };
    });

// 🔥 Enregistrement du Repository dans la DI (Dependency Injection)
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 🔥 Enregistrement du Service dans la DI
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // 🔥 Active Swagger UI en développement
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔥 Active l'authentification et l'autorisation
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // 🔥 Mappe les routes des Controllers

app.Run();
