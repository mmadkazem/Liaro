namespace Liaro.Infrastructure.ExternalServices.Jwt;

// public interface ITokenValidatorService
// {
//     Task ValidateAsync(TokenValidatedContext context);
// }

// public class TokenValidatorService(IUnitOfWork uow) : ITokenValidatorService
// {
//     private readonly IUnitOfWork _uow = uow;

//     public async Task ValidateAsync(TokenValidatedContext context)
//     {
//         var userPrincipal = context.Principal;

//         var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
//         if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
//         {
//             context.Fail("This is not our issued token. It has no claims.");
//             return;
//         }

//         var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
//         if (serialNumberClaim == null)
//         {
//             context.Fail("This is not our issued token. It has no serial.");
//             return;
//         }

//         var userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
//         if (!int.TryParse(userIdString, out int userId))
//         {
//             context.Fail("This is not our issued token. It has no user-id.");
//             return;
//         }

//         var user = await _uow.Users.FindAsync(userId);
//         if (user == null || user.SerialNumber != serialNumberClaim.Value || !user.IsActive)
//         {
//             // user has changed his/her password/roles/stat/IsActive
//             context.Fail("This token is expired. Please login again.");
//         }

//         var accessToken = context.SecurityToken as JsonWebToken;
//         if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.EncodedToken) ||
//             !await _uow.UserTokens.IsValidTokenAsync(accessToken.EncodedToken, userId))
//         {
//             context.Fail("This token is not in our database.");
//             return;
//         }

//         await UpdateUserLastActivityDateAsync(userId);
//     }

//     private async Task UpdateUserLastActivityDateAsync(int userId)
//     {
//         var user = await _uow.Users.FindAsync(userId);
//         if (user.LastLoggedIn != null)
//         {
//             var updateLastActivityDate = TimeSpan.FromMinutes(2);
//             var currentUtc = DateTimeOffset.UtcNow;
//             var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
//             if (timeElapsed < updateLastActivityDate)
//             {
//                 return;
//             }
//         }
//         user.LastLoggedIn = DateTimeOffset.UtcNow;
//         _uow.Users.Update(user);
//         await _uow.SaveChangeAsync();
//     }
// }
