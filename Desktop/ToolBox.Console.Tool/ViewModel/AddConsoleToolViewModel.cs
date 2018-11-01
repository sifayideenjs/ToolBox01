using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Common;
using ToolBox.DataAccess.Repositories;
using ToolBox.Models;

namespace ToolBox.ConsoleTool.ViewModel
{
    public class AddConsoleToolViewModel : ViewModelBase
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        private CenterOfExcellence _centerOfExcellence = null;
        private string _toolName = string.Empty;
        private string _shortDescription = string.Empty;
        private string _version = string.Empty;

        public AddConsoleToolViewModel(IUnityContainer container,
                                    IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            AddToolCommand = new RelayCommand(ExecuteAddToolCommand, CanExecuteAddToolCommand);
        }

        public void SetCenterOfExcellence(CenterOfExcellence coe)
        {
            _centerOfExcellence = coe;
        }

        public string ToolName
        {
            get
            {
                return _toolName;
            }
            set
            {
                _toolName = value;
                RaisePropertyChanged("ToolName");
            }
        }

        public string ShortDescription
        {
            get
            {
                return _shortDescription;
            }
            set
            {
                _shortDescription = value;
                RaisePropertyChanged("ShortDescription");
            }
        }

        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                RaisePropertyChanged("Version");
            }
        }

        public RelayCommand AddToolCommand { get; private set; }

        private void ExecuteAddToolCommand()
        {
            if (_centerOfExcellence == null) return;

            Tile tile = new Tile() { Name = this.ToolName, View = "ToolView", ShortDescription = this.ShortDescription };
            _centerOfExcellence.Hub.HubSections.First().Tiles.Add(tile);

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Update(_centerOfExcellence);
            repository.Save();

            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            var toolItemSelectionEvent = eventAggregator.GetEvent<ToolItemSelectionEvent>();
            toolItemSelectionEvent.Publish(tile);
        }

        private bool CanExecuteAddToolCommand()
        {
            if (string.IsNullOrEmpty(_toolName) | string.IsNullOrEmpty(_shortDescription) | string.IsNullOrEmpty(_version)) return false;
            else return true;
        }
    }
}