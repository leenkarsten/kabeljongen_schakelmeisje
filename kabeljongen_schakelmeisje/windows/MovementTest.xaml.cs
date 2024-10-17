using kabeljongen_schakelmeisje.Services;
using System.ComponentModel;
using System.Numerics;
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

        private CollisionDetectionService collisionService;

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

            CheckCollisons(Player, box, OnCollisionDetected);

            double screenHeight = SystemParameters.PrimaryScreenHeight;
            GroundHeight = screenHeight - 50; // Set the ground height based on the window height
        }

        private void CheckCollisons(UIElement obj1, UIElement obj2, System.Action method)
        {
            collisionService = new CollisionDetectionService(obj1, obj2);
            collisionService.CollisionDetected += OnCollisionDetected;
        }

        private void OnCollisionDetected()
        {
            MessageBox.Show("This is a basic alert message.", "Alert");
        }
    }
}
