using FastColoredTextBoxNS;
using System;
using System.Windows.Forms;

namespace Tester
{
    public partial class Sandbox : Form
    {
        public Sandbox()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ranges = fctb.Range.GetRanges(@"(?<=\[).*?(?=\])");

            foreach (var range in ranges)
            {
                if (fctb.Selection.Length > 0)
                {
                    //heck diapason
                    if (range.GetIntersectionWith(fctb.Selection).Length > 0)
                    {
                        fctb.Selection = range;
                        //...
                        break;
                    }
                }
                else
                //check caret
                if (range.Contains(fctb.Selection.Start))
                {
                    fctb.Selection = range;
                    //....
                    break;
                }
            }
        }
    }

    class MyFCTB : FastColoredTextBox
    {
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //to prevent drag&drop inside FCTB
            typeof(FastColoredTextBox).GetField("mouseIsDragDrop", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, false);
            base.OnMouseMove(e);
        }
    }
}