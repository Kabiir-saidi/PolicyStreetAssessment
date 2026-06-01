using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyStreetAssessment.Data;
using PolicyStreetAssessment.DTOs;
using PolicyStreetAssessment.Models;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/employees/next-code
    // TODO: Should have running number table not hardcode
    [HttpGet("next-code")]
    public async Task<ActionResult<string>> GetNextCode()
    {
        var lastCode = await _context.Employees
                    // .OrderByDescending(e => e.EmployeeCode) //sorts incorrectly due to string and not int 
            .OrderByDescending(e => e.EmployeeId) //sorting by id is not ideal
            .Select(e => e.EmployeeCode)
            .FirstOrDefaultAsync();

        int next = 1;
        if (lastCode != null && lastCode.StartsWith("EMP-") &&
            int.TryParse(lastCode[4..], out int parsed))
        {
            next = parsed + 1;
        }

        return Ok($"EMP-{next:D4}");
    }

    // GET: api/employees
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<EmployeeListItemDto>>> GetEmployees(
        string? search,
        string? status,
        string? sortField,
        string? sortDir,
        int page = 1,
        int pageSize = 10)
    {
        var query = _context.Employees
            .Where(e => e.IsActive)
            .Join(_context.Departments, e => e.DepartmentId, d => d.DepartmentId,
                (e, d) => new { Employee = e, DepartmentName = d.DepartmentName })
            .Join(_context.Positions, ed => ed.Employee.PositionId, p => p.PositionId,
                (ed, p) => new { ed.Employee, ed.DepartmentName, PositionTitle = p.PositionTitle });

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lower = search.ToLower();
            query = query.Where(x =>
                x.Employee.EmployeeCode.ToLower().Contains(lower) ||
                x.Employee.FirstName.ToLower().Contains(lower) ||
                x.Employee.LastName.ToLower().Contains(lower) ||
                x.Employee.Email.ToLower().Contains(lower));
        }

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(x => x.Employee.EmploymentStatus == status);

        query = (sortField?.ToLower(), sortDir?.ToLower()) switch
        {
            ("employeecode", "desc") => query.OrderByDescending(x => x.Employee.EmployeeCode),
            ("employeecode", _)      => query.OrderBy(x => x.Employee.EmployeeCode),
            ("firstname", "desc")    => query.OrderByDescending(x => x.Employee.FirstName),
            ("firstname", _)         => query.OrderBy(x => x.Employee.FirstName),
            ("lastname", "desc")     => query.OrderByDescending(x => x.Employee.LastName),
            ("lastname", _)          => query.OrderBy(x => x.Employee.LastName),
            ("email", "desc")        => query.OrderByDescending(x => x.Employee.Email),
            ("email", _)             => query.OrderBy(x => x.Employee.Email),
            ("hiredate", "desc")     => query.OrderByDescending(x => x.Employee.HireDate),
            ("hiredate", _)          => query.OrderBy(x => x.Employee.HireDate),
            ("employmentstatus", "desc") => query.OrderByDescending(x => x.Employee.EmploymentStatus),
            ("employmentstatus", _)      => query.OrderBy(x => x.Employee.EmploymentStatus),
            _ => query.OrderBy(x => x.Employee.EmployeeCode)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new EmployeeListItemDto
            {
                EmployeeId = x.Employee.EmployeeId,
                EmployeeCode = x.Employee.EmployeeCode,
                FullName = x.Employee.FirstName + " " + x.Employee.LastName,
                Email = x.Employee.Email,
                DepartmentName = x.DepartmentName,
                PositionTitle = x.PositionTitle,
                EmploymentStatus = x.Employee.EmploymentStatus,
                HireDate = x.Employee.HireDate
            })
            .ToListAsync();

        return Ok(new PagedResultDto<EmployeeListItemDto> { Items = items, TotalCount = totalCount });
    }

    // GET: api/employees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDetailDto>> GetEmployee(int id)
    {
        var result = await (
            from e in _context.Employees
            join d in _context.Departments on e.DepartmentId equals d.DepartmentId
            join p in _context.Positions on e.PositionId equals p.PositionId
            where e.EmployeeId == id && e.IsActive
            select new EmployeeDetailDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeCode = e.EmployeeCode,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                DateOfBirth = e.DateOfBirth,
                Gender = e.Gender,
                DepartmentId = e.DepartmentId,
                DepartmentName = d.DepartmentName,
                PositionId = e.PositionId,
                PositionTitle = p.PositionTitle,
                HireDate = e.HireDate,
                EmploymentStatus = e.EmploymentStatus,
                Salary = e.Salary,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            }
        ).FirstOrDefaultAsync();

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // POST: api/employees
    [HttpPost]
    public async Task<ActionResult<int>> PostEmployee(SaveEmployeeDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!string.IsNullOrWhiteSpace(dto.Phone) && !IsValidPhone(dto.Phone))
            return BadRequest(new { message = "Phone must start with + followed by at least 2 digits (e.g. +60 12 345 6789)." });

        if (await _context.Employees.AnyAsync(e => e.Email == dto.Email && e.IsActive))
            return Conflict(new { message = $"Email '{dto.Email}' is already in use." });

        if (!string.IsNullOrWhiteSpace(dto.Phone) &&
            await _context.Employees.AnyAsync(e => e.Phone == dto.Phone && e.IsActive))
            return Conflict(new { message = $"Phone number '{dto.Phone}' is already in use." });

        // TODO: Should have running number table not hardcode
        var lastCode = await _context.Employees
            // .OrderByDescending(e => e.EmployeeCode) //sorts incorrectly due to string and not int 
            .OrderByDescending(e => e.EmployeeId) //sorting by id is not ideal
            .Select(e => e.EmployeeCode)
            .FirstOrDefaultAsync();

        int next = 1;
        if (lastCode != null && lastCode.StartsWith("EMP-") &&
            int.TryParse(lastCode[4..], out int parsed))
        {
            next = parsed + 1;
        }

        var employee = new Employee
        {
            EmployeeCode = $"EMP-{next:D4}",
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            DepartmentId = dto.DepartmentId,
            PositionId = dto.PositionId,
            HireDate = dto.HireDate,
            EmploymentStatus = dto.EmploymentStatus,
            Salary = dto.Salary,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee.EmployeeId);
    }

    // PUT: api/employees/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, SaveEmployeeDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Phone) && !IsValidPhone(dto.Phone))
            return BadRequest(new { message = "Phone must start with + followed by at least 2 digits (e.g. +60102001051)." });

        if (await _context.Employees.AnyAsync(e => e.Email == dto.Email && e.EmployeeId != id && e.IsActive))
            return Conflict(new { message = $"Email '{dto.Email}' is already in use." });

        if (!string.IsNullOrWhiteSpace(dto.Phone) &&
            await _context.Employees.AnyAsync(e => e.Phone == dto.Phone && e.EmployeeId != id && e.IsActive))
            return Conflict(new { message = $"Phone number '{dto.Phone}' is already in use." });

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.DateOfBirth = dto.DateOfBirth;
        employee.Gender = dto.Gender;
        employee.DepartmentId = dto.DepartmentId;
        employee.PositionId = dto.PositionId;
        employee.HireDate = dto.HireDate;
        employee.EmploymentStatus = dto.EmploymentStatus;
        employee.Salary = dto.Salary;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/employees/5 (soft delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
            return NotFound();

        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static bool IsValidPhone(string phone)
    {
        if (!phone.StartsWith("+")) return false;
        var stripped = phone.Substring(1).Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        return stripped.Length >= 2 && stripped.All(char.IsDigit);
    }
}
