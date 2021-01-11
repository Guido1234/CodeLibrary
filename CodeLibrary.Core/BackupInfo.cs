using System;
using System.Text;

namespace CodeLibrary.Core
{
    public class BackupInfo
    { 
        public DateTime DateTime { get; set; }

        public string Day
        {
            get
            {
                if (DateTime.Now.Year == DateTime.Year && DateTime.Now.Month == DateTime.Month && DateTime.Now.Day == DateTime.Day)
                {
                    return "Today";
                }
                if (DateTime.Now.Year == DateTime.Year && DateTime.Now.Month == DateTime.Month && DateTime.Now.Day - 1 == DateTime.Day)
                {
                    return "Yesterday";
                }

                DateTime _weekstart = Utils.GetWeekStartDate(Utils.GetWeekNumber(DateTime.Now), DateTime.Now.Year);
                DateTime _weekend = _weekstart.AddDays(7);

                if (DateTime >= _weekstart && DateTime <= _weekend)
                {
                    return "Last Eeek";
                }

                DateTime _monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime _monthEnd = _monthStart.AddDays(31);

                if (DateTime >= _monthStart && DateTime <= _monthEnd)
                {
                    return "Last Month";
                }

                return "Older";
            }
        }

        public string FileName { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return $"{Age(DateTime)} ago";
        }

        private string Age(DateTime datetime)
        {
            StringBuilder _sb = new StringBuilder();
            TimeSpan old = DateTime.Now - datetime;
            if (old.Days > 0)
            {
                _sb.Append($" {old.Days} day(s),");
            }
            if (old.Hours > 0)
            {
                _sb.Append($" {old.Hours} hour(s),");
            }
            if (old.Minutes > 0)
            {
                _sb.Append($" {old.Minutes} minute(s),");
            }
            if (old.Seconds > 0)
            {
                _sb.Append($" {old.Seconds} seconds(s)");
            }
            return _sb.ToString();
        }
    }
}