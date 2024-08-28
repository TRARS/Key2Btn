using CommunityToolkit.Mvvm.Input;
using Key2Btn.Base.Helper.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlExBase.HorizontalFoldableContainerEx
{
    public partial class cHorizontalFoldableContainer : ContentControl
    {
        static cHorizontalFoldableContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cHorizontalFoldableContainer), new FrameworkPropertyMetadata(typeof(cHorizontalFoldableContainer)));
        }

        public cHorizontalFoldableContainer()
        {
            ArrowCommand = new(() =>
            {
                this.BodyVisibility = this.BodyVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            });
        }
    }

    public partial class cHorizontalFoldableContainer
    {
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            name: "Header",
            propertyType: typeof(object),
            ownerType: typeof(cHorizontalFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Visibility ArrowVisibility
        {
            get { return (Visibility)GetValue(ArrowVisibilityProperty); }
            set { SetValue(ArrowVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ArrowVisibilityProperty = DependencyProperty.Register(
            name: "ArrowVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(cHorizontalFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public Visibility BodyVisibility
        {
            get { return (Visibility)GetValue(BodyVisibilityProperty); }
            set { SetValue(BodyVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BodyVisibilityProperty = DependencyProperty.Register(
            name: "BodyVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(cHorizontalFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public RelayCommand ArrowCommand
        {
            get { return (RelayCommand)GetValue(ArrowCommandProperty); }
            private set { SetValue(ArrowCommandProperty, value); }
        }
        public static readonly DependencyProperty ArrowCommandProperty = DependencyProperty.Register(
            name: "ArrowCommand",
            propertyType: typeof(RelayCommand),
            ownerType: typeof(cHorizontalFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );
    }

    public partial class cHorizontalFoldableContainer
    {
        public double BodyScaleX
        {
            get { return (double)GetValue(BodyScaleXProperty); }
            set { SetValue(BodyScaleXProperty, value); }
        }
        public static readonly DependencyProperty BodyScaleXProperty = DependencyProperty.Register(
            name: "BodyScaleX",
            propertyType: typeof(double),
            ownerType: typeof(cHorizontalFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public double BodyScaleY
        {
            get { return (double)GetValue(BodyScaleYProperty); }
            set { SetValue(BodyScaleYProperty, value); }
        }
        public static readonly DependencyProperty BodyScaleYProperty = DependencyProperty.Register(
            name: "BodyScaleY",
            propertyType: typeof(double),
            ownerType: typeof(cHorizontalFoldableContainer),
            typeMetadata: new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        private const double duration = 128;

        private void BodyFadeOutAnimation(bool canExecute)
        {
            if (canExecute is false) { return; }

            //this.SetDoubleAnimation(BodyScaleXProperty, BodyScaleX, 0d, duration).Begin();
            this.SetDoubleAnimation(BodyScaleYProperty, BodyScaleY, 0d, duration).Begin();
        }
        private void BodyFadeInAnimation(bool canExecute)
        {
            if (canExecute is false) { return; }

            //this.SetDoubleAnimation(BodyScaleXProperty, BodyScaleX, 1d, duration).Begin();
            this.SetDoubleAnimation(BodyScaleYProperty, BodyScaleY, 1d, duration).Begin();
        }
    }
}
