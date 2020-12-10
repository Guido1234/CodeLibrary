using System;
using CodeLibrary.PluginPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPlugins
{
    [TestClass]
    public class PluginTests
    {
        [TestMethod]
        public void TestTrim()
        {
            TrimLines trimlines = new TrimLines();
            trimlines.Apply(@"\\AAA\");

        }
    }
}
