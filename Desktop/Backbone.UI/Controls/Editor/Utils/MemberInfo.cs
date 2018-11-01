//=======================================================================================
// Copyright                            : Copyright (c) Trane 2013 
// Application Name                     : TRACE 800
// File Name                            : MemberInfo.cs
// Author Name                          : Vallarasu Sambathkumar
// Created Date                         : 4/2/2013 6:11:39 PM
// Description                          : Defines members and helpers for manipulating entity information for presentation and editing.
//=======================================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Backbone.Common.Utilities;

namespace Backbone.Common.UI.Controls
{
    /// <summary>
    ///     Defines members and helpers for manipulating entity information for presentation and editing.
    /// </summary>
    public class MemberInfo : INotifyPropertyChanged
    {
        private static readonly Func<PropertyDescriptor, string> displayName = p =>
        {
            if (p.Attributes.OfType<DisplayAttribute>().Count() != 0)
            {
                var name = p.Attributes.OfType<DisplayAttribute>().First().GetName();
            }

            if (p.Attributes.OfType<DisplayNameAttribute>().Count() != 0)
            {
                var name = p.Attributes.OfType<DisplayNameAttribute>().First().DisplayName;
            }

            return SplitName(p.Name);
        };

        private static readonly Func<PropertyDescriptor, string> categoryName =
            p => p.Attributes.OfType<CategoryAttribute>().Count() == 0
                ? string.Empty
                : p.Attributes.OfType<CategoryAttribute>().First().Category;

        private static readonly Func<PropertyDescriptor, int> displayOrder = p =>
        {
            var attrib = p.Attributes.OfType<DisplayAttribute>();
            if (attrib.Count() == 0) return int.MaxValue;
            return attrib.First().GetOrder().HasValue ? (int) attrib.First().GetOrder() : int.MaxValue;
        };

        private static readonly Func<PropertyDescriptor, bool> required =
            p => p.Attributes.OfType<RequiredAttribute>().Count() != 0;

        private static readonly Func<PropertyDescriptor, string> requiredFieldErrorMessage = p =>
            p.Attributes.OfType<RequiredAttribute>().Count() != 0
                ? p.Attributes.OfType<RequiredAttribute>().First().ErrorMessage
                : string.Empty;

        private static readonly Func<string, string> SplitName = name => name
            .AsEnumerable()
            .Select(c => c.ToString())
            .Aggregate((agg, c) =>
                c.Equals("_")
                    ? agg + " " //  Just replace _
                    : char.IsUpper(char.Parse(c)) && !char.IsUpper(agg.Last())
                        ? agg + " " + c //  If value is upper append " "
                        : agg + c); //  No special case just append

        private string _validationErrorMessage = string.Empty;
        private IEnumerable<ValidationAttribute> _validators;
        private object _value;

        private MemberInfo()
        {
            CategoryName = string.Empty;
            Order = int.MaxValue;
        }

        public MemberInfo(string name, string groupName, Type type, DataTemplate template, object value, bool readOnly)
        {
            Name = name;
            Type = type;
            CategoryName = groupName;
            Template = template;
            Value = value;
            ReadOnly = readOnly;
            Choices = !ReadOnly && Type.IsEnum
                ? Enum.GetValues(Type)
                : null;
        }

        public Type Type { get; private set; }
        public string Name { get; private set; }
        public string CategoryName { get; private set; }
        public DataTemplate Template { get; private set; }
        public bool Required { get; private set; }
        public string RequiredFieldErrorMessage { get; set; }

        public string ValidationErrorMessage
        {
            get { return _validationErrorMessage; }
            set
            {
                _validationErrorMessage = value;

                RaisePropertyChanged();
            }
        }

        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public PropertyDescriptor PInfo { get; set; }
        public int Order { get; set; }
        private Action<PropertyDescriptor, object> Setter { get; set; }

        public object Value
        {
            get
            {
                if (IsGrouped)
                    return GroupedProperties
                        .OrderBy(d => d.Value.Order)
                        .Select(d => d.Value)
                        .Aggregate(string.Empty, (acc, m) => acc + " " + m.Value.ToString());

                return _value;
            }
            set
            {
                if (Equals(_value, value)) return;

                OnBeforeValueChange(_value, value);

                _value = value;

                if (Setter != null && !PInfo.IsReadOnly)
                    Setter(PInfo, value);

                OnValueChanged();

                RaisePropertyChanged();
            }
        }

        public bool ReadOnly { get; private set; }
        public object Choices { get; private set; }
        public bool IsGrouped { get; set; }
        public Dictionary<string, MemberInfo> GroupedProperties { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnBeforeValueChange(object oldValue, object newValue)
        {
            if (newValue == null) return;

            TypeDescriptor.GetProperties(newValue)
                .OfType<PropertyDescriptor>()
                .ToList()
                .ForEach(d =>
                    d.AddValueChanged(newValue,
                        (o, e) => RaisePropertyChanged(ReflectionUtility.GetPropertyName<MemberInfo>(m => m.Value))));
        }

        private void OnValueChanged()
        {
            ValidationErrorMessage = PInfo.Attributes
                .OfType<ValidationAttribute>()
                .Where(v => !v.IsValid(Value))
                .Select(v => v.ErrorMessage)
                .Aggregate(string.Empty, (acc, seed) => acc + " " + seed);
        }

        public static MemberInfo Create(PropertyDescriptor pi, DataTemplate template,
            Func<PropertyDescriptor, object> get, Action<PropertyDescriptor, object> set)
        {
            var mi = new MemberInfo
            {
                PInfo = pi,
                Name = displayName(pi),
                CategoryName = categoryName(pi),
                Type = pi.PropertyType,
                Template = template,
                Value = get(pi),
                ReadOnly = pi.IsReadOnly,
                Required = required(pi),
                Order = displayOrder(pi),
                Setter = set
            };

            if (mi.Required) mi.RequiredFieldErrorMessage = requiredFieldErrorMessage(pi);

            var rattrib = pi.Attributes.OfType<RangeAttribute>().FirstOrDefault();

            if (rattrib == null) return mi;

            var conv = TypeDescriptor.GetConverter(rattrib.OperandType);

            if (conv.CanConvertTo(typeof (double)))
            {
                mi.Minimum = (double) conv.ConvertTo(rattrib.Minimum, typeof (double));
                mi.Maximum = (double) conv.ConvertTo(rattrib.Maximum, typeof (double));
            }

            return mi;
        }

        public static MemberInfo CreateGroup(string groupName, IEnumerable<MemberInfo> infos)
        {
            return new MemberInfo
            {
                Name = groupName,
                IsGrouped = true,
                Order = infos.Min(m => m.Order),
                GroupedProperties = infos
                    .OrderBy(m => m.Order)
                    .ToDictionary(m => m.Name)
            };
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler == null) return;

            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void SetTemplate(DataTemplate template)
        {
            Template = template;
        }
    }
}