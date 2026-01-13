using Microsoft.EntityFrameworkCore;
using UsersApi.Models;

namespace UsersApi.Data;

/// <summary>
/// Contexte de base de données pour l'API Users
/// Gère la connexion et les entités de la base de données
/// </summary>
public class UsersDbContext : DbContext
{
    /// <summary>
    /// Constructeur qui passe les options au DbContext de base
    /// </summary>
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Table Users - définit l'entité User comme table dans la DB
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Configuration supplémentaire du modèle (optionnel)
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration spécifique pour l'entité User
        modelBuilder.Entity<User>(entity =>
        {
            // L'Id est déjà détecté automatiquement comme clé primaire
            // grâce aux conventions EF Core (propriété nommée "Id")

            // L'email doit être unique
            entity.HasIndex(u => u.Email).IsUnique();

            // Configuration des colonnes si nécessaire
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
        });
    }
}
