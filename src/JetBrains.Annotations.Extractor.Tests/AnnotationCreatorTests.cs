using System.IO;

using NUnit.Framework;

namespace JetBrains.Annotations.Extractor.Tests
{
    [TestFixture]
    public class AnnotationCreatorTests
    {
        [Test]
        public void ProcessAssembly_ThisAssembly_Works()
        {
            var creator = new AnnotationCreator();
            creator.ProcessAssembly(this.GetType().Assembly);
            using (var writer = new StringWriter())
            {
                creator.Document.Save(writer);
                var str = writer.ToString();
                Assert.That(str, Is.EqualTo(ThisAssemblyAnnotations));
            }
        }

        private const string ThisAssemblyAnnotations = @"<?xml version=""1.0"" encoding=""utf-16""?>
<assembly name=""JetBrains.Annotations.Extractor.Tests"">
  <member name=""M:JetBrains.Annotations.Extractor.Tests.FooAnnotated.Run(System.Collections.Generic.IEnumerable{System.Int32})"">
    <attribute ctor=""M:JetBrains.Annotations.PureAttribute.#ctor"" />
    <parameter name=""enumerable"">
      <attribute ctor=""M:JetBrains.Annotations.InstantHandleAttribute.#ctor"" />
    </parameter>
  </member>
  <member name=""M:JetBrains.Annotations.Extractor.Tests.FooAnnotated.Format(System.String,System.Object[])"">
    <attribute ctor=""M:JetBrains.Annotations.StringFormatMethodAttribute.#ctor(System.String)"">
      <argument>format</argument>
    </attribute>
  </member>
  <member name=""M:JetBrains.Annotations.Extractor.Tests.FooAnnotated.AssertTrue(System.Boolean)"">
    <attribute ctor=""M:JetBrains.Annotations.AssertionMethodAttribute.#ctor"" />
    <parameter name=""condition"">
      <attribute ctor=""M:JetBrains.Annotations.AssertionConditionAttribute.#ctor(JetBrains.Annotations.AssertionConditionType)"">
        <argument>0</argument>
      </attribute>
    </parameter>
  </member>
</assembly>";
    }
}