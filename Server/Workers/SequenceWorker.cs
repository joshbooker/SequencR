using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SequencR.Workers
{
    public class SequenceWorker : IHostedService, IDisposable
    {
        public SequenceWorker(ILogger<SequenceWorker> logger)
        {
            Logger = logger;

            Task.Run(() => StartHubConnectionAsync());
        }

        public int Delay { get; set; } = (60000/120);
        public int BPM { get; set; } = 120;
        public ILogger<SequenceWorker> Logger { get; }
        HubConnection HubConnection { get; set; }
        public int CurrentStep { get; set; } = 0;
        public Timer Timer { get; set; }

        private async Task StartHubConnectionAsync()
        {
            if(HubConnection == null)
            {
                HubConnection = new HubConnectionBuilder()
                    .WithUrl(new Uri("http://localhost:4000/sequencerHub"))
                    .WithAutomaticReconnect(new TimeSpan[] {
                        TimeSpan.FromSeconds(0),
                        TimeSpan.FromSeconds(0),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(8),
                        TimeSpan.FromSeconds(10),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)
                    })
                    .Build();

                HubConnection.On("Started", async () =>
                {
                    await StartAsync();
                });

                HubConnection.On("Stopped", async () =>
                {
                    await StopAsync();
                });

                HubConnection.On<int>("BpmSet", async (bpm) =>
                {
                    BPM = bpm;
                });

                await HubConnection.StartAsync();
            }
        }

        private void AdjustDelay()
        {
            Delay = 60000 / BPM;
            Timer?.Change(TimeSpan.FromMilliseconds(Delay), TimeSpan.Zero);
        } 

        public void AdvanceAsync(object state)
        {
            AdjustDelay();

            if (CurrentStep == 8)
            {
                CurrentStep = 1;
            }
            else
            {
                CurrentStep = (CurrentStep + 1);
            }

            HubConnection.SendAsync("Advance", CurrentStep);
        }

        public async Task StartAsync(CancellationToken token = default)
        {
            Logger.LogInformation("Starting");
            await StartHubConnectionAsync();
            Timer = new Timer(AdvanceAsync, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(Delay));
            Logger.LogInformation("Started");
        }

        public Task StopAsync(CancellationToken token = default)
        {
            Logger.LogInformation("Stopping");
            CurrentStep = 0;
            Timer?.Change(Timeout.Infinite, 0);
            Logger.LogInformation("Stopped");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}