namespace Model.Core
{
    public class WinningTicket : LotteryTicket // Объявление публичного класса WinningTicket, наследующего от LotteryTicket.
    {
        private decimal _prizeAmount; // Приватное поле для хранения суммы приза.
        public decimal PrizeAmount { get => _prizeAmount; private set => _prizeAmount = value >= 0 ? value : throw new ArgumentOutOfRangeException(); } // Публичное свойство PrizeAmount с проверкой неотрицательности.
        public WinningTicket(int number, DateTime purchaseDate, decimal price, decimal prizeAmount) : base(number, purchaseDate, price)
        { // Конструктор WinningTicket, вызывающий базовый конструктор.
            PrizeAmount = prizeAmount; // Установка поля _prizeAmount через свойство PrizeAmount.
        }
    }
}