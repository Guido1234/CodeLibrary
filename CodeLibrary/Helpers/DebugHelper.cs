namespace CodeLibrary.Helpers
{
    public class DebugHelper
    {
        private readonly FormCodeLibrary _mainform;
        private bool _Debug = false;
        private StateIconHelper _StateIconHelper;

        public DebugHelper(FormCodeLibrary mainform, StateIconHelper stateIconHelper)
        {
            _mainform = mainform;
            _StateIconHelper = stateIconHelper;

#if (DEBUG)
            _mainform.mnuDebug.Visible = true;
            _mainform.mnuDebugSeparator.Visible = true;
            _StateIconHelper.Debug = true;
            _Debug = true;
#else
            _mainform.mnuDebug.Visible = false;
            _mainform.mnuDebugSeparator.Visible = false;
            _StateIconHelper.Debug = false;
            _Debug = false;
#endif
        }

        public bool Debug => _Debug;
    }
}