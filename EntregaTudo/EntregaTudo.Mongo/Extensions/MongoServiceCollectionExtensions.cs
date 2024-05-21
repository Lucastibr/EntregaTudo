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
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        var settings = configuration.GetSection("MongoOptions").Get<MongoOptions>();

        services.AddSingleton(x => settings);

        var mongoContext = new MongoContext(settings);

        services.AddSingleton(mongoContext);

        var factory = new MongoCollectionFactory(mongoContext);

        services.AddSingleton(factory);

        return services;

    }
}