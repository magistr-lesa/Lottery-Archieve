using LotteryArchive.ViewModels;
using LotteryArchive.Views;
using Model.Core;
using Model.Core.Concrete;
using Model.Data;
using Model.Data.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace LotteryArchive
{
    public class MainViewModel : ViewModelBase // Основная модель представления приложения.
    {
        private readonly IDataSerializer _jsonSerializer; // Интерфейс сериализатора данных для JSON.
        private Lottery _currentLottery; // Текущая активная лотерея.
        private string _statusMessage = "Добро пожаловать!"; // Сообщение статуса по умолчанию.

        public ObservableCollection<LotteryParticipant> Participants { get; } = new(); // Коллекция участников лотереи.
        public ObservableCollection<Lottery> Lotteries { get; } = new(); // Коллекция созданных лотерей.

        public string StatusMessage // Свойство для получения и установки сообщения статуса.
        {
            get => _statusMessage; // Возвращает текущее значение сообщения.
            set => SetField(ref _statusMessage, value); // Устанавливает новое значение и уведомляет об изменении.
        }

        public ICommand CreateLotteryCommand { get; } // Команда для создания лотереи.
        public ICommand LoadStatisticsCommand { get; } // Команда для загрузки статистики.

        public MainViewModel() // Конструктор основной модели представления.
        {
            // Сериализатор JSON (базовый)
            _jsonSerializer = new JsonDataSerializer(); // Инициализирует сериализатор JSON.

            CreateLotteryCommand = new RelayCommand(_ => ExecuteCreateLottery()); // Привязывает команду создания лотереи.
            LoadStatisticsCommand = new RelayCommand(_ => ExecuteLoadStatistics()); // Привязывает команду загрузки статистики.
        }

        private void ExecuteCreateLottery() // Метод для обработки создания лотереи.
        {
            // Проверяем, что есть хотя бы один участник в списке
            if (!Participants.Any()) // Проверяет, что коллекция участников не пуста.
            {
                MessageBox.Show("Сначала добавьте хотя бы одного участника.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning); // Выводит предупреждение о необходимости добавить участника.
                return; // Прерывает выполнение метода.
            }

            // Открываем окно создания лотереи, передавая список существующих участников
            var wnd = new CreateLotteryWindow(Participants) // Создаёт экземпляр окна CreateLotteryWindow и передаёт текущих участников.
            {
                Owner = Application.Current.MainWindow // Устанавливает владельца окна.
            };
            if (wnd.ShowDialog() != true) // Показывает окно модально и проверяет результат диалога.
                return; // Если пользователь отменил, прерывает выполнение метода.

            try
            {
                // 1. Читаем параметры из окна
                string name = wnd.LotteryName; // Получает название лотереи из окна.
                int totalTickets = wnd.TotalTickets; // Получает общее количество билетов из окна.
                decimal prizeFund = wnd.PrizeFund; // Получает сумму призового фонда из окна.
                var chosenParticipants = wnd.SelectedParticipants; // Получает список выбранных участников.

                // 2. Создаём новый объект лотереи, указывая максимальное число участников
                _currentLottery = new Lottery(name, totalTickets, prizeFund, chosenParticipants.Count); // Создаёт объект Lottery с указанными параметрами.

                // Очищаем список лотерей и добавляем новую
                Lotteries.Clear(); // Очищает коллекцию Lotteries.
                Lotteries.Add(_currentLottery); // Добавляет новую лотерею в коллекцию.

                // 3. Раздаём билеты случайным образом среди выбранных участников:
                //    - формируем список номеров [1..totalTickets] и перемешиваем его
                var ticketNumbers = Enumerable.Range(1, totalTickets).OrderBy(_ => Guid.NewGuid()).ToList(); // Создаёт список номеров билетов и перемешивает его случайным образом.

                int idx = 0; // Индекс текущего билета.
                int countParticipants = chosenParticipants.Count; // Количество выбранных участников.
                while (idx < ticketNumbers.Count) // Пока остаются билеты, продолжаем распределять.
                {
                    for (int p = 0; p < countParticipants && idx < ticketNumbers.Count; p++, idx++) // Проходимся по каждому участнику по очереди, пока есть билеты.
                    {
                        var participant = chosenParticipants[p]; // Текущий участник.
                        // Пробуем «купить» билет (RegularTicket стоит 1 единицу)
                        var ticket = new RegularTicket(ticketNumbers[idx]); // Создаёт билет с текущим номером.
                        bool sold = _currentLottery.SellTicket(participant, ticket); // Пытается продать билет участнику.
                        if (!sold) // Если у участника недостаточно баланса, билет не продаётся.
                        {
                            // Если у участника недостаточно баланса, пропускаем его
                            continue; // Продолжаем к следующему участнику.
                        }
                    }
                }

                // 4. Сохраняем созданную лотерею в JSON
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saved_lottery.json"); // Формирует путь к файлу для сохранения.
                _jsonSerializer.Serialize(_currentLottery, path); // Сериализует объект лотереи в JSON и сохраняет в файл.

                StatusMessage = $"Лотерея \"{name}\" создана и сохранена в \"{path}\". Участников: {chosenParticipants.Count}, билетов: {totalTickets}."; // Обновляет сообщение статуса с параметрами созданной лотереи.
            }
            catch (Exception ex) // Обработка возможных исключений при создании лотереи.
            {
                StatusMessage = $"Ошибка при создании лотереи: {ex.Message}"; // Обновляет сообщение статуса с текстом ошибки.
            }
        }

        private void ExecuteLoadStatistics() // Метод для обработки загрузки статистики.
        {
            // Читаем файл saved_lottery.json и десериализуем лотерею
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saved_lottery.json"); // Формирует путь к файлу статистики.
            if (!File.Exists(path)) // Проверяет, существует ли файл.
            {
                StatusMessage = "Файл статистики (saved_lottery.json) не найден."; // Обновляет сообщение статуса о ненайденном файле.
                return; // Прерывает выполнение метода.
            }

            try
            {
                var loaded = _jsonSerializer.Deserialize<Lottery>(path); // Десериализует объект Lottery из файла JSON.
                if (loaded != null) // Если десериализация успешна.
                {
                    _currentLottery = loaded; // Сохраняет загруженную лотерею.

                    // Обновляем коллекции:
                    Lotteries.Clear(); // Очищает коллекцию Lotteries.
                    Lotteries.Add(_currentLottery); // Добавляет загруженную лотерею.

                    Participants.Clear(); // Очищает коллекцию участников.
                    // В базовой части мы не десериализуем отдельно участников,
                    // поэтому просто выводим тех, кто есть у лотереи через GetTicketsPerParticipant
                    foreach (var entry in _currentLottery.GetTicketsPerParticipant()) // Перебираем записи о количестве билетов на участника.
                    {
                        var p = new LotteryParticipant(entry.ParticipantName, "", "", 0m); // Создаём нового участника с именем и нулевыми значениями.
                        for (int i = 0; i < entry.TicketsCount; i++) // Для каждого билета у данного участника.
                        {
                            var ticket = new RegularTicket(i + 1); // Создаём билет с номером.
                            p.AddTicket(ticket); // Добавляем билет участнику.
                        }
                        Participants.Add(p); // Добавляем участника в коллекцию.
                    }

                    StatusMessage = $"Статистика загружена: {_currentLottery.Name}, продано {_currentLottery.GetTotalSoldTickets()} билетов."; // Обновляет сообщение статуса с данными загруженной лотереи.
                }
            }
            catch (Exception ex) // Обработка возможных исключений при загрузке статистики.
            {
                StatusMessage = $"Ошибка при загрузке статистики: {ex.Message}"; // Обновляет сообщение статуса с текстом ошибки.
            }
        }
    }
}
