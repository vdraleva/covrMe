using CovrMe.WebAPI.Services.Notifications.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using CovrMe.WebAPI.Models;
using System.Net;
using CovrMe.WebAPI.Shared.Templates;

namespace CovrMe.WebAPI.Services.Notifications.Implementations
{
    public class NotificationHubService : INotificationService
    {
        private readonly IAmazonSimpleNotificationService _hub;
        private readonly ILogger<NotificationHubService> _logger;
        private readonly AmazonSimpleNotificationServiceClient _client;
        private readonly string _arnAndroid;
        private readonly string _arnIOS;
        private readonly string _topicArnAndroid;
        private readonly string _topicArnIOS;

        public NotificationHubService(IConfiguration configuration, ILogger<NotificationHubService> logger)
        {
            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                "us-east-1:4c1f2163-2dff-4ac0-8911-4c2d9536355a",
                RegionEndpoint.USEast1
            );
            _logger = logger;

            this._arnAndroid = configuration
                 .GetSection("NotificationAWSOptions")
                 .GetSection("AndroidPlatformApplicationArn")
                 .Value;

            this._arnIOS = configuration
                .GetSection("NotificationAWSOptions")
                .GetSection("IOSPlatformApplicationArn")
                .Value;

            this._topicArnAndroid = configuration
                .GetSection("NotificationAWSOptions")
                .GetSection("AndroidTopicArn")
                .Value;

            this._topicArnIOS = configuration
                .GetSection("NotificationAWSOptions")
                .GetSection("IOSTopicArn")
                .Value;

            _client = new AmazonSimpleNotificationServiceClient(credentials, RegionEndpoint.USEast1);
        }
        public async Task<bool> SendNotificationAsync(NotificationMessage notificationMessage)
        {
            var androidPushTemplate = PushTemplates.Generic.AndroidAWS;

            var androidPayload = PrepareNotificationPayload(
                    androidPushTemplate,
                    notificationMessage.TextMessage,
                    notificationMessage.Header);

            var applePushTemplate = PushTemplates.Generic.AppleAWS_Live;

            var applePayload = PrepareNotificationPayload(
                    applePushTemplate,
                    notificationMessage.TextMessage,
                    notificationMessage.Header);

            try
            {

                if (notificationMessage.Users.Count == 0)
                {
                    await SendNotificationsAsync(androidPayload, this._topicArnAndroid);
                    await SendNotificationsAsync(applePayload, this._topicArnIOS);
                }
                else
                {
                    await SendNotificationsAsync(androidPayload, notificationMessage.Users, this._arnAndroid);
                    await SendNotificationsAsync(applePayload, notificationMessage.Users, this._arnIOS);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error sending notification");
                return false;
            }
        }
        string PrepareNotificationPayload(string template, string textMessage, string header)
        {
            template = template.Replace("$(textMessage)", String.IsNullOrEmpty(textMessage) ? String.Empty : textMessage, StringComparison.InvariantCulture);
            template = template.Replace("$(header)", String.IsNullOrEmpty(header) ? String.Empty : header, StringComparison.InvariantCulture);
          
            return template;
        }
        async Task SendNotificationsAsync(string payload, List<string> users, string arn)
        {
            var request = new ListEndpointsByPlatformApplicationRequest();
            request.PlatformApplicationArn = arn;

            var response = new ListEndpointsByPlatformApplicationResponse();

            PublishRequest publishRequest = new PublishRequest();

            try
            {
                do
                {
                    response = await this._client.ListEndpointsByPlatformApplicationAsync(request);

                    foreach (var endPoint in response.Endpoints)
                    {
                        var currentAttributes = endPoint.Attributes;

                        var currentUserId = currentAttributes["CustomUserData"];

                        if (users.Count > 0)
                        {
                            var currentEndpointArn = "";

                            if (users.Contains(currentUserId))
                            {
                                currentEndpointArn = endPoint.EndpointArn;
                            }

                            if (!String.IsNullOrEmpty(currentEndpointArn))
                            {
                                publishRequest.TargetArn = currentEndpointArn;
                                publishRequest.MessageStructure = "json";
                                publishRequest.Message = payload;

                                await this._client.PublishAsync(publishRequest);
                            }
                        }
                    }

                    request.NextToken = response.NextToken;

                } while (!string.IsNullOrEmpty(response.NextToken));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message); ;
            }
        }
        async Task SendNotificationsAsync(string payload, string topicArn)
        {
            try
            {
                var request = new PublishRequest
                {
                    Message = payload,
                    MessageStructure = "json",
                    TopicArn = topicArn
                };

                var response = await _client.PublishAsync(request);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public async Task<bool> RegisterDeviceAsync(string userId, DeviceInformation deviceInstallation)
        {
            bool success = false;
            var appResponse = new CreatePlatformEndpointResponse();

            if (string.IsNullOrWhiteSpace(deviceInstallation.InstallationId) ||
                string.IsNullOrWhiteSpace(deviceInstallation.Platform) ||
                string.IsNullOrWhiteSpace(deviceInstallation.PushChannel))
            {
                return false;
            }
            try
            {
                if (deviceInstallation.Platform == "fcm")
                {
                    var checkExistingEndpoint = await this.DeleteExistingEndpoint(userId, this._arnAndroid, this._topicArnAndroid);

                    if (checkExistingEndpoint)
                    {
                        appResponse = await this.CreatePlatformEndpoint(userId, deviceInstallation, this._arnAndroid);

                        var currentArn = "";
                        if (appResponse != null && !String.IsNullOrEmpty(appResponse.EndpointArn))
                        {
                            currentArn = appResponse.EndpointArn;
                        }

                        var topicResponse = await this.TopicSubscribe(currentArn, this._topicArnAndroid);

                        if (topicResponse != null && !String.IsNullOrEmpty(topicResponse.SubscriptionArn))
                        {
                            success = true;
                        }
                    }
                }
                else
                {
                    var checkExistingEndpoint = await this.DeleteExistingEndpoint(userId, this._arnIOS, this._topicArnIOS);

                    if (checkExistingEndpoint)
                    {
                        appResponse = await this.CreatePlatformEndpoint(userId, deviceInstallation, this._arnIOS);

                        var currentArn = "";
                        if (appResponse != null && !String.IsNullOrEmpty(appResponse.EndpointArn))
                        {
                            currentArn = appResponse.EndpointArn;
                        }

                        var topicResponse = await this.TopicSubscribe(currentArn, this._topicArnIOS);

                        if (topicResponse != null && !String.IsNullOrEmpty(topicResponse.SubscriptionArn))
                        {
                            success = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                success = false;
            }

            //_logger.LogError(success.ToString());
            return success;
        }
        private async Task<SubscribeResponse> TopicSubscribe(string endpointArn, string topicArn)
        {
            var response = new SubscribeResponse();

            if (String.IsNullOrEmpty(endpointArn))
            {
                response = null;
                return response;
            }
            try
            {
                SubscribeRequest subscribeRequest = new SubscribeRequest()
                {
                    Endpoint = endpointArn,
                    TopicArn = topicArn,
                    Protocol = "application"
                };

                response = await _client.SubscribeAsync(subscribeRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = null;
            }

            return response;
        }
        private async Task<CreatePlatformEndpointResponse> CreatePlatformEndpoint(string userId, DeviceInformation deviceInstallation, string arn)
        {

            var createResponse = new CreatePlatformEndpointResponse();
            string token = deviceInstallation.InstallationId;

            try
            {
                var currentAttributes = new Dictionary<string, string>();
                currentAttributes.Add("CustomUserData", userId);
                currentAttributes.Add("Enabled", "true");
                currentAttributes.Add("Token", deviceInstallation.PushChannel);

                var createRequest = new CreatePlatformEndpointRequest()
                {
                    PlatformApplicationArn = arn,
                    Attributes = currentAttributes,
                    Token = deviceInstallation.PushChannel
                };

                createResponse = await _client.CreatePlatformEndpointAsync(createRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                createResponse = null;
            }

            return createResponse;
        }
        private async Task<bool> DeleteExistingEndpoint(string userId, string platformArn, string topicArn)
        {
            bool result = true;

            var request = new ListEndpointsByPlatformApplicationRequest();
            request.PlatformApplicationArn = platformArn;

            int inputUserId = 0;
            bool parsedComming = int.TryParse(userId, out inputUserId);

            var response = new ListEndpointsByPlatformApplicationResponse();

            try
            {
                do
                {
                    response = await this._client.ListEndpointsByPlatformApplicationAsync(request);

                    foreach (var endPoint in response.Endpoints)
                    {
                        var currentAttributes = endPoint.Attributes;

                        var currentUserString = currentAttributes["CustomUserData"];
                        int currentUserId = 0;

                        bool parsed = int.TryParse(currentUserString, out currentUserId);

                        if (parsed && parsedComming)
                        {
                            var currentEndpointArn = "";

                            if (currentUserId == inputUserId)
                            {
                                currentEndpointArn = endPoint.EndpointArn;
                            }

                            if (!String.IsNullOrEmpty(currentEndpointArn))
                            {
                                var unsubscribeResult = await this.UnsubscribeFromTopic(currentEndpointArn, topicArn);

                                var deleteEndpointRequest = new DeleteEndpointRequest();
                                deleteEndpointRequest.EndpointArn = currentEndpointArn;

                                var deleteEndpointResponse = await _client.DeleteEndpointAsync(deleteEndpointRequest);

                                if (deleteEndpointResponse.HttpStatusCode != HttpStatusCode.OK && !unsubscribeResult)
                                {
                                    result = false;
                                }
                            }
                        }
                    }

                    request.NextToken = response.NextToken;

                } while (!string.IsNullOrEmpty(response.NextToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result = false;
            }

            return result;
        }
        private async Task<bool> UnsubscribeFromTopic(string endpoint, string topicArn)
        {
            bool success = false;
            try
            {
                var subscriptions = await this.GetSubscriptionsListAsync(topicArn);
                var currentSubscription = subscriptions.FirstOrDefault(x => x.Endpoint == endpoint);

                if (currentSubscription != null)
                {
                    var response = await this._client.UnsubscribeAsync(currentSubscription.SubscriptionArn);

                    if (response != null)
                    {
                        if (response.HttpStatusCode == HttpStatusCode.OK)
                        {
                            success = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return success;
        }
        public async Task<List<Subscription>> GetSubscriptionsListAsync(string topicArn)
        {
            var request = new ListSubscriptionsByTopicRequest();
            request.TopicArn = topicArn;
            var response = new ListSubscriptionsByTopicResponse();

            var result = new List<Subscription>();
            try
            {
                do
                {
                    response = await this._client.ListSubscriptionsByTopicAsync(request);

                    result.AddRange(response.Subscriptions);

                    request.NextToken = response.NextToken;

                } while (!string.IsNullOrEmpty(response.NextToken));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
    }
}
