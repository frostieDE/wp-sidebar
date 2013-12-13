using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SidebarWP8
{
    [TemplatePart(Name = "PART_HeaderButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Container", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ContainerTransform", Type = typeof(CompositeTransform))]
    public class SidebarControl : ContentControl
    {
        private const double SIDEBAR_WIDTH = 380d;

        private Button _headerButton;
        private Grid _containerGrid;
        private CompositeTransform _transform;

        #region DependencyProperties

        public static DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(SidebarControl), null);

        /// <summary>
        /// Gets or sets the header text
        /// </summary>
        public string HeaderText
        {
            get { return GetValue(HeaderTextProperty) as string; }
            set { SetValue(HeaderTextProperty, value); }
        }

        public static DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(SidebarControl), null);

        /// <summary>
        /// Gets or sets the header template (optional)
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return GetValue(HeaderTemplateProperty) as DataTemplate; }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(SidebarControl), null);

        /// <summary>
        /// Gets or sets the background color for header
        /// </summary>
        public Brush HeaderBackground
        {
            get { return GetValue(HeaderBackgroundProperty) as Brush; }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public static DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground", typeof(Brush), typeof(SidebarControl), null);

        /// <summary>
        /// Gets or sets the text color used for the header text
        /// </summary>
        public Brush HeaderForeground
        {
            get { return GetValue(HeaderForegroundProperty) as Brush; }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public static DependencyProperty HeaderButtonTemplateProperty = DependencyProperty.Register("HeaderButtonTemplate", typeof(DataTemplate), typeof(SidebarControl), null);

        /// <summary>
        /// Gets or sets the header template (optional)
        /// </summary>
        public DataTemplate HeaderButtonTemplate
        {
            get { return GetValue(HeaderButtonTemplateProperty) as DataTemplate; }
            set { SetValue(HeaderButtonTemplateProperty, value); }
        }

        public static DependencyProperty SidebarContentProperty = DependencyProperty.Register("SidebarContent", typeof(FrameworkElement), typeof(SidebarControl), null);

        /// <summary>
        /// Gets or sets the content of a sidebar
        /// </summary>
        public FrameworkElement SidebarContent
        {
            get { return GetValue(SidebarContentProperty) as FrameworkElement; }
            set { SetValue(SidebarContentProperty, value); }
        }

        public static DependencyProperty SidebarBackgroundProperty = DependencyProperty.Register("SidebarBackground", typeof(Brush), typeof(SidebarControl), null);

        /// <summary>
        /// Gets or sets the text color used for the header text
        /// </summary>
        public Brush SidebarBackground
        {
            get { return GetValue(SidebarBackgroundProperty) as Brush; }
            set { SetValue(SidebarBackgroundProperty, value); }
        }

        public static DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(SidebarControl), new PropertyMetadata(false, OnIsOpenChanged) );

        /// <summary>
        /// Gets or sets the IsOpen property (it opens/closes the sidebar)
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SidebarControl control = d as SidebarControl;

            if (control != null)
            {
                control.OnIsOpenChanged(e);
            }
        }

        private void OnIsOpenChanged(DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;

            if (newValue != oldValue)
            {
                if (newValue == true)
                {
                    OpenSidebar();
                }
                else
                {
                    CloseSidebar();
                }
            }
        }
        #endregion

        public SidebarControl()
        {
            DefaultStyleKey = typeof(SidebarControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_headerButton != null)
            {
                _headerButton.Click -= HeaderButton_Click;
            }

            _headerButton = GetTemplateChild("PART_HeaderButton") as Button;

            if (_headerButton != null)
            {
                _headerButton.Click += HeaderButton_Click;
            }

            if (_containerGrid != null)
            {
                _containerGrid.ManipulationStarted -= OnManipulationStarted;
                _containerGrid.ManipulationCompleted -= OnManipulationCompleted;
                _containerGrid.ManipulationDelta -= OnManipulationDelta;
            }

            _containerGrid = GetTemplateChild("PART_Container") as Grid;

            if (_containerGrid != null)
            {
                _containerGrid.DataContext = this;

                _containerGrid.ManipulationStarted += OnManipulationStarted;
                _containerGrid.ManipulationCompleted += OnManipulationCompleted;
                _containerGrid.ManipulationDelta += OnManipulationDelta;
            }

            _transform = GetTemplateChild("PART_ContainerTransform") as CompositeTransform;
        }

        #region Event-Handlers for dragging event

        private void OnManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (_transform != null)
            {
                Point transformedTranslation = GetTransformNoTranslation(_transform).Transform(e.DeltaManipulation.Translation);

                if (_transform.TranslateX + transformedTranslation.X >= SIDEBAR_WIDTH)
                {
                    _transform.TranslateX = SIDEBAR_WIDTH;
                    IsOpen = true;
                }
                else if (_transform.TranslateX + transformedTranslation.X < 0)
                {
                    _transform.TranslateX = 0;
                    IsOpen = false;
                }

                if ((_transform.TranslateX >= SIDEBAR_WIDTH && transformedTranslation.X > 0) ||
                    (_transform.TranslateX <= 0 && transformedTranslation.X < 0))
                {
                    return;
                }

                _transform.TranslateX += transformedTranslation.X;
            }
        }

        private void OnManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            Point transformedTranslation = GetTransformNoTranslation(_transform).Transform(e.TotalManipulation.Translation);

            if (transformedTranslation.X > 0)
            {
                IsOpen = true;
            }
            else if (transformedTranslation.X < 0)
            {
                IsOpen = false;
            }
        }

        private void OnManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            
        }
        #endregion

        /// <summary>
        /// Event handler for the header button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = !IsOpen;
        }

        /// <summary>
        /// Opens the sidebar (goes to the according visual state)
        /// </summary>
        private void OpenSidebar()
        {
            VisualStateManager.GoToState(this, "SidebarOpenState", true);
        }

        /// <summary>
        /// Closes the sidebar (goes to the according visual state)
        /// </summary>
        private void CloseSidebar()
        {
            VisualStateManager.GoToState(this, "SidebarClosedState", true);
        }

        #region Helpers
        private GeneralTransform GetTransformNoTranslation(CompositeTransform transform)
        {
            CompositeTransform newTransform = new CompositeTransform();
            newTransform.Rotation = transform.Rotation;
            newTransform.ScaleX = transform.ScaleX;
            newTransform.ScaleY = transform.ScaleY;
            newTransform.CenterX = transform.CenterX;
            newTransform.CenterY = transform.CenterY;
            newTransform.TranslateX = 0;
            newTransform.TranslateY = 0;

            return newTransform;
        }
        #endregion
    }
}
