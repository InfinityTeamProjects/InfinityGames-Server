using MediatR;
using System.ComponentModel.DataAnnotations;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Auths.Commands;

public class LoginUserCommand : IRequest<Response>
{
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты!")]
    public string Email { get; set; }
    public string Password { get; set; }
}
