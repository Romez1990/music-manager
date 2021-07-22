using System;
using System.Collections.Generic;

namespace Utils.Reflection.Exceptions {
    public sealed class ConstructionNotFountException : MemberNotFoundException {
        public ConstructionNotFountException(Type type, IEnumerable<Type> args)
            : base("constructor", type, null, $"with types {GetArgTypes(args)}") { }

        private static string GetArgTypes(IEnumerable<Type> argTypes) =>
            string.Join(", ", argTypes);
    }
}
