using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Coliseum.Api.Data;
using Coliseum.Api.DTOs;
using Coliseum.Api.Models;

namespace Coliseum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly ColiseumContext _context;
    private readonly IMapper _mapper;

    public PostsController(ColiseumContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        var posts = await _context.Posts
            .Include(p => p.Host)
            .Include(p => p.Media)
            .ToListAsync();

        return Ok(_mapper.Map<List<PostDto>>(posts));
    }

    // GET: api/Posts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(Guid id)
    {
        var post = await _context.Posts
            .Include(p => p.Host)
            .Include(p => p.Media)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PostDto>(post));
    }

    // POST: api/Posts
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost(PostDto postDto)
    {
        var post = _mapper.Map<PostEntity>(postDto);
        
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, _mapper.Map<PostDto>(post));
    }

    // PUT: api/Posts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(Guid id, PostDto postDto)
    {
        if (id != postDto.Id)
        {
            return BadRequest();
        }

        var post = await _context.Posts
            .Include(p => p.Host)
            .Include(p => p.Media)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        _mapper.Map(postDto, post);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Posts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Posts/5/Like
    [HttpPost("{id}/Like")]
    public async Task<IActionResult> LikePost(Guid id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        // TODO: Implement like functionality
        return NoContent();
    }

    // GET: api/Posts/Search?q=term
    [HttpGet("Search")]
    public async Task<ActionResult<IEnumerable<PostDto>>> SearchPosts(string q)
    {
        var posts = await _context.Posts
            .Include(p => p.Host)
            .Include(p => p.Media)
            .Where(p => p.Title.Contains(q) || 
                      p.Description.Contains(q) ||
                      (p.Tags != null && p.Tags.Contains(q)))
            .ToListAsync();

        return Ok(_mapper.Map<List<PostDto>>(posts));
    }
}
