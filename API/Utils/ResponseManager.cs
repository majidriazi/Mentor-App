namespace API.Utils;

public class ResponseManager
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
}
