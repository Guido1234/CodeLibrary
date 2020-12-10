using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeLibrary.Controls
{
    public partial class FormTypeSelector : Form
    {
        private readonly List<Type> _types = new List<Type>();
        private Type _selectedType = null;

        public FormTypeSelector()
        {
            InitializeComponent();
            this.typeList1.OnTypeClick += TypeList1_OnTypeClick;
            this.typeList1.OnTypeDoubleClick += TypeList1_OnTypeDoubleClick;
            this.DialogResult = DialogResult.Cancel;
        }

        public Type SelectedType
        {
            get
            {
                return _selectedType;
            }
        }

        public List<Type> Types
        {
            get
            {
                return _types;
            }
        }

        private void DialogButton1_DialogButtonClick(object sender, DialogButton.DialogButtonClickEventArgs e)
        {
            if (e.Result == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void FormTypeSelector_Load(object sender, EventArgs e)
        {
            this.typeList1.Types.AddRange(_types);
            this.typeList1.Refresh();
        }

        private void TypeList1_OnTypeClick(object sender, TypeList.TypeListEventArgs ea)
        {
            _selectedType = ea.Type;
        }

        private void TypeList1_OnTypeDoubleClick(object sender, TypeList.TypeListEventArgs ea)
        {
            _selectedType = ea.Type;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}