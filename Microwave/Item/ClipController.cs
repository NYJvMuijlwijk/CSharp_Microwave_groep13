using System.Windows.Threading;
using Microwave.json;

namespace Microwave.Item
{
    public class ClipController
    {
        /// <summary>
        /// Sets the MediaElement.Position in MainWindow to the start position of currentClip,
        /// Sets MainWindow.CurrentClip to currentClip,
        /// and MainWindow.NextClip to nextClip
        /// </summary>
        /// <param name="currentClip">The clip to be played</param>
        /// <param name="nextClip">The clip to be played after initial clip finishes</param>
        protected static void ClipSetup(Clip currentClip, Clip nextClip)
        {
            Dispatcher.CurrentDispatcher.InvokeAsync(() => {
                MainWindow.Main.MediaElement.Position = currentClip.Start;
                MainWindow.Main.CurrentClip = currentClip;
                MainWindow.Main.NextClip = nextClip;
            });
        }
    }
}