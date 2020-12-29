using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils.Reflection
{
    public static class ReflectionHelper
    {
        public static FieldInfo GetField(Type type, string name)
        {
            const BindingFlags bindingAttributes = BindingFlags.NonPublic |
                                                   BindingFlags.Instance;
            var fieldInfo = type.GetField(name, bindingAttributes);
            if (fieldInfo is null)
                throw new FieldNotFoundException();
            return fieldInfo;
        }

        public static PropertyInfo GetProperty(Type type, string name)
        {
            var propertyInfo = type.GetProperty(name);
            if (propertyInfo is null)
                throw new PropertyNotFoundException();
            return propertyInfo;
        }

        public static MethodInfo GetMethod(Type type, string name, Type[] argTypes)
        {
            var methodInfo = type.GetMethod(name, argTypes);
            if (methodInfo is null)
                throw new MethodNotFoundException();
            return methodInfo;
        }

        public static TValue GetField<TValue>(object obj, string name)
        {
            var fieldInfo = GetField(obj.GetType(), name);
            return (TValue)fieldInfo.GetValue(obj);
        }

        public static Type[] GetTypes(this IEnumerable<object> args)
        {
            return args.Map(arg => arg.GetType()).ToArray();
        }
    }
}
