using kabeljongen_schakelmeisje.windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kabeljongen_schakelmeisje
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_click(object sender, RoutedEventArgs e)
        {
            NamenInvoeren namenInvoeren = new NamenInvoeren();
            namenInvoeren.Show();
            this.Close();
        }

        private void Instellingen_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            //instellingenPagina instellingenS = new instellingenPagina();
            //instellingenS.Show();
            //this.Close();
        }

        private void Uitgang_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}