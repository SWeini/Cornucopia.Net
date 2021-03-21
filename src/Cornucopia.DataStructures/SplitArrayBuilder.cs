namespace Cornucopia.DataStructures
{
    internal struct SplitArrayBuilder<T>
    {
        private DynamicArray<T> _values;
        private DynamicArray<int> _firsts;

        public SplitArrayBuilder(int capacity, int numParts)
        {
            this._values = new DynamicArray<T>(capacity);
            this._firsts = new DynamicArray<int>(numParts + 1);
            this._firsts.AddLast(0);
        }

        public void AddValue(T value)
        {
            this._values.AddLast(value);
        }

        public void EndPart()
        {
            this._firsts.AddLast(this._values.Length);
        }

        public SplitArray<T> Build()
        {
            return new(this._values.AsSpan().ToArray(), this._firsts.AsSpan().ToArray());
        }
    }
}
