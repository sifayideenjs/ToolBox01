using ToolBox.Common;
using ToolBox.Common.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolBox.Dashboard.View;

namespace ToolBox.Dashboard.ViewModel
{
    public class DashboardMenuViewModel : ViewModelBase
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        
        public DashboardMenuViewModel(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this._container = container;
            this._regionManager = regionManager;
            this._eventAggregator = eventAggregator;

            this.BackToStartPageCommand = new RelayCommand(this.OnBackToStartPageCommand);
            this.ConfigCommand = new RelayCommand(ExecuteConfigCommand);
        }

        #region Commands

        public RelayCommand BackToStartPageCommand { get; private set; }

        private void OnBackToStartPageCommand()
        {
            var homeEvent = _eventAggregator.GetEvent<HomeEvent>();
            homeEvent.Publish("Home");
        }

        public RelayCommand ConfigCommand { get; private set; }

        private void ExecuteConfigCommand()
        {
            IRegion mainRegion = this._regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion != null)
            {
                ClearRegion(mainRegion);
                var view = this._container.Resolve<DashboardConfigView>();
                var viewModel = this._container.Resolve<DashboardConfigViewModel>();
                view.DataContext = viewModel;
                mainRegion.Add(view);
            }

            IRegion subMenuRegion = this._regionManager.Regions[RegionNames.SubMenuRegion];
            if (subMenuRegion != null)
            {
                ClearRegion(subMenuRegion);
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

        #endregion
    }
}

