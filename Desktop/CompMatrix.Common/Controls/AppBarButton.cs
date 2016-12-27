using System.Windows;
using System.Windows.Controls;

namespace ToolBox.Common.Controls
{
    public class AppBarButton : Button
    {
        static AppBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBarButton), new FrameworkPropertyMetadata(typeof(AppBarButton)));
        }

        public static readonly DependencyProperty IconStyleProperty =
            DependencyProperty.Register("IconStyle", typeof(Style), typeof(AppBarButton), new PropertyMetadata(default(Style)));

        public Style IconStyle
        {
            get { return (Style)GetValue(IconStyleProperty); }
            set { SetValue(IconStyleProperty, value); }
        }
    }
}
