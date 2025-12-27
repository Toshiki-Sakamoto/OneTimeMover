using System;
using System.Collections.Generic;
using Core.Common;
using Core.Game;

namespace Core.Phase
{
    /// <summary>
    /// フェーズ遷移とライフサイクルの仲介。Stateパターン相当。
    /// </summary>
    public abstract class PhaseService<TPhase> : IPhaseService<TPhase> where TPhase : struct, Enum
    {
        private readonly Dictionary<TPhase, IPhaseExecutor<TPhase>> _executors;
        private IPhaseExecutor<TPhase> _current;
        private readonly PhaseContext _context;

        public TPhase Current => _context.Current;

        protected PhaseService()
        {
            _context = new PhaseContext();
            _executors = new Dictionary<TPhase, IPhaseExecutor<TPhase>>();
        }

        public void Initialize()
        {
            // RegisterされたExecutorをまとめて取得
            var execs = ServiceLocator.Resolve<IPhaseExecutor<TPhase>[]>();
            foreach (var exec in execs)
            {
                if (exec == null) continue;
                var phase = exec.Phase;
                
                _executors.TryAdd(phase, exec);
            }
        }

        public void ChangePhase(TPhase next)
        {
            if (_current != null && EqualityComparer<TPhase>.Default.Equals(_current.Phase, next))
            {
                _context.ClearPending();
                return;
            }

            // 現在フェーズをExit
            _current?.Exit(_context);

            // 次をセット
            if (_executors.TryGetValue(next, out var exec))
            {
                _current = exec;
                _context.SetCurrent(next);
                _current.Enter(_context);
            }
            else
            {
                _current = null;
                _context.SetCurrent(default);
            }

            ProcessPending();
        }

        public void Update(float deltaTime)
        {
            _current?.Update(_context, deltaTime);
            ProcessPending();
        }
        
        private void ProcessPending()
        {
            while (_context.TryConsumePending(out var next))
            {
                // Exit 現在 → 次のEnter
                _current?.Exit(_context);

                if (_executors.TryGetValue(next, out var exec))
                {
                    _current = exec;
                    _context.SetCurrent(next);
                    _current.Enter(_context);
                }
                else
                {
                    _current = null;
                    _context.SetCurrent(default);
                }
            }
        }

        private class PhaseContext : IPhaseContext<TPhase>
        {
            public TPhase Current { get; private set; }

            private bool _hasPending;
            private TPhase _pending;

            public bool HasPendingChange => _hasPending;

            public void RequestChange(TPhase next)
            {
                _hasPending = true;
                _pending = next;
            }

            public bool TryConsumePending(out TPhase next)
            {
                if (!_hasPending)
                {
                    next = default;
                    return false;
                }

                _hasPending = false;
                next = _pending;
                return true;
            }

            public void SetCurrent(TPhase phase)
            {
                Current = phase;
            }

            public void ClearPending()
            {
                _hasPending = false;
            }
        }
    }
}
