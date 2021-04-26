using System;

namespace CodeLibrary.Core
{

    public class StopWatch
    {
        private DateTime _Start = new DateTime();

        public StopWatch() { }

        public TimeSpan Duration { get; private set; } = new TimeSpan();

        public void Start()
        {
            Duration = new TimeSpan();
            _Start = DateTime.Now;
        }

        public void Stop() => Duration = Elapsed;

        public override string ToString() => Duration.ToString();

        private TimeSpan Elapsed => (DateTime.Now - _Start);
    }
    
}
