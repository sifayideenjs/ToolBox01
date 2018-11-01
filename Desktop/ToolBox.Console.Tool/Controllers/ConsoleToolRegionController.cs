using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Common;
using ToolBox.Common.Models;
using ToolBox.ConsoleTool.DataSet;
using ToolBox.ConsoleTool.View;
using ToolBox.ConsoleTool.ViewModel;
using ToolBox.Models;

namespace ToolBox.ConsoleTool.Controllers
{
    public class ConsoleToolRegionController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        public Tile _currentTile = null;

        public ConsoleToolRegionController(IUnityContainer container,
                                    IRegionManager regionManager,
                                    IEventAggregator eventAggregator)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<ToolItemSelectionEvent>().Subscribe(this.OnToolsItemSelected, true);
            this.eventAggregator.GetEvent<AddToolEvent>().Subscribe(this.OnAddToolEvent, true);
        }

        private void OnToolsItemSelected(Tile tile)
        {
            _currentTile = tile;

            if (tile == null) return;
            //if (tile.Type != TileType.Tool) return;
            if (string.IsNullOrEmpty(tile.View)) return;

            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion != null)
            {
                ClearRegion(mainRegion);

                string viewName = tile.View;
                if (viewName == "ToolView")
                {
                    var ruleSet = tile.RuleSet;
                    if (ruleSet != null)
                    {
                        ConsoleToolView view = this.container.Resolve<ConsoleToolView>();
                        var viewModel = this.container.Resolve<ConsoleControlViewModel>();
                        viewModel.SetRuleSet(_currentTile);
                        view.DataContext = viewModel;
                        mainRegion.Add(view, tile.View);
                    }
                    else
                    {
                        ConsoleConfigView view = this.container.Resolve<ConsoleConfigView>();
                        var viewModel = this.container.Resolve<ConsoleConfigViewModel>();
                        viewModel.SetCurrentTile(_currentTile);
                        view.DataContext = viewModel;
                        mainRegion.Add(view, tile.View);
                    }
                }
            }            

            IRegion subMenuRegion = this.regionManager.Regions[RegionNames.SubMenuRegion];
            if (subMenuRegion != null)
            {
                ClearRegion(subMenuRegion);
                ConsoleToolSubMenuView subMenuView = this.container.Resolve<ConsoleToolSubMenuView>();
                var viewModel = this.container.Resolve<ConsoleToolMenuViewModel>();
                viewModel.SetTile(_currentTile);
                subMenuView.DataContext = viewModel;
                subMenuRegion.Add(subMenuView);
            }

            IRegion appBarRegion = this.regionManager.Regions[RegionNames.AppBarRegion];
            if (appBarRegion != null)
            {
                ClearRegion(appBarRegion);
                ConsoleToolAppBarView appBarView = this.container.Resolve<ConsoleToolAppBarView>();
                var viewModel = this.container.Resolve<ConsoleToolMenuViewModel>();
                viewModel.SetTile(_currentTile);
                appBarView.DataContext = viewModel;
                appBarRegion.Add(appBarView);
            }
        }

        private void OnAddToolEvent(CenterOfExcellence coe)
        {
            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            ClearRegion(mainRegion);
            var view = this.container.Resolve<AddConsoleToolView>();
            var viewModel = this.container.Resolve<AddConsoleToolViewModel>();
            viewModel.SetCenterOfExcellence(coe);
            view.DataContext = viewModel;
            mainRegion.Add(view);

            IRegion subMenuRegion = this.regionManager.Regions[RegionNames.SubMenuRegion];
            if (subMenuRegion != null)
            {
                ClearRegion(subMenuRegion);
                ConsoleToolSubMenuView subMenuView = this.container.Resolve<ConsoleToolSubMenuView>();
                var consoleToolMenuViewModel = this.container.Resolve<ConsoleToolMenuViewModel>();
                consoleToolMenuViewModel.SetTile(_currentTile);
                subMenuView.DataContext = consoleToolMenuViewModel;
                subMenuRegion.Add(subMenuView);
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

