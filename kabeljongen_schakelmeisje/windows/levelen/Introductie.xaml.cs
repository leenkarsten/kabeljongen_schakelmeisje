using kabeljongen_schakelmeisje.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace kabeljongen_schakelmeisje.windows.levelen
{
    public partial class Introductie : Window, INotifyPropertyChanged
    {
        private double _groundHeight;
        public double GroundHeight
        {
            get => _groundHeight;
            set
            {
                _groundHeight = value;
                OnPropertyChanged(nameof(GroundHeight));
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private CollisionDetectionService collisionService;
        private MovementService movementService;
        private SwitchService switchService;

        public Introductie()
        {
            InitializeComponent();
            DataContext = this;

            List<Rectangle> list = new List<Rectangle>();
            foreach (var x in GameCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "platform")
                {
                    list.Add(x);
                }
            }

            movementService = new MovementService(Player, Player2, this, Ground, list);

            switchService = new SwitchService(Player2, schakel, OnSwitchActivated, OnSwitchDeactivated);

            double screenHeight = SystemParameters.PrimaryScreenHeight;
            GroundHeight = screenHeight - 28;
        }

        private void OnSwitchActivated()
        {
            licht.Visibility = Visibility.Visible;
        }

        private void OnSwitchDeactivated()
        {
            licht.Visibility = Visibility.Collapsed;
        }
    }
}