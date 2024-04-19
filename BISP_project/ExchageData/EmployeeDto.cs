namespace BIS_project.Dtos;

public class EmployeeDto
{
    public int RoleId { get; set; }
    public int PositionId { get; set; }
    public int BranchId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string UserName { get; set; } = string.Empty;

}