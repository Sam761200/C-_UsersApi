using System.ComponentModel.DataAnnotations;

namespace UsersApi.DTOs;

/// <summary>
/// DTO pour la connexion d'un utilisateur
/// </summary>
public class LoginDto
{
    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Le format de l'email est invalide")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    public string Password { get; set; } = string.Empty;
}

