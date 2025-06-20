using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Coliseum.Api.Models.Auth;
using Coliseum.Api.Data;
using Coliseum.Api.DTOs;
using AutoMapper;

namespace Coliseum.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IMapper mapper,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Create user profile
        var userProfile = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Bio = request.Bio,
            AvatarUrl = "default-avatar.png"
        };

        using (var scope = HttpContext.RequestServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ColiseumContext>();
            context.Users.Add(userProfile);
            await context.SaveChangesAsync();
        }

        var token = await GenerateJwtToken(user);
        return Ok(new AuthResponse
        {
            Token = token,
            User = _mapper.Map<UserDto>(userProfile)
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid email or password");
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized("User not found");
        }

        var userProfile = await GetUserProfile(user.Id);
        if (userProfile == null)
        {
            return Unauthorized("User profile not found");
        }

        var token = await GenerateJwtToken(user);
        return Ok(new AuthResponse
        {
            Token = token,
            User = _mapper.Map<UserDto>(userProfile)
        });
    }

    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<UserEntity?> GetUserProfile(string userId)
    {
        using var scope = HttpContext.RequestServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ColiseumContext>();
        return await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
    }
}
