using ToolBox.Common;
using ToolBox.Common.Models;
using ToolBox.Excel.Utilities;
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

namespace ToolBox.Dashboard.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IUnityContainer _container;
        private HttpClient httpClient;
        private string addressSuffix = "api/config/";
        private string baseAddress = ConfigurationManager.AppSettings.Get("EndPoint");
        private Hub _hub = null;

        public DashboardViewModel(IUnityContainer container)
        {
            _container = container;
            httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress) };
            SelectCommand = new RelayCommand<Tile>(ExecuteSelectCommand);

            Tile tile01 = new Tile() { Name = "FMS", View = "SearchView", Type = TileType.COE, Url = "CompMatrix2015" };
            Tile tile02 = new Tile() { Name = "CNS", View = "SearchView", Type = TileType.COE, Url = "CompMatrix2016" };
            Tile tile03 = new Tile() { Name = "ARRT", View = "SearchView", Type = TileType.COE };
            Tile tile04 = new Tile() { Name = "EPC", View = "SearchView", Type = TileType.COE, Url = "CompMatrix2015" };
            Tile tile05 = new Tile() { Name = "D & G", View = "SearchView", Type = TileType.COE, Url = "CompMatrix2016" };
            Tile tile06 = new Tile() { Name = "NG", View = "SearchView", Type = TileType.COE };

            HubSection hubSection01 = new HubSection();
            hubSection01.Name = "COE's";
            hubSection01.Tiles.AddRange(new List<Tile> { tile01, tile02, tile03, tile04, tile05, tile06 });

            Tile tile07 = new Tile() { Name = "Simplify", Type = TileType.Favourite, ShortDescription = "This is Simplify tool for FMS" };
            Tile tile08 = new Tile() { Name = "WFM", Type = TileType.Favourite, ShortDescription = "This is Work Flow Management tool" };
            Tile tile09 = new Tile() { Name = "Common IO", Type = TileType.Favourite, ShortDescription = "This is Common IO tool for FMS" };

            HubSection hubSection02 = new HubSection();
            hubSection02.Name = "Favourites";
            hubSection02.Tiles.AddRange(new List<Tile> { tile07, tile08, tile09 });

            Tile tile13 = new Tile() { Name = "Microsoft", Type = TileType.News, Description = "Microsoft is in the news today" };
            Tile tile14 = new Tile() { Name = "Comp Matrix", Type = TileType.News, Description = "Search Comp Matrix for some usefull stuff" };
            Tile tile15 = new Tile() { Name = "Chennai", Type = TileType.News, Description = "Chennai is again in the picture" };

            HubSection hubSection03 = new HubSection();
            hubSection03.Name = "News & Updates";
            hubSection03.Tiles.AddRange(new List<Tile> { tile13, tile14, tile15 });

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01, hubSection02, hubSection03 });

            Configs configs = new Configs() { Hub = hub };

            //Configs configs = GetConfig().Result;
            _container.RegisterInstance<Configs>(configs);
            this.Hub = configs.Hub;
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
            if(tile != null)
            {
                switch(tile.Type)
                {
                    case TileType.COE:
                        {
                            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                            var dashboardSelectionEvent = eventAggregator.GetEvent<CompMatrixItemSelectionEvent>();
                            dashboardSelectionEvent.Publish(tile);
                            break;
                        }
                    case TileType.Favourite:
                        {
                            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                            var dashboardSelectionEvent = eventAggregator.GetEvent<CompMatrixItemSelectionEvent>();
                            dashboardSelectionEvent.Publish(tile);
                            break;
                        }
                    case TileType.News:
                        {
                            break;
                        }
                }
            }
        }

        public async Task<Configs> GetConfig()
        {
            try
            {
                HttpResponseMessage responseMessage = httpClient.GetAsync(addressSuffix).Result;
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string serializedContent = await responseMessage.Content.ReadAsStringAsync();
                    return serializedContent.JSONDeSerialize<Configs>();
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(responseMessage.Content.ReadAsAsync(typeof(string)).Result.ToString());
                }
                else
                {
                    throw new Exception(responseMessage.Content.ReadAsAsync(typeof(string)).Result.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
