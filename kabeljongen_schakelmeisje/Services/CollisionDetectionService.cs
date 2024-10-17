using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace kabeljongen_schakelmeisje.Services
{
    public class CollisionDetectionService
    {
        private readonly UIElement element1;
        private readonly UIElement element2;
        private readonly DispatcherTimer collisionTimer;

        public event Action CollisionDetected;

        public CollisionDetectionService(UIElement element1, UIElement element2)
        {
            this.element1 = element1;
            this.element2 = element2;

            collisionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(6)
            };
            collisionTimer.Tick += CheckCollision;
            collisionTimer.Start();
        }

        private void CheckCollision(object sender, EventArgs e)
        {
            if (AreColliding())
            {
                CollisionDetected?.Invoke();
            }
        }

        public bool AreColliding()
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
}
