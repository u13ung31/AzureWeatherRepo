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
            Parameters = "@UserName={Query.UserName}, @Password={Query.Password}",
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Object> result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

            /*
              
            dynamic myObject = JsonConvert.DeserializeObject<dynamic>(body);
            string UserName = myObject.UserName;
            log.LogInformation(UserName);

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;



            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            */
                
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
