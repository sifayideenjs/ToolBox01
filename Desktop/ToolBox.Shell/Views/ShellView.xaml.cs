using ToolBox.Shell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolBox.Shell.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView(ShellViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public void AppBarOpen(object sender, RoutedEventArgs e)
        {
            AppBarContent.Visibility = Visibility.Visible;
            AppBarSelector.Visibility = Visibility.Collapsed;
        }

        public void AppBarClose(object sender, RoutedEventArgs e)
        {
            AppBarContent.Visibility = Visibility.Collapsed;
            AppBarSelector.Visibility = Visibility.Visible;
        }
    }
}
