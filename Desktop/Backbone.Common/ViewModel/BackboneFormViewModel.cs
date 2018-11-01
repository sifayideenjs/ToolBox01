using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backbone.Common.Model;
using Backbone.Common.ViewModel;
using Backbone.Common.Interface;
using System.Windows.Input;

namespace Backbone.Common.ViewModel
{
    public class BackboneFormViewModel : BaseViewModel<BackboneForm>, IBackboneClassViewModel
    {
        Dictionary<string, IBackbonePropertyViewModel> mapper = null;
        public BackboneFormViewModel(BackboneForm model) : base(model)
        {
            mapper = new Dictionary<string, IBackbonePropertyViewModel>();
        }

        public string Category
        {
            get { return Model.Category; }
        }

        public string Name
        {
            get { return Model.Name; }
        }

        public ICommand ExecuteCommand { get; set; }

        private List<IBackbonePropertyViewModel> properties;
        public List<IBackbonePropertyViewModel> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new List<IBackbonePropertyViewModel>(Model.Properties.Select(p => WrapBackbonePropertyInViewModel(p)));
                }
                return properties;
            }
        }

        private IBackbonePropertyViewModel WrapBackbonePropertyInViewModel(BackboneProperty propertySettings, Type t = null, Action<object> set = null, Func<object> get = null)
        {
            object propertyObject = propertySettings.PropertyObject;
            Type propertyType = propertyObject.GetType();

            if (propertyType == BackboneType.BackboneStringType)
            {
                var viewModel = ((BackboneString)propertyObject).ViewModel;
                if (viewModel == null) viewModel = new BackboneStringViewModel(propertyObject as BackboneString, propertySettings);
                Map(viewModel.Name, viewModel);
                return viewModel;
            }

            if (propertyType == BackboneType.BackboneDoubleType)
            {
                var viewModel = ((BackboneDouble)propertyObject).ViewModel;
                if (viewModel == null) viewModel = new BackboneDoubleViewModel(propertyObject as BackboneDouble, propertySettings);
                Map(viewModel.Name, viewModel);
                return viewModel;
            }

            if (propertyType == BackboneType.BackboneBrowseFileType)
            {
                var viewModel = ((BackboneBrowseFile)propertyObject).ViewModel;
                if (viewModel == null) viewModel = new BackboneBrowseFileViewModel(propertyObject as BackboneBrowseFile, propertySettings);
                Map(viewModel.Name, viewModel);
                return viewModel;
            }

            if (propertyType == BackboneType.BackboneBrowseFolderType)
            {
                var viewModel = ((BackboneBrowseFolder)propertyObject).ViewModel;
                if (viewModel == null) viewModel = new BackboneBrowseFolderViewModel(propertyObject as BackboneBrowseFolder, propertySettings);
                Map(viewModel.Name, viewModel);
                return viewModel;
            }

            if (propertyType == BackboneType.BackboneButtonType)
            {
                var viewModel = ((BackboneButton)propertyObject).ViewModel;
                if (viewModel == null) viewModel = new BackboneButtonViewModel(propertyObject as BackboneButton, propertySettings);

                List<BackboneDependent> dependencyList = propertySettings.DependencyObject as List<BackboneDependent>;
                var dependentControlNameList = dependencyList.Select(d => d.Value);
                foreach(string ctrlName in dependentControlNameList)
                {
                    var dependentControl = mapper[ctrlName];
                    viewModel.Dependencies.Add(dependentControl);
                }                

                Map(viewModel.Name, viewModel);
                viewModel.ExecuteCommand = ExecuteCommand;
                return viewModel;
            }

            return null;
        }

        private void Map(string key, IBackbonePropertyViewModel value)
        {
            if (mapper == null) mapper = new Dictionary<string, IBackbonePropertyViewModel>();
            bool isExist = mapper.ContainsKey(key);
            if(!isExist)
            {
                mapper.Add(key, value);
            }
        }
    }
}
