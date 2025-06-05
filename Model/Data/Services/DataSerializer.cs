namespace Model.Data.Abstract
{
    public abstract class DataSerializer : IDataSerializer // Объявление публичного абстрактного класса DataSerializer, реализующего IDataSerializer.
    {
        public abstract void Serialize<T>(T data, string filePath) where T : class; // Абстрактный метод Serialize для сериализации данных типа T в файл.
        public abstract T Deserialize<T>(string filePath) where T : class; // Абстрактный метод Deserialize для десериализации данных типа T из файла.
        public virtual void Serialize<T>(T data, string filePath, bool indent) where T : class  // Виртуальный метод Serialize с параметром indent.
        {
            Serialize(data, filePath); // По умолчанию вызывает обычный метод Serialize без отступов.
        }
    }
}