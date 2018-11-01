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
    /// Interaction logic for BackboneButtonControl.xaml
    /// </summary>
    public partial class BackboneButtonControl : UserControl
    {
        public BackboneButtonControl()
        {
            InitializeComponent();
        }

        public BackboneButtonViewModel BackboneButtonViewModel
        {
            get { return (BackboneButtonViewModel)GetValue((BackboneButtonViewModelProperty)); }
            set { SetValue(BackboneButtonViewModelProperty, value); }
        }

        static FrameworkPropertyMetadata backboneButtonMetadata =
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnBackboneButtonChanged),
                null,
                false,
                UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty BackboneButtonViewModelProperty = DependencyProperty.Register("BackboneButtonViewModel", typeof(BackboneButtonViewModel), typeof(BackboneButtonControl), backboneButtonMetadata);

        private static void OnBackboneButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
