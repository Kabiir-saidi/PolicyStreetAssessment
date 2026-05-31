using System.ComponentModel.DataAnnotations;

namespace PolicyStreetAssessment.Models;

public class Position
{
    public int PositionId { get; set; }

    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string PositionCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string PositionTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
