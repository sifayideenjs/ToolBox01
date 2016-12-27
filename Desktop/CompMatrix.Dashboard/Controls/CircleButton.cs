using System.Windows;
using System.Windows.Controls;

namespace ToolBox.Dashboard.Controls
{
    public class CircleButton : Button
    {
        static CircleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleButton), new FrameworkPropertyMetadata(typeof(CircleButton)));
        }

        public static readonly DependencyProperty IconStyleProperty =
            DependencyProperty.Register("IconStyle", typeof(Style), typeof(CircleButton), new PropertyMetadata(default(Style)));

        public Style IconStyle
        {
            get { return (Style)GetValue(IconStyleProperty); }
            set { SetValue(IconStyleProperty, value); }
        }
    }
}
