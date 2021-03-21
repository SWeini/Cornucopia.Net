using NUnit.Framework;

namespace JetBrains.Annotations.Extractor.Tests
{
    [TestFixture]
    public class XmlDocIdGeneratorTests
    {
        [Test]
        public void GetId_FooRun_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod(nameof(Foo.Run))!);
            Assert.That(id, Is.EqualTo("M:JetBrains.Annotations.Extractor.Tests.Foo.Run"));
        }

        [Test]
        public void GetId_FooRun2_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod(nameof(Foo.Run2Parameters))!);
            Assert.That(id, Does.EndWith(".Run2Parameters(System.Int32,System.Double)"));
        }

        [Test]
        public void GetId_FooGeneric_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod(nameof(Foo.RunGeneric))!);
            Assert.That(id, Does.EndWith(".RunGeneric``1(System.Collections.Generic.IEnumerable{``0})"));
        }

        [Test]
        public void GetId_ImplicitOperator_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod("op_Implicit")!);
            Assert.That(id, Does.EndWith("~System.Int32"));
        }

        [Test]
        public void GetId_NestedGeneric_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo.Bar<,>.Baz).GetMethod(nameof(Foo.Bar<object, object>.Baz.Run))!);
            Assert.That(id, Does.EndWith(".Foo.Bar`2.Baz.Run(`0,`1)"));
        }

        [Test]
        public void GetId_SameGenericParameter_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo.Bar<,>.Baz).GetMethod(nameof(Foo.Bar<object, object>.Baz.RunOther))!);
            Assert.That(id, Does.EndWith(".RunOther(JetBrains.Annotations.Extractor.Tests.Foo.Bar{`0,`1}.Baz)"));
        }

        [Test]
        public void GetId_OtherGenericParameter_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo.Bar<,>.Baz).GetMethod(nameof(Foo.Bar<object, object>.Baz.RunDifferent))!);
            Assert.That(id, Does.EndWith(".RunDifferent(JetBrains.Annotations.Extractor.Tests.Foo.Bar{System.Int32,System.Double}.Baz)"));
        }

        [Test]
        public void GetId_PointerParameter_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod(nameof(Foo.RunPointer))!);
            Assert.That(id, Does.EndWith(".RunPointer(System.Int32*)"));
        }

        [Test]
        public void GetId_VoidPointerParameter_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod(nameof(Foo.RunVoidPointer))!);
            Assert.That(id, Does.EndWith(".RunVoidPointer(System.Void*)"));
        }

        [TestCase(nameof(Foo.RunIn))]
        [TestCase(nameof(Foo.RunOut))]
        [TestCase(nameof(Foo.RunRef))]
        public void GetId_RefParameter_IsCorrect(string name)
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod(name)!);
            Assert.That(id, Does.EndWith("(System.Int32@)"));
        }

        [Test]
        public void GetId_ArrayParameter_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod(nameof(Foo.RunArray))!);
            Assert.That(id, Does.EndWith(".RunArray(System.Int32[])"));
        }

        [Test]
        public void GetId_MultiArrayParameter_IsCorrect()
        {
            var id = XmlDocIdGenerator.GetId(typeof(Foo).GetMethod(nameof(Foo.RunMultiArray))!);
            Assert.That(id, Does.EndWith(".RunMultiArray(System.Int32[0:,0:,0:])"));
        }
    }
}