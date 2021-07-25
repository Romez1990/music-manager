using System;

namespace ConsoleApp.ArgumentParser {
    [AttributeUsage(AttributeTargets.Property)]
    public class OperationAttribute : Attribute {
        public OperationAttribute(string name) {
            Name = name;
        }

        public string Name { get; }
    }
}
