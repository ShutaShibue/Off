namespace CardSimulation;

/// <summary>
/// クジの等級を表す列挙型
/// </summary>
public enum TicketRank
{
    /// <summary>
    /// 3等
    /// </summary>
    Third = 3,

    /// <summary>
    /// 2等
    /// </summary>
    Second = 2,

    /// <summary>
    /// 1等
    /// </summary>
    First = 1
}

/// <summary>
/// クジを表すクラス
/// </summary>
public class Ticket
{
    /// <summary>
    /// クジの等級
    /// </summary>
    public TicketRank Rank { get; private set; }

    /// <summary>
    /// 賞金
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// Ticketクラスのコンストラクタ
    /// </summary>
    /// <param name="rank">クジの等級</param>
    public Ticket(TicketRank rank)
    {
        Rank = rank;
        Value = rank switch
        {
            TicketRank.First => 5000,
            TicketRank.Second => 2000,
            TicketRank.Third => 1000,
            _ => 0
        };
    }
}

