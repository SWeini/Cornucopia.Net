extern alias CcBitOperations;

using NUnit.Framework;

using static CcBitOperations::System.Numerics.BitOperations;

namespace Cornucopia.BitOperations.Tests
{
    [TestFixture]
    public class BitOperationsTests
    {
        [Test]
        public void Log2_AllBorders_Correct()
        {
            for (var i = 1; i < 32; i++)
            {
                var pow2 = 1u << i;
                Assert.That(Log2(pow2 + 1), Is.EqualTo(i));
                Assert.That(Log2(pow2), Is.EqualTo(i));
                Assert.That(Log2(pow2 - 1), Is.EqualTo(i - 1));
            }
        }

        [Test]
        public void Log2_Zero_ReturnsZero()
        {
            Assert.That(Log2(0), Is.Zero);
        }

        [Test]
        public void Log2_Uint32MaxValue_Returns31()
        {
            Assert.That(Log2(uint.MaxValue), Is.EqualTo(31));
        }

        [Test]
        public void Log2_Uint64MaxValue_Returns63()
        {
            Assert.That(Log2(ulong.MaxValue), Is.EqualTo(63));
        }

        [Test]
        public void Log2_Zero64_ReturnsZero()
        {
            Assert.That(Log2(0UL), Is.Zero);
        }

        [Test]
        public void Log2_One64_ReturnsZero()
        {
            Assert.That(Log2(1UL), Is.Zero);
        }
    }
}