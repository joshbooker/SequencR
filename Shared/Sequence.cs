using System.Collections.Generic;

namespace SequencR.Shared
{
    public class Sequence
    {
        public Sequence()
        {
            this.Steps = new List<Step>();
            this.Samples = new List<string>();

            for (int i = 1; i < 9; i++)
            {
                Steps.Add(new Step(i));
            }
        }

        public List<Step> Steps { get; set; }
        public List<string> Samples { get; set; }
    }
}