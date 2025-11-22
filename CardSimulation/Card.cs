namespace CardSimulation;

/// <summary>
/// カードを表すクラス
/// </summary>
public class Card
{
    /// <summary>
    /// カードの色
    /// </summary>
    public CardColor Color { get; protected set; }

    /// <summary>
    /// 乗数（0.5, 1, または 2）
    /// </summary>
    public double Multiplier { get; protected set; }

    /// <summary>
    /// 重み（1/3 または 1）
    /// </summary>
    public double Weight { get; protected set; }

    /// <summary>
    /// オプション（真偽値）
    /// </summary>
    public bool Options { get; protected set; }

    /// <summary>
    /// Cardクラスのコンストラクタ
    /// </summary>
    /// <param name="color">カードの色</param>
    /// <param name="multiplier">乗数（0.5, 1, または 2）</param>
    /// <param name="weight">重み（1/3 または 1）</param>
    /// <param name="options">オプション（真偽値）</param>
    /// <exception cref="ArgumentException">multiplierまたはweightが無効な値の場合</exception>
    protected Card(CardColor color, double multiplier, double weight, bool options)
    {
        // multiplierのバリデーション
        if (multiplier != 0.5 && multiplier != 1.0 && multiplier != 2.0)
        {
            throw new ArgumentException("multiplierは0.5, 1, または2である必要があります。", nameof(multiplier));
        }

        // weightのバリデーション（1/3 ≈ 0.333... または 1）
        const double oneThird = 1.0 / 3.0;
        const double tolerance = 0.0001; // 浮動小数点数の比較用
        if (Math.Abs(weight - oneThird) > tolerance && Math.Abs(weight - 1.0) > tolerance)
        {
            throw new ArgumentException("weightは1/3または1である必要があります。", nameof(weight));
        }

        Color = color;
        Multiplier = multiplier;
        Weight = weight;
        Options = options;
    }
}

