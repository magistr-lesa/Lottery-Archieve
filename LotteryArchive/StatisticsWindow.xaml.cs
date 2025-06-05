using Model.Core;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LotteryArchive.Views
{
    /// <summary>
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window // Код-бихайнд для StatisticsWindow.xaml.
    {
        private readonly IStatistics _statistics; // Интерфейс для получения статистических данных.

        public StatisticsWindow(IStatistics statistics) // Конструктор, принимающий объект статистики.
        {
            InitializeComponent(); // Инициализирует компоненты окна.
            _statistics = statistics; // Сохраняет ссылку на объект статистики.
            DrawChart(); // Вызывает метод для рисования диаграммы.
        }

        private void DrawChart() // Метод для построения столбчатой диаграммы.
        {
            // Получаем данные: [(Имя участника, число билетов)]
            var data = _statistics.GetTicketsPerParticipant().ToList(); // Получает список записей (имя участника, количество билетов).
            if (!data.Any()) // Проверяет, есть ли данные.
            {
                // Если данных нет — показываем сообщение
                var tb = new System.Windows.Controls.TextBlock // Создаёт текстовый блок для сообщения.
                {
                    Text = "Нет данных для отображения графика.", // Текст сообщения.
                    FontSize = 16, // Размер шрифта.
                    Foreground = Brushes.Gray // Цвет текста (серый).
                };
                Canvas.SetLeft(tb, 10); // Устанавливает горизонтальную позицию сообщения.
                Canvas.SetTop(tb, 10); // Устанавливает вертикальную позицию сообщения.
                chartCanvas.Children.Add(tb); // Добавляет сообщение на канвас.
                return; // Выходит из метода.
            }

            // Простая столбчатая диаграмма:
            // по оси X — имена участников, по оси Y — количество билетов.
            double canvasWidth = data.Count * 100;  // Вычисляет ширину канваса как количество участников * 100px.
            double canvasHeight = chartCanvas.Height - 40; // Вычисляет высоту области для столбцов, учитывая отступы.

            chartCanvas.Width = Math.Max(canvasWidth, chartCanvas.ActualWidth); // Устанавливает ширину канваса как максимум требуемой и текущей.

            int maxTickets = data.Max(d => d.TicketsCount); // Находит максимальное количество билетов среди участников.

            for (int i = 0; i < data.Count; i++) // Проходит по каждому участнику.
            {
                var (name, count) = data[i]; // Получает имя и количество билетов.
                double barHeight = (maxTickets == 0) ? 0 : (count / (double)maxTickets) * (canvasHeight - 40); // Вычисляет высоту столбца пропорционально максимальному количеству билетов.

                // Рисуем прямоугольник-столбец
                var rect = new Rectangle // Создаёт прямоугольник для столбца.
                {
                    Width = 50, // Фиксированная ширина столбца.
                    Height = barHeight, // Высота столбца.
                    Fill = Brushes.SteelBlue // Цвет столбца (синий).
                };
                // Располагаем снизу
                Canvas.SetLeft(rect, i * 100 + 20); // Устанавливает горизонтальную позицию столбца.
                Canvas.SetTop(rect, canvasHeight - barHeight + 20); // Устанавливает вертикальную позицию столбца.
                chartCanvas.Children.Add(rect); // Добавляет столбец на канвас.

                // Подпись — имя
                var tbName = new System.Windows.Controls.TextBlock // Создаёт текстовый блок для имени участника.
                {
                    Text = name, // Текст — имя участника.
                    Width = 80, // Фиксированная ширина подписи.
                    TextAlignment = TextAlignment.Center, // Выравнивание текста по центру.
                    FontSize = 12 // Размер шрифта.
                };
                Canvas.SetLeft(tbName, i * 100 + 10); // Устанавливает позицию подписи по горизонтали.
                Canvas.SetTop(tbName, canvasHeight + 25); // Устанавливает позицию подписи по вертикали (под столбцом).
                chartCanvas.Children.Add(tbName); // Добавляет подпись на канвас.

                // Подпись — число билетов над столбцом
                var tbCount = new System.Windows.Controls.TextBlock // Создаёт текстовый блок для отображения количества билетов.
                {
                    Text = count.ToString(), // Текст — число билетов.
                    FontWeight = FontWeights.Bold, // Жирный шрифт для значимого числа.
                    FontSize = 12 // Размер шрифта.
                };
                Canvas.SetLeft(tbCount, i * 100 + 20); // Устанавливает горизонтальную позицию текста.
                Canvas.SetTop(tbCount, canvasHeight - barHeight); // Устанавливает вертикальную позицию текста над столбцом.
                chartCanvas.Children.Add(tbCount); // Добавляет текст на канвас.
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e) // Обработчик кнопки «Сохранить статистику».
        {
            try
            {
                // Сохраним статистику в JSON
                var fileName = $"stats_{DateTime.Now:yyyyMMdd_HHmmss}.json"; // Формирует имя файла с текущей меткой времени.
                var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName); // Формирует полный путь к файлу.

                // Формируем анонимный объект
                var obj = new
                {
                    TotalSold = _statistics.GetTotalSoldTickets(), // Общее количество проданных билетов.
                    PrizeFund = _statistics.GetPrizeFund(), // Сумма призового фонда.
                    PerParticipant = _statistics.GetTicketsPerParticipant().ToList() // Список (участник, количество билетов).
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented); // Сериализует анонимный объект в форматированный JSON.
                File.WriteAllText(path, json); // Записывает JSON в файл.

                MessageBox.Show($"Статистика сохранена в файл {fileName}"); // Выводит сообщение об успешном сохранении.
            }
            catch (Exception ex) // Обработка возможных исключений при сохранении.
            {
                MessageBox.Show($"Не удалось сохранить статистику: {ex.Message}"); // Выводит сообщение об ошибке.
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) // Обработчик кнопки «Закрыть».
        {
            Close(); // Закрывает окно.
        }
    }
}