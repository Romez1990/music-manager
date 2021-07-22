using System;
using System.Collections.Generic;

namespace Utils.Reflection.Exceptions {
    public sealed class MethodNotFoundException : MemberNotFoundException {
        public MethodNotFoundException(Type type, string name, IEnumerable<Type> argTypes)
            : base("method", type, name, $"with types {GetArgTypes(argTypes)}") { }

        private static string GetArgTypes(IEnumerable<Type> argTypes) =>
            string.Join(", ", argTypes);
    }
}
