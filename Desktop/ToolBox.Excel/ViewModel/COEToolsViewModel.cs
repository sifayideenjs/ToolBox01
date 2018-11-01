using ToolBox.Common;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using ToolBox.Common.Models;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.PubSubEvents;
using ToolBox.Models;

namespace ToolBox.COE.ViewModel
{
    public class COEToolsViewModel : ViewModelBase
    {
        private readonly IUnityContainer _container;
        private Hub _hub = null;

        public COEToolsViewModel(IUnityContainer container)
        {
            _container = container;
            SelectCommand = new RelayCommand<Tile>(ExecuteSelectCommand);
        }

        public Hub Hub
        {
            get { return _hub; }
            set
            {
                _hub = value;
                RaisePropertyChanged("Hub");
            }
        }

        public RelayCommand<Tile> SelectCommand { get; private set; }

        private void ExecuteSelectCommand(Tile tile)
        {
            if (tile != null)
            {
                var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                var toolItemSelectionEvent = eventAggregator.GetEvent<ToolItemSelectionEvent>();
                toolItemSelectionEvent.Publish(tile);
            }
        }
    }
}