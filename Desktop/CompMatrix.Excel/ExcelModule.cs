using ToolBox.Common;
using ToolBox.Excel.Controllers;
using ToolBox.Excel.View;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Excel
{
    public class ExcelModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private ExcelRegionController _excelRegionController;

        public ExcelModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            //this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => this.container.Resolve<SearchView>());
            this._excelRegionController = this.container.Resolve<ExcelRegionController>();
        }
    }
}
