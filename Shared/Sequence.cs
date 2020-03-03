using System.Collections.Generic;

namespace SequencR.Shared
{
    public class Sequence
    {
        public Sequence()
        {
            this.Steps = new List<Step>();
        }

        public List<Step> Steps { get; set; }
    }
}