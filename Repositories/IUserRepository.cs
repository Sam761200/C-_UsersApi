using UsersApi.Models;

namespace UsersApi.Repositories;

/// <summary>
/// Interface définissant les opérations d'accès aux données pour les utilisateurs
/// Suit le pattern Repository pour séparer la logique métier de l'accès aux données
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Récupère tous les utilisateurs
    /// </summary>
    Task<IEnumerable<User>> GetAllAsync();

    /// <summary>
    /// Récupère un utilisateur par son ID
    /// </summary>
    /// <param name="id">ID de l'utilisateur</param>
    /// <returns>L'utilisateur ou null s'il n'existe pas</returns>
    Task<User?> GetByIdAsync(int id);

    /// <summary>
    /// Récupère un utilisateur par son email
    /// </summary>
    /// <param name="email">Email de l'utilisateur</param>
    /// <returns>L'utilisateur ou null s'il n'existe pas</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Ajoute un nouvel utilisateur
    /// </summary>
    /// <param name="user">Utilisateur à ajouter</param>
    Task AddAsync(User user);

    /// <summary>
    /// Met à jour un utilisateur existant
    /// </summary>
    /// <param name="user">Utilisateur à mettre à jour</param>
    Task UpdateAsync(User user);

    /// <summary>
    /// Supprime un utilisateur par son ID
    /// </summary>
    /// <param name="id">ID de l'utilisateur à supprimer</param>
    /// <returns>True si supprimé, False sinon</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Vérifie si un email existe déjà
    /// </summary>
    /// <param name="email">Email à vérifier</param>
    /// <param name="excludeUserId">ID d'utilisateur à exclure (pour les updates)</param>
    Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);

    /// <summary>
    /// Sauvegarde tous les changements en base
    /// </summary>
    Task SaveChangesAsync();
}
