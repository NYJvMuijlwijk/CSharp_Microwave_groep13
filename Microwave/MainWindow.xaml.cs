using System;
using System.IO;
using System.Media;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Microwave.Interfaces;
using Microwave.Item;
using Microwave.json;
using Newtonsoft.Json;
using static System.Int32;

namespace Microwave
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Main = this;
            DataContext = this;
            Display = "";


            empty = new EmptyItem();
            donut = new DonutItem();
            cup = new CupItem();

            CurrentItem = empty;

            FrameTimer.Elapsed += TimerOnElapsed;

            MediaElement.Play();
            MediaElement.Loaded += (sender, args) =>
            {
                MediaElement.Position = new TimeSpan(0, 0, 0, 0, 150);
                MediaElement.Pause();
            };
        }

        #region Methods

        /// <summary>
        ///     checks if the current position of the MediaElement has passed the end of the CurrentClip.
        ///     if so, it sets the Position to the start of the NextClip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.InvokeAsync(() =>
            {
                if (CurrentClip == ClipTimings.Donut.Radioactive) Display = "";

                if (MediaElement.Position < CurrentClip.End) return;

                if (CurrentClip == ClipTimings.Empty.Startup || CurrentClip == ClipTimings.Donut.Radioactive)
                {
                    Display = "00:00";
                    isOn = true;
                }

                MediaElement.Position = NextClip.Start;
                CurrentClip = NextClip;
            });
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsOpen || CurrentClip == ClipTimings.Empty.Startup || !isOn) return;

            if (!IsMicrowaving && Display != "00:00")
            {
                CurrentItem.Idle(IsOpen);
                DisplayTimer.Start();
            }
            else if (!IsMicrowaving && Display == "00:00")
            {
                DisplayTimer.AddMinute();
                DisplayTimer.Start();
            }
            else if (IsMicrowaving)
            {
                DisplayTimer.AddMinute();
            }

            //microwaveBeep.Play();
        }

        private void CloseDoorButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isOn || !IsOpen) return;

            IsOpen = false;

            CurrentItem.Close();
        }

        private void Open_RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isOn) return;

            if (IsOpen)
            {
                CurrentItem.Remove();
                CurrentItem = empty;
            }
            else
            {
                IsOpen = true;

                DisplayTimer.Stop();

                CurrentItem.Open();
            }
        }

        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NrButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isOn || IsMicrowaving) return;

            microwaveBeep.Play();

            var number = Parse(((Button) sender).Tag.ToString());
            DisplayTimer.Add(number);
        }

        private void StartUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (isOn) return;

            MediaElement.Play();
            CurrentClip = ClipTimings.Empty.Startup;
            NextClip = ClipTimings.Empty.IdleClosed;

            FrameTimer.Start();
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isOn) return;

            if (isOn && !IsMicrowaving && Display != "00:00")
            {
                DisplayTimer.Reset();
            }
            else if (IsMicrowaving)
            {
                IsMicrowaving = false;
                CurrentItem.Idle(IsOpen);
                DisplayTimer.Stop();
            }

            //microwaveBeep.Play();
        }

        private void DonutButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isOn || !IsOpen || CurrentItem != empty) return;

            CurrentItem = donut;

            donut.Add();
        }

        private void CupButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isOn || !IsOpen || CurrentItem != empty) return;

            CurrentItem = cup;

            cup.Add();
        }

        #endregion

        #region Attributes

        private bool isOn;

        private static readonly Timer FrameTimer = new Timer(1000 / 30f);

        private readonly SoundPlayer microwaveBeep = new SoundPlayer(Properties.Resources.microwave_beep);

        public static MainWindow Main;

        private readonly EmptyItem empty;
        private readonly DonutItem donut;
        private readonly CupItem cup;

        #endregion

        #region Properties

        public Timings ClipTimings { get; } =
            JsonConvert.DeserializeObject<Timings>(File.ReadAllText(@"resources/Timings.json"));

        public Clip CurrentClip { get; set; }
        public Clip NextClip { private get; set; }

        public IItem CurrentItem { get; private set; }

        private bool IsOpen { get; set; }
        public bool IsMicrowaving { private get; set; }

        public string Display
        {
            private get => MicrowaveDisplay.Content.ToString();
            set { Dispatcher.Invoke(() => { MicrowaveDisplay.Content = value; }); }
        }

        #endregion
    }
}