using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CeleryApp
{
    class Animation
    {
        Storyboard Storyboard = new Storyboard();
        public async void MoveAnimation(DependencyObject Object, Thickness Get, Thickness Set)//obsolete don't use, will be replaced soon use TimedMoveAnimation
        {
            var Animation = new ThicknessAnimation
            {
                From = new Thickness?(Get),
                To = new Thickness?(Set),
                Duration = TimeSpan.FromMilliseconds(1000.0),
                EasingFunction = this.Easing
            };
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.MarginProperty));
            this.Storyboard.Children.Add(Animation);
            this.Storyboard.Begin();
            await Task.Delay(100);
            this.Storyboard.Children.Remove(Animation);
        }

        public async void TimedMoveAnimation(DependencyObject Object, Thickness Get, Thickness Set, Double Time)
        {
            var Animation = new ThicknessAnimation
            {
                From = new Thickness?(Get),
                To = new Thickness?(Set),
                Duration = TimeSpan.FromMilliseconds(Time),
                EasingFunction = this.Easing
            };
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.MarginProperty));
            this.Storyboard.Children.Add(Animation);
            this.Storyboard.Begin();
            await Task.Delay(100);
            this.Storyboard.Children.Remove(Animation);
        } 
        public async void WidthAnimation(DependencyObject Object, double Set)
        {
            var Animation = new DoubleAnimation();
            Animation.EasingFunction = this.Easing;
            Animation.To = Set;
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.WidthProperty));
            this.Storyboard.Children.Add(Animation);
            this.Storyboard.Begin();
            await Task.Delay(1000);
            this.Storyboard.Children.Remove(Animation);
        }


        public async void HeightAnimation(DependencyObject Object, double Set)
        {
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = this.Easing;
            Animation.To = Set;

            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.HeightProperty));
            this.Storyboard.Children.Add(Animation);
            this.Storyboard.Begin();
            await Task.Delay(1000);
            this.Storyboard.Children.Remove(Animation);
        }

        public async void OpacityAnimation(DependencyObject Object,double Get, double Set, double Duration)
        {
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = this.Easing;
            Animation.Duration = TimeSpan.FromMilliseconds(Duration);
            Animation.From = Get;
            Animation.To = Set;        
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.OpacityProperty));
            this.Storyboard.Children.Add(Animation);
            this.Storyboard.Begin();
            await Task.Delay(1000);
            this.Storyboard.Children.Remove(Animation);
        }

        public async void WidthAnimation(DependencyObject Object, double Get, double Set, double Duration)
        {
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = this.Easing;
            Animation.Duration = TimeSpan.FromMilliseconds(Duration);
            Animation.From = Get;
            Animation.To = Set;
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.WidthProperty));
            this.Storyboard.Children.Add(Animation);
            this.Storyboard.Begin();
            await Task.Delay(100);
            this.Storyboard.Children.Remove(Animation);
        }

        public async void HeightAnimation(DependencyObject Object, double Get, double Set, double Duration)
        {
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = this.Easing;
            Animation.Duration = TimeSpan.FromMilliseconds(Duration);
            Animation.From = Get;
            Animation.To = Set;
            Storyboard.SetTarget(Animation, Object);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.HeightProperty));
            this.Storyboard.Children.Add(Animation);
            this.Storyboard.Begin();
            await Task.Delay(1000);
            this.Storyboard.Children.Remove(Animation);
        }

        public void Rotate(RotateTransform Object, double Get, double Set, double Duration)
        {
            DoubleAnimation Animation = new DoubleAnimation()
            {
                From = Get,
                To = Set,
                Duration = TimeSpan.FromMilliseconds(Duration),
                EasingFunction = Easing,
            };
            Object.BeginAnimation(RotateTransform.AngleProperty, Animation, HandoffBehavior.SnapshotAndReplace);
        }

        private IEasingFunction Easing { get; set; } = new QuadraticEase
        {
            EasingMode = EasingMode.EaseInOut
        };
    }
}
