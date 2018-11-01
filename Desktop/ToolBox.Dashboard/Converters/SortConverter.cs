using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ToolBox.Dashboard.Converters
{
    public class SortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Collections.IList collection = value as System.Collections.IList;

            if (collection != null)
            {
                ListCollectionView view = new ListCollectionView(collection);
                SortDescription sort = new SortDescription(parameter.ToString(), ListSortDirection.Ascending);
                view.SortDescriptions.Add(sort);
                return view;
            }

            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
