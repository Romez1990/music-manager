using System.Collections.Generic;
using System.Linq;
using Core.FileSystemElement;
using Core.IocContainer;
using static LanguageExt.Prelude;

namespace ConsoleApp.FsTreeFormatter {
    [Service]
    public class FsTreeFormatter : IFsTreeFormatter {
        public FsTreeFormatter() {
            _depth = 2;
            _bold = false;
        }

        private readonly int _depth;
        private readonly bool _bold;

        public string ToString(IDirectoryElement directory) =>
            FsNodeToString(directory, "", "");

        private readonly IReadOnlyDictionary<CheckState, string> _checkBoxes = new Dictionary<CheckState, string> {
                { CheckState.Unchecked, "[ ]" },
                { CheckState.CheckedPartially, "[-]" },
                { CheckState.Checked, "[*]" },
            };

        private string FsNodeToString(IFsNodeElement fsNode, string directChildPrefix, string indirectChildPrefix) {
            var checkBox = _checkBoxes[fsNode.CheckState];
            var name = fsNode.Name;
            var fsNodeLine = $"{directChildPrefix}{checkBox} {name}";
            var childrenLines = fsNode.Match(
                directory => ChildrenToString(directory, indirectChildPrefix),
                constant<string, IFileElement>("")
            );
            return fsNodeLine + childrenLines;
        }

        private string ChildrenToString(IDirectoryElement directory, string prefix) =>
            directory.IsExpanded
                ? ExpandedChildrenToString(directory, prefix)
                : CollapsedChildrenToString(prefix);

        private string ExpandedChildrenToString(IDirectoryElement directory, string prefix) {
            var lastFsNode = directory.Children.Last();
            var childrenLines = directory.Children.Map(fsNode => {
                var isLast = ReferenceEquals(fsNode, lastFsNode);
                var directChildPrefix = prefix + GetPrefix(true, isLast);
                var indirectChildPrefix = prefix + GetPrefix(false, isLast);
                return FsNodeToString(fsNode, directChildPrefix, indirectChildPrefix);
            });
            return '\n' + string.Join('\n', childrenLines);
        }

        private string CollapsedChildrenToString(string prefix) {
            var directChildPrefix = prefix + GetPrefix(true, true);
            const string children = "...";
            return $"\n{directChildPrefix} {children}";
        }

        private string GetPrefix(bool isDirect, bool isLast) {
            var head = GetHead(isDirect, isLast);
            var tail = GetTail(isDirect);
            var longTail = new string(tail, _depth);
            return $" {head}{longTail}";
        }

        private char GetHead(bool isDirect, bool isLast) =>
            isLast
                ? GetLastHead(isDirect)
                : GetNotLastHead(isDirect);

        private char GetLastHead(bool isDirect) {
            if (isDirect)
                return _bold ? '┗' : '└';
            return ' ';
        }

        private char GetNotLastHead(bool isDirect) {
            if (isDirect)
                return _bold ? '┣' : '├';
            return _bold ? '┃' : '│';
        }

        private char GetTail(bool isDirect) {
            if (isDirect)
                return _bold ? '━' : '─';
            return ' ';
        }
    }
}
