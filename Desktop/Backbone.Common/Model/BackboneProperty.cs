using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.Model
{
    public class BackboneProperty
    {
        public String Name { get; set; }
        public String ChildName { get; set; }
        public String DisplayName { get; set; }
        public Int32 DisplayIndex { get; set; }
        public UserVisibility FieldVisibility { get; set; }
        public PropertyType PropertyType { get; set; }
        public Boolean Required { get; set; }
        public Boolean IsEditable { get; set; }
        public object PropertyObject { get; set; }
        public object DependencyObject { get; set; }
        public string ParameterName { get; set; }
    }
}
