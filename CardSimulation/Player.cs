namespace CardSimulation;

/// <summary>
/// プレイヤーを表すクラス
/// </summary>
public class Player
{
    private readonly List<Card> _cards;
    private readonly Random _random;

    /// <summary>
    /// プレイヤーが持つカードのリスト
    /// </summary>
    public IReadOnlyList<Card> Cards => _cards.AsReadOnly();

    /// <summary>
    /// カードリストが空かどうかを示す値
    /// </summary>
    public bool IsEmpty => _cards.Count == 0;

    /// <summary>
    /// 計算されたraffles値
    /// </summary>
    public int Raffles { get; private set; }

    /// <summary>
    /// 計算されたoptions値
    /// </summary>
    public int Options { get; private set; }

    /// <summary>
    /// 計算されたmultiplier値（全カードのMultiplierの積）
    /// </summary>
    public double Multiplier { get; private set; }

    /// <summary>
    /// 獲得賞金（Multiplier適用前）
    /// </summary>
    public int PrizeMoney { get; private set; }

    /// <summary>
    /// 総獲得金額（Multiplier適用後）
    /// </summary>
    public int TotalPrizeMoney => (int)Math.Floor(PrizeMoney * Multiplier);

    /// <summary>
    /// Playerクラスのコンストラクタ
    /// </summary>
    public Player()
    {
        _cards = new List<Card>();
        _random = new Random();
    }

    /// <summary>
    /// 初期カードリストを指定してPlayerクラスを初期化するコンストラクタ
    /// </summary>
    /// <param name="initialCards">初期カードリスト</param>
    public Player(IEnumerable<Card> initialCards)
    {
        _cards = new List<Card>(initialCards);
        _random = new Random();
    }

    /// <summary>
    /// ランダムなインデックスからカードを削除して返す
    /// リストが空の場合は、リストを変更せずにnullを返す
    /// </summary>
    /// <returns>削除されたカード、またはリストが空の場合はnull</returns>
    public Card? RemoveCard()
    {
        if (_cards.Count == 0)
        {
            return null;
        }

        int randomIndex = _random.Next(_cards.Count);
        Card removedCard = _cards[randomIndex];
        _cards.RemoveAt(randomIndex);
        return removedCard;
    }

    /// <summary>
    /// カードをリストの末尾に追加する
    /// </summary>
    /// <param name="card">追加するカード</param>
    /// <exception cref="ArgumentNullException">cardがnullの場合</exception>
    public void AddCard(Card card)
    {
        if (card == null)
        {
            throw new ArgumentNullException(nameof(card), "カードはnullにできません。");
        }

        _cards.Add(card);
    }

    /// <summary>
    /// 最終スコアを計算する
    /// </summary>
    public void CalculateScore()
    {
        double mult = 1.0;
        double weight = 0.0;
        int cardsWithOptions = 0;

        // 各カードについて、Multiplierをmultに掛け算、Weightをweightに足し算
        foreach (var card in _cards)
        {
            mult *= card.Multiplier;
            weight += card.Weight;
            if (card.Options)
            {
                cardsWithOptions++;
            }
        }

        // 少数誤差を処理するため、許容誤差を設定
        const double tolerance = 0.0001;

        // multiplierを保存
        Multiplier = mult;

        // raffles = weight / 1（Weightは1ごとにRaffles=1換算、少数誤差を考慮）
        Raffles = (int)Math.Floor(weight + tolerance);

        // options = Min(weight % 1, (number of cards with option=true))
        // 換算されなかった余り（weightの小数部分）に関して、Optionが手持ちにある分だけOptionに計上
        double remainder = weight % 1.0;
        // 少数誤差を考慮して、remainderを調整
        if (remainder < 0)
        {
            remainder += 1.0;
        }
        // remainderが1に非常に近い場合は0として扱う（浮動小数点誤差のため）
        if (remainder > 1.0 - tolerance)
        {
            remainder = 0.0;
        }
        // 余りがある場合、余りの値に応じてOptionを計上
        // weightは1/3または1の値の合計なので、余りは0, 1/3, 2/3に近い値になる
        // 余りが1/3（約0.333...）の場合、Optionsを最大1まで
        // 余りが2/3（約0.666...）の場合、Optionsを最大2まで
        int optionsFromWeight = 0;
        const double oneThird = 1.0 / 3.0;
        const double twoThirds = 2.0 / 3.0;
        
        if (remainder > tolerance)
        {
            // 余りが2/3に近い場合（1/3より大きい）
            if (Math.Abs(remainder - twoThirds) < tolerance || remainder > oneThird + tolerance)
            {
                optionsFromWeight = 2;
            }
            // 余りが1/3に近い場合（1/3以下）
            else
            {
                optionsFromWeight = 1;
            }
        }
        
        Options = Math.Min(optionsFromWeight, cardsWithOptions);
    }

    /// <summary>
    /// 抽選を実行する
    /// </summary>
    /// <param name="lottery">抽選クラス</param>
    /// <param name="raffles">抽選回数</param>
    /// <param name="options">使用可能なOptions数</param>
    public void DrawLottery(Lottery lottery, int raffles, int options)
    {
        int usedOptions = 0;
        
        for (int i = 0; i < raffles; i++)
        {
            // Optionがある場合、2本引いて大きい方を採用
            if (usedOptions < options)
            {
                Ticket? ticket1 = lottery.DrawTicket();
                Ticket? ticket2 = lottery.DrawTicket();
                
                if (ticket1 != null && ticket2 != null)
                {
                    // 大きい方の賞金を加算
                    PrizeMoney += Math.Max(ticket1.Value, ticket2.Value);
                    usedOptions++;
                }
                else if (ticket1 != null)
                {
                    PrizeMoney += ticket1.Value;
                    usedOptions++;
                }
                else if (ticket2 != null)
                {
                    PrizeMoney += ticket2.Value;
                    usedOptions++;
                }
            }
            else
            {
                // Optionがない場合、1本引く
                Ticket? ticket = lottery.DrawTicket();
                if (ticket != null)
                {
                    PrizeMoney += ticket.Value;
                }
            }
        }
    }
}

