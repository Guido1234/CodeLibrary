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

    }
}