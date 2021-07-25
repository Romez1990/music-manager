using System;

namespace Core.IocContainer {
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute {
        public bool ToSelf { get; init; }
    }
}
