using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MQTTServerSample.Domain.Entities.IdentityModels;
using MQTTServerSample.Persistence.Data;
using MQTTServerSample.Statics;

namespace MQTTServerSample.Extensions;

public static class StaticExtensions
{
    public static void AppendMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<MQTTServerSampleDbContext>();
        context.Database.Migrate();
    }

    public static async void ApplyRoles(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync(RolesName.SuperAdmin))
            await roleManager.CreateAsync(RoleCreator(RolesName.SuperAdmin, RolesPersianName.SuperAdmin));

      
        #region AdminUserConfig

        var superAdminEmail = "m.p_996@hotmail.com";
        var superAdminPass = "123@asdfA";
        ApplicationUser? user = await userManager.FindByNameAsync("1");

        if (user == null)
        {
            await userManager.CreateAsync(new ApplicationUser
            {
                Email = superAdminEmail,
                UserName = "1",
                FirstName = "Admin",
                LastName = "System",
                EmailConfirmed = true,
            }, superAdminPass);

            user = await userManager.FindByEmailAsync(superAdminEmail);
        }

        await userManager.AddToRoleAsync(user, RolesName.SuperAdmin);

        #endregion
    }
    static ApplicationRole RoleCreator(string name, string persianName)
    {
        return new ApplicationRole()
        {
            Name = name,
            PersianRoleName = persianName
        };
    }
    public static IServiceCollection ConfigureIdentityPolicies(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                //.AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<MQTTServerSampleDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 0;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters = "1234567890";
            options.User.RequireUniqueEmail = false;

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(40);
            options.SlidingExpiration = true;
        });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(RolesName.SuperAdmin, policy =>
            {
                policy.RequireRole(RolesName.SuperAdmin);
            });

          
        });

        return services;
    }
}