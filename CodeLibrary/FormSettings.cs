using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeLibrary
{
    public partial class FormSettings : Form
    {
        private EnumComboBoxModeHelper<ETheme> _ComboBoxHelper;

        public FormSettings()
        {
            InitializeComponent();         
            _ComboBoxHelper = new EnumComboBoxModeHelper<ETheme>(comboBoxTheme, ETheme.Dark);
            _ComboBoxHelper.Fill();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            _ComboBoxHelper.SetSelectedIndex(Config.Theme);
            rbSortA.Checked = Config.SortMode == ESortMode.Alphabetic;
            rbSortB.Checked = Config.SortMode == ESortMode.AlphabeticGrouped;
        }

        private void dialogButton_DialogButtonClick(object sender, Controls.DialogButton.DialogButtonClickEventArgs e)
        {
            DialogResult = e.Result;
            if (e.Result == DialogResult.OK)
            {
                Config.Theme = (ETheme)_ComboBoxHelper.GetValue();
                if (rbSortA.Checked)
                {
                    Config.SortMode = ESortMode.Alphabetic;
                }
                if (rbSortB.Checked)
                {
                    Config.SortMode = ESortMode.AlphabeticGrouped;
                }
            }
            Close();
        }
    }
}
