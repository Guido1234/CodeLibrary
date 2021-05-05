using CodeLibrary.Core;
using CodeLibrary.Editor;
using System;
using System.Windows.Forms;

namespace CodeLibrary.Helpers
{
    public class ClipboardMonitorHelper
    {
        private readonly Timer _timer = new Timer();
        private FormCodeLibrary _mainform;
        private string _prevClipboard = string.Empty;
        private StateIconHelper _StateIconHelper;
        private TextBoxHelper _textBoxHelper;
        private TreeviewHelper _treeviewHelper;

        public ClipboardMonitorHelper(FormCodeLibrary mainform, TextBoxHelper textBoxHelper, TreeviewHelper treeviewHelper, StateIconHelper stateIconHelper)
        {
            _mainform = mainform;
            _textBoxHelper = textBoxHelper;
            _treeviewHelper = treeviewHelper;
            _StateIconHelper = stateIconHelper;

            _timer.Enabled = false;
            _timer.Interval = 100;
            _timer.Tick += _timer_Tick;

            _mainform.mnuRecordClipboard.Click += MnuRecordClipboard_Click;
            _mainform.mncClearClipboardMonitor.Click += MnuClearClipboardMonior_Click;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            string _text = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(_text))
                return;

            if (_prevClipboard.Equals(_text))
                return;

            CodeSnippet _currentSnippet = _treeviewHelper.FromNode(_treeviewHelper.SelectedNode);
            if (_currentSnippet.Name == Constants.CLIPBOARDMONITOR && _currentSnippet.CodeType == CodeType.System)
            {
                _textBoxHelper.Text = _textBoxHelper.Text + _text + "\r\n";
            }
            else
            {
                CodeSnippet _clipboardSnippet = CodeLib.Instance.ClipboardMonitor;
                _clipboardSnippet.SetCode(_clipboardSnippet.GetCode() + _text + "\r\n", out bool _changed);
            }
            _prevClipboard = _text;
        }

        private void MnuClearClipboardMonior_Click(object sender, EventArgs e)
        {
            CodeSnippet _currentSnippet = _treeviewHelper.FromNode(_treeviewHelper.SelectedNode);
            if (_currentSnippet.Name == Constants.CLIPBOARDMONITOR && _currentSnippet.CodeType == CodeType.System)
            {
                _textBoxHelper.Text = string.Empty;
            }
            else
            {
                CodeSnippet _clipboardSnippet = CodeLib.Instance.ClipboardMonitor;
                _clipboardSnippet.SetCode(string.Empty, out bool _changed);
            }
        }

        private void MnuRecordClipboard_Click(object sender, EventArgs e)
        {
            _timer.Enabled = !_timer.Enabled;
            _mainform.mnuRecordClipboard.Checked = _timer.Enabled;
            _StateIconHelper.ClipBoardMonitor = _timer.Enabled;
        }
    }
}