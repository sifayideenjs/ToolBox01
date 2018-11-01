using Backbone.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.Model
{
    public class BackboneDependent
    {
        public BackboneDependent()
        {
        }

        public string Value { get; set; }

        public BackboneDependentViewModel ViewModel { get; set; }
    }
}
