using System;
using System.Timers;
 
namespace CodeLibrary.Core
{
    public class TimedBool
    {
        private TimeSpan _retention = new TimeSpan(0, 0, 0, 0, 300);
        private DateTime _trueTime = DateTime.Now;

        public event EventHandler<EventArgs> Expired = delegate { };

        private Timer IntervalTimer = new Timer(300);

        public TimedBool()
        {
            IntervalTimer.Enabled = false;
            IntervalTimer.Interval = _retention.TotalMilliseconds;
            IntervalTimer.AutoReset = true;
            IntervalTimer.Elapsed += IntervalTimer_Elapsed;
        }

        private void IntervalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IntervalTimer.Enabled = false;
            Expired(this, new EventArgs());
        }

        public TimedBool(TimeSpan retention) : this()
        {
            _retention = retention;
        }

        public bool Value
        {
            set
            {
                IntervalTimer.Enabled = false;
                _trueTime = DateTime.Now;
                IntervalTimer.Enabled = true;
                
            }
            get
            {
                IntervalTimer.Enabled = false;
                return DateTime.Now - _trueTime < _retention;
            }
        }

        public static implicit operator bool(TimedBool value) => value.Value;

        public static bool operator !=(TimedBool a, TimedBool b) => a.Value != b.Value;

        public static bool operator !=(TimedBool a, bool b) => a.Value != b;

        public static bool operator !=(bool a, TimedBool b) => b.Value != a;

        public static bool operator ==(TimedBool a, TimedBool b) => a.Value == b.Value;

        public static bool operator ==(TimedBool a, bool b) => a.Value == b;

        public static bool operator ==(bool a, TimedBool b) => b.Value == a;
    }
}