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

namespace kabeljongen_schakelmeisje.windows.levelen
{
    public partial class Introductie : Window, INotifyPropertyChanged
    {
        private double _groundHeight;
        private bool isLeverFlipped = false;
        private bool isPlayerNearLever = false;
        private bool isPlayerCollidingWithBattery = false;

        // Track whether player is on either of the buttons
        private bool isPlayer1OnButton1 = false;
        private bool isPlayer2OnButton1 = false;
        private bool isPlayer1OnButton2 = false;
        private bool isPlayer2OnButton2 = false;

        // Track whether player is on Button3 and Button4
        private bool isPlayer1OnButton3 = false;
        private bool isPlayer2OnButton4 = false;

        // Fields to store the Door's position
        private double doorLeft;
        private double doorTop;

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

            List<System.Windows.Shapes.Rectangle> list = new List<System.Windows.Shapes.Rectangle>();

            foreach (var x in GameCanvas.Children.OfType<System.Windows.Shapes.Rectangle>())
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
            var leverCollisionService = new CollisionDetectionService(Player2, Lever);
            leverCollisionService.CollisionDetected += OnPlayerLeverCollision;

            // Set up collision detection for both players and both buttons
            var buttonCollisionService1 = new CollisionDetectionService(Player, Button);
            buttonCollisionService1.CollisionDetected += OnPlayer1Button1Collision;
            buttonCollisionService1.CollisionEnded += OnPlayer1LeftButton1;

            var buttonCollisionService1_2 = new CollisionDetectionService(Player2, Button);
            buttonCollisionService1_2.CollisionDetected += OnPlayer2Button1Collision;
            buttonCollisionService1_2.CollisionEnded += OnPlayer2LeftButton1;

            var buttonCollisionService2D = new CollisionDetectionService(Player, Buttons);
            buttonCollisionService2D.CollisionDetected += OnPlayer1Button2Collision;
            buttonCollisionService2D.CollisionEnded += OnPlayer1LeftButton2;

            var buttonCollisionService2_2D = new CollisionDetectionService(Player2, Buttons);
            buttonCollisionService2_2D.CollisionDetected += OnPlayer2Button2Collision;
            buttonCollisionService2_2D.CollisionEnded += OnPlayer2LeftButton2;

            // Set up collision detection for Button3 and Button4
            var button3CollisionService = new CollisionDetectionService(Player, Button3);
            button3CollisionService.CollisionDetected += OnPlayer1Button3Collision;
            button3CollisionService.CollisionEnded += OnPlayer1LeftButton3;

            var button4CollisionService = new CollisionDetectionService(Player2, Button4);
            button4CollisionService.CollisionDetected += OnPlayer2Button4Collision;
            button4CollisionService.CollisionEnded += OnPlayer2LeftButton4;

            InitializeDoorPosition();  // Store the initial door position
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

        private void InitializeDoorPosition()
        {
            // Save the current position of the Door before any action
            doorLeft = Canvas.GetLeft(Door);
            doorTop = Canvas.GetTop(Door);
        }

        // Collision logic for Player1 on Button1
        private void OnPlayer1Button1Collision()
        {
            isPlayer1OnButton1 = true;
            RemoveDoor(); // Remove the door when Player1 is on Button1
        }

        // Collision logic for Player2 on Button1
        private void OnPlayer2Button1Collision()
        {
            isPlayer2OnButton1 = true;
            RemoveDoor(); // Remove the door when Player2 is on Button1
        }

        // Collision logic for Player1 on Button2
        private void OnPlayer1Button2Collision()
        {
            isPlayer1OnButton2 = true;
            RemoveDoor(); // Remove the door when Player1 is on Button2
        }

        // Collision logic for Player2 on Button2
        private void OnPlayer2Button2Collision()
        {
            isPlayer2OnButton2 = true;
            RemoveDoor(); // Remove the door when Player2 is on Button2
        }

        // Collision logic for Player1 on Button3
        private void OnPlayer1Button3Collision()
        {
            isPlayer1OnButton3 = true;
            CheckBothPlayersOnButtons3And4();
        }

        // Collision logic for Player2 on Button4
        private void OnPlayer2Button4Collision()
        {
            isPlayer2OnButton4 = true;
            CheckBothPlayersOnButtons3And4();
        }

        // Logic when Player1 leaves Button3
        private void OnPlayer1LeftButton3()
        {
            isPlayer1OnButton3 = false;
        }

        // Logic when Player2 leaves Button4
        private void OnPlayer2LeftButton4()
        {
            isPlayer2OnButton4 = false;
        }

        private void CheckBothPlayersOnButtons3And4()
        {
            if (isPlayer1OnButton3 && isPlayer2OnButton4)
            {
                // Navigate to the next window when both players are on their buttons
                Level nextLevelWindow = new Level();
                nextLevelWindow.Show();
                this.Close();  // Optionally close the current window
            }
        }

        // Logic when Player1 leaves Button1
        private void OnPlayer1LeftButton1()
        {
            isPlayer1OnButton1 = false;
            CheckIfBothPlayersLeftBothButtons(); // Check if both players left the buttons
        }

        // Logic when Player2 leaves Button1
        private void OnPlayer2LeftButton1()
        {
            isPlayer2OnButton1 = false;
            CheckIfBothPlayersLeftBothButtons(); // Check if both players left the buttons
        }

        // Logic when Player1 leaves Button2
        private void OnPlayer1LeftButton2()
        {
            isPlayer1OnButton2 = false;
            CheckIfBothPlayersLeftBothButtons(); // Check if both players left the buttons
        }

        // Logic when Player2 leaves Button2
        private void OnPlayer2LeftButton2()
        {
            isPlayer2OnButton2 = false;
            CheckIfBothPlayersLeftBothButtons(); // Check if both players left the buttons
        }

        private void RemoveDoor()
        {
            if ((isPlayer1OnButton1 || isPlayer2OnButton1 || isPlayer1OnButton2 || isPlayer2OnButton2) && Door != null)
            {
                // Remove the Door from the canvas
                GameCanvas.Children.Remove(Door);

                // Remove the Door from the platforms list in MovementService
                movementService.RemovePlatform(Door);  // Ensure Door is not part of collision anymore
            }
        }

        private void CheckIfBothPlayersLeftBothButtons()
        {
            if (!isPlayer1OnButton1 && !isPlayer2OnButton1 && !isPlayer1OnButton2 && !isPlayer2OnButton2 && !GameCanvas.Children.Contains(Door))
            {
                // Re-add the Door to the canvas at its original position
                GameCanvas.Children.Add(Door);
                Canvas.SetLeft(Door, doorLeft);
                Canvas.SetTop(Door, doorTop);

                // Re-add the Door to the platforms list in MovementService
                movementService.AddPlatform(Door);  // Ensure Door is now part of collision again
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

        private void ChangeLeverImageElek()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = System.IO.Path.Combine(baseDirectory, "IMG", "image-removebg-preview (12).png");

            if (File.Exists(imagePath))
            {
                BitmapImage newImage = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                Lever.Source = newImage;

                Panel.SetZIndex(Lever, 1000);

                isLeverFlipped = true;
            }
        }

        private void RemoveLatch()
        {
            if (GameCanvas.Children.Contains(Latch))
            {
                GameCanvas.Children.Remove(Latch);

                movementService.RemovePlatform(Latch); // Remove the latch from the platforms list
                isLeverFlipped = false;
                isPlayerNearLever = false;
            }
        }
    }
}