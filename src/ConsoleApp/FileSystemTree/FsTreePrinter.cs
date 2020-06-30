using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void PrintTree(IDirectoryElement directoryElement)
        {
            Console.OutputEncoding = Encoding.UTF8;
            PrintTreeHelper(directoryElement, "", "");
        }

        private void PrintTreeHelper(IFsNodeElement fsNodeElement, string directChildStart, string indirectChildStart)
        {
            var checkBox = GetCheckBox(fsNodeElement.CheckState);
            var name = fsNodeElement.FsNode.Name;
            Console.WriteLine($"{directChildStart}{checkBox} {name}");

            if (!(fsNodeElement is IDirectoryElement directoryElement)) return;

            var lastChildFsNodeElement = directoryElement.Content.Last();
            directoryElement.Content
                .ToList()
                .ForEach(childFsNodeElement =>
                {
                    var isLast = childFsNodeElement == lastChildFsNodeElement;
                    PrintTreeHelper(childFsNodeElement,
                        indirectChildStart + GetStart(GetHead(true, isLast), GetTail(true)),
                        indirectChildStart + GetStart(GetHead(false, isLast), GetTail(false)));
                });
        }

        private readonly Dictionary<CheckState, string> _checkBoxes = new Dictionary<CheckState, string>
        {
            {CheckState.Unchecked, "[ ]"},
            {CheckState.CheckedPartially, "[-]"},
            {CheckState.Checked, "[*]"},
        };

        private string GetCheckBox(CheckState checkState)
        {
            return _checkBoxes[checkState];
        }

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
