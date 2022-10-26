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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Sql("SELECT * FROM [dbo].[Users] WHERE UserName = @UserName AND Password = @Password",
            CommandType = System.Data.CommandType.Text,
            Parameters = "@UserName={Query.name}, @Password={Query.password}",
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Object> result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

            
            if(result.Count() == 1)
            {
                // Login Succeded 
            }
            else
            {
                // Login failed        
            }

            return new OkObjectResult(result);
        }
    }
}
