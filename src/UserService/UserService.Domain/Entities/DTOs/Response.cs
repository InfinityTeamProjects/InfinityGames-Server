namespace UserService.Domain.Entities.DTOs;

public class Response
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
}
