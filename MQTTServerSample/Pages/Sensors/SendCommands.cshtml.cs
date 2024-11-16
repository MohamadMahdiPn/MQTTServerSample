using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MQTTnet.Client.Options;
using MQTTnet;

namespace MQTTServerSample.Pages.Sensors
{
    public class SendCommandsModel : PageModel
    {
        [BindProperty]
        public string Command { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Command))
            {
                Message = "No command received.";
                return Page();
            }

            try
            {
                using var cts = new CancellationTokenSource(); // Create a CancellationTokenSource
                var cancellationToken = cts.Token;

                // Configure MQTT client
                var mqttFactory = new MqttFactory();
                var mqttClient = mqttFactory.CreateMqttClient();

                var mqttOptions = new MqttClientOptionsBuilder()
                        .WithTcpServer("192.168.146.127", 1883) // Replace with your broker IP and port
                        .WithCredentials("admin", "password") // Replace with your MQTT username and password
                        .WithClientId("RazorPagePublisher")
                        .WithCleanSession() // Optional: Clean session
                        .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311) // Specify protocol version
                        .Build();

                // Connect to MQTT broker
                await mqttClient.ConnectAsync(mqttOptions, cancellationToken);

                // Publish the command
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("commands/device1") // The topic NodeMCU subscribes to
                    .WithPayload(Command) // The command (e.g., RED_ON, RED_OFF)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                    .Build();

                await mqttClient.PublishAsync(message, cancellationToken);

                // Disconnect after sending
                await mqttClient.DisconnectAsync(new()
                {
             
                    ReasonString = "DC"
                }, cancellationToken);

                Message = $"Command '{Command}' sent successfully!";
            }
            catch (Exception ex)
            {
                Message = $"Error: {ex.Message}";
            }

            return Page();
        }
    }
}
