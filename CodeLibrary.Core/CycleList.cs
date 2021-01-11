using System.Collections.Generic;

namespace CodeLibrary.Core
{
    public class CycleList<T> : List<T>
    {
        private int _index = 0;

        public int Position => _index;

        public T Current()
        {
            return this[_index];
        }

        public T Next()
        {
            if (_index < Count - 1)
            {
                _index++;
                return this[_index];
            }
            _index = 0;
            return this[_index];
        }

        public T Previous()
        {
            if (_index > 0)
            {
                _index--;
                return this[_index];
            }
            _index = Count - 1;
            return this[_index];
        }
    }
}