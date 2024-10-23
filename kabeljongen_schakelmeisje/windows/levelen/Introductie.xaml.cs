using kabeljongen_schakelmeisje.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace kabeljongen_schakelmeisje.windows.levelen
{
    /// <summary>
    /// Interaction logic for Introductie.xaml
    /// </summary>
    public partial class Introductie : Window, INotifyPropertyChanged
    {
        private double _groundHeight;
        private bool isLeverFlipped = false; // Flag to track if the lever is flipped
        private bool isPlayerNearLever = false; // Flag to check if the player is near the lever
        private bool isPlayerCollidingWithBattery = false; // Already existing flag
        private bool isPlayerOnButton = false;


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
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            GroundHeight = screenHeight - 28;

            collisionService = new CollisionDetectionService(Player, Battery);
            collisionService.CollisionDetected += OnPlayerBatteryCollision;

            // Set up collision detection with the lever
            var leverCollisionService = new CollisionDetectionService(Player, Lever);
            leverCollisionService.CollisionDetected += OnPlayerLeverCollision;

            // Create a new collision detection service between player and button
            var buttonCollisionService = new CollisionDetectionService(Player, Button);
            buttonCollisionService.CollisionDetected += OnPlayerButtonCollision;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the "E" key is pressed and if the player is colliding with the battery
            if (e.Key == Key.E && isPlayerCollidingWithBattery)
            {
                ChangeCableState();
                ChangeLeverImageElek();
                isPlayerCollidingWithBattery = false; // Reset the flag after action
            }
         
            if (e.Key == Key.H && isLeverFlipped && isPlayerNearLever)
            {
                RemoveLatch();
            }
        }
        private void OnPlayerButtonCollision()
        {
            isPlayerOnButton = true; // Set the flag to true when player is on the button
            RemoveDoor(); // Remove the door when the player is on the button
        }
        private void OnPlayerLeftButton()
        {
            isPlayerOnButton = false; // Reset the flag when the player leaves the button
            MessageBox.Show("Player left the button");
        }
        private void RemoveDoor()
        {
            if (isPlayerOnButton && Door != null)
            {
                GameCanvas.Children.Remove(Door); // Removes the door from the canvas
            }
        }

        private void ChangeCableState()
        {
            if (cable != null)
            {
                cable.Stroke = new SolidColorBrush(Colors.Black); // Change to any color you like
            }
        }

        private void OnPlayerBatteryCollision()
        {
            isPlayerCollidingWithBattery = true;
        }

        private void OnPlayerLeverCollision()
        {
            isPlayerNearLever = true;
        }

        //comments in het nederlands want deze images path meuk verdiend geen engelse comments 
        private void ChangeLeverImageElek()
        {
            // pak de base directory waar de solution op runned
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = System.IO.Path.Combine(baseDirectory, "IMG", "image-removebg-preview (12).png");

            // Checked of de file wel bestaad
            if (File.Exists(imagePath))
            {
                //laad image van een gemaakt absolute path
                BitmapImage newImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                Lever.Source = newImage;

                Panel.SetZIndex(Lever, 1000); // ik haat images

                // Set the flag indicating that the lever has been flipped
                isLeverFlipped = true;
            }
        }

        private void ChangeLeverState()
        {
            {
                // pak de base directory waar de solution op runned
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = System.IO.Path.Combine(baseDirectory, "IMG", "image-removebg-preview (14).png");

                // Checked of de file wel bestaad
                if (File.Exists(imagePath))
                {
                    //laad image van een gemaakt absolute path
                    BitmapImage newImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                    Lever.Source = newImage;

                    Panel.SetZIndex(Lever, 1001); // ik haat images

                }
            }
        }

        private void RemoveLatch()
        {
            if (GameCanvas.Children.Contains(Latch))
            {
                GameCanvas.Children.Remove(Latch);

                movementService.RemovePlatform(Latch);
                isLeverFlipped = false;
                isPlayerNearLever = false;
            }
     
        }



    }

}
