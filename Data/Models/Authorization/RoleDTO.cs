namespace Firebase_Auth.Data.Models.Authorization;
public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class RoleCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}