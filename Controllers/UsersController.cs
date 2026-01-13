using Microsoft.AspNetCore.Mvc;
using UsersApi.DTOs;
using UsersApi.Models;
using UsersApi.Services;

namespace UsersApi.Controllers;

/// <summary>
/// Controller pour la gestion des utilisateurs
/// Définit les endpoints REST pour l'API users
/// </summary>
[ApiController]
[Route("api/[controller]")] // Route: /api/users
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Constructeur avec injection du service utilisateur
    /// </summary>
    public UsersController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    /// <summary>
    /// GET /api/users - Récupère tous les utilisateurs
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = users.Select(UserDto.FromUser);
            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            // Log l'erreur en production
            return StatusCode(500, new { error = "Erreur interne du serveur" });
        }
    }

    /// <summary>
    /// GET /api/users/{id} - Récupère un utilisateur par son ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "Utilisateur non trouvé" });
            }

            var userDto = UserDto.FromUser(user);
            return Ok(userDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erreur interne du serveur" });
        }
    }

    /// <summary>
    /// POST /api/users - Crée un nouvel utilisateur
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { error = "Les données de l'utilisateur sont requises" });
            }

            var user = await _userService.CreateUserAsync(request.Name, request.Email);
            var userDto = UserDto.FromUser(user);

            // Retourne 201 Created avec l'URL de la ressource créée
            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                userDto
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message }); // 409 Conflict pour email déjà existant
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erreur interne du serveur" });
        }
    }

    /// <summary>
    /// PUT /api/users/{id} - Met à jour un utilisateur existant
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            if (request == null || !request.HasUpdates)
            {
                return BadRequest(new { error = "Au moins un champ doit être fourni pour la mise à jour" });
            }

            var user = await _userService.UpdateUserAsync(id, request.Name ?? string.Empty, request.Email ?? string.Empty);
            var userDto = UserDto.FromUser(user);

            return Ok(userDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("non trouvé"))
            {
                return NotFound(new { error = ex.Message });
            }
            return Conflict(new { error = ex.Message }); // Email déjà pris
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erreur interne du serveur" });
        }
    }

    /// <summary>
    /// DELETE /api/users/{id} - Supprime un utilisateur
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound(new { error = "Utilisateur non trouvé" });
            }

            return NoContent(); // 204 No Content pour les suppressions réussies
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erreur interne du serveur" });
        }
    }
}

