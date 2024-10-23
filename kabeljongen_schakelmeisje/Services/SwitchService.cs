using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Input;

namespace kabeljongen_schakelmeisje.Services
{
    public class SwitchService
    {
        private UIElement player;
        private UIElement switchElement;
        private Action onSwitchActivated;
        private Action onSwitchDeactivated;
        private bool isSwitchActivated = false;

        public SwitchService(UIElement player, UIElement switchElement, Action onSwitchActivated, Action onSwitchDeactivated)
        {
            this.player = player;
            this.switchElement = switchElement;
            this.onSwitchActivated = onSwitchActivated;
            this.onSwitchDeactivated = onSwitchDeactivated;

            Application.Current.MainWindow.KeyDown += OnKeyDown;

            StartGameLoop();
        }

        private void StartGameLoop()
        {
            var gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16); // 60 FPS
            gameTimer.Tick += (s, e) => Update();
            gameTimer.Start();
        }

        public void Update()
        {
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Oem2 && IsPlayerOnSwitch())
            {
                ToggleSwitch();
            }
        }

        private bool IsPlayerOnSwitch()
        {
            double playerTop = Canvas.GetTop(player);
            double playerLeft = Canvas.GetLeft(player);
            double playerRight = playerLeft + player.RenderSize.Width;
            double playerBottom = playerTop + player.RenderSize.Height;

            double switchTop = Canvas.GetTop(switchElement);
            double switchLeft = Canvas.GetLeft(switchElement);
            double switchRight = switchLeft + switchElement.RenderSize.Width;
            double switchBottom = switchTop + switchElement.RenderSize.Height;

            bool isOnSwitch = playerBottom >= switchTop && playerTop <= switchBottom &&
                              playerRight >= switchLeft && playerLeft <= switchRight;

            return isOnSwitch;
        }

        private void ToggleSwitch()
        {
            if (!isSwitchActivated)
            {
                onSwitchActivated?.Invoke();
                isSwitchActivated = true;
            }
            else
            {
                onSwitchDeactivated?.Invoke();
                isSwitchActivated = false;
            }
        }
    }
}