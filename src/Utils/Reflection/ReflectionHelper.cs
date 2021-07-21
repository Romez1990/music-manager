using System;
using System.Reflection;
using Utils.Reflection.Exceptions;

namespace Utils.Reflection {
    public static class ReflectionHelper {
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
    }
}
