using System;

namespace CodeLibrary.Core.DevToys
{
    public class StopWatch
    {
        private DateTime _Start = new DateTime();

        public StopWatch()
        {
        }

        public TimeSpan Duration { get; private set; } = new TimeSpan();

        private TimeSpan Elapsed => (DateTime.Now - _Start);

        public void Start()
        {
            Duration = new TimeSpan();
            _Start = DateTime.Now;
        }

        public void Stop() => Duration = Elapsed;

        public override string ToString() => Duration.ToString();
    }
}