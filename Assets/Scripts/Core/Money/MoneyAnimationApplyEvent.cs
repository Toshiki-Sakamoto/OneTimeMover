namespace Core.Money
{
    /// <summary>
    /// アニメ演出中に所持金表示を更新するタイミングを通知するイベント。
    /// </summary>
    public class MoneyAnimationApplyEvent
    {
        public int Current;
        public int Delta;
    }
}
