using System.IO;

using Cornucopia.DataStructures.Persistent;

using Jetbrains.Annotations.Extractor;

using NUnit.Framework;

namespace Cornucopia.DataStructures
{
    [TestFixture]
    public class GenerateAnnotationsTests
    {
        [Test]
        public void Generate()
        {
            var assembly = typeof(LinkedList<int>).Assembly;
            var annotationsFile = Path.Combine(Path.GetDirectoryName(assembly.Location)!, $"{assembly.GetName().Name}.ExternalAnnotations.xml");
            Program.GenerateExternalAnnotations(assembly, annotationsFile);
        }
    }
}