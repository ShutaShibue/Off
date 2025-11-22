namespace CardSimulation;

class Program
{
    static void Main(string[] args)
    {
        const int simulationCount = 100000;
        var payouts = new List<int>();
        Random random = new Random();

        // 100000セットのシミュレーションを実行
        for (int simulation = 0; simulation < simulationCount; simulation++)
        {
            int totalPayout = RunSimulation(random);
            payouts.Add(totalPayout);
        }

        // 平均と標準偏差を計算
        double average = payouts.Average();
        double variance = payouts.Select(p => Math.Pow(p - average, 2)).Average();
        double standardDeviation = Math.Sqrt(variance);

        // 結果を表示
        Console.WriteLine($"シミュレーション回数: {simulationCount}");
        Console.WriteLine($"総払い出し額の平均: {average:F2}");
        Console.WriteLine($"総払い出し額の標準偏差: {standardDeviation:F2}");
    }

    /// <summary>
    /// 1回のシミュレーションを実行し、総払い出し額を返す
    /// </summary>
    static int RunSimulation(Random random)
    {
        // プレイヤーを15人作成してリストにする
        var players = new List<Player>();
        for (int i = 0; i < 15; i++)
        {
            players.Add(new Player());
        }

        // それぞれに青3枚、紫1枚を付与する
        foreach (var player in players)
        {
            for (int i = 0; i < 3; i++)
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

        // それ以外（4-14）に金（GoldCard）を1枚ずつ付与
        for (int i = 4; i < 15; i++)
        {
            players[i].AddCard(new GoldCard());
        }

        // ラウンドを6回実行
        for (int round = 1; round <= 6; round++)
        {
            // ランダムに2プレイヤーを選び、swap(user1, user2, false)を行う
            int player1Index = random.Next(players.Count);
            int player2Index = random.Next(players.Count);
            while (player2Index == player1Index)
            {
                player2Index = random.Next(players.Count);
            }
            SwapHelper.Swap(players[player1Index], players[player2Index], false);

            // iteration(24): ランダムに2プレイヤーを選び、swap(user1, user2, true)を行う
            for (int iteration = 1; iteration <= 24; iteration++)
            {
                int user1Index = random.Next(players.Count);
                int user2Index = random.Next(players.Count);
                while (user2Index == user1Index)
                {
                    user2Index = random.Next(players.Count);
                }
                SwapHelper.Swap(players[user1Index], players[user2Index], true);
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

        // 総払い出し額を計算して返す
        return players.Sum(p => p.TotalPrizeMoney);
    }
}
