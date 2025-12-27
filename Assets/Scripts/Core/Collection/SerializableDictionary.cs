using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneTripMover.Core.Collection
{
    /// <summary>
    /// シリアライズ可能なDictionaryクラス基底
    /// </summary>
    [Serializable]
    public abstract class SerializableDictionaryBase<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> _keys = new();
        [SerializeField] private List<TValue> _values = new();

        [NonSerialized] private IDictionary<TKey, TValue> _storage;
        [NonSerialized] private IEqualityComparer<TKey> _comparer;

        protected SerializableDictionaryBase()
            : this(null, null)
        {
        }

        protected SerializableDictionaryBase(IEqualityComparer<TKey> comparer)
            : this(null, comparer)
        {
        }

        protected SerializableDictionaryBase(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null)
        {
        }

        protected SerializableDictionaryBase(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;

            EnsureStorage();
            
            if (dictionary == null || dictionary.Count == 0) return;

            foreach (var pair in dictionary)
            {
                _storage[pair.Key] = pair.Value;
            }
        }

        protected IEqualityComparer<TKey> Comparer => _comparer ??= EqualityComparer<TKey>.Default;

        protected IDictionary<TKey, TValue> Storage
        {
            get
            {
                EnsureStorage();
                return _storage;
            }
        }

        private void EnsureStorage()
        {
            if (_storage != null) return;
            _storage = CreateStorage(Comparer);
        }

        /// <summary>
        /// 実際のデータ保持するDictionaryの作成
        /// </summary>
        protected abstract IDictionary<TKey, TValue> CreateStorage(IEqualityComparer<TKey> comparer);

        public TValue this[TKey key]
        {
            get => Storage[key];
            set => Storage[key] = value;
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public ICollection<TKey> Keys => Storage.Keys;

        public ICollection<TValue> Values => Storage.Values;

        public int Count => Storage.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly =>
            ((ICollection<KeyValuePair<TKey, TValue>>)Storage).IsReadOnly;

        public void Add(TKey key, TValue value) => Storage.Add(key, value);

        public bool ContainsKey(TKey key) => Storage.ContainsKey(key);

        public bool Remove(TKey key) => Storage.Remove(key);

        public bool TryGetValue(TKey key, out TValue value) => Storage.TryGetValue(key, out value);

        public void Add(KeyValuePair<TKey, TValue> item) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)Storage).Add(item);

        public void Clear() => Storage.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)Storage).Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)Storage).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) =>
            ((ICollection<KeyValuePair<TKey, TValue>>)Storage).Remove(item);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Storage.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public virtual void OnBeforeSerialize()
        {
            EnsureStorage();
            BeforeSerialization(Storage);

            _keys.Clear();

            _values.Clear();

            foreach (var pair in GetSerializationSnapshot(Storage))
            {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }

        /// <summary>
        /// シリアライズ前に処理を挟みたい場合
        /// </summary>
        protected virtual void BeforeSerialization(IDictionary<TKey, TValue> storage)
        {
        }

        /// <summary>
        /// シリアライズデータのスナップショット作成
        /// </summary>
        protected virtual IEnumerable<KeyValuePair<TKey, TValue>> GetSerializationSnapshot(IDictionary<TKey, TValue> storage)
        {
            return storage;
        }

        public virtual void OnAfterDeserialize()
        {
            _storage = CreateStorage(Comparer);

            var count = Math.Min(_keys.Count, _values.Count);
            for (var i = 0; i < count; i++)
            {
                _storage[_keys[i]] = _values[i];
            }

            AfterDeserialization(_storage);
        }

        /// <summary>
        /// シリアライズ後のデータ処理
        /// </summary>
        protected virtual void AfterDeserialization(IDictionary<TKey, TValue> storage)
        {
        }
    }

    /// <summary>
    /// デフォルトシリアライズ可能なDictionary
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue>
    {
        public SerializableDictionary()
        {
        }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        public SerializableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
        }

        protected override IDictionary<TKey, TValue> CreateStorage(IEqualityComparer<TKey> comparer)
        {
            return new Dictionary<TKey, TValue>(comparer);
        }
    }
}
