namespace CardSimulation;

class Program
{
    static void Main(string[] args)
    {
        const int simulationCount = 100000;
        var payouts = new List<int>();
        var blueCardCounts = new List<int>();
        Random random = new Random();

        // 100000セットのシミュレーションを実行
        for (int simulation = 0; simulation < simulationCount; simulation++)
        {
            var (totalPayout, blueCardCount) = RunSimulation(random);
            payouts.Add(totalPayout);
            blueCardCounts.Add(blueCardCount);
        }

        // 平均と標準偏差を計算
        double averagePayout = payouts.Average();
        double variancePayout = payouts.Select(p => Math.Pow(p - averagePayout, 2)).Average();
        double standardDeviationPayout = Math.Sqrt(variancePayout);
        
        double averageBlueCards = blueCardCounts.Average();

        // 結果を表示
        Console.WriteLine($"シミュレーション回数: {simulationCount}");
        Console.WriteLine($"総払い出し額の平均: {averagePayout:F2}");
        Console.WriteLine($"総払い出し額の標準偏差: {standardDeviationPayout:F2}");
        Console.WriteLine($"平均青払い出し数: {averageBlueCards:F2}");
    }

    /// <summary>
    /// 1回のシミュレーションを実行し、総払い出し額とBlueCard作成数を返す
    /// </summary>
    static (int totalPayout, int blueCardCount) RunSimulation(Random random)
    {
        // プレイヤーを20人作成してリストにする
        var players = new List<Player>();
        for (int i = 0; i < 20; i++)
        {
            players.Add(new Player());
        }

        // それぞれに青3枚、紫1枚を付与する
        foreach (var player in players)
        {
            for (int i = 0; i < 2; i++)
            {
                player.AddCard(new BlueCard());
            }
            player.AddCard(new PurpleCard());
        }

        // プレイヤー0,1に虹を1枚ずつ付与
        players[0].AddCard(new RainbowCard());
        players[1].AddCard(new RainbowCard());

        // プレイヤー2,3に銀を1枚ずつ付与
        players[2].AddCard(new SilverCard());
        players[3].AddCard(new SilverCard());

        // それ以外（4-19）に金（GoldCard）を1枚ずつ付与
        for (int i = 4; i < 20; i++)
        {
            players[i].AddCard(new GoldCard());
        }

        int totalBlueCardsCreated = 0;

        // ラウンドを6回実行
        for (int round = 1; round <= 6; round++)
        {
            // iteration(24): ランダムに2プレイヤーを選び、swap(user1, user2, true)を行う
            for (int iteration = 1; iteration <= 18; iteration++)
            {
                int user1Index = random.Next(players.Count);
                int user2Index = random.Next(players.Count);
                while (user2Index == user1Index)
                {
                    user2Index = random.Next(players.Count);
                }
                int blueCardCount = SwapHelper.Swap(players[user1Index], players[user2Index], true);
                totalBlueCardsCreated += blueCardCount;
            }
        }

        // 各プレイヤーごとに最終スコアを計算
        for (int i = 0; i < players.Count; i++)
        {
            players[i].CalculateScore();
        }

        // 抽選を実行
        Lottery lottery = new Lottery();

        // 各プレイヤーが抽選を実行
        for (int i = 0; i < players.Count; i++)
        {
            players[i].DrawLottery(lottery, players[i].Raffles, players[i].Options);
        }

        // 総払い出し額とBlueCard作成数を返す
        int totalPayout = players.Sum(p => p.TotalPrizeMoney);
        return (totalPayout, totalBlueCardsCreated);
    }
}
