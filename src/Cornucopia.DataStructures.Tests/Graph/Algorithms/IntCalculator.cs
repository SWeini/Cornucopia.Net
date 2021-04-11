namespace Cornucopia.DataStructures.Graph.Algorithms
{
    internal class IntCalculator : IDistanceCalculator<int>, ICapacityCalculator<int>
    {
        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }

        public int Zero => 0;

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Negate(int a)
        {
            return -a;
        }
    }
}