namespace Cornucopia.DataStructures.Utils
{
    internal static class GrowingCapacity
    {
        public const int DefaultCapacity = 4;

        public const int MaxArrayLength = 0x7fefffff;

        public static int Grow(int oldCapacity, int min)
        {
            var newCapacity = oldCapacity == 0 ? DefaultCapacity : oldCapacity * 2;
            if ((uint) newCapacity > MaxArrayLength)
            {
                newCapacity = MaxArrayLength;
            }

            if (newCapacity < min)
            {
                newCapacity = min;
            }

            return newCapacity;
        }
    }
}
