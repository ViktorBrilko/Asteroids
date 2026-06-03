using System;
using JetBrains.Annotations;

namespace MVVM
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MethodAttribute : MemberAttribute
    {
        public MethodAttribute(object id) : base(id)
        {
        }
    }
}