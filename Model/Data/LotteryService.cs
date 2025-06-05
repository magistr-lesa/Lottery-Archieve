using Model.Core;
using Model.Data.Abstract;

namespace Model.Data
{
    public class LotteryService // Объявление публичного класса LotteryService.
    {
        private readonly DataSerializer _serializer; // Приватное неизменяемое поле для хранения ссылки на сериализатор.
        public LotteryService(DataSerializer serializer) // Конструктор класса LotteryService с параметром serializer.
        {
            _serializer = serializer; // Присваивание переданного сериализатора полю _serializer.
        }
        public void SaveLottery(Lottery lottery, string filePath) // Объявление метода SaveLottery для сохранения объекта Lottery в файл.
        {
            _serializer.Serialize(lottery, filePath); // Вызов метода Serialize у сериализатора для записи лотереи в файл.
        }
        public Lottery LoadLottery(string filePath) // Объявление метода LoadLottery для загрузки объекта Lottery из файла.
        {
            return _serializer.Deserialize<Lottery>(filePath); // Вызов метода Deserialize у сериализатора и возврат результата.
        }
    }
}