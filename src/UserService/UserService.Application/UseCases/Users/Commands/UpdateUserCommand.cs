using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UserService.Domain.Entities.DTOs;

namespace UserService.Application.UseCases.Users.Commands;

public class UpdateUserCommand : IRequest<Response>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты!")]
    public string Email { get; set; }
    public IFormFile ProfilePicture { get; set; }
}
