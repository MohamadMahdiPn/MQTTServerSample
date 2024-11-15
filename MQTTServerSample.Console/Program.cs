// See https://aka.ms/new-console-template for more information

using System.Text;
using MQTTnet.Server;
using MQTTnet;

Console.WriteLine("Hello, World!");
Console.WriteLine("Starting MQTT server...");

// Create MQTT server
var mqttFactory = new MqttFactory();
var mqttServerOptions = new MqttServerOptionsBuilder()
    .WithDefaultEndpoint() // Default endpoint
    .WithDefaultEndpointPort(1883) // Port 1883
    .Build();

var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions);

// Log when a client connects or disconnects
mqttServer.ClientConnectedAsync += async e =>
{
    Console.WriteLine($"Client connected: {e.ClientId}");
};
mqttServer.ClientDisconnectedAsync += async e =>
{
    Console.WriteLine($"Client disconnected: {e.ClientId}");
};

// Log incoming messages
mqttServer.InterceptingPublishAsync += async context =>
{
    var topic = context.ApplicationMessage.Topic;
    var payload = Encoding.UTF8.GetString(context.ApplicationMessage.Payload ?? Array.Empty<byte>());
    Console.WriteLine($"Received message on topic '{topic}': {payload}");
};

// Start the MQTT server
await mqttServer.StartAsync();
Console.WriteLine("MQTT server started successfully on port 1883.");

// Keep the server running
Console.WriteLine("Press Enter to stop the server...");
Console.ReadLine();

// Stop the server when done
await mqttServer.StopAsync();
Console.WriteLine("MQTT server stopped.");