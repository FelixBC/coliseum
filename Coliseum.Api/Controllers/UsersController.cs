using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Coliseum.Api.Data;
using Coliseum.Api.DTOs;
using Coliseum.Api.Models;

namespace Coliseum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ColiseumContext _context;
    private readonly IMapper _mapper;

    public UsersController(ColiseumContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _context.Users
            .ToListAsync();

        return Ok(_mapper.Map<List<UserDto>>(users));
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<UserDto>(user));
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
    {
        var user = _mapper.Map<UserEntity>(userDto);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, _mapper.Map<UserDto>(user));
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UserDto userDto)
    {
        if (id != userDto.Id)
        {
            return BadRequest();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        _mapper.Map(userDto, user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Users/5/Posts
    [HttpGet("{id}/Posts")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetUserPosts(Guid id)
    {
        var posts = await _context.Posts
            .Include(p => p.Host)
            .Include(p => p.Media)
            .Where(p => p.HostId == id)
            .ToListAsync();

        return Ok(_mapper.Map<List<PostDto>>(posts));
    }
}
