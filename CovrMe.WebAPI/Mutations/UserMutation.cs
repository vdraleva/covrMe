using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Users.Payloads;
using CovrMe.WebAPI.Models.Result.Vehicles.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Enums;

namespace CovrMe.WebAPI.Mutations
{
    [ExtendObjectType(typeof(BaseMutation))]
    public class UserMutation
    {
        [AllowAnonymous]
        public async Task<LoginPayload> LoginAsync([Service] IAuthenticationService authService, LoginInput input)
        {
            var result = await authService.LoginAsync(input);

            return result;
        }

        [AllowAnonymous]
        public async Task<RegisterUserPayload> RegisterUserAsync([Service] IAuthenticationService authService, RegisterUserInput input)
        {
            var result = await authService.RegisterUserAsync(input);

            return result;
        }

        [AllowAnonymous]
        public async Task<ResetUserPasswordPayload> ResetUserPasswordAsync([Service] IAuthenticationService authService, ResetUserPasswordInput input)
        {
            var result = await authService.ResetUserPasswordAsync(input);

            return result;
        }

        [AllowAnonymous]
        public async Task<SendEmailForgottenPasswordPayload> SendEmailForgottenPassword([Service] IAuthenticationService authService, SendEmailForgottenPasswordInput input)
        {
            var result = await authService.SendEmailWithCodeForgottenPassword(input);
            return result;
        }

        [AllowAnonymous]
        public async Task<CheckResetPasswordCodePayload> CheckResetPasswordCode([Service] IAuthenticationService authService, CheckResetPasswordCodeInput input)
        {
            var result = await authService.CheckResetPasswordCode(input);
            return result;
        }

        [AllowAnonymous]
        public async Task<ChangeUserPasswordPayload> ChangeUserPasswordAsync([Service] IAuthenticationService authService, ChangeUserPasswordInput input)
        {
            var result = await authService.ChangeUserPasswordAsync(input);

            return result;
        }

        [Authorize]
        public Task<AuthenticatePayload> Authenticate([Service] IAuthenticationService authService, AuthenticateInput input)
        {
            var result = authService.Authenticate(input);

            return result;
        }

        [Authorize]
        public async Task<AddUserToFamilyAndFriendsPayload> AddUserToFamilyAndFriends([Service] IUserService userService, AddUserToFamilyAndFriendsInput input)
        {
            var result = await userService.AddUserToFamilyAndFriends(input);

            return result;
        }

        [Authorize]
        public async Task<EditUserInfoPayload> EditUserInfo([Service] IUserService userService, EditUserInfoInput input)
        {
            var result = new EditUserInfoPayload();
            var user = new UserModel();

            if (string.IsNullOrEmpty(input.ParentUserId))
            {
                user = await userService.EditUserInfo(input);
            }
            else
            {
                user = await userService.EditFamilyFriendsUser(input);
            }
            

            result.User = user;
            return result;
        }

        [AllowAnonymous]
        public async Task<UserRolesPayload> UserRoles([Service] IAuthenticationService authService, UserRolesInput input)
        {
            var result = new UserRolesPayload();
            var roles = await authService.GetUserRoles(input.Email);

            result.Roles = roles;
            return result;
        }

        [Authorize]
        public async Task<DeleteUserPayload> DeleteUser([Service] IUserService userService, DeleteUserInput input)
        {
            var result = await userService.DeleteUser(input);

            return result;
        }

        [Authorize]
        public async Task<AddMultipleUserToFamilyAndFriendsPayload> AddMultipleUserToFamilyAndFriends([Service] IUserService userService, AddMultipleUserToFamilyAndFriendsInput input)
        {
            var payload = new AddMultipleUserToFamilyAndFriendsPayload();
            var result = await userService.AddMultipleUserToFamilyAndFriends(input.Users);

            payload.Code = result.Code;

            return payload;
        }

        [Authorize]
        public async Task<EditMultipleFamilyFriendsUserPayload> EditMultipleFamilyFriendsUser([Service] IUserService userService, EditMultipleFamilyFriendsUserInput input)
        {
            var payload = new EditMultipleFamilyFriendsUserPayload();

            var result = await userService.EditMultipleFamilyFriendsUser(input.Users);
            payload.Code = result.Code;

            return payload;
        }

        [Authorize]
        public GetUserGuiltTypesPayload GetUserGuiltTypes([Service] IUserService userService)
        {
            var result = new GetUserGuiltTypesPayload();
            var guiltTypes = userService.GetUsersGuiltTypes();

            result.GuiltTypes = guiltTypes;
            return result;
        }

    }
}
