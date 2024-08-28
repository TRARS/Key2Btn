using Key2Btn.Base.Helper.AttachedProperty;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Key2Btn.Base.CustomControlEx.MacroButtonGroupEx
{
    public class cWrapPanel : WrapPanel
    {
        static cWrapPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(cWrapPanel), new FrameworkPropertyMetadata(typeof(cWrapPanel)));
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double currentLineHeight = 0;
            double currentLineWidth = 0;
            double panelWidth = finalSize.Width;
            double x = 0;
            double y = 0;
            int lineIndex = 0;
            bool isFirstInLine = false;

            foreach (UIElement element in InternalChildren)
            {
                Size elementSize = element.DesiredSize;

                if (currentLineWidth + elementSize.Width > panelWidth)
                {
                    // Move to the next line
                    x = 0;
                    y += currentLineHeight;

                    // Reset line width and height for the new line
                    currentLineWidth = elementSize.Width;
                    currentLineHeight = elementSize.Height;
                    lineIndex++;
                    isFirstInLine = true; // 下一个元素是换行后的第一个
                }
                else
                {
                    // Continue adding to the current line
                    currentLineWidth += elementSize.Width;
                    currentLineHeight = Math.Max(currentLineHeight, elementSize.Height);
                    isFirstInLine = false; // 不是换行后的第一个
                }

                element.Arrange(new Rect(new Point(x, y), elementSize));
                WrapPanelHelper.SetLineIndex(element, lineIndex); // 设置行索引
                WrapPanelHelper.SetIsFirstInLine(element, isFirstInLine); // 设置是否是换行后的第一个元素
                x += elementSize.Width;
            }

            return base.ArrangeOverride(finalSize);
        }
    }
}
