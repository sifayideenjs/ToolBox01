using Backbone.Common.Interface;
using Backbone.Common.Model;
using Backbone.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.UI.Utilities
{
    public static class Helper
    {
        public static IBackbonePropertyViewModel WrapBackbonePropertyInViewModel(object backboneProperty, BackboneProperty propertySettings, Type t = null, Action<object> set = null, Func<object> get = null)
        {
            //if (backboneProperty is BackboneAutosize)
            //{
            //    return new BackboneAutoSizeViewModel(backboneProperty as BackboneAutosize, propertySettings);
            //}

            if (backboneProperty is BackboneDouble)
            {
                if (((BackboneDouble)backboneProperty).ViewModel != null) return ((BackboneDouble)backboneProperty).ViewModel;
                return new BackboneDoubleViewModel(backboneProperty as BackboneDouble, propertySettings);
            }

            if (backboneProperty is BackboneBrowseFile)
            {
                if (((BackboneBrowseFile)backboneProperty).ViewModel != null) return ((BackboneBrowseFile)backboneProperty).ViewModel;
                return new BackboneBrowseFileViewModel(backboneProperty as BackboneBrowseFile, propertySettings);
            }

            //if (backboneProperty is BackboneEnum)
            //{
            //    return new BackboneEnumViewModel(backboneProperty as BackboneEnum, propertySettings);
            //}

            //if (backboneProperty is LibraryBase)
            //{
            //    return new BackboneComboBoxListViewModel(backboneProperty as LibraryBase, propertySettings);
            //}

            //if (backboneProperty is ModelBase)
            //{
            //    return new BackboneComboBoxListViewModel(backboneProperty as ModelBase, propertySettings);
            //}

            //if (backboneProperty is Guid)
            //{
            //    return new BackboneComboBoxListViewModel((Guid)backboneProperty, propertySettings);
            //}

            var vm = new BackbonePropertyViewModel<object>(backboneProperty, propertySettings, set, get);
            if (set != null) vm.SetValueAction = set;
            return vm;
        }
    }
}
