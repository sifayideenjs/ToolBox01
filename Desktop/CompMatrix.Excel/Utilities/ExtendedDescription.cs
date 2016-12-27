using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Excel.Utilities
{
    public class ExtendedDescription : Attribute
    {
        public string[] Values { get; set; }

        public ExtendedDescription(params string[] values)
        {
            this.Values = values;
        }
    }
}
