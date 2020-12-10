using System;
using System.IO;
using System.Text.RegularExpressions;

namespace GK.Template
{
    /// <summary>
    /// Scale to use for filesizes.
    /// </summary>
    public enum FileSizeScale
    {
        /// <summary>
        /// Size in bytes
        /// </summary>
        Bytes = 0,

        /// <summary>
        /// size in kilobytes
        /// </summary>
        KB = 1,

        /// <summary>
        /// size in megabytes
        /// </summary>
        MB = 2,

        /// <summary>
        /// size in gigabytes
        /// </summary>
        GB = 3
    }

    public enum ParseSpecialFolderOption
    {
        WildCardToRealPath = 0,
        RealPathToWildCard = 1
    }

    /// <summary>
    /// <para>Set of Path related functions.</para>
    /// <para>All methods within this class affects the path but not the actual file or directory</para>
    /// <para>For real file operations see FileUtility</para>
    /// </summary>
    public static class PathUtility
    {
        /// <summary>
        /// ensures a directory ends with \ (Alias to StringUtility.EnsureEndWith)
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string CorrectDirectorySlash(string directory)
        {
            return Utils.EnsureEndWith(directory, Path.DirectorySeparatorChar);
        }

        /// <summary>
        ///  ensures a directory ends with \ (Alias to StringUtility.EnsureEndWith)
        /// </summary>
        /// <param name="directory">Web path or directory</param>
        /// <param name="separator">use \ or /</param>
        /// <returns></returns>
        public static string CorrectDirectorySlash(string directory, char separator)
        {
            return Utils.EnsureEndWith(directory, separator);
        }

        /// <summary>
        /// Method will find the most basic directory for all given paths on a specific drive.
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string FindBaseDirectory(DriveInfo drive, params string[] paths)
        {
            if (paths == null)
            {
                return drive.RootDirectory.FullName;
            }
            string path = string.Empty;
            string prevPath = string.Empty;
            path = paths[0];
            foreach (string file in paths)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    if (file.StartsWith(drive.RootDirectory.FullName, StringComparison.OrdinalIgnoreCase))
                    {
                        path = file;
                        break;
                    }
                }
            }
            foreach (string file in paths)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    if (file.StartsWith(drive.RootDirectory.FullName, StringComparison.OrdinalIgnoreCase))
                    {
                        path = GetComparingPath(file, path);
                    }
                }
            }
            return path;
        }

        /// <summary>
        /// Returns formatted file / directory size string.
        /// </summary>
        /// <param name="psize">size to format</param>
        /// <returns></returns>
        public static string FormatSize(long psize)
        {
            decimal size = psize;
            string postfix = "bytes";

            if (psize > 1024)
            {
                size = psize;
                postfix = "kb";
                size = size / 1024;
            }
            if (psize > (1024 * 1024))
            {
                size = psize;
                postfix = "mb";
                size = size / (1024 * 1024);
            }
            if (psize > (1024 * 1024 * 1024))
            {
                size = psize;
                postfix = "gb";
                size = size / (1024 * 1024 * 1024);
            }
            return string.Format("{0} {1}", Math.Round(size, 2), postfix);
        }

        /// <summary>
        /// return the matching part of the paths between two paths.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string GetComparingPath(string path1, string path2)
        {
            return GetComparingPath(path1, path2, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// return the matching part of the paths between two paths.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="separator">Separator to use</param>
        /// <returns></returns>
        public static string GetComparingPath(string path1, string path2, char separator)
        {
            if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2))
            {
                return string.Empty;
            }
            string[] patha = SplitPath(path1.ToLower(), separator);
            string[] pathb = SplitPath(path2.ToLower(), separator);
            string prevpath = string.Empty;
            if (patha.Length > pathb.Length)
            {
                for (int ii = 0; ii < pathb.Length; ii++)
                {
                    if (!patha[ii].Equals(pathb[ii], StringComparison.OrdinalIgnoreCase))
                    {
                        return prevpath;
                    }
                    prevpath = patha[ii];
                }
            }
            if (pathb.Length >= patha.Length)
            {
                for (int ii = 0; ii < patha.Length; ii++)
                {
                    if (!pathb[ii].Equals(patha[ii], StringComparison.OrdinalIgnoreCase))
                    {
                        return prevpath;
                    }
                    prevpath = pathb[ii];
                }
            }
            return prevpath;
        }

        /// <summary>
        /// Determines whether 2 directories share the same base directory.
        /// </summary>
        /// <param name="dir1"></param>
        /// <param name="dir2"></param>
        /// <returns></returns>
        public static bool IsPartOfPath(DirectoryInfo dir1, DirectoryInfo dir2)
        {
            if (dir1.FullName.StartsWith(dir2.FullName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (dir2.FullName.StartsWith(dir1.FullName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Match a pattern like: *.txt;*.jpg;
        /// </summary>
        /// <param name="s">string to test.</param>
        /// <param name="pattern">pattern to match</param>
        /// <returns></returns>
        public static bool MatchPattern(string s, string pattern)
        {
            if (pattern == null)
            {
                return false;
            }
            if (s == null)
            {
                return false;
            }
            string[] patterns = pattern.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string subpattern in patterns)
            {
                string pat = string.Format("^{0}$", Regex.Escape(subpattern).Replace("\\*", ".*").Replace("\\?", "."));
                Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
                if (regex.IsMatch(s))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fixes a filename only, removes invalid chars like :\/?*|: from string.
        /// </summary>
        /// <param name="filename">Filename only to normalize</param>
        /// <param name="invalidCharReplacement">Replacementchar for invalid chars.</param>
        /// <returns>string.Empty when filename is null or empty.</returns>
        public static string NormalizeFileName(string filename, char invalidCharReplacement)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return string.Empty;
            }
            char[] chfile = filename.ToCharArray();
            char[] invalidFileChars = Path.GetInvalidFileNameChars();
            for (int ii = 0; ii < chfile.Length; ii++)
            {
                for (int xx = 0; xx < invalidFileChars.Length; xx++)
                {
                    if (chfile[ii] == invalidFileChars[xx])
                    {
                        chfile[ii] = invalidCharReplacement;
                    }
                }
            }
            return new string(chfile);
        }

        /// <summary>
        /// Fixes a full path, removes invalid chars like :\/?*|: from string.
        /// </summary>
        /// <param name="fullpath">full path to normalize.</param>
        /// <returns>string.Empty when fullpath is null or empty.</returns>
        public static string NormalizeFullPath(string fullpath)
        {
            return NormalizeFullPath(fullpath, ' ');
        }

        /// <summary>
        /// Fixes a full path, removes invalid chars like :\/?*|: from string.
        /// </summary>
        /// <param name="fullpath">full path to normalize.</param>
        /// <param name="invalidCharReplacement">Replacementchar for invalid chars.</param>
        /// <returns>string.Empty when fullpath is null or empty.</returns>
        public static string NormalizeFullPath(string fullpath, char invalidCharReplacement)
        {
            if (string.IsNullOrEmpty(fullpath))
            {
                return string.Empty;
            }
            int filenameIndex = fullpath.LastIndexOf(Path.DirectorySeparatorChar);

            char[] chpath = fullpath.ToCharArray();
            char[] invalidPathChars = Path.GetInvalidPathChars();
            char[] invalidFileChars = Path.GetInvalidFileNameChars();

            for (int ii = 0; ii < chpath.Length; ii++)
            {
                if (ii <= filenameIndex)
                {
                    for (int xx = 0; xx < invalidPathChars.Length; xx++)
                    {
                        if (chpath[ii] == invalidPathChars[xx])
                        {
                            chpath[ii] = invalidCharReplacement;
                        }
                    }
                }
                else
                {
                    for (int xx = 0; xx < invalidFileChars.Length; xx++)
                    {
                        if (chpath[ii] == invalidFileChars[xx])
                        {
                            chpath[ii] = invalidCharReplacement;
                        }
                    }
                }
            }
            return new string(chpath);
        }

        public static string ParentPath(string path)
        {
            return ParentPath(path, '\\');
        }

        public static string ParentPath(string path, char separator)
        {
            string[] paths = PathUtility.SplitPath(path, separator);
            if (paths.Length <= 1)
                return string.Empty;
            return paths[paths.Length - 2];
        }

        /// <summary>
        /// replaces #[Special folder name]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ParseSpecialFoldersNames(string path, ParseSpecialFolderOption option)
        {
            if (path == null)
            {
                return string.Empty;
            }
            string[] names = Enum.GetNames(typeof(Environment.SpecialFolder));
            if (option == ParseSpecialFolderOption.WildCardToRealPath)
            {
                foreach (string name in names)
                {
                    Environment.SpecialFolder specialfolder = (Environment.SpecialFolder)EnumUtility.GetValueByName(typeof(Environment.SpecialFolder), name);
                    path = path.Replace(string.Format("#{0}", name), Environment.GetFolderPath(specialfolder));
                }
                return path;
            }

            foreach (string name in names)
            {
                Environment.SpecialFolder specialfolder = (Environment.SpecialFolder)EnumUtility.GetValueByName(typeof(Environment.SpecialFolder), name);
                string toreplace = Environment.GetFolderPath(specialfolder);
                if (!string.IsNullOrEmpty(toreplace))
                {
                    path = path.Replace(Environment.GetFolderPath(specialfolder), string.Format("#{0}", name));
                }
            }
            return path;
        }

        public static string PathName(string path)
        {
            return PathName(path, '\\');
        }

        public static string PathName(string path, char separator)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            string[] paths = path.Split(separator);
            if (paths.Length == 1)
                return paths[0];
            return paths[paths.Length - 1];
        }

        /// <summary>
        /// splits a path to each separate item
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <example>
        /// <code lang="cs">
        /// Console.WriteLine(SplitPath(@"C:\Program Files\Apps"));
        ///
        /// // result:
        /// // C:
        /// // C:\Program Files
        /// // C:\Program Files\Apps
        /// // </code>
        /// // </example>
        public static string[] SplitPath(string path)
        {
            return SplitPath(path, Path.DirectorySeparatorChar);
        }

        public static string[] SplitPath(string path, char separator)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new string[0];
            }
            string[] items = path.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 0)
            {
                return new string[0];
            }
            if (items.Length == 1)
            {
                return items;
            }
            string[] items2 = new string[items.Length];
            items2[0] = items[0];
            for (int ii = 1; ii < items.Length; ii++)
            {
                items2[ii] = string.Format("{0}{1}{2}", items2[ii - 1], separator, items[ii]);
            }
            for (int ii = 0; ii < items.Length; ii++)
            {
                items[ii] = items2[(items.Length - 1) - ii];
            }
            return items2;
        }
    }
}