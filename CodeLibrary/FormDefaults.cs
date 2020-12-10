using CodeLibrary.Controls;
using CodeLibrary.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormDefaults : Form
    {
        private readonly EnumComboBoxModeHelper<CodeType> _typeComboHelper;

        public FormDefaults()
        {
            InitializeComponent();
            _typeComboHelper = new EnumComboBoxModeHelper<CodeType>(comboBoxType, CodeType.None);
            _typeComboHelper.Fill();
        }

        public string DefaultCode { get; set; }

        public CodeType DefaultCodeType { get; set; }
        public bool DefaultCodeTypeEnabled { get; set; }
        public string DefaultName { get; set; }

        private void DarkTheme()
        {
            tbName.BackColor = Color.FromArgb(255, 40, 40, 40);
            tbName.ForeColor = Color.LightYellow;
            comboBoxType.BackColor = Color.FromArgb(255, 40, 40, 40);
            comboBoxType.ForeColor = Color.LightYellow;
            //ForeColor = Color.White;
            BackColor = Color.FromArgb(255, 100, 100, 100);
            tbCode.IndentBackColor = Color.FromArgb(255, 75, 75, 75);
            tbCode.BackColor = Color.FromArgb(255, 40, 40, 40);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            labelHelp.ForeColor = Color.White;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbCode.DarkStyle();
            tbCode.Refresh();
        }

        private void Defaults_Load(object sender, EventArgs e)
        {
            if (Config.HighContrastMode)
                HighContrastTheme();
            else if (Config.DarkMode)
                DarkTheme();
            else
                LightTheme();

            tbName.Text = DefaultName ?? string.Empty;
            tbCode.Text = DefaultCode ?? string.Empty;
            checkBoxCodeType.Checked = DefaultCodeTypeEnabled;
            _typeComboHelper.SetSelectedIndex(DefaultCodeType);
        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            this.DialogResult = e.Result;
            if (e.Result == DialogResult.OK)
            {
                DefaultName = tbName.Text ?? string.Empty;
                DefaultCode = tbCode.Text ?? string.Empty;
                DefaultCodeTypeEnabled = checkBoxCodeType.Checked;
                DefaultCodeType = (CodeType)_typeComboHelper.GetValue();
            }
            Close();
        }

        private void HighContrastTheme()
        {
            tbName.BackColor = Color.FromArgb(255, 10, 10, 10);
            tbName.ForeColor = Color.LightYellow;
            comboBoxType.BackColor = Color.FromArgb(255, 10, 10, 10);
            comboBoxType.ForeColor = Color.LightYellow;

            //ForeColor = Color.White;
            BackColor = Color.FromArgb(255, 80, 80, 80);
            tbCode.IndentBackColor = Color.FromArgb(255, 55, 55, 55);
            tbCode.BackColor = Color.FromArgb(255, 10, 10, 10);
            tbCode.CaretColor = Color.White;
            tbCode.ForeColor = Color.LightGray;
            labelHelp.ForeColor = Color.White;
            tbCode.SelectionColor = Color.Red;
            tbCode.LineNumberColor = Color.LightSeaGreen;
            tbCode.HighContrastStyle();
            tbCode.Refresh();
        }

        private void LightTheme()
        {
            tbName.BackColor = Color.White;
            tbName.ForeColor = Color.Black;
            comboBoxType.BackColor = Color.White;
            comboBoxType.ForeColor = Color.Black;

            //ForeColor = Color.FromArgb(255, 0, 0, 0);
            BackColor = Color.FromArgb(255, 240, 240, 240);
            tbCode.IndentBackColor = Color.FromArgb(255, 255, 255, 255);
            tbCode.BackColor = Color.FromArgb(255, 255, 255, 255);
            tbCode.ForeColor = Color.Black;
            tbCode.CaretColor = Color.White;
            labelHelp.ForeColor = Color.Black;
            tbCode.SelectionColor = Color.FromArgb(50, 0, 0, 255);
            tbCode.LineNumberColor = Color.FromArgb(255, 0, 128, 128);
            tbCode.LightStyle();
            tbCode.Refresh();
        }
    }
}