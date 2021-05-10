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

            foreach (DirectoryInfo backupLocation in BackupLocations())
            {
                FileInfo bakfile = new FileInfo(Path.Combine(backupLocation.FullName, newName));

                if (!bakfile.Exists)
                {
                    try
                    {
                        File.Move(file.FullName, bakfile.FullName);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                DeleteOlderbackupFiles(-21); // Delete everything.
                DeleteOlderbackupFilesKeepOnePerDay(-2); // keep latest per day.
            }
        }

        public IEnumerable<DirectoryInfo> BackupLocations()
        {
            if (Config.BackupMode == EBackupMode.FileLocation || Config.BackupMode == EBackupMode.Both)
            {
                FileInfo _currentfile = new FileInfo(CurrentFile);
                if (_currentfile.Exists)
                {
                    yield return _currentfile.Directory;
                }
            }
            if (Config.BackupMode == EBackupMode.SpecifiedDirectory || Config.BackupMode == EBackupMode.Both)
            {
                string documentBackupLocation = Path.Combine(Config.BackupLocation, CodeLib.Instance.DocumentId.ToString());

                DirectoryInfo _directory = new DirectoryInfo(documentBackupLocation);
                if (!_directory.Exists)
                {
                    Directory.CreateDirectory(_directory.FullName);
                }
                yield return _directory;

            }
        }

        public IEnumerable<BackupInfo> GetBackups()
        {
            FileInfo _currentfile = new FileInfo(CurrentFile);
            string _pattern = $"^{_currentfile.Name.Replace($".{_currentfile.Extension}", string.Empty)}_[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_[0-9][0-9][0-9][0-9][0-9][0-9].bak$";

            Regex _regEx = new Regex(_pattern);

            foreach (FileInfo file in MultiDirectoryGetFiles(BackupLocations().ToArray()))
            {
                if (_regEx.Match(file.Name).Success)
                {
                    DateTime _date = GeDateFromFileName(file.Name);

                    var _backupInfo = new BackupInfo()
                    {
                        Name = _currentfile.Name,
                        FileName = file.Name,
                        Path = file.FullName,
                        DateTime = _date
                    };

                    yield return _backupInfo;
                }
            }
        }

        public IEnumerable<FileInfo> MultiDirectoryGetFiles(params DirectoryInfo[] directories)
        {
            foreach (DirectoryInfo directory in directories)
            {
                foreach (FileInfo file in directory.EnumerateFiles())
                {
                    yield return file;
                }
            }
        }

        private void DeleteOlderbackupFiles(int days)
        {
            foreach (DirectoryInfo directory in BackupLocations())
            {
                FileInfo _fileCurrentFile = new FileInfo(CurrentFile);
                FileInfo file = new FileInfo(Path.Combine(directory.FullName, _fileCurrentFile.Name));

                if (!file.Directory.Exists)
                    continue;

                string pattern = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_*.bak";

                DateTime filterDate = DateTime.Now.AddDays(-2);

                IEnumerable<FileInfo> files = file.Directory.GetFiles()
                    .Where(p => Utils.MatchPattern(p.Name, pattern))
                    .Where(p => p.LastAccessTime < filterDate);

                foreach (FileInfo fileInfo in files)
                    fileInfo.Delete();
            }
        }

        private void DeleteOlderbackupFilesKeepOnePerDay(int days)
        {
            foreach (DirectoryInfo directory in BackupLocations())
            {
                FileInfo _fileCurrentFile = new FileInfo(CurrentFile);
                FileInfo file = new FileInfo(Path.Combine(directory.FullName, _fileCurrentFile.Name));

                if (!file.Directory.Exists)
                    continue;

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