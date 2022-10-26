using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class HttpTiggerSignUp
    {
        [FunctionName("HttpTiggerSignUp")]
         public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Sql("[dbo].[Users]", ConnectionStringSetting = "SqlConnectionString")] out User user,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            user = new User();
            user = JsonConvert.DeserializeObject<User>(requestBody);

            return new OkObjectResult(user);
        }
    }

    public class User
    {
        //public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
