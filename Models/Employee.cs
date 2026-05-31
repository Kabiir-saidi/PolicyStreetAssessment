using System.ComponentModel.DataAnnotations;

namespace PolicyStreetAssessment.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required]
    [StringLength(20)]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Phone { get; set; }

    public DateOnly DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    // FK of Employee (Employee -> Employee)
    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public DateOnly HireDate { get; set; }

    [Required]
    [StringLength(20)]
    public string EmploymentStatus { get; set; } = "Active";

    //monthly salary RM
    [Range(0, 99999.99)]
    public decimal Salary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
}
