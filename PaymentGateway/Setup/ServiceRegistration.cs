using Microsoft.Extensions.DependencyInjection;
using Services;

namespace PaymentGateway.Setup
{
    public static class ServiceRegistration
    {
        public static void AddPaymentGatewayServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizeService, AuthorizeService>();
            services.AddTransient<ICaptureService, CaptureService>();
        }
    }
}
