using MisterS.MessageBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisterS.MessageBus.Services
{
    public interface IMessageBusService
    {
        public Task PublishMessage(BaseMessage baseMessage, string topicName);
    }
}
