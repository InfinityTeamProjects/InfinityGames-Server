using MediatR;
using System.ComponentModel.DataAnnotations;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Commands;

public class CreateUserCommand : IRequest<Response>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Birthday { get; set; }
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты!")]
    public string Email { get; set; }
    public string Password { get; set; }
}
