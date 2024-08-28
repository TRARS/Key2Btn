using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Key2Btn.Base.CustomEffect
{
    public class HPivotEffect : ShaderEffect
    {
        string? AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(HPivotEffect), 0);
        public static readonly DependencyProperty PivotAmountProperty = DependencyProperty.Register("PivotAmount", typeof(double), typeof(HPivotEffect), new UIPropertyMetadata(((double)(0.2D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty EdgeProperty = DependencyProperty.Register("Edge", typeof(double), typeof(HPivotEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty ReversalProperty = DependencyProperty.Register("Reversal", typeof(double), typeof(HPivotEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(2)));

        public HPivotEffect()
        {
            PixelShader = new PixelShader
            {
                UriSource = new Uri($"pack://application:,,,/{AssemblyName};component/CustomEffect/HPivotEffect.ps"),
            };

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(PivotAmountProperty);
            UpdateShaderValue(EdgeProperty);
            UpdateShaderValue(ReversalProperty);
        }
        public Brush Input
        {
            get => ((Brush)(this.GetValue(InputProperty)));
            set => this.SetValue(InputProperty, value);
        }

        public double PivotAmount
        {
            get => ((double)(this.GetValue(PivotAmountProperty)));
            set => this.SetValue(PivotAmountProperty, value);
        }
        public double Edge
        {
            get => ((double)(this.GetValue(EdgeProperty)));
            set => this.SetValue(EdgeProperty, value);
        }
        public double Reversal
        {
            get => ((double)(this.GetValue(ReversalProperty)));
            set => this.SetValue(ReversalProperty, value);
        }
    }
}
