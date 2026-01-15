using UsersApi.Models;

namespace UsersApi.DTOs;

/// <summary>
/// DTO pour représenter un utilisateur dans les réponses API
/// Contrôle exactement les données exposées au client
/// </summary>
public class UserDto
{
    /// <summary>
    /// Identifiant unique de l'utilisateur
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom de l'utilisateur
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email de l'utilisateur
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Rôle de l'utilisateur
    /// </summary>
    public string Role { get; set; } = "User";

    /// <summary>
    /// Date de création de l'utilisateur
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Dernière connexion de l'utilisateur
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Indique si le compte est actif
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Crée un UserDto à partir d'une entité User
    /// </summary>
    public static UserDto FromUser(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt,
            IsActive = user.IsActive
        };
    }
}
