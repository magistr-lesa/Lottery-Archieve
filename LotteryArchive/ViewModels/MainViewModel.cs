using LotteryArchive.Views;
using Model.Core;
using Model.Core.Concrete;
using Model.Data.Abstract;
using Model.Data.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace LotteryArchive.ViewModels
{
    public class MainViewModel : ViewModelBase // Основная модель представления, реализующая логику приложения.
    {
        private readonly DataSerializer _jsonSerializer; // Сериализатор для формата JSON.
        private readonly DataSerializer _xmlSerializer; // Сериализатор для формата XML.

        private LotteryParticipant _selectedParticipant; // Выбранный участник для редактирования или отображения.
        private Lottery _currentLottery; // Текущая активная лотерея.
        private string _statusMessage = "Добро пожаловать!"; // Текущее сообщение статуса приложения.
        private string _selectedFormat = "JSON"; // Текущий выбранный формат сохранения данных.

        public ObservableCollection<LotteryParticipant> Participants { get; } = new(); // Коллекция участников лотереи.
        public ObservableCollection<Lottery> Lotteries { get; } = new(); // Коллекция проведённых лотерей.

        public IEnumerable<string> AvailableFormats { get; } = new List<string> { "JSON", "XML" }; // Доступные форматы для сохранения.

        public string SelectedFormat // Свойство для получения и установки формата сохранения.
        {
            get => _selectedFormat; // Получает текущее значение формата.
            set
            {
                if (SetField(ref _selectedFormat, value)) // Проверяет, изменилось ли значение формата.
                {
                    TryMigrateFiles(value); // Пытается мигрировать файлы из старого формата в новый.
                }
            }
        }

        public LotteryParticipant SelectedParticipant // Свойство для получения и установки выбранного участника.
        {
            get => _selectedParticipant; // Возвращает текущего выбранного участника.
            set => SetField(ref _selectedParticipant, value); // Устанавливает нового выбранного участника и уведомляет об изменении.
        }

        public string StatusMessage // Свойство для получения и установки сообщения статуса.
        {
            get => _statusMessage; // Возвращает текущее сообщение статуса.
            set => SetField(ref _statusMessage, value); // Устанавливает новое сообщение статуса и уведомляет об изменении.
        }

        public ICommand AddParticipantCommand { get; } // Команда на добавление участника.
        public ICommand EditParticipantCommand { get; } // Команда на редактирование выбранного участника.
        public ICommand CreateLotteryCommand { get; } // Команда на создание новой лотереи.
        public ICommand LoadStatisticsCommand { get; } // Команда на загрузку сохранённой статистики.
        public ICommand ShowGraphsCommand { get; } // Команда на отображение графиков.

        public MainViewModel() // Конструктор основной модели представления.
        {
            _jsonSerializer = new JsonDataSerializer(); // Инициализирует сериализатор JSON.
            _xmlSerializer = new XmlDataSerializer(); // Инициализирует сериализатор XML.

            AddParticipantCommand = new RelayCommand(_ => ExecuteAddParticipant()); // Назначает команду добавления участника.
            EditParticipantCommand = new RelayCommand(_ => ExecuteEditParticipant(), _ => SelectedParticipant != null); // Назначает команду редактирования участника, доступную при выборе участника.
            CreateLotteryCommand = new RelayCommand(_ => ExecuteCreateLottery(), _ => Participants.Any()); // Назначает команду создания лотереи, доступную при наличии участников.
            LoadStatisticsCommand = new RelayCommand(_ => ExecuteLoadStatistics()); // Назначает команду загрузки статистики.
            ShowGraphsCommand = new RelayCommand(_ => ExecuteShowGraphs(), _ => Participants.Any(p => p.IsSelectedForGraph)); // Назначает команду отображения графиков, доступную при выборе хотя бы одного участника для графика.

            // Пример: загружаем при старте, если файл уже есть
            var defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"saved_lottery.{_selectedFormat.ToLower()}"); // Определяет путь к файлу сохранённой лотереи.
            if (File.Exists(defaultPath)) // Проверяет существование файла.
            {
                ExecuteLoadStatistics(); // Загружает статистику, если файл найден.
            }
        }

        #region Участники

        private void ExecuteAddParticipant() // Метод для добавления нового участника.
        {
            var wnd = new AddParticipantWindow // Создаёт окно для ввода данных нового участника.
            {
                Owner = Application.Current.MainWindow // Устанавливает владельца окна.
            };
            if (wnd.ShowDialog() == true) // Если пользователь подтвердил ввод данных.
            {
                try
                {
                    var participant = new LotteryParticipant(wnd.FirstName, wnd.LastName, wnd.MiddleName, wnd.InitialBalance) // Создаёт участника с введёнными данными.
                    {
                        Greed = wnd.Greed // Устанавливает уровень жадности из окна.
                    };
                    Participants.Add(participant); // Добавляет нового участника в коллекцию.
                    StatusMessage = $"Добавлен участник {participant.FullName}."; // Обновляет сообщение статуса.
                }
                catch (Exception ex) // Обрабатывает возможные исключения при создании участника.
                {
                    MessageBox.Show($"Не удалось добавить участника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); // Показывает сообщение об ошибке.
                }
            }
        }

        private void ExecuteEditParticipant() // Метод для редактирования существующего участника.
        {
            if (SelectedParticipant == null) // Проверяет, выбран ли участник.
                return; // Если ни один участник не выбран, выходим.

            var wnd = new AddParticipantWindow(SelectedParticipant) // Создаёт окно, передавая выбранного участника для редактирования.
            {
                Owner = Application.Current.MainWindow // Устанавливает владельца окна.
            };
            if (wnd.ShowDialog() == true) // Если пользователь подтвердил изменения.
            {
                StatusMessage = $"Участник {SelectedParticipant.FullName} обновлён."; // Обновляет сообщение статуса.
            }
        }

        #endregion

        #region Создание лотереи

        private void ExecuteCreateLottery() // Метод для создания новой лотереи.
        {
            if (!Participants.Any()) // Проверяет, есть ли хотя бы один участник.
            {
                MessageBox.Show("Сначала добавьте хотя бы одного участника.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning); // Предупреждает пользователя.
                return; // Выходит, если участников нет.
            }

            var wnd = new CreateLotteryWindow(Participants) // Создаёт окно для параметров новой лотереи.
            {
                Owner = Application.Current.MainWindow // Устанавливает владельца окна.
            };
            if (wnd.ShowDialog() != true) // Если пользователь отменил создание лотереи.
                return; // Выходит без создания.

            try
            {
                string name = wnd.LotteryName; // Название новой лотереи.
                int totalTickets = wnd.TotalTickets; // Общее количество билетов.
                decimal prizeFund = wnd.PrizeFund; // Размер призового фонда.
                var chosenParticipants = wnd.SelectedParticipants; // Список участников, принимающих участие.

                _currentLottery = new Lottery(name, totalTickets, prizeFund, chosenParticipants.Count); // Создаёт объект лотереи.

                Lotteries.Clear(); // Очищает коллекцию предыдущих лотерей.
                Lotteries.Add(_currentLottery); // Добавляет новую лотерею в коллекцию.

                var ticketNumbers = Enumerable.Range(1, totalTickets) // Генерирует последовательность номеров билетов.
                                              .OrderBy(_ => Guid.NewGuid()) // Перемешивает номера случайным образом.
                                              .ToList(); // Преобразует в список.

                int idx = 0; // Индекс текущего билета в перемешанном списке.
                int countParticipants = chosenParticipants.Count; // Количество выбранных участников.
                while (idx < ticketNumbers.Count) // Распределяет билеты между участниками.
                {
                    for (int p = 0; p < countParticipants && idx < ticketNumbers.Count; p++, idx++) // Перебирает участников по кругу, пока есть билеты.
                    {
                        var participant = chosenParticipants[p]; // Текущий участник.
                        var ticket = new RegularTicket(ticketNumbers[idx]); // Создаёт билет с текущим номером.
                        bool sold = _currentLottery.SellTicket(participant, ticket); // Пытается продать билет участнику.
                        if (!sold) continue; // Если продажа не удалась, продолжаем.
                    }
                }

                // Сохраняем в текущем формате
                var extension = SelectedFormat.ToLower(); // Получает расширение файла на основе выбранного формата.
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"saved_lottery.{extension}"); // Формирует полный путь к файлу.
                if (SelectedFormat == "XML") // Если выбран формат XML.
                    _xmlSerializer.Serialize(_currentLottery, path, indent: true); // Сериализует лотерею в XML с отступами.
                else
                    _jsonSerializer.Serialize(_currentLottery, path, indent: true); // Сериализует лотерею в JSON с отступами.

                StatusMessage = $"Лотерея \"{name}\" создана и сохранена в \"{path}\"."; // Обновляет сообщение статуса.
            }
            catch (Exception ex) // Обрабатывает ошибки при создании лотереи.
            {
                StatusMessage = $"Ошибка при создании лотереи: {ex.Message}"; // Обновляет сообщение статуса с текстом ошибки.
            }
        }

        #endregion

        #region Загрузка статистики

        private void ExecuteLoadStatistics() // Метод для загрузки сохранённой статистики лотереи.
        {
            var extension = SelectedFormat.ToLower(); // Получает расширение файла на основе выбранного формата.
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"saved_lottery.{extension}"); // Формирует полный путь к файлу.
            if (!File.Exists(path)) // Проверяет существование файла.
            {
                StatusMessage = $"Файл статистики (saved_lottery.{extension}) не найден."; // Обновляет сообщение статуса о ненайденном файле.
                return; // Выходит, если файл не найден.
            }

            try
            {
                Lottery loaded; // Переменная для загруженной лотереи.
                if (SelectedFormat == "XML") // Если формат XML.
                    loaded = _xmlSerializer.Deserialize<Lottery>(path); // Десериализует объект лотереи из XML.
                else
                    loaded = _jsonSerializer.Deserialize<Lottery>(path); // Десериализует объект лотереи из JSON.

                if (loaded != null) // Если десериализация прошла успешно.
                {
                    _currentLottery = loaded; // Сохраняет загруженную лотерею как текущую.

                    Lotteries.Clear(); // Очищает коллекцию лотерей.
                    Lotteries.Add(_currentLottery); // Добавляет загруженную лотерею.

                    Participants.Clear(); // Очищает коллекцию участников.
                    foreach (var entry in _currentLottery.GetTicketsPerParticipant()) // Перебирает записи о проданных билетах по участникам.
                    {
                        var p = new LotteryParticipant(entry.ParticipantName, "", "", 0m) // Создаёт нового участника на основе имени.
                        {
                            Greed = 0m // Устанавливает жадность в 0 (не важна при просмотре статистики).
                        };
                        for (int i = 0; i < entry.TicketsCount; i++) // Для каждого проданного билета.
                        {
                            var ticket = new RegularTicket(i + 1); // Создаёт билет с номером.
                            p.AddTicket(ticket); // Добавляет билет участнику.
                        }
                        Participants.Add(p); // Добавляет участника в коллекцию.
                    }

                    StatusMessage = $"Статистика загружена: {_currentLottery.Name}, продано {_currentLottery.GetTotalSoldTickets()} билетов."; // Обновляет сообщение статуса о загруженной статистике.
                }
            }
            catch (Exception ex) // Обрабатывает ошибки при загрузке статистики.
            {
                StatusMessage = $"Ошибка при загрузке статистики: {ex.Message}"; // Обновляет сообщение статуса с текстом ошибки.
            }
        }

        #endregion

        #region Миграция форматов

        private void TryMigrateFiles(string newFormat) // Метод для миграции файлов из одного формата в другой.
        {
            try
            {
                var oldFormat = newFormat == "JSON" ? "xml" : "json"; // Определяет старый формат (противоположный текущему).
                var oldFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"*.*") // Получает все файлы в директории приложения.
                                        .Where(f => f.EndsWith($".{oldFormat}", StringComparison.OrdinalIgnoreCase)) // Отбирает файлы с расширением старого формата.
                                        .ToList(); // Преобразует в список.
                foreach (var oldPath in oldFiles) // Для каждого файла старого формата.
                {
                    // Десериализуем в объект Lottery
                    Lottery lottery; // Переменная для объекта лотереи.
                    if (oldFormat == "json") // Если старый формат JSON.
                        lottery = _jsonSerializer.Deserialize<Lottery>(oldPath); // Десериализует из JSON.
                    else
                        lottery = _xmlSerializer.Deserialize<Lottery>(oldPath); // Десериализует из XML.

                    // Сохраняем в newFormat
                    var newPath = Path.ChangeExtension(oldPath, newFormat.ToLower()); // Формирует новый путь с новым расширением.
                    if (newFormat == "JSON") // Если новый формат JSON.
                        _jsonSerializer.Serialize(lottery, newPath, indent: true); // Сериализует лотерею в JSON.
                    else
                        _xmlSerializer.Serialize(lottery, newPath, indent: true); // Сериализует лотерею в XML.
                }

                StatusMessage = $"Миграция форматов из {oldFormat.ToUpper()} в {newFormat} выполнена."; // Обновляет сообщение статуса о результатах миграции.
            }
            catch (Exception ex) // Обрабатывает ошибки при миграции.
            {
                StatusMessage = $"Ошибка миграции форматов: {ex.Message}"; // Обновляет сообщение статуса с текстом ошибки.
            }
        }

        #endregion

        #region Построение графиков

        private void ExecuteShowGraphs() // Метод для отображения графиков выбранных участников.
        {
            var selected = Participants.Where(p => p.IsSelectedForGraph).ToList(); // Получает список участников, отмеченных для графиков.
            if (!selected.Any()) // Проверяет, есть ли хотя бы один выбранный участник.
            {
                MessageBox.Show("Выберите хотя бы одного участника для построения графиков.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information); // Предупреждает пользователя.
                return; // Выходит, если участники не выбраны.
            }

            var wnd = new GraphsWindow(selected) // Создаёт окно для отображения графиков.
            {
                Owner = Application.Current.MainWindow // Устанавливает владельца окна.
            };
            wnd.ShowDialog(); // Отображает окно модально.
        }

        #endregion
    }
}
