namespace Cornucopia.DataStructures.Persistent
{
    public static class RandomAccessList
    {
        public static RandomAccessList<T> Create<T>(T item)
        {
            return RandomAccessList<T>.Empty.AddFirst(item);
        }

        public static RandomAccessList<T> Create<T>(params T[] items)
        {
            var result = RandomAccessList<T>.Empty;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                result = result.AddFirst(items[i]);
            }

            return result;
        }
    }
}