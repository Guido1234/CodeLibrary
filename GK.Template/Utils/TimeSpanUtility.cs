using System;
using System.Linq;

namespace GK.Template
{
    /// <summary>
    /// Utility class for the timespan
    /// </summary>
    [Obsolete("Consider using TimeSpan.ToString(format) in framework 4", false)]
    public static class TimeSpanUtility
    {
        /// <summary>
        /// <para>T : Total milliseconds</para>
        /// <para>t : tenth</para>
        /// <para>tt : hundreds</para>
        /// <para>ttt : miliseconds</para>
        /// <para>I : Ticks</para>
        /// <para>S : Total seconds</para>
        /// <para>s : seconds</para>
        /// <para>ss : seconds with leading zero</para>
        /// <para>M : Total minutes</para>
        /// <para>m : minutes</para>
        /// <para>mm : minutes with leading zero</para>
        /// <para>H : total hours</para>
        /// <para>h : hours</para>
        /// <para>hh : hours with leading zero</para>
        /// <para>d : days</para>
        /// <para>dd : days with leading zero</para>
        /// <para>dd : days with leading zero</para>
        /// <para>x negative sign</para>
        /// <para>xx positive/negative sign</para>
        /// </summary>
        /// <param name="timespan"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [Obsolete("Consider using TimeSpan.ToString(format) in framework 4", false)]
        public static string Format(TimeSpan timespan, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "hh:mm:ss,ttt";
            }
            string[] fmt = Utils.SplitByRepetative(format);
            string num = string.Empty;
            int size = 0;
            for (int ii = 0; ii < fmt.Length; ii++)
            {
                if (fmt[ii].Equals("x", StringComparison.OrdinalIgnoreCase))
                {
                    if (timespan.Ticks < 0)
                    {
                        fmt[ii] = "-";
                    }
                    else
                    {
                        fmt[ii] = string.Empty;
                    }
                }
                if (fmt[ii].Equals("xx", StringComparison.OrdinalIgnoreCase))
                {
                    if (timespan.Ticks < 0)
                    {
                        fmt[ii] = "-";
                    }
                    else
                    {
                        fmt[ii] = "+";
                    }
                }
                if (Utils.IsAlpha(fmt[ii]))
                {
                    size = fmt[ii].Length;
                    if (fmt[ii].Contains('t'))
                    {
                        // also support tt as hundreds or ttt as thousands or t as tenth
                        num = Utils.Truncate(Utils.Positive(timespan.Milliseconds).ToString(), size);
                        fmt[ii] = num.PadLeft(size, '0'); // StringUtility.LeadingZeros(num, size);
                    }
                    if (fmt[ii].Contains('T'))
                    {
                        fmt[ii] = Utils.Positive(timespan.TotalMilliseconds).ToString();
                    }
                    if (fmt[ii].Contains('I'))
                    {
                        fmt[ii] = Utils.Positive(timespan.Ticks).ToString();
                    }
                    if (fmt[ii].Contains('s'))
                    {
                        num = Utils.Positive(timespan.Seconds).ToString();
                        fmt[ii] = num.PadLeft(size, '0');
                    }
                    if (fmt[ii].Contains('S'))
                    {
                        fmt[ii] = Utils.Positive(timespan.TotalSeconds).ToString();
                    }
                    if (fmt[ii].Contains('m'))
                    {
                        num = Utils.Positive(timespan.Minutes).ToString();
                        fmt[ii] = num.PadLeft(size, '0');
                    }
                    if (fmt[ii].Contains('M'))
                    {
                        fmt[ii] = Utils.Positive(timespan.TotalMinutes).ToString();
                    }
                    if (fmt[ii].Contains('h'))
                    {
                        num = Utils.Positive(timespan.Hours).ToString();
                        fmt[ii] = num.PadLeft(size, '0');
                    }
                    if (fmt[ii].Contains('H'))
                    {
                        fmt[ii] = Utils.Positive(timespan.TotalHours).ToString();
                    }
                    if (fmt[ii].Contains('d'))
                    {
                        num = Utils.Positive(timespan.Days).ToString();
                        fmt[ii] = num.PadLeft(size, '0');
                    }
                    if (fmt[ii].Contains('D'))
                    {
                        fmt[ii] = Utils.Positive(timespan.TotalDays).ToString();
                    }
                }
            }
            return string.Join("", fmt);
        }

        /// <summary>
        /// <para>T : Total milliseconds</para>
        /// <para>t : tenth</para>
        /// <para>tt : hundreds</para>
        /// <para>ttt : miliseconds</para>
        /// <para>I : Ticks</para>
        /// <para>S : Total seconds</para>
        /// <para>s : seconds</para>
        /// <para>ss : seconds with leading zero</para>
        /// <para>M : Total minutes</para>
        /// <para>m : minutes</para>
        /// <para>mm : minutes with leading zero</para>
        /// <para>H : total hours</para>
        /// <para>h : hours</para>
        /// <para>hh : hours with leading zero</para>
        /// <para>d : days</para>
        /// <para>dd : days with leading zero</para>
        /// <para>dd : days with leading zero</para>
        /// <para>x negative sign</para>
        /// <para>xx positive/negative sign</para>
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [Obsolete("Consider using TimeSpan.ToString(format) in framework 4", false)]
        public static string Format(long ticks, string format)
        {
            TimeSpan ts = new TimeSpan(ticks);
            return Format(ts, format);
        }

        public static string GetDatePartByFormat(DateTime datetime, string format)
        {
            string temp = string.Format("{{0:{0}}}", format);
            return string.Format(temp, datetime);
        }

        /// <summary>
        /// <para>t : tenth</para>
        /// <para>tt : hundreds</para>
        /// <para>ttt : miliseconds</para>
        /// <para>s : seconds</para>
        /// <para>ss : seconds with leading zero</para>
        /// <para>m : minutes</para>
        /// <para>mm : minutes with leading zero</para>
        /// <para>h : hours</para>
        /// <para>hh : hours with leading zero</para>
        /// <para>d : days</para>
        /// <para>dd : days with leading zero</para>
        /// <para>x negative sign</para>
        /// </summary>
        /// <param name="timespan"></param>
        /// <param name="format"></param>
        [Obsolete("Consider using TimeSpan.ToString(format) in framework 4", false)]
        public static string GetTimePartByFormat(TimeSpan timespan, string format)
        {
            string num = string.Empty;
            int size = format.Length;
            if (Utils.IsAlpha(format))
            {
                if (format.Contains('x'))
                {
                    if (size == 1)
                    {
                        if (timespan.Ticks < 0)
                        {
                            return "-";
                        }
                    }
                    else
                    {
                        if (timespan.Ticks < 0)
                        {
                            return "-";
                        }
                        else
                        {
                            return "+";
                        }
                    }
                }
                if (format.Contains('t') || format.Contains('T'))
                {
                    // also support tt as hundreds or ttt as thousands or t as tenth
                    num = Utils.Truncate(Utils.Positive(timespan.Milliseconds).ToString(), size);
                    return num.PadLeft(size);
                }
                if (format.Contains('s') || format.Contains('S'))
                {
                    num = Utils.Positive(timespan.Seconds).ToString();
                    return num.PadLeft(size, '0');
                }
                if (format.Contains('m') || format.Contains('M'))
                {
                    num = Utils.Positive(timespan.Minutes).ToString();
                    return num.PadLeft(size, '0');
                }
                if (format.Contains('h') || format.Contains('H'))
                {
                    num = Utils.Positive(timespan.Hours).ToString();
                    return num.PadLeft(size, '0');
                }
                if (format.Contains('d') || format.Contains('D'))
                {
                    num = Utils.Positive(timespan.Days).ToString();
                    return num.PadLeft(size, '0');
                }
            }
            return string.Empty;
        }
    }
}