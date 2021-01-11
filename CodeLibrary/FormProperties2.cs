using CodeLibrary.Controls;
using CodeLibrary.Core;
using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormProperties2 : Form
    {
        private readonly EnumComboBoxModeHelper<CodeType> _defaultTypeComboBoxHelper;
        private readonly EnumComboBoxModeHelper<Keys> _shortCutKeysComboHelper;
        private readonly EnumComboBoxModeHelper<CodeType> _typeComboBoxHelper;
 
        public FormProperties2()
        {
            InitializeComponent();
            _typeComboBoxHelper = new EnumComboBoxModeHelper<CodeType>(cbType, CodeType.None);            
            _typeComboBoxHelper.Fill();

            _defaultTypeComboBoxHelper = new EnumComboBoxModeHelper<CodeType>(cbDefaultType, CodeType.None);
            _defaultTypeComboBoxHelper.Fill();

            _shortCutKeysComboHelper = new EnumComboBoxModeHelper<Keys>(comboBoxShortCutKeys, Keys.None);
            _shortCutKeysComboHelper.Fill();
        }

        public CodeSnippet Snippet { get; set; }

        private void Defaults_Load(object sender, EventArgs e)
        {
            lbName.Text = Snippet.Path;
            tbName.Text = Snippet.DefaultChildName ?? string.Empty;
            tbCode.Text = Snippet.DefaultChildCode ?? string.Empty;
            rtf.Rtf = Snippet.DefaultChildRtf ?? string.Empty;

            checkBoxCodeType.Checked = Snippet.DefaultChildCodeTypeEnabled;
            _defaultTypeComboBoxHelper.SetSelectedIndex(Snippet.DefaultChildCodeType);
            _typeComboBoxHelper.SetSelectedIndex(Snippet.CodeType);
            cbImportant.Checked = Snippet.Important;
            cbAlarm.Checked = Snippet.AlarmActive;
            cbWordWrap.Checked = Snippet.Wordwrap;
            cbHtmlPreview.Checked = Snippet.HtmlPreview;
            cbDefaultType.SelectedIndexChanged += CbDefaultType_SelectedIndexChanged;

            if (Snippet.AlarmDate.HasValue)
            {
                datePicker.Value = Snippet.AlarmDate.Value.Date;
                timeControl.Value = Snippet.AlarmDate.Value;
            }

            Keys _keys = Snippet.ShortCutKeys & ~Keys.Control;
            _keys = _keys & ~Keys.Alt;
            _keys = _keys & ~Keys.Shift;
            _shortCutKeysComboHelper.SetSelectedIndex(_keys);
        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            this.DialogResult = e.Result;
            if (e.Result == DialogResult.OK)
            {
                Snippet.DefaultChildCodeType = (CodeType)_defaultTypeComboBoxHelper.GetValue();
                Snippet.CodeType = (CodeType)_typeComboBoxHelper.GetValue();

                Snippet.DefaultChildName = tbName.Text ?? string.Empty;
                Snippet.DefaultChildCode = tbCode.Text ?? string.Empty;

                if (Snippet.DefaultChildCodeType == CodeType.RTF)
                {
                    Snippet.DefaultChildRtf = rtf.Rtf ?? string.Empty;
                }
                else
                {
                    Snippet.DefaultChildRtf = string.Empty;
                }
                Snippet.DefaultChildCodeTypeEnabled = checkBoxCodeType.Checked;

                Snippet.Important = cbImportant.Checked;
                Snippet.AlarmActive = cbAlarm.Checked;
                Snippet.Wordwrap = cbWordWrap.Checked;
                Snippet.HtmlPreview = cbHtmlPreview.Checked;
                if (Snippet.AlarmActive)
                {
                    Snippet.AlarmDate = datePicker.Value.Date.Add(timeControl.Value.TimeOfDay);
                }
                else
                {
                    Snippet.AlarmDate = null;
                }
                Keys _keys = (Keys)_shortCutKeysComboHelper.GetValue();
                _keys = _keys & ~Keys.Control;
                _keys = _keys & ~Keys.Alt;
                _keys = _keys & ~Keys.Shift;

                if (cbControl.Checked)
                {
                    _keys = _keys | Keys.Control;
                }
                if (cbShift.Checked)
                {
                    _keys = _keys | Keys.Shift;
                }
                if (cbAlt.Checked)
                {
                    _keys = _keys | Keys.Alt;
                }

                Snippet.ShortCutKeys = _keys;
            }
            Close();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _name = _typeComboBoxHelper.GetName().ToLower();
            picture.Image = Icons.Images[_name];
        }

        private void CbDefaultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int _value = _defaultTypeComboBoxHelper.GetValue();
            if (_value == (int)CodeType.RTF)
            {
                rtf.Left = tbCode.Left;
                rtf.Top = tbCode.Top;
                rtf.Width = tbCode.Width;
                rtf.Height = tbCode.Height;
                rtf.Visible = true;
                tbCode.Visible = false;
            }
            else
            {
                tbCode.Visible = true;
                rtf.Visible = false;
            }
        }
    }
}