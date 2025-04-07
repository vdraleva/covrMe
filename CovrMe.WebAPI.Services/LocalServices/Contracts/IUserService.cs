using CovrMe.WebAPI.Data;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Users.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IUserService
    {
        IQueryable<T> GetAspUsers<T>();
        IQueryable<T> GetUsersFamilyAndFriends<T>(string userId);
        IQueryable<UserModel> GetUserById(string userId);
        IQueryable<UserModel> GetUsers(); 
        int GetUsersCount();
        IQueryable<UserModel> GetUsersForPage(UsersForPageInput usersForPageInput);
        IQueryable<UserModel> GetUsersFamilyAndFriends(string userId);
        Task<AddUserToFamilyAndFriendsPayload> AddUserToFamilyAndFriends(AddUserToFamilyAndFriendsInput req);
        UserModel GetCurrentUserById(string userId);
        Task<UserModel> EditUserInfo(EditUserInfoInput req);
        Task<UserModel> EditFamilyFriendsUser(EditUserInfoInput req);
        Task<DeleteUserPayload> DeleteUser(DeleteUserInput req);
        UserModel GetUserByAspNetUserId(string aspNetUserId);
        Task<string> AddUser(UserModel req);
        Task<BaseResultModel> AddMultipleUserToFamilyAndFriends(List<AddUserToFamilyAndFriendsInput> users);
        Task<BaseResultModel> EditMultipleFamilyFriendsUser(List<EditUserInfoInput> users);
        string GetUsersGuiltTypes(int hashCode);
        string GetUsersGuiltTypes();
    }
}
