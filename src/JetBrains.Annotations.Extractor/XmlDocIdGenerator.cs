using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JetBrains.Annotations.Extractor
{
    public static class XmlDocIdGenerator
    {
        [Pure]
        public static string GetId(MethodBase method)
        {
            var result = new StringBuilder();
            result.Append("M:");
            PrintType(method.DeclaringType!, result);
            result.Append('.');
            result.Append(method.Name.Replace('.', '#'));
            if (method.IsGenericMethodDefinition)
            {
                result.Append("``");
                result.Append(method.GetGenericArguments().Length);
            }
            var parameters = method.GetParameters();
            if (parameters.Any())
            {
                result.Append('(');
                PrintType(parameters[0].ParameterType, result);
                for (var i = 1; i < parameters.Length; i++)
                {
                    result.Append(',');
                    PrintType(parameters[i].ParameterType, result);
                }
                result.Append(')');
            }

            if (method.IsSpecialName && (method.Name == "op_Implicit" || method.Name == "op_Explicit"))
            {
                result.Append('~');
                PrintType(((MethodInfo) method).ReturnType, result);
            }

            return result.ToString();
        }

        private static void PrintType(Type type, StringBuilder builder)
        {
            var numProcessedGenericArguments = 0;
            Print(type);

            void Print(Type t)
            {
                if (t.IsPointer)
                {
                    Print(t.GetElementType()!);
                    builder.Append('*');
                    return;
                }

                if (t.IsByRef)
                {
                    Print(t.GetElementType()!);
                    builder.Append('@');
                    return;
                }

                if (t.IsSZArray)
                {
                    Print(t.GetElementType()!);
                    builder.Append("[]");
                    return;
                }

                if (t.IsArray)
                {
                    Print(t.GetElementType()!);
                    builder.Append('[');
                    builder.Append(',', t.GetArrayRank() - 1);
                    builder.Append(']');
                    return;
                }

                if (t.IsGenericTypeParameter)
                {
                    builder.Append('`');
                    builder.Append(t.GenericParameterPosition);
                    return;
                }

                if (t.IsGenericMethodParameter)
                {
                    builder.Append("``");
                    builder.Append(t.GenericParameterPosition);
                    return;
                }

                if (t.IsNested)
                {
                    Print(t.DeclaringType!);
                    builder.Append('.');
                }
                else
                {
                    var ns = t.Namespace;
                    if (!string.IsNullOrEmpty(ns))
                    {
                        builder.Append(ns);
                        builder.Append('.');
                    }
                }

                if (t.IsGenericType && !type.IsGenericTypeDefinition)
                {
                    var genericArguments = t.GetGenericArguments();
                    if (genericArguments.Length > numProcessedGenericArguments)
                    {
                        builder.Append(t.Name.Split(new[] { '`' }, 2)[0]);
                        builder.Append('{');
                        Print(genericArguments[numProcessedGenericArguments]);
                        for (var i = numProcessedGenericArguments + 1; i < genericArguments.Length; i++)
                        {
                            builder.Append(',');
                            Print(genericArguments[i]);
                        }

                        numProcessedGenericArguments = genericArguments.Length;
                        builder.Append('}');
                    }
                    else
                    {
                        builder.Append(t.Name);
                    }
                }
                else
                {
                    builder.Append(t.Name);
                }
            }
        }
    }
}