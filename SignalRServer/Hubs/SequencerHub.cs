using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SequencR.Shared;

namespace SequencR.Server.Hubs
{
    public class SequencerHub : Hub
    {
        public SequencerHub(ILogger<SequencerHub> logger, 
            IWebHostEnvironment hostingEnvironment)
        {
            Logger = logger;
            HostingEnvironment = hostingEnvironment;
            Sequence = new Sequence();
        }

        public int BPM { get; set; } = 120;
        public ILogger<SequencerHub> Logger { get; }
        public IWebHostEnvironment HostingEnvironment { get; }
        public Sequence Sequence { get; set; }

        public async Task Init()
        {
            // todo: eventually support sound packs, like sub-folders here
            var webrootpath = HostingEnvironment.ContentRootPath;
            var root = Path.Combine(webrootpath, "Media", "909");
            var directory = new DirectoryInfo(root);
            var files = directory.GetFiles().Select(x => x.Name).ToArray();
            Array.Sort(files, StringComparer.InvariantCulture);

            await Clients.Caller.SendAsync("SoundsObtained", files);
        }

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

        public async Task ChangeBpm(int bpm)
        {
            Logger.LogInformation($"Changing BPM to {bpm}");
            await Clients.All.SendAsync("BpmSet", bpm);
            Logger.LogInformation($"Changed BPM to {bpm}");
        }
    }
}