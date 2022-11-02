using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurableFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>(nameof(GetDataFromDatabse), null));

            outputs.Add(await context.CallActivityAsync<string>(nameof(SaveDataToDatabase), "[{FavoriteWeatherId:17, CelsiusTemp:2.22}, {FavoriteWeatherId:18, CelsiusTemp:1.24}]"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(sayNumber), 0));

            return outputs;
        }

        [FunctionName(nameof(GetDataFromDatabse))]
        public static string GetDataFromDatabse([ActivityTrigger] string name, 
        [Sql("SELECT FavoriteWeatherId, Latitude, Longitude FROM [dbo].[FavoriteWeathers]",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<WeatherData> result,
            ILogger log)
        {
            string weatherDataString = JsonConvert.SerializeObject(result);
            log.LogInformation(weatherDataString);
            return weatherDataString;
        }

        public class WeatherData
        {
            public int FavoriteWeatherId { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
        }

        [FunctionName(nameof(SaveDataToDatabase))]
        public static string SaveDataToDatabase([ActivityTrigger] string newWeatherData,
            [Sql("[dbo].[FavoriteWeathers]", ConnectionStringSetting = "SqlConnectionString")] ICollector<NewWeatherData> collector,
            ILogger log)
        {

            List<NewWeatherData> data = JsonConvert.DeserializeObject<List<NewWeatherData>>(newWeatherData);
            log.LogInformation("List: " +data[0].CelsiusTemp);

            foreach (NewWeatherData weatherData in data)
            {
                log.LogInformation("object: " + weatherData.CelsiusTemp);
                                log.LogInformation("objectID: " + weatherData.FavoriteWeatherId);

                collector.Add(weatherData);
            }
            return newWeatherData;
        }

        public class NewWeatherData
        {
            public int FavoriteWeatherId { get; set; }
            public decimal CelsiusTemp { get; set; }
        }

        [FunctionName(nameof(sayNumber))]
        public static string sayNumber([ActivityTrigger] bool Bool, ILogger log)
        {
            log.LogInformation($"Saying hello to {Bool}.");
            return $"Hello {Bool}!";
        }

        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Function1", null);


            return starter.CreateCheckStatusResponse(req, instanceId);
        }
        [FunctionName("TimerTrigger")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, TraceWriter log)
        {//this runs for every 5minutes
        using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://localhost:7071/api/");
                var response = await client.GetAsync("Function1_HttpStart");
            }

            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            log.Info(" ");
            log.Info(" ");


            return;
        }
    }
}