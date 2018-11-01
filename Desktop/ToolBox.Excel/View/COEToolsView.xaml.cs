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
using ToolBox.Common;

namespace ToolBox.COE.View
{
    /// <summary>
    /// Interaction logic for DGToolsView.xaml
    /// </summary>
    public partial class COEToolsView : UserControl, IApplicationView
    {
        public COEToolsView()
        {
            InitializeComponent();
        }

        public string ViewName
        {
            get
            {
                return "COEToolsView";
            }
        }
    }
}
