using System.IO;
using Model.Data.Abstract;
using Newtonsoft.Json;

namespace Model.Data.Services
{
    public class JsonDataSerializer : DataSerializer // Объявление публичного класса JsonDataSerializer, наследующего DataSerializer.
    {
        public override void Serialize<T>(T data, string filePath) where T : class // Переопределение метода Serialize для JSON сериализации.
        {
            if (data == null) throw new ArgumentNullException(nameof(data)); // Проверка, что data не равна null.
            try
            {
                string json = JsonConvert.SerializeObject(data); // Сериализация объекта data в JSON-строку.
                File.WriteAllText(filePath, json); // Запись JSON-строки в указанный файл.
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка сериализации в JSON", ex); // Бросок нового исключения с информацией об ошибке.
            }
        }
        public override T Deserialize<T>(string filePath) where T : class // Переопределение метода Deserialize для JSON десериализации.
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("Файл не найден", filePath); // Проверка, что файл существует.
            try
            {
                string json = File.ReadAllText(filePath); // Чтение содержимого файла в строку.
                return JsonConvert.DeserializeObject<T>(json); // Десериализация JSON-строки в объект типа T.
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка десериализации из JSON", ex);
            }
        }
        public override void Serialize<T>(T data, string filePath, bool indent) where T : class  // Переопределение метода Serialize с параметром indent для JSON.
        {
            if (data == null) throw new ArgumentNullException(nameof(data)); // Проверка, что data не равна null.
            try
            {
                var formatting = indent ? Formatting.Indented : Formatting.None; // Выбор типа форматирования в зависимости от значения indent.
                string json = JsonConvert.SerializeObject(data, formatting); // Сериализация объекта data в JSON-строку с нужным форматированием.
                File.WriteAllText(filePath, json); // Запись JSON-строки в указанный файл.
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка сериализации в JSON", ex);
            }
        }
    }
}