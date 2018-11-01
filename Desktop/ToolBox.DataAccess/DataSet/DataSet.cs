using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox.Models;
using ToolBox.DataAccess.Repositories;
using System.IO;
using System.Xml.Serialization;

namespace ToolBox.DataAccess.DataSet
{
    public static class DataSet
    {
        public static void PopulateDataSet()
        {
            PopulateDG();
            PopulateFMS();
            PopulateCNS();
            PopulateFC();
            PopulateEPC();
            PopulateCBM();
            PopulateCommon();
        }

        private static void PopulateDG()
        {
            Tile tile01 = new Tile() { Name = "SS & P", View = "ToolView", ShortDescription = "Source Selection" };
            Tile tile02 = new Tile() { Name = "ECLD", View = "ToolView", ShortDescription = "Expansion of ECLD" };
            Tile tile03 = new Tile() { Name = "TVA", View = "ToolView", ShortDescription = "Test Vector Automation Tool" };
            Tile tile04 = new Tile() { Name = "Auto GIT", View = "ToolView", ShortDescription = "Auto GIT Tool" };
            Tile tile05 = new Tile() { Name = "SSI / TGS", View = "ToolView", ShortDescription = "Some tool desctiption" };

            HubSection hubSection01 = new HubSection();
            hubSection01.Name = "D & G Tools";
            hubSection01.Tiles.AddRange(new List<Tile> { tile01, tile02, tile03, tile04, tile05 });

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01 });

            CenterOfExcellence coe = new CenterOfExcellence() { Name = "DG" };
            coe.Hub = hub;

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Insert(coe);
            repository.Save();
        }

        private static void PopulateFMS()
        {
            Tile tile01 = new Tile() { Name = "Common IO", View = "ToolView", ShortDescription = "Common IO Tool" };
            Tile tile02 = new Tile() { Name = "TGS", View = "ToolView", ShortDescription = "Test Generation System" };
            Tile tile03 = new Tile() { Name = "ASG", View = "ToolView", ShortDescription = "Automatic Script Generator" };
            Tile tile04 = new Tile() { Name = "ERG", View = "ToolView", ShortDescription = "Expected Result Generation" };

            HubSection hubSection01 = new HubSection();
            hubSection01.Name = "FMS Tools";
            hubSection01.Tiles.AddRange(new List<Tile> { tile01, tile02, tile03, tile04 });

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01 });

            CenterOfExcellence coe = new CenterOfExcellence() { Name = "FMS" };
            coe.Hub = hub;

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Insert(coe);
            repository.Save();
        }

        private static void PopulateCNS()
        {
            Tile tile01 = new Tile() { Name = "DIGIT", View = "ToolView", ShortDescription = "DIGIT Tool" };

            HubSection hubSection01 = new HubSection();
            hubSection01.Name = "CNS Tools";
            hubSection01.Tiles.AddRange(new List<Tile> { tile01 });

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01 });

            CenterOfExcellence coe = new CenterOfExcellence() { Name = "CNS" };
            coe.Hub = hub;

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Insert(coe);
            repository.Save();
        }

        private static void PopulateFC()
        {
            Tile tile01 = new Tile() { Name = "Spiral", View = "ToolView", ShortDescription = "Spiral Tool" };
            Tile tile02 = new Tile() { Name = "Vector X", View = "ToolView", ShortDescription = "Vector X Tool" };
            Tile tile03 = new Tile() { Name = "INTEGRA", View = "ToolView", ShortDescription = "INTEGRA Tool" };
            Tile tile04 = new Tile() { Name = "CDTG", View = "ToolView", ShortDescription = "Constant Dictionary Testcase Generator Tool" };
            Tile tile05 = new Tile() { Name = "CTP", View = "ToolView", ShortDescription = "CTP Test Vector Generator Tool" };
            Tile tile06 = new Tile() { Name = "PDDIF", View = "ToolView", ShortDescription = "PDDIF Test Generator Tool" };
            Tile tile07 = new Tile() { Name = "Simitrix", View = "ToolView", ShortDescription = "Simitrix Tool" };
            Tile tile08 = new Tile() { Name = "sDirect", View = "ToolView", ShortDescription = "sDirect Tool" };
            Tile tile09 = new Tile() { Name = "IO Test Generator", View = "ToolView", ShortDescription = "IO Test Generator Tool" };
            Tile tile10 = new Tile() { Name = "PIPC-Invalid", View = "ToolView", ShortDescription = "PIPC-Invalid Test Vector Generator" };
            Tile tile11 = new Tile() { Name = "xlForge", View = "ToolView", ShortDescription = "xlForge Tool" };
            Tile tile12 = new Tile() { Name = "Mode Logic", View = "ToolView", ShortDescription = "Mode Logic Test Generator Tool" };
            Tile tile13 = new Tile() { Name = "Watcher", View = "ToolView", ShortDescription = "Watcher Test Generator Tool" };

            HubSection hubSection01 = new HubSection();
            hubSection01.Name = "FC Tools";
            hubSection01.Tiles.AddRange(new List<Tile> { tile01, tile02, tile03, tile04, tile05, tile06, tile07, tile08, tile09, tile10, tile11, tile12, tile13 });

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01 });

            CenterOfExcellence coe = new CenterOfExcellence() { Name = "FC" };
            coe.Hub = hub;

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Insert(coe);
            repository.Save();
        }

        private static void PopulateEPC()
        {
            Rule rule01 = new FilePathRule() { Expression = "DIRS Excel", ParameterName = "DIRSPATH", FilterName = "Excel File", Filter = "xlsx" };
            Rule rule02 = new FolderDirectoryRule() { Expression = "MDB Directory", ParameterName = "MDBDIRECTORY", IsOptional = true };
            Rule rule03 = new FolderDirectoryRule() { Expression = "CSV Directory", ParameterName = "CSVDIRECTORY" };
            Rule rule04 = new FilePathRule() { Expression = "CAN BUS RX XML File", ParameterName = "CANBUSRXXMLPATH", IsOptional = true, FilterName = "XML File", Filter = "xml" };
            Rule rule05 = new FilePathRule() { Expression = "CAN BUS TX XML File", ParameterName = "CANBUSTXXMLPATH", IsOptional = true, FilterName = "XML File", Filter = "xml" };

            RuleSet ruleSet = new RuleSet();
            ruleSet.Add(rule01);
            ruleSet.Add(rule02);
            ruleSet.Add(rule03);
            ruleSet.Add(rule04);
            ruleSet.Add(rule05);

            string xmlString = GenerateBackboneFormXMLString(ruleSet);

            Tile tile01 = new Tile() { Name = "Simplify", View = "ToolView", ShortDescription = "Simplify Tool" };
            Tile tile02 = new Tile() { Name = "HSITD", View = "ToolView", ShortDescription = "HSITD Tool", BackBoneXmlString = xmlString, RuleSet = ruleSet, ProcessInfo = @"C:\Users\h149041\Desktop\HDTCG_XMLGenerator\CLI\HDTCG.XMLGenerator.Console.exe" };
            Tile tile03 = new Tile() { Name = "TEF", View = "ToolView", ShortDescription = "Target Execution Framework Tool" };

            HubSection hubSection01 = new HubSection();
            hubSection01.Name = "EPC Tools";
            hubSection01.Tiles.AddRange(new List<Tile> { tile01, tile02, tile03 });

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01 });

            CenterOfExcellence coe = new CenterOfExcellence() { Name = "EPC" };
            coe.Hub = hub;

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Insert(coe);
            repository.Save();
        }

        private static void PopulateCBM()
        {
            Tile tile01 = new Tile() { Name = "Report Builder", View = "ToolView", ShortDescription = "Report Builder Tool" };
            Tile tile02 = new Tile() { Name = "Excel Sheet", View = "ToolView", ShortDescription = "Excel Sheet Tool" };
            Tile tile03 = new Tile() { Name = "Screen Builder", View = "ToolView", ShortDescription = "Screen Builder Tool" };
            Tile tile04 = new Tile() { Name = "Option Builder", View = "ToolView", ShortDescription = "Option Builder Tool" };
            Tile tile05 = new Tile() { Name = "SQLite Integration", View = "ToolView", ShortDescription = "SQLite Integration Tool" };
            Tile tile06 = new Tile() { Name = "Sikuli", View = "ToolView", ShortDescription = "Sikuli Tool" };
            Tile tile07 = new Tile() { Name = "SQL Lite 3 Professional", View = "ToolView", ShortDescription = "SQL Lite 3 Professional Tool" };

            HubSection hubSection01 = new HubSection();
            hubSection01.Name = "CBM Tools";
            hubSection01.Tiles.AddRange(new List<Tile> { tile01, tile02, tile03, tile04, tile05, tile06, tile07 });

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01 });

            CenterOfExcellence coe = new CenterOfExcellence() { Name = "CBM" };
            coe.Hub = hub;

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Insert(coe);
            repository.Save();
        }

        private static void PopulateCommon()
        {
            Tile tile01 = new Tile() { Name = "HILite", View = "ToolView", ShortDescription = "HILite Tool" };
            Tile tile02 = new Tile() { Name = "Vector Cast", View = "ToolView", ShortDescription = "Vector Cast Tool" };

            HubSection hubSection01 = new HubSection();
            hubSection01.Name = "Common Tools";
            hubSection01.Tiles.AddRange(new List<Tile> { tile01, tile02 });

            Hub hub = new Hub();
            hub.HubSections.AddRange(new List<HubSection> { hubSection01 });

            CenterOfExcellence coe = new CenterOfExcellence() { Name = "Common" };
            coe.Hub = hub;

            CenterOfExcellenceRepository repository = new CenterOfExcellenceRepository();
            repository.Insert(coe);
            repository.Save();
        }

        private static string GenerateBackboneFormXMLString(RuleSet ruleSet)
        {
            if (ruleSet == null) return null;

            BackboneForm backboneForm = new BackboneForm();
            backboneForm.FormInformation = new BackboneFormFormInformation();
            backboneForm.FormInformation.Name = "Backbone Tool";
            backboneForm.FormInformation.Category = "IO";

            int displayIndex = 0;
            List<string> dependenciesNameList = new List<string>();
            var backboneProperties = new List<BackboneFormBackboneProperty>();
            foreach (Rule rule in ruleSet.RuleList)
            {
                switch (rule.Type)
                {
                    case "Text":
                        {
                            BackboneFormBackboneProperty backboneProperty = new BackboneFormBackboneProperty();
                            backboneProperty.Name = "text" + displayIndex;
                            backboneProperty.ChildName = "String";
                            backboneProperty.DisplayName = rule.Expression;
                            backboneProperty.DisplayIndex = (byte)displayIndex;
                            backboneProperty.Required = rule.IsOptional;
                            backboneProperty.IsEditable = true;
                            backboneProperty.FieldVisibility = 0;
                            backboneProperty.PropertyType = "STRING";
                            backboneProperty.ParameterName = rule.ParameterName;

                            BackboneFormBackbonePropertyString stringProperty = new BackboneFormBackbonePropertyString();
                            stringProperty.Value = string.Empty;

                            backboneProperty.String = stringProperty;
                            backboneProperties.Add(backboneProperty);
                            dependenciesNameList.Add(backboneProperty.Name);
                            break;
                        }
                    case "File Path":
                        {
                            BackboneFormBackboneProperty backboneProperty = new BackboneFormBackboneProperty();
                            backboneProperty.Name = "fileDialog" + displayIndex;
                            backboneProperty.ChildName = "FileDialog";
                            backboneProperty.DisplayName = rule.Expression;
                            backboneProperty.DisplayIndex = (byte)displayIndex;
                            backboneProperty.Required = rule.IsOptional;
                            backboneProperty.IsEditable = true;
                            backboneProperty.FieldVisibility = 0;
                            backboneProperty.PropertyType = "BROWSEFILE";
                            backboneProperty.ParameterName = rule.ParameterName;

                            var filePathRule = rule as FilePathRule;
                            BackboneFormBackbonePropertyFileDialog fileDialog = new BackboneFormBackbonePropertyFileDialog();
                            fileDialog.Value = string.Empty;
                            fileDialog.FilterName = filePathRule.FilterName;
                            fileDialog.FileExtension = string.Format(".{0}", filePathRule.Filter);

                            backboneProperty.FileDialog = fileDialog;
                            backboneProperties.Add(backboneProperty);
                            dependenciesNameList.Add(backboneProperty.Name);
                            break;
                        }
                    case "Folder Directory":
                        {
                            BackboneFormBackboneProperty backboneProperty = new BackboneFormBackboneProperty();
                            backboneProperty.Name = "folderDialog" + displayIndex;
                            backboneProperty.ChildName = "FolderDialog";
                            backboneProperty.DisplayName = rule.Expression;
                            backboneProperty.DisplayIndex = (byte)displayIndex;
                            backboneProperty.Required = rule.IsOptional;
                            backboneProperty.IsEditable = true;
                            backboneProperty.FieldVisibility = 0;
                            backboneProperty.PropertyType = "BROWSEFOLDER";
                            backboneProperty.ParameterName = rule.ParameterName;
                            
                            BackboneFormBackbonePropertyFolderDialog folderDirectory = new BackboneFormBackbonePropertyFolderDialog();
                            folderDirectory.Value = string.Empty;

                            backboneProperty.FolderDialog = folderDirectory;
                            backboneProperties.Add(backboneProperty);
                            dependenciesNameList.Add(backboneProperty.Name);
                            break;
                        }
                    default:
                        {
                            BackboneFormBackboneProperty backboneProperty = new BackboneFormBackboneProperty();
                            backboneProperty.Name = "text" + displayIndex;
                            backboneProperty.ChildName = "String";
                            backboneProperty.DisplayName = rule.Expression;
                            backboneProperty.DisplayIndex = (byte)displayIndex;
                            backboneProperty.Required = rule.IsOptional;
                            backboneProperty.IsEditable = true;
                            backboneProperty.FieldVisibility = 0;
                            backboneProperty.PropertyType = "STRING";
                            backboneProperty.ParameterName = rule.ParameterName;

                            BackboneFormBackbonePropertyString stringProperty = new BackboneFormBackbonePropertyString();
                            stringProperty.Value = string.Empty;

                            backboneProperty.String = stringProperty;
                            backboneProperties.Add(backboneProperty);
                            dependenciesNameList.Add(backboneProperty.Name);
                            break;
                        }
                }
                displayIndex++;
            }

            BackboneFormBackboneProperty bProperty = new BackboneFormBackboneProperty();
            bProperty.Name = "button" + displayIndex;
            bProperty.ChildName = "Button";
            bProperty.DisplayName = string.Empty;
            bProperty.DisplayIndex = (byte)displayIndex;
            bProperty.Required = true;
            bProperty.IsEditable = true;
            bProperty.FieldVisibility = 0;
            bProperty.PropertyType = "BUTTON";
            bProperty.ParameterName = "EXECUTE";

            BackboneFormBackbonePropertyButton button = new BackboneFormBackbonePropertyButton();
            button.Value = " Execute ";

            bProperty.Button = button;

            var dependencies = new List<BackboneFormBackbonePropertyDependent>();
            foreach (string dependentName in dependenciesNameList)
            {
                BackboneFormBackbonePropertyDependent dependent = new BackboneFormBackbonePropertyDependent();
                dependent.Name = dependentName;
                dependencies.Add(dependent);
            }
            bProperty.Dependencies = dependencies.ToArray();
            backboneProperties.Add(bProperty);

            backboneForm.BackboneProperties = backboneProperties.ToArray();

            string xmlString = SerializeToString(backboneForm);
            return xmlString;
        }

        private static string SerializeToString(BackboneForm backboneForm)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BackboneForm));
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, backboneForm);
            string xmlString = stringWriter.GetStringBuilder().ToString();
            //TextReader textReader = new StringReader(xmlString);
            return xmlString;
        }
    }
}
