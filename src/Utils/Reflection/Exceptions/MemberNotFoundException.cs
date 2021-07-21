using System;
using Utils.String;

namespace Utils.Reflection.Exceptions {
    public abstract class MemberNotFoundException : Exception {
        protected MemberNotFoundException(string memberTypeName, Type type, string name)
            : base(GetMessage(memberTypeName, type, name)) { }

        private static string GetMessage(string memberTypeName, Type type, string name) =>
            $"{memberTypeName.Capitalize()} {name} not found in type {type}";
    }
}
