using Backbone.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.ViewModel
{
    public class BackboneDoubleViewModel : BackbonePropertyViewModel<BackboneDouble>
    {
        public BackboneDoubleViewModel(BackboneDouble backboneDouble, BackboneProperty propertySettings) : base(backboneDouble, propertySettings)
        {
            backboneDouble.ViewModel = this;
        }

        public new Double? Value
        {
            get
            {
                return Model.Value;
            }
            set
            {
                Model.Value = value;
                NotifyPropertyChanged("Value");
            }
        }
    }
}
