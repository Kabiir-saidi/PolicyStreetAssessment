using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyStreetAssessment.Models;
using PolicyStreetAssessment.Data;

[Route("api/[controller]")]
[ApiController]
public class PositionsController : ControllerBase
{
    private readonly AppDbContext _context;
    public PositionsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Position
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Position>>> GetPosition()
    {
        return await _context.Positions.ToListAsync();
    }

    // GET: api/Position/5
    [HttpGet("{positionid}")]
    public async Task<ActionResult<Position>> GetPosition(int positionid)
    {
        var position = await _context.Positions.FindAsync(positionid);

        if (position == null)
        {
            return NotFound();
        }

        return position;
    }

    // PUT: api/Position/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{positionid}")]
    public async Task<IActionResult> PutPosition(int? positionid, Position position)
    {
        if (positionid != position.PositionId)
        {
            return BadRequest();
        }

        _context.Entry(position).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PositionExists(positionid))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Position
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Position>> PostPosition(Position position)
    {
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPosition", new { positionid = position.PositionId }, position);
    }

    // DELETE: api/Position/5
    [HttpDelete("{positionid}")]
    public async Task<IActionResult> DeletePosition(int? positionid)
    {
        var position = await _context.Positions.FindAsync(positionid);
        if (position == null)
        {
            return NotFound();
        }

        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PositionExists(int? positionid)
    {
        return _context.Positions.Any(e => e.PositionId == positionid);
    }
}
