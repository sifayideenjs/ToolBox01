using Backbone.Common.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Backbone.Common.UI.Controls
{
    /// <summary>
    /// Interaction logic for BackboneBrowseFolderControl.xaml
    /// </summary>
    public partial class BackboneBrowseFolderControl : UserControl
    {
        public BackboneBrowseFolderControl()
        {
            InitializeComponent();
        }

        public BackboneBrowseFolderViewModel BackboneBrowseFolderViewModel
        {
            get { return (BackboneBrowseFolderViewModel)GetValue((BackboneBrowseFolderViewModelProperty)); }
            set { SetValue(BackboneBrowseFolderViewModelProperty, value); }
        }

        static FrameworkPropertyMetadata backboneBrowseFolderMetadata =
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnBackboneBrowseFolderChanged),
                null,
                false,
                UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty BackboneBrowseFolderViewModelProperty = DependencyProperty.Register("BackboneBrowseFolderViewModel", typeof(BackboneBrowseFolderViewModel), typeof(BackboneBrowseFolderControl), backboneBrowseFolderMetadata);

        private static void OnBackboneBrowseFolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
