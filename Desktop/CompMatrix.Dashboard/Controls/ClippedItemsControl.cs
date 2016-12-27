using ToolBox.Dashboard.Utilities;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToolBox.Dashboard.Controls
{
    public class ClippedItemsControl : ItemsControl
    {
        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register(
            "FooterTemplate", typeof(DataTemplate), typeof(ClippedItemsControl));

        private ClippedStackPanel _panel;

        static ClippedItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClippedItemsControl),
                new FrameworkPropertyMetadata(typeof(ClippedItemsControl)));
        }

        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            RefreshPanel();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RefreshPanel();
        }

        private void RefreshPanel()
        {
            // Unregister ClippedChanged event
            if (_panel != null)
                _panel.ClippedChanged -= Panel_ClippedChanged;

            var itemsPresenter = TreeHelper.GetVisualChild<ItemsPresenter>(this);

            if (itemsPresenter == null)
            {
                // ItemsPresenter is not found in nested ItemsControl scenarios
                // The visual tree needs a forced refresh to find the panel
                ApplyTemplate();

                var control = TreeHelper.GetVisualChild<ItemsControl>(this);

                if (control != null)
                {
                    control.ApplyTemplate();
                    itemsPresenter = TreeHelper.GetVisualChild<ItemsPresenter>(control);

                    if (itemsPresenter == null)
                        return;

                    itemsPresenter.ApplyTemplate();
                    if (VisualTreeHelper.GetChildrenCount(itemsPresenter) == 0)
                        return;
                }
            }

            if(itemsPresenter != null)
            {
                var panel = VisualTreeHelper.GetChild(itemsPresenter, 0) as ClippedStackPanel;
                if (panel == null)
                    return;

                // Update panel and register for ClippedChanged event
                _panel = panel;
                _panel.ClippedChanged += Panel_ClippedChanged;
            }
        }

        /// <summary>
        ///     Update Clipped State
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private void Panel_ClippedChanged(object obj, ClippedChangedEventArgs args)
        {
            if (args.Clipped)
                VisualStateManager.GoToState(this, "Clipped", false);
            else
                VisualStateManager.GoToState(this, "Normal", false);
        }
    }
}
