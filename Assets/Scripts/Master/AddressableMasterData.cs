using System;
using System.Security.Cryptography;
using System.Text;
using OneTripMover.Master;
using UnityEngine;

namespace OneTripMover.Master
{
    /// <summary>
    /// Addressable に登録されたアセット GUID から安定した数値 ID を生成する基底クラス。
    /// </summary>
    public abstract class AddressableMasterData<TMaster> : ScriptableObject, IMasterData<TMaster>
        where TMaster : IMasterData<TMaster>
    {
        [SerializeField]
        private MasterId<TMaster> _id;

        public MasterId<TMaster> Id => _id;

        
        public bool Equals(IMasterData<TMaster> other) => 
            Id.Equals(other.Id);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (UnityEngine.Application.isPlaying) return;
            TryAssignId();

            OnValidateCore();
        }
        
        protected virtual void OnValidateCore() {}

        [ContextMenu("Regenerate Id From Addressable")]
        private void RegenerateIdContextMenu()
        {
            TryAssignId(force: true);
        }

        private bool TryAssignId(bool force = false)
        {
            var uniqueKey = ResolveAddressableGuid();
            if (string.IsNullOrEmpty(uniqueKey))
            {
                Debug.LogWarning($"[{name}] Addressables entry not found. Ensure the asset is added to Addressables before generating an ID.", this);
                return false;
            }

            var generated = GenerateStableId(uniqueKey);
            if (!force && _id.Value == generated) return true;

            _id = new MasterId<TMaster>(generated);
            UnityEditor.EditorUtility.SetDirty(this);
            return true;
        }

        private static int GenerateStableId(string key)
        {
            using var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            var value = BitConverter.ToUInt32(bytes, 0);
            if (value == 0)
            {
                value = BitConverter.ToUInt32(bytes, 4);
                if (value == 0)
                {
                    value = BitConverter.ToUInt32(bytes, 8);
                    if (value == 0)
                    {
                        value = BitConverter.ToUInt32(bytes, 12);
                    }
                }
            }

            var candidate = (int)(value % int.MaxValue);
            return candidate == 0 ? int.MaxValue : candidate;
        }

        private string ResolveAddressableGuid()
        {
            var path = UnityEditor.AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path)) return string.Empty;

            var assetGuid = UnityEditor.AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(assetGuid)) return string.Empty;

            var settings = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings?.FindAssetEntry(assetGuid);
            if (entry == null)
            {
                return string.Empty;
            }

            return entry.guid;
        }
#endif
    }
}
