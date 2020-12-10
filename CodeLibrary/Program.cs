using System;
using System.IO;
using System.Windows.Forms;

namespace CodeLibrary
{
    internal static class Program
    {
        public static string AppDataPath
        {
            get
            {
                return Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "GK.CodeLibrary");
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormCodeLibrary());
        }
    }
}