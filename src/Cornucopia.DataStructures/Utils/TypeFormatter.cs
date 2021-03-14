using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Cornucopia.DataStructures.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [ExcludeFromCodeCoverage]
    internal static class TypeFormatter
    {
        private static Dictionary<Type, string> _basicTypes = new Dictionary<Type, string>
        {
            { typeof(void), "void" },
            { typeof(object), "object" },
            { typeof(string), "string" },
            { typeof(bool), "bool" },
            { typeof(char), "char" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(byte), "byte" },
            { typeof(ushort), "ushort" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
        };

        public static string Format(Type type)
        {
            var builder = new StringBuilder();
            var generic = type.GetTypeInfo().GenericTypeArguments;
            var genericStart = 0;
            Print(type);
            return builder.ToString();

            void Print(Type t)
            {
                if (_basicTypes.TryGetValue(t, out var basic))
                {
                    builder.Append(basic);
                    return;
                }

                if (t.IsGenericParameter)
                {
                    builder.Append(t.Name);
                    return;
                }

                if (t.IsNested)
                {
                    Print(t.DeclaringType!);
                    builder.Append(".");
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

                if (t.GetTypeInfo().IsGenericTypeDefinition && type.GetTypeInfo().IsGenericTypeDefinition)
                {
                    var genericLength = t.GetTypeInfo().GenericTypeArguments.Length;
                    if (genericLength > genericStart)
                    {
                        builder.Append(t.Name.Split(new[] { '`' }, 2)[0]);
                        builder.Append("<");
                        for (var i = genericStart; i < genericLength; i++)
                        {
                            if (i > genericStart)
                            {
                                builder.Append(",");
                            }
                        }

                        genericStart = genericLength;
                        builder.Append(">");
                    }
                    else
                    {
                        builder.Append(t.Name);
                    }
                }
                else if (t.GetTypeInfo().IsGenericType)
                {
                    var genericLength = t.GetTypeInfo().GenericTypeArguments.Length;
                    if (genericLength > genericStart)
                    {
                        builder.Append(t.Name.Split(new[] { '`' }, 2)[0]);
                        builder.Append("<");
                        for (var i = genericStart; i < genericLength; i++)
                        {
                            if (i > genericStart)
                            {
                                builder.Append(", ");
                            }

                            Print(generic[i]);
                        }

                        genericStart = genericLength;
                        builder.Append(">");
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
