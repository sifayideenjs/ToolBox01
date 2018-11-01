using Backbone.Common.Interface;
using Backbone.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.Model
{
    public class BackboneBrowseFile
    {
        public BackboneBrowseFile()
        {
            FilterName = "All File";
            FileExtension = ".*";
        }

        public BackboneBrowseFile(IBackboneProperty propertySettings) : this()
        {
            //Initialize(propertySettings);
        }

        public string Value { get; set; }

        public string FilterName { get; set; }

        public string FileExtension { get; set; }

        public BackboneBrowseFileViewModel ViewModel { get; set; }

        public static BackboneDouble CreateInstance<T>(Type classType, Expression<Func<T>> propertyExpression)
        {
            return null;
        }
    }
}
