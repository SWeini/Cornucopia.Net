using System.Collections.Generic;

namespace JetBrains.Annotations.Extractor.Tests
{
    public class Foo
    {
        public void Run()
        {
        }

        public void Run2Parameters(int a, double b)
        {
        }

        public void RunGeneric<T>(IEnumerable<T> enumerable)
        {
        }

        public static implicit operator int(Foo x)
        {
            return 0;
        }

        public unsafe void RunPointer(int* ptr)
        {
        }

        public unsafe void RunVoidPointer(void* ptr)
        {
        }

        public void RunIn(in int i)
        {
        }

        public void RunOut(out int i)
        {
            i = 0;
        }

        public void RunRef(ref int i)
        {
        }

        public void RunArray(int[] array)
        {
        }

        public void RunMultiArray(int[,,] array)
        {
        }

        public class Bar<T1, T2>
        {
            public class Baz
            {
                public void Run(T1 t1, T2 t2)
                {
                }

                public void RunOther(Baz baz)
                {
                }

                public void RunDifferent(Bar<int, double>.Baz baz)
                {
                }
            }
        }
    }
}