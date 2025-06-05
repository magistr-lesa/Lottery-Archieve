using Model.Core.Concrete;

namespace Model.Core
{
    public partial class Lottery : IStatistics // Объявление публичного частичного класса Lottery, реализующего IStatistics.
    {
        private readonly string _name; // Приватное неизменяемое поле для хранения названия лотереи.
        private readonly int _totalTickets; // Приватное неизменяемое поле для хранения общего количества билетов.
        private decimal _prizeFund; // Приватное поле для текущего призового фонда.
        private readonly LotteryParticipant[] _participants; // Приватное неизменяемое поле-массив для хранения участников.
        private int _participantsCount; // Приватное поле для хранения текущего числа участников.
        private int _soldTicketsCount; // Приватное поле для хранения числа проданных билетов.
        public string Name => _name; // Публичное свойство Name возвращает значение поля _name.
        public int TotalTickets => _totalTickets; // Публичное свойство TotalTickets возвращает значение поля _totalTickets.
        public decimal PrizeFund => _prizeFund; // Публичное свойство PrizeFund возвращает значение поля _prizeFund.
        public int ParticipantsCount => _participantsCount; // Публичное свойство ParticipantsCount возвращает значение поля _participantsCount.
        public int SoldTicketsCount => _soldTicketsCount; // Публичное свойство SoldTicketsCount возвращает значение поля _soldTicketsCount.
        public delegate void LotteryResultHandler(string message); // Объявление делегата LotteryResultHandler с одним параметром message.
        public Lottery(string name, int totalTickets, decimal prizeFund, int maxParticipants) // Конструктор класса Lottery с четырьмя параметрами.
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Название лотереи не может быть пустым"); // Проверка, что имя лотереи не пустое.
            if (totalTickets <= 0) throw new ArgumentOutOfRangeException(nameof(totalTickets)); // Проверка, что общее число билетов положительное.
            if (prizeFund < 0) throw new ArgumentOutOfRangeException(nameof(prizeFund)); // Проверка, что начальный призовой фонд не отрицательный.
            if (maxParticipants <= 0) throw new ArgumentOutOfRangeException(nameof(maxParticipants)); // Проверка, что максимальное число участников положительное.
            _name = name; // Присваивание значения параметра name полю _name.
            _totalTickets = totalTickets; // Присваивание значения параметра totalTickets полю _totalTickets.
            _prizeFund = prizeFund; // Присваивание значения параметра prizeFund полю _prizeFund.
            _participants = new LotteryParticipant[maxParticipants]; // Создание массива участников заданного размера maxParticipants.
            _participantsCount = 0; // Инициализация счётчика участников нулём.
            _soldTicketsCount = 0; // Инициализация счётчика проданных билетов нулём.
        }
        public bool AddParticipant(LotteryParticipant participant) // Объявление метода AddParticipant для добавления нового участника.
        {
            if (_participantsCount >= _participants.Length) return false; // Если число участников достигло максимума, вернуть false.
            _participants[_participantsCount++] = participant; // Добавление участника в массив и увеличение счётчика участников.
            return true; // Возвращение true после успешного добавления участника.
        }
        public bool SellTicket(LotteryParticipant participant, LotteryTicket ticket) // Объявление метода SellTicket для продажи билета участнику.
        {
            if (participant == null || ticket == null) return false; // Если участник или билет равны null, вернуть false.
            if (_soldTicketsCount >= _totalTickets) return false; // Если все билеты распроданы, вернуть false.
            if (participant.TrySpendMoney(ticket.Price)) // Если участник может потратить деньги на билет:
            {
                _prizeFund += ticket.Price; // Увеличение призового фонда на стоимость билета.
                _soldTicketsCount++; // Увеличение общего числа проданных билетов.
                return participant.AddTicket(ticket); // Добавление билета участнику и возврат результата.
            }
            return false; // Если участник не смог потратить деньги, вернуть false.
        }
        public bool ConductLottery(LotteryResultHandler resultHandler, out WinningTicket winningTicket) // Объявление метода ConductLottery с делегатом и выходным параметром winningTicket.
        {
            winningTicket = null; // Инициализация выходного параметра null.
            if ((float)_soldTicketsCount / _totalTickets < 0.25f) // Если продано менее 25% всех билетов:
            {
                foreach (var participant in _participants) // Проход по всем участникам.
                {
                    if (participant != null) // Если участник не равен null:
                    {
                        participant.AddMoney(_prizeFund * 0.9m / _participantsCount); // Возврат 90% призового фонда каждому участнику.
                    }
                }
                resultHandler?.Invoke("Лотерея отменена. Продано менее 25% билетов."); // Вызов делегата с сообщением об отмене лотереи.
                return false; // Возвращение false при отмене лотереи.
            }
            var random = new Random(); // Создание экземпляра Random для выбора случайных значений.
            var winnerIndex = random.Next(_participantsCount); // Генерация случайного индекса победителя.
            var winner = _participants[winnerIndex]; // Получение победителя по сгенерированному индексу.
            var winningTicketNumber = random.Next(1, _totalTickets + 1); // Генерация случайного номера выигрышного билета.
            winningTicket = new WinningTicket(winningTicketNumber, DateTime.Now, 0, _prizeFund); // Создание объекта WinningTicket с рассчитанными параметрами.
            winner.AddTicket(winningTicket); // Добавление выигрышного билета победителю.
            resultHandler?.Invoke($"Победитель: {winner}. Выигрыш: {_prizeFund:C}"); // Вызов делегата с сообщением о победителе и размере выигрыша.
            return true; // Возвращение true при успешном проведении лотереи.
        }
        public WinningTicket ConductLottery() // Объявление перегруженного метода ConductLottery без делегата.
        {
            ConductLottery(null, out WinningTicket ticket); // Вызов основной версии метода ConductLottery без делегата.
            return ticket; // Возвращение выигрышного билета или null, если розыгрыш был отменён.
        }
        public int GetTotalSoldTickets() => _soldTicketsCount; // Реализация метода GetTotalSoldTickets: возвращает число проданных билетов.
        public IEnumerable<(string ParticipantName, int TicketsCount)> GetTicketsPerParticipant() // Объявление метода GetTicketsPerParticipant.
        {
            var list = new List<(string, int)>(); // Создание локального списка для хранения кортежей (имя участника, число билетов).
            for (int i = 0; i < _participantsCount; i++)  // Цикл по индексам добавленных участников.
            {
                var p = _participants[i]; // Получение участника по индексу.
                if (p != null) // Проверка, что участник не равен null.
                {
                    list.Add((p.ToString(), p.TicketsCount)); // Добавление кортежа с именем и числом купленных билетов в список.
                }
            }
            return list; // Возвращение сформированного списка.
        }
        public decimal GetPrizeFund() => _prizeFund; // Реализация метода GetPrizeFund: возвращает текущий призовой фонд.
    }
}