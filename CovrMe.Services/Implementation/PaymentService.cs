using CovrMe.Models;
using CovrMe.Models.Payments.Request;
using CovrMe.Models.Payments.Result;
using CovrMe.Models.Payments.Result.Payloads;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Reactive.Linq;

namespace CovrMe.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private string _url;
        private GraphQLHttpClient _client;

        public PaymentService(string baseUrl)
        {
            this._url = baseUrl + GlobalConstants.GraphQlUrl;
        }

        public async Task<CreatePaymentResultModel> Payment(CreatePaymentInput createPaymentInput, string jwt, HttpClient client)
        {
            var result = new CreatePaymentResultModel();

            string createPaymentMutation = Mutations.CivilInsurancePayment;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = createPaymentMutation;
                request.Variables = new { createPaymentInput };

                var response = await this._client.SendMutationAsync<CreatePaymentPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.PaymentInfo;
            }
            catch (Exception ex)
            {

                return result;
            }
        }

        public async Task CheckPaymentStatusSubscription(string localOrderId, string jwt, HttpClient client)
        {
            var result = new BaseResultModel();
            IDisposable currentSubscription = null;
            TaskCompletionSource<bool> subscriptionCompleted = new TaskCompletionSource<bool>();

            string paymentStatustSubscription = Subscriptions.PaymentStatus;

            localOrderId = "1111";
            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {

                var request = new GraphQLRequest();
                request.Query = paymentStatustSubscription;
                request.Variables = new { localOrderId };

                var subscriptionStream = _client.CreateSubscriptionStream<PaymentStatusPayload>(request);

                currentSubscription = subscriptionStream.Subscribe(response =>
                {
                    if (response.Data != null)
                    {
                        // Обработка на получените данни от сървъра
                        Console.WriteLine($"Received data: {response.Data}");
                        subscriptionCompleted.SetResult(true);
                    }
                }, ex =>
                {
                    // Обработка на грешки
                    Console.WriteLine($"Error: {ex.Message}");
                    subscriptionCompleted.SetResult(false);
                }, () =>
                {
                    // Приключване на Subscription
                    Console.WriteLine("Subscription completed.");

                    subscriptionCompleted.SetResult(true);
                });

                await subscriptionCompleted.Task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting subscription: {ex.Message}");
            }
            finally
            {
                currentSubscription?.Dispose();
            }
        }

        public async Task<CheckPaymentStatusResultModel> CheckPaymentStatus(string localOrderId, string jwt, HttpClient client)
        {
            var result = new CheckPaymentStatusResultModel();

            var checkPaymentStatusInput = new CheckPaymentStatusInput();
            checkPaymentStatusInput.LocalOrderId = localOrderId;

            string checkPaymentStatusMutation = Mutations.CheckPaymentStatus;

            var graphQLOptions = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_url)
            };
            this._client = new GraphQLHttpClient(graphQLOptions, new NewtonsoftJsonSerializer(), client);
            this._client.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            try
            {
                var request = new GraphQLRequest();
                request.Query = checkPaymentStatusMutation;
                request.Variables = new { checkPaymentStatusInput };

                var response = await this._client.SendMutationAsync<CheckPaymentStatusPayload>(request);

                if (response.Errors != null)
                {
                    return result;
                }

                return response.Data.StatusInfo;
            }
            catch (Exception ex)
            {

                return result;
            }
        }
    }
}
