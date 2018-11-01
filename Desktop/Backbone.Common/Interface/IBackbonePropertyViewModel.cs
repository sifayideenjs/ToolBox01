using Backbone.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Backbone.Common.Interface
{
    public interface IBackbonePropertyViewModel : INotifyPropertyChanged
    {
        BackboneProperty PropertySettings { get; set; }
        Type PropertyType { get; }
        DataTemplate Template { get; set; }
        UserVisibility FieldVisibility { get; }
    }
}
