using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace kabeljongen_schakelmeisje
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application

    {
        public static MediaPlayer mediaPlayer = new MediaPlayer();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            mediaPlayer.Open(new Uri("assets/game-music-loop-6-144641.mp3", UriKind.Relative));

            mediaPlayer.Volume = 0.5;

            mediaPlayer.MediaEnded += (s, ev) => mediaPlayer.Position = TimeSpan.Zero;

            mediaPlayer.Play();
        }
    }



}
