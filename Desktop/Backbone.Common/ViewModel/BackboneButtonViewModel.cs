using Backbone.Common.Commands;
using Backbone.Common.Interface;
using Backbone.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Backbone.Common.ViewModel
{
    public class BackboneButtonViewModel : BackbonePropertyViewModel<BackboneButton>
    {
        public BackboneButtonViewModel(BackboneButton backboneButton, BackboneProperty propertySettings) : base(backboneButton, propertySettings)
        {
            backboneButton.ViewModel = this;
            Dependencies = new List<IBackbonePropertyViewModel>();
            ParameterDictionary = new Dictionary<string, string>();

            ButtonCommand = new RelayCommand(Excecute, CanExecute);
        }

        public new string Value
        {
            get
            {
                return Model.Value;
            }
            set
            {
                Model.Value = value;
                NotifyPropertyChanged("Value");
            }
        }

        public List<IBackbonePropertyViewModel> Dependencies
        {
            get; set;
        }

        public Dictionary<string, string> ParameterDictionary
        {
            get;
            private set;
        }

        public ICommand ButtonCommand { get; set; }

        public ICommand ExecuteCommand { get; set; }

        private void Excecute()
        {
            foreach(var dependent in Dependencies)
            {
                string parameterName = dependent.PropertySettings.ParameterName;
                PropertyType propertyType = dependent.PropertySettings.PropertyType;
                string value = string.Empty;

                switch (propertyType)
                {
                    case Common.PropertyType.STRING:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneString;
                            value = propertyObject.Value;
                            break;
                        }
                    case Common.PropertyType.DOUBLE:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneDouble;
                            value = propertyObject.Value.ToString();
                            break;
                        }
                    case Common.PropertyType.BROWSEFILE:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneBrowseFile;
                            value = propertyObject.Value.ToString();
                            break;
                        }
                    case Common.PropertyType.BROWSEFOLDER:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneBrowseFolder;
                            value = propertyObject.Value.ToString();
                            break;
                        }
                    case Common.PropertyType.BUTTON:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneButton;
                            value = propertyObject.Value.ToString();
                            break;
                        }
                }

                this.ParameterDictionary.Add(parameterName, value);
            }

            if (ExecuteCommand != null) ExecuteCommand.Execute(this.ParameterDictionary);
        }

        private bool CanExecute()
        {
            bool canExecute = true;
            foreach (var dependent in Dependencies)
            {
                PropertyType propertyType = dependent.PropertySettings.PropertyType;
                string value = string.Empty;

                switch (propertyType)
                {
                    case Common.PropertyType.STRING:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneString;
                            value = propertyObject.Value;
                            break;
                        }
                    case Common.PropertyType.DOUBLE:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneDouble;
                            value = propertyObject.Value.ToString();
                            break;
                        }
                    case Common.PropertyType.BROWSEFILE:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneBrowseFile;
                            value = propertyObject.Value.ToString();
                            break;
                        }
                    case Common.PropertyType.BROWSEFOLDER:
                        {
                            var propertyObject = dependent.PropertySettings.PropertyObject as BackboneBrowseFolder;
                            value = propertyObject.Value.ToString();
                            break;
                        }
                }

                if (string.IsNullOrEmpty(value)) canExecute = false;
            }

            return canExecute;
        }
    }
}