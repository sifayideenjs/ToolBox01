using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ToolBox.Common.Controls
{
    public class AppBar : ItemsControl
    {
        static AppBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppBar), new FrameworkPropertyMetadata(typeof(AppBar)));
        }
    }
}
