using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.DataTransferObjects;
using Test_Taste_Console_Application.Domain.DataTransferObjects.JsonObjects;
using Test_Taste_Console_Application.Domain.Objects;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application.Domain.Services
{
    /// <inheritdoc />
    public class MoonService : IMoonService
    {
        private readonly HttpClientService _httpClientService;


        public MoonService(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public IEnumerable<Moon> GetAllMoons()
        {
            var allMoons = new Collection<Moon>();

            try
            {
                var response = _httpClientService.Client
                    .GetAsync(UriPath.GetAllMoonsWithMassQueryParameters)
                    .Result;

                if (!response.IsSuccessStatusCode)
                {
                    Logger.Instance.Warn($"{LoggerMessage.GetRequestFailed}{response.StatusCode}");
                    return allMoons;
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var results = JsonConvert.DeserializeObject<JsonResult<MoonDto>>(content);

                if (results == null) return allMoons;

                foreach (MoonDto moonDto in results.Bodies)
                {
                    allMoons.Add(new Moon(moonDto));
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.Instance.Error($"Network error fetching moons: {ex.Message}");
                Console.WriteLine($"Error: Could not connect to the Solar System API. {ex.Message}");
            }
            catch (JsonException ex)
            {
                Logger.Instance.Error($"Failed to parse moon data: {ex.Message}");
                Console.WriteLine($"Error: Could not parse moon data from the API. {ex.Message}");
            }
            catch (AggregateException ex)
            {
                Logger.Instance.Error($"API request failed: {ex.InnerException?.Message ?? ex.Message}");
                Console.WriteLine($"Error: API request failed. {ex.InnerException?.Message ?? ex.Message}");
            }

            return allMoons;
        }
    }
}
