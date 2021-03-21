using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jetbrains.Annotations.Extractor
{
    public static class XmlDocIdGenerator
    {
        public static string GetId(MethodBase method)
        {
            var result = new StringBuilder();
            result.Append("M:");
            Build(method.DeclaringType!, result);
            result.Append('.');
            result.Append(method.Name.Replace('.', '#'));
            if (method.IsGenericMethodDefinition)
            {
                result.Append("``");
                result.Append(method.GetGenericArguments().Length);
            }
            var genericArguments = BuildGenericTypes(method);
            var parameters = method.GetParameters();
            if (parameters.Any())
            {
                result.Append('(');
                Build(parameters[0].ParameterType, result, genericArguments);
                for (var i = 1; i < parameters.Length; i++)
                {
                    result.Append(',');
                    Build(parameters[i].ParameterType, result, genericArguments);
                }
                result.Append(')');
            }

            if (method.IsSpecialName && (method.Name == "op_Implicit" || method.Name == "op_Explicit"))
            {
                result.Append('~');
                Build(((MethodInfo) method).ReturnType, result, genericArguments);
            }

            return result.ToString();
        }

        private static Dictionary<Type, string> BuildGenericTypes(MethodBase method)
        {
            var result = new Dictionary<Type, string>();

            if (method.IsGenericMethodDefinition)
            {
                var genericArguments = method.GetGenericArguments();
                for (var i = 0; i < genericArguments.Length; i++)
                {
                    result.Add(genericArguments[i], $"``{i}");
                }
            }

            var type = method.DeclaringType!;
            if (type.IsGenericTypeDefinition)
            {
                var genericArguments = type.GetGenericArguments();
                for (var i = 0; i < genericArguments.Length; i++)
                {
                    result.Add(genericArguments[i], $"`{i}");
                }
            }
            return result;
        }

        private static void Build(Type type, StringBuilder str)
        {
            if (type.IsNested)
            {
                Build(type.DeclaringType!, str);
                str.Append('.');
            }
            else
            {
                if (!string.IsNullOrEmpty(type.Namespace))
                {
                    str.Append(type.Namespace);
                    str.Append('.');
                }
            }

            str.Append(type.Name);
        }

        private static void Build(Type type, StringBuilder str, Dictionary<Type, string> genericTypes)
        {
            if (type.IsPointer)
            {
                Build(type.GetElementType()!, str, genericTypes);
                str.Append('*');
                return;
            }

            if (type.IsByRef)
            {
                Build(type.GetElementType()!, str, genericTypes);
                str.Append('@');
                return;
            }

            if (type.IsSZArray)
            {
                Build(type.GetElementType()!, str, genericTypes);
                str.Append("[]");
                return;
            }

            if (type.IsArray)
            {
                Build(type.GetElementType()!, str, genericTypes);
                str.Append('[');
                str.Append(',', type.GetArrayRank() - 1);
                str.Append(']');
                return;
            }

            if (genericTypes.TryGetValue(type, out var genericType))
            {
                str.Append(genericType);
                return;
            }

            if (type.IsNested)
            {
                Build(type.DeclaringType!, str, genericTypes);
                str.Append('.');
            }
            else
            {
                if (!string.IsNullOrEmpty(type.Namespace))
                {
                    str.Append(type.Namespace);
                    str.Append('.');
                }
            }

            if (type.IsConstructedGenericType)
            {
                var name = type.Name.Split('`', 2)[0];
                str.Append(name);
                str.Append('{');
                var genericArguments = type.GenericTypeArguments;
                Build(genericArguments[0], str, genericTypes);
                for (var i = 1; i < genericArguments.Length; i++)
                {
                    str.Append(',');
                    Build(genericArguments[1], str, genericTypes);
                }
                str.Append('}');
            }
            else
            {
                str.Append(type.Name);
            }
        }
    }
}