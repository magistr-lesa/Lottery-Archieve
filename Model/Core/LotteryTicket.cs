namespace Model.Core
{
    public abstract partial class LotteryTicket // Объявление публичного абстрактного частичного класса LotteryTicket.
    {
        private readonly int _number; // Приватное неизменяемое поле для хранения номера билета.
        private readonly DateTime _purchaseDate; // Приватное неизменяемое поле для хранения даты покупки билета.
        private decimal _price; // Приватное поле для хранения цены билета.
        public int Number => _number; // Публичное свойство Number возвращает значение поля _number.
        public DateTime PurchaseDate => _purchaseDate; // Публичное свойство PurchaseDate возвращает значение поля _purchaseDate.
        public decimal Price { get => _price; protected set => _price = value >= 0 ? value : throw new ArgumentOutOfRangeException(); } // Публичное свойство Price с проверкой неотрицательности.
        protected LotteryTicket(int number, DateTime purchaseDate, decimal price) // Защищённый конструктор класса LotteryTicket с тремя параметрами.
        {
            _number = number; // Присваивание параметра number полю _number.
            _purchaseDate = purchaseDate; // Присваивание параметра purchaseDate полю _purchaseDate.
            Price = price; // Установка свойства Price с передачей параметра price.
        }
        public override bool Equals(object obj) => obj is LotteryTicket ticket && _number == ticket._number; // Переопределение метода Equals на сравнение по номеру билета.
        public static bool operator ==(LotteryTicket left, LotteryTicket right) => ReferenceEquals(left, right) || (left?.Equals(right) ?? false); // Перегрузка оператора == для сравнения билетов.
        public static bool operator !=(LotteryTicket left, LotteryTicket right) => !(left == right); // Перегрузка оператора != для сравнения билетов.
        public override int GetHashCode() => _number.GetHashCode(); // Переопределение метода GetHashCode, возвращающее хэш кода номера билета.
    }
}