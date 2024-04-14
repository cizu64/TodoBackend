namespace TODOBACKEND.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TODOBACKEND.Repositories;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly UserRepository userRepository;
    private readonly IConfiguration _configuration;
    public UserController(UserRepository userRepository, IConfiguration configuration)
    {
        this.userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO dto)
    {
        byte[] hashedPwd = SHA512.HashData(Encoding.UTF8.GetBytes(dto.Password));

        await userRepository.AddUser(new Entities.User
        {
            Email = dto.Email,
            Firstname = dto.Firstname,
            LastName = dto.LastName,
            Password = Convert.ToBase64String(hashedPwd)
        });

        return Ok(new Response { detail = "Account created successfully" });
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        //await Task.Delay(TimeSpan.FromSeconds(20));
        byte[] pwd = SHA512.HashData(Encoding.UTF8.GetBytes(dto.Password));
        string password = Convert.ToBase64String(pwd);
        var user = await userRepository.GetUser(u => u.Email.ToLower() == dto.Email.ToLower());
        if (user is null) return BadRequest(new Response { detail = "User not found", statusCode = StatusCodes.Status400BadRequest });
        if (user.Password != password) return BadRequest(new Response { detail = "Invalid password", statusCode = StatusCodes.Status400BadRequest});

        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
        };
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]);
        var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["JWT:ISSUER"],
            Audience = _configuration["JWT:AUDIENCE"]
        });
        string jwt = tokenHandler.WriteToken(securityToken);
        return Ok(new Response { detail = jwt });
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> ValidateToken(string token)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]);
            var validationResult = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:ISSUER"],
                ValidAudience = _configuration["JWT:AUDIENCE"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken securityToken);
            return Ok(new Response{detail = securityToken != null});
        }
        catch (Exception)
        {
            return BadRequest(new Response{detail = false, statusCode = StatusCodes.Status400BadRequest});
        }
    }
}