using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.Model
{
    public class BackboneForm
    {
        public BackboneForm()
        {
            Properties = new List<BackboneProperty>();
        }


        public String Name { get; set; }
        public String Category { get; set; }
        public List<BackboneProperty> Properties { get; set; }
    }
}
