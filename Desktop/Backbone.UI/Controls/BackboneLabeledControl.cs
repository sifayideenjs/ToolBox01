using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Backbone.Common.UI.Controls
{
    public class BackboneLabeledControl : HeaderedContentControl
    {
        static BackboneLabeledControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BackboneLabeledControl), new FrameworkPropertyMetadata(typeof(BackboneLabeledControl)));
        }
    }
}
