using CodeLibrary.Controls.Forms;
using CodeLibrary.Core;
using FastColoredTextBoxNS;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CodeLibrary.Controls.Controls
{
    public partial class RtfControl : UserControl, ITextEditor
    {
        private ComboBoxHelper<string> _FontComboBoxHelper;
        private ComboBoxHelper<int> _FontSizeComboBoxHelper;
        private ComboBoxHelper<RtfControlStyle> _FontStyleComboBoxHelper;
        private bool both = false;

        private int maxwid = 0;

        private Font nfont;

        private string samplestr = " - Hello World";

        private Image ttimg;

        public RtfControl()
        {
            InitializeComponent();
            DoubleBuffered = true;

            btBold.Click += BtBold_Click;
            btItalic.Click += BtItalic_Click;
            btUnderline.Click += BtUnderline_Click;
            btSwitchTheme.Click += BtSwitchTheme_Click;

            btForeColor.Click += BtForeColor_Click;

            btIndentMin.Click += BtIndentMin_Click;
            btIndentPlus.Click += BtIndentPlus_Click;
            btBullets.Click += BtBullets_Click;

            btAlignLeft.Click += BtAlignLeft_Click;
            btAlignRight.Click += BtAlignRight_Click;
            btAlignCenter.Click += BtAlignCenter_Click;

            btFont.Click += BtFont_Click;
            rtf.MouseUp += Rtf_MouseUp;

            rtf.SelectionChanged += Rtf_SelectionChanged;
            rtf.TextChanged += Rtf_TextChanged;
            rtf.KeyDown += Rtf_KeyDown;

            // rtf.SelectionBullet

            rtf.ZoomFactor = 1;
            toolStrip1.Resize += ToolStrip1_Resize;

            _FontSizeComboBoxHelper = new ComboBoxHelper<int>(cmbFontSize.ComboBox);
            _FontSizeComboBoxHelper.ManualSelectedIndexChanged += _FontSizeComboBoxHelper_ManualSelectedIndexChanged;
            _FontSizeComboBoxHelper.Fill(new int[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });

            updateStyle.Click += UpdateStyle_Click;
            addStyle.Click += AddStyle_Click;
            removeStyle.Click += RemoveStyle_Click;

            ttimg = global::CodeLibrary.Controls.Properties.Resources.ttfbmp;

            _FontComboBoxHelper = new ComboBoxHelper<string>(cmbFont.ComboBox);
            _FontComboBoxHelper.ManualSelectedIndexChanged += _FontComboBox_ManualSelectedIndexChanged;
            _FontComboBoxHelper.Fill(FontFamily.Families.Where(f => f.IsStyleAvailable(FontStyle.Regular)).Select(s => s.Name));

            UpdateStyles();

            cmbFont.ComboBox.DrawItem += ComboBox_DrawItem;
            cmbFont.ComboBox.MeasureItem += ComboBox_MeasureItem;
            cmbFont.ComboBox.DropDown += ComboBox_DropDown;
            cmbFont.ComboBox.MaxDropDownItems = 20;
            cmbFont.ComboBox.IntegralHeight = false;
            cmbFont.ComboBox.Sorted = false;
            cmbFont.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFont.ComboBox.DrawMode = DrawMode.OwnerDrawVariable;

            this.Resize += RtfControl_Resize;
        }


        public void UpdateStyles()
        {
            cbStyles.ComboBox.Items.Clear();
            _FontStyleComboBoxHelper = new ComboBoxHelper<RtfControlStyle>(cbStyles.ComboBox);
            _FontStyleComboBoxHelper.ManualSelectedIndexChanged += _FontStyleComboBoxHelper_ManualSelectedIndexChanged;
            _FontStyleComboBoxHelper.Fill(StyleCollection.Instance.Styles);
        }


        private RtfTheme _theme;

        public RtfTheme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                if (OwnTheme)
                {
                    return;
                }
                _theme = value;
                SwitchTheme(_theme);
            }
        }

        public void SetOwnTheme(RtfTheme theme)
        {
            _theme = theme;
            SwitchTheme(_theme);
        }

        public bool OwnTheme { get; set; } = false;

        private void BtSwitchTheme_Click(object sender, EventArgs e)
        {
            OwnTheme = true;
            switch (_theme)
            {
                case RtfTheme.Dark:
                    _theme = RtfTheme.HighContrast;
                    break;
                case RtfTheme.HighContrast:
                    _theme = RtfTheme.Light;
                    break;
                case RtfTheme.Light:
                    _theme = RtfTheme.Dark;
                    break;
            }
            SwitchTheme(_theme);
        }

        private void SwitchTheme(RtfTheme theme)
        {
            switch (theme)
            {
                case RtfTheme.Dark:
                    rtf.BackColor = Color.FromArgb(255, 40, 40, 40);
                    rtf.ForeColor = Color.FromArgb(255, 255, 255, 255);
                    break;
                case RtfTheme.Light:
                    rtf.BackColor = Color.FromArgb(255, 255, 255, 255);
                    rtf.ForeColor = Color.FromArgb(255, 0, 0, 0);
                    break;
                case RtfTheme.HighContrast:
                    rtf.BackColor = Color.FromArgb(255, 10, 10, 10);
                    rtf.ForeColor = Color.FromArgb(255, 255, 255, 255);
                    break;
            }
        }


        public event EventHandler TextChanged;


        public string Rtf
        {
            get
            {
                return rtf.Rtf;
            }
            set
            {
                rtf.Rtf = value;
            }
        }

        public string SelectedRtf
        {
            get
            {
                return rtf.SelectedRtf;
            }
            set
            {
                rtf.SelectedRtf = value;
            }
        }

        public string SelectedText
        {
            get
            {
                return rtf.SelectedText;
            }
            set
            {
                rtf.SelectedText = value;
            }
        }

        public override string Text
        {
            get
            {
                return rtf.Text;
            }
            set
            {
                rtf.Text = value;
            }
        }

        public int Zoom
        {
            get
            {
                return (int)(decimal)(rtf.ZoomFactor * 100);
            }
            set
            {
                float _zoom = (float)(((decimal)value) / 100);
                rtf.ZoomFactor = _zoom;
            }
        }

        public void ClearUndo()
        {
            rtf.ClearUndo();
        }

        public void Copy()
        {
            rtf.Copy();
            SetButtons();
        }

        public string CurrentLine()
        {
            try
            {
                int _index = rtf.GetLineFromCharIndex(rtf.SelectionStart);
                return rtf.Lines[_index];
            }
            catch { }

            return string.Empty;
        }

        public void Cut()
        {
            rtf.Cut();
            SetButtons();
        }

        public void GotoLine()
        {
            FastColoredTextBoxNS.GoToForm _gotoForm = new FastColoredTextBoxNS.GoToForm();
            _gotoForm.SelectedLineNumber = CurrentLineNumber();
            _gotoForm.TotalLineCount = rtf.Lines.Length;

            DialogResult _result = _gotoForm.ShowDialog();
            if (_result == DialogResult.OK)
            {
                GotoLine(_gotoForm.SelectedLineNumber);
            }
        }

        public void GotoLine(int line)
        {
            try
            {
                int _start = rtf.GetFirstCharIndexFromLine(line);
                rtf.SelectionStart = _start;
                rtf.SelectionLength = 0;
            }
            catch
            {
            }
        }

        public void Paste()
        {
            rtf.Paste();
            SetButtons();
        }

        public void SelectAll()
        {
            rtf.SelectAll();
        }

        public void SelectLine()
        {
            int _start = rtf.GetFirstCharIndexOfCurrentLine();
            int _line = rtf.GetLineFromCharIndex(_start);
            int _length = rtf.Lines[_line].Length;
            rtf.SelectionStart = _start;
            rtf.SelectionLength = _length;
        }

        public void ShowFindDialog()
        {
            FindForm _f = new FindForm(rtf);
            _f.Show();
        }

        public void ShowReplaceDialog()
        {
            CodeLibrary.Controls.Forms.ReplaceForm _f = new CodeLibrary.Controls.Forms.ReplaceForm(rtf);
            _f.Show();
        }

        public void Undo()
        {
            rtf.Undo();
            SetButtons();
        }

        private void _FontComboBox_ManualSelectedIndexChanged(object sender, EventArgs e)
        {
            string _fontname = cmbFont.SelectedItem as string;
            if (string.IsNullOrEmpty(_fontname))
            {
                return;
            }

            Font _f = rtf.SelectionFont;
            if (_f == null)
            {
                _f = rtf.Font;
            }

            _f = new Font(_fontname, _f.Size, _f.Style);
            rtf.SelectionFont = _f;
        }

        private void _FontSizeComboBoxHelper_ManualSelectedIndexChanged(object sender, EventArgs e)
        {
            int _fontSize = (int)cmbFontSize.SelectedItem;

            Font _f = rtf.SelectionFont;
            if (_f == null)
            {
                _f = rtf.Font;
            }
            _f = new Font(_f.FontFamily, (float)_fontSize, _f.Style);
            rtf.SelectionFont = _f;
        }

        private void _FontStyleComboBoxHelper_ManualSelectedIndexChanged(object sender, EventArgs e)
        {
            RtfControlStyle _item = cbStyles.SelectedItem as RtfControlStyle;
            if (_item == null)
            {
                return;
            }

            Font _f = new Font(_item.FontFamily, _item.FontSize, _item.FontStyle);
            rtf.SelectionFont = _f;
            if (_item.Color.HasValue)
                rtf.SelectionColor = _item.Color.Value;

            rtf.SelectionAlignment = _item.HorizontalAlignment;
        }

        private void AddStyle_Click(object sender, EventArgs e)
        {
            var _f = new FormInput();
            _f.Message = "Enter name for style";
            var _r = _f.ShowDialog();
            if (_r == DialogResult.OK)
            {
                var _style = new RtfControlStyle();

                if (rtf.SelectionColor != null)
                    _style.Color = rtf.SelectionColor;

                if (rtf.SelectionFont != null)
                {
                    _style.FontFamily = rtf.SelectionFont.Name;
                    _style.FontSize = rtf.SelectionFont.Size;
                    _style.FontStyle = rtf.SelectionFont.Style;
                }

                _style.HorizontalAlignment = rtf.SelectionAlignment;

                _style.StyleName = _f.InputText;

                StyleCollection.Instance.Styles.Add(_style);
                _FontStyleComboBoxHelper.Add(_style);

                SetButtons();
            }
        }

        private void BtAlignCenter_Click(object sender, EventArgs e)
        {
            rtf.SelectionAlignment = HorizontalAlignment.Center;
            SetButtons();
        }

        private void BtAlignLeft_Click(object sender, EventArgs e)
        {
            rtf.SelectionAlignment = HorizontalAlignment.Left;
            SetButtons();
        }

        private void BtAlignRight_Click(object sender, EventArgs e)
        {
            rtf.SelectionAlignment = HorizontalAlignment.Right;
            SetButtons();
        }

        private void BtBold_Click(object sender, EventArgs e)
        {
            FontStyle _style = SetFontStyle(!rtf.SelectionFont.Bold, rtf.SelectionFont.Italic, rtf.SelectionFont.Underline);
            rtf.SelectionFont = new Font(rtf.SelectionFont, _style);
            SetButtons();
        }

        private void BtBullets_Click(object sender, EventArgs e)
        {
            rtf.SelectionBullet = !rtf.SelectionBullet;
        }

        private void BtFont_Click(object sender, EventArgs e)
        {
            FontDialog _dialog = new FontDialog();
            _dialog.ShowColor = true;
            _dialog.ShowEffects = true;
            _dialog.ShowApply = true;
            _dialog.Font = rtf.SelectionFont;
            _dialog.Color = rtf.SelectionColor;

            DialogResult _result = _dialog.ShowDialog();

            if (_result == DialogResult.OK)
            {
                rtf.SelectionFont = _dialog.Font;
                rtf.SelectionColor = _dialog.Color;
            }
        }

        private void BtForeColor_Click(object sender, EventArgs e)
        {
            ColorDialog _dialog = new ColorDialog();

            _dialog.Color = rtf.SelectionColor;

            DialogResult _result = _dialog.ShowDialog();

            if (_result == DialogResult.OK)
            {
                rtf.SelectionColor = _dialog.Color;
            }
        }

        private void BtIndentMin_Click(object sender, EventArgs e)
        {
            rtf.SelectionIndent -= 20;
        }

        private void BtIndentPlus_Click(object sender, EventArgs e)
        {
            rtf.SelectionIndent += 20;
        }

        private void BtItalic_Click(object sender, EventArgs e)
        {
            FontStyle _style = SetFontStyle(rtf.SelectionFont.Bold, !rtf.SelectionFont.Italic, rtf.SelectionFont.Underline);
            rtf.SelectionFont = new Font(rtf.SelectionFont, _style);
            SetButtons();
        }

        private void BtUnderline_Click(object sender, EventArgs e)
        {
            FontStyle _style = SetFontStyle(rtf.SelectionFont.Bold, rtf.SelectionFont.Italic, !rtf.SelectionFont.Underline);
            rtf.SelectionFont = new Font(rtf.SelectionFont, _style);
            SetButtons();
        }

        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                string fontstring = cmbFont.Items[e.Index].ToString();
                nfont = new Font(fontstring, 10);
                Font afont = new Font("Arial", 10);

                if (both)
                {
                    Graphics g = CreateGraphics();
                    int w = (int)g.MeasureString(fontstring, afont).Width;

                    if ((e.State & DrawItemState.Focus) == 0)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window),
                            e.Bounds.X + ttimg.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                        e.Graphics.DrawString(fontstring, afont, new SolidBrush(SystemColors.WindowText),
                            e.Bounds.X + ttimg.Width * 2, e.Bounds.Y);
                        e.Graphics.DrawString(samplestr, nfont, new SolidBrush(SystemColors.WindowText),
                            e.Bounds.X + w + ttimg.Width * 2, e.Bounds.Y);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight),
                            e.Bounds.X + ttimg.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                        e.Graphics.DrawString(fontstring, afont, new SolidBrush(SystemColors.HighlightText),
                            e.Bounds.X + ttimg.Width * 2, e.Bounds.Y);
                        e.Graphics.DrawString(samplestr, nfont, new SolidBrush(SystemColors.HighlightText),
                            e.Bounds.X + w + ttimg.Width * 2, e.Bounds.Y);
                    }
                }
                else
                {
                    if ((e.State & DrawItemState.Focus) == 0)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window),
                            e.Bounds.X + ttimg.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                        e.Graphics.DrawString(fontstring, nfont, new SolidBrush(SystemColors.WindowText),
                            e.Bounds.X + ttimg.Width * 2, e.Bounds.Y);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight),
                            e.Bounds.X + ttimg.Width, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                        e.Graphics.DrawString(fontstring, nfont, new SolidBrush(SystemColors.HighlightText),
                            e.Bounds.X + ttimg.Width * 2, e.Bounds.Y);
                    }
                }

                e.Graphics.DrawImage(ttimg, new Point(e.Bounds.X, e.Bounds.Y));
            }
        }

        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            cmbFont.ComboBox.DropDownWidth = maxwid + 30;
        }

        private void ComboBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index > -1)
            {
                int w = 0;
                string fontstring = cmbFont.Items[e.Index].ToString();
                Graphics g = CreateGraphics();
                e.ItemHeight = (int)g.MeasureString(fontstring, new Font(fontstring, 10)).Height;
                w = (int)g.MeasureString(fontstring, new Font(fontstring, 10)).Width;
                if (both)
                {
                    int h1 = (int)g.MeasureString(samplestr, new Font(fontstring, 10)).Height;
                    int h2 = (int)g.MeasureString(cmbFont.Items[e.Index].ToString(), new Font("Arial", 10)).Height;
                    int w1 = (int)g.MeasureString(samplestr, new Font(fontstring, 10)).Width;
                    int w2 = (int)g.MeasureString(cmbFont.Items[e.Index].ToString(), new Font("Arial", 10)).Width;
                    if (h1 > h2)
                        h2 = h1;
                    e.ItemHeight = h2;
                    w = w1 + w2;
                }
                w += ttimg.Width * 2;
                if (w > maxwid)
                    maxwid = w;
                if (e.ItemHeight > 20)
                    e.ItemHeight = 20;
            }
        }

        private int CurrentLineNumber()
        {
            int _start = rtf.GetFirstCharIndexOfCurrentLine();
            return rtf.GetLineFromCharIndex(_start);
        }

        private void RemoveStyle_Click(object sender, EventArgs e)
        {
            if (!_FontStyleComboBoxHelper.HasSelection())
                return;

            RtfControlStyle _style = _FontStyleComboBoxHelper.GetSelected();
            _FontStyleComboBoxHelper.Remove(_style);
            StyleCollection.Instance.Styles.Remove(_style);

            SetButtons();
        }

        private void Rtf_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void Rtf_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        private void Rtf_SelectionChanged(object sender, EventArgs e)
        {
            SetButtons();
            Font _f = rtf.SelectionFont;

            if (_f != null)
            {
                _FontSizeComboBoxHelper.Select((int)_f.Size);
                _FontComboBoxHelper.Select(_f.Name);
            }
            else
            {
                _FontSizeComboBoxHelper.Deselect();
                _FontComboBoxHelper.Deselect();
            }
        }

        private void Rtf_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, new EventArgs());
            }
        }

        private void RtfControl_Resize(object sender, EventArgs e)
        {
            rtf.Left = 0;
            rtf.Top = toolStrip1.Height;
            rtf.Height = this.Height - toolStrip1.Height;
            rtf.Width = this.Width;
        }

        private void SetButtons()
        {
            if (rtf.SelectionFont == null)
            {
                btBold.Checked = false;
                btItalic.Checked = false;
                btUnderline.Checked = false;
                return;
            }

            btBold.Checked = rtf.SelectionFont.Bold;
            btItalic.Checked = rtf.SelectionFont.Italic;
            btUnderline.Checked = rtf.SelectionFont.Underline;

            removeStyle.Enabled = _FontStyleComboBoxHelper.HasSelection();
            updateStyle.Enabled = _FontStyleComboBoxHelper.HasSelection();
            addStyle.Enabled = true;
        }

        private FontStyle SetFontStyle(bool Bold, bool Italic, bool Underline)
        {
            FontStyle _style = FontStyle.Regular;
            if (Bold)
            {
                _style = _style | FontStyle.Bold;
            }
            if (Italic)
            {
                _style = _style | FontStyle.Italic;
            }
            if (Underline)
            {
                _style = _style | FontStyle.Underline;
            }
            return _style;
        }

        private void ToolStrip1_Resize(object sender, EventArgs e)
        {
            rtf.Top = toolStrip1.Height;
            rtf.Height = this.Height - toolStrip1.Height;
        }

        private void UpdateStyle_Click(object sender, EventArgs e)
        {
            var _item = _FontStyleComboBoxHelper.GetSelected();
            if (_item == null)
                return;

            _item.Color = rtf.SelectionColor;
            _item.FontSize = rtf.SelectionFont.Size;
            _item.FontFamily = rtf.SelectionFont.Name;
            _item.FontStyle = rtf.SelectionFont.Style;
            _item.HorizontalAlignment = rtf.SelectionAlignment;
            SetButtons();
        }

    }
}