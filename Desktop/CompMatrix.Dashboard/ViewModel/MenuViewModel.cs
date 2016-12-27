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

namespace ToolBox.Dashboard.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        private Visibility _uploadVisibility = Visibility.Collapsed;
        private string clientUploadKey = string.Empty;

        public MenuViewModel(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this._container = container;
            this._regionManager = regionManager;
            this._eventAggregator = eventAggregator;

            this.BackToStartPageCommand = new DelegateCommand(this.OnBackToStartPageCommand, this.CanBackToStartPageCommand);
            this.ConfigCommand = new DelegateCommand(this.OnConfigCommand, this.CanConfigCommand);

            clientUploadKey = ConfigurationManager.AppSettings["config:settings:upload:key"];

            var configs = _container.Resolve<Configs>();
            //string serverUploadKey = configs.Settings.UploadKey;

            //if(serverUploadKey == clientUploadKey)
            //{
            //    UploadVisibility = Visibility.Visible;
            //}
        }

        public Visibility UploadVisibility
        {
            get
            {
                return _uploadVisibility;
            }
            set
            {
                _uploadVisibility = value;
                RaisePropertyChanged("UploadVisibility");
            }
        }

        #region ExitCommand

        public DelegateCommand BackToStartPageCommand { get; private set; }

        private void OnBackToStartPageCommand()
        {
            var homeEvent = _eventAggregator.GetEvent<HomeEvent>();
            homeEvent.Publish("Home");
        }

        private bool CanBackToStartPageCommand()
        {
            //IRegion mainRegion = this._regionManager.Regions[RegionNames.MainRegion];
            //if (mainRegion == null) return false;

            //IApplicationView activeView = mainRegion.ActiveViews.FirstOrDefault() as IApplicationView;
            //if (activeView == null) return false;

            //if (activeView.ViewName == "DashboardView") return false;
            //else return true;

            return true;
        }

        #endregion


        #region ConfigsCommand

        public DelegateCommand ConfigCommand { get; private set; }

        private void OnConfigCommand()
        {
            var homeEvent = _eventAggregator.GetEvent<ConfigEvent>();
            homeEvent.Publish(clientUploadKey);
        }

        private bool CanConfigCommand()
        {
            if (UploadVisibility == Visibility.Visible) return true;
            else return true;
        }

        #endregion
    }
}

