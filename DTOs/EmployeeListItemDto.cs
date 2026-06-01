namespace PolicyStreetAssessment.DTOs;

public class EmployeeListItemDto
{
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string PositionTitle { get; set; } = string.Empty;
    public string EmploymentStatus { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }
}
