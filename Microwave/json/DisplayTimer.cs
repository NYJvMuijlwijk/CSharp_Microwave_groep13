using System;
using System.Timers;

namespace Microwave.json
{
    public static class DisplayTimer
    {
        private static string _counter = "";
        private static readonly Timer Timer = new Timer(1000);
        private static DateTime _lastDateTime;
        private static double _timeLeft;

        public static void Add(int time)
        {
            if (int.Parse(_counter + time) >= 10000)
            {
                _counter = "9999";
                return;
            }

            _counter += time;

            UpdateTimer();
        }

        /// <summary>
        ///     Starts the microwave
        /// </summary>
        public static void Start()
        {
            _lastDateTime = DateTime.Now;
            _timeLeft = TotalSeconds();
            Timer.Elapsed += TimerOnElapsed;
            Timer.Start();
            MainWindow.Main.CurrentItem.Cook();
            MainWindow.Main.IsMicrowaving = true;
            // voor onduidelijke reden werkt deze niet
            MainWindow.Main.MicrowaveRunning.PlayLooping();
        }

        /// <summary>
        ///     Periodically checks if the microwave timer has finished and updates the timer.
        ///     Plays done animation when the timer has finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            var elapsedTime = now - _lastDateTime;
            _lastDateTime = now;

            _timeLeft -= elapsedTime.TotalSeconds;

            if (_timeLeft <= 0)
            {
                Stop();
                MainWindow.Main.CurrentItem.Done();
                MainWindow.Main.MicrowaveDone.Play();
                Reset();
                return;
            }

            var seconds = (int) Math.Ceiling(_timeLeft);

            _counter = (seconds / 60).ToString().PadLeft(2, '0') +
                       (seconds % 60).ToString().PadLeft(2, '0');

            UpdateTimer();
        }

        private static void UpdateTimer()
        {
            var display = _counter.PadLeft(4, '0');
            MainWindow.Main.Display = display.Substring(0, 2) + ":" + display.Substring(2, 2);
        }

        private static int TotalSeconds()
        {
            var display = _counter.PadLeft(4, '0');

            return int.Parse(display.Substring(0, 2)) * 60 + int.Parse(display.Substring(2, 2));
        }

        /// <summary>
        ///     Resets the timer on the microwave
        /// </summary>
        public static void Reset()
        {
            _counter = "";
            MainWindow.Main.IsMicrowaving = false;
            MainWindow.Main.Display = "00:00";
        }

        /// <summary>
        ///     Stops the timer
        /// </summary>
        public static void Stop()
        {
            Timer.Stop();
            MainWindow.Main.MicrowaveRunning.Stop();
        }

        /// <summary>
        ///     Adds 1 minute to the microwave timer
        /// </summary>
        public static void AddMinute()
        {
            if (int.Parse(_counter.PadLeft(1, '0')) + 100 >= 10000)
            {
                _counter = "9999";
                return;
            }

            _counter = (int.Parse(_counter.PadLeft(1, '0')) + 100).ToString();
            _timeLeft = TotalSeconds();

            UpdateTimer();
        }
    }
}