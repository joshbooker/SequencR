using System.Collections.Generic;

namespace SequencR.Shared
{
    public class Sequence
    {
        public Sequence()
        {
            this.Steps = new List<Step>();
            this.AvailableSamples = new List<string>();
            this.SamplesInUse = new List<string>();

            for (int i = 1; i < 9; i++)
            {
                Steps.Add(new Step(i));
            }
        }

        public List<Step> Steps { get; set; }
        public List<string> AvailableSamples { get; set; }
        public List<string> SamplesInUse { get; set; }
    }
}