using CovrMe.Models;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Services.Contracts
{
    public interface IUserService
    {
        Task<GetUsersFamilyAndFriendsResultModel> GetUserFamilyAndFriends(string userId, HttpClient client, string jwt);
        Task<UserModel> GetUserById(string userId, HttpClient client, string jwt);
        Task<UserModel> EditUserInfo(EditUserInfoInput editUserInfoInput, HttpClient client, string jwt);
        Task<UserModel> AddUserToFamilyAndFriends(AddUserToFamilyAndFriendsInput addUserToFamilyAndFriendsInput, HttpClient client, string jwt);
        Task<BaseResultModel> DeleteUser(DeleteUserInput deleteUserInput, HttpClient client, string jwt);
        Task<UserModel> GetUserVatUin(string userId, HttpClient client, string jwt);
        //Task<BaseResultModel> AddMultipleUserToFamilyAndFriends(AddMultipleUserToFamilyAndFriendsInput req, HttpClient client, string jwt);
        //Task<BaseResultModel> EditMultipleUserToFamilyAndFriends(EditMultipleFamilyFriendsUserInput req, HttpClient client, string jwt);
        Task<int> AddMultipleUserToFamilyAndFriends(AddMultipleUserToFamilyAndFriendsInput addMultipleUserToFamilyAndFriendsInput, HttpClient client, string jwt);
        Task<int> EditMultipleUserToFamilyAndFriends(EditMultipleFamilyFriendsUserInput editMultipleUserToFamilyAndFriendsInput, HttpClient client, string jwt);
        Task<string> GetUserGuiltTypes(HttpClient client, string jwt);
    }
}
