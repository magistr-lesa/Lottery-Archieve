namespace Model.Core.Concrete
{
    public partial class LotteryParticipant : PersonBase // Объявление публичного частичного класса LotteryParticipant, наследующего PersonBase.
    {
        private decimal _balance; // Приватное поле для хранения баланса участника.
        private readonly LotteryTicket[] _tickets; // Приватное неизменяемое поле-массив для хранения билетов участника.
        private int _ticketsCount; // Приватное поле для хранения числа билетов, добавленных участнику.
        private decimal _greed; // Приватное поле для хранения коэффициента жадности участника.
        public decimal Balance => _balance; // Публичное свойство Balance возвращает значение поля _balance.
        public int TicketsCount => _ticketsCount; // Публичное свойство TicketsCount возвращает значение поля _ticketsCount.
        public decimal Greed { get => _greed; set => _greed = (value >= 0 && value <= 1) ? value : throw new ArgumentOutOfRangeException(); } // Публичное свойство Greed с проверкой диапазона 0..1.
        public LotteryParticipant(string firstName, string lastName, string middleName, decimal initialBalance) : base(firstName, lastName, middleName)
        { // Конструктор класса LotteryParticipant с параметрами и вызовом базового конструктора.
            _balance = initialBalance; // Присваивание значения параметра initialBalance полю _balance.
            _tickets = new LotteryTicket[100]; // Создание массива для хранения до 100 билетов.
            _ticketsCount = 0; // Инициализация счётчика билетов нулём.
        }
        public bool AddTicket(LotteryTicket ticket) // Объявление метода AddTicket для добавления билета участнику.
        {
            if (_ticketsCount >= _tickets.Length) return false; // Если число билетов достигло максимума, вернуть false.
            _tickets[_ticketsCount++] = ticket; // Добавление билета в массив и увеличение счётчика.
            OnPropertyChanged(nameof(TicketsCount)); // Вызов уведомления об изменении свойства TicketsCount.
            return true; // Возвращение true после успешного добавления билета.
        }
        public bool TrySpendMoney(decimal amount) // Объявление метода TrySpendMoney для попытки списания денег с баланса участника.
        {
            if (_balance >= amount) // Если баланс участника достаточен:
            {
                _balance -= amount; // Уменьшение баланса на переданную сумму.
                OnPropertyChanged(nameof(Balance)); // Вызов уведомления об изменении свойства Balance.
                return true; // Возвращение true при успешном списании.
            }
            return false; // Если баланс недостаточен, вернуть false.
        }
        public void AddMoney(decimal amount)  // Объявление метода AddMoney для добавления денег на баланс участника.
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(); // Если переданная сумма неположительная, выбросить исключение.
            _balance += amount; // Увеличение баланса на переданную сумму.
            OnPropertyChanged(nameof(Balance)); // Вызов уведомления об изменении свойства Balance.
        }
        private bool _isSelectedForGraph; // Приватное поле для хранения флага выбора участника для графика.
        public bool IsSelectedForGraph { get => _isSelectedForGraph; set { if (_isSelectedForGraph == value) return; _isSelectedForGraph = value; OnPropertyChanged(nameof(IsSelectedForGraph)); } } // Публичное свойство IsSelectedForGraph с уведомлением при изменении.
        public string FullName => base.ToString(); // Публичное свойство FullName возвращает строковое представление ФИО из базового класса.
        public decimal TotalSpent => _tickets.Take(_ticketsCount).Sum(t => t.Price); // Публичное свойство TotalSpent возвращает сумму цен проданных билетов.
        public decimal TotalWon => _tickets.Take(_ticketsCount).OfType<WinningTicket>().Sum(w => w.PrizeAmount); // Публичное свойство TotalWon возвращает сумму выигранных призов.
        public override string ToString() => $"{LastName} {FirstName} {MiddleName}"; // Переопределение метода ToString(), возвращающее ФИО участника.
    }
}