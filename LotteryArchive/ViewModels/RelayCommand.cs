using System.Windows.Input;

namespace LotteryArchive.ViewModels
{
    /// <summary>
    /// Реализация команды для MVVM (RelayCommand)
    /// </summary>
    public class RelayCommand : ICommand // Класс, реализующий интерфейс ICommand для привязки команд в MVVM.
    {
        private readonly Action<object> _execute; // Делегат, выполняющий действие команды.
        private readonly Func<object, bool> _canExecute; // Делегат для проверки, доступна ли команда.

        public event EventHandler CanExecuteChanged // Событие, возникающее при изменении возможности выполнения команды.
        {
            add => CommandManager.RequerySuggested += value; // Подписывается на стандартное событие перерасчёта доступности команд.
            remove => CommandManager.RequerySuggested -= value; // Отписывается от события перерасчёта.
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) // Конструктор с обязательным делегатом выполнения и необязательным делегатом проверки.
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute)); // Присваивает делегат выполнения или выбрасывает исключение, если он null.
            _canExecute = canExecute; // Присваивает делегат проверки доступности команды (может быть null).
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter); // Проверяет, может ли команда выполниться (если нет делегата проверки, возвращает true).

        public void Execute(object parameter) => _execute(parameter); // Выполняет действие команды.
    }
}