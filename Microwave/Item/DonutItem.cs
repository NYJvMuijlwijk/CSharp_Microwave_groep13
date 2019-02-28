using System;
using System.Diagnostics;
using Microwave.controller;
using Microwave.Interfaces;
using Microwave.json;

namespace Microwave.Item
{
    internal class DonutItem : ClipController, IItem
    {
        private readonly Timings ClipTimings;
        private readonly Random rand = new Random();

        public DonutItem()
        {
            ClipTimings = MainWindow.Main.ClipTimings;
        }

        public void Idle(bool open)
        {
            var clip = open ? ClipTimings.Donut.IdleOpen : ClipTimings.Donut.IdleClosed;
            ClipSetup(clip, clip);
        }

        public void Open()
        {
            ClipSetup(ClipTimings.Donut.Open, ClipTimings.Donut.IdleOpen);
        }

        public void Close()
        {
            ClipSetup(ClipTimings.Donut.Close, ClipTimings.Donut.IdleClosed);
        }

        public void Cook()
        {
            ClipSetup(ClipTimings.Donut.Cook, ClipTimings.Donut.Cook);
        }

        public void Done()
        {
            var result = rand.Next(100);

            if (result > 80)
            {
                Debug.WriteLine(result);

                ClipSetup(ClipTimings.Donut.Radioactive, ClipTimings.Donut.IdleClosed);
                return;
            }
                Debug.WriteLine(result);

                ClipSetup(ClipTimings.Donut.Done, ClipTimings.Donut.IdleClosed);
            

            
        }

        public void Add()
        {
            ClipSetup(ClipTimings.Donut.Add, ClipTimings.Donut.IdleOpen);
        }

        public void Remove()
        {
            ClipSetup(ClipTimings.Donut.Remove, ClipTimings.Empty.IdleOpen);
        }
    }
}