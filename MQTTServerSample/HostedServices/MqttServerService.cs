using System.Text;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
public class MqttBackgroundService : BackgroundService
{
    private readonly ILogger<MqttBackgroundService> _logger;

    public MqttBackgroundService(ILogger<MqttBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting MQTT server...");

        var mqttFactory = new MqttFactory();

        var mqttServerOptions = new MqttServerOptionsBuilder()
            .WithDefaultEndpointPort(1883) // Default MQTT port
            .WithConnectionValidator(context =>
            {
                if (context.Username != "admin" || context.Password != "password")
                {
                    context.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
                    _logger.LogWarning($"Connection rejected for client: {context.ClientId}");
                    return;
                }

                _logger.LogInformation($"Client connected: {context.ClientId}");
            })
            .WithApplicationMessageInterceptor(context =>
            {
                var payload = Encoding.UTF8.GetString(context.ApplicationMessage.Payload ?? Array.Empty<byte>());
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