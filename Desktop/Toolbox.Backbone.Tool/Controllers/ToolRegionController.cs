using Backbone.Common.Parser;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Backbone.Tool.View;
using ToolBox.Common;
using ToolBox.Common.Models;
using ToolBox.Models;

namespace ToolBox.Backbone.Tool.Controllers
{
    public class ToolRegionController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public ToolRegionController(IUnityContainer container,
                                    IRegionManager regionManager,
                                    IEventAggregator eventAggregator)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<BackboneToolEvent>().Subscribe(this.OnBackboneToolSelected, true);
        }

        private void OnBackboneToolSelected(Tile tile)
        {
            if (tile == null) return;
            //if (tile.Type != TileType.Tool) return;
            if (string.IsNullOrEmpty(tile.View)) return;

            IRegion mainRegion = this.regionManager.Regions[RegionNames.MainRegion];
            if (mainRegion == null) return;

            ClearRegion(mainRegion);

            string viewName = tile.View;
            if (viewName == "ToolView")
            {
                ToolView view = this.container.Resolve<ToolView>();
                //var viewModel = BackboneXMLParser.GetBackboneFormViewModel(@"C:\Users\h149041\Source\Repos\ToolBox01\ToolForm.xml");
                TextReader textReader = new StringReader(tile.BackBoneXmlString);
                var viewModel = BackboneXMLParser.GetBackboneFormViewModel(textReader);
                viewModel.ExecuteCommand = new RelayCommand<object>(OnExecuteCommand);
                view.DataContext = viewModel;
                mainRegion.Add(view, tile.View);
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

        private void OnExecuteCommand(object param)
        {
            if(param != null && param is Dictionary<string, string>)
            {
                Dictionary<string, string> parameterDictionary = param as Dictionary<string, string>;

            }
        }
    }
}
