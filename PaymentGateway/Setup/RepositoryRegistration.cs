using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Repo;
using Repository.Services;

namespace PaymentGateway.Setup
{
    public static class RepositoryRegistration
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddSingleton<IPaymentGatewayRepo, PaymentGatewayRepo>();
            services.AddSingleton<IUniqueIdGenerator, UniqueIdGenerator>();
        }
    }
}
