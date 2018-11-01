using ToolBox.Common;
using ToolBox.Dashboard.View;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Dashboard.ViewModel;

namespace ToolBox.Dashboard.Controllers
{
    public class DashboardRegionController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public DashboardRegionController(IUnityContainer container,
                                        IRegionManager regionManager,
                                        IEventAggregator eventAggregator)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<HomeEvent>().Subscribe(this.OnHomeSelected, true);
        }

        private void OnHomeSelected(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion != null)
            {
                ClearRegion(mainRegion);
                DashboardView view = this.container.Resolve<DashboardView>();
                mainRegion.Add(view, view.ViewName);
            }

            IRegion subMenuRegion = this.regionManager.Regions[RegionNames.SubMenuRegion];
            if (subMenuRegion != null)
            {
                ClearRegion(subMenuRegion);
                DashboardSubMenuView subMenuView = this.container.Resolve<DashboardSubMenuView>();
                var viewModel = this.container.Resolve<DashboardMenuViewModel>();
                subMenuView.DataContext = viewModel;
                subMenuRegion.Add(subMenuView);
            }

            IRegion appBarRegion = this.regionManager.Regions[RegionNames.AppBarRegion];
            if (appBarRegion != null)
            {
                ClearRegion(appBarRegion);
                DashboardAppBarView appBarView = this.container.Resolve<DashboardAppBarView>();
                var viewModel = this.container.Resolve<DashboardMenuViewModel>();
                appBarView.DataContext = viewModel;
                appBarRegion.Add(appBarView);
            }
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