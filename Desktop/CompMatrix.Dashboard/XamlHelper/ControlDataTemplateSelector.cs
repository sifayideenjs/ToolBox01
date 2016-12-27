using ToolBox.Common;
using ToolBox.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ToolBox.Dashboard.XamlHelper
{
    public class ControlDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate COEItemTemplate { get; set; }
        public DataTemplate FavouriteItemTemplate { get; set; }
        public DataTemplate NewsItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate result = COEItemTemplate;

            if (item == null) return result;

            Tile tile = item as Tile;
            if (tile == null) return result;

            switch(tile.Type)
            {
                case TileType.COE:
                    {
                        result = COEItemTemplate;
                        break;
                    }
                case TileType.Favourite:
                    {
                        result = FavouriteItemTemplate;
                        break;
                    }
                case TileType.News:
                    {
                        result = NewsItemTemplate;
                        break;
                    }
            }

            return result;
        }
    }
}
