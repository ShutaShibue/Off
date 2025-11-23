namespace CardSimulation;

class Program
{
    static void Main(string[] args)
    {
        const int simulationCount = 10000;
        var payouts = new List<int>();
        Random random = new Random();

        // 10000セットのシミュレーションを実行
        for (int simulation = 0; simulation < simulationCount; simulation++)
        {
            int totalPayout = RunLotterySimulation(random);
            payouts.Add(totalPayout);
        }

        // 平均と標準偏差を計算
        double average = payouts.Average();
        double variance = payouts.Select(p => Math.Pow(p - average, 2)).Average();
        double standardDeviation = Math.Sqrt(variance);

        // 結果を表示
        Console.WriteLine($"シミュレーション回数: {simulationCount}");
        Console.WriteLine($"総合支払額の平均: {average:F2}");
        Console.WriteLine($"総合支払額の標準偏差: {standardDeviation:F2}");
    }

    /// <summary>
    /// 1回のLotteryシミュレーションを実行し、総合支払額を返す
    /// </summary>
    static int RunLotterySimulation(Random random)
    {
        Lottery lottery = new Lottery();
        int totalPayout = 0;

        // 最初の2枚は、引いた額の4倍を計上
        for (int i = 0; i < 3; i++)
        {
            Ticket? ticket = lottery.DrawTicket();
            if (ticket != null)
            {
                totalPayout += ticket.Value * 4;
            }
        }

        // 次の4枚は、引いた額の2倍を計上
        for (int i = 0; i < 4; i++)
        {
            Ticket? ticket = lottery.DrawTicket();
            if (ticket != null)
            {
                totalPayout += ticket.Value * 2;
            }
        }

        // その後、40枚引いて1倍計上
        for (int i = 0; i < 40; i++)
        {
            Ticket? ticket = lottery.DrawTicket();
            if (ticket != null)
            {
                totalPayout += ticket.Value;
            }
        }

        return totalPayout;
    }
}
