using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MVVM
{
    public static class BinderFactory
    {
        private static readonly List<Type> _concreteBinders = new();

        public static void RegisterBinder(Type type)
        {
            _concreteBinders.Add(type);
        }

        public static void RegisterBinder<T>() where T : IBinder
        {
            _concreteBinders.Add(typeof(T));
        }

        public static BinderComposite CreateComposite(object view, object model)
        {
            var children = new List<IBinder>();

            //Bind self:
            if (TryCreateConcrete(view, model, out var binder)) children.Add(binder);

            //Bind children:
            var viewMembers = Scanner.ScanMembers(view);
            var modelMembers = Scanner.ScanMembers(model);

            foreach (var (id, viewMember) in viewMembers)
                if (modelMembers.TryGetValue(id, out var modelMember))
                    if (TryResolve(viewMember, modelMember, view, model, out var childBinder))
                        children.Add(childBinder);

            return new BinderComposite(children);
        }

        private static bool TryResolve(
            MemberInfo viewMember,
            MemberInfo modelMember,
            object view,
            object model,
            out IBinder binder
        )
        {
            var matchesChildren = Resolver.TryResolve(
                viewMember,
                modelMember,
                view, model,
                out var childView,
                out var childModel
            );

            if (!matchesChildren)
            {
                binder = default;
                return false;
            }

            if (TryCreateConcrete(childView, childModel, out binder)) return true;

            Debug.LogWarning("Can't create binder for " +
                             $"View: {childView.GetType().Name} and ViewModel: {childModel.GetType().Name}");
            return false;
        }

        public static bool TryCreateConcrete(object view, object model, out IBinder binder)
        {
            foreach (var binderType in _concreteBinders)
            {
                var constructor = binderType.GetConstructors(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly
                )[0];

                var parameters = constructor.GetParameters();
                if (parameters.Length != 2) continue;

                if (parameters[0].ParameterType.IsInstanceOfType(view) &&
                    parameters[1].ParameterType.IsInstanceOfType(model))
                {
                    binder = Activator.CreateInstance(binderType, view, model) as IBinder;
                    return true;
                }

                if (parameters[0].ParameterType.IsInstanceOfType(model) &&
                    parameters[1].ParameterType.IsInstanceOfType(view))
                {
                    binder = Activator.CreateInstance(binderType, model, view) as IBinder;
                    return true;
                }
            }

            binder = null;
            return false;
        }
    }
}