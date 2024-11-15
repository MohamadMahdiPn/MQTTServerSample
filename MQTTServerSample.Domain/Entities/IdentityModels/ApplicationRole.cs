using Microsoft.AspNetCore.Identity;

namespace MQTTServerSample.Domain.Entities.IdentityModels;

public class ApplicationRole:IdentityRole
{
    public string PersianRoleName { get; set; }
}