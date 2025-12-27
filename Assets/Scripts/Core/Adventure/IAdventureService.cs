using System.Collections.Generic;

namespace Core.Adventure
{
    public interface IAdventureService
    {
        void Play(AdvText scriptable, string viewKey = null, System.Action onFinished = null);
        void Play(IEnumerable<string> lines, string viewKey = null, System.Action onFinished = null);
        bool IsPlaying { get; }
    }
}
