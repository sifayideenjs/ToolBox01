using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Models
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class BackboneForm
    {

        private BackboneFormFormInformation formInformationField;

        private BackboneFormBackboneProperty[] backbonePropertiesField;

        /// <remarks/>
        public BackboneFormFormInformation FormInformation
        {
            get
            {
                return this.formInformationField;
            }
            set
            {
                this.formInformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("BackboneProperty", IsNullable = false)]
        public BackboneFormBackboneProperty[] BackboneProperties
        {
            get
            {
                return this.backbonePropertiesField;
            }
            set
            {
                this.backbonePropertiesField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BackboneFormFormInformation
    {

        private string nameField;

        private string categoryField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BackboneFormBackboneProperty
    {

        private BackboneFormBackbonePropertyButton buttonField;

        private BackboneFormBackbonePropertyDependent[] dependenciesField;

        private BackboneFormBackbonePropertyFolderDialog folderDialogField;

        private BackboneFormBackbonePropertyFileDialog fileDialogField;

        private BackboneFormBackbonePropertyDouble doubleField;

        private BackboneFormBackbonePropertyString stringField;

        private string nameField;

        private string childNameField;

        private string displayNameField;

        private string parameterNameField;

        private byte displayIndexField;

        private bool requiredField;

        private bool isEditableField;

        private byte fieldVisibilityField;

        private string propertyTypeField;

        /// <remarks/>
        public BackboneFormBackbonePropertyButton Button
        {
            get
            {
                return this.buttonField;
            }
            set
            {
                this.buttonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Dependent", IsNullable = false)]
        public BackboneFormBackbonePropertyDependent[] Dependencies
        {
            get
            {
                return this.dependenciesField;
            }
            set
            {
                this.dependenciesField = value;
            }
        }

        /// <remarks/>
        public BackboneFormBackbonePropertyFolderDialog FolderDialog
        {
            get
            {
                return this.folderDialogField;
            }
            set
            {
                this.folderDialogField = value;
            }
        }

        /// <remarks/>
        public BackboneFormBackbonePropertyFileDialog FileDialog
        {
            get
            {
                return this.fileDialogField;
            }
            set
            {
                this.fileDialogField = value;
            }
        }

        /// <remarks/>
        public BackboneFormBackbonePropertyDouble Double
        {
            get
            {
                return this.doubleField;
            }
            set
            {
                this.doubleField = value;
            }
        }

        /// <remarks/>
        public BackboneFormBackbonePropertyString String
        {
            get
            {
                return this.stringField;
            }
            set
            {
                this.stringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ChildName
        {
            get
            {
                return this.childNameField;
            }
            set
            {
                this.childNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DisplayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ParameterName
        {
            get
            {
                return this.parameterNameField;
            }
            set
            {
                this.parameterNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte DisplayIndex
        {
            get
            {
                return this.displayIndexField;
            }
            set
            {
                this.displayIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Required
        {
            get
            {
                return this.requiredField;
            }
            set
            {
                this.requiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsEditable
        {
            get
            {
                return this.isEditableField;
            }
            set
            {
                this.isEditableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte FieldVisibility
        {
            get
            {
                return this.fieldVisibilityField;
            }
            set
            {
                this.fieldVisibilityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PropertyType
        {
            get
            {
                return this.propertyTypeField;
            }
            set
            {
                this.propertyTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BackboneFormBackbonePropertyButton
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BackboneFormBackbonePropertyDependent
    {

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BackboneFormBackbonePropertyFolderDialog
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BackboneFormBackbonePropertyFileDialog
    {

        private string valueField;

        private string filterNameField;

        private string fileExtensionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FilterName
        {
            get
            {
                return this.filterNameField;
            }
            set
            {
                this.filterNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FileExtension
        {
            get
            {
                return this.fileExtensionField;
            }
            set
            {
                this.fileExtensionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BackboneFormBackbonePropertyDouble
    {

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BackboneFormBackbonePropertyString
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}
