using FluentAssertions;
using Liaro.Application.Account.Exceptions;
using Liaro.Application.Account.Queries.LoginUser;
using Liaro.Application.ExternalServices.Jwt;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Liaro.UnitTest.UseCase.Account;


public class LoginUserQueryHandlerTest
{
    async Task Act(LoginUserQueryRequest request)
        => await _requestHandler.Handle(request, CancellationToken.None);

    [Fact]
    public async Task HandelAsync_Throw_UserNotFoundException_When_There_Is_No_User_Found_With_This_Information()
    {
        // ARRANGE
        var request = new LoginUserQueryRequest("TestEmail@test.com", "@@Test11@@");
        _uow.Users.FindUserAsync(request.UserName, request.Password).Returns(default(User));

        // ACT
        var act = () => Act(request);

        // ASSERT
        act.Should().Throws<UserNotExistByUserNameAndPasswordException>();
    }
    #region ARRANGE

    private readonly IUnitOfWork _uow;
    private readonly Mock<ITokenFactoryService> _tokenFactory;

    private readonly IRequestHandler<LoginUserQueryRequest, JwtTokensResponse> _requestHandler;
    public LoginUserQueryHandlerTest()
    {
        _uow =  Substitute.For<IUnitOfWork>();
        _tokenFactory = new Mock<ITokenFactoryService>();

        _requestHandler = new LoginUserQueryHandler(_uow, _tokenFactory.Object);
    }

    #endregion
}