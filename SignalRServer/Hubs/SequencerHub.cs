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
        static SequencerHub()
        {
            Sequence = new Sequence();
        }

        public SequencerHub(ILogger<SequencerHub> logger, 
            IWebHostEnvironment hostingEnvironment)
        {
            Logger = logger;
            HostingEnvironment = hostingEnvironment;
        }

        public int BPM { get; set; } = 120;
        public ILogger<SequencerHub> Logger { get; }
        public IWebHostEnvironment HostingEnvironment { get; }
        static Sequence Sequence { get; set; }
        static object _lock = new object();

        private string[] GetSamples()
        {
            var webrootpath = HostingEnvironment.WebRootPath;
            var root = Path.Combine(webrootpath, "media", "909");
            var directory = new DirectoryInfo(root);
            var files = directory.GetFiles().Select(x => x.Name).ToArray();
            Array.Sort(files, StringComparer.InvariantCulture);
            return files;
        }

        public async Task Init()
        {
            // get the list of samples
            var files = GetSamples();

            lock(_lock)
            {
                // if the sequence is new, initialize it with the files
                if(Sequence.AvailableSamples.Count == 0 && Sequence.SamplesInUse.Count == 0)
                {
                    foreach (var file in files)
                    {
                        Sequence.AvailableSamples.Add(file);   
                    }
                }
            }

            // send the sequence back to the client
            await Clients.Caller.SendAsync("SequenceReceived", Sequence);
        }

        public async Task AddInstrumentToSequence(string sample)
        {
            lock(_lock)
            {
                Sequence.AvailableSamples.Remove(sample);
                Sequence.SamplesInUse.Add(sample);
                
                Sequence.Steps.ForEach(s => s.Trigs.Add(new Trig 
                { 
                    IsArmed = false,
                    SampleFileName = sample
                }));
            }

            // send the sequence back to the client
            await Clients.All.SendAsync("SequenceReceived", Sequence);
        }

        public async Task ToggleTrigger(string sample, int step, bool isArmed)
        {
            lock(_lock)
            {
                // toggle the trigger for this sample & step
                Sequence
                    .Steps
                    .First(x => x.Index == step)
                        .Trigs
                        .First(x => x.SampleFileName == sample)
                            .IsArmed = isArmed;

            }

            // send the sequence back to the client
            await Clients.All.SendAsync("SequenceReceived", Sequence);
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
    }
}