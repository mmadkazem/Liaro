using Liaro.Application.Common;
using Liaro.Application.ViewModels;

namespace Liaro.Application.ExternalServices.Contracts;


public interface IKavenegarService
{
    Task<SmsResultVM> SendLoginCode(string loginCode, string mobile, string fullName);
    Task<SmsStatusResultVM> CheckMessageStatus(List<string> messageIds);
    Task<SmsResultVM> SendLookup(string templateName, string receptor, PhoneCodeType type,
    string token, string? token2 = null, string? token3 = null);

}