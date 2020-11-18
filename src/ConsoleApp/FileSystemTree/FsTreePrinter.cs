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

        public string Print(IDirectoryElement directoryElement) =>
            PrintHelper(directoryElement, "", "");

        private string PrintHelper(IFsNodeElement<object> fsNodeElement, string directChildStart,
            string indirectChildStart)
        {
            var checkBox = GetCheckBox(fsNodeElement.CheckState);
            var name = fsNodeElement.Name;
            var directoryElementString = $"{directChildStart}{checkBox} {name}";

            if (!(fsNodeElement is IDirectoryElement directoryElement)) return directoryElementString;

            var lastChildFsNodeElement = directoryElement.Content.Last();
            var directoryElementContent = string.Join('\n', directoryElement.Content
                .Select(childFsNodeElement =>
                {
                    var isLast = childFsNodeElement == lastChildFsNodeElement;
                    return PrintHelper(childFsNodeElement,
                        indirectChildStart + GetStart(GetHead(true, isLast), GetTail(true)),
                        indirectChildStart + GetStart(GetHead(false, isLast), GetTail(false)));
                }));
            return directoryElementString + '\n' + directoryElementContent;
        }

        private readonly Dictionary<CheckState, string> _checkBoxes = new Dictionary<CheckState, string>
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
