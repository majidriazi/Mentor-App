namespace API.Dtos;

public record LoginDto
{
    public required string  UserName  { get; set; }
    public required string  Password  { get; set; }
}
