using Newtonsoft.Json;
using System.Net;

namespace Academy.DTO
{
    public static class CountryData
    {
        private static List<Country> _countries;
        public static async Task<List<Country>> GetAllCountriesAsync()
        {
            //using var client = new HttpClient();

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };

            using var client = new HttpClient(handler)
            {
                DefaultRequestVersion = HttpVersion.Version11, // Use HTTP/1.1
                Timeout = TimeSpan.FromSeconds(30) // Increase timeout
            };

            var response = await client.GetStringAsync("https://restcountries.com/v3.1/all");

            var countriesFromApi = JsonConvert.DeserializeObject<List<dynamic>>(response);

            return countriesFromApi.Select(c => new Country
            {
                Name = c.name.common.ToString(),
                Code = c.cca2.ToString() // ISO Alpha-2 code
            }).ToList();
        }

        public static async Task InitializeAsync()
        {
            if (_countries == null)
            {
                _countries = await GetAllCountriesAsync();
            }
        }

        public static List<Country> GetCountries()
        {
            return _countries ?? new List<Country>();
        }

    }
}

public class Country
{
    public string Name { get; set; }
    public string Code { get; set; } // Optional: For country codes (e.g., "US", "IN")
}
