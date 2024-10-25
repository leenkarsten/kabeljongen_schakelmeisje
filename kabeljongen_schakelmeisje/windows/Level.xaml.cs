using kabeljongen_schakelmeisje.windows.levelen;
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
        public string naam1;
        public string naam2;
        public Level(string Naam1, string Naam2)
        {
            InitializeComponent();
            //ik maak een change

            naam1 = Naam1;
            naam2 = Naam2;

            name1.Text = Naam1;
            name2.Text = Naam2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow startscherm = new MainWindow();
            startscherm.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Introductie movementTest = new Introductie(naam1, naam2);
            movementTest.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
