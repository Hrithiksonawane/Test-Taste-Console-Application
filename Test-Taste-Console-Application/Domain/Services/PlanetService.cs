using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Text;
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
    public class PlanetService : IPlanetService
    {
        private readonly HttpClientService _httpClientService;

        public PlanetService(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public IEnumerable<Planet> GetAllPlanets()
        {
            var allPlanetsWithTheirMoons = new Collection<Planet>();

            try
            {
                var response = _httpClientService.Client
                    .GetAsync(UriPath.GetAllPlanetsWithMoonsQueryParameters)
                    .Result;

                if (!response.IsSuccessStatusCode)
                {
                    Logger.Instance.Warn($"{LoggerMessage.GetRequestFailed}{response.StatusCode}");
                    return allPlanetsWithTheirMoons;
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var results = JsonConvert.DeserializeObject<JsonResult<PlanetDto>>(content);

                if (results == null) return allPlanetsWithTheirMoons;

                foreach (var planet in results.Bodies)
                {
                    if (planet.Moons != null)
                    {
                        var newMoonsCollection = new Collection<MoonDto>();
                        foreach (var moon in planet.Moons)
                        {
                            try
                            {
                                var moonResponse = _httpClientService.Client
                                    .GetAsync(UriPath.GetMoonByIdQueryParameters + moon.URLId)
                                    .Result;

                                if (!moonResponse.IsSuccessStatusCode)
                                {
                                    Logger.Instance.Warn($"Failed to fetch moon '{moon.URLId}': {moonResponse.StatusCode}");
                                    continue;
                                }

                                var moonContent = moonResponse.Content.ReadAsStringAsync().Result;
                                var moonDto = JsonConvert.DeserializeObject<MoonDto>(moonContent);

                                if (moonDto != null)
                                {
                                    newMoonsCollection.Add(moonDto);
                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                Logger.Instance.Error($"Network error fetching moon '{moon.URLId}': {ex.Message}");
                                Console.WriteLine($"Warning: Could not fetch data for moon '{moon.URLId}'. Skipping.");
                            }
                            catch (JsonException ex)
                            {
                                Logger.Instance.Error($"Failed to parse moon '{moon.URLId}': {ex.Message}");
                                Console.WriteLine($"Warning: Could not parse data for moon '{moon.URLId}'. Skipping.");
                            }
                        }

                        planet.Moons = newMoonsCollection;
                    }

                    allPlanetsWithTheirMoons.Add(new Planet(planet));
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.Instance.Error($"Network error fetching planets: {ex.Message}");
                Console.WriteLine($"Error: Could not connect to the Solar System API. {ex.Message}");
            }
            catch (JsonException ex)
            {
                Logger.Instance.Error($"Failed to parse planet data: {ex.Message}");
                Console.WriteLine($"Error: Could not parse planet data from the API. {ex.Message}");
            }
            catch (AggregateException ex)
            {
                Logger.Instance.Error($"API request failed: {ex.InnerException?.Message ?? ex.Message}");
                Console.WriteLine($"Error: API request failed. {ex.InnerException?.Message ?? ex.Message}");
            }

            return allPlanetsWithTheirMoons;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }
    }
}
