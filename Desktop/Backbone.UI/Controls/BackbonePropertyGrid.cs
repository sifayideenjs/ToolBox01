using Backbone.Common.Interface;
using Backbone.Common.Model;
using Backbone.Common.UI.Utilities;
using Backbone.Common.Utilities;
using Backbone.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Backbone.Common.UI.Controls
{
    [TemplatePart(Name = "PART_ItemsPresenter", Type = typeof(ItemsControl))]
    public class BackbonePropertyGrid : Control
    {
        private static BackbonePropertyGrid _grid;
        private BackbonePropertyTemplateSelector _defTemplateSelector;
        private ItemsControl _itemsPresenter;        

        #region Constructor

        static BackbonePropertyGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BackbonePropertyGrid), new FrameworkPropertyMetadata(typeof(BackbonePropertyGrid)));
        }

        public BackbonePropertyGrid()
        {
            Loaded += delegate { GenerateMemberInfo(); };
        }

        #endregion

        #region Overriden Members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _itemsPresenter = GetTemplateChild("PART_ItemsPresenter") as ItemsControl;
            _defTemplateSelector = SharedDictionaryManager.SharedDictionary["BackboneTemplateSelector"] as BackbonePropertyTemplateSelector;
        }

        #endregion

        #region Properties        
        
        public static readonly DependencyProperty BackboneClassProperty =
            DependencyProperty.Register("BackboneClass", typeof(BackboneFormViewModel), typeof(BackbonePropertyGrid), new PropertyMetadata(null, OnBackboneClassPropertyChanged));

        public static readonly DependencyProperty TemplateSelectorProperty =
            DependencyProperty.Register("TemplateSelector", typeof(BackbonePropertyTemplateSelector), typeof(BackbonePropertyGrid), new UIPropertyMetadata(OnTemplateSelectorPropertyChanged));

        public static readonly DependencyProperty EditorModeProperty =
            DependencyProperty.Register("EditorMode", typeof(EditorMode), typeof(BackbonePropertyGrid), new UIPropertyMetadata(EditorMode.Edit, (s, _) => ((BackbonePropertyGrid)s).GenerateMemberInfo()));

        public DataTemplateSelector TemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        public BackboneFormViewModel BackboneClass
        {
            get { return (BackboneFormViewModel)GetValue(BackboneClassProperty); }
            set { SetValue(BackboneClassProperty, value); }
        }

        public EditorMode EditorMode
        {
            get { return (EditorMode)GetValue(EditorModeProperty); }
            set { SetValue(EditorModeProperty, value); }
        }

        private static void OnBackboneClassPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            _grid = d as BackbonePropertyGrid;

            if (_grid == null) return;

            _grid.OnBackboneClassChanged(e);
        }

        private static void OnTemplateSelectorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //_defTemplateSelector = d as BackbonePropertyTemplateSelector;
        }

        #endregion

        #region AddItem
        public static readonly DependencyProperty ExecuteCommandProperty =
            DependencyProperty.Register(
                "ExecuteCommand",
                typeof(ICommand),
                typeof(BackbonePropertyGrid),
                new UIPropertyMetadata(null));
        public ICommand ExecuteCommand
        {
            get { return (ICommand)GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }
        #endregion

        #region Implementations

        private void OnBackboneClassChanged(DependencyPropertyChangedEventArgs e)
        {
            GenerateMemberInfo();
        }

        private void GenerateMemberInfo()
        {
            if (!IsLoaded || DesignerProperties.GetIsInDesignMode(this) || _itemsPresenter == null) return;

            if (BackboneClass == null)
            {
                _itemsPresenter.ItemsSource = null;
                return;
            }

            if (_itemsPresenter == null) _itemsPresenter = GetTemplateChild("PART_ItemsPresenter") as ItemsControl;
            if(_defTemplateSelector == null) _defTemplateSelector = SharedDictionaryManager.SharedDictionary["BackboneTemplateSelector"] as BackbonePropertyTemplateSelector;
            
            var memberInfos = BackboneClass.Properties
                                .Where(p => p.FieldVisibility != UserVisibility.Hidden)
                                .Select(p => p)
                                .ToList();

            var templateSelector = TemplateSelector ?? _defTemplateSelector;

            if (EditorMode == EditorMode.Edit) memberInfos.ForEach(info => info.Template = templateSelector.SelectTemplate(info, this));
            else memberInfos.ForEach(info => info.Template = _defTemplateSelector.BackboneReadOnlyTemplate);

            var viewSource = CollectionViewSource.GetDefaultView(memberInfos);
            //viewSource.GroupDescriptions.Add(new PropertyGroupDescription("FieldGroupName"));
            viewSource.SortDescriptions.Add(new SortDescription("DisplayIndex", ListSortDirection.Ascending));

            _itemsPresenter.ItemsSource = viewSource;
        }

        #endregion
    }
}
