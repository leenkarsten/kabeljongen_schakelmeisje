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
    /// Interaction logic for MovementTest.xaml
    /// </summary>
    public partial class MovementTest : Window
    {   
        private MovementService movementService;
        public MovementTest()
        {
            InitializeComponent();

            movementService = new MovementService(Player, Player2, this);
        }
    }
}
