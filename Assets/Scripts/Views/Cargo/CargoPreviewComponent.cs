using System;
using UnityEngine;

namespace Views.Cargo
{
    [Serializable]
    public class CargoPreviewComponent :　ICargoComponent
    {
        [SerializeField] private float _rotationDelta = 20f;
        
        public bool IsPreview => true;


        public void UpdateCargoView(CargoView view, float delta)
        {
            // Y軸回転
            view.transform.Rotate(Vector3.up, _rotationDelta * delta);
        }
    }
}