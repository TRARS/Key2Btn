using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Key2Btn.Base.Helper.Extensions
{
    public static partial class SolidColorBrushExtensions
    {
        /// <summary>
        /// 调用该方法前，必须先给将要调用该方法的对象赋值。即，先 xxx =  new SolidColorBrush(); 再 xxx.BeginColorAnimation(...);
        /// </summary>
        public static void BeginColorAnimation(this SolidColorBrush target,
                                               Color from,
                                               Color to,
                                               double duration,
                                               bool autoReverse,
                                               RepeatBehavior repeatBehavior,
                                               FillBehavior fillBehavior,
                                               Action? onAnimationCompleted = null)
        {
            var animation = new ColorAnimation()
            {
                From = from, // 起始值
                To = to,     // 终止值
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = autoReverse,
                RepeatBehavior = repeatBehavior,
                FillBehavior = fillBehavior
            };
            animation.Completed += (s, e) =>
            {
                onAnimationCompleted?.Invoke();
            };

            target.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
}
