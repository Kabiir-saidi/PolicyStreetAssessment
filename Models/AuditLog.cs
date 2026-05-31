using System.ComponentModel.DataAnnotations;

namespace PolicyStreetAssessment.Models;

public class AuditLog
{
    public int AuditLogId { get; set; }

    [Required]
    [StringLength(10)]
    public string HttpMethod { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Endpoint { get; set; } = string.Empty;

    public string? Parameters { get; set; }

    public int StatusCode { get; set; }

    public DateTime CalledAt { get; set; } = DateTime.UtcNow;

    [StringLength(50)]
    public string? IpAddress { get; set; }
}
