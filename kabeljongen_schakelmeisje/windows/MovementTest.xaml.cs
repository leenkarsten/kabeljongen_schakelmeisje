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
        private MovementService movementService;

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

            double screenHeight = SystemParameters.PrimaryScreenHeight;
            GroundHeight = screenHeight - 28;
            BlockHeight = screenHeight - 190;
        }

        private void CheckCollisons(UIElement obj1, UIElement obj2, System.Action method)
        {
            collisionService = new CollisionDetectionService(obj1, obj2);
            collisionService.CollisionDetected += method;
        }

        private void OnCollisionDetected()
        {
            //MessageBox.Show("This is a basic alert message.", "Alert");
        }

        // Nieuwe KeyDown-handler voor het Window
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Check of de 'E'-toets is ingedrukt
            if (e.Key == Key.E)
            {
                // Toon het lichtobject
                licht.Visibility = Visibility.Visible;
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
