using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.DataAccess
{
    class ToolBoxContextSingleton
    {
        private static readonly ToolBoxContext instance = new ToolBoxContext();

        static ToolBoxContextSingleton() { }

        private ToolBoxContextSingleton() { }

        public static ToolBoxContext Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
