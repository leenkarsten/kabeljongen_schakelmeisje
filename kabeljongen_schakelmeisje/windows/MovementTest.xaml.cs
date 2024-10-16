using System.ComponentModel;
using System.Windows;

namespace kabeljongen_schakelmeisje.windows
{
    /// <summary>
    /// Interaction logic for MovementTest.xaml
    /// </summary>
    public partial class MovementTest : Window, INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private MovementService movementService;

        public MovementTest()
        {
            InitializeComponent();
            DataContext = this;

            movementService = new MovementService(Player, Player2, this, Ground);

            double screenHeight = SystemParameters.PrimaryScreenHeight;
            GroundHeight = screenHeight - 50; // Set the ground height based on the window height
        }
    }
}
