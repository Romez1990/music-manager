using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utils.Reflection.Exceptions;

namespace Utils.Reflection {
    public static class ReflectionHelper {
        public static MethodInfo GetMethodInfo(this Type type, string methodName, IEnumerable<object> args) {
            var argTypes = args.GetTypes();
            var methodInfo = type.GetMethod(methodName, argTypes);
            if (methodInfo is null)
                throw new MethodNotFoundException(type, methodName, argTypes);
            return methodInfo;
        }

        public static T Invoke<T>(this MethodInfo methodInfo, object @object, object[] args) =>
            (T)methodInfo.Invoke(@object, args);

        public static T Invoke<T>(this object @object, string methodName, object[] args) =>
            @object.GetType().GetMethodInfo(methodName, args).Invoke<T>(@object, args);

        public static T InvokeGeneric<T>(this object @object, string methodName, Type[] typeArgs, object[] args) =>
            @object.GetType().GetMethodInfo(methodName, args).MakeGenericMethod(typeArgs).Invoke<T>(@object, args);

        private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName) {
            var propertyInfo = type.GetProperty(propertyName, DefaultLookup);
            if (propertyInfo is null)
                throw new PropertyNotFoundException(type, propertyName);
            return propertyInfo;
        }

        public static T GetValue<T>(this PropertyInfo propertyInfo, object @object) =>
            (T)propertyInfo.GetValue(@object);

        public static T GetPropertyValue<T>(this object @object, string propertyName) =>
            @object.GetType().GetPropertyInfo(propertyName).GetValue<T>(@object);

        public static void SetPropertyValue(this object @object, string propertyName, object value) =>
            @object.GetType().GetPropertyInfo(propertyName).SetValue(@object, value);

        public static FieldInfo GetFieldInfo(this Type type, string fieldName) {
            var fieldInfo = type.GetField(fieldName, DefaultLookup);
            if (fieldInfo is null)
                throw new FieldNotFoundException(type, fieldName);
            return fieldInfo;
        }

        public static T GetValue<T>(this FieldInfo fieldInfo, object @object) =>
            (T)fieldInfo.GetValue(@object);

        public static T GetFieldValue<T>(this object @object, string fieldName) =>
            @object.GetType().GetFieldInfo(fieldName).GetValue<T>(@object);

        public static void SetFieldValue(this object @object, string fieldName, object value) =>
            @object.GetType().GetFieldInfo(fieldName).SetValue(@object, value);

        private static Type[] GetTypes(this IEnumerable<object> args) =>
            args.Map(arg => arg.GetType()).ToArray();
    }
}
