using CodeLibrary.Controls;
using CodeLibrary.Core;
using CodeLibrary.Helpers;
using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormProperties : Form
    { 
        private readonly EnumComboBoxModeHelper<CodeType> _defaultTypeComboBoxHelper;
        private readonly EnumComboBoxModeHelper<Keys> _shortCutKeysComboHelper;
        private readonly EnumComboBoxModeHelper<CodeType> _typeComboBoxHelper;
        private readonly ThemeHelper _themeHelper;

        public FormProperties(ThemeHelper themeHelper)
        {
            _themeHelper = themeHelper;
            InitializeComponent();
            _typeComboBoxHelper = new EnumComboBoxModeHelper<CodeType>(cbType, CodeType.None);
            _typeComboBoxHelper.Fill();

            _defaultTypeComboBoxHelper = new EnumComboBoxModeHelper<CodeType>(cbDefaultType, CodeType.None);
            _defaultTypeComboBoxHelper.Fill();

            _shortCutKeysComboHelper = new EnumComboBoxModeHelper<Keys>(comboBoxShortCutKeys, Keys.None);
            _shortCutKeysComboHelper.Fill();
            AcceptButton = dialogButton.buttonOk;
            CancelButton = dialogButton.buttonCancel;
        }

        public CodeSnippet Snippet { get; set; }

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

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _name = _typeComboBoxHelper.GetName().ToLower();
            picture.Image = Icons.Images[_name];
        }

        private void Defaults_Load(object sender, EventArgs e)
        {
            lbName.Text = Snippet.GetPath();
            tbName.Text = Snippet.DefaultChildName ?? string.Empty;
            tbCode.Text = Snippet.GetDefaultChildCode() ?? string.Empty;
            rtf.Rtf = Snippet.GetDefaultChildRtf() ?? string.Empty;
            lblModifiedOn.Text = $"Modified on: {Snippet.CodeLastModificationDate:yyyy-MM-dd HH:mm:ss}";
            cbExpand.Checked = Snippet.Expanded;
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
            Keys _keys = Snippet.ShortCutKeys;

            cbControl.Checked = _keys.HasFlag(Keys.Control);
            cbAlt.Checked = _keys.HasFlag(Keys.Alt);
            cbShift.Checked = _keys.HasFlag(Keys.Shift);

            _keys = Snippet.ShortCutKeys & ~Keys.Control;
            _keys = _keys & ~Keys.Alt;
            _keys = _keys & ~Keys.Shift;
            _shortCutKeysComboHelper.SetSelectedIndex(_keys);


        }

        private void DialogButton_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            RichTextBox _richTextBox = new RichTextBox();
            _themeHelper.RichTextBoxTheme(_richTextBox);

            CodeType _newType = (CodeType)_typeComboBoxHelper.GetValue();
            CodeType _oldType = Snippet.CodeType;

            DialogResult = e.Result;
            if (e.Result == DialogResult.OK)
            {
                Snippet.DefaultChildCodeType = (CodeType)_defaultTypeComboBoxHelper.GetValue();
                Snippet.CodeType = _newType;

                bool _changed = false;

                if (_newType == CodeType.RTF && _oldType != CodeType.RTF)
                {
                    _richTextBox.Text = Snippet.GetCode();
                    Snippet.SetRtf(_richTextBox.Rtf, out _changed);
                }
                else if (_oldType == CodeType.RTF && _newType != CodeType.RTF)
                {
                    _richTextBox.Rtf = Snippet.GetRTF();
                    Snippet.SetCode(_richTextBox.Text, out _changed); 
                }


                Snippet.DefaultChildName = tbName.Text ?? string.Empty;
                Snippet.SetDefaultChildCode(tbCode.Text ?? string.Empty, out _changed);

                if (Snippet.DefaultChildCodeType == CodeType.RTF)
                {
                    if (!string.IsNullOrEmpty(rtf.Text))
                    {
                        Snippet.SetDefaultChildRtf(rtf.Rtf, out _changed);
                    }
                }
                else
                {
                    Snippet.SetDefaultChildRtf(string.Empty, out _changed);
                }
                Snippet.DefaultChildCodeTypeEnabled = checkBoxCodeType.Checked;

                Snippet.Expanded = cbExpand.Checked;
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
    }
}