using System.ComponentModel.DataAnnotations;

namespace UsersApi.DTOs;

/// <summary>
/// DTO pour la mise à jour d'un utilisateur existant
/// Les champs sont optionnels pour permettre des mises à jour partielles
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// Nouveau nom de l'utilisateur (optionnel)
    /// </summary>
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
    public string? Name { get; set; }

    /// <summary>
    /// Nouvel email de l'utilisateur (optionnel)
    /// </summary>
    [EmailAddress(ErrorMessage = "Le format de l'email est invalide")]
    [StringLength(255, ErrorMessage = "L'email ne peut pas dépasser 255 caractères")]
    public string? Email { get; set; }

    /// <summary>
    /// Vérifie si au moins un champ est fourni pour la mise à jour
    /// </summary>
    public bool HasUpdates => !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Email);
}
