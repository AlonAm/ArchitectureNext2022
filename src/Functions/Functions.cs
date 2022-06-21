using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureNext
{
    public class Functions
    {
        [FunctionName("ProccessDeviceMessages")]
        public async Task Run(
            [EventHubTrigger("device-telemetry", Connection = "EventHub")] EventData[] events,
            [SignalR(HubName = "signalrhub", ConnectionStringSetting = "SignalR")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            foreach (EventData eventData in events)
            {
                string json = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                var signalRMessage = new SignalRMessage { Target = "deviceTelemetry", Arguments = new[] { json } };

                await signalRMessages.AddAsync(signalRMessage);
            }
        }
    }
}
