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
    /// Date de création de l'utilisateur
    /// </summary>
    public DateTime CreatedAt { get; set; }

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
            CreatedAt = user.CreatedAt
        };
    }
}
