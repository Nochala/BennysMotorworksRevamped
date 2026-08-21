using System;

namespace BennysMotorworksRevamped
{
    public sealed class RealTimer
    {
        public RealTimer(DateTime start)
        {
            Start = start;
        }

        public RealTimer()
        {
            Start = DateTime.Now;
        }

        public DateTime Start { get; private set; }

        public void Reset(DateTime? start = null)
        {
            Start = start ?? DateTime.Now;
        }

        public bool TotalSeconds(int span) => (DateTime.Now - Start).TotalSeconds > span;
        public bool TotalMilliseconds(int span) => (DateTime.Now - Start).TotalMilliseconds > span;
        public bool TotalMinutes(int span) => (DateTime.Now - Start).TotalMinutes > span;
        public bool TotalHours(int span) => (DateTime.Now - Start).TotalHours > span;

        public bool TotalSeconds(double span) => (DateTime.Now - Start).TotalSeconds > span;
        public bool TotalMilliseconds(double span) => (DateTime.Now - Start).TotalMilliseconds > span;
        public bool TotalMinutes(double span) => (DateTime.Now - Start).TotalMinutes > span;
        public bool TotalHours(double span) => (DateTime.Now - Start).TotalHours > span;
    }
}
