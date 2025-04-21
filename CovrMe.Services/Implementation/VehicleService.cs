using CovrMe.Models.Vehicles.Request;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Models.Vehicles.Result.Payloads;
using CovrMe.Models.Vehicles.Result.QueryResults;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace CovrMe.Services.Implementation
{
    public class VehicleService : IVehicleService
    {
        private string _url;
        private GraphQLHttpClient _client;
        public VehicleService(string baseUrl)
        {
            this._url = baseUrl + GlobalConstants.GraphQlUrl;
        }

        public async Task<GetUserVehiclesResultModel> GetUserVehicles(string userId, HttpClient client, string jwt)
        {
            var result = new GetUserVehiclesResultModel();

            string query = Queries.GetUserVehicles;

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

                var response = await this._client.SendMutationAsync<GetUserVehiclesResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Vehicles;
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<GetVehicleUsagesResultModel> GetVehicleUsages(HttpClient client, string jwt)
        {
            var result = new GetVehicleUsagesResultModel();

            string query = Queries.GetVehicleUsages;

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

                var response = await this._client.SendMutationAsync<GetVehicleUsagesResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.VehicleUsages;
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<GetVehicleTypesResultModel> GetVehicleTypes(HttpClient client, string jwt)
        {
            var result = new GetVehicleTypesResultModel();

            string query = Queries.GetVehicleTypes;

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

                var response = await this._client.SendMutationAsync<GetVehicleTypesResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.VehicleTypes;
            }
            catch (Exception ex)
            {

                return result;
            }
        }


        public async Task<string> GetVehicleModels(string brand, HttpClient client, string jwt)
        {
            var result = string.Empty;
            var vehicleModelsInput = new VehicleModelsInput();
            vehicleModelsInput.Brand = brand;

            string query = Mutations.GetVehicleModelsMutation;

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
                request.Variables = new { vehicleModelsInput };

                var response = await this._client.SendMutationAsync<VehicleModelsPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Result.Models;
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<VehicleResultModel> AddVehicle(AddVehicleInput addVehicleInput, HttpClient client, string jwt)
        {
            string addVehicleMutation = Mutations.AddVehicleMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = addVehicleMutation;
                request.Variables = new { addVehicleInput };

                var response = await this._client.SendMutationAsync<AddVehiclePayload>(request);

                if (response.Errors != null)
                {
                    return new VehicleResultModel();
                }

                return response.Data.Result.Vehicle;
            }
            catch (Exception ex)
            {
                return new VehicleResultModel();
            }
        }

        public async Task<VehicleResultModel> EditVehicle(EditVehicleInput editVehicleInput, HttpClient client, string jwt)
        {

            string editVehicleMutation = Mutations.EditVehicleMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = editVehicleMutation;
                request.Variables = new { editVehicleInput };

                var response = await this._client.SendMutationAsync<EditVehiclePayload>(request);

                if (response.Errors != null)
                {
                    return new VehicleResultModel();
                }

                return response.Data.Result.Vehicle;
            }
            catch (Exception ex)
            {

                return new VehicleResultModel();
            }
        }
        public async Task<VehicleResultModel> GetVehicleById(string vehicleId, HttpClient client, string jwt)
        {
            var result = new VehicleResultModel();

            string query = Queries.GetVehicleById;

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
                request.Variables = new { vehicleId };

                var response = await this._client.SendMutationAsync<GetVehicleByIdResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.VehicleByVehicleId.Vehicles.FirstOrDefault();
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task<string> GetVehicleBodyTypes(HttpClient client, string jwt)
        {
            string addVehicleMutation = Mutations.GetVehicleBodyTypesMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = addVehicleMutation;

                var response = await this._client.SendMutationAsync<GetVehicleBodyTypesPayload>(request);

                if (response.Errors != null)
                {
                    return string.Empty;
                }

                return response.Data.BodyTypes;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> GetVehicleColors(HttpClient client, string jwt)
        {
            string addVehicleMutation = Mutations.GetVehicleColorsMutation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = addVehicleMutation;

                var response = await this._client.SendMutationAsync<GetVehicleColorsPayload>(request);

                if (response.Errors != null)
                {
                    return string.Empty;
                }

                return response.Data.Colors;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> GetVehicleEngineTypes(HttpClient client, string jwt)
        {
            string addVehicleMutation = Mutations.GetVehiclesEngineTypes;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = addVehicleMutation;

                var response = await this._client.SendMutationAsync<GetVehicleEngineTypesPayload>(request);

                if (response.Errors != null)
                {
                    return string.Empty;
                }

                return response.Data.EngineTypes;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}
