using System;
using Core.Common;
using Core.Common.Messaging;
using Core.Cargo;
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

        [Inject]
        public void Construct()
        {
            _cargoRegistry = ServiceLocator.Resolve<ICargoRegistry>();
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
    }
}
