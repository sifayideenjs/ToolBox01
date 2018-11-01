using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolBox.Models;

namespace ToolBox.Common.Models
{
    public delegate void RuleEngineExecutionEventHandler(string message);
    public delegate void RuleExecutionEventCallback(Rule rule);
    public delegate void OptionalRuleExecutionEventCallback(Rule rule, string expression);
    public delegate void RuleExecutionEventHandler(RuleEventArgs eventArgs, RuleExecutionEventCallback ruleCallback);
    public delegate void OptionalRuleExecutionEventHandler(RuleEventArgs eventArgs, OptionalRuleExecutionEventCallback optionalRuleCallback);

    public class RuleEventArgs : EventArgs
    {
        public RuleEventArgs(Rule rule)
        {
            this.Rule = rule;
        }

        public RuleEventArgs(Rule rule, string expression)
        {
            this.Rule = rule;
            this.Expression = expression;
        }

        public Rule Rule
        {
            get;
            private set;
        }

        public string Expression
        {
            get;
            private set;
        }
    }

    public class RuleEngine
    {
        public event RuleEngineExecutionEventHandler OnRuleEngineExecutionComplete;
        public event RuleEngineExecutionEventHandler OnRuleEngineErrorDetection;
        public event RuleExecutionEventHandler OnRuleExecution;
        public event OptionalRuleExecutionEventHandler OnOptionalRuleExecution;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private BackgroundWorker _workerThread;
        private RuleSet _ruleSet;

        private string _argument = string.Empty;

        public RuleEngine()
        {
        }

        public RuleEngine(List<Rule> ruleList)
        {
            _ruleSet = new RuleSet(ruleList);
        }

        public RuleSet CreateRuleSet()
        {
            if (_ruleSet == null) _ruleSet = new RuleSet();
            return _ruleSet;
        }

        public void SetRuleSet(RuleSet ruleSet)
        {
            _ruleSet = ruleSet;
        }

        public void Start()
        {
            if (_workerThread == null || !_workerThread.IsBusy)
            {
                _workerThread = new BackgroundWorker();
                _workerThread.DoWork += workerThread_DoWork;
                _workerThread.RunWorkerCompleted += workerThread_RunWorkerCompleted;
                _workerThread.RunWorkerAsync();
            }
        }

        private void workerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_ruleSet != null)
            {
                var ruleList = _ruleSet.RuleList;
                foreach (var rule in ruleList)
                {
                    RuleEventArgs eventArg = null;
                    if (rule.IsOptional)
                    {
                        string expression = rule.GetExpression();
                        eventArg = new RuleEventArgs(rule, expression);
                        if (this.OnOptionalRuleExecution != null)
                        {
                            OnOptionalRuleExecution(eventArg, OnValidateOption);
                            RuleEngineWait(rule);
                        }
                    }
                    else
                    {
                        eventArg = new RuleEventArgs(rule);
                        if (this.OnRuleExecution != null)
                        {
                            OnRuleExecution(eventArg, OnRuleEngineResume);
                            RuleEngineWait(rule);
                        }
                    }

                    GenerateArgument(rule);
                }
            }
        }

        private void GenerateArgument(Rule rule)
        {
            string message = string.Empty;
            bool isValid = rule.Validate(out message);
            if (isValid)
            {
                if(string.IsNullOrEmpty(rule.ParameterName)) _argument += string.Format("\"{1}\" ", rule.ParameterName, rule.Value);
                else _argument += string.Format("{0}=\"{1}\" ", rule.ParameterName, rule.Value);
            }
        }

        private void workerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _workerThread = null;
            if (this.OnRuleEngineExecutionComplete != null) OnRuleEngineExecutionComplete(_argument);
            //this.Run();
        }

        internal void RuleEngineWait(Rule rule)
        {
            _resetEvent.Reset();
            _resetEvent.WaitOne();
        }

        internal void OnRuleEngineResume(Rule rule)
        {
            while (true)
            {
                string message = string.Empty;
                bool isValid = rule.Validate(out message);
                if (isValid)
                {
                    _resetEvent.Set();
                    return;
                }
                else
                {
                    if (this.OnRuleEngineErrorDetection != null) OnRuleEngineErrorDetection(message);
                    return;
                }
            }
        }

        internal void OnValidateOption(Rule rule, string option)
        {
            while (true)
            {
                String r = (option ?? "").ToLower();
                if (r == "y")
                {
                    RuleEventArgs eventArg = new RuleEventArgs(rule);
                    if (this.OnRuleExecution != null) OnRuleExecution(eventArg, OnRuleEngineResume);
                    return;
                }
                else if (r == "n")
                {
                    _resetEvent.Set();
                    return;
                }
                else
                {
                    if (this.OnRuleEngineErrorDetection != null) OnRuleEngineErrorDetection("!! Please select a valid option !!");
                    return;
                }
            }
        }
    }

    //public class RuleSet
    //{
    //    public RuleSet()
    //    {
    //        _ruleList = new List<Rule>();
    //    }

    //    public RuleSet(List<Rule> ruleList)
    //    {
    //        _ruleList = ruleList;
    //    }

    //    protected List<Rule> _ruleList;

    //    public List<Rule> RuleList
    //    {
    //        get { return _ruleList; }
    //    }

    //    public void Add(Rule rule)
    //    {
    //        if (rule != null) _ruleList.Add(rule);
    //    }

    //    public void AddRange(IEnumerable<Rule> ruleList)
    //    {
    //        if (ruleList != null) _ruleList.AddRange(ruleList);
    //    }
    //}

    //public abstract class Rule
    //{
    //    protected string _value = string.Empty;
    //    protected List<Rule> _dependentRules;
    //    protected string _expression;
    //    protected string _optionalExpression;
    //    protected string _parameterName;
    //    protected bool _isOptional = false;

    //    public Rule()
    //    {
    //        _dependentRules = new List<Rule>();
    //    }

    //    public string Value
    //    {
    //        get { return _value; }
    //        set { _value = value; }
    //    }

    //    public List<Rule> DependentRules
    //    {
    //        get { return _dependentRules; }
    //    }

    //    public string Expression
    //    {
    //        get { return _expression; }
    //        set { _expression = value; }
    //    }

    //    public string OptionalExpression
    //    {
    //        get { return _optionalExpression; }
    //        set { _optionalExpression = value; }
    //    }

    //    public string ParameterName
    //    {
    //        get { return _parameterName; }
    //        set { _parameterName = value; }
    //    }

    //    public bool IsOptional
    //    {
    //        get { return _isOptional; }
    //        set { _isOptional = value; }
    //    }

    //    internal void AddDependency(Rule rule)
    //    {
    //        if (rule != null) _dependentRules.Add(rule);
    //    }

    //    internal void AddDependencyRange(IEnumerable<Rule> ruleList)
    //    {
    //        if (ruleList != null) _dependentRules.AddRange(ruleList);
    //    }

    //    public string GetExpression()
    //    {
    //        string expression = string.Format("Do you want to enter {0}? (Y/N)", _expression);
    //        if (!string.IsNullOrEmpty(_optionalExpression)) expression = _optionalExpression;
    //        return expression;
    //    }

    //    internal abstract bool Validate(out string message);
    //}

    //public class TextRule : Rule
    //{
    //    public TextRule()
    //    {
    //    }

    //    internal override bool Validate(out string message)
    //    {
    //        message = string.Empty;
    //        while (true)
    //        {
    //            bool isValid = !string.IsNullOrEmpty(_value);
    //            if (!isValid) message = "!! Please enter a valid entry !!";
    //            return isValid;
    //        }
    //    }
    //}

    //public class FilePathRule : Rule
    //{
    //    public FilePathRule()
    //    {
    //    }

    //    internal override bool Validate(out string message)
    //    {
    //        message = string.Empty;
    //        while (true)
    //        {
    //            bool isValid = System.IO.File.Exists(_value);
    //            if (!isValid) message = "!! Please enter a valid file path !!";
    //            return isValid;
    //        }
    //    }
    //}

    //public class FolderDirectoryRule : Rule
    //{
    //    public FolderDirectoryRule()
    //    {
    //    }

    //    internal override bool Validate(out string message)
    //    {
    //        message = string.Empty;
    //        while (true)
    //        {
    //            bool isValid = System.IO.Directory.Exists(_value);
    //            if (!isValid) message = "!! Please enter a valid folder directory !!";
    //            return isValid;
    //        }
    //    }
    //}

    //public class ExecuteRule : Rule
    //{
    //    public ExecuteRule()
    //    {
    //    }

    //    internal override bool Validate(out string message)
    //    {
    //        message = string.Empty;
    //        while (true)
    //        {
    //            bool isValid = !string.IsNullOrEmpty(_value);
    //            if (!isValid) message = "!! Please enter a valid entry !!";
    //            return isValid;
    //        }
    //    }
    //}
}
