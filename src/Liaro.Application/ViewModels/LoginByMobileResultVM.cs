namespace Liaro.Application.ViewModels;


public class LoginByMobileResultVM
{
    public string? Error { get; set; }
    public bool IsSuccess => string.IsNullOrEmpty(Error);
}