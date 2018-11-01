using ToolBox.Common;
using ToolBox.Common.Models;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Common.Utilities;
using ToolBox.Models;
using ToolBox.DataAccess;
using ToolBox.DataAccess.Repositories;

namespace ToolBox.Dashboard.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IUnityContainer _container;
        private ObservableCollection<CenterOfExcellence> _centerOfExcellenceList = new ObservableCollection<CenterOfExcellence>();

        public DashboardViewModel(IUnityContainer container)
        {
            _container = container;
            SelectCommand = new RelayCommand<CenterOfExcellence>(ExecuteSelectCommand);
            
            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            var coeList = repository.Get();
            CenterOfExcellenceList = new ObservableCollection<CenterOfExcellence>(coeList);
        }

        public ObservableCollection<CenterOfExcellence> CenterOfExcellenceList
        {
            get { return _centerOfExcellenceList; }
            set
            {
                _centerOfExcellenceList = value;
                RaisePropertyChanged("CenterOfExcellenceList");
            }
        }

        public RelayCommand<CenterOfExcellence> SelectCommand { get; private set; }

        private void ExecuteSelectCommand(CenterOfExcellence coe)
        {
            if(coe != null)
            {
                var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                var dashboardSelectionEvent = eventAggregator.GetEvent<COEItemSelectionEvent>();
                dashboardSelectionEvent.Publish(coe);
            }
        }
    }
}
