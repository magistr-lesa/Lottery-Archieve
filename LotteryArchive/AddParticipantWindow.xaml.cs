using Model.Core.Concrete;
using System.Globalization;
using System.Windows;

namespace LotteryArchive.Views
{
    /// <summary>
    /// Логика взаимодействия для AddParticipantWindow.xaml
    /// (теперь может принимать существующего участника для редактирования)
    /// </summary>
    public partial class AddParticipantWindow : Window // Код-бихайнд для окна AddParticipantWindow.
    {
        private readonly LotteryParticipant _existingParticipant; // Ссылка на редактируемого участника (если есть).

        public string FirstName => txtFirstName.Text.Trim(); // Получает введённое имя, обрезая пробелы.
        public string LastName => txtLastName.Text.Trim(); // Получает введённую фамилию, обрезая пробелы.
        public string MiddleName => txtMiddleName.Text.Trim(); // Получает введённое отчество, обрезая пробелы.
        public decimal InitialBalance { get; private set; } // Свойство для хранения введённого начального баланса.
        public decimal Greed { get; private set; } // Свойство для хранения введённого уровня жадности.

        /// <summary>
        /// Конструктор для создания нового участника
        /// </summary>
        public AddParticipantWindow() // Конструктор без параметров для добавления нового участника.
        {
            InitializeComponent(); // Инициализирует компоненты окна.
            _existingParticipant = null; // Указывает, что участника для редактирования нет.
        }

        /// <summary>
        /// Конструктор для редактирования существующего участника
        /// </summary>
        public AddParticipantWindow(LotteryParticipant participant) : this() // Конструктор для редактирования передаёт участника в базовый конструктор.
        {
            _existingParticipant = participant; // Сохраняет ссылку на существующего участника.

            // Предзаполняем поля
            txtFirstName.Text = participant.FirstName; // Устанавливает существующее имя в поле ввода.
            txtLastName.Text = participant.LastName; // Устанавливает существующую фамилию в поле ввода.
            txtMiddleName.Text = participant.MiddleName; // Устанавливает существующее отчество в поле ввода.
            txtBalance.Text = participant.Balance.ToString(CultureInfo.InvariantCulture); // Устанавливает существующий баланс в поле ввода.
            txtGreed.Text = participant.Greed.ToString(CultureInfo.InvariantCulture); // Устанавливает существующий уровень жадности в поле ввода.
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) // Обработчик нажатия кнопки OK.
        {
            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)) // Проверяет, введены ли имя и фамилия.
            {
                MessageBox.Show("Имя и фамилия обязательны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Показывает сообщение об ошибке.
                return; // Прерывает выполнение, если данные некорректны.
            }

            // Баланс
            if (!decimal.TryParse(txtBalance.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal bal) // Пытается распарсить введённый баланс.
                || bal < 0) // Проверяет, что баланс неотрицательный.
            {
                MessageBox.Show("Баланс должен быть неотрицательным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Показывает сообщение об ошибке.
                return; // Прерывает выполнение, если баланс некорректен.
            }

            // Жадность
            if (!decimal.TryParse(txtGreed.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal gr) // Пытается распарсить введённый уровень жадности.
                || gr < 0 || gr > 1) // Проверяет, что жадность в диапазоне от 0 до 1.
            {
                MessageBox.Show("Жадность — число от 0 до 1.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Показывает сообщение об ошибке.
                return; // Прерывает выполнение, если жадность некорректна.
            }

            InitialBalance = bal; // Сохраняет значение начального баланса.
            Greed = gr; // Сохраняет значение жадности.

            // Если передан существующий участник — обновляем его поля
            if (_existingParticipant != null) // Проверяет, передан ли участник для редактирования.
            {
                _existingParticipant.ChangeName(FirstName, LastName, MiddleName); // Обновляет имя участника.
                var current = _existingParticipant.Balance; // Текущий баланс участника.
                if (current > bal) // Проверяет, больше ли текущий баланс нового значения.
                    // Если текущий > нового, то «отнимаем» разницу
                    _existingParticipant.TrySpendMoney(current - bal); // Уменьшает баланс на разницу.
                else if (current < bal) // Проверяет, меньше ли текущий баланс нового значения.
                    // Если текущий < нового, то «добавляем» недостающую сумму
                    _existingParticipant.AddMoney(bal - current); // Увеличивает баланс на недостающую сумму.

                _existingParticipant.Greed = Greed; // Обновляет уровень жадности участника.
            }

            DialogResult = true; // Устанавливает результат диалога как успешный.
            Close(); // Закрывает окно.
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) // Обработчик нажатия кнопки Отмена.
        {
            DialogResult = false; // Устанавливает результат диалога как отменённый.
            Close(); // Закрывает окно.
        }
    }
}
