using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

public class MovementService
{
    private DispatcherTimer gameTimer;
    private double velocityX1 = 0;
    private double velocityY1 = 0;
    private double velocityX2 = 0;
    private double velocityY2 = 0;
    private double gravity = 1;
    private double jumpStrength = -15;
    private double speed = 4;
    private bool isJumping1 = false;
    private bool isJumping2 = false;

    private UIElement player1;
    private UIElement player2;

    public MovementService(UIElement player1, UIElement player2, Window window)
    {
        this.player1 = player1;
        this.player2 = player2;

        window.KeyDown += OnKeyDown;
        window.KeyUp += OnKeyUp;

        StartGameLoop();
    }

    private void StartGameLoop()
    {
        gameTimer = new DispatcherTimer();
        gameTimer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
        gameTimer.Tick += GameLoop;
        gameTimer.Start();
    }

    private void GameLoop(object sender, EventArgs e)
    {
        velocityY1 += gravity;
        velocityY2 += gravity;

        double player1Top = Canvas.GetTop(player1);
        Canvas.SetTop(player1, player1Top + velocityY1);

        double player1Left = Canvas.GetLeft(player1);
        Canvas.SetLeft(player1, player1Left + velocityX1);

        double player2Top = Canvas.GetTop(player2);
        Canvas.SetTop(player2, player2Top + velocityY2);

        double player2Left = Canvas.GetLeft(player2);
        Canvas.SetLeft(player2, player2Left + velocityX2);

        if (player1Top >= 860)
        {
            if (!isJumping1)
            {
                velocityY1 = 0;
                Canvas.SetTop(player1, 860);
            }
            isJumping1 = false;
        }

        if (player2Top >= 860)
        {
            if (!isJumping2)
            {
                velocityY2 = 0;
                Canvas.SetTop(player2, 860);
            }
            isJumping2 = false;
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.W && !isJumping1)
        {
            velocityY1 = jumpStrength;
            isJumping1 = true;
        }

        if (e.Key == Key.D)
        {
            velocityX1 = speed;
        }

        if (e.Key == Key.A)
        {
            velocityX1 = -speed;
        }

        // Player2 controls
        if (e.Key == Key.Up && !isJumping2)
        {
            velocityY2 = jumpStrength;
            isJumping2 = true;
        }

        if (e.Key == Key.Right)
        {
            velocityX2 = speed;
        }

        if (e.Key == Key.Left)
        {
            velocityX2 = -speed;
        }
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {

        if (e.Key == Key.D || e.Key == Key.A)
        {
            velocityX1 = 0;
        }

        if (e.Key == Key.Right || e.Key == Key.Left)
        {
            velocityX2 = 0;
        }
    }
}
