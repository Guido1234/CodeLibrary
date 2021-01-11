using System;
using System.Drawing;
using System.Windows.Forms;
 
namespace CodeLibrary.Core
{
    public class RtfControlStyle
    {
        public Color? Color { get; set; } 
        public string FontFamily { get; set; } = "Arial";
        public Guid Id { get; set; } = Guid.NewGuid();
        public float FontSize { get; set; } = 9;
        public FontStyle FontStyle { get; set; } = FontStyle.Regular;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;
        public string StyleName { get; set; } = string.Empty;

        public override string ToString()
        {
            return StyleName;
        }
    }
}