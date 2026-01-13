using UsersApi.Models;

namespace UsersApi.Services;

/// <summary>
/// Interface définissant les services métier pour la gestion des utilisateurs
/// Contient la logique métier et les règles de validation
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Récupère tous les utilisateurs
    /// </summary>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// Récupère un utilisateur par son ID
    /// </summary>
    /// <param name="id">ID de l'utilisateur</param>
    /// <returns>L'utilisateur ou null s'il n'existe pas</returns>
    Task<User?> GetUserByIdAsync(int id);

    /// <summary>
    /// Crée un nouvel utilisateur
    /// </summary>
    /// <param name="name">Nom de l'utilisateur</param>
    /// <param name="email">Email de l'utilisateur</param>
    /// <returns>L'utilisateur créé</returns>
    /// <exception cref="ArgumentException">Si les données sont invalides</exception>
    /// <exception cref="InvalidOperationException">Si l'email existe déjà</exception>
    Task<User> CreateUserAsync(string name, string email);

    /// <summary>
    /// Met à jour un utilisateur existant
    /// </summary>
    /// <param name="id">ID de l'utilisateur</param>
    /// <param name="name">Nouveau nom (optionnel)</param>
    /// <param name="email">Nouvel email (optionnel)</param>
    /// <returns>L'utilisateur mis à jour</returns>
    /// <exception cref="ArgumentException">Si les données sont invalides</exception>
    /// <exception cref="InvalidOperationException">Si l'utilisateur n'existe pas ou l'email est déjà pris</exception>
    Task<User> UpdateUserAsync(int id, string? name, string? email);

    /// <summary>
    /// Supprime un utilisateur
    /// </summary>
    /// <param name="id">ID de l'utilisateur à supprimer</param>
    /// <returns>True si supprimé, False s'il n'existait pas</returns>
    Task<bool> DeleteUserAsync(int id);
}
