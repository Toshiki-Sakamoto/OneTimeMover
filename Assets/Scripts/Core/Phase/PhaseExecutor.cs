using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common;
using Core.Game;

namespace Core.Phase
{
    /// <summary>
    /// 汎用フェーズ実行クラス。登録済みハンドラを順に呼び出す。
    /// </summary>
    public abstract class PhaseExecutor<TPhase, THandler> : IPhaseExecutor<TPhase>
        where TPhase : struct, Enum
        where THandler : IPhaseHandler<TPhase>
    {
        private readonly IReadOnlyList<THandler> _handlers;

        protected PhaseExecutor()
        {
            // Singleton登録されているハンドラを全取得
            var handlers = ServiceLocator.Resolve<THandler[]>() ?? Array.Empty<THandler>();
            _handlers = handlers.OrderBy(x => x.Priority).ToList();
        }

        public abstract TPhase Phase { get; }

        public void Enter(IPhaseContext<TPhase> context)
        {
            foreach (var h in _handlers)
            {
                h.OnEnter(context);
                if (context.HasPendingChange) break;
            }
        }

        public void Update(IPhaseContext<TPhase> context, float deltaTime)
        {
            foreach (var h in _handlers)
            {
                h.OnUpdate(context, deltaTime);
                if (context.HasPendingChange) break;
            }
        }

        public void Exit(IPhaseContext<TPhase> context)
        {
            foreach (var h in _handlers)
            {
                h.OnExit(context);
                if (context.HasPendingChange) break;
            }
        }
    }
}
