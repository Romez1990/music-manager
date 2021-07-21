using System.Windows;
using System.Windows.Controls;
using Core.FileSystemElement;

namespace WpfApp {
    public class FsNodeTemplateSelector : DataTemplateSelector {
        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            var templateName = ((IFsNodeElement)item).Match(
                _ => "DirectoryTemplate",
                _ => "DirectoryTemplate"
            );
            var element = (FrameworkElement)container;
            return (DataTemplate)element.FindResource(templateName);
        }
    }
}
