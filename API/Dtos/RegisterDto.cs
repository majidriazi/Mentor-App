namespace API.Dtos;

public record RegisterDto
{
    public required string  UserName { get; set; }
    public required string  Password  { get; set; }
    public required string  ConfirmPassword  { get; set; } 
}
