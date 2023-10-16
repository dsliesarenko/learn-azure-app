using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "";
//const string queueName = "az-course-queue-1";
const string topicName = "az-course-topic";
const int maxNumberOfMessages = 100;

ServiceBusClient client;
ServiceBusSender sender;

client = new ServiceBusClient(serviceBusConnectionString);
sender = client.CreateSender(topicName);

using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= maxNumberOfMessages; i++)
{
    if (!batch.TryAddMessage(new ServiceBusMessage($"This is a message - {i}")))
    {
        Console.WriteLine($"Message - {i} was not added to the batch");
    }
}

try
{
    await sender.SendMessagesAsync(batch);
    Console.WriteLine("Messages Sent");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}