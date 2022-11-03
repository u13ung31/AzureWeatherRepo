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
            List<string> stringList = new List<string>();
            IEnumerable<WeatherData> wD = await context.CallActivityAsync<IEnumerable<WeatherData>>(nameof(GetDataFromDatabse), null);
            List<NewWeatherData> SaveData = new List<NewWeatherData>();
            foreach (var item in wD)
            {
                double[] doubArray = new double[2];
                string weatherDataString = JsonConvert.SerializeObject(item);

                doubArray[0] = await context.CallActivityAsync<double>(nameof(MeteoSourceAPI), weatherDataString);
                doubArray[1] = KelvinToCelsius(await context.CallActivityAsync<double>(nameof(OpenweatherAPI), weatherDataString));
                double middleValue = MiddleValue(doubArray);
                SaveData.Add(new NewWeatherData(item.FavoriteWeatherId,(decimal)middleValue));
            }

            await context.CallActivityAsync<string>(nameof(SaveDataToDatabase), SaveData);
            return stringList;
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

            return;
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

        [FunctionName(nameof(GetDataFromDatabse))]
        public static IEnumerable<WeatherData> GetDataFromDatabse([ActivityTrigger] string name, 
        [Sql("SELECT FavoriteWeatherId, Latitude, Longitude FROM [dbo].[FavoriteWeathers]",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<WeatherData> result,
            ILogger log)
        {
            string weatherDataString = JsonConvert.SerializeObject(result);
            return result;
        }

        [FunctionName(nameof(MeteoSourceAPI))]
        public static async Task<double> MeteoSourceAPI(
            [ActivityTrigger] string WeatherObjdataString,
            ILogger log)
        {
            var API = Environment.GetEnvironmentVariable("ApiKeyMeteo");
            WeatherData WeatherObjdata = JsonConvert.DeserializeObject<WeatherData>(WeatherObjdataString);

            HttpClient client = new HttpClient();

            string url = $"https://www.meteosource.com/api/v1/free/point?lat={WeatherObjdata.Latitude.ToString()}&lon={WeatherObjdata.Longitude.ToString()}&key={API}";
            string UpdateUrl = url.Replace(",", ".");
            var tempValue = await client.GetAsync(UpdateUrl);

            var jsonString = tempValue.Content.ReadAsStringAsync().Result;

            dynamic myObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            double temp = myObject.current.temperature;

            return temp;
        }
        [FunctionName(nameof(OpenweatherAPI))]
        public static async Task<double> OpenweatherAPI(
            [ActivityTrigger] string WeatherObjdataString,
            ILogger log)
        {
            WeatherData WeatherObjdata = JsonConvert.DeserializeObject<WeatherData>(WeatherObjdataString);

            var API = Environment.GetEnvironmentVariable("ApiKeyOpenweather");

            HttpClient client = new HttpClient();

            var tempValue = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={WeatherObjdata.Latitude}&lon={WeatherObjdata.Longitude}&appid={API}");

            var jsonString = tempValue.Content.ReadAsStringAsync().Result;

            dynamic myObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            double temp = myObject.main.temp;

            return temp;
        }

        private static double KelvinToCelsius(double Kelvin)
        {
            double cel = Kelvin - 273.15;
            double cel2 = Math.Round((cel) * 100) / 100;

            Console.WriteLine(cel2);
            return cel2;
        }
        private static double MiddleValue(double[] sourceNumbers)
        {
            if (sourceNumbers == null || sourceNumbers.Length == 0)
                throw new System.Exception("Median of empty array not defined.");

            //make sure the list is sorted, but use a new array
            double[] sortedPNumbers = (double[])sourceNumbers.Clone();
            Array.Sort(sortedPNumbers);

            //get the median
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? (double)sortedPNumbers[mid] : ((double)sortedPNumbers[mid] + (double)sortedPNumbers[mid - 1]) / 2;
            Console.WriteLine(median);
            return median;
        }


        [FunctionName(nameof(SaveDataToDatabase))]
        public static void SaveDataToDatabase([ActivityTrigger] string stringList,
            [Sql("[dbo].[FavoriteWeathers]", ConnectionStringSetting = "SqlConnectionString")] ICollector<NewWeatherData> collector,
            ILogger log)
        {

            List<NewWeatherData> dataList = JsonConvert.DeserializeObject<List<NewWeatherData>>(stringList);

            foreach (NewWeatherData weatherData in dataList)
            {
                collector.Add(weatherData);
            }
        }

        public class WeatherData
        {
            public int FavoriteWeatherId { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
        }
        public class NewWeatherData
        {
            public int FavoriteWeatherId { get; set; }
            public decimal CelsiusTemp { get; set; }

            public NewWeatherData(int favoriteWeatherId,decimal celsiusTemp){
                FavoriteWeatherId = favoriteWeatherId;
                CelsiusTemp = celsiusTemp;
            }

        }
    }
}