using Microsoft.Azure.Devices.Client;
using System.Text;
using System.Text.Json;

Console.WriteLine("IoT Device Simulator");

await Task.Delay(5000);

var rnd = new Random();

var azureIotHubConnectionString = "";

Console.WriteLine("Connecting to IoT Hub...");

using var client = DeviceClient.CreateFromConnectionString(azureIotHubConnectionString);

while (true)
{
    var telemetry = new { Key = "Temperature", Value = rnd.Next(100) };

    var json = JsonSerializer.Serialize(telemetry);

    var bytes = Encoding.UTF8.GetBytes(json);

    var message = new Message(bytes)
    {
        To = "device/telemetry",
        MessageSchema = "device/telemetry 1.0",
        ContentType = "application/json",
        ContentEncoding = "utf-8",
        MessageId = Guid.NewGuid().ToString(),
        CreationTimeUtc = DateTime.UtcNow,
    };

    message.Properties.Add("hotpath", "true");
    message.Properties.Add("persist", "true");

    await client.SendEventAsync(message);

    Console.WriteLine($"[{DateTime.Now}] {telemetry.Key}: {telemetry.Value}");

    await Task.Delay(2000);
}
