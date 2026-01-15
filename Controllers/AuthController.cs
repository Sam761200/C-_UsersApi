using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UsersApi.DTOs;
using UsersApi.Models;
using UsersApi.Services;

namespace UsersApi.Controllers;

/// <summary>
/// Contrôleur pour l'authentification et la gestion des comptes utilisateur
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Connexion d'un utilisateur
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), 200)]
    [ProducesResponseType(typeof(AuthResponseDto), 401)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (loginDto == null)
            {
                return BadRequest(AuthResponseDto.ErrorResponse("Données de connexion requises"));
            }

            var user = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
            if (user == null)
            {
                return Unauthorized(AuthResponseDto.ErrorResponse("Email ou mot de passe incorrect"));
            }

            var token = GenerateJwtToken(user);
            var userDto = UserDto.FromUser(user);

            return Ok(AuthResponseDto.SuccessResponse(token, userDto));
        }
        catch (Exception ex)
        {
            return StatusCode(500, AuthResponseDto.ErrorResponse("Erreur lors de la connexion"));
        }
    }

    /// <summary>
    /// Inscription d'un nouvel utilisateur
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), 201)]
    [ProducesResponseType(typeof(AuthResponseDto), 400)]
    [ProducesResponseType(typeof(AuthResponseDto), 409)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (registerDto == null)
            {
                return BadRequest(AuthResponseDto.ErrorResponse("Données d'inscription requises"));
            }

            var user = await _userService.RegisterUserAsync(registerDto);
            var token = GenerateJwtToken(user);
            var userDto = UserDto.FromUser(user);

            return CreatedAtAction(
                nameof(Login),
                AuthResponseDto.SuccessResponse(token, userDto)
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(AuthResponseDto.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(AuthResponseDto.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, AuthResponseDto.ErrorResponse("Erreur lors de l'inscription"));
        }
    }

    /// <summary>
    /// Génère un token JWT pour l'utilisateur
    /// </summary>
    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "super-secret-jwt-key-for-development-only";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "UsersApi";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "UsersApiClient";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim("role", user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(24), // Token valide 24h
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
