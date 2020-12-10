using System;

namespace DevToys
{
    public class VersionNumber : IComparable
    {
        private static VersionNumber zeroVersion;

        public VersionNumber(int major, int minor, int revision, int build)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
            Build = build;
        }

        public VersionNumber(string versionNumber)
        {
            if (string.IsNullOrWhiteSpace(versionNumber))
                return;

            string[] items = versionNumber.Split(new char[] { '.' });
            if (items.Length > 0)
            {
                if (IsNumeric(items[0]))
                    Major = Convert.ToInt32(items[0]);
            }
            if (items.Length > 1)
            {
                if (IsNumeric(items[1]))
                    Minor = Convert.ToInt32(items[1]);
            }
            if (items.Length > 2)
            {
                if (IsNumeric(items[2]))
                    Revision = Convert.ToInt32(items[2]);
            }
            if (items.Length > 3)
            {
                if (IsNumeric(items[3]))
                    Build = Convert.ToInt32(items[3]);
            }
        }

        public static VersionNumber ZeroVersion => zeroVersion ?? (zeroVersion = new VersionNumber(0, 0, 0, 0));
        public int Build { get; set; }
        public int Major { get; set; }

        public int Minor { get; set; }

        public int Revision { get; set; }

        private long CompareValue
        {
            get
            {
                long ma = (long)Major * 1000000000;
                long mi = (long)Minor * 10000000;
                long re = (long)Revision * 10000;
                return ma + mi + re + Build;
            }
        }

        public static bool operator <(VersionNumber v1, VersionNumber v2) => v1.CompareTo(v2) < 0;

        public static bool operator >(VersionNumber v1, VersionNumber v2) => v1.CompareTo(v2) > 0;

        public int CompareTo(object obj)
        {
            if (obj is string)
                return CompareTo(obj.ToString());

            if (obj is VersionNumber)
                return CompareTo((VersionNumber)obj);

            return 0;
        }

        public int CompareTo(string version) => CompareTo(new VersionNumber(version));

        public int CompareTo(VersionNumber other)
        {
            if (other == null)
                return 1;

            if (CompareValue == other.CompareValue)
            {
                if (CompareValue == other.CompareValue)
                    return 0;
                else if (CompareValue < other.CompareValue)
                    return -1;
                else
                    return 1;
            }
            else if (CompareValue < other.CompareValue)
                return -1;
            else
                return 1;
        }

        public override int GetHashCode() => ToString().GetHashCode();

        //public static bool operator ==(VersionNumber v1, VersionNumber v2) => v1.CompareTo(v2) == 0;
        public override string ToString() => $"{Major}.{Minor}.{Revision}.{Build}";

        private static bool IsNumeric(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;

            char[] chars = s.ToCharArray();
            for (int ii = 0; ii < chars.Length; ii++)
            {
                if (!char.IsDigit(chars[ii]))
                    return false;
            }
            return true;
        }
    }
}