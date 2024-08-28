using System;
using System.Runtime.CompilerServices;

namespace Key2Btn.Base.Helper.ExClass
{
    /// <summary>
    /// <para>继承ColorHelper.HSV</para>
    /// <para>增加Alpha属性，值范围0~1</para>
    /// </summary>
    public class HSVA : ColorHelper.HSV
    {
        public double Alpha { get; set; }

        public HSVA(int h, byte s, byte v, double a, [CallerMemberName] string? caller = null) : base(h, s, v)
        {
            if (a < 0 || a > 1) { throw new ArgumentOutOfRangeException(nameof(a), $"{caller}: Alpha value must be between 0 and 1."); }
            Alpha = a;
        }

        public override string ToString()
        {
            return $"{base.ToString()} (Alpha:{Alpha})";
        }
    }
}
