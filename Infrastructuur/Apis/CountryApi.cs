using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Apis
{
    public sealed class CountryApi
    {
        // The instance field is used to hold the single instance of the class
        private static CountryApi? instance;
        private readonly ILogger _logger;


        // The constructor is private so that no other class can instantiate the object
        public CountryApi()
        {

        }
        public CountryApi(ILogger logger)
        {
            _logger = logger;
        }

        public static CountryApi Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CountryApi();
                }

                return instance;
            }
        }
        public async  Task<List<Country>> GetCountriesAsync()
        {
            string file = "countries.json";
            if (File.Exists(file))
            {
                string jsonToRead = File.ReadAllText(file);
                return JsonConvert.DeserializeObject<List<Country>>(jsonToRead);
            }
            // Create an HTTP client to send a request to the REST countries API
            try
            {
                HttpClient client = new HttpClient();

                // Send a GET request to the API to get a list of all the countries
                HttpResponseMessage response = await client.GetAsync("https://restcountries.com/v2/all");

                // Read the response as a string
                string responseString = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON string into a list of Country objects
                List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(responseString);

                // Serialize the list of countries into a JSON string
                string json = JsonConvert.SerializeObject(countries, Formatting.Indented);

                // Write the JSON string to a file

                File.WriteAllText("countries.json", json);
                return countries;
            } catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
          
          return null;
        }
    }
    // Define a class to represent a country
    public class Country
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
