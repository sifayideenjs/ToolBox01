using Backbone.Common.Commands;
using Backbone.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Backbone.Common.ViewModel
{
    public class BackboneBrowseFileViewModel : BackbonePropertyViewModel<BackboneBrowseFile>
    {
        public BackboneBrowseFileViewModel(BackboneBrowseFile backboneBrowseFile, BackboneProperty propertySettings) : base(backboneBrowseFile, propertySettings)
        {
            backboneBrowseFile.ViewModel = this;

            BrowseCommand = new RelayCommand(Browse, CanBrowse);
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

        public ICommand BrowseCommand { get; set; }

        private void Browse()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = Model.FileExtension;
            ofd.Filter = string.Format("{0}({1})|*{1}", Model.FilterName, Model.FileExtension);
            ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Nullable<bool> result = ofd.ShowDialog();
            if (result != true)
            {
                return;
            }

            Value = ofd.FileName;
        }

        private bool CanBrowse()
        {
            return true;
        }
    }
}
