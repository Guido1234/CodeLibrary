using CodeLibrary.Core;
using System;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary.Helpers
{
    public class MenuHelper
    {
        private readonly FormCodeLibrary _mainform;
        private TreeviewHelper _treeviewHelper;

        public MenuHelper(FormCodeLibrary mainform, TreeviewHelper treeviewHelper)
        {
            _mainform = mainform;
            _treeviewHelper = treeviewHelper;
            _mainform.mncPasteFilelist.Click += mnuPasteFilelist_Click;
            _mainform.mnuPasteFilelist.Click += mnuPasteFilelist_Click;

            _mainform.mncPasteImage.Click += mnuPasteImage_Click;
            _mainform.mnuPasteImage.Click += mnuPasteImage_Click;

            _mainform.mncPasteImageNoCompression.Click += mnuPasteImageNoCompression_Click;
            _mainform.mnuPasteImageNoCompression.Click += mnuPasteImageNoCompression_Click;

            _mainform.mncPasteTextPerLine.Click += mnuPasteTextPerLine_Click;
            _mainform.mnuPasteTextPerLine.Click += mnuPasteTextPerLine_Click;

            _mainform.mncPasteText.Click += mnuPasteText_Click;
            _mainform.mnuPasteText.Click += mnuPasteText_Click;

            _mainform.mncCopyImage.Click += mnuCopyImage_Click;
            _mainform.mncSaveImage.Click += mnuSaveImage_Click;
            _mainform.mncCopyImageAsBase64String.Click += mnuCopyImageAsBase64String_Click;
            _mainform.mncCopyImageAsMarkDownImage.Click += mnuCopyImageAsMarkDownImage_Click;
            _mainform.mncCopyImageAsHTMLIMG.Click += mnuCopyImageAsHTMLIMG_Click;
        }

        private void mnuCopyImage_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(_mainform.imageViewer.Image);
        }

        private void mnuCopyImageAsBase64String_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.Library.Get(_treeviewHelper.SelectedId);
            string _base64 = Convert.ToBase64String(_snippet.Blob);
            Clipboard.SetText(_base64);
        }

        private void mnuCopyImageAsHTMLIMG_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.Library.Get(_treeviewHelper.SelectedId);
            string _base64 = Convert.ToBase64String(_snippet.Blob);
            Clipboard.SetText(string.Format(@"<img src=""data:image/png;base64,{0}"" />", _base64));
        }

        private void mnuCopyImageAsMarkDownImage_Click(object sender, EventArgs e)
        {
            CodeSnippet _snippet = CodeLib.Instance.Library.Get(_treeviewHelper.SelectedId);
            string _base64 = Convert.ToBase64String(_snippet.Blob);
            Clipboard.SetText(string.Format(@"![{0}](data:image/png;base64,{1})", _snippet.Title(), _base64));
        }

        private void mnuPasteFilelist_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsFileDropList())
            {
                _treeviewHelper.PasteClipBoardFileList();
            }
        }

        private void mnuPasteImage_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                _treeviewHelper.AddImageNode(_mainform.treeViewLibrary.SelectedNode, Clipboard.GetImage(), "image");
            }
        }

        private void mnuPasteImageNoCompression_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                _treeviewHelper.AddImageNodeNoCompression(_mainform.treeViewLibrary.SelectedNode, Clipboard.GetImage(), "image");
            }
        }

        private void mnuPasteText_Click(object sender, EventArgs e)
        {            
            if (Clipboard.ContainsText())
            {                
                _treeviewHelper.CreateNewNode(_mainform.treeViewLibrary.SelectedNode.Nodes, CodeType.None, "New Note", Clipboard.GetText(), "");
            }
        }

        private void mnuPasteTextPerLine_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                _treeviewHelper.PasteClipBoardEachLine();
            }
        }

        private void mnuSaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.Filter = "JPEG Image|*.jpg";
            DialogResult _result = _dialog.ShowDialog();
            if (_result == DialogResult.OK)
            {
                string _filename = _dialog.FileName;

                CodeSnippet _snippet = CodeLib.Instance.Library.Get(_treeviewHelper.SelectedId);
                File.WriteAllBytes(_filename, _snippet.Blob);
            }
        }
    }
}