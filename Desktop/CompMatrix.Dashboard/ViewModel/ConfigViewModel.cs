using ToolBox.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ToolBox.Dashboard.ViewModel
{
    public class ConfigViewModel : ViewModelBase
    {
        #region Fields
        private HttpClient httpClient;
        private string baseAddress = ConfigurationManager.AppSettings.Get("EndPoint");

        private string _configFilePath = string.Empty;
        private string _compMatrixFilePath = string.Empty;
        private string _buildDetailFilePath = string.Empty;
        #endregion //Fields

        #region Constructor
        public ConfigViewModel()
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress) };

            BrowseConfigFileCommand = new RelayCommand(ExecuteBrowseConfigFileCommand);
            UploadConfigCommand = new RelayCommand(ExecuteUploadConfigCommand, CanExecuteUploadConfigCommand);

            BrowseCompMatrixFileCommand = new RelayCommand(ExecuteBrowseCompMatrixFileCommand);
            UploadCompMatrixCommand = new RelayCommand(ExecuteUploadCompMatrixCommand, CanExecuteUploadCompMatrixCommand);

            BrowseBuildDetailFileCommand = new RelayCommand(ExecuteBrowseBuildDetailFileCommand);
            UploadBuildDetailCommand = new RelayCommand(ExecuteUploadBuildDetailCommand, CanExecuteUploadBuildDetailCommand);
        }
        #endregion //Constructor

        #region Config
        public string ConfigFilePath
        {
            get { return _configFilePath; }
            set
            {
                if (_configFilePath != value)
                {
                    _configFilePath = value;
                }
                RaisePropertyChanged("ConfigFilePath");
            }
        }

        public RelayCommand BrowseConfigFileCommand { get; set; }

        public RelayCommand UploadConfigCommand { get; private set; }

        private void ExecuteBrowseConfigFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Config file (*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.ConfigFilePath = openFileDialog.FileName;
            }
        }

        private bool CanExecuteUploadConfigCommand()
        {
            if (string.IsNullOrEmpty(_configFilePath)) return false;
            else return true;
        }

        private async void ExecuteUploadConfigCommand()
        {
            await UploadFile("api/config/upload");
        }
        #endregion //Config

        #region CompMatrix
        public string CompMatrixFilePath
        {
            get { return _compMatrixFilePath; }
            set
            {
                if (_compMatrixFilePath != value)
                {
                    _compMatrixFilePath = value;
                }
                RaisePropertyChanged("CompMatrixFilePath");
            }
        }

        public RelayCommand BrowseCompMatrixFileCommand { get; set; }

        public RelayCommand UploadCompMatrixCommand { get; private set; }

        private void ExecuteBrowseCompMatrixFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Comp Matrix file (*.xlsx)|*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.CompMatrixFilePath = openFileDialog.FileName;
            }
        }

        private bool CanExecuteUploadCompMatrixCommand()
        {
            if (string.IsNullOrEmpty(_compMatrixFilePath)) return false;
            else return true;
        }

        private async void ExecuteUploadCompMatrixCommand()
        {
            await UploadFile("api/compmatrix/upload");
        }
        #endregion //CompMatrix

        #region BuildDetail
        public string BuildDetailFilePath
        {
            get { return _buildDetailFilePath; }
            set
            {
                if (_buildDetailFilePath != value)
                {
                    _buildDetailFilePath = value;
                }
                RaisePropertyChanged("BuildDetailFilePath");
            }
        }

        public RelayCommand BrowseBuildDetailFileCommand { get; set; }

        public RelayCommand UploadBuildDetailCommand { get; private set; }

        private void ExecuteBrowseBuildDetailFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Building Details file (*.xlsx)|*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.BuildDetailFilePath = openFileDialog.FileName;
            }
        }

        private bool CanExecuteUploadBuildDetailCommand()
        {
            if (string.IsNullOrEmpty(_buildDetailFilePath)) return false;
            else return true;
        }

        private async void ExecuteUploadBuildDetailCommand()
        {
            await UploadFile("api/buildingdetails/upload");
        }
        #endregion //CompMatrix

        #region WebAPI
        public async Task UploadFile(string address)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(_configFilePath);
                var requestContent = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileInfo.OpenRead());
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    Name = "\"file\"",
                    FileName = "\"" + fileInfo.Name + "\""
                };
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MimeMapping.GetMimeMapping(fileInfo.Name));
                requestContent.Add(fileContent);

                HttpResponseMessage responseMessage = await httpClient.PostAsync(address, requestContent);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return;
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(responseMessage.Content.ReadAsAsync(typeof(string)).Result.ToString());
                }
                else
                {
                    throw new Exception(responseMessage.Content.ReadAsAsync(typeof(string)).Result.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion //WebAPI
    }
}
