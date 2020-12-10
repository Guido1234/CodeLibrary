using System;

namespace CodeLibrary
{
    public class TimedBool
    {
        private TimeSpan _retention = new TimeSpan(0, 0, 0, 0, 300);
        private DateTime _trueTime = DateTime.Now;

        public TimedBool()
        {
        }

        public TimedBool(TimeSpan retention)
        {
            _retention = retention;
        }

        public bool Value
        {
            set
            {
                _trueTime = DateTime.Now;
            }
            get
            {
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