using ToolBox.Common.Models;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Common
{
    public class CompMatrixItemSelectionEvent : PubSubEvent<Tile>
    {
    }

    public class ToolItemSelectionEvent : PubSubEvent<Tile>
    {
    }

    public class ConfigEvent : PubSubEvent<string>
    {
    }

    public class HomeEvent : PubSubEvent<string>
    {
    }
}
