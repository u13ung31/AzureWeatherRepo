using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class AddQueueToDatabase
    {
        [FunctionName("AddQueueToDatabase")]
         public static void Run(
            [QueueTrigger("favoritelist", Connection = "QueueStorageConnection")] TemperatureNow myQueueItem,
            [Sql("[dbo].[FavoriteWeathers]", ConnectionStringSetting = "SqlConnectionString")] ICollector<FavoriteWeathers> collector,
            ILogger log)
        {
            Console.WriteLine(myQueueItem.favoriteWeatherId.GetType() +" | "+myQueueItem.favoriteWeatherId);
                        Console.WriteLine(myQueueItem.CurrentWeather.GetType()+" | "+myQueueItem.CurrentWeather);
            Console.WriteLine(myQueueItem.Temperature.GetType()+" | "+myQueueItem.Temperature);

             collector.Add(new FavoriteWeathers(myQueueItem.CurrentWeather, myQueueItem.Temperature, myQueueItem.favoriteWeatherId));
        }
    }

    public record TemperatureNow(int favoriteWeatherId, string CurrentWeather, decimal Temperature);

    public class FavoriteWeathers
    {

        public FavoriteWeathers(string currentWeather, decimal temperature, int favoriteWeatherId)
        {
            this.CurrentWeather = currentWeather;
            this.Temperature = temperature;
            this.FavoriteWeatherId = favoriteWeatherId;
        }


        public string CurrentWeather { get; set; }
        public decimal Temperature { get; set; }
        public int FavoriteWeatherId { get; set; }
    }

}
