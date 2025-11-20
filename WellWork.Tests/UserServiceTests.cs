using Moq;
using WellWork.Application;
using WellWork.Domain;
using WellWork.Domain.Interfaces;

namespace WellWork.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _service = new UserService(_repoMock.Object);
    }

    [Fact]
    public async void CreateUser_ShouldReturnCreatedUser()
    {
        // Arrange
                var fakeUser = new User(Guid.NewGuid(), "Teste","123456");

                _repoMock
                    .Setup(r => r.AddAsync(It.IsAny<User>()))
                    .Returns(Task.CompletedTask);
        
                // Act
                var result = await _service.CreateUserAsync("edu", "123", "ROLE_USER");
        
                // Assert
                Assert.NotNull(result);
                Assert.Equal("edu", result.Username);
                _repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnUser_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new User(id, "UserTeste", "123456" );

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(user);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    
    [Fact]
    public async Task DeleteUser_ShouldCallRepository()
    {
        // Arrange
        var id = Guid.NewGuid();

        var fakeUser = new User(id, "edu", "123456");

        // O service vai chamar _repo.GetByIdAsync(id)
        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(fakeUser);

        // DeleteAsync recebe User, não Guid
        _repoMock
            .Setup(r => r.DeleteAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteUserAsync(id);

        // Assert
        _repoMock.Verify(
            r => r.DeleteAsync(It.Is<User>(u => u.Id == id)),
            Times.Once
        );
    }

}