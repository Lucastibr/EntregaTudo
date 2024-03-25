using EntregaTudo.Core.Repository;
using EntregaTudo.Dal.Context;
using EntregaTudo.Dal.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntregaTudo.Dal.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EntregaTudoDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),x => x.MigrationsAssembly("EntregaTudo.Api")));

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}