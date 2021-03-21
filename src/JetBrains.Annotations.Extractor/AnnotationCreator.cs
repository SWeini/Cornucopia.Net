using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace JetBrains.Annotations.Extractor
{
    public class AnnotationCreator
    {
        public XmlDocument Document { get; }
        private readonly XmlElement _rootElement;

        public AnnotationCreator()
        {
            this.Document = new();
            this._rootElement = this.Document.CreateElement("assembly");
        }

        public void ProcessAssembly(Assembly assembly)
        {
            this._rootElement.SetAttribute("name", assembly.GetName().Name);
            foreach (var type in assembly.GetExportedTypes())
            {
                this.ProcessType(type);
            }

            this.Document.AppendChild(this._rootElement);
        }

        private void ProcessType(Type type)
        {
            foreach (var constructor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                this.ProcessMethod(constructor);
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
            {
                this.ProcessMethod(method);
            }
        }

        private void ProcessMethod(MethodBase method)
        {
            var elements = new List<XmlElement>();
            var annotations = GetAnnotationAttributes(method).ToArray();
            if (annotations.Any())
            {
                foreach (var annotation in annotations)
                {
                    elements.Add(this.CreateElementForAnnotation(annotation));
                }
            }

            foreach (var parameter in method.GetParameters())
            {
                var element = this.CreateParameterElement(parameter);
                if (element != null)
                {
                    elements.Add(element);
                }
            }

            if (elements.Any())
            {
                var id = XmlDocIdGenerator.GetId(method);
                var element = this._rootElement.OwnerDocument!.CreateElement("member");
                element.SetAttribute("name", id);
                elements.ForEach(x => element.AppendChild(x));
                this._rootElement.AppendChild(element);
            }
        }

        private XmlElement? CreateParameterElement(ParameterInfo parameter)
        {
            var annotations = GetAnnotationAttributes(parameter).ToArray();
            if (!annotations.Any())
            {
                return null;
            }

            var result = this.Document.CreateElement("parameter");
            result.SetAttribute("name", parameter.Name);
            foreach (var annotation in annotations)
            {
                result.AppendChild(this.CreateElementForAnnotation(annotation));
            }

            return result;
        }

        private XmlElement CreateElementForAnnotation(CustomAttributeData annotation)
        {
            var result = this.Document.CreateElement("attribute");
            result.SetAttribute("ctor", XmlDocIdGenerator.GetId(annotation.Constructor));
            foreach (var argument in annotation.ConstructorArguments)
            {
                var argElement = this.Document.CreateElement("argument");
                if (argument.Value is string str)
                {
                    argElement.InnerText = str;
                }
                else
                {
                    argElement.InnerText = argument.Value?.ToString() ?? "";
                }

                result.AppendChild(argElement);
            }

            return result;
        }

        private static IEnumerable<CustomAttributeData> GetAnnotationAttributes(ParameterInfo parameter)
        {
            foreach (var attribute in parameter.GetCustomAttributesData())
            {
                if (attribute.AttributeType.Namespace == "JetBrains.Annotations")
                {
                    yield return attribute;
                }
            }
        }

        private static IEnumerable<CustomAttributeData> GetAnnotationAttributes(MemberInfo member)
        {
            foreach (var attribute in member.GetCustomAttributesData())
            {
                if (attribute.AttributeType.Namespace == "JetBrains.Annotations")
                {
                    yield return attribute;
                }
            }
        }
    }
}