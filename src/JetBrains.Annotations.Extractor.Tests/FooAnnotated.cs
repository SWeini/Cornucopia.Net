using System.Collections.Generic;
using System.Linq;

namespace JetBrains.Annotations.Extractor.Tests
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class FooAnnotated
    {
        [Pure]
        public int Run([InstantHandle] IEnumerable<int> enumerable)
        {
            return enumerable.Max();
        }

        [StringFormatMethod("format")]
        public string Format(string format, object?[] args)
        {
            return string.Format(format, args);
        }

        [AssertionMethod]
        public void AssertTrue([AssertionCondition(AssertionConditionType.IS_TRUE)] bool condition)
        {
        }
    }
}