using System;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace SequencR.Client.Infrastructure
{
    public class BpmHelper
    {
        public BpmHelper(HubConnection hubConnection)
        {
            HubConnection = hubConnection;
        }

        public HubConnection HubConnection { get; }

        [JSInvokable]
        public string LogBpm(int bpm)
        {
            HubConnection.SendAsync("ChangeBpm", bpm);
            return $"The BPM is now {bpm}!";
        }
    }
}
