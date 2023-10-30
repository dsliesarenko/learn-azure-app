using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace AzCourse.Function
{
    public class MessageSender
    {
        [FunctionName("MessageSender")]
        public void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var message = $"C# Timer trigger function executed at: {DateTime.Now}";
            HttpClient client = new();
            HttpRequestMessage requestMessage = new(HttpMethod.Post, "http://localhost:7130/api/MessageReceiver");
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            client.Send(requestMessage);

            log.LogInformation(message);
        }
    }
}
