using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Cornucopia.DataStructures.Graph.Algorithms;
using Cornucopia.DataStructures.Utils;

using NUnit.Framework;

namespace Cornucopia.DataStructures.Graph
{
    [TestFixture]
    public class DependencyGraphTests
    {
        [Test]
        public void ComputeStronglyConnectedComponents_LongLine_HaveCorrectLength()
        {
            var g = new DependencyGraph<int>(i => i == 5 ? new[] { 12, 4 } : i <= 0 ? ArrayTools.Empty<int>() : new[] { i - 1 });
            var a = new TarjanStronglyConnectedComponentsAlgorithm<DependencyGraph<int>, int, DependencyGraph<int>.Edge>(g);
            var scc = a.ComputeStronglyConnectedComponents(100);
            Assert.That(scc.Length, Is.EqualTo(94));
            Assert.That(scc[5].Length, Is.EqualTo(8));
            var scc2 = a.ComputeStronglyConnectedComponents(105);
            Assert.That(scc2.Length, Is.EqualTo(5));
        }

        [Test]
        public void GetOutDegree_LongLine_IsCorrect()
        {
            var g = new DependencyGraph<int>(i => i == 5 ? new[] { 12, 4 } : i <= 0 ? ArrayTools.Empty<int>() : new[] { i - 1 });
            Assert.That(g.GetOutDegree(5), Is.EqualTo(2));
        }

        [Test]
        public void ComputeStronglyConnectedComponents_TypeDependencies_Print()
        {
            var assembly = typeof(DynamicArray<>).Assembly;
            var g = new DependencyGraph<Type>(t => CalculateTypeDependencies(t, assembly).ToArray());
            var a = new TarjanStronglyConnectedComponentsAlgorithm<DependencyGraph<Type>, Type, DependencyGraph<Type>.Edge>(g);

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var scc = a.ComputeStronglyConnectedComponents(type);
                for (var i = 0; i < scc.Length; i++)
                {
                    var component = scc[i];
                    if (component.Length == 1)
                    {
                        var t = component[0];
                        Console.WriteLine(t);
                    }
                    else
                    {
                        Console.WriteLine("╒════");
                        foreach (var t in component)
                        {
                            Console.WriteLine($"╞ {t}");
                        }
                        Console.WriteLine("╘════");
                    }
                }
            }
        }

        private static IEnumerable<Type> CalculateTypeDependencies(Type type, Assembly assembly)
        {
            var result = new HashSet<Type>();

            if (type.BaseType != null)
            {
                Add(type.BaseType);
            }

            foreach (var @interface in type.GetInterfaces())
            {
                Add(@interface);
            }

            foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
            {
                Add(fieldInfo.FieldType);
            }

            foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
            {
                Add(methodInfo.ReturnType);
                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    Add(parameterInfo.ParameterType);
                }
            }

            return result;

            void Add(Type t)
            {
                var et = t.GetElementType();
                while (et != null)
                {
                    t = et;
                    et = t.GetElementType();
                }

                if (t.IsGenericParameter)
                {
                    return;
                }

                if (t.Assembly != assembly)
                {
                    return;
                }

                if (t.IsGenericType)
                {
                    foreach (var genericArgument in t.GetGenericArguments())
                    {
                        Add(genericArgument);
                    }

                    t = t.GetGenericTypeDefinition();
                }

                result.Add(t);
            }
        }
    }
}