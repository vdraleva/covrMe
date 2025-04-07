using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Users.Payloads;
using CovrMe.WebAPI.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IAuthenticationService
    {
        Task<LoginPayload> LoginAsync(LoginInput req);
        Task<LoginPayload> OAuthLoginAsync(OauthLoginInput req, AuthenticationProviderType authType);
        Task<RegisterUserPayload> RegisterUserAsync(RegisterUserInput req);
        Task<ResetUserPasswordPayload> ResetUserPasswordAsync(ResetUserPasswordInput req);
        Task<SendEmailForgottenPasswordPayload> SendEmailWithCodeForgottenPassword(SendEmailForgottenPasswordInput req);
        Task<CheckResetPasswordCodePayload> CheckResetPasswordCode(CheckResetPasswordCodeInput req);
        Task<ChangeUserPasswordPayload> ChangeUserPasswordAsync(ChangeUserPasswordInput req);
        Task<AuthenticatePayload> Authenticate(AuthenticateInput req);
        Task<ICollection<string>> GetUserRoles(string email);
    }
}
