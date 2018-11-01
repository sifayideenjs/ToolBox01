using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Common;
using ToolBox.Common.Utilities;
using ToolBox.ConsoleTool.Controllers;
using ToolBox.DataAccess;
using ToolBox.DataAccess.Repositories;
using ToolBox.Models;

namespace ToolBox.ConsoleTool.ViewModel
{
    public class ConsoleConfigViewModel : ViewModelBase
    {
        #region Fields
        private Tile _currentTile = null;
        private string _processInfo = string.Empty;
        private ObservableCollection<Rule> _ruleList = new ObservableCollection<Rule>();

        private List<string> _types = new List<string>() { "Text", "File Path", "Folder Directory" };
        private string _selectedType = "Text";
        private string _expression = string.Empty;
        private string _parameterName = string.Empty;
        private List<string> _optionals = new List<string>() { "False", "True" };
        private string _selectedOptional = "False";
        private string _optionalExpression = string.Empty;
        private string _filterName = string.Empty;
        private string _filter = string.Empty;
        #endregion //Fields

        #region Constructor
        public ConsoleConfigViewModel(IUnityContainer container)
        {
            BrowseToolPathCommand = new RelayCommand(OnBrowseToolPathCommand);
            ConfigreCommand = new RelayCommand(OnConfigreCommand, CanConfigreCommand);
            AddRuleCommand = new RelayCommand(OnAddRuleCommand, CanAddRuleCommand);
            DeleteRuleCommand = new RelayCommand<Rule>(OnDeleteRuleCommand, CanDeleteRuleCommand);
        }
        #endregion //Constructor

        #region Properties
        public string ProcessInfo
        {
            get
            {
                return _processInfo;
            }
            set
            {
                _processInfo = value;
                RaisePropertyChanged("ProcessInfo");
            }
        }

        public List<string> Types
        {
            get
            {
                return _types;
            }
            set
            {
                _types = value;
                RaisePropertyChanged("Types");
            }
        }

        public string SelectedType
        {
            get
            {
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                RaisePropertyChanged("SelectedType");
            }
        }

        public string Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                _expression = value;
                RaisePropertyChanged("Expression");
            }
        }

        public string ParameterName
        {
            get
            {
                return _parameterName;
            }
            set
            {
                _parameterName = value;
                RaisePropertyChanged("ParameterName");
            }
        }

        public List<string> Optionals
        {
            get
            {
                return _optionals;
            }
            set
            {
                _optionals = value;
                RaisePropertyChanged("Optionals");
            }
        }

        public string SelectedOptional
        {
            get
            {
                return _selectedOptional;
            }
            set
            {
                _selectedOptional = value;
                RaisePropertyChanged("SelectedOptional");
            }
        }

        public string OptionalExpression
        {
            get
            {
                return _optionalExpression;
            }
            set
            {
                _optionalExpression = value;
                RaisePropertyChanged("OptionalExpression");
            }
        }

        public string FilterName
        {
            get
            {
                return _filterName;
            }
            set
            {
                _filterName = value;
                RaisePropertyChanged("FilterName");
            }
        }

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                RaisePropertyChanged("Filter");
            }
        }

        public ObservableCollection<Rule> RuleList
        {
            get
            {
                return _ruleList;
            }
            set
            {
                _ruleList = value;
                RaisePropertyChanged("RuleList");
            }
        }
        #endregion //Properties

        #region Methods
        public void SetCurrentTile(Tile currentTile)
        {
            this._currentTile = currentTile;
            if (_currentTile != null)
            {
                ProcessInfo = _currentTile.ProcessInfo;
                if(_currentTile.RuleSet != null) RuleList = new ObservableCollection<Rule>(_currentTile.RuleSet.RuleList);
            }
        }
        #endregion //Methods

        #region BrowseToolPathCommand
        public RelayCommand BrowseToolPathCommand { get; private set; }

        private void OnBrowseToolPathCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Exe file (*.exe)|*.exe";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                this.ProcessInfo = openFileDialog.FileName;
            }
        }
        #endregion

        #region ConfigreCommand
        public RelayCommand ConfigreCommand { get; private set; }

        private void OnConfigreCommand()
        {
            _currentTile.ProcessInfo = _processInfo;
            if (_currentTile.RuleSet == null) _currentTile.RuleSet = new RuleSet();
            _currentTile.RuleSet.RuleList = this.RuleList.ToList();

            string xmlString = BackboneUtility.GenerateBackboneFormXMLString(_currentTile.RuleSet);
            _currentTile.BackBoneXmlString = xmlString;

            TileRepository repository = new TileRepository();
            repository.Update(_currentTile);
            repository.Save();
        }

        private bool CanConfigreCommand()
        {
            if (string.IsNullOrEmpty(this.ProcessInfo) | this.RuleList.Count == 0) return false;
            else return true;
        }
        #endregion

        #region AddRuleCommand
        public RelayCommand AddRuleCommand { get; private set; }

        private void OnAddRuleCommand()
        {
            Rule newRule = null;
            switch (this.SelectedType)
            {
                case "Text":
                    {
                        newRule = new TextRule()
                        {
                            Expression = this.Expression,
                            ParameterName = this.ParameterName,
                            IsOptional = this.SelectedOptional == "False" ? false : true,
                            OptionalExpression = this.OptionalExpression
                        };
                        break;
                    }
                case "File Path":
                    {
                        newRule = new FilePathRule()
                        {
                            Expression = this.Expression,
                            ParameterName = this.ParameterName,
                            IsOptional = this.SelectedOptional == "False" ? false : true,
                            OptionalExpression = this.OptionalExpression,
                            FilterName = this.FilterName,
                            Filter = this.Filter
                        };
                        break;
                    }
                case "Folder Directory":
                    {
                        newRule = new FolderDirectoryRule()
                        {
                            Expression = this.Expression,
                            ParameterName = this.ParameterName,
                            IsOptional = this.SelectedOptional == "False" ? false : true,
                            OptionalExpression = this.OptionalExpression
                        };
                        break;
                    }
                default:
                    {
                        newRule = new TextRule()
                        {
                            Expression = this.Expression,
                            ParameterName = this.ParameterName,
                            IsOptional = this.SelectedOptional == "False" ? false : true,
                            OptionalExpression = this.OptionalExpression
                        };
                        break;
                    }
            }

            this.RuleList.Add(newRule);
            this.ClearRuleEntry();
        }

        private bool CanAddRuleCommand()
        {
            if (string.IsNullOrEmpty(this._selectedType)
                | string.IsNullOrEmpty(this._expression)
                | string.IsNullOrEmpty(this._selectedOptional)) return false;
            else
            {
                if(this.SelectedType == "File Path")
                {
                    if (string.IsNullOrEmpty(this._filterName)
                        | string.IsNullOrEmpty(this._filter)) return false;
                    else return true;
                }
                else return true;
            }
        }

        private void ClearRuleEntry()
        {
            this.SelectedType = "Text";
            this.Expression = string.Empty;
            this.ParameterName = string.Empty;
            this.SelectedOptional = "False";
            this.OptionalExpression = string.Empty;
            this.FilterName = string.Empty;
            this.Filter = string.Empty;
        }
        #endregion

        #region DeleteRuleCommand
        public RelayCommand<Rule> DeleteRuleCommand { get; private set; }

        private void OnDeleteRuleCommand(Rule rule)
        {
            if(this.RuleList.Contains(rule))
            {
                this.RuleList.Remove(rule);
            }
        }

        private bool CanDeleteRuleCommand(Rule rule)
        {
            return true;
        }
        #endregion
    }
}
