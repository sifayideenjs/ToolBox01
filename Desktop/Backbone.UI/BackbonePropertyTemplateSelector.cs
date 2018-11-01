using System;
using Backbone.Common.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Backbone.Common.Model;
using Backbone.Common.ViewModel;
using System.ComponentModel;
using Backbone.Common.Interface;

namespace Backbone.Common.UI
{
    public class BackbonePropertyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BackboneReadOnlyTemplate { get; set; }

        public DataTemplate BackboneStringTemplate { get; set; }

        public DataTemplate BackboneDoubleTemplate { get; set; }

        public DataTemplate BackboneBrowseFileTemplate { get; set; }

        public DataTemplate BackboneBrowseFolderTemplate { get; set; }

        public DataTemplate BackboneBooleanTemplate { get; set; }

        public DataTemplate BackboneEnumTemplate { get; set; }

        public DataTemplate BackboneComboBoxTemplate { get; set; }
        
        public DataTemplate BackboneButtonTemplate { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var minfo = item as IBackbonePropertyViewModel;

            var dataTemplate = minfo.PropertyType.Match<object, DataTemplate>()
            .IfEquals(typeof(BackboneString), _ => BackboneStringTemplate)
            .IfEquals(typeof(BackboneDouble), _ => BackboneDoubleTemplate)
            .IfEquals(typeof(BackboneBrowseFile), _ => BackboneBrowseFileTemplate)
            .IfEquals(typeof(BackboneBrowseFolder), _ => BackboneBrowseFolderTemplate)
            .IfEquals(typeof(BackboneButton), _ => BackboneButtonTemplate)
            .Build(_ => BackboneReadOnlyTemplate);

            return dataTemplate;
        }
    }
}
