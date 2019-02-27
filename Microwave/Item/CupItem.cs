using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microwave.Item
{
    public class CupItem : ClipController,IItem
    {
        private readonly Timings ClipTimings;

        public CupItem()
        {
            ClipTimings = MainWindow.Main.ClipTimings;
        }

        public void Idle(bool open)
        {
            var clip = open ? ClipTimings.Cup.IdleOpen : ClipTimings.Cup.IdleClosed;
            ClipSetup(clip, clip);
        }

        public void Open()
        {
            ClipSetup(ClipTimings.Cup.Open, ClipTimings.Cup.IdleOpen);
        }

        public void Close()
        {
            ClipSetup(ClipTimings.Cup.Close, ClipTimings.Cup.IdleClosed);
            }

        public void Cook()
        {
            ClipSetup(ClipTimings.Cup.Cook, ClipTimings.Cup.Cook);
        }

        public void Done()
        {
            ClipSetup(ClipTimings.Cup.Done, ClipTimings.Cup.IdleClosed);
        }

        public void Add()
        {
            ClipSetup(ClipTimings.Cup.Add, ClipTimings.Cup.IdleOpen);
        }

        public void Remove()
        {
            ClipSetup(ClipTimings.Cup.Remove, ClipTimings.Empty.IdleOpen);
        }
    }
}
