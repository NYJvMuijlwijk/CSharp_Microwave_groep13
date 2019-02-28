namespace Microwave.json
{
    public abstract class Empty
    {
        public Clip IdleOpen { get; set; }
        public Clip IdleClosed { get; set; }
        public Clip Close { get; set; }
        public Clip Open { get; set; }
        public Clip Cook { get; set; }
        public Clip Done { get; set; }
    }
}