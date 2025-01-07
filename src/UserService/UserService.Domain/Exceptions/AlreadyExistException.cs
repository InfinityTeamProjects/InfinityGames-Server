namespace UserService.Domain.Exceptions;

public class AlreadyExistException(string message) : Exception(message)
{ 
    public int StatusCode = 409;
}