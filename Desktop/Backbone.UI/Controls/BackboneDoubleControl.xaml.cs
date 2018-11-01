using Backbone.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for BackboneDoubleTextBox.xaml
    /// </summary>
    public partial class BackboneDoubleControl : UserControl
    {
        public BackboneDoubleControl()
        {
            InitializeComponent();
        }

        public BackboneDoubleViewModel BackboneDoubleViewModel
        {
            get { return (BackboneDoubleViewModel)GetValue((BackboneDoubleViewModelProperty)); }
            set { SetValue(BackboneDoubleViewModelProperty, value); }
        }

        static FrameworkPropertyMetadata backboneDoublePropertyMetadata =
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnBackboneDoubleViewModelChanged),
                null,
                false,
                UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty BackboneDoubleViewModelProperty = DependencyProperty.Register("BackboneDoubleViewModel", typeof(BackboneDoubleViewModel), typeof(BackboneDoubleControl), backboneDoublePropertyMetadata);

        private static void OnBackboneDoubleViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
