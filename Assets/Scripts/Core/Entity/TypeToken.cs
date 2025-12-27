using System;
using UnityEngine;

namespace Core.Common
{
    /// <summary>
    /// Typeを保存、復元できるようにするラッパクラス
    /// </summary>
    [Serializable]
    public sealed class TypeToken
    {
        [SerializeField] private string _assemblyQualifiedName;
        
        public static TypeToken FromType(Type type) => new (type);

        public TypeToken(string assemblyQualifiedName)
        {
            _assemblyQualifiedName = assemblyQualifiedName;
        }

        public TypeToken(Type type)
        {
            _assemblyQualifiedName = type.AssemblyQualifiedName ?? string.Empty;
        }

        public string AssemblyQualifiedName => _assemblyQualifiedName;

        public Type? Resolve() =>
            Type.GetType(_assemblyQualifiedName);

        public override string ToString() => _assemblyQualifiedName ?? string.Empty;
    }
}
