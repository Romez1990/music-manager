using System.Windows;
using System.Windows.Controls;
using Core.FileSystemElement;

namespace WpfApp {
    public class FsNodeStyleSelector : StyleSelector {
        public Style DirectoryStyle { get; set; }
        public Style FileStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container) =>
            ((IFileElement)item).Match(
                _ => DirectoryStyle,
                _ => FileStyle
            );
    }
}
