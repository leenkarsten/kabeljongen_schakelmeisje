using kabeljongen_schakelmeisje.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace kabeljongen_schakelmeisje.windows
{
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
        private MovementService movementService;
        private bool batteryActivated = true; 
        public MovementTest()
        {
            InitializeComponent();
            DataContext = this;

            movementService = new MovementService(Player, Player2, this, Ground);

            CheckCollisons(Player, box, OnCollisionDetected);

            double screenHeight = SystemParameters.PrimaryScreenHeight;
            GroundHeight = screenHeight - 28; // Set the ground height based on the window height
        }

        private void CheckCollisons(UIElement obj1, UIElement obj2, System.Action method)
        {
            collisionService = new CollisionDetectionService(obj1, obj2);
            collisionService.CollisionDetected += method;
        }
        
        private void OnCollisionDetected()
        {
            if (batteryActivated)
            {
                MessageBox.Show("Battery action triggered!", "Action");
                batteryActivated = false;
            }  
        }

        // Handle key events
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                if(batteryActivated)
                { 
                    CheckCollisons(Player, Battery, OnCollisionDetected); 
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
