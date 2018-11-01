using Backbone.Common.Interface;
using Backbone.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Backbone.Common.ViewModel
{
    public class BackbonePropertyViewModel<T> : BaseViewModel<T>, IBackbonePropertyViewModel
    {
        public BackbonePropertyViewModel(T backboneProperty, BackboneProperty propertySettings) : base(backboneProperty)
        {
            PropertyType = typeof(T);
            PropertySettings = propertySettings;
        }

        public BackbonePropertyViewModel(T backboneProperty, BackboneProperty propertySettings, Action<object> setValueAction = null, Func<object> getValueFunction = null) : this(backboneProperty, propertySettings)
        {
            PropertySettings = propertySettings;
        }
        
        public BackboneProperty PropertySettings { get; set; }
        public DataTemplate Template { get; set; }
        public Type PropertyType { get; set; }
        public Action<object> SetValueAction { get; set; }

        public override string ToString()
        {
            if (Model == null) return string.Empty;

            return Model.ToString();
        }

        #region IBackboneProperty

        public string Name
        {
            get { return PropertySettings.Name; }
        }

        public string ChildName
        {
            get { return PropertySettings.ChildName; }
        }

        public string DisplayName
        {
            get { return PropertySettings.DisplayName; }
        }

        public UserVisibility FieldVisibility
        {
            get { return PropertySettings.FieldVisibility; }
        }

        public int DisplayIndex
        {
            get { return PropertySettings.DisplayIndex; }
        }

        public virtual object Value
        {
            get { return Model; }
            set
            {
                if (value == null)
                    value = default(T);
                if (SetValueAction != null)
                    SetValueAction(value);

                var t = PropertyType;
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                    t = Nullable.GetUnderlyingType(t);
                if (value != null)
                    Model = (T)Convert.ChangeType(value, t);
                else
                    Model = default(T);
                NotifyPropertyChanged("Value");
            }
        }

        #endregion
    }
}
