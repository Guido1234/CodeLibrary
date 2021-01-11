using CodeLibrary.Controls;
using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormSearch : Form
    {
        public FormSearch()
        {
            InitializeComponent();
            listViewSearch.SelectedIndexChanged += ListViewSearch_SelectedIndexChanged;
        }

        public string SelectedPath { get; set; }

        public void SetEditorCodeType(CodeType type)
        {
            switch (type)
            {
                case CodeType.Template:
                    tbCode.Language = FastColoredTextBoxNS.Language.CSharp;
                    break;

                case CodeType.CSharp:
                    tbCode.Language = FastColoredTextBoxNS.Language.CSharp;
                    break;

                case CodeType.Folder:
                    tbCode.Language = FastColoredTextBoxNS.Language.Custom;
                    break;

                case CodeType.SQL:
                    tbCode.Language = FastColoredTextBoxNS.Language.SQL;
                    break;

                case CodeType.VB:
                    tbCode.Language = FastColoredTextBoxNS.Language.VB;
                    break;

                case CodeType.JS:
                    tbCode.Language = FastColoredTextBoxNS.Language.JS;
                    break;

                case CodeType.XML:
                    tbCode.Language = FastColoredTextBoxNS.Language.XML;
                    break;

                case CodeType.PHP:
                    tbCode.Language = FastColoredTextBoxNS.Language.PHP;
                    break;

                case CodeType.Lua:
                    tbCode.Language = FastColoredTextBoxNS.Language.Lua;
                    break;

                case CodeType.None:
                    tbCode.Language = FastColoredTextBoxNS.Language.Custom;
                    break;

                case CodeType.RTF:
                    tbCode.Language = FastColoredTextBoxNS.Language.Custom;
                    break;

                case CodeType.HTML:
                    tbCode.Language = FastColoredTextBoxNS.Language.HTML;
                    break;
            }
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (chkPattern.Checked)
            {
                SearchPattern();
                return;
            }
            if (chkMatchCase.Checked)
            {
                SearchMatchCase();
                return;
            }
            Search();
        }

        private void ChkMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMatchCase.Checked)
                chkPattern.Checked = false;
        }

        private void ChkPattern_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPattern.Checked)
                chkMatchCase.Checked = false;
        }

        private void DarkTheme()
        {
            tbSearch.BackColor = Color.FromArgb(255, 40, 40, 40);
            tbSearch.ForeColor = Color.LightYellow;

            listViewSearch.BackColor = Color.FromArgb(255, 40, 40, 40);
            listViewSearch.ForeColor = Color.LightYellow;

            chkPattern.BackColor = Color.FromArgb(255, 80, 80, 80);
            chkPattern.ForeColor = Color.White;
            chkMatchCase.BackColor = Color.FromArgb(255, 80, 80, 80);
            chkMatchCase.ForeColor = Color.White;

            //ForeColor = Color.White;
            BackColor = Color.FromArgb(255, 100, 100, 100);
            tbCode.IndentBackColor = Color.FromArgb(255, 75, 75, 75);
            tbCode.BackColor = Color.FromArgb(255, 40, 40, 40);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            //labelHelp.ForeColor = Color.White;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbCode.DarkStyle();
            tbCode.Refresh();
        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            Close();
        }

        private void DialogButton1_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            DialogResult = e.Result;
            Close();
        }

        private void HighContrastTheme()
        {
            tbSearch.BackColor = Color.FromArgb(255, 10, 10, 10);
            tbSearch.ForeColor = Color.LightYellow;
            listViewSearch.BackColor = Color.FromArgb(255, 10, 10, 10);
            listViewSearch.ForeColor = Color.LightYellow;

            chkPattern.BackColor = Color.FromArgb(255, 80, 80, 80);
            chkPattern.ForeColor = Color.White;
            chkMatchCase.BackColor = Color.FromArgb(255, 80, 80, 80);
            chkMatchCase.ForeColor = Color.White;

            //ForeColor = Color.White;
            BackColor = Color.FromArgb(255, 80, 80, 80);
            tbCode.IndentBackColor = Color.FromArgb(255, 55, 55, 55);
            tbCode.BackColor = Color.FromArgb(255, 10, 10, 10);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            //labelHelp.ForeColor = Color.White;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbCode.HighContrastStyle();
            tbCode.Refresh();
        }

        private void LightTheme()
        {
            tbSearch.BackColor = Color.White;
            tbSearch.ForeColor = Color.Black;
            listViewSearch.BackColor = Color.White;
            listViewSearch.ForeColor = Color.Black;

            chkPattern.BackColor = Color.White;
            chkPattern.ForeColor = Color.Black;
            chkMatchCase.BackColor = Color.White;
            chkMatchCase.ForeColor = Color.Black;

            //ForeColor = Color.FromArgb(255, 0, 0, 0);
            BackColor = Color.FromArgb(255, 240, 240, 240);
            tbCode.IndentBackColor = Color.FromArgb(255, 255, 255, 255);
            tbCode.BackColor = Color.FromArgb(255, 255, 255, 255);
            tbCode.ForeColor = Color.Black;
            tbCode.CaretColor = Color.White;
            //labelHelp.ForeColor = Color.Black;
            tbCode.SelectionColor = Color.FromArgb(50, 0, 0, 255);
            tbCode.LineNumberColor = Color.FromArgb(255, 0, 128, 128);
            tbCode.LightStyle();
            tbCode.Refresh();
        }

        private void ListViewSearch_DoubleClick(object sender, EventArgs e)
        {
            //Select();
            // Close();
        }

        private void ListViewSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectSnippet();
        }

        private void ListViewSearch_SelectedIndexChanged_1(object sender, EventArgs e)
        {
        }

        private void Search()
        {
            listViewSearch.BeginUpdate();
            listViewSearch.Clear();
            IEnumerable<CodeSnippetSearch> _result = CodeLib.Instance.Search(tbSearch.Text).Select(p => new CodeSnippetSearch() { Code = p.Code, Name = p.Name, CodeType = p.CodeType, CreationDate = p.CreationDate, Id = p.Id, Path = p.Path });
            listViewSearch.Fill<CodeSnippetSearch, string>(CultureInfo.InvariantCulture, _result, p => p.Name);
            listViewSearch.EndUpdate();
        }

        private void SearchMatchCase()
        {
            listViewSearch.BeginUpdate();
            listViewSearch.Clear();
            IEnumerable<CodeSnippetSearch> _result = CodeLib.Instance.SearchMatchCase(tbSearch.Text).Select(p => new CodeSnippetSearch() { Code = p.Code, Name = p.Name, CodeType = p.CodeType, CreationDate = p.CreationDate, Id = p.Id, Path = p.Path });
            listViewSearch.Fill<CodeSnippetSearch, string>(CultureInfo.InvariantCulture, _result, p => p.Name);
            listViewSearch.EndUpdate();
        }

        private void SearchPattern()
        {
            listViewSearch.BeginUpdate();
            listViewSearch.Clear();
            IEnumerable<CodeSnippetSearch> _result = CodeLib.Instance.SearchMatchPattern(tbSearch.Text).Select(p => new CodeSnippetSearch() { Code = p.Code, Name = p.Name, CodeType = p.CodeType, CreationDate = p.CreationDate, Id = p.Id, Path = p.Path });
            listViewSearch.Fill<CodeSnippetSearch, string>(CultureInfo.InvariantCulture, _result, p => p.Name);
            listViewSearch.EndUpdate();
        }

        private void SelectSnippet()
        {
            string selected = listViewSearch.GetSelectedByKeys<string>().FirstOrDefault();
            if (string.IsNullOrEmpty(selected))
            {
                tbCode.Text = string.Empty;
                return;
            }
            var _item = CodeLib.Instance.GetByName(selected).FirstOrDefault();
            tbCode.Text = _item.Code;
            SelectedPath = _item.Path;
            SetEditorCodeType(_item.CodeType);
            tbCode.WordWrap = _item.Wordwrap;
        }

        private void TbCode_Load(object sender, EventArgs e)
        {
            tbCode.Language = FastColoredTextBoxNS.Language.Custom;

            tbCode.IndentBackColor = Color.FromArgb(255, 35, 35, 35);
            tbCode.BackColor = Color.FromArgb(255, 10, 10, 10);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbCode.HighContrastStyle();
            tbCode.WordWrap = true;
            tbCode.GotoLine(0);
            tbSearch.Focus();

            LightTheme();

            //if (Config.HighContrastMode)
            //    HighContrastTheme();
            //else if (Config.DarkMode)
            //    DarkTheme();
            //else
            //    LightTheme();
        }

        private void TbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }
    }
}