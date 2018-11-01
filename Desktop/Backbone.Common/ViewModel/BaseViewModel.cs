using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.ViewModel
{
    public class BaseViewModel<T> : INotifyPropertyChanged
    {
        private T _model;

        public BaseViewModel(T model)
        {
            this._model = model;
            if (model == null) return;
        }

        public T Model
        {
            get { return _model; }
            set
            {
                if (Equals(_model, value)) return;
                _model = value;
                NotifyPropertyChanged("");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
