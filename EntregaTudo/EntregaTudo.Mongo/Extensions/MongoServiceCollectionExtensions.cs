using Codout.Framework.Mongo;
using EntregaTudo.Core.Repository;
using EntregaTudo.Mongo.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntregaTudo.Mongo.Extensions;

public static class MongoServiceCollectionExtensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services, ConfigurationManager configuration)
    {
        var context = new MongoContext(configuration.GetSection("MongoOptions").Get<MongoOptions>());
        services.AddSingleton(context);

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        return services;

    }
}