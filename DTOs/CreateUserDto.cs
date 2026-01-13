using System.ComponentModel.DataAnnotations;

namespace UsersApi.DTOs;

/// <summary>
/// DTO pour la création d'un nouvel utilisateur
/// Inclut les validations côté client et serveur
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// Nom de l'utilisateur (requis, 2-100 caractères)
    /// </summary>
    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email de l'utilisateur (requis, format email valide)
    /// </summary>
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Le format de l'email est invalide")]
    [StringLength(255, ErrorMessage = "L'email ne peut pas dépasser 255 caractères")]
    public string Email { get; set; } = string.Empty;
}
