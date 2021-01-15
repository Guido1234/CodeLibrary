using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeLibrary
{
    public class BackupHelper
    { 
        private string _patternDate = $"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_[0-9][0-9][0-9][0-9][0-9][0-9]";
        private Regex _regExDate;

        public BackupHelper(string currentFile)
        {
            CurrentFile = currentFile;
            _regExDate = new Regex(_patternDate);
        }

        public string CurrentFile { get; set; }

        public void Backup()
        {
            if (string.IsNullOrEmpty(CurrentFile))
                return;

            if (!File.Exists(CurrentFile))
                return;

            FileInfo file = new FileInfo(CurrentFile);
            string newName = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.bak";
            FileInfo bakfile = new FileInfo(Path.Combine(file.Directory.FullName, newName));
            
            if (!bakfile.Exists)
            {
                try
                {
                    File.Move(file.FullName, bakfile.FullName);
                }
                catch (UnauthorizedAccessException ua)
                {
                    return;
                }
                catch (Exception e)
                {
                    return;
                }
            }
            DeleteOlderbackupFiles(-21); // Delete everything.
            DeleteOlderbackupFilesKeepOnePerDay(-2); // keep latest per day.
        }

        public IEnumerable<BackupInfo> GetBackups()
        {
            FileInfo _currentfile = new FileInfo(CurrentFile);
            string _pattern = $"^{_currentfile.Name.Replace($".{_currentfile.Extension}", string.Empty)}_[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_[0-9][0-9][0-9][0-9][0-9][0-9].bak$";

            Regex _regEx = new Regex(_pattern);

            foreach (string file in Directory.GetFiles(_currentfile.Directory.FullName))
            {
                FileInfo _file = new FileInfo(file);

                if (_regEx.Match(_file.Name).Success)
                {
                    DateTime _date = GeDateFromFileName(_file.Name);

                    var _backupInfo = new BackupInfo()
                    {
                        Name = _currentfile.Name,
                        FileName = _file.Name,
                        Path = _file.FullName,
                        DateTime = _date
                    };

                    yield return _backupInfo;
                }
            }
        }

        private void DeleteOlderbackupFiles(int days)
        {
            FileInfo file = new FileInfo(CurrentFile);

            if (!file.Directory.Exists)
                return;
 
            string pattern = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_*.bak";

            DateTime filterDate = DateTime.Now.AddDays(-2);

            IEnumerable<FileInfo> files = file.Directory.GetFiles()
                .Where(p => Utils.MatchPattern(p.Name, pattern))
                .Where(p => p.LastAccessTime < filterDate);

            foreach (FileInfo fileInfo in files)
                fileInfo.Delete();
        }

        private void DeleteOlderbackupFilesKeepOnePerDay(int days)
        {
            FileInfo file = new FileInfo(CurrentFile);

            if (!file.Directory.Exists)
                return;

            string pattern = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_*.bak";

            DateTime filterDate = DateTime.Now.AddDays(-2);

            IEnumerable<FileInfo> files = file.Directory.GetFiles()
                .Where(p => Utils.MatchPattern(p.Name, pattern))
                .Where(p => p.LastAccessTime < filterDate)
                .GroupBy(d => d.LastAccessTime.Date)
                .Select(g => g.OrderBy(o => o.LastAccessTime).Last());

            foreach (FileInfo fileInfo in files)
                fileInfo.Delete();
        }

        private DateTime GeDateFromFileName(string filename)
        {
            string _date = _regExDate.Match(filename).Value;

            int _y = Convert.ToInt32(_date.Substring(0, 4));
            int _m = Convert.ToInt32(_date.Substring(4, 2));
            int _d = Convert.ToInt32(_date.Substring(6, 2));
            int _hr = Convert.ToInt32(_date.Substring(9, 2));
            int _mn = Convert.ToInt32(_date.Substring(11, 2));
            int _sc = Convert.ToInt32(_date.Substring(13, 2));

            return new DateTime(_y, _m, _d, _hr, _mn, _sc);
        }
    }
}