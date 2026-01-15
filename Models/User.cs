using System.ComponentModel.DataAnnotations;

namespace UsersApi.Models;

/// Entité représentant un utilisateur dans la base de données
public class User
{
    /// Identifiant unique de l'utilisateur (clé primaire)
    public int Id { get; set; }

    /// Nom de l'utilisateur
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    /// Email de l'utilisateur (doit être unique)
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    /// Mot de passe hashé de l'utilisateur
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// Rôle de l'utilisateur (Admin, User, etc.)
    public string Role { get; set; } = "User";

    /// Date de création de l'utilisateur
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// Dernière connexion de l'utilisateur
    public DateTime? LastLoginAt { get; set; }

    /// Indique si le compte est actif
    public bool IsActive { get; set; } = true;
}
