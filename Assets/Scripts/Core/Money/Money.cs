using System;
using UnityEngine;

namespace Core.Money
{
    [Serializable]
    public struct Money : IEquatable<Money>
    {
        [SerializeField] private int _amount;

        public int Amount => _amount;

        public Money(int amount)
        {
            _amount = amount;
        }

        public bool Equals(Money other) => _amount == other._amount;

        public override bool Equals(object obj) => obj is Money other && Equals(other);

        public override int GetHashCode() => _amount.GetHashCode();

        public override string ToString() => _amount.ToString();

        public static implicit operator int(Money money) => money._amount;
        public static implicit operator Money(int amount) => new Money(amount);
    }
}
