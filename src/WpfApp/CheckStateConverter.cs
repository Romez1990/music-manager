using System;
using System.Globalization;
using System.Windows.Data;
using Core.FileSystemElement;

namespace WpfApp {
    [ValueConversion(typeof(CheckState), typeof(bool?))]
    public class CheckStateConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (CheckState)value switch {
                CheckState.Unchecked => false,
                CheckState.CheckedPartially => null,
                CheckState.Checked => true,
                _ => throw new NotSupportedException(),
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
