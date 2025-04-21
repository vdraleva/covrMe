using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using CovrMe.Models.Users.Request;
using CovrMe.Models.Users.Result.Payloads;
using CovrMe.Models.Users.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CovrMe.Models.Deliveries.Request;
using CovrMe.Models.Deliveries.Result.Payloads;
using CovrMe.Models.Deliveries.Result;
using CovrMe.Models.Deliveries;

namespace CovrMe.Services.Implementation
{
    public class DeliveryService : IDeliveryService
    {
        private string _url;
        private GraphQLHttpClient _client;
        public DeliveryService(string baseUrl)
        {
            this._url = baseUrl + GlobalConstants.GraphQlUrl;
        }

        public async Task<List<SpeedyOfficeModel>> FindOffice(string city, string neighborhood, string jwt, HttpClient client)
        {
            var findOfficeInput = new FindOfficeInput();

            findOfficeInput.City = city;
            findOfficeInput.Neighborhood = neighborhood;

            var result = new FindOfficePayload();
            var offices = new List<SpeedyOfficeModel>();

            string findOfficeMutation = Mutations.SpeedyFindOffice;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = findOfficeMutation;
                request.Variables = new { findOfficeInput };

                var response = await this._client.SendMutationAsync<FindOfficePayload>(request);

                if (response.Errors != null)
                {
                    return offices;
                }

                return response.Data.SpeedyOffices.Offices;
            }
            catch (Exception ex)
            {

                return offices;
            }
        }

        public async Task<SpeedyPriceModel> Calculation(string postalCode, int officeId, string jwt, HttpClient client)
        {
            var calculationInput = new CalculationInput();

            calculationInput.OfficeId = officeId;
            calculationInput.PostalCode = postalCode;

            var result = new CalculationPayload();
            var price = new SpeedyPriceModel();

            string calculationMutation = Mutations.SpeedyCalculation;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = calculationMutation;
                request.Variables = new { calculationInput };

                var response = await this._client.SendMutationAsync<CalculationPayload>(request);

                if (response.Errors != null)
                {
                    return price;
                }


                var speedyPriceResultModel = response.Data.Calculation.Calculations.FirstOrDefault();

                if(speedyPriceResultModel != null)
                {
                    return speedyPriceResultModel.Price;
                }
                else
                {
                    return price;
                }
                
            }
            catch (Exception ex)
            {

                return price;
            }
        }

        public async Task<string> Shipment(InsuranceDeliveryModel req, string jwt, HttpClient client)
        {
            var shipmentInput = new ShipmentInput();

            shipmentInput.PolicyNo = req.PolicyNo;
            shipmentInput.OfficeId = req.OfficeId;
            shipmentInput.PostalCode = req.PostalCode;
            shipmentInput.Names = req.Name;
            shipmentInput.Phone = req.Phone;
            shipmentInput.Email = req.Email;
            shipmentInput.Street = req.Street;
            shipmentInput.BlockNo = req.BlockNo;
            shipmentInput.Entrance = req.Entrance;
            shipmentInput.Floor = req.Floor;
            shipmentInput.Appartment = req.Appartment;
            shipmentInput.Info = req.Info;

            var result = new ShipmentPayload();

            string shipmentMutation = Mutations.SpeedyShipment;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = shipmentMutation;
                request.Variables = new { shipmentInput };

                var response = await this._client.SendMutationAsync<ShipmentPayload>(request);

                if (response.Errors != null)
                {
                    return string.Empty;
                }

                return response.Data.ShipmentInfo.Id;

            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }
    }
}
