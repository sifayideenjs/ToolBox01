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

namespace ToolBox.ConsoleTool.View
{
    /// <summary>
    /// Interaction logic for ConsoleBackboneToolView.xaml
    /// </summary>
    public partial class ConsoleBackboneToolView : UserControl, IApplicationView
    {
        public ConsoleBackboneToolView()
        {
            InitializeComponent();
        }

        public string ViewName
        {
            get
            {
                return "ConsoleBackboneToolView";
            }
        }
    }
}
