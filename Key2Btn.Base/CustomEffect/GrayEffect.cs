using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Key2Btn.Base.CustomEffect
{
    public class GrayEffect : ShaderEffect
    {
        string? AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GrayEffect), 0);
        public static readonly DependencyProperty FactorProperty = DependencyProperty.Register("Factor", typeof(double), typeof(GrayEffect), new UIPropertyMetadata(System.Convert.ToDouble(0D), PixelShaderConstantCallback(0)));

        public GrayEffect()
        {
            PixelShader = new PixelShader()
            {
                UriSource = new Uri($"pack://application:,,,/{AssemblyName};component/CustomEffect/GrayEffect.ps")
            };
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(FactorProperty);
        }
        public Brush Input
        {
            get => (Brush)this.GetValue(InputProperty);
            set => Input = value;
        }
        public double Factor
        {
            get => System.Convert.ToDouble(this.GetValue(FactorProperty));
            set => Factor = value;
        }
    }
}
