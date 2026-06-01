using System.ComponentModel.DataAnnotations;

namespace PolicyStreetAssessment.DTOs;

public class SaveEmployeeDto
{
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

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int PositionId { get; set; }

    [Required]
    public DateOnly HireDate { get; set; }

    [Required]
    [StringLength(20)]
    public string EmploymentStatus { get; set; } = "Active";

    [Range(0, 99999.99)]
    public decimal Salary { get; set; }
}
