using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Models
{
    public class CenterOfExcellence
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Hub Hub { get; set; }
    }

    public class Hub
    {
        public Hub()
        {
            this.HubSections = new List<HubSection>();
        }

        public int Id { get; set; }
        public virtual List<HubSection> HubSections { get; set; }
    }

    public class HubSection
    {
        public HubSection()
        {
            this.Tiles = new List<Tile>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Tile> Tiles { get; set; }
    }

    public class Tile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string View { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public virtual RuleSet RuleSet { get; set; }
        public string ProcessInfo { get; set; }
        public bool IsFavourite { get; set; }
        public string BackBoneXmlString { get; set; }
    }

    public class RuleSet
    {
        public RuleSet()
        {
            _ruleList = new List<Rule>();
        }

        public RuleSet(List<Rule> ruleList)
        {
            _ruleList = ruleList;
        }
        public int Id { get; set; }

        protected List<Rule> _ruleList;

        public virtual List<Rule> RuleList
        {
            get { return _ruleList; }
            set { _ruleList = value; }
        }

        public void Add(Rule rule)
        {
            if (rule != null) _ruleList.Add(rule);
        }

        public void AddRange(IEnumerable<Rule> ruleList)
        {
            if (ruleList != null) _ruleList.AddRange(ruleList);
        }
    }

    public abstract class Rule
    {
        protected string _value = string.Empty;
        protected List<Rule> _dependentRules;
        protected string _expression;
        protected string _optionalExpression;
        protected string _parameterName;
        protected bool _isOptional = false;

        public Rule()
        {
            _dependentRules = new List<Rule>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public List<Rule> DependentRules
        {
            get { return _dependentRules; }
        }

        public string Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }

        public string OptionalExpression
        {
            get { return _optionalExpression; }
            set { _optionalExpression = value; }
        }

        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        public bool IsOptional
        {
            get { return _isOptional; }
            set { _isOptional = value; }
        }

        internal void AddDependency(Rule rule)
        {
            if (rule != null) _dependentRules.Add(rule);
        }

        internal void AddDependencyRange(IEnumerable<Rule> ruleList)
        {
            if (ruleList != null) _dependentRules.AddRange(ruleList);
        }

        public string GetExpression()
        {
            string expression = string.Format("Do you want to enter {0}? (Y/N)", _expression);
            if (!string.IsNullOrEmpty(_optionalExpression)) expression = _optionalExpression;
            return expression;
        }

        public abstract bool Validate(out string message);
    }

    public class TextRule : Rule
    {
        public TextRule()
        {
            Type = "Text";
        }

        public int Id { get; set; }
        public override bool Validate(out string message)
        {
            message = string.Empty;
            while (true)
            {
                bool isValid = !string.IsNullOrEmpty(_value);
                if (!isValid) message = "!! Please enter a valid entry !!";
                return isValid;
            }
        }
    }

    public class FilePathRule : Rule
    {
        public FilePathRule()
        {
            Type = "File Path";
        }

        public int Id { get; set; }
        public string FilterName { get; set; }
        public string Filter { get; set; }
        public override bool Validate(out string message)
        {
            message = string.Empty;
            while (true)
            {
                bool isValid = System.IO.File.Exists(_value);
                if (!isValid) message = "!! Please enter a valid file path !!";
                return isValid;
            }
        }
    }

    public class FolderDirectoryRule : Rule
    {
        public FolderDirectoryRule()
        {
            Type = "Folder Directory";
        }

        public int Id { get; set; }
        public override bool Validate(out string message)
        {
            message = string.Empty;
            while (true)
            {
                bool isValid = System.IO.Directory.Exists(_value);
                if (!isValid) message = "!! Please enter a valid folder directory !!";
                return isValid;
            }
        }
    }
}
