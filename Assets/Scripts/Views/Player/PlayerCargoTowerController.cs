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
    public class PlayerCargoTowerController : MonoBehaviour, IPlayerBalanceHandler, ICargoJointBreakHandler
    {
        [SerializeField] private Rigidbody2D _anchorRigidbody;
        [SerializeField] private Transform _cargoTowerRoot;
        [SerializeField] private float _gap = 0f;
        
        private List<IAssetLoadHandle<CargoView>> _cargoViews = new ();
        private IAssetLoader _assetLoader;
        private CancellationToken _cancellationToken;
        private FixedJoint2D _joint;
        private IPublisher<CargoJointViewBreakEvent> _jointBreakPublisher;
        
        public CargoView TopCargoView { get; private set; }

        [Inject]
        public void Construct()
        {
            _assetLoader = ServiceLocator.Resolve<IAssetLoader>();
            _jointBreakPublisher = ServiceLocator.Resolve<IPublisher<CargoJointViewBreakEvent>>();
        }
        
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
            
            PositionOnTop(view);
            ConnectJoint(view);

            _cargoViews.Add(handle);
            TopCargoView = view;
        }
        
        public void SetAllCargoBreakAction(JointBreakAction2D breakAction)
        {
            foreach (var handle in _cargoViews)
            {
                var view = handle.Asset;
                if (view == null) continue;

                var joint = view.GetComponent<FixedJoint2D>();
                if (joint == null) continue;

                joint.breakAction = breakAction;
            }
        }
        

        public void AddBalanceForce(Vector2 force)
        {
            if (TopCargoView == null) return;
            TopCargoView.AddForce(force);
        }

        public void OnCargoJointBreak(CargoView cargoView)
        {
            _jointBreakPublisher.Publish(new CargoJointViewBreakEvent { CargoView = cargoView });
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

        private float GetTopY(GameObject go) =>
            go.transform.position.y + GetHeight(go) * 0.5f;

    }
}
