using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
using MQTTServerSample.Application.Contracts.Sensors;
using MQTTServerSample.Application.DTOs.Sensors;
using MQTTServerSample.Domain.Enums;
using MQTTServerSample.Extensions;
public class MqttBackgroundService : BackgroundService
{
    private readonly ILogger<MqttBackgroundService> _logger;
    private readonly ISensorService _sensorService;
    private readonly IHubContext<MqttHub> _hubContext;
    public MqttBackgroundService(IServiceScopeFactory serviceProvider,
        ILogger<MqttBackgroundService> logger, IHubContext<MqttHub> hubContext)
    {
        _logger = logger;
        _sensorService = serviceProvider.CreateScope()
            .ServiceProvider.GetRequiredService<ISensorService>();
        _hubContext = hubContext;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting MQTT server...");
        await _sensorService.SaveListToRedis();
        var mqttFactory = new MqttFactory();

        var mqttServerOptions = new MqttServerOptionsBuilder()
            .WithDefaultEndpointPort(1883) // Default MQTT port
            .WithConnectionValidator(async context =>
            {
                if (context.Username != "admin" || context.Password != "password")
                {
                    context.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
                    _logger.LogWarning($"Connection rejected for client: {context.ClientId}");
                    return;
                }
                var checkExists =await _sensorService.CheckExists(context.ClientId);
                if (!checkExists.IsSucceeded)
                {
                    var sensorItem = new SensorDto()
                    {
                        CreatedDate = DateTime.Now,
                        SensorIp = context.Endpoint,
                        SensorName = context.ClientId,
                        //UserId = "24a62487-a32d-4e6a-b9a8-f9dfdd0139b8",
                        UserId = "52b85f78-b520-45b3-872f-89baac1e2e9c",
                        SensorType = SensorType.Temperature
                    };
                    var insert = await _sensorService.AddNew(sensorItem);
                }
                _logger.LogInformation($"Client connected: {context.ClientId}");
            })
            .WithApplicationMessageInterceptor(async context =>
            {
                var payload = Encoding.UTF8.GetString(context.ApplicationMessage.Payload ?? Array.Empty<byte>());
                var getId = await _sensorService.ReadIdFromNameRedis(context.ClientId);
                var sensorMessage = new SensorMessageDto()
                {
                    CreatedDate = DateTime.Now,
                    Payload = context.ApplicationMessage.Topic,
                    SensorId = getId.DataItem.Id,
                    //UserId = "24a62487-a32d-4e6a-b9a8-f9dfdd0139b8",
                    UserId = "52b85f78-b520-45b3-872f-89baac1e2e9c",
                    Value = payload

                };
                var insert = await _sensorService.AddNewMessage(sensorMessage);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", context.ClientId, payload, cancellationToken: stoppingToken);
                _logger.LogInformation($"Received message on topic '{context.ApplicationMessage.Topic}': {payload}");
            })
            .Build();

        var mqttServer = mqttFactory.CreateMqttServer();

        await mqttServer.StartAsync(mqttServerOptions);
        _logger.LogInformation("MQTT server started on port 1883.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        await mqttServer.StopAsync();
        _logger.LogInformation("MQTT server stopped.");
    }

  
    private Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs args)
    {
        var payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
        _logger.LogInformation($"Received message on topic '{args.ApplicationMessage.Topic}': {payload}");

        // Optionally: Parse the JSON payload
        try
        {
            var data = JsonSerializer.Deserialize<TemperatureData>(payload);
            if (data != null)
            {
                _logger.LogInformation($"Parsed temperature: {data.Temperature} °C");
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError($"Failed to parse JSON payload: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}

public record TemperatureData
{
    public float Temperature { get; set; }
}