using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTServerSample.Application.Contracts.Redis;
using MQTTServerSample.Application.Contracts.Repositories;
using MQTTServerSample.Application.Contracts.Sensors;
using MQTTServerSample.Application.Contracts.Users;
using MQTTServerSample.Persistence.Data;
using MQTTServerSample.Persistence.Implementations.Repositories;
using MQTTServerSample.Persistence.Implementations.Sensors;
using MQTTServerSample.Persistence.Implementations.Users;
using MQTTServerSample.Persistence.RedisRepository;

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


        #region DI
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<ISensorService, SensorService>();
        services.AddScoped<IRedisService, RedisDataService>();
        #endregion

        return services;

    }
}