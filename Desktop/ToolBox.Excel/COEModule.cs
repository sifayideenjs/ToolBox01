using ToolBox.Common;
using ToolBox.COE.Controllers;
using ToolBox.COE.View;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.COE
{
    public class COEModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private COEToolsRegionController _excelRegionController;

        public COEModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            //this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => this.container.Resolve<DGToolsView>());
            this.regionManager.RegisterViewWithRegion(RegionNames.SubMenuRegion, () => this.container.Resolve<COEToolsSubMenuView>());
            this.regionManager.RegisterViewWithRegion(RegionNames.AppBarRegion, () => this.container.Resolve<COEToolsAppBarView>());
            this._excelRegionController = this.container.Resolve<COEToolsRegionController>();
        }
    }
}
