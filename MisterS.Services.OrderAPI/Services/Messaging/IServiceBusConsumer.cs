namespace MisterS.Services.OrderAPI.Services.Messaging
{
    public interface IServiceBusConsumer
    {
        public Task Start();
        public Task Stop();
    }
}
