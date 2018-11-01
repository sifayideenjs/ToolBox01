using ToolBox.Common;
using ToolBox.Common.Models;
using ToolBox.COE.View;
using ToolBox.COE.ViewModel;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.COE.Controllers
{
    public class COEToolsRegionController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private CenterOfExcellence _centerOfExcellence = null;

        public COEToolsRegionController(IUnityContainer container,
                                    IRegionManager regionManager,
                                    IEventAggregator eventAggregator)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<COEItemSelectionEvent>().Subscribe(this.OnCOEItemSelected, true);
            this.eventAggregator.GetEvent<ActivateCOE>().Subscribe(this.OnActivateCOE, true);
        }

        private void OnCOEItemSelected(CenterOfExcellence coe)
        {
            if (coe == null) return;
            //if (coe.Name != "DG") return;

            _centerOfExcellence = coe;

            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion != null)
            {
                ClearRegion(mainRegion);

                if(coe.Hub.HubSections.First().Tiles.Count == 0)
                {
                    var addToolEvent = eventAggregator.GetEvent<AddToolEvent>();
                    addToolEvent.Publish(coe);
                }
                else
                {
                    COEToolsView view = this.container.Resolve<COEToolsView>();
                    COEToolsViewModel viewModel = this.container.Resolve<COEToolsViewModel>();
                    viewModel.Hub = coe.Hub;
                    view.DataContext = viewModel;
                    mainRegion.Add(view);
                }
            }

            IRegion subMenuRegion = this.regionManager.Regions[RegionNames.SubMenuRegion];
            if (subMenuRegion != null)
            {
                ClearRegion(subMenuRegion);
                COEToolsSubMenuView subMenuView = this.container.Resolve<COEToolsSubMenuView>();
                var viewModel = this.container.Resolve<COEToolsMenuViewModel>();
                viewModel.SetCenterOfExcellence(this._centerOfExcellence);
                subMenuView.DataContext = viewModel;
                subMenuRegion.Add(subMenuView);
            }

            IRegion appBarRegion = this.regionManager.Regions[RegionNames.AppBarRegion];
            if (appBarRegion != null)
            {
                ClearRegion(appBarRegion);
                COEToolsAppBarView appBarView = this.container.Resolve<COEToolsAppBarView>();
                var viewModel = this.container.Resolve<COEToolsMenuViewModel>();
                viewModel.SetCenterOfExcellence(this._centerOfExcellence);
                appBarView.DataContext = viewModel;
                appBarRegion.Add(appBarView);
            }
        }

        private void OnActivateCOE(string arg)
        {
            OnCOEItemSelected(_centerOfExcellence);
        }

        private void ClearRegion(IRegion region)
        {
            if (region == null) return;
            IViewsCollection views = region.ActiveViews;
            foreach (var v in views)
            {
                region.Remove(v);
            }
        }
    }
}