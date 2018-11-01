using Backbone.Common.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace Backbone.Common.Interface
{
    public interface IBackboneClassViewModel : INotifyPropertyChanged
    {
        string Category { get; }
        string Name { get; }
        List<IBackbonePropertyViewModel> Properties { get; }
    }
}