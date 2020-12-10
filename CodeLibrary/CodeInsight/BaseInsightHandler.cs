using FastColoredTextBoxNS;
using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public abstract class BaseInsightHandler
    {
        protected ListBox _ListBoxInsight;
        protected FastColoredTextBox _TbCode;

        public bool IsInitialized { get; set; } = false;

        public void Init(FastColoredTextBox tbCode, ListBox listBoxInsight)
        {
            _TbCode = tbCode;
            _ListBoxInsight = listBoxInsight;
        }

        public virtual void Initialize()
        { }

        public virtual void ListBoxInsight_Click(object sender, EventArgs e)
        { }

        public virtual void ListBoxInsight_KeyUp(object sender, KeyEventArgs e)
        { }

        public virtual void ListBoxInsight_Leave(object sender, EventArgs e)
        { }

        public virtual void TbCode_KeyPressed(object sender, KeyPressEventArgs e)
        { }
    }
}