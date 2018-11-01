using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using ToolBox.Common;
using ToolBox.ConsoleTool.Controllers;
using ToolBox.ConsoleTool.View;

namespace ToolBox.ConsoleTool
{
    public class ConsoleToolModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private ConsoleToolRegionController _consoleToolRegionController;

        public ConsoleToolModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => this.container.Resolve<ConsoleToolView>());
            this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => this.container.Resolve<ConsoleBackboneToolView>());
            this.regionManager.RegisterViewWithRegion(RegionNames.SubMenuRegion, () => this.container.Resolve<ConsoleToolSubMenuView>());
            this.regionManager.RegisterViewWithRegion(RegionNames.AppBarRegion, () => this.container.Resolve<ConsoleToolAppBarView>());
            this._consoleToolRegionController = this.container.Resolve<ConsoleToolRegionController>();
        }
    }
}
