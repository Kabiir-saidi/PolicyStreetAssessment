using System.ComponentModel.DataAnnotations;

namespace PolicyStreetAssessment.Models;

public class Department
{
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string DepartmentCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Position> Positions { get; set; } = new List<Position>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
