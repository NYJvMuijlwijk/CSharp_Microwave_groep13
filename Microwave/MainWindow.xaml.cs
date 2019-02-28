using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microwave.Annotations;
using Microwave.Item;
using Newtonsoft.Json;
using static System.Int32;

namespace Microwave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Attributes

        private bool isOn;

        private static readonly Timer frameTimer = new Timer(1000/30);

        private readonly SoundPlayer microwaveBeep = new SoundPlayer(Properties.Resources.microwave_beep);

        public static MainWindow Main;

        readonly EmptyItem empty;
        readonly DonutItem donut;
        readonly CupItem cup;

        #endregion

        #region Properties

        public SoundPlayer MicrowaveRunning { get; } = new SoundPlayer(Properties.Resources.microwave_running_short);
        public SoundPlayer MicrowaveDone { get; } = new SoundPlayer(Properties.Resources.microwave_done);

        public Timings ClipTimings { get; } = JsonConvert.DeserializeObject<Timings>(File.ReadAllText(@"resources/Timings.json"));

        public Clip CurrentClip { get; set; }
        public Clip NextClip { get; set; }

        public IItem CurrentItem { get; set; }

        public bool IsOpen { get; private set; }
        public bool IsMicrowaving { get; set; }


        public string Display
        {
            get => MicrowaveDisplay.Content.ToString();
            set { Dispatcher.Invoke(() => { MicrowaveDisplay.Content = value; }); }
        }

        #endregion

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

            frameTimer.Elapsed += TimerOnElapsed;

            MediaElement.LoadedBehavior = MediaState.Manual;
            MediaElement.Play();
        }

        /// <summary>
        /// checks if the current position of the MediaElement has passed the end of the CurrentClip.
        /// if so, it sets the Position to the start of the NextClip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (!isOn) return;
            
            Dispatcher.InvokeAsync(()=>
            {
                if (MediaElement.Position < CurrentClip.End) return;
                Debug.WriteLine("Clip End" );
                MediaElement.Position = NextClip.Start;
                CurrentClip = NextClip;
            });
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isOn)
            {
                MediaElement.Source = new Uri("resources/microwave.mp4", UriKind.RelativeOrAbsolute);
                MediaElement.Play();
                CurrentItem.Idle(IsOpen);

                frameTimer.Start();
                
                Display = "00:00";

                isOn = true;
            }
            else if (!IsMicrowaving && Display != "00:00")
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

            microwaveBeep.Play();
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

            var number = Parse(((Button)sender).Tag.ToString());
            DisplayTimer.Add(number);
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isOn) return;

            if (isOn && !IsMicrowaving && Display == "00:00")
            {
                isOn = false;
                frameTimer.Stop();
                MicrowaveDisplay.Content = "";
                MediaElement.Source = new Uri("resources/Off.png", UriKind.RelativeOrAbsolute);
                MediaElement.Play();
            }
            else if (isOn && !IsMicrowaving && Display != "00:00")
            {
                DisplayTimer.Reset();
            }
            else if (IsMicrowaving)
            {
                IsMicrowaving = false;
                CurrentItem.Idle(IsOpen);
                DisplayTimer.Stop();
            }

            microwaveBeep.Play();
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
    }
}