using ToolBox.Common;
using ToolBox.Common.Models;
using ToolBox.Excel.View;
using ToolBox.Excel.ViewModel;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Excel.Controllers
{
    public class ExcelRegionController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public ExcelRegionController(IUnityContainer container,
                                    IRegionManager regionManager,
                                    IEventAggregator eventAggregator)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<CompMatrixItemSelectionEvent>().Subscribe(this.OnCompMatrixItemSelected, true);
        }

        private void OnCompMatrixItemSelected(Tile tile)
        {
            if (tile == null) return;
            if (tile.Type != TileType.COE) return;
            if (string.IsNullOrEmpty(tile.View)) return;
            if (string.IsNullOrEmpty(tile.Url)) return;

            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion == null) return;

            IViewsCollection views = mainRegion.ActiveViews;
            foreach(var v in views)
            {
                mainRegion.Remove(v);
            }

            string viewName = tile.View;
            if(viewName == "SearchView")
            {
                SearchView view = this.container.Resolve<SearchView>();
                SearchViewModel viewModel = this.container.Resolve<SearchViewModel>();
                viewModel.CompMatrixFileName = tile.Url;
                view.DataContext = viewModel;
                mainRegion.Add(view, tile.View);
            }
            else if(viewName == "BuildingDetailView")
            {
                BuildingDetailView view = this.container.Resolve<BuildingDetailView>();
                SearchViewModel viewModel = this.container.Resolve<SearchViewModel>();
                viewModel.CompMatrixFileName = tile.Url;
                view.DataContext = viewModel;
                mainRegion.Add(view, tile.View);
            }            
        }
    }
}
