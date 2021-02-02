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
        private TextBoxHelper _textBoxHelper;
        private TreeviewHelper _treeviewHelper;

        public ClipboardMonitorHelper(FormCodeLibrary mainform, TextBoxHelper textBoxHelper, TreeviewHelper treeviewHelper)
        {
            _mainform = mainform;
            _textBoxHelper = textBoxHelper;
            _treeviewHelper = treeviewHelper;

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
                _clipboardSnippet.Code = _clipboardSnippet.Code + _text + "\r\n";
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
                _clipboardSnippet.Code = string.Empty;
            }
        }

        private void MnuRecordClipboard_Click(object sender, EventArgs e)
        {
            _timer.Enabled = !_timer.Enabled;
            _mainform.mnuRecordClipboard.Checked = _timer.Enabled;
        }
    }
}