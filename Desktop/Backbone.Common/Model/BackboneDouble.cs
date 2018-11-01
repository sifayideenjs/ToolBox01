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
    public class BackboneDouble
    {
        public BackboneDouble()
        {
        }

        public BackboneDouble(IBackboneProperty propertySettings)
        {
            //Initialize(propertySettings);
        }

        public Double? Value { get; set; }

        public bool HasValue
        {
            get
            {
                return Value.HasValue;
            }
        }

        public BackboneDoubleViewModel ViewModel { get; set; }        

        public static BackboneDouble CreateInstance<T>(Type classType, Expression<Func<T>> propertyExpression)
        {
            //IBackboneProperty propertySettings = BackbonePropertyManager.GetModelProperty(classType, propertyExpression);
            //return new BackboneDouble(propertySettings);
            return null;
        }
    }
}
