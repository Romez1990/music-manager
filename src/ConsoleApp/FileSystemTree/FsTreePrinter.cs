using System.Collections.Generic;
using System.Linq;
using Core.FileSystem;

namespace ConsoleApp.FileSystemTree
{
    public class FsTreePrinter : IFsTreePrinter
    {
        public FsTreePrinter()
        {
            _bold = false;
        }

        private const int Depth = 2;

        private readonly bool _bold;

        public string Print(IDirectoryElement directory) =>
            PrintHelper(directory, "", "");

        private string PrintHelper(IFsNodeElement<object> fsNode, string directChildStart,
            string indirectChildStart)
        {
            var checkBox = GetCheckBox(fsNode.CheckState);
            var name = fsNode.Name;
            var directoryString = $"{directChildStart}{checkBox} {name}";

            if (fsNode is not IDirectoryElement directoryElement) return directoryString;

            var lastChildFsNodeElement = directoryElement.Content.Last();
            var directoryElementContent = string.Join('\n', directoryElement.Content
                .Map(childFsNodeElement =>
                {
                    var isLast = childFsNodeElement == lastChildFsNodeElement;
                    return PrintHelper(childFsNodeElement,
                        indirectChildStart + GetStart(GetHead(true, isLast), GetTail(true)),
                        indirectChildStart + GetStart(GetHead(false, isLast), GetTail(false)));
                }));
            return directoryString + '\n' + directoryElementContent;
        }

        private readonly Dictionary<CheckState, string> _checkBoxes = new()
        {
            {CheckState.Unchecked, "[ ]"},
            {CheckState.CheckedPartially, "[-]"},
            {CheckState.Checked, "[*]"},
        };

        private string GetCheckBox(CheckState checkState) =>
            _checkBoxes[checkState];

        private string GetStart(char head, char tail)
        {
            var longTail = new string(tail, Depth);
            return $" {head}{longTail}";
        }

        private char GetHead(bool direct, bool isLast)
        {
            if (isLast)
            {
                if (direct)
                    return _bold ? '┗' : '└';
                return ' ';
            }

            if (direct)
                return _bold ? '┣' : '├';
            return _bold ? '┃' : '│';
        }

        private char GetTail(bool direct)
        {
            if (direct)
                return _bold ? '━' : '─';
            return ' ';
        }
    }
}
