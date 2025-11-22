namespace CardSimulation;

/// <summary>
/// 抽選を表すクラス
/// </summary>
public class Lottery
{
    private readonly List<Ticket> _tickets;
    private readonly Random _random;

    /// <summary>
    /// 残りのクジの数
    /// </summary>
    public int RemainingTickets => _tickets.Count;

    /// <summary>
    /// Lotteryクラスのコンストラクタ
    /// 1等を5枚、2等を15枚、3等を30枚持つ
    /// </summary>
    public Lottery()
    {
        _tickets = new List<Ticket>();
        _random = new Random();

        // 1等を5枚追加
        for (int i = 0; i < 5; i++)
        {
            _tickets.Add(new Ticket(TicketRank.First));
        }

        // 2等を15枚追加
        for (int i = 0; i < 15; i++)
        {
            _tickets.Add(new Ticket(TicketRank.Second));
        }

        // 3等を30枚追加
        for (int i = 0; i < 30; i++)
        {
            _tickets.Add(new Ticket(TicketRank.Third));
        }
    }

    /// <summary>
    /// ランダムにくじを1本引く
    /// </summary>
    /// <returns>引いたクジ。残りがない場合はnull</returns>
    public Ticket? DrawTicket()
    {
        if (_tickets.Count == 0)
        {
            return null;
        }

        int randomIndex = _random.Next(_tickets.Count);
        Ticket drawnTicket = _tickets[randomIndex];
        _tickets.RemoveAt(randomIndex);
        return drawnTicket;
    }
}

