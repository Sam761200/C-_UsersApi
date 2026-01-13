using System.Text.RegularExpressions;
using UsersApi.Models;
using UsersApi.Repositories;

namespace UsersApi.Services;

/// <summary>
/// Service métier pour la gestion des utilisateurs
/// Implémente les règles de validation et logique métier
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Constructeur avec injection du repository
    /// </summary>
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Récupère tous les utilisateurs
    /// </summary>
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    /// <summary>
    /// Récupère un utilisateur par son ID
    /// </summary>
    public async Task<User?> GetUserByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID doit être positif", nameof(id));

        return await _userRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Crée un nouvel utilisateur avec validation
    /// </summary>
    public async Task<User> CreateUserAsync(string name, string email)
    {
        // Validation des données d'entrée
        ValidateUserData(name, email);

        // Vérifier que l'email n'existe pas déjà
        if (await _userRepository.EmailExistsAsync(email))
        {
            throw new InvalidOperationException("Un utilisateur avec cet email existe déjà");
        }

        // Créer l'utilisateur
        var user = new User
        {
            Name = name.Trim(),
            Email = email.ToLower().Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    /// <summary>
    /// Met à jour un utilisateur existant avec validation
    /// </summary>
    public async Task<User> UpdateUserAsync(int id, string? name, string? email)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID doit être positif", nameof(id));

        // Vérifier qu'au moins un champ est fourni
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Au moins un champ (nom ou email) doit être fourni");

        // Validation des données fournies
        if (!string.IsNullOrWhiteSpace(name))
            ValidateName(name);
        if (!string.IsNullOrWhiteSpace(email))
            ValidateEmail(email);

        // Récupérer l'utilisateur existant
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            throw new InvalidOperationException("Utilisateur non trouvé");
        }

        // Préparer les nouvelles valeurs (garde les anciennes si non fournies)
        var newName = string.IsNullOrWhiteSpace(name) ? existingUser.Name : name.Trim();
        var newEmail = string.IsNullOrWhiteSpace(email) ? existingUser.Email : email.ToLower().Trim();

        // Vérifier que l'email n'est pas déjà pris par un autre utilisateur (seulement si email changé)
        if (!string.IsNullOrWhiteSpace(email) && await _userRepository.EmailExistsAsync(newEmail, id))
        {
            throw new InvalidOperationException("Un autre utilisateur utilise déjà cet email");
        }

        // Mettre à jour les données
        existingUser.Name = newName;
        existingUser.Email = newEmail;

        await _userRepository.UpdateAsync(existingUser);
        await _userRepository.SaveChangesAsync();

        return existingUser;
    }

    /// <summary>
    /// Supprime un utilisateur
    /// </summary>
    public async Task<bool> DeleteUserAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("L'ID doit être positif", nameof(id));

        var deleted = await _userRepository.DeleteAsync(id);
        if (deleted)
        {
            await _userRepository.SaveChangesAsync();
        }

        return deleted;
    }

    /// <summary>
    /// Valide les données d'un utilisateur (nom et email requis)
    /// </summary>
    private static void ValidateUserData(string name, string email)
    {
        ValidateName(name);
        ValidateEmail(email);
    }

    /// <summary>
    /// Valide uniquement le nom
    /// </summary>
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Le nom est requis", nameof(name));

        if (name.Trim().Length < 2)
            throw new ArgumentException("Le nom doit contenir au moins 2 caractères", nameof(name));

        if (name.Trim().Length > 100)
            throw new ArgumentException("Le nom ne peut pas dépasser 100 caractères", nameof(name));
    }

    /// <summary>
    /// Valide uniquement l'email
    /// </summary>
    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("L'email est requis", nameof(email));

        // Validation basique du format email
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        if (!emailRegex.IsMatch(email.Trim()))
            throw new ArgumentException("Le format de l'email est invalide", nameof(email));
    }
}
