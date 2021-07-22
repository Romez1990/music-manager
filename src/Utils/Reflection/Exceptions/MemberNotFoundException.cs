using System;
using Utils.String;

namespace Utils.Reflection.Exceptions {
    public abstract class MemberNotFoundException : Exception {
        protected MemberNotFoundException(string memberTypeName, Type type, string name, string postName = null)
            : base(GetMessage(memberTypeName, type, name, postName)) { }

        private static string GetMessage(string memberTypeName, Type type, string nameOrNull, string postNameOrNull) =>
            $"{memberTypeName.Capitalize()}{GetName(nameOrNull)}{GetPostName(postNameOrNull)} not found in type {type}";

        private static string GetName(string nameOrNull) =>
            nameOrNull switch {
                null => "",
                var name => $" {name}",
            };

        private static string GetPostName(string postNameOrNull) =>
            postNameOrNull switch {
                null => "",
                var postName => $" {postName}",
            };
    }
}
