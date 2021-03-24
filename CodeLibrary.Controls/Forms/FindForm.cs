using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeLibrary.Controls
{
    public partial class FindForm : Form
    {
        private RichTextBox tb;

        public FindForm(RichTextBox tb)
        {
            // tb.SelectionStart
            // tb.SelectionLength

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

        protected override void OnActivated(EventArgs e)
        {
            tbFind.Focus();
            ResetSerach();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
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
            FindNext(tbFind.Text);
        }

        private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            ResetSerach();
        }

        private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
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
            {
                btFindNext.PerformClick();
                e.Handled = true;
                return;
            }
            if (e.KeyChar == '\x1b')
            {
                Hide();
                e.Handled = true;
                return;
            }
        }
    }
}