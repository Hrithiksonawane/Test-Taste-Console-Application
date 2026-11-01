using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Test_Taste_Console_Application.Constants;

namespace Test_Taste_Console_Application.Domain.Services
{
    ///<summary>
    /// Configures and provides a shared <see cref="HttpClient"/> instance for accessing the Solar System OpenData API.
    /// The API key is read from appsettings.json (ApiSettings:ApiKey).
    ///</summary>
    public class HttpClientService
    {
        public HttpClient Client { get; }

        public HttpClientService(HttpClient client, IConfiguration configuration)
        {
            Client = client;
            Client.BaseAddress = new Uri(UriPath.BaseUri);
            Client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue(HttpClientSettings.JsonType));

            var apiKey = configuration["ApiSettings:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "YOUR_API_KEY_HERE")
            {
                Console.WriteLine("Error: No API key configured. Please copy appsettings.example.json to appsettings.json and add your API key.");
                Console.WriteLine("Get a free key at: https://api.le-systeme-solaire.net/en/generatekey.html");
            }
            else
            {
                Client.DefaultRequestHeaders.Authorization = new
                    AuthenticationHeaderValue("Bearer", apiKey);
            }
        }
    }
}
