using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTServerSample.Persistence.Data;

namespace MQTTServerSample.Persistence;

public static class MQTTServerSamplePersistenceRegistration
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Database

        services.AddDbContext<MQTTServerSampleDbContext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("MQTTServerSampleConnectionString"));
        });


        #endregion

        return services;

    }
}