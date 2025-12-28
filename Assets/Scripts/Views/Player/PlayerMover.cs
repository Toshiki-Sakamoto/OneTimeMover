using System;
using Core.Common;
using Core.Common.Messaging;
using Core.Cargo;
using Core.Player;
using OneTripMover.Core.Player;
using OneTripMover.Input;
using UnityEngine;

namespace OneTripMover.Views.Player
{
    public class PlayerMover : MonoBehaviour, IPlayerMovementInputHandler
    {
        [SerializeField] private PlayerMotion _playerMotion;
        [SerializeField] private PlayerFootCircle _leftFootCircle;
        [SerializeField] private PlayerFootCircle _rightFootCircle;
        [SerializeField] private float _moveForce = 14f;
        [SerializeField] private float _maxSpeed = 16f;
        [SerializeField] private float referenceMass = 10f;
        
        private Rigidbody2D _anchorBody;
        private ICargoRegistry _cargoRegistry;
        private IPlayerMasterRegistry _playerMasterRegistry;

        [Inject]
        public void Construct()
        {
            _cargoRegistry = ServiceLocator.Resolve<ICargoRegistry>();
            _playerMasterRegistry = ServiceLocator.Resolve<IPlayerMasterRegistry>();
        }

        public void SetMovementInput(Vector2 input)
        {
            var effectiveForce = _moveForce;

            _anchorBody.AddForce(Vector2.right * (input * effectiveForce));
            
            // 水平速度をある程度で抑える
            var velocity = _anchorBody.linearVelocity;
            velocity.x = Mathf.Clamp(velocity.x, -_maxSpeed, _maxSpeed);
            _anchorBody.linearVelocity = velocity;
            
            _leftFootCircle.AddAngle(-input.x);
            _rightFootCircle.AddAngle(-input.x);
        }

        private void Awake()
        {
            _anchorBody = GetComponent<Rigidbody2D>();
        }

        public void ApplyMasterSettings(IPlayerMaster master)
        {
            if (master == null) return;
            _moveForce = master.MoveForce;
            _maxSpeed = master.MaxMoveSpeed;
        }
    }
}
