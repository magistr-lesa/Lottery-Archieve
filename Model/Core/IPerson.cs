namespace Model.Core
{
    public interface IPerson  // Объявление публичного интерфейса IPerson.
    {
        string FirstName { get; } // Определение свойства FirstName только для чтения.
        string LastName { get; } // Определение свойства LastName только для чтения.
        string MiddleName { get; } // Определение свойства MiddleName только для чтения.
        void ChangeName(string firstName, string lastName, string middleName); // Объявление метода ChangeName с тремя параметрами.
    }
}