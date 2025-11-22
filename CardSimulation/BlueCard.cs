namespace CardSimulation;

/// <summary>
/// 青色のカード
/// </summary>
public class BlueCard : Card
{
    /// <summary>
    /// BlueCardクラスのコンストラクタ
    /// </summary>
    public BlueCard() : base(CardColor.Blue, 1.0, 1.0 / 3.0, false)
    {
    }
}

