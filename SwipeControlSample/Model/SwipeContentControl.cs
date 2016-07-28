using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace SwipeControlSample.Model
{
    [ContentProperty(Name = "Content")]
    public class SwipeContentControl : Control
    {
        static readonly double SIGNIFICANT_TRANSLATE_FACTOR = 0.20d;

        private FrameworkElement contentElement;

        public event EventHandler<SwipeEventArgs> Swiped;

        #region Left
        public object LeftContent
        {
            get { return base.GetValue(LeftContentProperty); }
            set { base.SetValue(LeftContentProperty, value); }
        }

        public static readonly DependencyProperty LeftContentProperty =
            DependencyProperty.Register(nameof(LeftContent), typeof(object), typeof(SwipeContentControl),
                null);

        public DataTemplate LeftContentTemplate
        {
            get { return (DataTemplate)base.GetValue(LeftContentTemplateProperty); }
            set { base.SetValue(LeftContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty LeftContentTemplateProperty =
            DependencyProperty.Register(nameof(LeftContentTemplate), typeof(DataTemplate), typeof(SwipeContentControl), null);

        #endregion

        #region Right
        public object RightContent
        {
            get { return base.GetValue(RightContentProperty); }
            set { base.SetValue(RightContentProperty, value); }
        }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(SwipeContentControl), null);

        public DataTemplate RightContentTemplate
        {
            get { return (DataTemplate)base.GetValue(RightContentTemplateProperty); }
            set { base.SetValue(RightContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty RightContentTemplateProperty =
            DependencyProperty.Register(nameof(RightContentTemplate), typeof(DataTemplate), typeof(SwipeContentControl), null);

        #endregion

        #region Content

        public object Content
        {
            get { return base.GetValue(ContentProperty); }
            set { base.SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(SwipeContentControl), null);

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)base.GetValue(ContentTemplateProperty); }
            set { base.SetValue(ContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(SwipeContentControl), null);

        #endregion

        public SwipeContentControl()
        {
            this.DefaultStyleKey = typeof(SwipeContentControl);
            this.ManipulationMode = ManipulationModes.TranslateX;
            this.ManipulationDelta += OnManipulationDelta;
            this.ManipulationCompleted += OnManipulationCompleted;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.TranslateContent(0);

            if (IsManipulationSignificant(e.Cumulative.Translation.X))
            {
                SwipeEventArgs args =
                    new SwipeEventArgs(e.Cumulative.Translation.X < 0 ? SwipeDirection.Left : SwipeDirection.Right);
                this.Swiped?.Invoke(this, args);
            }
        }

        private bool IsManipulationSignificant(double x)
        {
            return Math.Abs(x) > (SIGNIFICANT_TRANSLATE_FACTOR * this.contentElement.ActualWidth);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.contentElement = this.GetTemplateChild("contentPresenter") as FrameworkElement;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.SetVisualStateForManipulation(e.Cumulative.Translation);
            this.TranslateContent(e.Cumulative.Translation.X);
        }

        private void SetVisualStateForManipulation(Point point)
        {
            var direction = this.DirectionOfManipulation(point);
            VisualStateManager.GoToState(this, direction.ToString(), true);
        }

        SwipeDirection DirectionOfManipulation(Point p)
        {
            SwipeDirection d = SwipeDirection.Default;
            if (p.X != 0)
            {
                d = p.X < 0 ? SwipeDirection.Left : SwipeDirection.Right;
            }
            return (d);
        }

        private void TranslateContent(double x)
        {
            if (this.contentElement != null)
            {
                // This may well break if there's already a transform on this element😦
                TranslateTransform transform = (this.contentElement.RenderTransform as TranslateTransform);

                if (transform == null)
                {
                    transform = new TranslateTransform();
                    this.contentElement.RenderTransform = transform;
                }
                transform.X = x;
            }
        }
    }
}
