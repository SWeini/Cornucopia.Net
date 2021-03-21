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
            PrintType(method.DeclaringType!, result, true);
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

        private static void PrintType(Type type, StringBuilder builder, bool isTypeDefinition = false)
        {
            var genericArguments = type.GetGenericArguments();
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
                    builder.Append("[0:");
                    for (var i = 1; i < t.GetArrayRank(); i++)
                    {
                        builder.Append(",0:");
                    }
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

                if (t.IsGenericType && !isTypeDefinition)
                {
                    var genericArgumentsLength = t.GetGenericArguments().Length;
                    if (genericArgumentsLength > numProcessedGenericArguments)
                    {
                        builder.Append(t.Name.Split(new[] { '`' }, 2)[0]);
                        builder.Append('{');
                        Print(genericArguments[numProcessedGenericArguments]);
                        for (var i = numProcessedGenericArguments + 1; i < genericArgumentsLength; i++)
                        {
                            builder.Append(',');
                            Print(genericArguments[i]);
                        }

                        numProcessedGenericArguments = genericArgumentsLength;
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