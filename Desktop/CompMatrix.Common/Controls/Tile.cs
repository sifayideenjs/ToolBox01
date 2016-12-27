using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToolBox.Common.Controls
{
    public class Tile : Button
    {
        static Tile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tile), new FrameworkPropertyMetadata(typeof(Tile)));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Tile), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(Tile), new PropertyMetadata(default(ImageSource)));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ContentVisibleProperty =
            DependencyProperty.Register("ContentVisible", typeof(Visibility), typeof(Tile), new PropertyMetadata(default(Visibility)));

        public Visibility ContentVisible
        {
            get { return (Visibility)GetValue(ContentVisibleProperty); }
            set { SetValue(ContentVisibleProperty, value); }
        }

        public static readonly DependencyProperty IconStyleProperty =
            DependencyProperty.Register("IconStyle", typeof(Style), typeof(Tile), new PropertyMetadata(default(Style)));

        public Style IconStyle
        {
            get { return (Style)GetValue(IconStyleProperty); }
            set { SetValue(IconStyleProperty, value); }
        }

    }
}
