using Microsoft.AspNetCore.Mvc;
using NotesAPI.DTOs.Request;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUserAsync(LoginUserDto dto)
    {
        var user = await _authService.LoginUserAsync(dto);
        return Ok(new { success = true, data = user });
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUserAsync(RegisterUserDto dto)
    {
        _logger.LogInformation(
            "User | Name: {Name} | Email: {Email} | Time: {Time} UTC",
            dto.Name,
            dto.Email,
            DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        );
        var user = await _authService.RegisterUserAsync(dto);
        return Ok(new { success = true, data = user });
    }

    [HttpGet("profile")]
    public async Task<ActionResult> GetProfileAsync()
    {
        var userId = HttpContext.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            return BadRequest(new { success = false, message = "Missing X-User-Id header" });

        var user = await _authService.GetProfileAsync(Guid.Parse(userId));

        if (user == null)
            return NotFound(new { success = false, message = "User not found" });

        return Ok(new { success = true, data = user });
    }
}