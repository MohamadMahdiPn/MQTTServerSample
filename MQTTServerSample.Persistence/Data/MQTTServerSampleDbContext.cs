using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MQTTServerSample.Domain.Entities.IdentityModels;

namespace MQTTServerSample.Persistence.Data;

public class MQTTServerSampleDbContext(DbContextOptions<MQTTServerSampleDbContext> options) 
    :IdentityDbContext<ApplicationUser,ApplicationRole,string>(options)
{
    
}