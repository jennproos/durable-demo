using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Meijer.Function
{
    public static class FanOutFanInOrchestration
    {
        [FunctionName("FanOutFanInOrchestration")]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            log.LogInformation("Inside Fan Out Fan In Orchestrator");
            
            var parallelTasks = new List<Task<int>>();

            // Get a list of N work items to process in parallel.
            int[] workBatch = await context.CallActivityAsync<int[]>("F1", 5);
            for (int i = 0; i < workBatch.Length; i++)
            {
                Task<int> task = context.CallActivityAsync<int>("F2", workBatch[i]);
                parallelTasks.Add(task);
            }

            var taskResults = await Task.WhenAll(parallelTasks);

            // Aggregate all N outputs and send the result to F3
            int sum = 0;
            foreach(int result in taskResults)
            {
                sum += result;
            }

            return await context.CallActivityAsync<string>("F3", sum);
        }

        [FunctionName("F1")]
        public static int[] F1_CreateWorkBatch([ActivityTrigger] int count, ILogger log)
        {
            log.LogInformation("Inside F1");
            var rand = new Random();

            int[] workBatch = new int[count];
            for (int i = 0; i < count; i ++)
            {
                // Get a random integer from 1 to 100
                workBatch[i] = rand.Next(1, 100);
            }

            return workBatch;
        }

        [FunctionName("F2")]
        public static int F2_MultiplyByTwo([ActivityTrigger] int input, ILogger log)
        {
            log.LogInformation($"Inside F2 with input {input}");
            return input*2;
        }

        [FunctionName("F3")]
        public static string F3_ReportSum([ActivityTrigger] int sum, ILogger log)
        {
            log.LogInformation($"Inside F3 with input {sum}");
            return $"The result of this run is: {sum}!";
        }

        [FunctionName("FanOutFanInOrchestration_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("FanOutFanInOrchestration", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}