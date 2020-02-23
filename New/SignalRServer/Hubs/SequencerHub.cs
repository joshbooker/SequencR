using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace SequencR.Server.Hubs
{
    public class SequencerHub : Hub
    {
        public SequencerHub(ILogger<SequencerHub> logger)
        {
            Logger = logger;
        }

        public int BPM { get; set; } = 120;
        public ILogger<SequencerHub> Logger { get; }

        public async Task Advance(int currentStep)
        {
            await Clients.All.SendAsync("MovedToStep", currentStep);
            Logger.LogInformation($"MovedToStep: {currentStep}");
        }

        public async Task StartSequencerAsync()
        {
            Logger.LogInformation("Starting");
            await Clients.All.SendAsync("Started");
            Logger.LogInformation("Started");
        }

        public async Task StopSequencerAsync()
        {
            Logger.LogInformation("Stopping");
            await Clients.All.SendAsync("Stopped");
            Logger.LogInformation("Stopped");
        }
    }
}