namespace Model.Data
{
    public interface IDataSerializer  // Объявление публичного интерфейса IDataSerializer.
    {
        void Serialize<T>(T data, string filePath) where T : class; // Объявление метода Serialize для сериализации данных в файл.
        T Deserialize<T>(string filePath) where T : class; // Объявление метода Deserialize для десериализации данных из файла.
        void Serialize<T>(T data, string filePath, bool indent) where T : class; // Объявление перегруженного метода Serialize с параметром indent.
    }
}