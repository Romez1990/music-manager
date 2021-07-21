using System;

namespace Utils.Reflection.Exceptions {
    public sealed class PropertyNotFoundException : MemberNotFoundException {
        public PropertyNotFoundException(Type type, string name)
            : base("property", type, name) { }
    }
}
