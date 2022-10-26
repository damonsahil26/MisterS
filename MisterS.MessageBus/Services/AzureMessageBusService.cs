using Azure.Messaging.ServiceBus;
using MisterS.MessageBus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisterS.MessageBus.Services
{
    public class AzureMessageBusService : IMessageBusService
    {
        private string _connectionString = "Endpoint=sb://misters.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=nmYETFIZuwLZv2YbHhrP+PBp00HxbvteSL49DfBxsY8=";
        public async Task PublishMessage(BaseMessage baseMessage, string topicName)
        {
            await using var client = new ServiceBusClient(_connectionString);

            ServiceBusSender sender = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(baseMessage);

            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);

            await client.DisposeAsync();
        }
    }
}
