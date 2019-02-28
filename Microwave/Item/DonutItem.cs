using System;
using System.Windows;
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
            var result = rand.Next(100) > 80 ? 1 : 0;

            switch (result)
            {
                case 0 :
                    ClipSetup(ClipTimings.Donut.Done, ClipTimings.Donut.IdleClosed);
                    break;
                case 1 :
                    Application.Current.Dispatcher.Invoke(()=>
                    {
                        MainWindow.Main.EasterEgg.Pause();
                        MainWindow.Main.EasterEggPlaying = false;
                    });
                    ClipSetup(ClipTimings.Donut.Radioactive, ClipTimings.Donut.IdleClosed);
                    break;
            }
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