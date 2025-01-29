using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationCenter.Infrastructure.Data;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly NotificationDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(NotificationDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Username == request.Username);

        if (client == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, client.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        // Generate a JWT token
        var token = _jwtService.GenerateToken(client.Id.ToString(), client.Username);

        return Ok(new
        {
            token,
            clientId = client.Id
        });
    }

   
    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
