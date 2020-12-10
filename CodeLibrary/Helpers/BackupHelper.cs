using CodeLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeLibrary
{
    public class BackupHelper
    {
        public BackupHelper(string currentFile)
        {
            CurrentFile = currentFile;
        }

        public string CurrentFile { get; set; }

        public void Backup()
        {
            if (string.IsNullOrEmpty(CurrentFile))
                return;

            if (!File.Exists(CurrentFile))
                return;

            FileInfo file = new FileInfo(CurrentFile);
            string newName = $"{file.Name.Replace($".{file.Extension}", string.Empty)}_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.bak";
            FileInfo bakfile = new FileInfo(Path.Combine(file.Directory.FullName, newName));
            if (!bakfile.Exists)
                File.Move(file.FullName, bakfile.FullName);

            DeleteOlderbackupFiles(-21); // Delete everything.
            DeleteOlderbackupFilesKeepOnePerDay(-2); // keep latest per day.
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
    }
}