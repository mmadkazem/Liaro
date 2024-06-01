

namespace Liaro.UnitTest.UseCase.Account;


public class RegisterUserCommandHandlerTest
{
    async Task Act(RegisterUserCommandRequest request)
        => await _requestHandler.Handle(request, CancellationToken.None);

    [Fact]
    public async void HandleAsync_Calls_UnitOfWork_UserRepository_Add_On_Success()
    {
        // ARRANGE
        var request = new RegisterUserCommandRequest("TestUserName", "TestPassword", "Test@Test.com", "09111111111", "تست");
        _uow.Setup(u => u.Users.Add(It.IsAny<User>()));

        // ACT
        await Act(request);

        // ASSERT
        _uow.Verify(u => u.Users.Add(It.IsAny<User>()), Times.Once());
    }

    [Fact]
    public async void HandleAsync_Calls_UnitOfWork_SaveChange_On_Success()
    {
        // ARRANGE
        var request = new RegisterUserCommandRequest("TestUserName", "TestPassword", "Test@Test.com", "09111111111", "تست");
        _uow.Setup(u => u.Users.Add(It.IsAny<User>()));

        // ACT
        await Act(request);

        // ASSERT
        _uow.Verify(u => u.SaveChangeAsync(), Times.Once());
    }


    #region ARRANGE

    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<ISecurityService> _securityService;

    private readonly IRequestHandler<RegisterUserCommandRequest> _requestHandler;
    public RegisterUserCommandHandlerTest()
    {
        _uow = new Mock<IUnitOfWork>();
        _securityService = new Mock<ISecurityService>();

        _requestHandler = new RegisterUserCommandHandler(_uow.Object, _securityService.Object);
    }

    #endregion
}