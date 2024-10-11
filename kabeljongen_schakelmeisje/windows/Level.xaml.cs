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
    /// Interaction logic for Level.xaml
    /// </summary>
    public partial class Level : Window
    {
        public Level()
        {
            InitializeComponent();
            //ik maak een change
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow startscherm = new MainWindow();
            startscherm.Show();
            this.Close();
        }
    }
}
