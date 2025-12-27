using System;
using UnityEngine;

namespace OneTripMover.Views.Player
{
    /// <summary>
    /// 移動による足の踏みかえ
    /// </summary>
    public class PlayerFootStepper : MonoBehaviour
    {
        [SerializeField] private Transform _leftTarget, _rightTarget;
        [SerializeField] private LayerMask _groundMask;

        private void LateUpdate()
        {
            
        }
    }
}