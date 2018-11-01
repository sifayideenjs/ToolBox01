using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ToolBox.Models;

namespace ToolBox.Common.Utilities
{
    public static class BackboneUtility
    {
        public static string GenerateBackboneFormXMLString(RuleSet ruleSet)
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
