using Microwave.Interfaces;
using Microwave.json;

namespace Microwave.Item
{
    public class EmptyItem : ClipController, IItem
    {
        private readonly Timings ClipTimings;

        public EmptyItem()
        {
            ClipTimings = MainWindow.Main.ClipTimings;
        }

        public void Idle(bool open)
        {
            var clip = open ? ClipTimings.Empty.IdleOpen : ClipTimings.Empty.IdleClosed;
            ClipSetup(clip, clip);
        }

        public void Open()
        {
            ClipSetup(ClipTimings.Empty.Open,ClipTimings.Empty.IdleOpen);
        }

        public void Close()
        {
            ClipSetup(ClipTimings.Empty.Close, ClipTimings.Empty.IdleClosed);
        }

        public void Cook()
        {
            ClipSetup(ClipTimings.Empty.Cook, ClipTimings.Empty.Cook);
        }

        public void Done()
        {
            ClipSetup(ClipTimings.Empty.Done, ClipTimings.Empty.IdleClosed);
        }

        public void Add()
        {
        }

        public void Remove()
        {
        }
    }
}
