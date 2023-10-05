using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Console.QueueConsumer.Demo.Models;
using Microsoft.Extensions.Configuration;
using System.Text;

internal class Program
{
    static AppSettings _appSettings;

    private static async Task Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false, true)
                        .Build();

        _appSettings = config.GetRequiredSection("AppSettings").Get<AppSettings>()!;

        QueueClient queue = new QueueClient(_appSettings.StorageConnectionString, "attendee-emails");

        if (await queue.ExistsAsync())
        {
            QueueProperties properties = await queue.GetPropertiesAsync();

            for (int i = 0; i < properties.ApproximateMessagesCount; i++)
            {
                var message = await RetrieveNextMessage(queue);
                System.Console.WriteLine($"Received: {message}");
            }

            System.Console.ReadLine();
        }
    }

    static async Task<string> RetrieveNextMessage(QueueClient queue)
    {
        QueueMessage[] retrievedMessage = await queue.ReceiveMessagesAsync(1);

        var data = Convert.FromBase64String(retrievedMessage[0].Body.ToString());
        var theMessage = Encoding.UTF8.GetString(data);

        await queue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);

        return theMessage;
    }
}