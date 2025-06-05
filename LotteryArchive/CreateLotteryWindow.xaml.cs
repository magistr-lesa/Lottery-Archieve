using Model.Core.Concrete;
using System.Globalization;
using System.Windows;

namespace LotteryArchive.Views
{
    /// <summary>
    /// Окно для ввода параметров новой лотереи (теперь позволяет выбрать из существующих участников)
    /// </summary>
    public partial class CreateLotteryWindow : Window // Код-бихайнд для CreateLotteryWindow.xaml.
    {
        public string LotteryName => txtName.Text.Trim(); // Свойство для получения введённого названия лотереи.
        public int TotalTickets { get; private set; } // Свойство для хранения общего количества билетов.
        public decimal PrizeFund { get; private set; } // Свойство для хранения суммы призового фонда.

        /// <summary>
        /// Список выбранных пользователем участников
        /// </summary>
        public List<LotteryParticipant> SelectedParticipants { get; private set; } = new(); // Список участников, выбранных из ListBox.

        public CreateLotteryWindow(IEnumerable<LotteryParticipant> existingParticipants) // Конструктор, принимающий существующих участников.
        {
            InitializeComponent(); // Инициализирует компоненты окна.
            // Заполняем ListBox существующими участниками
            lstParticipants.ItemsSource = existingParticipants.ToList(); // Присваивает источник элементов для ListBox из переданного списка участников.
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) // Обработчик кнопки «ОК».
        {
            // Валидация Названия
            if (string.IsNullOrWhiteSpace(LotteryName)) // Проверяет, что название не пустое.
            {
                MessageBox.Show("Название лотереи не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Выводит предупреждение об ошибке.
                return; // Прерывает выполнение метода.
            }

            // Всего билетов
            if (!int.TryParse(txtTotalTickets.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int total) // Пытается распознать введённое целое число.
                || total <= 0) // Проверяет, что число больше нуля.
            {
                MessageBox.Show("«Всего билетов» должно быть целым положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Выводит предупреждение об ошибке.
                return; // Прерывает выполнение метода.
            }

            // Призовой фонд
            if (!decimal.TryParse(txtPrizeFund.Text.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal fund) // Пытается распознать введённое десятичное число.
                || fund < 0) // Проверяет, что сумма неотрицательная.
            {
                MessageBox.Show("Призовой фонд должен быть неотрицательным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Выводит предупреждение об ошибке.
                return; // Прерывает выполнение метода.
            }

            // Проверяем, что выбрано хотя бы 1 участник
            if (lstParticipants.SelectedItems.Count == 0) // Проверяет, выбран ли хотя бы один элемент в ListBox.
            {
                MessageBox.Show("Нужно выбрать хотя бы одного участника из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Выводит предупреждение об ошибке.
                return; // Прерывает выполнение метода.
            }

            TotalTickets = total; // Сохраняет общее количество билетов.
            PrizeFund = fund; // Сохраняет сумму призового фонда.

            // Сохраняем выбранных участников
            SelectedParticipants = lstParticipants.SelectedItems // Получает выбранные элементы ListBox.
                                    .Cast<LotteryParticipant>() // Приводит их к типу LotteryParticipant.
                                    .ToList(); // Преобразует в List<LotteryParticipant>.

            DialogResult = true; // Устанавливает результат диалога в true.
            Close(); // Закрывает окно.
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) // Обработчик кнопки «Отмена».
        {
            DialogResult = false; // Устанавливает результат диалога в false.
            Close(); // Закрывает окно.
        }
    }
}