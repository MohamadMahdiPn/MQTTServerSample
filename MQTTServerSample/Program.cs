
using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.Extensions;
using MQTTnet.Server;
using MQTTServerSample.Extensions;
using MQTTServerSample.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configurations = builder.Configuration;
builder.Services.ConfigurePersistenceServices(configurations);
builder.Services.ConfigureIdentityPolicies();

builder.Services.AddHostedService<MqttBackgroundService>();


//builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.AppendMigrations();
app.ApplyRoles();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
