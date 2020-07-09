using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils.Reflection
{
    public static class ReflectionHelper
    {
        public static ConstructorInfo GetConstructor(Type type, Type[] argTypes)
        {
            const BindingFlags bindingAttributes = BindingFlags.NonPublic |
                                                   BindingFlags.Instance;
            var constructorInfo = type.GetConstructor(bindingAttributes, null, argTypes, null);
            if (constructorInfo == null)
                throw new ConstructionNotFountException();
            return constructorInfo;
        }

        public static FieldInfo GetField(Type type, string name)
        {
            const BindingFlags bindingAttributes = BindingFlags.NonPublic |
                                                   BindingFlags.Instance;
            var fieldInfo = type.GetField(name, bindingAttributes);
            if (fieldInfo == null)
                throw new FieldNotFoundException();
            return fieldInfo;
        }

        public static PropertyInfo GetProperty(Type type, string name)
        {
            var propertyInfo = type.GetProperty(name);
            if (propertyInfo == null)
                throw new PropertyNotFoundException();
            return propertyInfo;
        }

        public static MethodInfo GetMethod(Type type, string name, Type[] argTypes)
        {
            var methodInfo = type.GetMethod(name, argTypes);
            if (methodInfo == null)
                throw new MethodNotFoundException();
            return methodInfo;
        }

        public static T Construct<T>(params object[] args)
        {
            var type = typeof(T);
            var constructorInfo = GetConstructor(type, args.GetTypes());
            return (T)constructorInfo.Invoke(args);
        }

        public static TValue GetField<TValue>(object obj, string name)
        {
            var fieldInfo = GetField(obj.GetType(), name);
            return (TValue)fieldInfo.GetValue(obj);
        }

        public static Type[] GetTypes(this IEnumerable<object> args)
        {
            return args.Select(arg => arg.GetType()).ToArray();
        }
    }
}
