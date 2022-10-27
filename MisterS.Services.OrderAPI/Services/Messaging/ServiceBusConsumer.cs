using Azure.Messaging.ServiceBus;
using MisterS.Services.OrderAPI.Models;
using MisterS.Services.OrderAPI.Models.DTO;
using MisterS.Services.OrderAPI.Repository;
using MisterS.Services.OrderAPI.Repository.IRepository;
using Newtonsoft.Json;
using System.Text;

namespace MisterS.Services.OrderAPI.Services.Messaging
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly OrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        private readonly string _serviceBusConnectionString;
        private readonly string _serviceBusTopicName;
        private readonly string _serviceBusSubscriptionName;

        private ServiceBusProcessor _checkoutProcessor;

        public ServiceBusConsumer(OrderRepository orderRepository,
            IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;

            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            _serviceBusTopicName = _configuration.GetValue<string>("ServiceBusTopicName");
            _serviceBusSubscriptionName = _configuration.GetValue<string>("ServiceBusSubscriptionName");
            var client = new ServiceBusClient(_serviceBusConnectionString);

            _checkoutProcessor = client.CreateProcessor(_serviceBusTopicName, _serviceBusSubscriptionName);
        }

        public async Task Start()
        {
            _checkoutProcessor.ProcessMessageAsync += OnCheckOutMessageReceived;
            _checkoutProcessor.ProcessErrorAsync += ErrorHandler;
            await _checkoutProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _checkoutProcessor.StopProcessingAsync();
            await _checkoutProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Exception.ToString());
            return Task.CompletedTask;
        }

        public async Task OnCheckOutMessageReceived(ProcessMessageEventArgs eventArgs)
        {
            var message = eventArgs.Message;

            if (message != null)
            {
                var body = Encoding.UTF8.GetString(message.Body);

                var checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

                if (checkoutHeaderDto != null)
                {
                    var orderHeader = new OrderHeader
                    {
                        FirstName = checkoutHeaderDto.FirstName,
                        LastName = checkoutHeaderDto.LastName,
                        PaymentStatus = false,
                        CardNumber = checkoutHeaderDto.CardNumber,
                        CouponCode = checkoutHeaderDto.CouponCode,
                        CVV = checkoutHeaderDto.CVV,
                        DiscountTotal = checkoutHeaderDto.DiscountTotal,
                        Email = checkoutHeaderDto.Email,
                        ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                        OrderDetails = new List<OrderDetails>(),
                        OrderTime = DateTime.UtcNow,
                        OrderTotal = checkoutHeaderDto.OrderTotal,
                        Phone = checkoutHeaderDto.Phone,
                        PickUpDateTime = checkoutHeaderDto.PickUpDateTime,
                        UserId = checkoutHeaderDto.UserId
                    };

                    foreach (var item in checkoutHeaderDto.CartDetails)
                    {
                        var orderDeatils = new OrderDetails
                        {
                            ProductId = item.ProductId,
                            Price = item.Product.Price,
                            ProductName = item.Product.Name,
                            Count = item.Count
                        };

                        orderHeader.CartTotalItems += item.Count;

                        orderHeader.OrderDetails.Add(orderDeatils);
                    }

                    await _orderRepository.AddOrder(orderHeader);
                }

            }
        }
    }
}
