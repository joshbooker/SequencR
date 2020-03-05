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
            var webrootpath = HostingEnvironment.WebRootPath;
            var root = Path.Combine(webrootpath, "media", "909");
            var directory = new DirectoryInfo(root);
            var files = directory.GetFiles().Select(x => x.Name).ToArray();
            Array.Sort(files, StringComparer.InvariantCulture);

            await Clients.Caller.SendAsync("SoundsObtained", files);
        }

        public async Task Advance(int currentStep)
        {
            await Clients.All.SendAsync("MovedToStep", currentStep);
        }

        public async Task StartSequencerAsync()
        {
            await Clients.All.SendAsync("Started");
        }

        public async Task StopSequencerAsync()
        {
            await Clients.All.SendAsync("Stopped");
        }

        public async Task ChangeBpm(int bpm)
        {
            await Clients.All.SendAsync("BpmSet", bpm);
        }

        public async Task AddInstrumentToSequence(string sample)
        {
            await Clients.Others.SendAsync("InstrumentAddedToSequence", sample);
        }
    }
}