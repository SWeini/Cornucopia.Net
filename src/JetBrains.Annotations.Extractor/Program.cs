using System.Reflection;
using System.Xml;

namespace JetBrains.Annotations.Extractor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assembly = Assembly.LoadFrom(args[0]);
            GenerateExternalAnnotations(assembly, args[1]);
        }

        public static void GenerateExternalAnnotations(Assembly assembly, string annotationsFile)
        {
            var xmlDoc = new XmlDocument();
            var rootElement = xmlDoc.CreateElement("assembly");
            rootElement.SetAttribute("name", assembly.GetName().Name);
            new AnnotationCreator(rootElement).ProcessAssembly(assembly);
            xmlDoc.AppendChild(rootElement);
            xmlDoc.Save(annotationsFile);
        }
    }
}
