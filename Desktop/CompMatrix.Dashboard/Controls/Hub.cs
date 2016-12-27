using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToolBox.Dashboard.Controls
{
    [TemplatePart(Name = "PART_Scroll", Type = typeof(ScrollViewer))]
    public class Hub : ItemsControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string),
            typeof(Hub), new PropertyMetadata(default(string)));

        private bool _isScrollEnabled = true;
        private bool _mouseDown;
        private Point _scrollStartOffset;
        private Point _scrollStartPoint;
        private ScrollViewer _scrollViewer;

        static Hub()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Hub), new FrameworkPropertyMetadata(typeof(Hub)));
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = (ScrollViewer)GetTemplateChild("PART_Scroll");

            if (_scrollViewer != null)
            {
                _scrollViewer.PreviewMouseDown += Hub_PreviewMouseDown;
                _scrollViewer.PreviewMouseMove += Hub_PreviewMouseMove;
                _scrollViewer.PreviewMouseUp += Hub_PreviewMouseUp;
                _scrollViewer.PreviewMouseWheel += Hub_PreviewMouseWheel;
                //TODO
            }
        }

        private void Hub_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_scrollViewer != null)
            {
                var isScrollPositive = e.Delta > 0;
                var currentX = _scrollViewer.HorizontalOffset;

                _scrollViewer.ScrollToHorizontalOffset(
                    currentX + e.Delta);

                e.Handled = true;
            }
        }

        private void Hub_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_scrollViewer != null && _scrollViewer.IsMouseOver)
            {
                // Save starting point, used later when determining how much to scroll.
                _scrollStartPoint = e.GetPosition(this);
                _scrollStartOffset.X = _scrollViewer.HorizontalOffset;
                _scrollStartOffset.Y = _scrollViewer.VerticalOffset;

                // Update the cursor if can scroll or not.
                Cursor = (_scrollViewer.ExtentWidth >
                          _scrollViewer.ViewportWidth) ||
                         (_scrollViewer.ExtentHeight > _scrollViewer.ViewportHeight)
                    ? Cursors.ScrollAll
                    : Cursors.Arrow;

                //this.CaptureMouse();
                _mouseDown = true;
                //Trace.WriteLine("Mouse Captured1? " + this.IsMouseCaptured);
            }

            OnPreviewMouseDown(e);
        }

        private void Hub_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //Trace.WriteLine("Mouse Captured2? " + mouseDown);
            if (_mouseDown) //this.IsMouseCaptured)
            {
                // Get the new scroll position.
                var point = e.GetPosition(this);

                // Determine the new amount to scroll.
                var delta = new Point(
                    (point.X > _scrollStartPoint.X)
                        ? -(point.X - _scrollStartPoint.X)
                        : (_scrollStartPoint.X - point.X),
                    (point.Y > _scrollStartPoint.Y)
                        ? -(point.Y - _scrollStartPoint.Y)
                        : (_scrollStartPoint.Y - point.Y));

                // Scroll to the new position.
                //Trace.WriteLine("Scroll AppScrollView to:" + this.scrollStartOffset.X + delta.X);

                _scrollViewer.ScrollToHorizontalOffset(
                    _scrollStartOffset.X + delta.X);
                //AppMain.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
            }

            OnPreviewMouseMove(e);
        }

        private void Hub_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseDown)
            {
                Cursor = Cursors.Arrow;
                _mouseDown = false;
                //this.ReleaseMouseCapture();
            }

            OnPreviewMouseUp(e);
        }

        public void SetScrollable(bool isScrollable)
        {
            _isScrollEnabled = isScrollable;
            Cursor = Cursors.Arrow;
            _mouseDown = false;
        }
    }
}
