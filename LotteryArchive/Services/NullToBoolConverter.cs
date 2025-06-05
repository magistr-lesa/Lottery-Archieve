using System.Globalization;
using System.Windows.Data;

namespace LotteryArchive.Services
{
    /// <summary>
    /// Превращает null → False, всё остальное → True (для IsEnabled)
    /// </summary>
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
