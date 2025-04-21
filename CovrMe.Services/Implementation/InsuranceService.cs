using CovrMe.Models;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.CivilInsurance.Request;
using CovrMe.Models.Insurances.Request;
using CovrMe.Models.Insurances.Request.Casco;
using CovrMe.Models.Insurances.Request.CivilInsurances;
using CovrMe.Models.Insurances.Request.HealthInsurance;
using CovrMe.Models.Insurances.Request.MountainInsurance;
using CovrMe.Models.Insurances.Request.MyThings;
using CovrMe.Models.Insurances.Request.TravelInsurance;
using CovrMe.Models.Insurances.Result;
using CovrMe.Models.Insurances.Result.CivilInsurances;
using CovrMe.Models.Insurances.Result.Payloads;
using CovrMe.Models.Questions.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CovrMe.Services.Implementation
{
    public class InsuranceService : IInsuranceService
    {
        private string _url;
        private string _apiUrl;
        private GraphQLHttpClient _client;

        public InsuranceService(string baseUrl)
        {
            this._url = baseUrl + GlobalConstants.GraphQlUrl;
            this._apiUrl = baseUrl;
        }

        #region Civil Insurance

        public async Task<CivilInsuranceSearchResultModel> CivilInsuranceSearch(CivilInsuranceSearchInput civilInsuranceSearchInput, HttpClient client, string jwt)
        {
            var result = new CivilInsuranceSearchResultModel();

            string query = Mutations.CivilInsuranceSearch;

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
                request.Variables = new { civilInsuranceSearchInput };

                var response = await this._client.SendMutationAsync<CivilInsuranceSearchPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.CivilInsuranceSearch;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<CivilInsuranceSearchResultModel> CivilInsuranceLongSearch(CivilInsuranceLongSearchInput civilInsuranceLongSearchInput, HttpClient client, string jwt)
        {
            var result = new CivilInsuranceSearchResultModel();

            string longSearchMutation = Mutations.CivilInsuranceLongSearch;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = longSearchMutation;
                request.Variables = new { civilInsuranceLongSearchInput };

                var response = await this._client.SendMutationAsync<CivilInsuranceLongSearchPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.CivilInsuranceLongSearch;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<CivilInsurancePolicyResultModel> CivilInsurancePolicy(CivilInsurancePolicyInput civilInsurancePolicyInput, HttpClient client, string jwt)
        {
            var result = new CivilInsurancePolicyResultModel();

            string query = Mutations.CivilInsurancePolicy;

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
                request.Variables = new { civilInsurancePolicyInput };

                var response = await this._client.SendMutationAsync<CivilInsurancePolicyPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.PolicyInfo;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<CivilInsurancePolicyResultModel> CivilInsuranceLongPolicy(CivilInsuranceLongPolicyInput civilInsuranceLongPolicyInput, HttpClient client, string jwt)
        {
            var result = new CivilInsurancePolicyResultModel();

            string civilInsuranceLongPolicyMutation = Mutations.CivilInsuranceLongPolicy;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = civilInsuranceLongPolicyMutation;
                request.Variables = new { civilInsuranceLongPolicyInput };

                var response = await this._client.SendMutationAsync<CivilInsuranceLongPolicyPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.PolicyInfo;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<bool> UpdateCivilInsuranceDocumentStatus(string docId, string jwt, HttpClient client)
        {
            var updateCivilInsuranceDocumentStatusInput = new UpdateCivilInsuranceDocumentStatusInput();
            updateCivilInsuranceDocumentStatusInput.DocumentId = docId;

            string UpdateCivilInsuranceDocumentStatusMutation = Mutations.UpdateCivilInsuranceDocs;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = UpdateCivilInsuranceDocumentStatusMutation;
                request.Variables = new { updateCivilInsuranceDocumentStatusInput };

                var response = await this._client.SendMutationAsync<UpdateCivilInsuranceDocumentStatusPayload>(request);

                if (response.Errors != null)
                {
                    return false;
                }

                return response.Data.Info.IsUpdated;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<BaseResultModel> CivilInsuranceInstallmentPay(CivilInsuranceInstallmentInput civilInsuranceInstallmentInput, string jwt, HttpClient client)
        {

            string civilInsuranceInstallmentMutation = Mutations.CivilInsuranceInstallment;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = civilInsuranceInstallmentMutation;
                request.Variables = new { civilInsuranceInstallmentInput };

                var response = await this._client.SendMutationAsync<CivilInsuranceInstallmentPayload>(request);

                if (response.Errors != null)
                {
                    return new BaseResultModel();
                }

                return response.Data.Result;

            }
            catch (Exception ex)
            {

                return new BaseResultModel();
            }
        }

        public async Task<Stream> GetCivilInsuranceGreenCardPdf(string userId, string insuranceId, string policyNo, string jwt, HttpClient client)
        {
            var request = new GetPolicyRequestModel();
            request.UserId = userId;
            request.InsuranceId = insuranceId;
            request.PolicyNo = policyNo;

            var url = _apiUrl + GlobalConstants.GetGreenCard;

            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, GlobalConstants.JsonType);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Travel Insurance

        public async Task<List<CalculationModel>> TravelCalculation(TravelCalculationInput travelCalculationInput, string jwt, HttpClient client)
        {

            string travelCalculationMutation = Mutations.TravelCalculation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = travelCalculationMutation;
                request.Variables = new { travelCalculationInput };

                var response = await this._client.SendMutationAsync<TravelCalculationPayload>(request);

                if (response.Errors != null)
                {
                    return new List<CalculationModel>();
                }

                return response.Data.Result.Offers;

            }
            catch (Exception ex)
            {

                return new List<CalculationModel>();
            }
        }

        public async Task<PolicyResultModel> TravelPolicy(TravelPolicyInput travelPolicyInput, string jwt, HttpClient client)
        {
            var result = new PolicyResultModel();
            string travelPolicyMutation = Mutations.TravelPolicy;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = travelPolicyMutation;
                request.Variables = new { travelPolicyInput };

                var response = await this._client.SendMutationAsync<TravelPolicyPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                if (response.Data.Result != null)
                {
                    result.PolicyNumber = response.Data.Result.PolicyNo;
                    result.ErrorCode = response.Data.Result.ErrorId;
                    result.Message = response.Data.Result.Message;
                }
                return result;

            }
            catch (Exception ex)
            {

                return result;
            }
        }

        #endregion

        #region Mountain

        public async Task<List<CalculationModel>> MountainCalculation(MountainCalculationInput mountainCalculationInput, string jwt, HttpClient client)
        {

            string mountainalculationMutation = Mutations.MountainCalculation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = mountainalculationMutation;
                request.Variables = new { mountainCalculationInput };

                var response = await this._client.SendMutationAsync<MountainCalculationPayload>(request);

                if (response.Errors != null)
                {
                    return new List<CalculationModel>();
                }

                return response.Data.Result.Offers;

            }
            catch (Exception ex)
            {

                return new List<CalculationModel>();
            }
        }

        public async Task<PolicyResultModel> MountainPolicy(MountainPolicyInput mountainPolicyInput, string jwt, HttpClient client)
        {
            var result = new PolicyResultModel();
            string mountainPolicyMutation = Mutations.MountainPolicy;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = mountainPolicyMutation;
                request.Variables = new { mountainPolicyInput };

                var response = await this._client.SendMutationAsync<MountainPolicyPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                if (response.Data.Result != null)
                {
                    result.PolicyNumber = response.Data.Result.PolicyNo;
                    result.ErrorCode = response.Data.Result.ErrorId;
                    result.Message = response.Data.Result.Message;

                }
                return result;

            }
            catch (Exception ex)
            {

                return result;
            }
        }

        #endregion

        #region Health

        public async Task<List<CalculationInstallmentsModel>> HealthCalculation(HealthCalculationInput healthCalculationInput, string jwt, HttpClient client)
        {

            string healthCalculationMutation = Mutations.HealthCalculation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = healthCalculationMutation;
                request.Variables = new { healthCalculationInput };

                var response = await this._client.SendMutationAsync<HealthCalculationPayload>(request);

                if (response.Errors != null)
                {
                    return new List<CalculationInstallmentsModel>();
                }

                return response.Data.Result.Offers;

            }
            catch (Exception ex)
            {

                return new List<CalculationInstallmentsModel>();
            }
        }

        public async Task<PolicyResultModel> HealthPolicy(HealthPolicyInput healthPolicyInput, string jwt, HttpClient client)
        {
            var result = new PolicyResultModel();
            string healthPolicyMutation = Mutations.HealthPolicy;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = healthPolicyMutation;
                request.Variables = new { healthPolicyInput };

                var response = await this._client.SendMutationAsync<HealthPolicyPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                if (response.Data.Result != null)
                {
                    result.PolicyNumber = response.Data.Result.PolicyNo;
                    result.ErrorCode = response.Data.Result.ErrorId;
                    result.Message = response.Data.Result.Message;
                }
                return result;

            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<BaseResultModel> HealthInsuranceInstallment(HealthInsuranceInstallmentInput healthInsuranceInstallmentInput, string jwt, HttpClient client)
        {

            string healthInstallmentMutation = Mutations.HealthInstallment;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = healthInstallmentMutation;
                request.Variables = new { healthInsuranceInstallmentInput };

                var response = await this._client.SendMutationAsync<HealthInsuranceInstallmentPayload>(request);

                if (response.Errors != null)
                {
                    return new BaseResultModel();
                }

                return response.Data.Result;

            }
            catch (Exception ex)
            {

                return new BaseResultModel();
            }
        }

        #endregion

        #region MyThings

        public async Task<List<CalculationModel>> MyThingsCalculation(MyThingsCalculationInput myThingsCalculationInput, string jwt, HttpClient client)
        {

            string myThingsCalculationMutation = Mutations.MyThingsCalculation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = myThingsCalculationMutation;
                request.Variables = new { myThingsCalculationInput };

                var response = await this._client.SendMutationAsync<MyThingsCalculationPayload>(request);

                if (response.Errors != null)
                {
                    return new List<CalculationModel>();
                }

                return response.Data.Result.Offers;

            }
            catch (Exception ex)
            {

                return new List<CalculationModel>();
            }
        }

        public async Task<PolicyResultModel> MyThingsPolicy(MyThingsPolicyInput myThingsPolicyInput, string jwt, HttpClient client)
        {
            var result = new PolicyResultModel();
            string myThingsPolicyMutation = Mutations.MyThingsPolicy;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = myThingsPolicyMutation;
                request.Variables = new { myThingsPolicyInput };

                var response = await this._client.SendMutationAsync<MyThingsPolicyPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                if (response.Data.Result != null)
                {
                    result.PolicyNumber = response.Data.Result.PolicyNo;
                    result.ErrorCode = response.Data.Result.ErrorId;
                    result.Message = response.Data.Result.Message;
                }
                return result;

            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<List<QuestionModel>> GetMyThingsInsuranceQuestions(string myThingsInsuranceId, string jwt, HttpClient client)
        {
            string query = Queries.GetMyThingsInsuranceQuestions;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);

            try
            {
                var request = new GraphQLRequest();
                request.Query = query;
                request.Variables = new { myThingsInsuranceId };

                var response = await this._client.SendMutationAsync<GetQuestionsResultModel>(request);

                if (response.Errors != null)
                {
                    return new List<QuestionModel>();
                }

                return response.Data.Questions;
            }
            catch (Exception ex)
            {

                return new List<QuestionModel>();
            }
        }

        #endregion

        #region Casco

        public async Task<BaseResultModel> SendEmailCasco(CascoRequestEmailInput input, HttpClient client, string jwt)
        {
            var sendEmailCascoInput = input;
            var result = new BaseResultModel();

            string sendEmailCascoMutation = Mutations.SendEmailCascoMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };


            //for test
            //// Bypass SSL certificate validation (only for testing purposes)
            //var handler = new HttpClientHandler
            //{
            //    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            //};
            //this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), new HttpClient(handler));


            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest
                {
                    Query = sendEmailCascoMutation,
                    Variables = new { sendEmailCascoInput }
                };

                var response = await this._client.SendMutationAsync<SendEmailCascoRequestPayload>(request);

                if (response.Errors != null)
                {
                    return new BaseResultModel();
                }

                return response.Data.Result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        #endregion

        #region Common

        public async Task<List<InsuranceModel>> UserInsurances(UserInsurancesInput userInsurancesInput, string jwt, HttpClient client)
        {

            string userInsurancesMutation = Mutations.UserInsurancesMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = userInsurancesMutation;
                request.Variables = new { userInsurancesInput };

                var response = await this._client.SendMutationAsync<UserInsurancesPayload>(request);

                if (response.Errors != null)
                {
                    return new List<InsuranceModel>();
                }

                return response.Data.UserInsurances.Insurances;

            }
            catch (Exception ex)
            {

                return new List<InsuranceModel>();
            }
        }
        public async Task<Stream> GetPolicyPdf(string userId, string insuranceId, string policyNo, string jwt, HttpClient client)
        {
            var request = new GetPolicyRequestModel();
            request.UserId = userId;
            request.InsuranceId = insuranceId;
            request.PolicyNo = policyNo;

            var url = _apiUrl + GlobalConstants.Policy;

            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, GlobalConstants.JsonType);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<CheckVehicleCivilInsuranceAllowedResultModel> CheckVehicleCivilInsuranceAllowed(string vehiclePlateNumber, string jwt, HttpClient client)
        {
            var result = new CheckVehicleCivilInsuranceAllowedResultModel();

            string mutation = Mutations.CheckVehicleCivilInsuranceAllowed;
            var checkVehicleCivilInsuranceAllowedInput = new CheckVehicleCivilInsuranceAllowedInput
            {
                VehiclePlateNumber = vehiclePlateNumber
            };

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = mutation;
                request.Variables = new { checkVehicleCivilInsuranceAllowedInput };

                var response = await this._client.SendMutationAsync<CheckVehicleCivilInsuranceAllowedPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Result;

            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<Stream> GetReceiptPdf(string userId, string insuranceId, string policyNo, string jwt, HttpClient client)
        {
            var request = new GetPolicyRequestModel();
            request.UserId = userId;
            request.InsuranceId = insuranceId;
            request.PolicyNo = policyNo;

            var url = _apiUrl + GlobalConstants.GetReceipt;

            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, GlobalConstants.JsonType);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.GeneralError);
            }

        }

        #endregion
    }
}
