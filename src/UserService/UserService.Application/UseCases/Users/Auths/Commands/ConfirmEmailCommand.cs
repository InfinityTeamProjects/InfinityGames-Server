using MediatR;
using System.ComponentModel.DataAnnotations;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Auths.Commands;

public class ConfirmEmailCommand : IRequest<Response>
{
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты!")]
    public string Email { get; set; }
    [Length(6, 6, ErrorMessage = "Код должен быть 6-значным!")]
    public string Code { get; set; }
}
