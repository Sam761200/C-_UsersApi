using Microsoft.EntityFrameworkCore;
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
app.UseAuthorization(); // 🔥 Active l'autorisation (pas utilisé ici, mais bonne pratique)

app.MapControllers(); // 🔥 Mappe les routes des Controllers

app.Run();
