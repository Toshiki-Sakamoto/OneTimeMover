using System;
using UnityEngine;

namespace Core.Stage
{
    [Serializable]
    public struct StageId : IEquatable<StageId>
    {
        [SerializeField] private int _value;

        public int Value => _value;

        public StageId(int value)
        {
            _value = value;
        }

        public bool Equals(StageId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is StageId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value.ToString();

        public static implicit operator int(StageId id) => id.Value;
        public static implicit operator StageId(int value) => new StageId(value);
    }
}
