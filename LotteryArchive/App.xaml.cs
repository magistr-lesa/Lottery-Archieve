using System.Windows;

namespace LotteryArchive
{
    public partial class App : Application // Главный класс приложения.
    {
        protected override void OnStartup(StartupEventArgs e) // Переопределённый метод запуска приложения.
        {
            base.OnStartup(e); // Вызывает реализацию базового класса.
            // Можно инициализировать глобальные сервисы, DI-контейнер и т. п.
        }
    }
}