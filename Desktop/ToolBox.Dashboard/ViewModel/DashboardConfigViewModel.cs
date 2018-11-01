using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Common;
using ToolBox.DataAccess.Repositories;
using ToolBox.Models;

namespace ToolBox.Dashboard.ViewModel
{
    class DashboardConfigViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator = null;
        private string _coeName = string.Empty;

        public DashboardConfigViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AddCOECommand = new RelayCommand(ExecuteAddCOECommand, CanExecuteAddCOECommand);
        }

        public string COEName
        {
            get
            {
                return _coeName;
            }
            set
            {
                _coeName = value;
                RaisePropertyChanged("COEName");
            }
        }

        public RelayCommand AddCOECommand { get; private set; }

        private void ExecuteAddCOECommand()
        {
            HubSection hubSection01 = new HubSection();
            hubSection01.Name = string.Format("{0} Tools", this.COEName);

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01 });

            CenterOfExcellence centerOfExcellence = new CenterOfExcellence() { Name = this.COEName, Hub = hub };

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Insert(centerOfExcellence);
            repository.Save();
            
            var dashboardSelectionEvent = _eventAggregator.GetEvent<COEItemSelectionEvent>();
            dashboardSelectionEvent.Publish(centerOfExcellence);
        }

        private bool CanExecuteAddCOECommand()
        {
            if (string.IsNullOrEmpty(_coeName)) return false;
            else return true;
        }
    }
}