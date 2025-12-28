using System;

namespace UI.Adventure
{
    public interface IAdventureUIView
    {
        string Key { get; }
        bool IsAnimating { get; }
        void ShowLine(string text, Action onCompleted);
        void SkipAnimation();
        void ClearView();
        void ExitCurrentItem();
        void Complete();
    }
}
