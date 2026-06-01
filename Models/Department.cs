using System.ComponentModel.DataAnnotations;

namespace PolicyStreetAssessment.Models;

public class Department
{
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(4, MinimumLength = 2)]
    public string DepartmentCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
