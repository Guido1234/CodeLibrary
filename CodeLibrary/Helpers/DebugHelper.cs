﻿using CodeLibrary.Core;
using DevToys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace CodeLibrary.Helpers
{
    public class DebugHelper
    {
        private readonly FormCodeLibrary _mainform;
        private bool _Debug;

        public DebugHelper(FormCodeLibrary mainform)
        {
            _mainform = mainform;

#if (DEBUG)
            _mainform.mnuDebug.Visible = true;
            _mainform.debugSeparator.Visible = true;
            _mainform.mnuReadOnlyLibrary.Click += MnuReadOnlyLibrary_Click;
#else
            _mainform.mnuDebug.Visible = false;
            _mainform.debugSeparator.Visible = false;
#endif

        }

        public bool Debug => _Debug;

        private void MnuReadOnlyLibrary_Click(object sender, EventArgs e)
        {
            
        }
    }
}