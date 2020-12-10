using CodeLibrary.PluginPack;
using CodeLibrary.PluginPack.Commands.Lines;
using EditorPlugins.Core;
using EditorPlugins.Engine;
using FastColoredTextBoxNS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PluginTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txt.IndentBackColor = Color.FromArgb(255, 75, 75, 75);
            txt.BackColor = Color.FromArgb(255, 40, 40, 40);
            txt.CaretColor = Color.White;
            txt.ForeColor = Color.LightGray;
            txt.SelectionColor = Color.Red;
            txt.LineNumberColor = Color.LightSeaGreen;

            txt.DarkStyle();
            txt.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Apply(new TrimLines());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Apply(new Reverse());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Apply(new ShuffleLines());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Apply(new SurroundLines());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Apply(new RemoveEmptyLines());
        }

        private void Apply(IEditorPlugin plugin)
        {
            Range _line = txt.GetLine(txt.Selection.Start.iLine);

            SelInfo _selInfo = new SelInfo()
            {
                Text = txt.Text,
                SelectedText = txt.SelectedText,
                CurrentLine = _line.Text
            };

            try
            {
                if (!plugin.OmitResult)
                {
                    plugin.Apply(_selInfo);
                    txt.SelectedText = _selInfo.SelectedText;
                }
                else
                {
                    plugin.Apply(_selInfo);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Apply(new StringFormatLines());

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Apply(new RemoveDuplicateLines());
        }
    }
}
