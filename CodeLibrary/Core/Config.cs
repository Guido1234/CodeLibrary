using CodeLibrary.Core;
using DevToys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CodeLibrary
{
    public static class Config
    {
        public static string AppFolder { get; set; }
        public static bool DarkMode { get; set; } = true;

        public static string FavoriteFile => Utils.PathCombine(AppFolder, "Favorite.json");
        public static List<string> FavoriteLibs { get; set; } = new List<string>();
        public static bool HighContrastMode { get; set; } = true;
        public static string LastOpenedDir { get; set; }
        public static string LastOpenedFile { get; set; }
        public static bool OpenDefaultOnStart { get; set; }
        public static string PluginPath { get; set; }
        public static int Zoom { get; set; } = 100;

        public static bool IsNewVersion()
        {
            string regpath = Regpath();
            VersionNumber _prevVersion = new VersionNumber(Utils.GetCurrentUserRegisterKey(regpath, Constants.VERSION));
            VersionNumber _currentVersion = new VersionNumber(Assembly.GetExecutingAssembly().GetName().Version.ToString());

            return _currentVersion > _prevVersion;
        }

        public static void Load()
        {
            string regpath = Regpath();
            LastOpenedDir = Utils.GetCurrentUserRegisterKey(regpath, Constants.LASTOPENEDDIR);
            LastOpenedFile = Utils.GetCurrentUserRegisterKey(regpath, Constants.LASTOPENEDFILE);
            OpenDefaultOnStart = ConvertUtility.ToBoolean(Utils.GetCurrentUserRegisterKey(regpath, Constants.OPENDEFAULTONSTART), false);
            Zoom = ConvertUtility.ToInt32(Utils.GetCurrentUserRegisterKey(regpath, Constants.ZOOM), 100);
            DarkMode = ConvertUtility.ToBoolean(Utils.GetCurrentUserRegisterKey(regpath, Constants.DARKMODE), true);

            HighContrastMode = ConvertUtility.ToBoolean(Utils.GetCurrentUserRegisterKey(regpath, Constants.HIGHCONTRASTMODE), false);

            PluginPath = Utils.GetCurrentUserRegisterKey(regpath, Constants.PLUGINPATH);

            AppFolder = Utils.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Guido Utilities", "CodeLibrary");

            if (File.Exists(FavoriteFile))
            {
                string _json = File.ReadAllText(FavoriteFile);
                FavoriteLibs = Utils.FromJson<List<string>>(_json);
            }
        }

        public static void Save()
        {
            string regpath = string.Format(Utils.REG_USRSETTING, Constants.CODELIBRARY);
            Utils.SetCurrentUserRegisterKey(regpath, Constants.LASTOPENEDDIR, LastOpenedDir);
            Utils.SetCurrentUserRegisterKey(regpath, Constants.LASTOPENEDFILE, LastOpenedFile);
            Utils.SetCurrentUserRegisterKey(regpath, Constants.OPENDEFAULTONSTART, OpenDefaultOnStart.ToString());
            Utils.SetCurrentUserRegisterKey(regpath, Constants.ZOOM, Zoom.ToString());
            Utils.SetCurrentUserRegisterKey(regpath, Constants.DARKMODE, DarkMode.ToString());
            Utils.SetCurrentUserRegisterKey(regpath, Constants.HIGHCONTRASTMODE, HighContrastMode.ToString());

            Utils.SetCurrentUserRegisterKey(regpath, Constants.PLUGINPATH, PluginPath);

            if (!Directory.Exists(AppFolder))
                Directory.CreateDirectory(AppFolder);

            if (File.Exists(FavoriteFile))
                File.Delete(FavoriteFile);

            string _json = Utils.ToJson<List<string>>(FavoriteLibs);

            File.WriteAllText(FavoriteFile, _json);

            try
            {
                VersionNumber _version = new VersionNumber(Assembly.GetExecutingAssembly().GetName().Version.ToString());
                string _versionString = _version.ToString();
                if (string.IsNullOrEmpty(_versionString))
                    _versionString = string.Empty;
                Utils.SetCurrentUserRegisterKey(regpath, Constants.VERSION, _versionString);
            }
            catch { }
        }

        private static string Regpath() => string.Format(Utils.REG_USRSETTING, Constants.CODELIBRARY);
    }
}