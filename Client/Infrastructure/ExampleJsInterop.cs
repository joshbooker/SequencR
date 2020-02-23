using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace SequencR.Client.Infrastructure
{
    public class ExampleJsInterop : IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<BpmHelper> _objRef;

        public ExampleJsInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<string> SetBpm(int bpm)
        {
            _objRef = DotNetObjectReference.Create(new BpmHelper(bpm));

            return _jsRuntime.InvokeAsync<string>(
                "bpmFunctions.setBpm",
                _objRef);
        }

        public void Dispose()
        {
            _objRef?.Dispose();
        }
    }
}
