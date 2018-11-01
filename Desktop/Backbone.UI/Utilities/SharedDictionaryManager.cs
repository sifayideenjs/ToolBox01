using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Backbone.Common.UI.Utilities
{
    internal static class SharedDictionaryManager
    {
        internal static ResourceDictionary SharedDictionary
        {
            get
            {
                if (_sharedDictionary == null)
                {
                    System.Uri resourceLocater = new System.Uri("/Backbone.Common.UI;component/Themes/Generic.xaml", System.UriKind.Relative);
                    _sharedDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
                }
                return _sharedDictionary;
            }
        }

        private static ResourceDictionary _sharedDictionary;
    }
}
