using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Backbone.Tool.Controllers;
using ToolBox.Backbone.Tool.View;
using ToolBox.Common;

namespace ToolBox.Backbone.Tool
{
    public class ToolModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private ToolRegionController _toolRegionController;

        public ToolModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => this.container.Resolve<ToolView>());
            this._toolRegionController = this.container.Resolve<ToolRegionController>();
        }
    }
}
