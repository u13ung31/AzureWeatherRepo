using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DurableFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            //await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo")
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(SayBool), true));
            outputs.Add(await context.CallActivityAsync<string>(nameof(sayNumber), 0));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }
        [FunctionName(nameof(sayNumber))]
        public static string sayNumber([ActivityTrigger] int number, ILogger log)
        {
            log.LogInformation($"Saying hello to {number}.");
            return $"Hello {number}!";
        }
        [FunctionName(nameof(SayBool))]
        public static string SayBool([ActivityTrigger] bool Bool, ILogger log)
        {
            log.LogInformation($"Saying hello to {Bool}.");
            return $"Hello {Bool}!";
        }

        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Function1", null);


            return starter.CreateCheckStatusResponse(req, instanceId);
        }
        [FunctionName("TimerTrigger")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, TraceWriter log)
        {//this runs for every 5minutes
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("", "")
                });
                //making request to above function by http trigger
                var result = await client.PostAsync("http://localhost:7131/api/Function1_HttpStart", content);
            }
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            log.Info(" ");
            log.Info(" ");


            return;
        }
    }
}