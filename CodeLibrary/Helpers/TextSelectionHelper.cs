using FastColoredTextBoxNS;

namespace CodeLibrary
{
    public class TextSelectionHelper
    {
        private FastColoredTextBox _fastColoredTextBox;

        public TextSelectionHelper(FastColoredTextBox fastColoredTextBox)
        {
            _fastColoredTextBox = fastColoredTextBox;
        }

        public string SelectedText
        {
            get
            {
                if (!string.IsNullOrEmpty(_fastColoredTextBox.SelectedText))
                    return _fastColoredTextBox.SelectedText;

                return _fastColoredTextBox.Text;
            }
            set
            {
                if (!string.IsNullOrEmpty(_fastColoredTextBox.SelectedText))
                    _fastColoredTextBox.SelectedText = value;
                else
                    _fastColoredTextBox.Text = value;
            }
        }

        public void SelectLine()
        {
            Range _line = _fastColoredTextBox.GetLine(_fastColoredTextBox.Selection.Start.iLine);
            Place _start = new Place(0, _fastColoredTextBox.Selection.Start.iLine);
            Place _end = new Place(_line.End.iChar, _fastColoredTextBox.Selection.Start.iLine);
            _fastColoredTextBox.Selection = new Range(_fastColoredTextBox, _start, _end);
        }
    }
}