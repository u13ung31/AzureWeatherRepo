using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Company.Function
{
    public static class HttpTriggerRemoveFromList
    {
        [FunctionName("HttpTriggerRemoveFromList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Sql("DELETE * FROM [dbo].[FavoriteWeathers] WHERE FavoriteWeatherId = @ID",
            CommandType = System.Data.CommandType.Text,
            Parameters = "@ID={Query.FavoriteWeatherID}",
            ConnectionStringSetting = "SqlConnectionString")] string result,
            ILogger log)
        {
            return new OkObjectResult(result);
        }}
}
