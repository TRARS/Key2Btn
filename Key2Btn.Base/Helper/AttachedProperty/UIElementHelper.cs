using System.Windows;

namespace Key2Btn.Base.Helper.AttachedProperty
{
    public partial class UIElementHelper
    {
        public static readonly DependencyProperty AlternationIndexAttachedProperty = DependencyProperty.RegisterAttached(
            name: "AlternationIndexAttached",
            propertyType: typeof(int),
            ownerType: typeof(UIElementHelper),
            defaultMetadata: new FrameworkPropertyMetadata(-1)
        );
        public static int GetAlternationIndexAttached(DependencyObject target)
        {
            return (int)target.GetValue(AlternationIndexAttachedProperty);
        }
        public static void SetAlternationIndexAttached(DependencyObject target, int value)
        {
            target.SetValue(AlternationIndexAttachedProperty, value);
        }

        public static readonly DependencyProperty PosXProperty = DependencyProperty.RegisterAttached(
            name: "PosX",
            propertyType: typeof(double),
            ownerType: typeof(UIElementHelper),
            defaultMetadata: new FrameworkPropertyMetadata(0d)
        );
        public static double GetPosX(DependencyObject target)
        {
            return (double)target.GetValue(PosXProperty);
        }
        public static void SetPosX(DependencyObject target, double value)
        {
            target.SetValue(PosXProperty, value);
        }

        public static readonly DependencyProperty PosYProperty = DependencyProperty.RegisterAttached(
            name: "PosY",
            propertyType: typeof(double),
            ownerType: typeof(UIElementHelper),
            defaultMetadata: new FrameworkPropertyMetadata(0d)
        );
        public static double GetPosY(DependencyObject target)
        {
            return (double)target.GetValue(PosYProperty);
        }
        public static void SetPosY(DependencyObject target, double value)
        {
            target.SetValue(PosYProperty, value);
        }
    }

    public partial class UIElementHelper
    {
        public static readonly DependencyProperty DoubleAFromProperty = DependencyProperty.RegisterAttached(
        name: "DoubleAFrom",
        propertyType: typeof(double),
        ownerType: typeof(UIElementHelper),
        defaultMetadata: new FrameworkPropertyMetadata(0d, (s, e) =>
        {
            if (s is DependencyObject dependencyObject)
            {
                SetDoubleATo(dependencyObject, (double)e.NewValue);
            }
        })
    );
        public static double GetDoubleAFrom(DependencyObject target)
        {
            return (double)target.GetValue(DoubleAFromProperty);
        }
        public static void SetDoubleAFrom(DependencyObject target, double value)
        {
            target.SetValue(DoubleAFromProperty, value);
        }

        public static readonly DependencyProperty DoubleAToProperty = DependencyProperty.RegisterAttached(
            name: "DoubleATo",
            propertyType: typeof(double),
            ownerType: typeof(UIElementHelper),
            defaultMetadata: new FrameworkPropertyMetadata(0d)
        );
        public static double GetDoubleATo(DependencyObject target)
        {
            return (double)target.GetValue(DoubleAToProperty);
        }
        public static void SetDoubleATo(DependencyObject target, double value)
        {
            target.SetValue(DoubleAToProperty, value);
        }
    }

    public partial class UIElementHelper
    {
        public static readonly DependencyProperty DoubleBFromProperty = DependencyProperty.RegisterAttached(
            name: "DoubleBFrom",
            propertyType: typeof(double),
            ownerType: typeof(UIElementHelper),
            defaultMetadata: new FrameworkPropertyMetadata(0d, (s, e) =>
            {
                if (s is DependencyObject dependencyObject)
                {
                    SetDoubleBTo(dependencyObject, (double)e.NewValue);
                }
            })
        );
        public static double GetDoubleBFrom(DependencyObject target)
        {
            return (double)target.GetValue(DoubleBFromProperty);
        }
        public static void SetDoubleBFrom(DependencyObject target, double value)
        {
            target.SetValue(DoubleBFromProperty, value);
        }

        public static readonly DependencyProperty DoubleBToProperty = DependencyProperty.RegisterAttached(
            name: "DoubleBTo",
            propertyType: typeof(double),
            ownerType: typeof(UIElementHelper),
            defaultMetadata: new FrameworkPropertyMetadata(0d)
        );
        public static double GetDoubleBTo(DependencyObject target)
        {
            return (double)target.GetValue(DoubleBToProperty);
        }
        public static void SetDoubleBTo(DependencyObject target, double value)
        {
            target.SetValue(DoubleBToProperty, value);
        }
    }
}
