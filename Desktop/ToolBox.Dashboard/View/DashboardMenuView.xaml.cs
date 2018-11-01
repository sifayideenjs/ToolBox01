using ToolBox.Common;
using ToolBox.Dashboard.ViewModel;
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

namespace ToolBox.Dashboard.View
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class DashboardMenuView : UserControl, IApplicationView
    {
        public DashboardMenuView(DashboardMenuViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public string ViewName
        {
            get
            {
                return "DashboardMenuView";
            }
        }
    }
}
