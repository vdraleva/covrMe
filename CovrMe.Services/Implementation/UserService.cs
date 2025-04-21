using CovrMe.Models;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Users.Result.Payloads;
using CovrMe.Models.Users.Result.QueryResults;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace CovrMe.Services.Implementation
{
    public class UserService : IUserService
    {
        private string _url;
        private string _apiUrl;
        private GraphQLHttpClient _client;
        public UserService(string baseUrl)
        {
            this._url = baseUrl + GlobalConstants.GraphQlUrl;
            this._apiUrl = baseUrl;
        }

        public async Task<GetUsersFamilyAndFriendsResultModel> GetUserFamilyAndFriends(string userId, HttpClient client, string jwt)
        {
            var result = new GetUsersFamilyAndFriendsResultModel();

            string query = Queries.GetUserFamilyAndFriends;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = query;
                request.Variables = new { userId };

                var response = await this._client.SendMutationAsync<GetUsersFamilyAndFriendsResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.UsersFamilyAndFriends;
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<UserModel> GetUserById(string userId, HttpClient client, string jwt)
        {
            var result = new UserModel();

            string query = Queries.GetUserById;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = query;
                request.Variables = new { userId };

                var response = await this._client.SendMutationAsync<GetUserByIdResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.User.User.FirstOrDefault();
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<UserModel> EditUserInfo(EditUserInfoInput editUserInfoInput, HttpClient client, string jwt)
        {
            var result = new UserModel();

            string editUserInfoMutation = Mutations.EditUserInfoMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = editUserInfoMutation;
                request.Variables = new { editUserInfoInput };

                var response = await this._client.SendMutationAsync<EditUserInfoPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Result.User;
            }
            catch (Exception)
            {

                return result;
            }
        }

        public async Task<UserModel> AddUserToFamilyAndFriends(AddUserToFamilyAndFriendsInput addUserToFamilyAndFriendsInput, HttpClient client, string jwt)
        {
            var result = new UserModel();
            string addUserToFamilyAndFriendsMutation = Mutations.AddUserToFamilyAndFriends;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = addUserToFamilyAndFriendsMutation;
                request.Variables = new { addUserToFamilyAndFriendsInput };

                var response = await this._client.SendMutationAsync<AddUserToFamilyAndFriendsPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Result.User;
            }
            catch (Exception)
            {

                return result;
            }
        }

        public async Task<BaseResultModel> DeleteUser(DeleteUserInput deleteUserInput, HttpClient client, string jwt)
        {
            var result = new BaseResultModel();

            string deleteUserMutation = Mutations.DeleteUser;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = deleteUserMutation;
                request.Variables = new { deleteUserInput };

                var response = await this._client.SendMutationAsync<DeleteUserPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Result;
            }
            catch (Exception)
            {

                return result;
            }
        }

        public async Task<UserModel> GetUserVatUin(string userId, HttpClient client, string jwt)
        {
            var result = new UserModel();

            string query = Queries.GetUserVatUin;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = query;
                request.Variables = new { userId };

                var response = await this._client.SendMutationAsync<GetUserByIdResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.User.User.FirstOrDefault();
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        //public async Task<BaseResultModel> AddMultipleUserToFamilyAndFriends(AddMultipleUserToFamilyAndFriendsInput req, HttpClient client, string jwt)
        //{
        //    var url = this._apiUrl + GlobalConstants.AddMultipleUserToFamilyAndFriends;

        //    var result = new BaseResultModel();
        //    try
        //    {
        //        using (client)
        //        {
        //            StringContent content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, GlobalConstants.JsonType);
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        //            using (var response = await client.PostAsync(url, content))
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //                {
        //                    result = JsonConvert.DeserializeObject<BaseResultModel>(apiResponse);
        //                    if (result.Code == 1)
        //                    {
        //                        return result;
        //                    }
        //                    else
        //                    {
        //                        return result;
        //                    }
        //                }
        //                else 
        //                {
        //                    return result;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return result;
        //    }
        //}

        //public async Task<BaseResultModel> EditMultipleUserToFamilyAndFriends(EditMultipleFamilyFriendsUserInput req, HttpClient client, string jwt)
        //{
        //    var url = this._apiUrl + GlobalConstants.EditMultipleUserToFamilyAndFriends;

        //    var result = new BaseResultModel();
        //    try
        //    {
        //        using (client)
        //        {
        //            StringContent content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, GlobalConstants.JsonType);
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        //            using (var response = await client.PostAsync(url, content))
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //                {
        //                    result = JsonConvert.DeserializeObject<BaseResultModel>(apiResponse);
        //                    if (result.Code == 1)
        //                    {
        //                        return result;
        //                    }
        //                    else
        //                    {
        //                        return result;
        //                    }
        //                }
        //                else
        //                {
        //                    return result;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return result;
        //    }
        //}

        public async Task<int> AddMultipleUserToFamilyAndFriends(AddMultipleUserToFamilyAndFriendsInput addMultipleUserToFamilyAndFriendsInput, HttpClient client, string jwt)
        {
            int result = 0;

            string addMultipleUsersToFamilyAndFriendsMutation = Mutations.AddMultipleUsersToFamilyAndFriends;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = addMultipleUsersToFamilyAndFriendsMutation;
                request.Variables = new { addMultipleUserToFamilyAndFriendsInput };

                var response = await this._client.SendMutationAsync<AddMultipleUserToFamilyAndFriendsPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Result.Code;
            }
            catch (Exception)
            {

                return result;
            }
        }

        public async Task<int> EditMultipleUserToFamilyAndFriends(EditMultipleFamilyFriendsUserInput editMultipleFamilyFriendsUserInput, HttpClient client, string jwt)
        {
            int result = 0;

            string editMultipleUsersToFamilyAndFriendsMutation = Mutations.EditMultipleFamilyAndFriendsUser;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = editMultipleUsersToFamilyAndFriendsMutation;
                request.Variables = new { editMultipleFamilyFriendsUserInput };

                var response = await this._client.SendMutationAsync<EditMultipleFamilyFriendsUserPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Result.Code;
            }
            catch (Exception)
            {

                return result;
            }
        }
        public async Task<string> GetUserGuiltTypes(HttpClient client, string jwt)
        {
            string userGuiltMutation = Mutations.GetUserGuiltTypes;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = userGuiltMutation;

                var response = await this._client.SendMutationAsync<GetUserGuiltTypesPayload>(request);

                if (response.Errors != null)
                {
                    return string.Empty;
                }

                return response.Data.GuiltTypes;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
