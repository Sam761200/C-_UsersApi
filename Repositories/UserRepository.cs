using Microsoft.EntityFrameworkCore;
using UsersApi.Data;
using UsersApi.Models;

namespace UsersApi.Repositories;

/// <summary>
/// Implémentation du repository pour les utilisateurs utilisant Entity Framework Core
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;

    /// <summary>
    /// Constructeur avec injection de dépendance du DbContext
    /// </summary>
    public UserRepository(UsersDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Récupère tous les utilisateurs
    /// </summary>
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Récupère un utilisateur par son ID
    /// </summary>
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <summary>
    /// Récupère un utilisateur par son email
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    /// <summary>
    /// Ajoute un nouvel utilisateur
    /// </summary>
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    /// <summary>
    /// Met à jour un utilisateur existant
    /// </summary>
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
    }

    /// <summary>
    /// Supprime un utilisateur par son ID
    /// </summary>
    public Task<bool> DeleteAsync(int id)
    {
        var user = _context.Users.Find(id); // Utilise Find au lieu de GetByIdAsync pour éviter l'avertissement
        if (user == null)
            return Task.FromResult(false);

        _context.Users.Remove(user);
        return Task.FromResult(true);
    }

    /// <summary>
    /// Vérifie si un email existe déjà
    /// </summary>
    public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
    {
        var query = _context.Users.Where(u => u.Email.ToLower() == email.ToLower());

        if (excludeUserId.HasValue)
        {
            query = query.Where(u => u.Id != excludeUserId.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Sauvegarde tous les changements en base
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
