using FastColoredTextBoxNS;
using System;
using System.Windows.Forms;

namespace CodeLibrary
{
    public class CodeInsight
    {
        private static CodeInsight _Instance = null;

        private bool _Initialized = false;
        private BaseInsightHandler _InsightHandler;
        private ListBox _ListBoxInsight;
        private FastColoredTextBox _TbCode;

        private CodeInsight()
        { }

        public static CodeInsight Instance => _Instance ?? (_Instance = new CodeInsight());

        public void Init(ListBox listBoxInsight, FastColoredTextBox tbCode)
        {
            if (_Initialized == true)
                return;

            _ListBoxInsight = listBoxInsight;
            _ListBoxInsight.Height = 200;
            _TbCode = tbCode;
            _ListBoxInsight.Click += ListBoxInsight_Click;
            _ListBoxInsight.KeyUp += ListBoxInsight_KeyUp;
            _ListBoxInsight.Leave += ListBoxInsight_Leave;
            _TbCode.KeyPressed += TbCode_KeyPressed;
            _Initialized = true;
        }

        public void SetInsightHandler(BaseInsightHandler insightHandler)
        {
            _InsightHandler = insightHandler;
            if (_InsightHandler == null)
                return;

            _InsightHandler.Init(_TbCode, _ListBoxInsight);
        }

        private void ListBoxInsight_Click(object sender, EventArgs e)
        {
            if (_InsightHandler == null)
                return;

            _InsightHandler.ListBoxInsight_Click(sender, e);
        }

        private void ListBoxInsight_KeyUp(object sender, KeyEventArgs e)
        {
            if (_InsightHandler == null)
                return;

            _InsightHandler.ListBoxInsight_KeyUp(sender, e);
        }

        private void ListBoxInsight_Leave(object sender, EventArgs e)
        {
            if (_InsightHandler == null)
                return;

            _InsightHandler.ListBoxInsight_Leave(sender, e);
        }

        private void TbCode_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (_InsightHandler == null)
                return;

            if (!_InsightHandler.IsInitialized)
                _InsightHandler.Initialize();

            _InsightHandler.IsInitialized = true;

            _InsightHandler.TbCode_KeyPressed(sender, e);
        }
    }
}