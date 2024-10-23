using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

public class MovementService
{
    private DispatcherTimer gameTimer;
    private double velocityX1 = 0;
    private double velocityY1 = 0;
    private double velocityX2 = 0;
    private double velocityY2 = 0;
    private double gravity = 1;
    private double jumpStrength = -25;
    private double speed = 4;
    private bool isJumping1 = false;
    private bool isJumping2 = false;
    private double screenHeight;


    private UIElement player1;
    private UIElement player2;
    private UIElement ground;
    private List<Rectangle> platforms;
    private Window window;

    public MovementService(UIElement player1, UIElement player2, Window window, UIElement ground, List<Rectangle> platforms)
    {
        this.player1 = player1;
        this.player2 = player2;
        this.ground = ground;
        this.platforms = platforms;
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

        // Handle ground collision for player 1
        if (IsColliding(player1, ground))
        {
            if (!isJumping1)
            {
                velocityY1 = 0;
                Canvas.SetTop(player1, groundTop - 42);
            }
            isJumping1 = false;
        }

        // Handle ground collision for player 2
        if (IsColliding(player2, ground))
        {
            if (!isJumping2)
            {
                velocityY2 = 0;
                Canvas.SetTop(player2, groundTop - 42);
            }
            isJumping2 = false;
        }

        // Handle platform collision and side collision logic
        foreach (var platform in platforms)
        {
            // Player 1
            if (IsOnTopOfPlatform(player1, platform))
            {
                velocityY1 = 0;
                Canvas.SetTop(player1, Canvas.GetTop(platform) - player1.RenderSize.Height);
                isJumping1 = false;
            }
            if (IsUnderPlatform(player1, platform))
            {
                velocityY1 = gravity;
            }
            // Left side collision for player 1, allowing movement away
            if (IsOnLeftSideOfPlatform(player1, platform) && velocityX1 > 0) // Moving right into the wall
            {
                velocityX1 = 0;
                Canvas.SetLeft(player1, Canvas.GetLeft(platform) - player1.RenderSize.Width);
            }
            // Right side collision for player 1, allowing movement away
            else if (IsOnRightSideOfPlatform(player1, platform) && velocityX1 < 0) // Moving left into the wall
            {
                velocityX1 = 0;
                Canvas.SetLeft(player1, Canvas.GetLeft(platform) + platform.RenderSize.Width);
            }

            // Player 2
            if (IsOnTopOfPlatform(player2, platform))
            {
                velocityY2 = 0;
                Canvas.SetTop(player2, Canvas.GetTop(platform) - player2.RenderSize.Height);
                isJumping2 = false;
            }
            if (IsUnderPlatform(player2, platform))
            {
                velocityY2 = gravity;
            }
            // Left side collision for player 2, allowing movement away
            if (IsOnLeftSideOfPlatform(player2, platform) && velocityX2 > 0) // Moving right into the wall
            {
                velocityX2 = 0;
                Canvas.SetLeft(player2, Canvas.GetLeft(platform) - player2.RenderSize.Width);
            }
            // Right side collision for player 2, allowing movement away
            else if (IsOnRightSideOfPlatform(player2, platform) && velocityX2 < 0) // Moving left into the wall
            {
                velocityX2 = 0;
                Canvas.SetLeft(player2, Canvas.GetLeft(platform) + platform.RenderSize.Width);
            }
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

    public bool IsOnTopOfPlatform(UIElement element1, Rectangle element2)
    {
        double left1 = Canvas.GetLeft(element1);
        double top1 = Canvas.GetTop(element1);
        double right1 = left1 + element1.RenderSize.Width;
        double bottom1 = top1 + element1.RenderSize.Height;

        double left2 = Canvas.GetLeft(element2);
        double top2 = Canvas.GetTop(element2);
        double right2 = left2 + element2.RenderSize.Width;
        double bottom2 = top2 + element2.RenderSize.Height;

        bool isAbovePlatform = bottom1 <= top2 + 10 && bottom1 >= top2 - 10;
        bool isHorizontallyAligned = right1 > left2 && left1 < right2;

        return isAbovePlatform && isHorizontallyAligned;
    }

    public bool IsUnderPlatform(UIElement element1, Rectangle element2)
    {
        double left1 = Canvas.GetLeft(element1);
        double top1 = Canvas.GetTop(element1);
        double right1 = left1 + element1.RenderSize.Width;
        double bottom1 = top1 + element1.RenderSize.Height;

        double left2 = Canvas.GetLeft(element2);
        double top2 = Canvas.GetTop(element2);
        double right2 = left2 + element2.RenderSize.Width;
        double bottom2 = top2 + element2.RenderSize.Height;

        bool isBelowPlatform = top1 >= bottom2 - 10 && top1 <= bottom2 + 10;
        bool isHorizontallyAligned = right1 > left2 && left1 < right2;

        return isBelowPlatform && isHorizontallyAligned;
    }

    public bool IsOnLeftSideOfPlatform(UIElement element1, Rectangle element2)
    {
        double left1 = Canvas.GetLeft(element1);
        double top1 = Canvas.GetTop(element1);
        double right1 = left1 + element1.RenderSize.Width;
        double bottom1 = top1 + element1.RenderSize.Height;

        double left2 = Canvas.GetLeft(element2);
        double top2 = Canvas.GetTop(element2);
        double right2 = left2 + element2.RenderSize.Width;
        double bottom2 = top2 + element2.RenderSize.Height;

        bool isVerticallyAligned = bottom1 > top2 && top1 < bottom2;
        bool isLeftOfPlatform = right1 >= left2 - 5 && right1 <= left2 + 5;

        return isVerticallyAligned && isLeftOfPlatform;
    }

    public bool IsOnRightSideOfPlatform(UIElement element1, Rectangle element2)
    {
        double left1 = Canvas.GetLeft(element1);
        double top1 = Canvas.GetTop(element1);
        double right1 = left1 + element1.RenderSize.Width;
        double bottom1 = top1 + element1.RenderSize.Height;

        double left2 = Canvas.GetLeft(element2);
        double top2 = Canvas.GetTop(element2);
        double right2 = left2 + element2.RenderSize.Width;
        double bottom2 = top2 + element2.RenderSize.Height;

        bool isVerticallyAligned = bottom1 > top2 && top1 < bottom2;
        bool isRightOfPlatform = left1 <= right2 + 10 && left1 >= right2 - 10;

        return isVerticallyAligned && isRightOfPlatform;
    }
    public void RemovePlatform(Rectangle platform)
    {
        if (platforms.Contains(platform))
        {
            platforms.Remove(platform);
        }
    }
}
