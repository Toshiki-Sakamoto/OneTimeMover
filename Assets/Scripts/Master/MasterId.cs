using System;
using UnityEngine;

namespace OneTripMover.Master
{
    [Serializable]
    public struct MasterId<T> : IEquatable<MasterId<T>>
    {
        [SerializeField]
        private int _value;

        public MasterId(int value)
        {
            _value = value;
        }

        public static MasterId<T> None => default;

        public int Value => _value;

        public bool IsValid => _value != 0;

        public bool Equals(MasterId<T> other) => _value == other._value;

        public override bool Equals(object obj) => obj is MasterId<T> other && Equals(other);

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => _value.ToString();

        public static bool operator ==(MasterId<T> left, MasterId<T> right) => left.Equals(right);

        public static bool operator !=(MasterId<T> left, MasterId<T> right) => !left.Equals(right);
    }
}
