using CovrMe.WebAPI.Data;
using CovrMe.WebAPI.Data.Context;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Models;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Users.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Services.Messaging;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMailService _mailService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<SocialUser> _socialUserRepository;
        private readonly IRepository<UserPasswordResetCode> _userPasswordResetCodeRepository;
        private readonly IUserService _userService;

        public AuthenticationService(ILogger<AuthenticationService> logger,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext dbContext,
            IMailService mailService, IRepository<SocialUser> socialUserRepository, IRepository<User> userRepository, IUserService userService,
            IRepository<UserPasswordResetCode> userPasswordResetCodeRepository)
        {
            this._logger = logger;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._configuration = configuration;
            this._dbContext = dbContext;
            this._mailService = mailService;
            this._userRepository = userRepository;
            this._socialUserRepository = socialUserRepository;
            this._userService = userService;
            this._userPasswordResetCodeRepository = userPasswordResetCodeRepository;
        }

        public async Task<LoginPayload> LoginAsync(LoginInput req)
        {
            var result = new LoginPayload();
            var user = new UserModel();
            result.User = user;
            result.Code = (int)GeneralStatusEnum.Unsuccess;
            result.Message = GeneralStatusEnum.Unsuccess.ToString();

            try
            {
                var currentAspNetUser = await this._userManager.FindByEmailAsync(req.Email);

                if (currentAspNetUser != null)
                {
                    var validCredentials = await this._signInManager.UserManager.CheckPasswordAsync(currentAspNetUser, req.Password);
                    if (validCredentials)
                    {
                        await PopulateLoginPayload(currentAspNetUser, result);
                    }
                    else
                    {
                        result.Code = (int)GeneralStatusEnum.Unsuccess;
                        result.Message = MessageConstants.InvalidCredentials;
                    }
                }
                else
                {
                    result.Code = (int)GeneralStatusEnum.Unsuccess;
                    result.Message = MessageConstants.UserDoNotExists;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                this._logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<LoginPayload> OAuthLoginAsync(OauthLoginInput req, AuthenticationProviderType authType)
        {
            var result = new LoginPayload();
            result.User = new UserModel();
            result.Code = (int)GeneralStatusEnum.Unsuccess;
            result.Message = GeneralStatusEnum.Unsuccess.ToString();

            try
            {
                SocialUser socialLoginUser = this._socialUserRepository.Filter(x => x.SocialUserId == req.NameIdentifier).FirstOrDefault();
                var currentAspNetUser = await GetAppUserByOauthIdOrEmail(req);

                //CREATE USER IF WE HAVE ALREADY HAVE USER EMAIL REGISTERED NORMALLY
                if (socialLoginUser == null && currentAspNetUser != null) 
                {
                    await CreateSocialUser(req.NameIdentifier, currentAspNetUser.Id, authType);
                }

                if (currentAspNetUser != null)
                {
                    await PopulateLoginPayload(currentAspNetUser, result);
                }
                else
                {
                    // CREATE USER
                    using var transaction = _dbContext.Database.BeginTransaction();
                    try
                    {
                        ApplicationUser aspNetUser = await CreateIdentity(req, result);
                        await CreateSocialUser(req.NameIdentifier, aspNetUser.Id, authType);
                        await PopulateLoginPayload(aspNetUser, result);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex.Message);
                        result.Message = MessageConstants.Error;
                        transaction.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                this._logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<AuthenticatePayload> Authenticate(AuthenticateInput req)
        {
            var result = new AuthenticatePayload();
            var user = new UserModel();
            result.User = user;
            result.Code = (int)GeneralStatusEnum.Unsuccess;
            result.Message = GeneralStatusEnum.Unsuccess.ToString();

            try
            {
                var currentUser = this._userService.GetCurrentUserById(req.UserId);

                if (currentUser != null)
                {
                    result.User.Id = currentUser.Id.ToString();
                    result.User.Email = currentUser.Email;
                    result.User.PhoneNumber = currentUser.PhoneNumber;
                    result.User.LastName = currentUser.LastName;
                    result.User.FirstName = currentUser.FirstName;
                    result.User.SurName = currentUser.SurName;
                    result.User.UiNumber = currentUser.UiNumber;
                    result.User.Address = currentUser.Address;
                    result.User.VatNumber = currentUser.VatNumber;

                    result.Code = (int)GeneralStatusEnum.Success;
                    result.Message = GeneralStatusEnum.Success.ToString();
                }
            }


            catch (Exception ex)
            {
                result.Message = ex.Message;
                this._logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<RegisterUserPayload> RegisterUserAsync(RegisterUserInput req)
        {
            var result = new RegisterUserPayload();
            result.Code = (int)GeneralStatusEnum.Unsuccess;
            result.Message = MessageConstants.Error;

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await CreateIdentity(req, result);
                transaction.Commit();

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                result.Message = MessageConstants.Error;
                transaction.Rollback();
            }

            return result;
        }

        public async Task<ResetUserPasswordPayload> ResetUserPasswordAsync(ResetUserPasswordInput req)
        {
            var result = new ResetUserPasswordPayload();
            result.Code = (int)GeneralStatusEnum.Unsuccess;
            result.Message = GeneralStatusEnum.Unsuccess.ToString();

            try
            {
                var currentUser = await this._userManager.FindByEmailAsync(req.Email);

                if (currentUser != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(currentUser);

                    var resetResult = await _userManager.ResetPasswordAsync(currentUser, token, req.Password);

                    if (resetResult.Succeeded)
                    {
                        var userPasswordResetCodeExists = _userPasswordResetCodeRepository
                        .AllAsNoTracking()
                        .Where(x => x.AspNetUserId == currentUser.Id)
                        .FirstOrDefault();

                        if (userPasswordResetCodeExists != null)
                        {
                            _userPasswordResetCodeRepository.Delete(userPasswordResetCodeExists);
                            await _userPasswordResetCodeRepository.SaveChangesAsync();
                        }

                        result.Code = (int)GeneralStatusEnum.Success;
                        result.Message = GeneralStatusEnum.Success.ToString();
                    }
                }
                else
                {
                    result.Code = (int)GeneralStatusEnum.Unsuccess;
                    result.Message = MessageConstants.UserDoNotExists;
                }
            }
            catch (Exception ex)
            {

                this._logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<SendEmailForgottenPasswordPayload> SendEmailWithCodeForgottenPassword(SendEmailForgottenPasswordInput req)
        {
            var result = new SendEmailForgottenPasswordPayload();
            result.Code = (int)GeneralStatusEnum.Unsuccess;
            result.Message = GeneralStatusEnum.Unsuccess.ToString();

            try
            {
                var currentUser = await this._userManager.FindByEmailAsync(req.Email);

                if (currentUser != null)
                {
                    string confirmationCode = Helpers.GenerateVerificationCode();

                    var isSent = SendUserCodeEmail(confirmationCode, req.Email);

                    if (isSent)
                    {
                        await SaveUserPasswordResetCode(currentUser, confirmationCode);

                        result.Code = (int)GeneralStatusEnum.Success;
                        result.Message = GeneralStatusEnum.Success.ToString();
                        // result.ConfirmationCode = confirmationCode;
                    }
                }
                else
                {
                    result.Code = (int)GeneralStatusEnum.Unsuccess;
                    result.Message = MessageConstants.UserDoNotExists;
                }
            }
            catch (Exception ex)
            {

                this._logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<CheckResetPasswordCodePayload> CheckResetPasswordCode(CheckResetPasswordCodeInput req)
        {
            var result = new CheckResetPasswordCodePayload();
            result.Code = (int)GeneralStatusEnum.Unsuccess;
            result.Message = GeneralStatusEnum.Unsuccess.ToString();
            result.IsCodeValid = false;

            try
            {
                var currentUser = await this._userManager.FindByEmailAsync(req.Email);

                if (currentUser != null)
                {
                    string confirmationCode = req.Code;

                    var userPasswordResetCodeExists = _userPasswordResetCodeRepository
                        .AllAsNoTracking()
                        .Where(x => x.AspNetUserId == currentUser.Id && x.Code == confirmationCode)
                        .FirstOrDefault();

                    if (userPasswordResetCodeExists != null)
                    {
                        result.Code = (int)GeneralStatusEnum.Success;
                        result.Message = GeneralStatusEnum.Success.ToString();
                        result.IsCodeValid = true;
                    }
                }
                else
                {
                    result.Code = (int)GeneralStatusEnum.Unsuccess;
                    result.Message = MessageConstants.CodeDoNotExists;
                    result.IsCodeValid = false;
                }
            }
            catch (Exception ex)
            {

                this._logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<ChangeUserPasswordPayload> ChangeUserPasswordAsync(ChangeUserPasswordInput req)
        {
            var result = new ChangeUserPasswordPayload();
            result.Code = (int)GeneralStatusEnum.Unsuccess;
            result.Message = GeneralStatusEnum.Unsuccess.ToString();

            try
            {
                var currentUser = await this._userManager.FindByEmailAsync(req.Email);

                if (currentUser != null)
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, req.Password, req.NewPassword);

                    if (changePasswordResult.Succeeded)
                    {
                        result.Code = (int)GeneralStatusEnum.Success;
                        result.Message = GeneralStatusEnum.Success.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                this._logger.LogError(ex.Message);
            }

            return result;
        }

        public async Task<ICollection<string>> GetUserRoles(string email)
        {
            var currentAspNetUser = await this._userManager.FindByEmailAsync(email);
            IList<string> roles = new List<string>();
            if (currentAspNetUser != null)
            {
                roles = await _userManager.GetRolesAsync(currentAspNetUser);
            }

            return roles;
        }

        private string GenerateToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[GlobalConstants.JwtKey]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,email),
                new Claim(ClaimTypes.Role,GlobalConstants.UserRoleName)
            };
            var token = new JwtSecurityToken(_configuration[GlobalConstants.JwtIssuer],
                _configuration[GlobalConstants.JwtAudience],
                claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool SendUserCodeEmail(string code, string emailTo)
        {
            string email = GlobalConstants.PasswordRecoveryEmail;
            string subject = GlobalConstants.PasswordRecoveryHeader;

            email = email.Replace("$code$", code);

            var mailData = new MailData();
            mailData.EmailSubject = subject;
            mailData.EmailBody = email;
            mailData.EmailToId = emailTo;
            mailData.EmailToName = emailTo;

            var result = _mailService.SendMail(mailData);

            return result;
        }

        private async Task<SocialUser> CreateSocialUser(string socialUserId, string aspUserId, AuthenticationProviderType authType)
        {
            SocialUser socialUser = new SocialUser()
            {
                SocialUserId = socialUserId,
                AspUserId = aspUserId,
                Provider = authType.ToString(),
                ProviderId = (byte)authType
            };

            await this._socialUserRepository.AddAsync(socialUser);
            await this._socialUserRepository.SaveChangesAsync();

            return socialUser;
        }

        private async Task<ApplicationUser> CreateIdentity(RegisterUserInput req, BaseResultModel result)
        {
            // IN CASE SOCIAL LOGIN DOESN'T HAVE EMAIL ADDRESS
            string serviceEmail = $"srusr-{Guid.NewGuid().ToString()}@mail.com";

            var identityResult = new IdentityResult();

            var aspNetUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = req.Email != null ? req.Email : serviceEmail,
                UserName = req.Email != null ? req.Email : serviceEmail,
            };

            identityResult = await _userManager.CreateAsync(aspNetUser, req.Password != null ? req.Password : "P@ssw0rd");

            if (identityResult.Succeeded)
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    AspNetUserId = aspNetUser.Id,
                    Email = req.Email != null ? req.Email : serviceEmail,
                };

                await this._userRepository.AddAsync(newUser);
                await this._userRepository.SaveChangesAsync();

                var userRole = await _roleManager.FindByNameAsync(GlobalConstants.UserRoleName);

                if (userRole != null)
                {
                    await this._userManager.AddToRoleAsync(aspNetUser, GlobalConstants.UserRoleName);
                }
                else
                {
                    var resultRole = await this._roleManager.CreateAsync(new IdentityRole(GlobalConstants.UserRoleName));
                    if (resultRole.Succeeded)
                    {
                        await this._userManager.AddToRoleAsync(aspNetUser, GlobalConstants.UserRoleName);
                    }
                }
                result.Code = (int)GeneralStatusEnum.Success;
                result.Message = GeneralStatusEnum.Success.ToString();
            }
            else
            {
                if (identityResult.Errors.Any(x => x.Code == GlobalConstants.DuplicateEmailCode))
                {
                    result.Message = MessageConstants.DuplicateEmailError;
                }
            }
            return aspNetUser;
        }

        private async Task PopulateLoginPayload(ApplicationUser appUser, LoginPayload result)
        {
            var currentUser = this._userService.GetUserByAspNetUserId(appUser.Id);

            string jwt = this.GenerateToken(appUser.Email);
            result.Jwt = jwt;

            var roles = await this.GetUserRoles(appUser.Email);
            result.User = currentUser;
            result.User.Roles = roles;

            result.Code = (int)GeneralStatusEnum.Success;
            result.Message = GeneralStatusEnum.Success.ToString();
        }

        private async Task<ApplicationUser?> GetAppUserByOauthIdOrEmail(OauthLoginInput req)
        {
            ApplicationUser? aspUser = null;
            SocialUser? socialUser = _socialUserRepository.Filter(x => x.SocialUserId == req.NameIdentifier).FirstOrDefault();

            aspUser = await this._userManager.FindByEmailAsync(req.Email != null ? req.Email : "");

            if (aspUser == null && socialUser != null)
            {
                aspUser = await this._userManager.FindByIdAsync(socialUser.AspUserId);
            }

            return aspUser;
        }

        private User GetUserByAspNetUserId(string userId)
        {
            var user = this._userRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.AspNetUserId == userId);


            return user;
        }

        private User GetUserById(string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var user = this._userRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == currentUserId);

            return user;
        }
        private async Task SaveUserPasswordResetCode(ApplicationUser? currentUser, string confirmationCode)
        {
            var userPasswordResetCode = new UserPasswordResetCode
            {
                Id = Guid.NewGuid(),
                AspNetUserId = currentUser.Id,
                Code = confirmationCode,
            };

            await _userPasswordResetCodeRepository.AddAsync(userPasswordResetCode);
            await _userPasswordResetCodeRepository.SaveChangesAsync();
        }
    }
}
