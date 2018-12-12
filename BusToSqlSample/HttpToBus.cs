using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace BusToSqlSample
{
    public static class HttpToBus
    {
        [FunctionName("HttpToBus")]        
        public static void Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, 
            TraceWriter log,
            [ServiceBus("ionmessages", Connection = "sbConnection", EntityType = Microsoft.Azure.WebJobs.ServiceBus.EntityType.Queue)]
            out string queueMessages)
        {
            log.Info("C# HTTP trigger function processed a request.");

            MyMessage m = new MyMessage()
            {
                FirstName = "Ion",
                LastName = DateTime.Now.ToShortDateString(),
                Id = DateTime.Now.Minute
            };
            
            string body = JsonConvert.SerializeObject(m);

            log.Info($"Message Body: {body}");

            queueMessages = body;

            //return req.CreateResponse(HttpStatusCode.OK, "Message Sent");
        }
    }
}
