using System;

namespace CodeLibrary.Core
{
    public class Idle
    {
        private DateTime _LastRefresh = DateTime.Now;
        private TimeSpan _Treshhold = new TimeSpan(0, 2, 0);

        public Idle(TimeSpan treshhold)
        {
            _Treshhold = treshhold;
        }

        private bool IsIdle => DateTime.Now - _LastRefresh > _Treshhold;

        public void Refresh()
        {
            _LastRefresh = DateTime.Now;
        }

        public static implicit operator bool(Idle value) => value.IsIdle;

        public static bool operator !=(Idle a, Idle b) => a.IsIdle != b.IsIdle;

        public static bool operator !=(Idle a, bool b) => a.IsIdle != b;

        public static bool operator !=(bool a, Idle b) => b.IsIdle != a;

        public static bool operator ==(Idle a, Idle b) => a.IsIdle == b.IsIdle;

        public static bool operator ==(Idle a, bool b) => a.IsIdle == b;

        public static bool operator ==(bool a, Idle b) => b.IsIdle == a;

    }
}