using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Configuration;
namespace Company.Function
{
    public static class HttpTriggerAddToList
    {


        [FunctionName("HttpTriggerAddToList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic myObject = JsonConvert.DeserializeObject<dynamic>(body);

            var EnvString = Environment.GetEnvironmentVariable("SqlConnectionString");

            try
            {
                using (SqlConnection connection = new SqlConnection(EnvString))
                {
                    String query = "INSERT INTO dbo.FavoriteWeathers (Latitude,Longitude,CityName,UserId) VALUES (@LAT,@LON,@CITY,@ID)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlParameter parameter = new SqlParameter("@LAT", SqlDbType.Decimal);
                        parameter.Scale = 8;
                        parameter.Precision = 12;
                        parameter.Value = myObject.Latitude;
                        command.Parameters.Add(parameter);

                        SqlParameter parameter2 = new SqlParameter("@LON", SqlDbType.Decimal);
                        parameter2.Scale = 8;
                        parameter2.Precision = 12;
                        parameter2.Value = myObject.Longitude;
                        command.Parameters.Add(parameter2);

                        command.Parameters.Add("@CITY", SqlDbType.VarChar).Value = myObject.CityName;
                        
                        Console.WriteLine(myObject.UserId + " Type is = "+myObject.UserId.GetType());

                        command.Parameters.Add("@ID", SqlDbType.Int).Value = myObject.UserId;
                       

                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Check Error
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                }
            }
            catch (System.Exception e)
            {
                return new OkObjectResult(e);
            }
            return new OkObjectResult("{Result:\"Call sucess\"}");
        }
    }
}
