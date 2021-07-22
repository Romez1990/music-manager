using System;

namespace Utils.Reflection.Exceptions {
    public sealed class FieldNotFoundException : MemberNotFoundException {
        public FieldNotFoundException(Type type, string name)
            : base("field", type, name) { }
    }
}
