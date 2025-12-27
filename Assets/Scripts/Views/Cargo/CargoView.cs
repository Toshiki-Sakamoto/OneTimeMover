using System;
using Core.Attribute;
using OneTripMover.Core;
using UnityEngine;

namespace Views.Cargo
{
    public interface ICargoJointBreakHandler
    {
        void OnCargoJointBreak(CargoView cargoView);
    }
    
    /// <summary>
    /// 一つの荷物のビュー
    /// </summary>
    public class CargoView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private FixedJoint2D _fixedJoint;
        [SerializeField] private float _spawnRandomRotationRange = 30f;
        [SerializeField] private Transform _cargoRoot;
        [SerializeField] private Transform _cargoVisualRoot;
        [SerializeField] private bool _isPreview;
        [SerializeReference, SelectableSerializeReference] private ICargoComponent[] _previewComponents;
        
        private ICargoJointBreakHandler _jointBreakHandler;
        private Collider2D _collider2D;
        
        public CargoId Id { get; }
        public bool IsPreview => _isPreview;
        private Collider2D Collider2D => _collider2D ??= GetComponent<Collider2D>();
        
        public void SetJointBreakHandler(ICargoJointBreakHandler handler)
        {
            _jointBreakHandler = handler;
        }

        public void AddForce(Vector2 force)
        {
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
        
        public void SetMass(float mass)
        {
            _rigidbody.mass = mass;
        }

        public void Break()
        {
            _fixedJoint.enabled = false;
        }

        public void ChangePreview()
        {
            _isPreview = true;
            _rigidbody.bodyType = RigidbodyType2D.Static;
            _fixedJoint.enabled = false;
            Collider2D.enabled = false;
        }

        public void ChangeNormal()
        {
            _isPreview = false;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _fixedJoint.enabled = true;
            Collider2D.enabled = true;
        }


        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            _jointBreakHandler?.OnCargoJointBreak(this);
        }
        
        private void Awake()
        {
            if (_isPreview)
            {
                ChangePreview();
            }
            else
            {
                // 左右反転(1/2)
                var isFlip = UnityEngine.Random.value > 0.5f;

                var randomRotation = UnityEngine.Random.Range(-_spawnRandomRotationRange, _spawnRandomRotationRange);
                _cargoRoot.rotation = Quaternion.Euler(0f, 0f, randomRotation) * _cargoRoot.rotation *
                                      (isFlip ? Quaternion.Euler(0f, 0f, 180f) : Quaternion.identity);
            }
        }

        private void Update()
        {
            if (IsPreview)
            {
                foreach (var component in _previewComponents)
                {
                    component.UpdateCargoView(this, Time.deltaTime);
                }
            }
        }
    }
}