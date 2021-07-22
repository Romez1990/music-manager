using System;
using Utils.String;

namespace Utils.Reflection.Exceptions {
    public abstract class MemberNotFoundException : Exception {
        protected MemberNotFoundException(string memberTypeName, Type type, string name, string postName = null)
            : base(GetMessage(memberTypeName, type, name, postName)) { }

        private static string GetMessage(string memberTypeName, Type type, string name, string postNameOrNull) =>
            $"{memberTypeName.Capitalize()} {name}{GetPostName(postNameOrNull)} not found in type {type}";

        private static string GetPostName(string postNameOrNull) =>
            postNameOrNull switch {
                null => "",
                var postName => $" {postName}",
            };
    }
}
