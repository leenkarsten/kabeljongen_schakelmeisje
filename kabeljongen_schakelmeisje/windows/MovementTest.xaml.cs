using kabeljongen_schakelmeisje.Services;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
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

        private double _blockHeight;
        public double BlockHeight
        {
            get => _blockHeight;
            set
            {
                _blockHeight = value;
                OnPropertyChanged(nameof(BlockHeight));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private CollisionDetectionService collisionService;
        private CollisionDetectionService switchCollisionService;
        private MovementService movementService;

        private bool isPlayer2OnSwitch = false; 

        public MovementTest()
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

            CheckCollisons(Player, box, OnCollisionDetected);

            CheckCollisons(Player2, schakel, OnSwitchCollisionDetected, OnSwitchCollisionEnded);

            double screenHeight = SystemParameters.PrimaryScreenHeight;
            GroundHeight = screenHeight - 28;
            BlockHeight = screenHeight - 190;
        }

        private void CheckCollisons(UIElement obj1, UIElement obj2, System.Action onCollisionDetected, System.Action onCollisionEnded = null)
        {
            var collisionDetectionService = new CollisionDetectionService(obj1, obj2);
            collisionDetectionService.CollisionDetected += onCollisionDetected;

            if (onCollisionEnded != null)
            {
                collisionDetectionService.CollisionEnded += onCollisionEnded;
            }
        }

        private void OnSwitchCollisionDetected()
        {
            isPlayer2OnSwitch = true;
        }

        private void OnSwitchCollisionEnded()
        {
            isPlayer2OnSwitch = false; 
        }

        private void OnCollisionDetected()
        {
            //MessageBox.Show("This is a basic alert message.", "Alert");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Oem2 && isPlayer2OnSwitch)
            {
                if (licht.Visibility == Visibility.Visible)
                {
                    licht.Visibility = Visibility.Collapsed;
                }
                else
                {
                    licht.Visibility = Visibility.Visible;
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}