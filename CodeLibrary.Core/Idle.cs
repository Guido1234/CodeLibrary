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

        public bool IsIdle => DateTime.Now - _LastRefresh > _Treshhold;

        public void Refresh()
        {
            _LastRefresh = DateTime.Now;
        }
    }
}