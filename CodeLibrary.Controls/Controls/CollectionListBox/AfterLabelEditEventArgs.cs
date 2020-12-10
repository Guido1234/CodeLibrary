using System;

namespace CodeLibrary.Controls
{
    public class AfterLabelEditEventArgs : EventArgs
    {
        public string NewLabel { get; set; }
        public object Tag { get; set; }
    }
}