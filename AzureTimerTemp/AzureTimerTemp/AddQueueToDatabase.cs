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
            Console.WriteLine(myQueueItem.Id.GetType() +" | "+myQueueItem.Id);
                        Console.WriteLine(myQueueItem.CurrentWeather.GetType()+" | "+myQueueItem.CurrentWeather);
            Console.WriteLine(myQueueItem.Temperature.GetType()+" | "+myQueueItem.Temperature);

            collector.Add(new FavoriteWeathers(1, 1, myQueueItem.Temperature));
        }
    }

    public record TemperatureNow(int Id, string CurrentWeather, decimal Temperature);

    public class FavoriteWeathers
    {

        public FavoriteWeathers(int id, int currentWeather, decimal temperature)
        {
            this.FavoriteWeatherId = id;
            this.CurrentWeather = currentWeather;
            this.Temperature = temperature;
            this.UserId = 1;
        }

        
        public int UserId { get; set; }
        public int CurrentWeather { get; set; }
        public decimal Temperature { get; set; }
        public int FavoriteWeatherId { get; set; }
    }

}
