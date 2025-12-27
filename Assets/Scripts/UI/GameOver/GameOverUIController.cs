using Core.Common;

namespace UI.GameOver
{
    public interface IGameOverUIController
    {
        void Play();
    }
    
    /// <summary>
    /// 純C#のゲームオーバーUIコントローラ。ビューはDIで解決する。
    /// </summary>
    public class GameOverUIController : IGameOverUIController
    {
        private readonly GameOverUIView _view;

        public GameOverUIController()
        {
            _view = ServiceLocator.Resolve<GameOverUIView>();
        }

        public void Play()
        {
            _view?.Play();
        }
    }
}
