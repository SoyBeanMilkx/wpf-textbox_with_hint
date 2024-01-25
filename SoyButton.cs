using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Image2Image
{
    public class SoyButton : Button
    {
        static SoyButton() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SoyButton),
                new FrameworkPropertyMetadata(typeof(SoyButton)));
        
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(SoyButton),
                new PropertyMetadata());

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);//不知道为什么有红色下划线
        }

        public SoyButton()
        {
            PreviewMouseDown += SoyButton_PreviewMouseDown;
            PreviewMouseUp += SoyButton_PreviewMouseUp;
            Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            
            DropShadowEffect dropShadowEffect = new DropShadowEffect
            {
                ShadowDepth = 2,
                Direction = 315,
                Opacity = 0.3
            };
            
            Effect = dropShadowEffect;
        }

        private async void SoyButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            await AnimateButtonAsync(0.9, new QuadraticEase());
        }

        private async void SoyButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            await AnimateButtonAsync(1.0, new BounceEase());
        }

        private async Task AnimateButtonAsync(double targetScale, EasingFunctionBase easingFunction)
        {
            await Task.Run(() => 
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    ScaleTransform scaleTransform = new ScaleTransform(1.0, 1.0);
                    RenderTransformOrigin = new Point(0.5, 0.5);

                    DoubleAnimation scaleAnimation = new DoubleAnimation
                    {
                        To = targetScale,
                        Duration = TimeSpan.FromSeconds(0.2),
                        EasingFunction = easingFunction
                    };
                    
                    RenderTransform = scaleTransform;
                    
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                });
            });
        }
    }
}