using HotChocolate.Authorization;
using CovrMe.WebAPI.Data;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Users.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;

namespace CovrMe.WebAPI.Queries
{
    [ExtendObjectType(typeof(BaseQuery))]
    public class UserQuery
    {
        [GraphQLName("user")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]        
        public IQueryable<UserModel> GetUserById([Service] IUserService userService, string userId)
        {
            var user = userService.GetUserById(userId);

            return user;
        }

        [GraphQLName("users")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<UserModel> GetUsers([Service] IUserService userService)
        {
            var users = userService.GetUsers();

            return users;
        }

        [GraphQLName("usersCount")]
        [Authorize]
        public int GetUsersCount([Service] IUserService userService)
        {
            var users = userService.GetUsersCount();

            return users;
        }

        [GraphQLName("usersForPage")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<UserModel> GetUsersForPage([Service] IUserService userService, UsersForPageInput usersForPageInput)
        {
            var users = userService.GetUsersForPage(usersForPageInput);

            return users;
        }

        [GraphQLName("aspUsers")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<UserModel> GetAspUsers([Service] IUserService userService)
        {
            var user = userService.GetAspUsers<UserModel>();

            return user;
        }

        [GraphQLName("getUsersFamilyAndFriends")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<UserModel> GetUsersFamilyAndFriends([Service] IUserService userService, string userId)
        {
            var user = userService.GetUsersFamilyAndFriends(userId);

            return user;
        }



    }
}
