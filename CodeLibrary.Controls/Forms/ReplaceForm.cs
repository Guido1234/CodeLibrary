using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary.Controls.Forms
{
    public partial class ReplaceForm : Form
    {
        private RichTextBox tb;

        public ReplaceForm(RichTextBox tb)
        {
            InitializeComponent();
            this.tb = tb;
        }

        public virtual void FindNext(string pattern)
        {
            try
            {
                RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
                if (!cbRegex.Checked)
                    pattern = Regex.Escape(pattern);
                if (cbWholeWord.Checked)
                    pattern = "\\b" + pattern + "\\b";

                Regex _regex = new Regex(pattern);

                Range range = new Range() { Start = tb.SelectionStart, Length = tb.SelectionLength };
                //

                var _matches = _regex.Matches(tb.Text);
                if (_matches.Count > 0)
                {
                    foreach (Match match in _matches)
                    {
                        if (match.Index > range.Start)
                        {
                            if (match.Success)
                            {
                                tb.SelectionStart = match.Index;
                                tb.SelectionLength = match.Length;
                                return;
                            }
                        }
                    }
                    tb.SelectionStart = 0;
                    tb.SelectionLength = 0;
                    MessageBox.Show("No more results.");
                }
                else
                {
                    MessageBox.Show("Not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public virtual void ReplaceAll(string pattern, string replace)
        {
            try
            {
                RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
                if (!cbRegex.Checked)
                    pattern = Regex.Escape(pattern);
                if (cbWholeWord.Checked)
                    pattern = "\\b" + pattern + "\\b";

                Regex _regex = new Regex(pattern);

                Range range = new Range() { Start = tb.SelectionStart, Length = tb.SelectionLength };
                //

                var _matches = _regex.Matches(tb.Text);
                if (_matches.Count > 0)
                {
                    for (int ii = _matches.Count - 1; ii > -1; ii--)
                    {
                        Match match = _matches[ii];
                        if (match.Success)
                        {
                            tb.SelectionStart = match.Index;
                            tb.SelectionLength = match.Length;
                            tb.SelectedText = replace;
                        }
                    }
                    tb.SelectionStart = 0;
                    tb.SelectionLength = 0;
                }
                else
                {
                    MessageBox.Show("Not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public virtual void ReplaceNext(string pattern, string replace)
        {
            try
            {
                RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
                if (!cbRegex.Checked)
                    pattern = Regex.Escape(pattern);
                if (cbWholeWord.Checked)
                    pattern = "\\b" + pattern + "\\b";

                Regex _regex = new Regex(pattern);

                var _matches = _regex.Matches(tb.Text);
                if (_matches.Count > 0)
                {
                    Match match = _matches[0];
                    if (match.Success)
                    {
                        tb.SelectionStart = match.Index;
                        tb.SelectionLength = match.Length;
                        tb.SelectedText = replace;
                        return;
                    }

                    tb.SelectionStart = 0;
                    tb.SelectionLength = 0;
                }
                else
                {
                    MessageBox.Show("Not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            tbFind.Focus();
            ResetSerach();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // David
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btFindNext_Click(object sender, EventArgs e)
        {
            try
            {
                FindNext(tbFind.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btReplace_Click(object sender, EventArgs e)
        {
            try
            {
                ReplaceNext(tbFind.Text, tbReplace.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btReplaceAll_Click(object sender, EventArgs e)
        {
            try
            {
                ReplaceAll(tbFind.Text, tbReplace.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            ResetSerach();
        }

        private void ReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            this.tb.Focus();
        }

        private void ResetSerach()
        {
        }

        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                btFindNext_Click(sender, null);
            if (e.KeyChar == '\x1b')
                Hide();
        }
    }
}