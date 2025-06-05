namespace Model.Core
{
    public interface IStatistics // Объявление публичного интерфейса IStatistics.
    { 
        int GetTotalSoldTickets(); // Объявление метода GetTotalSoldTickets, возвращающего целое число.
        IEnumerable<(string ParticipantName, int TicketsCount)> GetTicketsPerParticipant(); // Объявление метода GetTicketsPerParticipant, возвращающего перечисление кортежей.
        decimal GetPrizeFund(); // Объявление метода GetPrizeFund, возвращающего decimal.
    }
}