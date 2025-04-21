using CovrMe.Models;
using CovrMe.Models.Users.Result;

namespace CovrMe.Services.Contracts
{
    public interface IAuthenticationService
    {
        public string GetBaseUrl();
        Task<RegisterUserResultModel> Register(string email, string password, HttpClient client);
        Task<LoginResultModel> Login(string email, string password, HttpClient client);
        Task<SendEmailForgottenPasswordResultModel> SendEmailWithCodeForgottenPassword(string email, HttpClient client);
        Task<CheckResetPasswordCodeResultModel> CheckResetPasswordCode(string email, string code, HttpClient client);
        Task<BaseResultModel> ResetUserPassword(string email, string password, HttpClient client);
        Task<UserModel> Authenticate(string userId, string jwt, HttpClient client);
        Task<BaseResultModel> ChangeUserPassword(string email, string password, string newPassword, HttpClient client);
    }
}
