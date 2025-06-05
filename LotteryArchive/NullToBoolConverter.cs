using System.Globalization;
using System.Windows.Data;

namespace LotteryArchive
{
    /// <summary>
    /// Превращает null → False, всё остальное → True (для IsEnabled)
    /// </summary>
    public class NullToBoolConverter : IValueConverter // Класс-конвертер, реализующий IValueConverter.
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) // Метод для преобразования значения.
        {
            return value != null; // Возвращает true, если значение не null, иначе false.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) // Метод обратного преобразования (не поддерживается).
        {
            throw new NotSupportedException(); // Выбрасывает исключение, так как обратное преобразование не реализовано.
        }
    }
}