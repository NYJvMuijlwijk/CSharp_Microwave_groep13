using System;

namespace Microwave.json
{
    public abstract class Clip
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}