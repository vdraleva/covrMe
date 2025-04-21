using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using CovrMe.Models.Vehicles.Result.QueryResults;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Locations.Result.QueryResults;

namespace CovrMe.Services.Implementation
{
    public class LocationService : ILocationService
    {
        private string _url;
        private GraphQLHttpClient _client;
        public LocationService(string baseUrl)
        {
            this._url = baseUrl + GlobalConstants.GraphQlUrl;
        }
        public async Task<GetRegionsByCountryResultModel> GetRegionsByCountry(string countryId, HttpClient client, string jwt)
        {
            var result = new GetRegionsByCountryResultModel();

            string query = Queries.GetRegions;

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
                request.Variables = new { countryId };

                var response = await this._client.SendMutationAsync<GetRegionsByCountryResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Regions;
            }
            catch (Exception ex)
            {

                return result;
            }
        }
        public async Task<GetCountriesResultModel> GetCountries(HttpClient client, string jwt)
        {
            var result = new GetCountriesResultModel();

            string query = Queries.GetCountries;

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

                var response = await this._client.SendMutationAsync<GetCountriesResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Countries;
            }
            catch (Exception ex)
            {

                return result;
            }
        }
        public async Task<GetMunicipalityResultModel> GetMunicipalityByRegionId(string regionId, HttpClient client, string jwt)
        {
            var result = new GetMunicipalityResultModel();

            string query = Queries.GetMunicipality;

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
                request.Variables = new { regionId };

                var response = await this._client.SendMutationAsync<GetMunicipalityResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Municipality;
            }
            catch (Exception ex)
            {

                return result;
            }
        }
        public async Task<GetCityResultModel> GetCityByMunicipalityId(string municipalityId, HttpClient client, string jwt)
        {
            var result = new GetCityResultModel();

            string query = Queries.GetCities;

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
                request.Variables = new { municipalityId };

                var response = await this._client.SendMutationAsync<GetCityResultQuery>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.Cities;
            }
            catch (Exception ex)
            {

                return result;
            }
        }
    }
}
