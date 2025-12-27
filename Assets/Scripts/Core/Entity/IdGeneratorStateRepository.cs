using System;
using System.Collections.Generic;
using Core.Common;
using OneTripMover.Core.Collection;
using UnityEngine;

namespace OneTripMover.Core.Entity
{
    public sealed class IdGeneratorStateRepository : IIdGeneratorStateRepository
    {
        [SerializeField] private readonly SerializableDictionary<TypeToken, long> _stateMap = new();

        public IReadOnlyDictionary<TypeToken, long> GetStates()
        {
            return _stateMap;
        }

        public void SetStates(IReadOnlyDictionary<TypeToken, long> state)
        {
            _stateMap.Clear();

            foreach (var entry in state)
            {
                _stateMap[entry.Key] = entry.Value;
            }
        }
    }
}
