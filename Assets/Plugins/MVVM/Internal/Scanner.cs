using System.Collections.Generic;
using System.Reflection;

namespace MVVM
{
    internal static class Scanner
    {
        public static IReadOnlyDictionary<object, MemberInfo> ScanMembers(object target)
        {
            var members = new Dictionary<object, MemberInfo>();

            var type = target.GetType();
            var fields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy
            );

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<MemberAttribute>();
                if (attribute != null) members[attribute.id] = field;
            }

            var methods = type.GetMethods(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy
            );

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<MemberAttribute>();
                if (attribute != null) members[attribute.id] = method;
            }

            var properties = type.GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy
            );

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<MemberAttribute>();
                if (attribute != null) members[attribute.id] = property;
            }

            var events = type.GetEvents(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy
            );

            foreach (var eventInfo in events)
            {
                var attribute = eventInfo.GetCustomAttribute<MemberAttribute>();
                if (attribute != null) members[attribute.id] = eventInfo;
            }

            return members;
        }
    }
}