using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToolBox.Dashboard.Controls
{
    public class HubSection : ContentControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(HubSection), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderCommandProperty = DependencyProperty.Register("HeaderCommand", typeof(ICommand), typeof(HubSection), new PropertyMetadata(default(ICommand)));

        public string HeaderCommand
        {
            get { return (string)GetValue(HeaderCommandProperty); }
            set { SetValue(HeaderCommandProperty, value); }
        }

        static HubSection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HubSection), new FrameworkPropertyMetadata(typeof(HubSection)));
        }
    }
}
