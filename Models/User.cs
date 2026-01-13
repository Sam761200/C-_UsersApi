namespace UsersApi.Models;


/// Entité représentant un utilisateur dans la base de données

public class User
{

    /// Identifiant unique de l'utilisateur (clé primaire)

    public int Id { get; set; }


    /// Nom de l'utilisateur

    public string Name { get; set; } = string.Empty;


    /// Email de l'utilisateur (doit être unique)

    public string Email { get; set; } = string.Empty;


    /// Date de création de l'utilisateur

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
