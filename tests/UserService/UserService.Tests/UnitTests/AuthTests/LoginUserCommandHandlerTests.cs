using Microsoft.AspNetCore.Identity;
using Moq;
using UserService.Application.Abstractions;
using UserService.Application.UseCases.Users.Auths.Commands;
using UserService.Domain.Entities.Auth;
using UserService.Domain.Exceptions;

namespace UserService.Tests.UnitTests.AuthTests;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new LoginUserCommandHandler(_tokenServiceMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserManagerFails()
    {
        // Arrange
        var request = new LoginUserCommand { Email = "user01@gmail.com", Password = "!User01!" };

        _userManagerMock.Setup(um => um.FindByEmailAsync(request.Email))
                        .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Database error", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var request = new LoginUserCommand { Email = "user01@gmail.com", Password = "!User01!" };

        _userManagerMock.Setup(um => um.FindByEmailAsync(request.Email))
                        .ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new User { Email = "user01@gmail.com" };
        var request = new LoginUserCommand { Email = user.Email, Password = "!User01!" };

        _userManagerMock.Setup(um => um.FindByEmailAsync(request.Email))
                        .ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, request.Password))
                        .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CustomException>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal(400, exception.StatusCode);
    }

    [Theory]
    [InlineData("user01@gmail.com", "!User01!", "userToken", false)]
    [InlineData("admin01@gmail.com", "!Admin01!", "adminToken", true)]
    public async Task Handle_ShouldReturnCorrectToken_WhenCredentialsAreValid(string email, string password, string expectedToken, bool isAdmin)
    {
        // Arrange
        var user = new User { Email = email };
        var request = new LoginUserCommand { Email = email, Password = password };

        _userManagerMock.Setup(um => um.FindByEmailAsync(request.Email))
                        .ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, request.Password))
                        .ReturnsAsync(true);
        _userManagerMock.Setup(um => um.IsInRoleAsync(user, "Admin"))
                        .ReturnsAsync(isAdmin);
        _tokenServiceMock.Setup(ts => ts.GenerateTokenToUser(user))
                         .Returns("userToken");
        _tokenServiceMock.Setup(ts => ts.GenerateTokenToAdmin(user))
                         .Returns("adminToken");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expectedToken, result.Token);
    }
}
