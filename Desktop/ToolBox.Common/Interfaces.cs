using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Common
{
    //Interface for identifying the Views
    public interface IApplicationView
    {
        string ViewName { get; }
    }
}
