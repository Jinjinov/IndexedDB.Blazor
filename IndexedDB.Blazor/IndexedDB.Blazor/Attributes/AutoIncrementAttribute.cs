using System;

namespace IndexedDB.Blazor.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AutoIncrementAttribute : Attribute { }
}
