using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class HtttpTriggerLogin
    {
        [FunctionName("HtttpTriggerLogin")]
         public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Sql("SELECT * FROM [dbo].[Users] WHERE UserName = @UserName AND Password = @Password",
            CommandType = System.Data.CommandType.Text,
            Parameters = "@UserName={Query.UserName},@Password={Query.Password}",
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<User> result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

            log.LogInformation("Result: " +  result);

            return new OkObjectResult(result);
        }
    }
   
}