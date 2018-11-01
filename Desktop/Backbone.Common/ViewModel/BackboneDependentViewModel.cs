using Backbone.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.ViewModel
{
    public class BackboneDependentViewModel : BackbonePropertyViewModel<BackboneDependent>
    {
        public BackboneDependentViewModel(BackboneDependent backboneDependent, BackboneProperty propertySettings) : base(backboneDependent, propertySettings)
        {
            backboneDependent.ViewModel = this;
        }

        public new string Value
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
