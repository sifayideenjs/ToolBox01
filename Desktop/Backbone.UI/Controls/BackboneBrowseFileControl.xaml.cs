using Backbone.Common.Interface;
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
    /// Interaction logic for BackboneBrowseFileControl.xaml
    /// </summary>
    public partial class BackboneBrowseFileControl : UserControl
    {
        public BackboneBrowseFileControl()
        {
            InitializeComponent();
        }

        public BackboneBrowseFileViewModel BackboneBrowseFileViewModel
        {
            get { return (BackboneBrowseFileViewModel)GetValue((BackboneBrowseFileViewModelProperty)); }
            set { SetValue(BackboneBrowseFileViewModelProperty, value); }
        }

        static FrameworkPropertyMetadata backboneBrowseFileMetadata =
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnBackboneBrowseFileChanged),
                null,
                false,
                UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty BackboneBrowseFileViewModelProperty = DependencyProperty.Register("BackboneBrowseFileViewModel", typeof(BackboneBrowseFileViewModel), typeof(BackboneBrowseFileControl), backboneBrowseFileMetadata);

        private static void OnBackboneBrowseFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //IBackboneProperty backboneProperty = e.NewValue as IBackboneProperty;
            //filePathTextBox.Text = backboneProperty.DefaultValue;
        }
    }
}
