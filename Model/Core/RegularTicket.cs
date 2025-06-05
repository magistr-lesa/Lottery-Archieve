namespace Model.Core.Concrete
{
    public class RegularTicket : LotteryTicket // Объявление публичного класса RegularTicket, наследующего от LotteryTicket.
    {
        public RegularTicket(int number) : base(number, DateTime.Now, price: 1m) // Конструктор RegularTicket, вызывающий базовый конструктор с номером, текущей датой и ценой 1.
        {
        }
    }
}