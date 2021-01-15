using CodeLibrary.Core;
using System.Drawing;
using System.Windows.Forms;

namespace CodeLibrary.Helpers
{
    public class ThemeHelper
    {
        private readonly FormCodeLibrary _mainform;
        private RtfTheme _CurrentTheme;


        public ThemeHelper(FormCodeLibrary mainform)
        {
            _mainform = mainform;
        }


        public void RichTextBoxTheme(RichTextBox rtb)
        {
            switch (_CurrentTheme)
            {
                case RtfTheme.Dark:
                    rtb.BackColor = Color.FromArgb(255, 40, 40, 40);
                    rtb.ForeColor = Color.FromArgb(255, 255, 255, 255);
                    break;
                case RtfTheme.Light:
                    rtb.BackColor = Color.FromArgb(255, 255, 255, 255);
                    rtb.ForeColor = Color.FromArgb(255, 0, 0, 0);
                    break;
                case RtfTheme.HighContrast:
                    rtb.BackColor = Color.FromArgb(255, 10, 10, 10);
                    rtb.ForeColor = Color.FromArgb(255, 255, 255, 255);
                    break;
            }
        }

        public void DarkTheme()
        {
            _CurrentTheme = RtfTheme.Dark;

            Config.DarkMode = true;
            Config.HighContrastMode = false;
            _mainform.mnuModeDark.Checked = true;
            _mainform.mnuModeLight.Checked = false;
            _mainform.mnuModeHighContrast.Checked = false;

            _mainform.textBoxFind.BackColor = Color.FromArgb(255, 40, 40, 40);
            _mainform.textBoxFind.ForeColor = Color.LightYellow;
            _mainform.buttonFind.BackColor = Color.FromArgb(255, 100, 100, 100);
            _mainform.buttonFind.ForeColor = Color.White;

            _mainform.mnuMainStrip.ForeColor = Color.White;
            _mainform.mnuMainStrip.BackColor = Color.FromArgb(255, 100, 100, 100);

            _mainform.mnuMain.BackColor = Color.FromArgb(255, 100, 100, 100);
            _mainform.ForeColor = Color.White;
            _mainform.BackColor = Color.FromArgb(255, 100, 100, 100);
            _mainform.treeViewLibrary.ForeColor = Color.White;
            _mainform.treeViewLibrary.BackColor = Color.FromArgb(255, 75, 75, 75);
            _mainform.rtfEditor.Theme = RtfTheme.Dark;

            _mainform.fastColoredTextBox.IndentBackColor = Color.FromArgb(255, 75, 75, 75);
            _mainform.fastColoredTextBox.BackColor = Color.FromArgb(255, 40, 40, 40);
            _mainform.fastColoredTextBox.CaretColor = Color.White;
            _mainform.fastColoredTextBox.ForeColor = Color.LightGray;
            _mainform.fastColoredTextBox.SelectionColor = Color.Red;
            _mainform.fastColoredTextBox.LineNumberColor = Color.LightSeaGreen;
            _mainform.tbPath.ForeColor = Color.Yellow;
            _mainform.tbPath.BackColor = Color.FromArgb(255, 100, 100, 100);
            _mainform.pictureBox1.BackColor = Color.FromArgb(255, 100, 100, 100);
            _mainform.containerTreeview.BackColor = Color.FromArgb(255, 75, 75, 75);
            _mainform.containerLeft.BackColor = Color.FromArgb(255, 75, 75, 75);

            _mainform.containerImage.BackColor = Color.FromArgb(255, 75, 75, 75);
            _mainform.containerCode.BackColor = Color.FromArgb(255, 75, 75, 75);
            _mainform.containerRtfEditor.BackColor = Color.FromArgb(255, 75, 75, 75);

            _mainform.containerInfoBar.BackColor = Color.FromArgb(255, 75, 75, 75);
            _mainform.labelStartText.ForeColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.labelEndText.ForeColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.lblStart.ForeColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.lblEnd.ForeColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.labelZoomPerc.ForeColor = Color.FromArgb(255, 255, 255, 255);

            _mainform.imageViewer.BackColor = Color.FromArgb(255, 0, 0, 0);

            _mainform.labelEndText.ForeColor = Color.Yellow;
            _mainform.labelStartText.ForeColor = Color.Yellow;
            _mainform.labelZoomPerc.ForeColor = Color.Yellow;
            _mainform.lblStart.ForeColor = Color.Yellow;
            _mainform.lblEnd.ForeColor = Color.Yellow;
            _mainform.lblLength.ForeColor = Color.Yellow;
            _mainform.lblLengthText.ForeColor = Color.Yellow;


            _mainform.fastColoredTextBox.DarkStyle();
            _mainform.fastColoredTextBox.Refresh();
        }

        public void HighContrastTheme()
        {
            _CurrentTheme = RtfTheme.HighContrast;

            Config.DarkMode = true;
            Config.HighContrastMode = true;
            _mainform.mnuModeDark.Checked = false;
            _mainform.mnuModeLight.Checked = false;
            _mainform.mnuModeHighContrast.Checked = true;

            _mainform.textBoxFind.BackColor = Color.FromArgb(255, 10, 10, 10);
            _mainform.textBoxFind.ForeColor = Color.LightYellow;
            _mainform.buttonFind.BackColor = Color.FromArgb(255, 60, 60, 60);
            _mainform.buttonFind.ForeColor = Color.White;

            _mainform.mnuMainStrip.ForeColor = Color.White;
            _mainform.mnuMainStrip.BackColor = Color.FromArgb(255, 60, 60, 60);

            _mainform.mnuMain.BackColor = Color.FromArgb(255, 60, 60, 60);
            _mainform.ForeColor = Color.White;
            _mainform.BackColor = Color.FromArgb(255, 100, 100, 100);
            _mainform.treeViewLibrary.ForeColor = Color.White;
            _mainform.treeViewLibrary.BackColor = Color.FromArgb(255, 35, 35, 35);
            _mainform.fastColoredTextBox.IndentBackColor = Color.FromArgb(255, 35, 35, 35);
            _mainform.rtfEditor.Theme = RtfTheme.HighContrast;

            _mainform.fastColoredTextBox.BackColor = Color.FromArgb(255, 10, 10, 10);
            _mainform.fastColoredTextBox.CaretColor = Color.White;
            _mainform.fastColoredTextBox.ForeColor = Color.LightGray;
            _mainform.fastColoredTextBox.SelectionColor = Color.Red;
            _mainform.fastColoredTextBox.LineNumberColor = Color.LightSeaGreen;
            _mainform.tbPath.ForeColor = Color.Yellow;
            _mainform.tbPath.BackColor = Color.FromArgb(255, 60, 60, 60);
            _mainform.pictureBox1.BackColor = Color.FromArgb(255, 60, 60, 60);
            _mainform.containerTreeview.BackColor = Color.FromArgb(255, 35, 35, 35);
            _mainform.containerLeft.BackColor = Color.FromArgb(255, 35, 35, 35);

            _mainform.containerInfoBar.BackColor = Color.FromArgb(255, 35, 35, 35);

            _mainform.containerImage.BackColor = Color.FromArgb(255, 35, 35, 35);
            _mainform.containerCode.BackColor = Color.FromArgb(255, 35, 35, 35);
            _mainform.containerRtfEditor.BackColor = Color.FromArgb(255, 35, 35, 35);

            _mainform.labelStartText.ForeColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.labelEndText.ForeColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.lblStart.ForeColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.lblEnd.ForeColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.labelZoomPerc.ForeColor = Color.FromArgb(255, 255, 255, 255);

            _mainform.imageViewer.BackColor = Color.FromArgb(255, 0, 0, 0);

            _mainform.labelEndText.ForeColor = Color.Yellow;
            _mainform.labelStartText.ForeColor = Color.Yellow;
            _mainform.labelZoomPerc.ForeColor = Color.Yellow;
            _mainform.lblStart.ForeColor = Color.Yellow;
            _mainform.lblEnd.ForeColor = Color.Yellow;
            _mainform.lblLength.ForeColor = Color.Yellow;
            _mainform.lblLengthText.ForeColor = Color.Yellow;


            _mainform.fastColoredTextBox.HighContrastStyle();
            _mainform.fastColoredTextBox.Refresh();
        }

        public void LightTheme()
        {
            _CurrentTheme = RtfTheme.Light;

            Config.DarkMode = false;
            Config.HighContrastMode = false;
            _mainform.mnuModeDark.Checked = false;
            _mainform.mnuModeLight.Checked = true;
            _mainform.mnuModeHighContrast.Checked = false;

            _mainform.textBoxFind.BackColor = Color.White;
            _mainform.textBoxFind.ForeColor = Color.Black;
            _mainform.buttonFind.BackColor = SystemColors.ButtonFace;
            _mainform.buttonFind.ForeColor = Color.Black;

            _mainform.mnuMainStrip.ForeColor = Color.FromArgb(255, 0, 0, 0);
            _mainform.mnuMainStrip.BackColor = Color.FromArgb(255, 240, 240, 240);
            _mainform.ForeColor = Color.FromArgb(255, 0, 0, 0);
            _mainform.BackColor = Color.FromArgb(255, 240, 240, 240);
            _mainform.treeViewLibrary.ForeColor = Color.FromArgb(255, 0, 0, 0);
            _mainform.treeViewLibrary.BackColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.fastColoredTextBox.IndentBackColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.rtfEditor.Theme = RtfTheme.Light;

            _mainform.fastColoredTextBox.BackColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.fastColoredTextBox.ForeColor = Color.Black;
            _mainform.fastColoredTextBox.CaretColor = Color.White;
            _mainform.fastColoredTextBox.SelectionColor = Color.FromArgb(50, 0, 0, 255);
            _mainform.fastColoredTextBox.LineNumberColor = Color.FromArgb(255, 0, 128, 128);
            _mainform.tbPath.ForeColor = Color.Black;
            _mainform.tbPath.BackColor = Color.FromArgb(255, 240, 240, 240);
            _mainform.pictureBox1.BackColor = Color.FromArgb(255, 100, 100, 100);

            _mainform.mncLibrary.BackColor = SystemColors.ButtonFace;
            _mainform.containerTreeview.BackColor = Color.FromArgb(255, 255, 255, 255);

            _mainform.containerTreeview.BackColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.containerLeft.BackColor = Color.FromArgb(255, 255, 255, 255);

            _mainform.containerImage.BackColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.containerCode.BackColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.containerRtfEditor.BackColor = Color.FromArgb(255, 255, 255, 255);

            _mainform.containerInfoBar.BackColor = Color.FromArgb(255, 255, 255, 255);
            _mainform.labelStartText.ForeColor = Color.FromArgb(255, 0, 0, 0);
            _mainform.labelEndText.ForeColor = Color.FromArgb(255, 0, 0, 0);
            _mainform.lblStart.ForeColor = Color.FromArgb(255, 0, 0, 0);
            _mainform.lblEnd.ForeColor = Color.FromArgb(255, 0, 0, 0);
            _mainform.labelZoomPerc.ForeColor = Color.FromArgb(255, 0, 0, 0);

            _mainform.imageViewer.BackColor = Color.FromArgb(255, 125, 125, 125);

            _mainform.labelEndText.ForeColor = Color.Black;
            _mainform.labelStartText.ForeColor = Color.Black;
            _mainform.labelZoomPerc.ForeColor = Color.Black;
            _mainform.lblStart.ForeColor = Color.Black;
            _mainform.lblEnd.ForeColor = Color.Black;
            _mainform.lblLength.ForeColor = Color.Black;
            _mainform.lblLengthText.ForeColor = Color.Black;


            _mainform.fastColoredTextBox.LightStyle();
            _mainform.fastColoredTextBox.Refresh();
        }
    }
}