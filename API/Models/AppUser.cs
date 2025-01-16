using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class AppUser
{
    [Key]
    public int UserId  { get; set; }
    public required string  UserName { get; set; }
    public required byte[]  PasswordHash  { get; set; }
    public required byte[]  PasswordSalt { get; set; }

    // TODO: additional attributes 
}
