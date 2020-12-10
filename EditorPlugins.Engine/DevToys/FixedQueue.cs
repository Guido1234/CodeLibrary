using System.Linq;

namespace DevToys
{
    public class FixedQueue<T>
    {
        private readonly T[] _items;
        private int _counter = 0;
        private int _position = 0;

        public FixedQueue(int fixedsize) => _items = new T[fixedsize];

        public bool Full => (_counter >= _items.Length);

        public T[] Items => _items;
        public int Size => (_counter < _items.Length) ? _counter : _items.Length;

        public void Add(T item)
        {
            if (Contains(item))
                return;

            if (_counter < _items.Length)
                _counter++;

            int _last = _items.Count() - 1;

            for (int ii = 0; ii < _last; ii++)
                _items[ii] = _items[ii + 1];

            _items[_items.Count() - 1] = item;
        }

        public bool Contains(T item) => _items.Contains(item);

        public T Next()
        {
            if (_counter == 0)
                return default;

            _position++;
            if (_position > _counter - 1)
                _position = 0;

            return _items[(_items.Length - 1) - _position];
        }

        public T Prev()
        {
            if (_counter == 0)
                return default;

            _position--;
            if (_position < 0)
                _position = _counter - 1;

            return _items[(_items.Length - 1) - _position];
        }
    }
}