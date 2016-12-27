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
            this.eventAggregator.GetEvent<ConfigEvent>().Subscribe(this.OnConfigSelected, true);
        }

        private void OnHomeSelected(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion == null) return;

            IViewsCollection views = mainRegion.ActiveViews;
            foreach (var v in views)
            {
                mainRegion.Remove(v);
            }

            DashboardView view = this.container.Resolve<DashboardView>();
            mainRegion.Add(view, view.ViewName);
        }

        private void OnConfigSelected(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion == null) return;

            IViewsCollection views = mainRegion.ActiveViews;
            foreach (var v in views)
            {
                mainRegion.Remove(v);
            }

            ConfigView view = this.container.Resolve<ConfigView>();
            mainRegion.Add(view, view.ViewName);
        }
    }
}