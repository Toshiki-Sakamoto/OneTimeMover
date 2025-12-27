using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Cargo;
using Core.Common;
using Core.Common.Messaging;
using OneTripMover.Asset;
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
        [SerializeField] private float _collapseAngleDeg = 45f;
        [SerializeField] private float _dangerMarginDeg = 10f;
        
        private List<IAssetLoadHandle<CargoView>> _cargoViews = new ();
        private readonly List<CargoView> _cargoViewRefs = new();
        private IAssetLoader _assetLoader;
        private CancellationToken _cancellationToken;
        private FixedJoint2D _joint;
        private IPublisher<CargoJointViewBreakEvent> _jointBreakPublisher;
        private IPublisher<CargoTowerDangerLeanEvent> _dangerPublisher;
        private IPublisher<CargoViewDetachedEvent> _detachedPublisher;
        private IPublisher<CargoViewGroundHitEvent> _groundHitPublisher;
        private IPublisher<CargoTowerDangerClearedEvent> _dangerClearedPublisher;
        private PlayerController _playerController;
        private bool _isInDanger;
        
        public CargoView TopCargoView { get; private set; }

        [Inject]
        public void Construct()
        {
            _assetLoader = ServiceLocator.Resolve<IAssetLoader>();
            _jointBreakPublisher = ServiceLocator.Resolve<IPublisher<CargoJointViewBreakEvent>>();
            _dangerPublisher = ServiceLocator.Resolve<IPublisher<CargoTowerDangerLeanEvent>>();
            _detachedPublisher = ServiceLocator.Resolve<IPublisher<CargoViewDetachedEvent>>();
            _groundHitPublisher = ServiceLocator.Resolve<IPublisher<CargoViewGroundHitEvent>>();
            _dangerClearedPublisher = ServiceLocator.Resolve<IPublisher<CargoTowerDangerClearedEvent>>();
        }

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void Update() => CheckDangerLean();
        
        public void SetCancellationToken(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
        }
        
        public void AddCargoView(ICargoMaster cargoMaster)
        {
            var handle = _assetLoader.LoadInstantiate<CargoView>(cargoMaster.CargoView, _cargoTowerRoot, false);
            if (handle?.Asset == null) return;
            
            var view = handle.Asset;
            view.SetJointBreakHandler(this);
            view.SetGroundHitHandler(this);
            
            PositionOnTop(view);
            ConnectJoint(view);

            _cargoViews.Add(handle);
            RegisterCargoView(view);
            TopCargoView = view;
        }

        public void AttachExistingCargo(CargoView view)
        {
            if (view == null) return;

            view.SetJointBreakHandler(this);
            view.SetGroundHitHandler(this);
            view.ChangeNormal();
            view.transform.SetParent(_cargoTowerRoot, false);
            view.transform.localRotation = Quaternion.identity;

            PositionOnTop(view);
            ConnectJoint(view);

            RegisterCargoView(view);
            TopCargoView = view;
        }
        
        public void SetAllCargoBreakAction(JointBreakAction2D breakAction)
        {
            foreach (var view in _cargoViewRefs)
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
            _jointBreakPublisher.Publish(new CargoJointViewBreakEvent { CargoView = cargoView });
            _detachedPublisher.Publish(new CargoViewDetachedEvent
            {
                CargoId = cargoView.Id,
                CargoView = cargoView
            });
            RemoveCargoView(cargoView);
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

            var dir = (Vector2)(TopCargoView.transform.position - (Vector3)_anchorRigidbody.position);
            if (dir == Vector2.zero) return;

            var angle = Mathf.Abs(Vector2.SignedAngle(Vector2.up, dir));
            var dangerThreshold = Mathf.Max(0f, _collapseAngleDeg - _dangerMarginDeg);
            var inDanger = angle >= dangerThreshold && angle < _collapseAngleDeg;

            if (inDanger && !_isInDanger)
            {
                _isInDanger = true;
                _dangerPublisher?.Publish(new CargoTowerDangerLeanEvent
                {
                    Player = _playerController,
                    CurrentAngleDeg = angle,
                    CollapseAngleDeg = _collapseAngleDeg,
                    DangerMarginDeg = _dangerMarginDeg
                });
            }
            else if (!inDanger && _isInDanger)
            {
                _isInDanger = false;
                _dangerClearedPublisher?.Publish(new CargoTowerDangerClearedEvent
                {
                    Player = _playerController,
                    CurrentAngleDeg = angle,
                    CollapseAngleDeg = _collapseAngleDeg,
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

        private void RegisterCargoView(CargoView view)
        {
            if (view == null) return;
            _cargoViewRefs.Add(view);
        }

        private void RemoveCargoView(CargoView view)
        {
            if (view == null) return;
            _cargoViewRefs.Remove(view);
            TopCargoView = _cargoViewRefs.Count > 0 ? _cargoViewRefs[^1] : null;
        }

        private float GetTopY(GameObject go) =>
            go.transform.position.y + GetHeight(go) * 0.5f;

    }
}
