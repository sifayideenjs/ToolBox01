using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToolBox.Dashboard.Controls
{
    public class ClippedStackPanel : StackPanel
    {
        public delegate void ClippedChangedEventHandler(object obj, ClippedChangedEventArgs args);

        public static readonly DependencyProperty ClippedProperty = DependencyProperty.Register("Clipped", typeof(bool),
            typeof(ClippedStackPanel), new PropertyMetadata(false));

        public bool Clipped
        {
            get { return (bool)GetValue(ClippedProperty); }
            set { SetValue(ClippedProperty, value); }
        }

        public event ClippedChangedEventHandler ClippedChanged;

        protected override Size MeasureOverride(Size constraint)
        {
            var maxHeight = constraint.Height;
            double calcHeight = 0;
            double maxWidth = 0;
            var resultSize = new Size(0, 0);

            if (Children.Count == 0)
                return resultSize;

            foreach (UIElement item in Children)
            {
                item.Measure(constraint);
                var newHeight = calcHeight + item.DesiredSize.Height;

                if (newHeight < maxHeight)
                {
                    var rect = new Rect(new Point(0, calcHeight), item.DesiredSize);
                    item.Measure(constraint);
                    calcHeight = newHeight;
                    maxWidth = Math.Max(maxWidth, item.DesiredSize.Width);
                }
                else
                {
                    break;
                }
            }

            resultSize.Width = maxWidth;
            resultSize.Height = calcHeight;

            return resultSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var isClipped = false;

            var maxHeight = arrangeSize.Height;
            double calcHeight = 0;

            foreach (UIElement item in Children)
            {
                var newHeight = calcHeight + item.DesiredSize.Height;

                if (newHeight < maxHeight)
                {
                    var rect = new Rect(new Point(0, calcHeight), item.DesiredSize);
                    item.Arrange(rect);
                    calcHeight = newHeight;
                }
                else
                {
                    //if (calcHeight > 0)
                    {
                        isClipped = true;
                        Clip = new RectangleGeometry { Rect = new Rect(0, 0, arrangeSize.Width, calcHeight) };
                    }
                    break;
                }
            }

            SetClipped(isClipped);

            return base.ArrangeOverride(arrangeSize);
        }

        private void SetClipped(bool isClipped)
        {
            var changed = Clipped != isClipped;
            Clipped = isClipped;

            if (!isClipped)
                Clip = null;

            if (ClippedChanged != null)
                ClippedChanged(this, new ClippedChangedEventArgs(Clipped));
        }
    }

    public class ClippedChangedEventArgs : EventArgs
    {
        public ClippedChangedEventArgs(bool clipped)
        {
            Clipped = clipped;
        }

        public bool Clipped { get; set; }
    }
}
