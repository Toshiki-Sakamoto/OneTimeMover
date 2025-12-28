using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Player;
using Core.Stage;
using OneTripMover.Asset;
using OneTripMover.Core.Entity;
using OneTripMover.Master;
using UnityEngine;
using Views.Cargo;

namespace OneTripMover.Views.Player
{
    public class PlayerCargoTowerController : MonoBehaviour, 
        IPlayerBalanceHandler, 
        ICargoJointBreakHandler,
        ICargoGroundHitHandler
    {
        [SerializeField] private Rigidbody2D _anchorRigidbody;
        [SerializeField] private Transform _cargoTowerRoot;
        [SerializeField] private float _gap = 0f;
        [SerializeField] private float _movementBalanceAssist = 1.5f;
        [SerializeField] private float _dangerMarginDeg = 10f;
        [SerializeField] private CargoDangerIndicatorView _dangerIndicator;
        [SerializeField] private float _topBreakCooldownSeconds = 0.5f;
        
        private List<IAssetLoadHandle<CargoView>> _cargoViews = new ();
        private IAssetLoader _assetLoader;
        private FixedJoint2D _joint;
        private IPublisher<CargoJointViewBreakEvent> _jointBreakPublisher;
        private IPublisher<CargoTowerDangerLeanEvent> _dangerPublisher;
        private IPublisher<CargoViewDetachedEvent> _detachedPublisher;
        private IPublisher<CargoViewGroundHitEvent> _groundHitPublisher;
        private IPublisher<CargoTowerDangerClearedEvent> _dangerClearedPublisher;
        private IPublisher<OneMoreBonusAcquiredEvent> _oneMoreBonusAcquiredPublisher;
        private IPlayerStatusUseCase _playerStatusUseCase;
        private PlayerController _playerController;
        private bool _isInDanger;
        private float _topBreakCooldownUntil;
        private ICargoViewRegistry _cargoViewRegistry;

        public CargoView TopCargoView { get; private set; }
        public IReadOnlyList<CargoView> CargoViews => _cargoViewRegistry?.GetCurrentViews();

        [Inject]
        public void Construct()
        {
            _assetLoader = ServiceLocator.Resolve<IAssetLoader>();
            _jointBreakPublisher = ServiceLocator.Resolve<IPublisher<CargoJointViewBreakEvent>>();
            _dangerPublisher = ServiceLocator.Resolve<IPublisher<CargoTowerDangerLeanEvent>>();
            _detachedPublisher = ServiceLocator.Resolve<IPublisher<CargoViewDetachedEvent>>();
            _groundHitPublisher = ServiceLocator.Resolve<IPublisher<CargoViewGroundHitEvent>>();
            _dangerClearedPublisher = ServiceLocator.Resolve<IPublisher<CargoTowerDangerClearedEvent>>();
            _oneMoreBonusAcquiredPublisher = ServiceLocator.Resolve<IPublisher<OneMoreBonusAcquiredEvent>>();
            _playerStatusUseCase = ServiceLocator.Resolve<IPlayerStatusUseCase>();
            _cargoViewRegistry = ServiceLocator.Resolve<ICargoViewRegistry>();
        }

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void Update() => CheckDangerLean();
        
        public void SetCancellationToken(CancellationToken cancellationToken)
        {
        }
        
        public void AddCargoView(IEntityId cargoId, ICargoMaster cargoMaster)
        {
            var handle = _assetLoader.LoadInstantiate<CargoView>(cargoMaster.CargoView, _cargoTowerRoot, false);
            if (handle?.Asset == null) return;
            
            var view = handle.Asset;
            view.SetJointBreakHandler(this);
            view.SetGroundHitHandler(this);
            view.SetCargoId(cargoId);
            view.SetMasterId(cargoMaster.Id);
            
            PositionOnTop(view);
            ConnectJoint(view);

            _cargoViews.Add(handle);
            _cargoViewRegistry?.AddCurrent(view);
            TopCargoView = _cargoViewRegistry?.GetTopCurrentView() ?? view;
        }

        public void AttachExistingCargo(CargoView view, bool isOneMoreCargo)
        {
            if (view == null) return;
            if (isOneMoreCargo)
            {
                _oneMoreBonusAcquiredPublisher?.Publish(new OneMoreBonusAcquiredEvent { CargoMasterId = view.MasterId });
            }
            _cargoViewRegistry?.AddCurrent(view);
            TopCargoView = _cargoViewRegistry?.GetTopCurrentView() ?? view;
        }
        
        public void SetAllCargoBreakAction(JointBreakAction2D breakAction)
        {
            var views = _cargoViewRegistry?.GetCurrentViews();
            if (views == null) return;
            foreach (var view in views)
            {
                if (view == null) continue;

                var joint = view.GetComponent<FixedJoint2D>();
                if (joint == null) continue;

                joint.breakAction = breakAction;
            }
        }
        

        public void AddBalanceForce(Vector2 force)
        {
            if (TopCargoView == null) return;

            var assistMultiplier = GetAssistMultiplierFromMovement();
            TopCargoView.AddForce(force * assistMultiplier);
        }

        public void OnCargoJointBreak(CargoView cargoView)
        {
            if (Time.time < _topBreakCooldownUntil) return;
            
            Debug.Log($"Cargo Joint Broken: {cargoView.Id} {Time.time}");

            _jointBreakPublisher.Publish(new CargoJointViewBreakEvent { CargoView = cargoView });
            _detachedPublisher.Publish(new CargoViewDetachedEvent { CargoId = cargoView.Id, });
            _cargoViewRegistry?.RemoveCurrent(cargoView);
            _cargoViewRegistry?.AddDropped(cargoView);

            TopCargoView = _cargoViewRegistry?.GetTopCurrentView();

            _topBreakCooldownUntil = Time.time + _topBreakCooldownSeconds;
        }
        
        public void OnCargoGroundHit(CargoView cargoView, Collision2D collision)
        {
            _groundHitPublisher.Publish(new CargoViewGroundHitEvent
            {
                CargoId = cargoView.Id,
                CargoView = cargoView,
                Collision = collision
            });
        }
        
        private void PositionOnTop(CargoView view)
        {
            var go = view.gameObject;
            var baseX = _cargoTowerRoot.position.x;
            var baseZ = _cargoTowerRoot.position.z;

            // 現在の最上段高さと接続対象を取得
            var topY = _cargoTowerRoot.position.y;
            if (TopCargoView != null)
            {
                topY = GetTopY(TopCargoView.gameObject);
            }

            var newHeight = GetHeight(go);
            var targetY = topY + newHeight * 0.5f + _gap;

            go.transform.position = new Vector3(baseX, targetY, baseZ);
        }

        private void ConnectJoint(CargoView view)
        {
            var go = view.gameObject;
            _joint = view.GetComponent<FixedJoint2D>();

            var connectedBody = _anchorRigidbody;
            if (TopCargoView != null)
            {
                connectedBody = TopCargoView.GetComponent<Rigidbody2D>() ?? connectedBody;
            }

            _joint.connectedBody = connectedBody;
            _joint.enableCollision = true;
        }

        private float GetHeight(GameObject go)
        {
            var col = go.GetComponent<Collider2D>();
            return col != null ? col.bounds.size.y : 1f;
        }

        private void CheckDangerLean()
        {
            if (_anchorRigidbody == null || TopCargoView == null)
            {
                _isInDanger = false;
                return;
            }

            var limitAngle = _playerStatusUseCase.GetLimitAngle();
            
            var angle = Vector2.SignedAngle(Vector2.up, TopCargoView.transform.up);
            var angleAbs = Mathf.Abs(angle);
            if (angleAbs <= 0.01f)
            {
                _dangerIndicator?.SetAngle(limitAngle, 0);
                return;
            }

            _dangerIndicator?.SetAngle(limitAngle, angle);
            
            var dangerThreshold = Mathf.Max(0f, limitAngle - _dangerMarginDeg);
            var inDanger = angleAbs >= dangerThreshold && angleAbs < limitAngle;

            if (inDanger && !_isInDanger)
            {
                _isInDanger = true;
                _dangerPublisher?.Publish(new CargoTowerDangerLeanEvent
                {
                    Player = _playerController,
                    CurrentAngleDeg = angleAbs,
                    CollapseAngleDeg = limitAngle,
                    DangerMarginDeg = _dangerMarginDeg
                });
            }
            else if (!inDanger && _isInDanger)
            {
                _isInDanger = false;
                _dangerClearedPublisher?.Publish(new CargoTowerDangerClearedEvent
                {
                    Player = _playerController,
                    CurrentAngleDeg = angleAbs,
                    CollapseAngleDeg = limitAngle,
                    DangerMarginDeg = _dangerMarginDeg
                });
            }
        }

        private float GetAssistMultiplierFromMovement()
        {
            // タワーが傾いている方向へ歩いているときはバランス入力を強める
            if (_anchorRigidbody == null || TopCargoView == null) return 1f;

            var leanDir = Mathf.Sign(TopCargoView.transform.position.x - _anchorRigidbody.position.x);
            var moveDir = Mathf.Sign(_anchorRigidbody.linearVelocity.x);

            if (Mathf.Approximately(leanDir, 0f) || Mathf.Approximately(moveDir, 0f)) return 1f;
            if (!Mathf.Approximately(leanDir, moveDir)) return 1f;

            return 1f + _movementBalanceAssist;
        }

        private float GetTopY(GameObject go) =>
            go.transform.position.y + GetHeight(go) * 0.5f;

        public void OnGoalCleared(GoalClearedEvent evt)
        {
            FreezeAllCargo();
        }

        private void FreezeAllCargo()
        {
            var views = _cargoViewRegistry?.GetCurrentViews();
            if (views == null) return;
            foreach (var view in views)
            {
                if (view == null) continue;
                FreezeCargoView(view);
            }

            if (_anchorRigidbody != null)
            {
                _anchorRigidbody.linearVelocity = Vector2.zero;
                _anchorRigidbody.angularVelocity = 0f;
                _anchorRigidbody.bodyType = RigidbodyType2D.Static;
            }
        }

        private void FreezeCargoView(CargoView view)
        {
            if (view == null) return;
            var joint = view.GetComponent<FixedJoint2D>();
            if (joint != null) joint.enabled = false;

            var rb = view.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static;
            }

            var col = view.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
        
    }
}
