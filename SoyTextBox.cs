using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Image2Image;

public class SoyTextBox : TextBox
{
    public static readonly DependencyProperty HintProperty =
        DependencyProperty.Register("Hint", typeof(string), typeof(SoyTextBox));
    
    public static readonly DependencyProperty BorderColorProperty =
        DependencyProperty.Register("BorderColor", typeof(Brush), typeof(SoyTextBox), new PropertyMetadata(Brushes.Black, OnBorderColorChanged));
    

    public string Hint
    {
        get { return (string)GetValue(HintProperty); }
        set { SetValue(HintProperty, value); }
    }
    
    public Brush BorderColor
    {
        get { return (Brush)GetValue(BorderColorProperty); }
        set { SetValue(BorderColorProperty, value); }
    }
    
    private static void OnBorderColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SoyTextBox soyTextBox)
        {
            soyTextBox.UpdateBorderColor();
        }
    }
    
    private void UpdateBorderColor()
    {
        BorderBrush = BorderColor;
    }
    

    // 在构造函数中初始化
    public SoyTextBox()
    {
        // 添加 GotFocus 和 LostFocus 事件处理程序
        this.GotFocus += SoyTextBox_GotFocus;
        this.LostFocus += SoyTextBox_LostFocus;
        
        // 添加鼠标悬停和离开事件处理程序
        this.MouseEnter += SoyTextBox_MouseEnter;
        this.MouseLeave += SoyTextBox_MouseLeave;
        
        this.Loaded += SoyTextBox_Loaded;
        
        BorderThickness = new Thickness(0, 0, 0, 2);
        BorderColor = Brushes.Black;
        TextWrapping = TextWrapping.Wrap;
        CaretBrush = Brushes.LightBlue;
    }
    
    private void SoyTextBox_Loaded(object sender, RoutedEventArgs e)
    {
        SetHintProperties();
        UpdateBorderColor();
    }
    
    private void SoyTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        if (Text == Hint)
        {
            Text = string.Empty;
            Foreground = Brushes.Black;
        }
    }
    
    private void SoyTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        SetHintProperties();
    }
    
    private void SoyTextBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (sender is TextBox textBox)
            AnimateBorderColor(textBox, Brushes.LightBlue);
        
    }

    private void SoyTextBox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (sender is TextBox textBox)
            AnimateBorderColor(textBox, Brushes.Black);

    }
    
    private void AnimateBorderColor(TextBox textBox, SolidColorBrush targetColor)
    {
        ColorAnimation colorAnimation = new ColorAnimation();
        colorAnimation.To = ((SolidColorBrush)targetColor).Color;
        colorAnimation.Duration = TimeSpan.FromSeconds(0.5);
        
        Storyboard storyboard = new Storyboard();
        storyboard.Children.Add(colorAnimation);
        
        Storyboard.SetTarget(colorAnimation, textBox);
        Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

        storyboard.Begin();
    }
    
    private void SetHintProperties()
    {
        if (string.IsNullOrEmpty(Text) && !IsFocused)
        {
            Text = Hint;
            Foreground = Brushes.Gray;
        }
    }
}