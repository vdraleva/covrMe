using CovrMe.Services.Contracts;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CovrMe.Services.Implementation
{
    public class OcrService : IOcrService
    {
        private readonly IConfiguration _config;
        public OcrService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> RecognizeImage(byte[] image)
        {
            var apiUrl1 = "https://api.ocr.space/parse/image";
            var apiUrl2 = "https://api.ocr.space/parse/image";

            using var client = new HttpClient();
            var apikey = _config.GetSection("Ocr:ApiKey").Value;
            client.DefaultRequestHeaders.Add("apikey", apikey);

            var formData = new MultipartFormDataContent
                {
                    { new StringContent("2"), "ocrengine" },
                    { new StringContent("true"), "scale" },
                    { new StringContent("eng"), "language" },
                    { new StringContent("true"), "istable" },
                    { new StringContent("true"), "detectOrientation" },
                    { new ByteArrayContent(image), "file", "image.jpg"}
                };

            var response = new HttpResponseMessage();
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(12)))
            {
                try
                {
                    response = await client.PostAsync(apiUrl1, formData, cts.Token);

                    return await HandleResponse(response);
                }
                catch (Exception)
                {
                    try
                    {
                        response = await client.PostAsync(apiUrl2, formData, cts.Token);

                        return await HandleResponse(response);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public async Task<string> HandleResponse(HttpResponseMessage response)
        {
            string noText = "No text found";
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var ocrResult = JsonSerializer.Deserialize<OcrApiResponse>(jsonResponse);
                var rawText = ocrResult?.ParsedResults?.FirstOrDefault()?.ParsedText ?? noText;

                return rawText;
            }
            else
            {
                return noText;
            }
        }

        public class OcrApiResponse
        {
            public ParsedResult[] ParsedResults { get; set; }
        }

        public class ParsedResult
        {
            public string ParsedText { get; set; }
        }
    }
}
