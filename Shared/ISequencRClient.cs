using System.Threading.Tasks;

namespace SequencR.Shared
{
    public interface ISequencRClient
    {
        Task Started();
        Task Stopped();
        Task MovedToStep(int step);
        Task SetBpm(int bpm);
        Task BpmChanged(int bpm);
    }
}