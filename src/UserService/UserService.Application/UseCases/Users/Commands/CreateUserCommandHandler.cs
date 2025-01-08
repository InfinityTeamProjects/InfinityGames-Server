using MediatR;
using UserService.Application.Abstractions;
using UserService.Domain.Entities.Auth;

namespace UserService.Application.UseCases.Users.Commands;

public class CreateUserCommandHandler(IUserDbContext dbContext) : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUserDbContext _dbContext = dbContext;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {



        return null;
    }
}