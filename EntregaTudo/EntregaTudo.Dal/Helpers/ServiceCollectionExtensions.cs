using EntregaTudo.Dal.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EntregaTudo.Dal.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EntregaTudoDbContext>(options => 
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));

        return services;
    }
}