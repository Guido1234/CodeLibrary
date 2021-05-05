using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CodeLibrary.Core.DevToys
{


    /// <summary>
    /// Map identical properties from object TSOURCE to new object TTARGET. TSOURCE may or may not be same type as TTARGET for exact cloning.
    /// </summary>
    public sealed class PocoMapper<TSOURCE, TTARGET>
        where TTARGET : class, new()
        where TSOURCE : class, new()
    {
        private Dictionary<string, PropertyInfo> _trgproperties;
        private PropertyInfo[] _srcproperties;
        private Action<TSOURCE, TTARGET> _custommapper;
        private IFormatProvider _format = null; // for implicit mapping to target string.

        private void Init(bool sourceBrowsable, bool targetBrowseable)
        {
            TTARGET trg = new TTARGET();
            TSOURCE src = new TSOURCE();

            if (!targetBrowseable)
                _trgproperties = trg.GetType().GetProperties().ToDictionary(p => p.Name);
            else
                _trgproperties = trg.GetType()
                    .GetProperties()
                    .Where(p => p.GetCustomAttribute(typeof(BrowsableAttribute)) != null && ((BrowsableAttribute)p.GetCustomAttribute(typeof(BrowsableAttribute))).Browsable == true)
                    .ToDictionary(p => p.Name);


            if (!sourceBrowsable)
                _srcproperties = src.GetType().GetProperties();
            else
                _srcproperties = src.GetType()
                    .GetProperties()
                    .Where(p => p.GetCustomAttribute(typeof(BrowsableAttribute)) != null && ((BrowsableAttribute)p.GetCustomAttribute(typeof(BrowsableAttribute))).Browsable == true)
                    .ToArray();
        }

        public PocoMapper(bool sourceBrowsable, bool targetBrowseable) => Init(sourceBrowsable, targetBrowseable);

        /// <param name="format">Implicit conversion only possible when format provider supplied</param>
        public PocoMapper(IFormatProvider format, bool sourceBrowsable, bool targetBrowseable) : this(sourceBrowsable, targetBrowseable) => _format = format;

        public PocoMapper(Action<TSOURCE, TTARGET> custommapper, bool sourceBrowsable, bool targetBrowseable)
        {
            _custommapper = custommapper;
            Init(sourceBrowsable, targetBrowseable);
        }

        /// <param name="format">Implicit conversion only possible when format provider supplied</param>
        public PocoMapper(IFormatProvider format, Action<TSOURCE, TTARGET> custommapper, bool sourceBrowsable, bool targetBrowseable) : this(custommapper, sourceBrowsable, targetBrowseable) => _format = format;

        public void Map(TSOURCE source, TTARGET target)
        {
            foreach (PropertyInfo srcprop in _srcproperties)
            {
                if (IsSimpleType(srcprop.PropertyType) && _trgproperties.ContainsKey(srcprop.Name))
                {
                    PropertyInfo trgprop = _trgproperties[srcprop.Name];
                    if (_format == null)
                    {
                        if (srcprop.PropertyType == trgprop.PropertyType && trgprop.CanWrite && srcprop.CanRead)
                            trgprop.SetValue(target, srcprop.GetValue(source, null), null);
                    }
                    else
                    {
                        if (srcprop.PropertyType == trgprop.PropertyType && trgprop.CanWrite && srcprop.CanRead)
                            trgprop.SetValue(target, srcprop.GetValue(source, null), null);

                        if ((srcprop.PropertyType != trgprop.PropertyType && trgprop.PropertyType == typeof(string)) && trgprop.CanWrite && srcprop.CanRead)
                            trgprop.SetValue(target, Convert(srcprop.GetValue(source, null), trgprop.PropertyType, _format), null);
                    }
                }
            }
        }

        public TTARGET Map(TSOURCE source)
        {
            TTARGET trg = new TTARGET();
            Map(source, trg);
            _custommapper?.Invoke(source, trg);

            return trg;
        }
        public IEnumerable<TTARGET> Map(IEnumerable<TSOURCE> source)
        {
            foreach (TSOURCE src in source)
                yield return Map(src);
        }

        private static object Convert(object value, Type target, IFormatProvider format)
        {
            target = Nullable.GetUnderlyingType(target) ?? target;
            return (target.IsEnum) ? Enum.Parse(target, value.ToString()) : (value == null) ? null : System.Convert.ChangeType(value, target, format);
        }

        private static bool IsSimpleType(Type type) => (type = Nullable.GetUnderlyingType(type) ?? type).IsPrimitive || type.IsEnum || type.Equals(typeof(string)) || type.Equals(typeof(decimal));
    }
}
