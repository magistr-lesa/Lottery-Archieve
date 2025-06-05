using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Model.Data.Abstract;

namespace Model.Data.Services
{
    public class XmlDataSerializer : DataSerializer  // Объявление публичного класса XmlDataSerializer, наследующего DataSerializer.
    {
        public override void Serialize<T>(T data, string filePath) where T : class // Переопределение метода Serialize для XML сериализации.
        { 
            if (data == null) throw new ArgumentNullException(nameof(data)); // Проверка, что data не равна null.
            try
            { // Начало блока try для обработки исключений при сериализации.
                var xmlSerializer = new XmlSerializer(typeof(T)); // Создание экземпляра XmlSerializer для типа T.
                using var writer = new StreamWriter(filePath, false, Encoding.UTF8); // Создание StreamWriter с кодировкой UTF-8.
                xmlSerializer.Serialize(writer, data); // Сериализация объекта data в XML и запись в файл.
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка сериализации в XML", ex);
            }
        }
        public override T Deserialize<T>(string filePath) where T : class // Переопределение метода Deserialize для XML десериализации.
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("Файл не найден", filePath); // Проверка, что файл существует.
            try
            { // Начало блока try для обработки исключений при десериализации.
                var xmlSerializer = new XmlSerializer(typeof(T)); // Создание экземпляра XmlSerializer для типа T.
                using var reader = new StreamReader(filePath, Encoding.UTF8); // Создание StreamReader с кодировкой UTF-8.
                return (T)xmlSerializer.Deserialize(reader); // Десериализация XML из файла в объект типа T.
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка десериализации из XML", ex);
            }
        }
        public override void Serialize<T>(T data, string filePath, bool indent) where T : class  // Переопределение метода Serialize с параметром indent для XML.
        {
            if (data == null) throw new ArgumentNullException(nameof(data)); // Проверка, что data не равна null.
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T)); // Создание экземпляра XmlSerializer для типа T.
                var settings = new XmlWriterSettings { Indent = indent, Encoding = Encoding.UTF8 }; // Создание настроек XmlWriter с включённым или выключенным отступом.
                using var writer = XmlWriter.Create(filePath, settings); // Создание XmlWriter с указанными настройками.
                xmlSerializer.Serialize(writer, data); // Сериализация объекта data в XML с нужным форматированием.
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка сериализации в XML с отступами", ex);
            }
        }
    }
}