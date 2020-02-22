using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SequencR.Shared;

namespace SequencR.Server.Hubs
{
    public class SequencRHub : Hub
    {
        public int BPM { get; set; } = 120;
        public int CurrentStep { get; set; } = 0;
        public bool IsRunning { get; set; } = false;

        public async Task StartAsync()
        {
            await Clients.All.SendAsync("Started");
            IsRunning = true;
            var delay = 60000/BPM;

            while(IsRunning) {
                CurrentStep = (CurrentStep == 8) ? CurrentStep++ : 1;
                await Clients.All.SendAsync("MovedToStep", CurrentStep);
                await Task.Delay(delay);
            }
        }

        public async Task StopAsync()
        {
            IsRunning = false;
            CurrentStep = 0;
            await Clients.All.SendAsync("Stopped");
        }
    }
}