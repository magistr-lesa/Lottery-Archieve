using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LotteryArchive.ViewModels
{
    /// <summary>
    /// Базовый класс для ViewModel (INotifyPropertyChanged)
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged // Абстрактный базовый класс, реализующий INotifyPropertyChanged.
    {
        public event PropertyChangedEventHandler PropertyChanged; // Событие, оповещающее об изменении свойства.

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) // Метод для вызова события изменения свойства.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Вызывает событие, если подписчики есть.
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) // Универсальный метод для установки поля и уведомления об изменении.
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false; // Если новое значение равно старому, возвращает false без уведомления.
            field = value; // Устанавливает новое значение поля.
            OnPropertyChanged(propertyName); // Вызывает уведомление об изменении свойства.
            return true; // Возвращает true, показывая, что значение изменилось.
        }
    }
}
