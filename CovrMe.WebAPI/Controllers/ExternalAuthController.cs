using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Shared.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Web;
using IAuthenticationService = CovrMe.WebAPI.Services.LocalServices.Contracts.IAuthenticationService;

namespace CovrMe.WebAPI.Controllers
{
    public class ExternalAuthController : Controller
    {
        private IAuthenticationService _authService;
        public ExternalAuthController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService;
        }

        [HttpGet("auth/google/login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/auth/google/callback",

            };
            properties.Parameters.Add("prompt", "consent");

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("auth/google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (authenticateResult.Succeeded)
            {
                var email = authenticateResult.Principal?.FindFirst(ClaimTypes.Email)?.Value;
                var givenName = authenticateResult.Principal?.FindFirst(ClaimTypes.GivenName)?.Value;
                var name = authenticateResult.Principal?.FindFirst(ClaimTypes.Name)?.Value;
                var nameIdentifier = authenticateResult.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                OauthLoginInput loginInput = new OauthLoginInput();
                loginInput.Email = email;
                loginInput.FirstName = givenName;
                loginInput.NameIdentifier = nameIdentifier;

                var result = await _authService.OAuthLoginAsync(loginInput, AuthenticationProviderType.Google);
                string loginPayloadJson = JsonSerializer.Serialize(result);
                var encodedJson = HttpUtility.UrlEncode(loginPayloadJson);
                return Redirect($"myapp://callback?loginResult={encodedJson}");
            }
            else
            {
                return Unauthorized("Sign In with Google - Denied!");
            }

        }

        [HttpGet("auth/facebook/login")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/auth/facebook/callback"
            };

            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("auth/facebook/callback")]
        public async Task<IActionResult> FacebookCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

            if (authenticateResult.Succeeded)
            {
                var email = authenticateResult.Principal?.FindFirst(ClaimTypes.Email)?.Value;
                var givenName = authenticateResult.Principal?.FindFirst(ClaimTypes.GivenName)?.Value;
                var surName = authenticateResult.Principal?.FindFirst(ClaimTypes.Surname)?.Value;
                var nameIdentifier = authenticateResult.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                OauthLoginInput loginInput = new OauthLoginInput();
                loginInput.Email = email;
                loginInput.FirstName = givenName;
                loginInput.SurName = surName;
                loginInput.NameIdentifier = nameIdentifier;

                var result = await _authService.OAuthLoginAsync(loginInput, AuthenticationProviderType.Facebook);
                string loginPayloadJson = JsonSerializer.Serialize(result);
                var encodedJson = HttpUtility.UrlEncode(loginPayloadJson);
                return Redirect($"myapp://callback?loginResult={encodedJson}");
            }
            else
            {
                return Unauthorized("Sign In with Facebook - Denied!");
            }
        }
    }
}
