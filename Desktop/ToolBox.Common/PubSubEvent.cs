using ToolBox.Common.Models;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.Common
{
    public class COEItemSelectionEvent : PubSubEvent<CenterOfExcellence>
    {
    }

    public class AddToolEvent : PubSubEvent<CenterOfExcellence>
    {
    }

    public class ToolItemSelectionEvent : PubSubEvent<Tile>
    {
    }

    public class ActivateCOE : PubSubEvent<string>
    {
    }

    public class HomeEvent : PubSubEvent<string>
    {
    }

    public class BackboneToolEvent : PubSubEvent<Tile>
    {
    }
}
