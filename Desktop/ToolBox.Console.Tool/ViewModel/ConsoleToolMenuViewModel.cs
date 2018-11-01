using Backbone.Common.Parser;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolBox.Common;
using ToolBox.ConsoleTool.View;
using ToolBox.Models;

namespace ToolBox.ConsoleTool.ViewModel
{
    public class ConsoleToolMenuViewModel : ViewModelBase
    {
        private IUnityContainer _container = null;
        private IRegionManager _regionManager = null;
        private Tile _tile = null;

        public ConsoleToolMenuViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
            COEToolsCommand = new RelayCommand(ExecuteCOEToolsCommand);
            ConsoleConfigCommand = new RelayCommand(ExecuteConsoleConfigCommand, CanExecuteConsoleConfigCommand);
            ConsoleRunCommand = new RelayCommand(ExecuteConsoleRunCommand, CanExecuteConsoleRunCommand);
            ToolRunCommand = new RelayCommand(ExecuteToolRunCommand, CanExecuteToolRunCommand);
        }

        public void SetTile(Tile tile)
        {
            _tile = tile;
        }

        public RelayCommand COEToolsCommand { get; private set; }

        private void ExecuteCOEToolsCommand()
        {
            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            var activateCOEEvent = eventAggregator.GetEvent<ActivateCOE>();
            activateCOEEvent.Publish(string.Empty);
        }

        public RelayCommand ConsoleConfigCommand { get; private set; }

        private void ExecuteConsoleConfigCommand()
        {
            IRegion mainRegion = this._regionManager.Regions[RegionNames.MainRegion];
            ClearRegion(mainRegion);
            ConsoleConfigView view = this._container.Resolve<ConsoleConfigView>();
            var viewModel = this._container.Resolve<ConsoleConfigViewModel>();
            viewModel.SetCurrentTile(_tile);
            view.DataContext = viewModel;
            mainRegion.Add(view);
        }

        private bool CanExecuteConsoleConfigCommand()
        {
            if (_tile == null) return false;
            else return true;
        }

        public RelayCommand ConsoleRunCommand { get; private set; }

        private void ExecuteConsoleRunCommand()
        {
            IRegion mainRegion = this._regionManager.Regions[RegionNames.MainRegion];
            ClearRegion(mainRegion);
            ConsoleToolView view = this._container.Resolve<ConsoleToolView>();
            var viewModel = this._container.Resolve<ConsoleControlViewModel>();
            viewModel.SetRuleSet(_tile);
            view.DataContext = viewModel;
            mainRegion.Add(view);
        }

        private bool CanExecuteConsoleRunCommand()
        {
            if (_tile == null) return false;
            else return true;
        }

        public RelayCommand ToolRunCommand { get; private set; }

        private void ExecuteToolRunCommand()
        {
            //var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            //var backboneToolEvent = eventAggregator.GetEvent<BackboneToolEvent>();
            //backboneToolEvent.Publish(_tile);

            IRegion mainRegion = this._regionManager.Regions[RegionNames.MainRegion];
            ClearRegion(mainRegion);
            ConsoleBackboneToolView view = this._container.Resolve<ConsoleBackboneToolView>();
            var viewModel = this._container.Resolve<ConsoleControlViewModel>();
            viewModel.SetRuleSet(_tile);
            view.DataContext = viewModel;
            mainRegion.Add(view);
        }

        private bool CanExecuteToolRunCommand()
        {
            if (_tile == null) return false;
            else return true;
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
