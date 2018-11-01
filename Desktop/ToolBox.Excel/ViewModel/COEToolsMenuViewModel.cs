using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolBox.COE.View;
using ToolBox.Common;
using ToolBox.Models;

namespace ToolBox.COE.ViewModel
{
    public class COEToolsMenuViewModel : ViewModelBase
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private CenterOfExcellence _currentCOE = null;

        public COEToolsMenuViewModel(IUnityContainer container,
                                    IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            ConfigCommand = new RelayCommand(ExecuteConfigCommand);
        }

        public void SetCenterOfExcellence(CenterOfExcellence coe)
        {
            this._currentCOE = coe;
        }

        public RelayCommand ConfigCommand { get; private set; }

        private void ExecuteConfigCommand()
        {
            var addToolEvent = eventAggregator.GetEvent<AddToolEvent>();
            addToolEvent.Publish(this._currentCOE);
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