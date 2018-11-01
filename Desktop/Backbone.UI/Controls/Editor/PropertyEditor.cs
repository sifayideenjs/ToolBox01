
using Backbone.Common.Utilities;
///=======================================================================================
/// Copyright                            : Copyright (c) 2013 
/// Company Name                         : TRANE
/// Application Name                     : TRACE 800
/// File Name                            : PropertyEditor.cs
/// Author Name                          : Vallarasu Sambathkumar
/// Created Date                         : 4/2/2013 6:11:39 PM
/// Description                          : Defines PropertyGrid like editor with added support for templating and category editor.
///=======================================================================================
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Backbone.Common.UI.Controls
{
    /// <summary>
    ///     Defines PropertyGrid like editor with added support for templating and category editor.
    /// </summary>
    [TemplatePart(Name = "PART_PRESENTER", Type = typeof (ListView))]
    public class PropertyEditor : Control
    {
        private EditorTemplateSelector _defTemplateSelector;
        private ListView _editorPresenter;
        private DataTemplate _readOnlyTemplate;

        #region Overriden Members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _editorPresenter = GetTemplateChild("PART_PRESENTER") as ListView;

            _readOnlyTemplate = SharedDictionaryManager.SharedDictionary["readOnlyTemplate"] as DataTemplate;

            _defTemplateSelector =
                SharedDictionaryManager.SharedDictionary["defTemplateSelector"] as EditorTemplateSelector;
        }

        #endregion

        #region Ctor

        public PropertyEditor()
        {
            Loaded += (_, __) => UpdateMemberInfo();
        }

        static PropertyEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (PropertyEditor),
                new FrameworkPropertyMetadata(typeof (PropertyEditor)));
        }

        #endregion

        #region Implementations

        private void OnObjectSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;

            UpdateMemberInfo();
        }

        private void UpdateMemberInfo()
        {
            if (_editorPresenter == null) return;

            if (ObjectSource == null)
            {
                _editorPresenter.ItemsSource = null;
                return;
            }

            try
            {
                /* Getter and Setter of PropertyDescriptor */

                Func<PropertyDescriptor, object> get = p => p.GetValue(ObjectSource);

                Action<PropertyDescriptor, object> set =
                    (p, v) => p.SetValue(ObjectSource, Convert.ChangeType(v, p.PropertyType));

                /* Honors DisplayAttribute */

                Func<MemberInfo, bool> isGrouped =
                    m =>
                        m.PInfo.Attributes.OfType<DisplayAttribute>().Count() != 0 &&
                        !string.IsNullOrEmpty(m.PInfo.Attributes.OfType<DisplayAttribute>().First().GroupName);
                Func<MemberInfo, bool> notGrouped =
                    m =>
                        m.PInfo.Attributes.OfType<DisplayAttribute>().Count() == 0 ||
                        string.IsNullOrEmpty(m.PInfo.Attributes.OfType<DisplayAttribute>().First().GroupName);

                /* Find a template from User's TemplateSelector or from default template selector */

                Func<PropertyDescriptor, DataTemplate> template = m => EditorMode == EditorMode.ReadOnly
                    ? _readOnlyTemplate
                    : (TemplateSelector == null || TemplateSelector.SelectTemplate(m, this) == null)
                        ? _defTemplateSelector.SelectTemplate(m, this)
                        : TemplateSelector.SelectTemplate(m, this);

                /* Subscribe to member changes so does the editor is in sync with entity */

                Action<MemberInfo> subscribe = m =>
                    m.PInfo.AddValueChanged(ObjectSource, (s, e) => m.Value = get(m.PInfo));

                var members = TypeDescriptor.GetProperties(ObjectSource)
                    .OfType<PropertyDescriptor>()
                    .Where(p => p.IsBrowsable) /* Honors BrowsableAttribute */
                    .Where(p => PropertyFilter == null || PropertyFilter.Invoke(p))
                    .Select(p => MemberInfo.Create(p, template(p), get, set))
                    .ToList();

                members.ForEach(subscribe);

                var displayGroups = members
                    .Where(isGrouped)
                    .GroupBy(m => m.PInfo.Attributes.OfType<DisplayAttribute>().First().GroupName)
                    .Select(g => MemberInfo.CreateGroup(g.Key, g.AsEnumerable()))
                    .ToList();

                if (GroupEditorTemplateSelector != null)
                    displayGroups.ForEach(m => m.SetTemplate(
                        EditorMode == EditorMode.ReadOnly
                            ? _readOnlyTemplate
                            : GroupEditorTemplateSelector.SelectTemplate(m, this)));

                var memSrc = displayGroups.Union(members.Where(notGrouped));

                var source = CollectionViewSource.GetDefaultView(memSrc);

                /* Honors Category Name attribute */

                var category = ReflectionUtility.GetPropertyName<MemberInfo>(m => m.CategoryName);
                var order = ReflectionUtility.GetPropertyName<MemberInfo>(m => m.Order);
                var name = ReflectionUtility.GetPropertyName<MemberInfo>(m => m.Name);

                source.GroupDescriptions.Add(new PropertyGroupDescription(category));
                source.SortDescriptions.Add(new SortDescription(category, ListSortDirection.Ascending));
                source.SortDescriptions.Add(new SortDescription(order, ListSortDirection.Ascending));
                source.SortDescriptions.Add(new SortDescription(name, ListSortDirection.Ascending));

                _editorPresenter.ItemsSource = source;

                /* Honors DefaultProperty Attribute */

                if (EditorMode == EditorMode.ReadOnly) return;

                var def = TypeDescriptor.GetDefaultProperty(ObjectSource);

                if (def == null) return;

                _editorPresenter.SelectedItem = members.First(m => m.PInfo.Name.Equals(def.Name));
            }
            catch (Exception ex)
            {
                // TODO : Create a Routed Event to pass on in case of exception, rather leaving it unhandled.
                throw;
            }
        }

        #endregion

        #region Memebers

        /// <summary>
        ///     When set to true applies ReadOnlyPresenter template to items when focus is lost. Otherwise inheriters has to handle
        ///     it in triggers with IsFocused property.
        /// </summary>
        public bool HandleFocusChange
        {
            get { return (bool) GetValue(HandleFocusChangeProperty); }
            set { SetValue(HandleFocusChangeProperty, value); }
        }

        public static readonly DependencyProperty HandleFocusChangeProperty =
            DependencyProperty.Register("HandleFocusChange", typeof (bool), typeof (PropertyEditor),
                new PropertyMetadata(true));

        public EditorMode EditorMode
        {
            get { return (EditorMode) GetValue(EditorModeProperty); }
            set { SetValue(EditorModeProperty, value); }
        }

        public object ObjectSource
        {
            get { return GetValue(ObjectSourceProperty); }
            set { SetValue(ObjectSourceProperty, value); }
        }

        public DataTemplateSelector TemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        public DataTemplateSelector GroupEditorTemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(GroupEditorTemplateSelectorProperty); }
            set { SetValue(GroupEditorTemplateSelectorProperty, value); }
        }

        public GroupStyleSelector GroupStyleSelector
        {
            get { return (GroupStyleSelector) GetValue(GroupStyleSelectorProperty); }
            set { SetValue(GroupStyleSelectorProperty, value); }
        }

        public bool AllowWrapping
        {
            get { return (bool) GetValue(AllowWrappingProperty); }
            set { SetValue(AllowWrappingProperty, value); }
        }


        /// <summary>
        ///     Gets or sets a method used to determine if a property is suitable for inclusion in the editor.
        /// </summary>
        /// <returns>
        ///     A delegate that represents the method used to determine if an item is suitable for inclusion in the view.
        /// </returns>
        public Predicate<PropertyDescriptor> PropertyFilter
        {
            get { return (Predicate<PropertyDescriptor>) GetValue(PropertyFilterProperty); }
            set { SetValue(PropertyFilterProperty, value); }
        }


        public static readonly DependencyProperty AllowWrappingProperty =
            DependencyProperty.Register("AllowWrapping", typeof (bool), typeof (PropertyEditor),
                new PropertyMetadata(false));

        public static readonly DependencyProperty GroupStyleSelectorProperty =
            DependencyProperty.Register("GroupStyleSelector", typeof (GroupStyleSelector), typeof (PropertyEditor),
                new UIPropertyMetadata(OnGroupStyleSelectorPropertyChanged));

        public static readonly DependencyProperty GroupEditorTemplateSelectorProperty =
            DependencyProperty.Register("GroupEditorTemplateSelector", typeof (DataTemplateSelector),
                typeof (PropertyEditor), new PropertyMetadata(null));

        public static readonly DependencyProperty TemplateSelectorProperty =
            DependencyProperty.Register("TemplateSelector", typeof (DataTemplateSelector), typeof (PropertyEditor),
                new UIPropertyMetadata(OnTemplateSelectorPropertyChanged));

        public static readonly DependencyProperty EditorModeProperty =
            DependencyProperty.Register("EditorMode", typeof (EditorMode), typeof (PropertyEditor),
                new UIPropertyMetadata(EditorMode.Edit, (s, _) => ((PropertyEditor) s).UpdateMemberInfo()));

        public static readonly DependencyProperty ObjectSourceProperty =
            DependencyProperty.Register("ObjectSource", typeof (object), typeof (PropertyEditor),
                new UIPropertyMetadata(OnObjectSourcePropertyChanged));

        public static readonly DependencyProperty PropertyFilterProperty =
            DependencyProperty.Register("PropertyFilter", typeof (Predicate<PropertyDescriptor>),
                typeof (PropertyEditor));

        private static void OnObjectSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as PropertyEditor;

            if (editor == null) return;

            editor.OnObjectSourceChanged(e);
        }

        private static void OnTemplateSelectorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as PropertyEditor;

            if (editor == null) return;

            if (e.NewValue == null)
                editor.TemplateSelector = editor.FindResource("defTemplateSelector") as DataTemplateSelector;
        }

        private static void OnGroupStyleSelectorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as PropertyEditor;

            if (editor == null) return;

            editor.GroupStyleSelector = e.NewValue as GroupStyleSelector;
        }

        #endregion
    }
}