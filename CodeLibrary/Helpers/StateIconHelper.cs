namespace CodeLibrary.Helpers
{
    public class StateIconHelper
    {
        private readonly FormCodeLibrary _mainform;

        public StateIconHelper(FormCodeLibrary mainform)
        {
            _mainform = mainform;

            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.save_16x16, "save");
            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.key_16x16, "key");
            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.paste_16x16, "clipboardmonitor", true);
            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.computer_edit_16x16, "debug");
        }

        public bool Changed
        {
            get
            {
                return _mainform.stateIcons["save"];
            }
            set
            {
                _mainform.stateIcons["save"] = value;
            }
        }

        public bool ClipBoardMonitor
        {
            get
            {
                return _mainform.stateIcons["clipboardmonitor"];
            }
            set
            {
                _mainform.stateIcons["clipboardmonitor"] = value;
            }
        }

        public bool Debug
        {
            get
            {
                return _mainform.stateIcons["debug"];
            }
            set
            {
                _mainform.stateIcons["debug"] = value;
            }
        }

        public bool Encrypted
        {
            get
            {
                return _mainform.stateIcons["key"];
            }
            set
            {
                _mainform.stateIcons["key"] = value;
            }
        }
    }
}