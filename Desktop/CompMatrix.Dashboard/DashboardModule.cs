using ToolBox.Common;
using ToolBox.Dashboard.Controllers;
using ToolBox.Dashboard.View;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Dashboard
{
    public class DashboardModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private DashboardRegionController _dashboardRegionController;

        public DashboardModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => this.container.Resolve<DashboardView>());
            this.regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, () => this.container.Resolve<MenuView>());
            this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => this.container.Resolve<ConfigView>());
            this._dashboardRegionController = this.container.Resolve<DashboardRegionController>();
        }
    }
}
