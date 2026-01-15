namespace UsersApi.DTOs;

/// <summary>
/// DTO pour la réponse d'authentification
/// </summary>
public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public UserDto? User { get; set; }

    public static AuthResponseDto SuccessResponse(string token, UserDto user)
    {
        return new AuthResponseDto
        {
            Success = true,
            Message = "Connexion réussie",
            Token = token,
            User = user
        };
    }

    public static AuthResponseDto ErrorResponse(string message)
    {
        return new AuthResponseDto
        {
            Success = false,
            Message = message
        };
    }
}

