using Microsoft.AspNetCore.Identity;

namespace MQTTServerSample.Domain.Entities.IdentityModels;

public class ApplicationUser:IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;




    public string GetDisplayName() => $"{FirstName} {LastName}";
}