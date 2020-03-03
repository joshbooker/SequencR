
using System.Collections.Generic;

namespace SequencR.Shared
{
    public class Step
    {
        public Step()
        {
            this.Trigs = new List<Trig>();
        }
        
        public int Index { get; set; }
        public List<Trig> Trigs { get; set; }
    }
}