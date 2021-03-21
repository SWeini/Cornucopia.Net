using System.Reflection;

namespace JetBrains.Annotations.Extractor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var creator = new AnnotationCreator();
            creator.ProcessAssembly(Assembly.LoadFrom(args[0]));
            creator.Document.Save(args[1]);
        }
    }
}
