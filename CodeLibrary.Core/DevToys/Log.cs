using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CodeLibrary.Core.DevToys
{
    /// <summary>
    /// Full featured Logger in just over 100 lines of code.
    /// </summary>
    [Flags]
    public enum LogLevel { Verbose = 1, Info = 2, Warning = 4, Exception = 8, StackTrace = 16 }

    public class BaseLogConfig
    {
        public virtual string LogDirectory { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Log");

        public virtual string FileName { get; set; } = "log_{0:yyyyMMdd}.log";

        public virtual TimeSpan FileExpiration { get; set; } = TimeSpan.FromDays(20);

        public virtual LogLevel Levels { get; set; } = LogLevel.Verbose | LogLevel.Info | LogLevel.Warning | LogLevel.Exception | LogLevel.StackTrace;

        public virtual char SeparatorLineChar { get; set; } = '-';
    }

    public sealed class Log : Log<BaseLogConfig>
    {
        private Log()
        { }
    }

    public class Log<TCONFIG> : IDisposable where TCONFIG : BaseLogConfig, new()
    {
        private StreamWriter _Writer = null;

        private static Log<TCONFIG> _Instance = null;

        private string _Path = string.Empty;

        private readonly BaseLogConfig _Settings = new TCONFIG();

        public static BaseLogConfig Settings { get; } = Instance._Settings;

        protected Log() => AppDomain.CurrentDomain.ProcessExit += (sender, e) => Dispose();

        private static Log<TCONFIG> Instance => _Instance ?? (_Instance = new Log<TCONFIG>());

        public static void Verbose(string message, object sender = null, object poco = null, DateTime? datetime = null, bool separator = false) => Instance.Write(LogLevel.Verbose, message, sender, poco, datetime ?? DateTime.Now, false, separator);

        public static void Info(string message, object sender = null, object poco = null, DateTime? datetime = null, bool separator = false) => Instance.Write(LogLevel.Info, message, sender, poco, datetime ?? DateTime.Now, false, separator);

        public static void Warning(string message, object sender = null, object poco = null, DateTime? datetime = null, bool separator = false) => Instance.Write(LogLevel.Warning, message, sender, poco, datetime ?? DateTime.Now, false, separator);

        public static void Exception(Exception exception, object sender = null, object poco = null, DateTime? datetime = null, bool separator = false) => Instance.Write(LogLevel.Exception, $"{exception?.Message}\r\n{exception?.StackTrace}", sender, poco, datetime ?? DateTime.Now, false, separator);

        public static void StackTrace(string message, object sender = null, object poco = null, DateTime? datetime = null, bool separator = false) => Instance.Write(LogLevel.StackTrace, message, sender, poco, datetime ?? DateTime.Now, true, separator);

        private static bool IsSimpleType(Type t) => (t = Nullable.GetUnderlyingType(t) ?? t).IsPrimitive || t.IsEnum || t.Equals(typeof(string)) || t.Equals(typeof(decimal));

        private static string Escape(string s) => s.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\"", "\\\"");

        public void Dispose()
        {
            if (_Writer != null)
                _Writer.Close();
        }

        private void Write(LogLevel level, string message, object sender, object poco, DateTime datetime, bool trace = false, bool separator = false)
        {
            if (!_Settings.Levels.HasFlag(level))
                return;

            Initialize();
            _Writer.WriteLine($"{datetime.ToString("yyyy-MM-dd HH:mm:ss")} {level,-10} {(sender != null ? $"{sender.GetType().FullName} - " : "")}{message}");
            if (poco != null)
            {
                _Writer.WriteLine($"{poco.GetType().FullName}\r\n{{");
                foreach (var p in poco.GetType().GetProperties().Where(p => IsSimpleType(p.PropertyType) && p.CanRead))
                    _Writer.WriteLine($"\t{p.Name} = {((p.GetValue(poco) == null) ? "null" : (p.PropertyType == typeof(string)) ? $"\"{ Escape(p.GetValue(poco).ToString()) }\"" : Convert.ToString(p.GetValue(poco), CultureInfo.InvariantCulture))},");

                _Writer.WriteLine("}");
            }
            if (trace)
            {
                foreach (var s in new StackTrace(0, true).GetFrames().Skip(1).Reverse())
                    _Writer.WriteLine($"{s.GetFileLineNumber()} {s.GetMethod().DeclaringType.FullName}.{s.GetMethod().Name}");
            }
            if (separator)
                _Writer.WriteLine(new string(_Settings.SeparatorLineChar, 100));

            _Writer.Flush();
        }

        private void Initialize()
        {
            if (_Path.ToLower().Equals(Path.Combine(_Settings.LogDirectory, string.Format(_Settings.FileName, DateTime.Now)).ToLower()))
                return; // Path not changed.

            Dispose();
            Directory.CreateDirectory(_Settings.LogDirectory);
            _Writer = File.AppendText(_Path = Path.Combine(_Settings.LogDirectory, string.Format(_Settings.FileName, DateTime.Now)));
            try
            {
                foreach (var f in new DirectoryInfo(_Settings.LogDirectory).GetFiles().Where(p => p.CreationTime < DateTime.Now.Subtract(_Settings.FileExpiration)))
                    f.Delete();
            }
            catch (Exception) { }
        }
    }
    
}
