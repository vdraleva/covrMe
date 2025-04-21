using CovrMe.Models;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Users.Result.Payloads;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace CovrMe.Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private string _url;
        private string _baseUrl;
        private GraphQLHttpClient _client;
        public AuthenticationService(string baseUrl)
        {
            this._url = baseUrl + GlobalConstants.GraphQlUrl;
            this._baseUrl = baseUrl;
        }

        public string GetBaseUrl()
        {
            return this._baseUrl;
        }

        public async Task<RegisterUserResultModel> Register(string email, string password, HttpClient client)
        {
            var registerUserInput = new RegisterUserInput();


            registerUserInput.Email = email;
            registerUserInput.Password = password;

            var result = new RegisterUserResultModel();
            string registerUserAsyncMutation = Mutations.RegisterMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);

            try
            {
                var request = new GraphQLRequest();
                request.Query = registerUserAsyncMutation;
                request.Variables = new { registerUserInput };

                var response = await this._client.SendMutationAsync<RegisterUserPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }


                return response.Data.RegisterUser;
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<LoginResultModel> Login(string email, string password, HttpClient client)
        {
            var loginInput = new LoginInput();

            loginInput.Email = email;
            loginInput.Password = password;

            var result = new LoginResultModel();
            var user = new UserModel();
            result.User = user;

            string loginMutation = Mutations.LoginMutation;
            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            try
            {
                var request = new GraphQLRequest();
                request.Query = loginMutation;
                request.Variables = new { loginInput };

                var response = await this._client.SendMutationAsync<LoginPayload>(request);
                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Login;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"this error is: {ex.Message}");
                return result;
            }
        }

        public async Task<UserModel> Authenticate(string userId, string jwt, HttpClient client)
        {
            var authenticateInput = new AuthenticateInput();

            authenticateInput.UserId = userId;

            var result = new AuthenticatePayload();
            var user = new UserModel();

            string authenticateMutation = Mutations.AuthenticateMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = authenticateMutation;
                request.Variables = new { authenticateInput };

                var response = await this._client.SendMutationAsync<AuthenticatePayload>(request);

                if (response.Errors != null)
                {
                    return user;
                }

                return response.Data.Authenticate.User;
            }
            catch (Exception ex)
            {

                return user;
            }
        }

        public async Task<SendEmailForgottenPasswordResultModel> SendEmailWithCodeForgottenPassword(string email, HttpClient client)
        {
            var sendEmailInput = new SendEmailForgottenPasswordInput();

            sendEmailInput.Email = email;

            var result = new SendEmailForgottenPasswordResultModel();

            string sendEmailForgottenPassword = Mutations.SendEmailForgottenPassword;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);

            try
            {
                var request = new GraphQLRequest();
                request.Query = sendEmailForgottenPassword;
                request.Variables = new { sendEmailInput };

                var response = await this._client.SendMutationAsync<SendEmailForgottenPasswordPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.SendEmailForgottenPassword;
            }
            catch (Exception)
            {

                return result;
            }
        }
        public async Task<CheckResetPasswordCodeResultModel> CheckResetPasswordCode(string email, string code, HttpClient client)
        {
            var checkResetPasswordCodeInput = new CheckResetPasswordCodeInput();

            checkResetPasswordCodeInput.Email = email;
            checkResetPasswordCodeInput.Code = code;

            var result = new CheckResetPasswordCodeResultModel();

            string checkResetPasswordCode = Mutations.CheckResetPasswordCode;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);

            try
            {
                var request = new GraphQLRequest();
                request.Query = checkResetPasswordCode;
                request.Variables = new { checkResetPasswordCodeInput };

                var response = await this._client.SendMutationAsync<CheckResetPasswordCodePayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.CheckResetPasswordCode;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public async Task<BaseResultModel> ResetUserPassword(string email, string password, HttpClient client)
        {
            var resetUserPasswordInput = new ResetUserPasswordInput();

            resetUserPasswordInput.Email = email;
            resetUserPasswordInput.Password = password;

            var result = new BaseResultModel();

            string resetUserPasswordMutation = Mutations.ResetUserPassword;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);

            try
            {
                var request = new GraphQLRequest();
                request.Query = resetUserPasswordMutation;
                request.Variables = new { resetUserPasswordInput };

                var response = await this._client.SendMutationAsync<ResetUserPasswordPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.ResetUserPassword;
            }
            catch (Exception)
            {

                return result;
            }
        }

        public async Task<BaseResultModel> ChangeUserPassword(string email, string password, string newPassword, HttpClient client)
        {
            var changeUserPasswordInput = new ChangeUserPasswordInput();

            changeUserPasswordInput.Email = email;
            changeUserPasswordInput.Password = password;
            changeUserPasswordInput.NewPassword = newPassword;

            var result = new BaseResultModel();

            string changeUserPasswordMutation = Mutations.ChangeUserPassword;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);

            try
            {
                var request = new GraphQLRequest();
                request.Query = changeUserPasswordMutation;
                request.Variables = new { changeUserPasswordInput };

                var response = await this._client.SendMutationAsync<ChangeUserPasswordPayload>(request);

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
    }
}
