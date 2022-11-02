using System;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker;

namespace AzureTimerTemp
{
    /*
    För att köra lokalt måste du skriva 

    cd \AzureWeatherRepo\AzureTimerTemp\AzureTimerTemp

    sen 

    func start --port 1705
    */
    public class Function1
    { 
        [FunctionName("AzureTimerTemp")] 
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
        [Queue("favoritelist", Connection = "QueueStorageConnection")] ICollector<TemperatureNow> collector,
         ILogger log)
        {
            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            var API = Environment.GetEnvironmentVariable("APIKey");
            dynamic favoriteweather = null;
            string ID = null;

            string queryString = "SELECT Latitude,Longitude,FavoriteWeatherId FROM [dbo].[FavoriteWeathers];";
            using (SqlConnection connection = new SqlConnection(
                       str))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        string Lat = reader[0].ToString();
                        string Lot = reader[1].ToString();
                        ID = reader[2].ToString();
                        //Console.WriteLine("\nLat ="+Lat +"\nLot ="+Lot +"\nID ="+ID+"\n");
                        favoriteweather = await FetchData(Lat, Lot, API);
                        //Console.WriteLine(favoriteweather);
                        //Console.WriteLine("\nID ="+ ID +"\nweather ="+favoriteweather.weather[0].main +"\nweather ="+favoriteweather.main.temp +"\n");
                        
                        TemperatureNow temperatureNow = new TemperatureNow( int.Parse(ID), favoriteweather.weather[0].main.ToString(), decimal.Parse(favoriteweather.main.temp.ToString())); 
                        collector.Add(temperatureNow); 
                        // 6 3
                    }
                }               
            }
            
            
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        public record TemperatureNow(int favoriteWeatherId, string CurrentWeather, decimal Temperature);

        private static async Task<dynamic> FetchData(string LAT,string LON,string APIKey)
        {
            HttpClient client = new HttpClient();
            var tempValue = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={LAT}&lon={LON}&appid={APIKey}");

            var jsonString = tempValue.Content.ReadAsStringAsync().Result;
            
            dynamic myObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            
            return myObject;
        }
    }
}
/*
 {
      "coord":{
         "lon":-0.1278,
         "lat":51.5074
      },
      "weather":[
         {
            "id":803,
            "main":"Clouds",
            "description":"broken clouds",
            "icon":"04d"
        }
      ],
      "base":"stations",
      "main":{
         "temp":291.6,
         "feels_like":291.33,
         "temp_min":290.39,
         "temp_max":292.23,
         "pressure":1014,
         "humidity":70
      },
      "visibility":10000,
      "wind":{},
      "clouds":{},
      "dt":1666961538,
      "sys":{},
      "timezone":3600,
      "id":2643743,
      "name":"London",
      "cod":200
   }
 
 
 */