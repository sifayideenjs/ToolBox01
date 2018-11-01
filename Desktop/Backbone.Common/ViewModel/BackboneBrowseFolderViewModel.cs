using Backbone.Common.Commands;
using Backbone.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Backbone.Common.ViewModel
{
    public class BackboneBrowseFolderViewModel : BackbonePropertyViewModel<BackboneBrowseFolder>
    {
        public BackboneBrowseFolderViewModel(BackboneBrowseFolder backboneBrowseFolder, BackboneProperty propertySettings) : base(backboneBrowseFolder, propertySettings)
        {
            backboneBrowseFolder.ViewModel = this;

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
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                dlg.ShowNewFolderButton = true;
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Value = dlg.SelectedPath;
                }
            }
        }

        private bool CanBrowse()
        {
            return true;
        }
    }
}
