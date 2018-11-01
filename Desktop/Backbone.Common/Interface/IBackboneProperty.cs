using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.Interface
{
    public interface IBackboneProperty
    {
        String Name { get; }
        String DisplayName { get; }
        Int32 DisplayIndex { get; }
        UserVisibility FieldVisibility { get; }
        Boolean Required { get; }
        Boolean IsEditable { get; }
        object PropertyObject { get; set; }
    }
}
