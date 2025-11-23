namespace CardSimulation;

/// <summary>
/// カードのスワップ処理を提供するヘルパークラス
/// </summary>
public static class SwapHelper
{
    private static readonly Random _random = new Random();

    /// <summary>
    /// プレイヤー間でカードをスワップする
    /// </summary>
    /// <param name="playerLose">カードを失うプレイヤー</param>
    /// <param name="playerGet">カードを得るプレイヤー</param>
    /// <param name="probability">確率フラグ（False: 100%実行, True: 50%で0回、33%で1回、17%で2回）</param>
    /// <returns>作成したBlueCardの枚数</returns>
    public static int Swap(Player playerLose, Player playerGet, bool probability)
    {
        int blueCardCount = 0;
        int executionCount = 1;

        // probabilityがTrueの場合、実行回数を決定
        if (probability)
        {
            double randomValue = _random.NextDouble();
            if (randomValue < 0.5)
            {
                // 50%の確率で何もしない
                return 0;
            }
            else if (randomValue < 0.972)
            {
                // 33%の確率で1回実行
                executionCount = 1;
            }
            else
            {
                // 17%の確率で2回実行
                executionCount = 2;
            }
        }

        // 決定された回数分だけ処理を実行
        for (int i = 0; i < executionCount; i++)
        {
            // playerget.IsEmptyの場合、Playerget.AddCard(new BlueCard)
            if (playerGet.IsEmpty)
            {
                playerGet.AddCard(new BlueCard());
                blueCardCount++;
                continue;
            }

            // Playerlose.IsEmptyの場合、playerget.Addcard(new BlueCard)
            if (playerLose.IsEmpty)
            {
                playerGet.AddCard(new BlueCard());
                blueCardCount++;
                continue;
            }

            // else, loseからremove()したものをgetにadd()する
            Card? removedCard = playerLose.RemoveCard();
            if (removedCard != null)
            {
                playerGet.AddCard(removedCard);
            }
        }

        return blueCardCount;
    }
}

