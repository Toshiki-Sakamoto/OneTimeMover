using System;
using JetBrains.Annotations;
using UnityEngine;

namespace OneTripMover.Core
{
    /// <summary>
    /// 荷物ID
    /// </summary>
    [Serializable]
    public struct CargoId : IEquatable<CargoId>
    {
        [SerializeField] private int _value;
        
        /// <summary>
        /// ID値
        /// </summary>
        public int Value => _value;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">ID値</param>
        public CargoId(int value)
        {
            _value = value;
        }

        public bool Equals(CargoId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is CargoId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}