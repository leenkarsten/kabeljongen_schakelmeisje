using System;
using System.ComponentModel;
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
    private double screenHeight;

    private UIElement player1;
    private UIElement player2;
    private UIElement ground;
    private Window window;

    public MovementService(UIElement player1, UIElement player2, Window window, UIElement ground)
    {
        this.player1 = player1;
        this.player2 = player2;
        this.ground = ground;
        this.window = window;

        screenHeight = window.Height;

        window.KeyDown += OnKeyDown;
        window.KeyUp += OnKeyUp;

        StartGameLoop();
    }

    private void StartGameLoop()
    {
        gameTimer = new DispatcherTimer();
        gameTimer.Interval = TimeSpan.FromMilliseconds(6);
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

        double groundTop = Canvas.GetTop(ground);

        if (IsColliding(player1, ground))
        {
            if (!isJumping1)
            {
                velocityY1 = 0;
                Canvas.SetTop(player1, groundTop - 28);
            }
            isJumping1 = false;
        }

        if (IsColliding(player2, ground))
        {
            if (!isJumping2)
            {
                velocityY2 = 0;
                Canvas.SetTop(player2, groundTop - 28);
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

    public bool IsColliding(UIElement element1, UIElement element2)
    {
        double left1 = Canvas.GetLeft(element1);
        double top1 = Canvas.GetTop(element1);
        double right1 = left1 + element1.RenderSize.Width;
        double bottom1 = top1 + element1.RenderSize.Height;

        double left2 = Canvas.GetLeft(element2);
        double top2 = Canvas.GetTop(element2);
        double right2 = left2 + element2.RenderSize.Width;
        double bottom2 = top2 + element2.RenderSize.Height;

        return left1 < right2 && right1 > left2 && top1 < bottom2 && bottom1 > top2;
    }
}