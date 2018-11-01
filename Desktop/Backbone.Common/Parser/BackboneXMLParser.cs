using Backbone.Common.Model;
using Backbone.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Backbone.Common.Parser
{
    public static class BackboneXMLParser
    {
        public static BackboneFormViewModel GetBackboneFormViewModel(BackboneForm backboneForm)
        {
            var backboneFormViewModel = new BackboneFormViewModel(backboneForm);
            return backboneFormViewModel;
        }

        public static BackboneFormViewModel GetBackboneFormViewModel(string xmlFilePath)
        {
            var backboneForm = GetBackboneForm(xmlFilePath);
            var backboneFormViewModel = new BackboneFormViewModel(backboneForm);
            return backboneFormViewModel;
        }

        public static BackboneFormViewModel GetBackboneFormViewModel(Stream xmlFileStream)
        {
            var backboneForm = GetBackboneForm(xmlFileStream);
            var backboneFormViewModel = new BackboneFormViewModel(backboneForm);
            return backboneFormViewModel;
        }

        public static BackboneFormViewModel GetBackboneFormViewModel(TextReader textReader)
        {
            var backboneForm = GetBackboneForm(textReader);
            var backboneFormViewModel = new BackboneFormViewModel(backboneForm);
            return backboneFormViewModel;
        }

        private static BackboneForm GetBackboneForm(string xmlFilePath)
        {
            if (!File.Exists(xmlFilePath)) return null;
            XDocument xDocument = XDocument.Load(xmlFilePath);
            BackboneForm backboneForm = GenerateBackboneForm(xDocument);
            return backboneForm;
        }

        private static BackboneForm GetBackboneForm(Stream xmlFileStream)
        {
            XDocument xDocument = XDocument.Load(xmlFileStream);
            BackboneForm backboneForm = GenerateBackboneForm(xDocument);
            return backboneForm;
        }

        private static BackboneForm GetBackboneForm(TextReader textReader)
        {
            XDocument xDocument = XDocument.Load(textReader);
            BackboneForm backboneForm = GenerateBackboneForm(xDocument);
            return backboneForm;
        }

        private static BackboneForm GenerateBackboneForm(XDocument xDocument)
        {
            Func<string, PropertyType> getPropertyType = delegate (string propertyType)
            {
                PropertyType result = PropertyType.STRING;
                switch (propertyType.ToUpper())
                {
                    case "STRING":
                        {
                            result = PropertyType.STRING;
                            break;
                        }
                    case "DOUBLE":
                        {
                            result = PropertyType.DOUBLE;
                            break;
                        }
                    case "BROWSEFILE":
                        {
                            result = PropertyType.BROWSEFILE;
                            break;
                        }
                    case "BROWSEFOLDER":
                        {
                            result = PropertyType.BROWSEFOLDER;
                            break;
                        }
                    case "BUTTON":
                        {
                            result = PropertyType.BUTTON;
                            break;
                        }
                }
                return result;
            };

            Func<string, XElement, object> getPropertyObject = delegate (string propertyType, XElement propertyElement)
            {
                object result = null;
                switch (propertyType.ToUpper())
                {
                    case "STRING":
                        {
                            result = new BackboneString() { Value = propertyElement.Attribute("Value").Value };
                            break;
                        }
                    case "DOUBLE":
                        {
                            result = new BackboneDouble() { Value = Convert.ToDouble(propertyElement.Attribute("Value").Value) };
                            break;
                        }
                    case "BROWSEFILE":
                        {
                            result = new BackboneBrowseFile()
                            {
                                Value = propertyElement.Attribute("Value").Value,
                                FilterName = propertyElement.Attribute("FilterName").Value,
                                FileExtension = propertyElement.Attribute("FileExtension").Value
                            };
                            break;
                        }
                    case "BROWSEFOLDER":
                        {
                            result = new BackboneBrowseFolder() { Value = propertyElement.Attribute("Value").Value };
                            break;
                        }
                    case "BUTTON":
                        {
                            result = new BackboneButton() { Value = propertyElement.Attribute("Value").Value };
                            break;
                        }
                }
                return result;
            };

            Func<string, XElement, object> getDependecies = delegate (string propertyType, XElement dependecyElement)
            {
                List<BackboneDependent> result = null;
                if (dependecyElement == null) return result;
                switch (propertyType.ToUpper())
                {
                    case "BUTTON":
                        {
                            result = new List<BackboneDependent>();
                            var dependentElements = dependecyElement.Descendants("Dependent");
                            foreach(var d in dependentElements)
                            {
                                result.Add(new BackboneDependent() { Value = d.Attribute("Name").Value });
                            }
                            
                            break;
                        }
                }
                return result;
            };

            var backboneForm = new BackboneForm();

            XElement generalElement = xDocument
                    .Element("BackboneForm")
                    .Element("FormInformation");
            backboneForm.Name = generalElement.Element("Name").Value;
            backboneForm.Category = generalElement.Element("Category").Value;

            var elements = xDocument.Descendants("BackboneProperty");
            foreach (var c in elements)
            {
                var backboneProperty = new BackboneProperty()
                {
                    Name = c.Attribute("Name").Value,
                    ChildName = c.Attribute("ChildName").Value,
                    DisplayName = c.Attribute("DisplayName").Value,
                    ParameterName = c.Attribute("ParameterName").Value,
                    DisplayIndex = Convert.ToInt16(c.Attribute("DisplayIndex").Value),
                    Required = Convert.ToBoolean(c.Attribute("Required").Value),
                    IsEditable = Convert.ToBoolean(c.Attribute("IsEditable").Value),
                    FieldVisibility = (UserVisibility)Convert.ToInt16(c.Attribute("FieldVisibility").Value),
                    PropertyType = getPropertyType(c.Attribute("PropertyType").Value),
                    PropertyObject = getPropertyObject(c.Attribute("PropertyType").Value, c.Descendants(c.Attribute("ChildName").Value).First()),
                    DependencyObject = getDependecies(c.Attribute("PropertyType").Value, c.Descendants("Dependencies").FirstOrDefault())
                };

                backboneForm.Properties.Add(backboneProperty);
            }

            return backboneForm;
        }
    }
}
