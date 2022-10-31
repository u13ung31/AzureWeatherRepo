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
    public static class HttpTriggerGetList
    {
        [FunctionName("HttpTriggerGetList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Sql("SELECT * FROM [dbo].[FavoriteWeathers] WHERE UserId = @ID",
            CommandType = System.Data.CommandType.Text,
            Parameters = "@ID={Query.UserID}",
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Object> result,
            ILogger log)
        {
            log.LogInformation("Result: Got List");
            return new OkObjectResult(result);
        }
    }
}
