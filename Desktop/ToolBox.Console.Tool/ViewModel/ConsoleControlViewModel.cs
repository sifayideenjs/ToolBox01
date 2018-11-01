using Backbone.Common.Parser;
using Backbone.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ToolBox.Common;
using ToolBox.Common.Models;
using ToolBox.ConsoleTool.Utility;
using ToolBox.Models;

namespace ToolBox.ConsoleTool.ViewModel
{
    public class ConsoleControlViewModel : ViewModelBase
    {
        private string consoleExpression = "Do you want to start the process ? (Y/N)";
        private string consoleInput = string.Empty;
        private string processAppPath = string.Empty;
        private ObservableCollection<string> consoleOutput = new ObservableCollection<string>();
        private RuleEngine _engine = null;
        private RuleExecutionEventCallback _currentRuleCallback = null;
        private OptionalRuleExecutionEventCallback _currentOptionalRuleCallback = null;
        private Rule _currentRule = null;
        private bool userShouldEditValueNow = true;
        private bool _isProcessRunning = false;
        private BackboneFormViewModel _backboneFormViewModel = null;
        private bool _isToolConfigured = false;

        private ProcessInterface processInterace = new ProcessInterface();

        public ConsoleControlViewModel()
        {
            InitializeRuleSet();
            DrawASCIIArt();
            RunCommand = new RelayCommand(ExecuteRunCommand);
            ProcessAppPath = string.Empty;

            //  Handle process events.
            processInterace.OnProcessOutput += new ProcessEventHanlder(processInterace_OnProcessOutput);
            processInterace.OnProcessError += processInterace_OnProcessError;
            processInterace.OnProcessInput += new ProcessEventHanlder(processInterace_OnProcessInput);
            processInterace.OnProcessExit += new ProcessEventHanlder(processInterace_OnProcessExit);
        }

        private void DrawASCIIArt()
        {
            var arr = new[]
            {
                @"                                                                     ",
                @"     ___________           .__    __________ ________  ____  ___     ",
                @"     \__    ___/___   ____ |  |   \______   \\_____  \ \   \/  /     ",
                @"       |    | /  _ \ /  _ \|  |    |    |  _/ /   |   \ \     /      ",
                @"       |    |(  <_> |  <_> )  |__  |    |   \/    |    \/     \      ",
                @"       |____| \____/ \____/|____/  |______  /\_______  /___/\  \     ",
                @"                                          \/         \/      \_/     ",
                @"                                                                     ",
            };

            foreach (string line in arr)
                ConsoleOutput.Add(line);
        }

        private void InitializeRuleSet()
        {
            _engine = new RuleEngine();
            _engine.OnRuleExecution += _engine_OnRuleExecution;
            _engine.OnOptionalRuleExecution += _engine_OnOptionalRuleExecution;
            _engine.OnRuleEngineErrorDetection += _engine_OnRuleEngineErrorDetection;
            _engine.OnRuleEngineExecutionComplete += _engine_OnRuleEngineExecutionComplete;
        }

        public string ConsoleExpression
        {
            get
            {
                return consoleExpression;
            }
            set
            {
                consoleExpression = value;
                RaisePropertyChanged("ConsoleExpression");
            }
        }

        public string ConsoleInput
        {
            get
            {
                return consoleInput;
            }
            set
            {
                consoleInput = value;
                RaisePropertyChanged("ConsoleInput");
            }
        }

        public string ProcessAppPath
        {
            get
            {
                return processAppPath;
            }
            set
            {
                processAppPath = value;
                RaisePropertyChanged("ProcessAppPath");
            }
        }

        public bool UserShouldEditValueNow
        {
            get
            {
                return userShouldEditValueNow;
            }
            set
            {
                userShouldEditValueNow = value;
                RaisePropertyChanged("UserShouldEditValueNow");
            }
        }

        public bool IsProcessRunning
        {
            get
            {
                return _isProcessRunning;
            }
            private set
            {
                _isProcessRunning = value;
            }
        }

        public ObservableCollection<string> ConsoleOutput
        {
            get
            {
                return consoleOutput;
            }
            set
            {
                consoleOutput = value;
                RaisePropertyChanged("ConsoleOutput");
            }
        }

        public BackboneFormViewModel BackboneFormViewModel
        {
            get
            {
                return _backboneFormViewModel;
            }
            set
            {
                _backboneFormViewModel = value;
                RaisePropertyChanged("BackboneFormViewModel");
            }
        }

        public RelayCommand RunCommand { get; private set; }

        public void SetRuleSet(Tile tile)
        {
            if (_engine != null) _engine.SetRuleSet(tile.RuleSet);
            this.processAppPath = tile.ProcessInfo;

            if(tile.RuleSet != null && tile.RuleSet.RuleList.Count > 0 && !string.IsNullOrEmpty(tile.ProcessInfo))
            {
                _isToolConfigured = true;

                if (!string.IsNullOrEmpty(tile.BackBoneXmlString))
                {
                    TextReader textReader = new StringReader(tile.BackBoneXmlString);
                    this.BackboneFormViewModel = BackboneXMLParser.GetBackboneFormViewModel(textReader);
                    BackboneFormViewModel.ExecuteCommand = new RelayCommand<object>(OnExecuteCommand);
                }
            }
            else
            {
                _isToolConfigured = false;
                ConsoleExpression = string.Empty;
                ConsoleOutput.Add("** Please configure the tool to start the process **");
            }            
        }

        void _engine_OnRuleExecution(RuleEventArgs eventArgs, RuleExecutionEventCallback ruleCallback)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Rule rule = eventArgs.Rule;
                this.ConsoleExpression = rule.Expression;
                _currentOptionalRuleCallback = null;
                _currentRuleCallback = ruleCallback;
                _currentRule = rule;
            }));
        }

        void _engine_OnOptionalRuleExecution(RuleEventArgs eventArgs, OptionalRuleExecutionEventCallback optionalRuleCallback)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Rule rule = eventArgs.Rule;
                this.ConsoleExpression = eventArgs.Expression;
                _currentRuleCallback = null;
                _currentOptionalRuleCallback = optionalRuleCallback;
                _currentRule = rule;
            }));
        }

        void _engine_OnRuleEngineErrorDetection(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ConsoleOutput.Add(message);
            }));
        }

        void _engine_OnRuleEngineExecutionComplete(string argument)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ConsoleOutput.Add(string.Empty);
                _currentRule = null;
                _currentRuleCallback = null;
                _currentOptionalRuleCallback = null;

                //StartProcess("cmd.exe", "/c ping www.google.com -t");
                StartProcess(ProcessAppPath, argument);
                ConsoleExpression = "Do you want to exit the process again ? (Y/N)";
            }));
        }

        #region Events
        void processInterace_OnProcessError(object sender, ProcessEventArgs args)
        {
            //  Write the output, in red
            WriteOutput(args.Content);
        }

        void processInterace_OnProcessOutput(object sender, ProcessEventArgs args)
        {
            //  Write the output, in white
            WriteOutput(args.Content);
        }

        void processInterace_OnProcessInput(object sender, ProcessEventArgs args)
        {
            //FireProcessInputEvent(args);
        }

        void processInterace_OnProcessExit(object sender, ProcessEventArgs args)
        {
            RunOnUIDespatcher((Action)(() =>
            {
                IsProcessRunning = false;
                Thread.Sleep(2000);
                ConsoleOutput.Add("** Successfully executed the process **");
                ConsoleExpression = "Do you want to start the process again ? (Y/N)";
            }));
        }
        #endregion //Events

        public void ExecuteRunCommand()
        {
            if (!_isToolConfigured) return;

            ConsoleOutput.Add(string.Format("{0} :>{1}", ConsoleExpression, ConsoleInput));

            if (_currentRuleCallback != null)
            {
                _currentRule.Value = ConsoleInput;
                _currentRuleCallback(_currentRule);
            }
            else if (_currentOptionalRuleCallback != null)
            {
                _currentOptionalRuleCallback(_currentRule, ConsoleInput);
            }
            else
            {
                if(IsProcessRunning)
                {
                    bool result = ReadBool(ConsoleInput);
                    if (result) StopProcess();
                }
                else
                {
                    bool result = ReadBool(ConsoleInput);
                    if (result) _engine.Start();
                }
            }

            ConsoleInput = String.Empty;
        }

        private bool ReadBool(string option)
        {
            String r = (option ?? "").ToLower();
            if (r == "y") return true;
            if (r == "n") return false;
            ConsoleOutput.Add("!! Please select a valid option !!");
            return false;
        }

        #region Methods
        public void WriteOutput(string output)
        {
            RunOnUIDespatcher((Action)(() =>
            {
                ConsoleOutput.Add(output);
            }));
        }

        public void WriteInput(string input, bool echo)
        {
            RunOnUIDespatcher((Action)(() =>
            {
                //  Are we echoing?
                if (echo)
                {
                    ConsoleOutput.Add(input);
                }

                //  Write the input.
                processInterace.WriteInput(input);
            }));
        }

        private void RunOnUIDespatcher(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }

        public void StartProcess(string fileName, string arguments)
        {
            if(File.Exists(fileName))
            {
                //  Start the process.
                processInterace.StartProcess(fileName, arguments);

                IsProcessRunning = true;
            }
        }

        public void StopProcess()
        {
            processInterace.StopProcess();
        }
        #endregion //Methods

        private void OnExecuteCommand(object param)
        {
            if (param != null && param is Dictionary<string, string>)
            {
                Dictionary<string, string> parameterDictionary = param as Dictionary<string, string>;
                string argument = GenerateArgument(parameterDictionary);
                StartProcess(ProcessAppPath, argument);
            }
        }

        private string GenerateArgument(Dictionary<string, string> parameterDictionary)
        {
            string argument = string.Empty;
            foreach(string key in parameterDictionary.Keys)
            {
                string value = parameterDictionary[key];
                argument += string.Format("{0}=\"{1}\" ", key, value);
            }
            return argument;
        }
    }
}
