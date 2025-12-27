using System.Collections.Generic;
using Core.Adventure;
using Core.Common;
using Core.Common.Messaging;

namespace OneTripMover.UseCase
{
    /// <summary>
    /// アドベンチャー再生要求をイベントでUIへ投げるUseCase。
    /// </summary>
    public class AdventureService : IAdventureService
    {
        private readonly IPublisher<AdventurePlayEvent> _playPublisher;

        public bool IsPlaying => false; // 状態はUI側で管理

        public AdventureService()
        {
            _playPublisher = ServiceLocator.Resolve<IPublisher<AdventurePlayEvent>>();
        }

        public void Play(AdvText scriptable, string viewKey = null, System.Action onFinished = null)
        {
            if (scriptable == null || scriptable.Lines == null) return;
            Play(scriptable.Lines, viewKey ?? scriptable.ViewKey, onFinished);
        }

        public void Play(IEnumerable<string> lines, string viewKey = null, System.Action onFinished = null)
        {
            if (lines == null) return;
            var list = new List<string>(lines);
            if (list.Count == 0) return;

            _playPublisher.Publish(new AdventurePlayEvent
            {
                ViewKey = viewKey,
                Lines = list,
                OnFinished = onFinished
            });
        }
    }
}
