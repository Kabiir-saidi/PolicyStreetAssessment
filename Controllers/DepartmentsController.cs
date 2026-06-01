using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyStreetAssessment.Models;
using PolicyStreetAssessment.Data;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _context;
    public DepartmentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Department
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
    {
        return await _context.Departments.ToListAsync();
    }

    // GET: api/Department/5
    [HttpGet("{departmentid}")]
    public async Task<ActionResult<Department>> GetDepartment(int departmentid)
    {
        var department = await _context.Departments.FindAsync(departmentid);

        if (department == null)
        {
            return NotFound();
        }

        return department;
    }

    // PUT: api/Department/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{departmentid}")]
    public async Task<IActionResult> PutDepartment(int? departmentid, Department department)
    {
        if (departmentid != department.DepartmentId)
        {
            return BadRequest();
        }

        _context.Entry(department).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DepartmentExists(departmentid))
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

    // POST: api/Department
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Department>> PostDepartment(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDepartment", new { departmentid = department.DepartmentId }, department);
    }

    // DELETE: api/Department/5
    [HttpDelete("{departmentid}")]
    public async Task<IActionResult> DeleteDepartment(int? departmentid)
    {
        var department = await _context.Departments.FindAsync(departmentid);
        if (department == null)
        {
            return NotFound();
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DepartmentExists(int? departmentid)
    {
        return _context.Departments.Any(e => e.DepartmentId == departmentid);
    }
}
