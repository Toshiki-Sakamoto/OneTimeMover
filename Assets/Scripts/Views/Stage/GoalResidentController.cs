using System;
using Core.Common;
using UnityEngine;

namespace OneTripMover.Views.Stage
{
    public class GoalResidentController : MonoBehaviour
    {
        [SerializeField] private Transform _goalResidentPoint;
        private GoalController _goalController;
        
        public Transform GoalResidentPoint => _goalResidentPoint;

        private void Awake()
        {
            _goalController = ServiceLocator.Resolve<GoalController>();
            _goalController.SetGoalResident(this);
        }
    }
}