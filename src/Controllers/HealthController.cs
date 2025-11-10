using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;

namespace NotesAPI.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HealthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("helloworld")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            await _context.Database.CanConnectAsync();
            var userCount = await _context.Users.CountAsync();

            return Ok(new
            {
                Status = "Connected",
                UserCount = userCount,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = "Failed",
                Error = ex.Message
            });
        }
    }
}