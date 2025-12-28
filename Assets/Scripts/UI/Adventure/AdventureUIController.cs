using System.Collections.Generic;
using Core.Common;
using Core.Common.Messaging;
using OneTripMover.Input;

namespace UI.Adventure
{
    public class AdventureUIController
    {
        private readonly Dictionary<string, IAdventureUIView> _views = new();
        private readonly IPublisher<PlayerInputEnableEvent> _playerInputPublisher;
        private readonly OneTripMover.Input.IInputContextUseCase _inputContextUseCase;

        private string[] _lines;
        private int _index;
        private bool _isPlaying;
        private IAdventureUIView _currentView;
        private System.Action _onFinished;

        public bool IsPlaying => _isPlaying;

        public AdventureUIController()
        {
            var views = ServiceLocator.Resolve<IAdventureUIView[]>() ?? System.Array.Empty<IAdventureUIView>();
            foreach (var v in views)
            {
                if (v == null || string.IsNullOrEmpty(v.Key)) continue;
                _views[v.Key] = v;
            }

            _playerInputPublisher = ServiceLocator.Resolve<IPublisher<PlayerInputEnableEvent>>();
            _inputContextUseCase = ServiceLocator.Resolve<OneTripMover.Input.IInputContextUseCase>();
        }

        public void Play(string[] lines, string viewKey = null, System.Action onFinished = null)
        {
            if (lines == null || lines.Length == 0) return;
            _lines = lines;
            _index = 0;
            _onFinished = onFinished;
            _isPlaying = true;
            _currentView = ResolveView(viewKey) ?? ResolveView("default");

            _playerInputPublisher?.Publish(new PlayerInputEnableEvent { Enabled = false });
            _inputContextUseCase?.SetUIContext();
            _currentView?.ClearView();
            ShowCurrent();
        }

        public void OnAdvance()
        {
            if (!_isPlaying || _currentView == null) return;

            if (_currentView.IsAnimating)
            {
                _currentView.SkipAnimation();
                return;
            }

            _currentView.ExitCurrentItem();
            
            _index++;
            if (_index >= _lines.Length)
            {
                Finish();
            }
            else
            {
                ShowCurrent();
            }
        }

        public void OnPlayRequested(Core.Adventure.AdventurePlayEvent evt)
        {
            if (evt?.Lines == null) return;
            var array = new List<string>(evt.Lines).ToArray();
            Play(array, evt.ViewKey, evt.OnFinished);
        }

        public void OnPlayScriptRequested(Core.Adventure.AdventurePlayScriptEvent evt)
        {
            if (evt?.Script == null || evt.Script.Lines == null) return;
            var array = new List<string>(evt.Script.Lines).ToArray();
            Play(array, evt.ViewKey ?? evt.Script.ViewKey, evt.OnFinished);
        }

        private void ShowCurrent()
        {
            if (_currentView == null || _lines == null || _index >= _lines.Length) return;
            _currentView.ShowLine(_lines[_index], null);
        }

        private void Finish()
        {
            _isPlaying = false;
            _onFinished?.Invoke();
            _onFinished = null;
        }

        private IAdventureUIView ResolveView(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            return _views.TryGetValue(key, out var view) ? view : null;
        }
    }
}
