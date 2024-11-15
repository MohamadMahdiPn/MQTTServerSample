using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MQTTServerSample.Domain.Entities.IdentityModels;
using MQTTServerSample.Domain.Entities.Sensors;

namespace MQTTServerSample.Persistence.Data;

public class MQTTServerSampleDbContext(DbContextOptions<MQTTServerSampleDbContext> options) 
    :IdentityDbContext<ApplicationUser,ApplicationRole,string>(options)
{
    public virtual DbSet<Sensor> Sensors { get; set; }
    public virtual DbSet<SensorMessage> SensorMessages { get; set; }

}