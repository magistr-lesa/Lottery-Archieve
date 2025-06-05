using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Model.Core
{
    public abstract class PersonBase : IPerson, INotifyPropertyChanged // Объявление публичного абстрактного класса PersonBase, реализующего IPerson и INotifyPropertyChanged.
    {
        private string _firstName; // Приватное поле для хранения имени.
        private string _lastName; // Приватное поле для хранения фамилии.
        private string _middleName; // Приватное поле для хранения отчества.
        public string FirstName => _firstName; // Публичное свойство FirstName возвращает значение поля _firstName.
        public string LastName => _lastName; // Публичное свойство LastName возвращает значение поля _lastName.
        public string MiddleName => _middleName; // Публичное свойство MiddleName возвращает значение поля _middleName.
        protected PersonBase(string firstName, string lastName, string middleName) // Защищённый конструктор класса PersonBase с тремя строковыми параметрами.
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("Имя не может быть пустым"); // Проверка, что имя не пустое.
            _firstName = firstName; // Присваивание параметра firstName полю _firstName.
            _lastName = lastName; // Присваивание параметра lastName полю _lastName.
            _middleName = middleName; // Присваивание параметра middleName полю _middleName.
        }
        public virtual void ChangeName(string firstName, string lastName, string middleName) // Публичный виртуальный метод ChangeName для изменения ФИО.
        {
            _firstName = firstName; // Изменение поля _firstName на переданное значение.
            _lastName = lastName; // Изменение поля _lastName на переданное значение.
            _middleName = middleName; // Изменение поля _middleName на переданное значение.
            OnPropertyChanged(nameof(FirstName)); // Вызов уведомления об изменении свойства FirstName.
            OnPropertyChanged(nameof(LastName)); // Вызов уведомления об изменении свойства LastName.
            OnPropertyChanged(nameof(MiddleName)); // Вызов уведомления об изменении свойства MiddleName.
        }
        public override string ToString() => $"{LastName} {FirstName} {MiddleName}"; // Переопределение метода ToString(), возвращающее строку "Фамилия Имя Отчество".
        public event PropertyChangedEventHandler PropertyChanged; // Объявление события PropertyChanged из интерфейса INotifyPropertyChanged.
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) // Защищённый виртуальный метод OnPropertyChanged с атрибутом CallerMemberName.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Вызов события PropertyChanged с именем изменённого свойства.
        }
    }
}