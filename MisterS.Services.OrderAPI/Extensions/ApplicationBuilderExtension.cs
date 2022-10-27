using MisterS.Services.OrderAPI.Services.Messaging;

namespace MisterS.Services.OrderAPI.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IServiceBusConsumer serviceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            serviceBusConsumer = app.ApplicationServices.GetService<IServiceBusConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStartUp);
            hostApplicationLife.ApplicationStopped.Register(OnStop);
            return app;
        }

        private static void OnStop()
        {
            serviceBusConsumer.Stop();
        }

        private static void OnStartUp()
        {
            serviceBusConsumer.Start();
        }
    }
}
