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
    /// Interaction logic for levelOverview.xaml
    /// </summary>
    public partial class levelOverview : Window
    {
        public levelOverview()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MovementTest movementtest = new MovementTest();
            movementtest.Show();
            this.Close();
        }
    }
}
