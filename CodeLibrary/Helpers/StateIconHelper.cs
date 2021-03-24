namespace CodeLibrary.Helpers
{
    public class StateIconHelper
    {
        private readonly FormCodeLibrary _mainform;

        private const string _SAVE = "save";
        private const string _KEY = "key";
        private const string _CLIPBOARDMONITOR = "clipboardmonitor";
        private const string _DEBUG = "debug";
        private const string _SELECTISCOPY = "selectiscopy";

        public StateIconHelper(FormCodeLibrary mainform)
        {
            _mainform = mainform;

            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.save_16x16, _SAVE);
            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.key_16x16, _KEY);
            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.paste_16x16, _CLIPBOARDMONITOR, true);
            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.computer_edit_16x16, _DEBUG);
            _mainform.stateIcons.AddIcon(global::CodeLibrary.Properties.Resources.copy_16x16, _SELECTISCOPY, true);
        }

        public bool Changed
        {
            get
            {
                return _mainform.stateIcons[_SAVE];
            }
            set
            {
                _mainform.stateIcons[_SAVE] = value;
            }
        }

        public bool ClipBoardMonitor
        {
            get
            {
                return _mainform.stateIcons[_CLIPBOARDMONITOR];
            }
            set
            {
                _mainform.stateIcons[_CLIPBOARDMONITOR] = value;
            }
        }

        public bool SelectIsCopy
        {
            get
            {
                return _mainform.stateIcons[_SELECTISCOPY];
            }
            set
            {
                _mainform.stateIcons[_SELECTISCOPY] = value;
            }
        }

        public bool Debug
        {
            get
            {
                return _mainform.stateIcons[_DEBUG];
            }
            set
            {
                _mainform.stateIcons[_DEBUG] = value;
            }
        }

        public bool Encrypted
        {
            get
            {
                return _mainform.stateIcons[_KEY];
            }
            set
            {
                _mainform.stateIcons[_KEY] = value;
            }
        }
    }
}