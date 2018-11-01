using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ToolBox.Common.Converters
{
    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            DependencyObject item = (DependencyObject)value;
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(item);
            int index = itemsControl.ItemContainerGenerator.IndexFromContainer(item) + 1;
            return index.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
