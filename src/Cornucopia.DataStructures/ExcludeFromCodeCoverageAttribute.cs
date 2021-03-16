#if !NETCOREAPP3_1

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false)]
    internal sealed class ExcludeFromCodeCoverageAttribute : Attribute
    {
        /// <summary>Gets or sets the justification for excluding the member from code coverage.</summary>
        public string? Justification { get; set; }
    }
}

#endif