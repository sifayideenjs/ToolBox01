using ToolBox.Common;
using ToolBox.Excel.Model;
using ToolBox.Excel.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ToolBox.Excel.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        #region Fields
        private HttpClient httpClient;
        private string addressSuffix = "api/compmatrix/";
        private string baseAddress = ConfigurationManager.AppSettings.Get("EndPoint");

        private string _compMatrixFilePath = string.Empty;
        private string _compMatrixFileName = string.Empty;
        private string _incentivePlan = string.Empty;
        private string _country = string.Empty;
        private ObservableCollection<CompMatrixModel> _compMatrixList;
        private ObservableCollection<CompMatrixModel> _searchList;
        private Visibility _isExcelParsed = Visibility.Hidden;
        private int _tabIndex = 0;
        private string _searchTime;
        private const float _minConfidence = 0.8f;
        private Random rnd = new Random();
        public static string QEvent;
        private DateTime timenow = DateTime.Now;
        #endregion

        #region Constructor
        public SearchViewModel()
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress) };

            InitializeDatas();
            InitializeCommands();
        }
        #endregion Constructor

        #region Properties
        public string CompMatrixFilePath
        {
            get { return _compMatrixFilePath; }
            set
            {
                if(_compMatrixFilePath != value)
                {
                    _compMatrixFilePath = value;
                    //ConfigHelper.UpdateSetting("RecentFile", _compMatrixFilePath);
                }
                RaisePropertyChanged("CompMatrixFilePath");
            }
        }

        public string CompMatrixFileName
        {
            get { return _compMatrixFileName; }
            set
            {
                if (_compMatrixFileName != value)
                {
                    _compMatrixFileName = value;
                }
                RaisePropertyChanged("CompMatrixFileName");
            }
        }

        public string IncentivePlan
        {
            get { return _incentivePlan; }
            set
            {
                _incentivePlan = value;
                RaisePropertyChanged("IncentivePlan");
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                RaisePropertyChanged("Country");
            }
        }

        public ObservableCollection<CompMatrixModel> CompMatrixList
        {
            get { return _compMatrixList; }
            set
            {
                _compMatrixList = value;
                RaisePropertyChanged("CompMatrixList");
            }
        }

        public ObservableCollection<CompMatrixModel> SearchList
        {
            get { return _searchList; }
            set
            {
                _searchList = value;
                RaisePropertyChanged("SearchList");
            }
        }

        public Visibility IsExcelParsed
        {
            get { return _isExcelParsed; }
            set
            {
                _isExcelParsed = value;
                RaisePropertyChanged("IsExcelParsed");
            }
        }

        public int TabIndex
        {
            get { return _tabIndex; }
            set
            {
                _tabIndex = value;
                RaisePropertyChanged("TabIndex");
            }
        }

        public string SearchTime
        {
            get { return _searchTime; }
            set
            {
                _searchTime = value;
                RaisePropertyChanged("SearchTime");
            }
        }

        public Window MainWindow { get; set; }
        #endregion //Properties

        #region Commands
        public RelayCommand BrowseCompMatrixFileCommand { get; set; }
        public RelayCommand SearchCommand { get; private set; }
        public AsyncRelayCommand OnLoadCommand { get; private set; }
        #endregion //Commands

        #region Methods
        private void InitializeDatas()
        {
            //var compMatrixList = await GetCompMatrixModel();
            //this.CompMatrixList = new ObservableCollection<CompMatrixModel>(compMatrixList);
            //this.SearchList = new ObservableCollection<CompMatrixModel>();
        }

        private void InitializeCommands()
        {
            BrowseCompMatrixFileCommand = new RelayCommand(ExecuteBrowseCompMatrixFileCommand);
            SearchCommand = new RelayCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
            OnLoadCommand = new AsyncRelayCommand(ExecuteOnLoadCommand);
        }

        private void ExecuteBrowseCompMatrixFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Comp Matrix file (*.xlsx)|*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.IsExcelParsed = Visibility.Hidden;
                try
                {
                    this.CompMatrixFilePath = openFileDialog.FileName;

                    CompMatrixDocumentParser parser = new CompMatrixDocumentParser();
                    var compMatrixList = parser.Parse(this.CompMatrixFilePath);
                    this.CompMatrixList = new ObservableCollection<CompMatrixModel>(compMatrixList);
                    this.SearchList = new ObservableCollection<CompMatrixModel>();
                    this.IsExcelParsed = Visibility.Visible;
                    this.TabIndex = 0;
                }
                catch(Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Comp Matrix");
                }
            }
        }

        private bool CanExecuteSearchCommand()
        {
            if (/*string.IsNullOrEmpty(_compMatrixFilePath)
                || */string.IsNullOrEmpty(_incentivePlan))
                return false;
            else
            {
                //if (this.IsExcelParsed == Visibility.Visible) return true;
                //else return false;
                return true;
            }
        }

        private void ExecuteSearchCommand()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            IEnumerable<CompMatrixModel> result = null;
            if(string.IsNullOrEmpty(_country))
            {
                result = this.CompMatrixList.Where(cm => cm.IncentivePlan.ToLower() == _incentivePlan.ToLower());
            }
            else
            {
                result = this.CompMatrixList.Where(cm => cm.IncentivePlan.ToLower() == _incentivePlan.ToLower() && cm.Country.ToLower().Contains(_country.ToLower()));
            }
            this.SearchList = new ObservableCollection<CompMatrixModel>(result);
            watch.Stop();
            double elapsedMS = watch.ElapsedMilliseconds;
            this.SearchTime = string.Format("About {0} results ({1} milli seconds)", this.SearchList.Count, elapsedMS);
            this.TabIndex = 1;
        }

        public void UpdateSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion //Methods

        public async Task<List<CompMatrixModel>> GetCompMatrixModel()
        {
            try
            {
                if (string.IsNullOrEmpty(_compMatrixFileName))
                {
                    System.Windows.Forms.MessageBox.Show("Comp Matrix File Name not configured!");
                    return new List<CompMatrixModel>();
                }

                HttpResponseMessage responseMessage = await httpClient.GetAsync(addressSuffix);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string serializedContent = await responseMessage.Content.ReadAsStringAsync();
                    return serializedContent.JSONDeSerialize<List<CompMatrixModel>>();
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

        public async Task ExecuteOnLoadCommand(object param)
        {
            var compMatrixList = await GetCompMatrixModel();
            this.CompMatrixList = new ObservableCollection<CompMatrixModel>(compMatrixList);
            this.SearchList = new ObservableCollection<CompMatrixModel>();
        }
    }
}