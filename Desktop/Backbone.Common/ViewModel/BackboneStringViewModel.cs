using Backbone.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.ViewModel
{
    public class BackboneStringViewModel : BackbonePropertyViewModel<BackboneString>
    {
        public BackboneStringViewModel(BackboneString backboneString, BackboneProperty propertySettings) : base(backboneString, propertySettings)
        {
            //backboneString.ViewModel = this;
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
