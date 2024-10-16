using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kabeljongen_schakelmeisje.windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Audio(object sender, RoutedEventArgs e)
        {
            AudioOverlay.Visibility = Visibility.Visible;

        }

        private void Button_Terug(object sender, RoutedEventArgs e)
        {
            AudioOverlay.Visibility = Visibility.Collapsed;
        }

        private void MuziekSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.mediaPlayer.Volume = MuziekSlider.Value / 100;
        }

        private void HoofdMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
