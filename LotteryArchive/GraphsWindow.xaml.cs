using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Model.Core.Concrete;
using System.Windows.Controls;

namespace LotteryArchive
{
    /// <summary>
    /// Логика взаимодействия для GraphsWindow.xaml
    /// Отображает 2 столбчатые диаграммы: потрачено и выиграно
    /// </summary>
    public partial class GraphsWindow : Window // Код-бихайнд для GraphsWindow.xaml.
    {
        private readonly List<LotteryParticipant> _participants; // Список участников, для которых строятся графики.

        public GraphsWindow(IEnumerable<LotteryParticipant> participants) // Конструктор, принимающий выбранных участников.
        {
            InitializeComponent(); // Инициализирует компоненты окна.
            _participants = participants.ToList(); // Преобразует входной список в List<LotteryParticipant>.
            DrawSpentChart(); // Вызывает метод для рисования графика «Потрачено».
            DrawWonChart(); // Вызывает метод для рисования графика «Выиграно».
        }

        private void DrawSpentChart() // Метод для построения графика «Потрачено».
        {
            var data = _participants.Select(p => (p.FullName, p.TotalSpent)).ToList(); // Формирует список кортежей (ФИО, сумма потраченного) для всех участников.
            if (!data.Any()) // Проверяет, есть ли данные для графика.
            {
                ShowNoDataMessage(canvasSpent, "Нет данных для графика «Потрачено»."); // Если данных нет, отображает сообщение.
                return; // Выходит из метода.
            }

            double canvasWidth = data.Count * 100; // Вычисляет ширину канваса как количество участников * 100px.
            double canvasHeight = canvasSpent.Height - 40; // Вычисляет высоту области для столбцов, учитывая отступы.
            canvasSpent.Width = Math.Max(canvasWidth, canvasSpent.ActualWidth); // Устанавливает ширину канваса как максимум требуемой и текущей.

            decimal maxValue = data.Max(d => d.TotalSpent); // Находит максимальную потраченную сумму.
            for (int i = 0; i < data.Count; i++) // Проходит по каждому участнику.
            {
                var (name, amount) = data[i]; // Получает имя и сумму потраченного.
                double barHeight = (maxValue == 0) ? 0 : (double)(amount / maxValue) * (canvasHeight - 40); // Вычисляет высоту столбца пропорционально максимальной сумме.

                var rect = new Rectangle // Создаёт прямоугольник для столбца.
                {
                    Width = 50, // Фиксированная ширина столбца.
                    Height = barHeight, // Высота столбца.
                    Fill = Brushes.IndianRed // Цвет столбца (красный).
                };
                Canvas.SetLeft(rect, i * 100 + 20); // Устанавливает горизонтальную позицию столбца.
                Canvas.SetTop(rect, canvasHeight - barHeight + 20); // Устанавливает вертикальную позицию столбца.
                canvasSpent.Children.Add(rect); // Добавляет столбец на канвас.

                var tbName = new System.Windows.Controls.TextBlock // Создаёт текстовый блок для имени участника.
                {
                    Text = name, // Текст — имя участника.
                    Width = 80, // Фиксированная ширина подписи.
                    TextAlignment = TextAlignment.Center, // Выравнивание текста по центру.
                    FontSize = 12 // Размер шрифта.
                };
                Canvas.SetLeft(tbName, i * 100 + 10); // Устанавливает позицию подписи по горизонтали.
                Canvas.SetTop(tbName, canvasHeight + 25); // Устанавливает позицию подписи по вертикали (под столбцом).
                canvasSpent.Children.Add(tbName); // Добавляет подпись на канвас.

                var tbValue = new System.Windows.Controls.TextBlock // Создаёт текстовый блок для значения суммы.
                {
                    Text = amount.ToString("0.##"), // Текст — значение суммы потраченного с форматированием.
                    FontWeight = FontWeights.Bold, // Жирный шрифт для значения.
                    FontSize = 12 // Размер шрифта.
                };
                Canvas.SetLeft(tbValue, i * 100 + 20); // Устанавливает позицию значения по горизонтали.
                Canvas.SetTop(tbValue, canvasHeight - barHeight); // Устанавливает позицию значения над столбцом.
                canvasSpent.Children.Add(tbValue); // Добавляет значение на канвас.
            }
        }

        private void DrawWonChart() // Метод для построения графика «Выиграно».
        {
            var data = _participants.Select(p => (p.FullName, p.TotalWon)).ToList(); // Формирует список кортежей (ФИО, сумма выигранного) для всех участников.
            if (!data.Any()) // Проверяет, есть ли данные для графика.
            {
                ShowNoDataMessage(canvasWon, "Нет данных для графика «Выиграно»."); // Если данных нет, отображает сообщение.
                return; // Выходит из метода.
            }

            double canvasWidth = data.Count * 100; // Вычисляет ширину канваса как количество участников * 100px.
            double canvasHeight = canvasWon.Height - 40; // Вычисляет высоту области для столбцов, учитывая отступы.
            canvasWon.Width = Math.Max(canvasWidth, canvasWon.ActualWidth); // Устанавливает ширину канваса как максимум требуемой и текущей.

            decimal maxValue = data.Max(d => d.TotalWon); // Находит максимальную выигранную сумму.
            for (int i = 0; i < data.Count; i++) // Проходит по каждому участнику.
            {
                var (name, amount) = data[i]; // Получает имя и сумму выигранного.
                double barHeight = (maxValue == 0) ? 0 : (double)(amount / maxValue) * (canvasHeight - 40); // Вычисляет высоту столбца пропорционально максимальной сумме.

                var rect = new Rectangle // Создаёт прямоугольник для столбца.
                {
                    Width = 50, // Фиксированная ширина столбца.
                    Height = barHeight, // Высота столбца.
                    Fill = Brushes.SeaGreen // Цвет столбца (зелёный).
                };
                Canvas.SetLeft(rect, i * 100 + 20); // Устанавливает горизонтальную позицию столбца.
                Canvas.SetTop(rect, canvasHeight - barHeight + 20); // Устанавливает вертикальную позицию столбца.
                canvasWon.Children.Add(rect); // Добавляет столбец на канвас.

                var tbName = new System.Windows.Controls.TextBlock // Создаёт текстовый блок для имени участника.
                {
                    Text = name, // Текст — имя участника.
                    Width = 80, // Фиксированная ширина подписи.
                    TextAlignment = TextAlignment.Center, // Выравнивание текста по центру.
                    FontSize = 12 // Размер шрифта.
                };
                Canvas.SetLeft(tbName, i * 100 + 10); // Устанавливает позицию подписи по горизонтали.
                Canvas.SetTop(tbName, canvasHeight + 25); // Устанавливает позицию подписи по вертикали (под столбцом).
                canvasWon.Children.Add(tbName); // Добавляет подпись на канвас.

                var tbValue = new System.Windows.Controls.TextBlock // Создаёт текстовый блок для значения суммы.
                {
                    Text = amount.ToString("0.##"), // Текст — значение суммы выигранного с форматированием.
                    FontWeight = FontWeights.Bold, // Жирный шрифт для значения.
                    FontSize = 12 // Размер шрифта.
                };
                Canvas.SetLeft(tbValue, i * 100 + 20); // Устанавливает позицию значения по горизонтали.
                Canvas.SetTop(tbValue, canvasHeight - barHeight); // Устанавливает позицию значения над столбцом.
                canvasWon.Children.Add(tbValue); // Добавляет значение на канвас.
            }
        }

        private void ShowNoDataMessage(System.Windows.Controls.Canvas canvas, string message) // Метод для отображения сообщения при отсутствии данных.
        {
            var tb = new System.Windows.Controls.TextBlock // Создаёт текстовый блок для сообщения.
            {
                Text = message, // Текст сообщения.
                FontSize = 16, // Размер шрифта.
                Foreground = Brushes.Gray // Цвет текста (серый).
            };
            Canvas.SetLeft(tb, 10); // Устанавливает горизонтальную позицию сообщения.
            Canvas.SetTop(tb, 10); // Устанавливает вертикальную позицию сообщения.
            canvas.Children.Add(tb); // Добавляет сообщение на канвас.
        }
    }
}
