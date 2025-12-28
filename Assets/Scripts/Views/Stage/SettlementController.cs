using System.Collections;
using System.Collections.Generic;
using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using OneTripMover.Master;
using UnityEngine;
using Views.Cargo;

namespace OneTripMover.Views.Stage
{
    /// <summary>
    /// ゴール後の精算演出を制御
    /// </summary>
    public class SettlementController : MonoBehaviour
    {
        [SerializeField] private float _startIntervalSeconds = 0.7f;
        [SerializeField] private float _moveDuration = 1.5f;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _arcOffset = 2f;
        [SerializeField] private float _arcOffsetVariance = 3f;

        private IPublisher<SettlementFinishedEvent> _settlementFinishedPublisher;
        private IPublisher<SettlementRewardEvent> _settlementRewardPublisher;
        private ICargoViewRegistry _cargoViewRegistry;
        private ICargoMasterRegistry _cargoMasterRegistry;
        private ICargoRegistry _cargoRegistry;
        private GoalController _goalController;
        private List<CargoView> _cachedCargoViews = new();
        private bool _isSettling;

        private void Awake()
        {
            _cargoRegistry = ServiceLocator.Resolve<ICargoRegistry>();
            _goalController = ServiceLocator.Resolve<GoalController>();
            _cargoMasterRegistry = ServiceLocator.Resolve<ICargoMasterRegistry>();
            _settlementFinishedPublisher = ServiceLocator.Resolve<IPublisher<SettlementFinishedEvent>>();
            _settlementRewardPublisher = ServiceLocator.Resolve<IPublisher<SettlementRewardEvent>>();
            _cargoViewRegistry = ServiceLocator.Resolve<ICargoViewRegistry>();

            var clearAnimFinishedSub = ServiceLocator.Resolve<ISubscriber<GoalClearAnimationFinishedEvent>>();
            clearAnimFinishedSub.Subscribe(OnGoalClearAnimationFinished);
        }

        private void OnGoalClearAnimationFinished(GoalClearAnimationFinishedEvent evt)
        {
            if (_isSettling) return;
            _isSettling = true;
            CacheCargoViews();
            StartCoroutine(SettlementRoutine());
        }

        private void CacheCargoViews()
        {
            _cachedCargoViews.Clear();
            if (_cargoViewRegistry != null)
            {
                var views = _cargoViewRegistry.GetCurrentViews();
                if (views != null)
                {
                    _cachedCargoViews.AddRange(views);
                }
            }

            // 上から順に
            _cachedCargoViews.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));
        }

        private IEnumerator SettlementRoutine()
        {
            var target = _goalController.GoalResident.GoalResidentPoint.position;

            foreach (var view in _cachedCargoViews)
            {
                if (view == null) continue;
                StartCoroutine(AnimateCargo(view, target));
                yield return new WaitForSeconds(_startIntervalSeconds);
            }

            // 後続の飛行完了を待つ余裕を少し確保
            yield return new WaitForSeconds(_moveDuration + 0.1f);

            _settlementFinishedPublisher?.Publish(new SettlementFinishedEvent());
        }

        private IEnumerator AnimateCargo(CargoView view, Vector3 targetPos)
        {
            var start = view.transform.position;
            var arcDistance = _arcOffset + (_arcOffsetVariance > 0f ? Random.Range(0f, _arcOffsetVariance) : 0f);
            var control = start + (Vector3.left + Vector3.up) * arcDistance;
            var time = 0f;
            
            var rb = view.GetComponent<Rigidbody2D>();
            var col = view.GetComponent<Collider2D>();
            if (rb)
            {
                rb.simulated = false;
            }
            if (col)
            {
                col.enabled = false;
            }

            while (time < _moveDuration)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / _moveDuration);
                var ease = _curve.Evaluate(t);

                var pos = Mathf.Pow(1 - ease, 2) * start + 2 * (1 - ease) * ease * control + Mathf.Pow(ease, 2) * targetPos;
                view.transform.position = pos;

                view.transform.Rotate(0f, 0f, 360f * Time.deltaTime, Space.Self);
                yield return null;
            }

            view.transform.position = targetPos;
            PublishReward(view);
        }

        private void PublishReward(CargoView view)
        {
            var cargoId = view.Id;
            if (cargoId == null) return;
            if (!_cargoRegistry.TryGet(cargoId, out var cargo)) return;
            if (!_cargoMasterRegistry.TryGetValue(cargo.MasterId, out var master)) return;

            var amount = master.Cost.Amount;
            if (amount <= 0) return;

            _settlementRewardPublisher?.Publish(new SettlementRewardEvent { CargoView = view, Amount = amount });
        }
    }
}
