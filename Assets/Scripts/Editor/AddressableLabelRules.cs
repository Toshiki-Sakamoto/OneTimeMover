using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace OneTripMover.EditorTools
{
    /// <summary>
    /// Addressable用のラベル自動付与ルール。ScriptableObjectを作成し、対象フォルダ/アセットに対するラベルを定義する。
    /// インポート時にマッチしたルールのラベルを付与する。
    /// </summary>
    public class AddressableLabelRules : ScriptableObject
    {
        [SerializeField] private List<Rule> rules = new();

        [System.Serializable]
        private struct Rule
        {
            public Object target;          // フォルダまたはアセット
            public string label;           // 付与するラベル
            public bool includeSubfolders; // フォルダ時、サブフォルダも対象にするか
            public string groupName;       // 追加先グループ（空ならDefault Group）
            public bool createGroupIfMissing;
        }

        [MenuItem("Assets/Create/Addressables/Label Rules Asset", priority = 2000)]
        private static void CreateAsset()
        {
            var asset = CreateInstance<AddressableLabelRules>();
            var path = EditorUtility.SaveFilePanelInProject("Create Addressable Label Rules", "AddressableLabelRules", "asset", "保存先を選択してください");
            if (string.IsNullOrEmpty(path)) return;
            
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = asset;
        }

        private static IReadOnlyList<AddressableLabelRules> LoadAllRuleAssets()
        {
            var guids = AssetDatabase.FindAssets("t:AddressableLabelRules");
            var list = new List<AddressableLabelRules>(guids.Length);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<AddressableLabelRules>(path);
                if (asset != null)
                {
                    list.Add(asset);
                }
            }
            return list;
        }

        private bool Matches(string assetPath, Rule rule)
        {
            if (rule.target == null || string.IsNullOrEmpty(rule.label))
            {
                return false;
            }

            var rulePath = AssetDatabase.GetAssetPath(rule.target);
            if (string.IsNullOrEmpty(rulePath))
            {
                return false;
            }

            // フォルダ: StartsWithで判定（必要ならサブフォルダ含む）
            if (AssetDatabase.IsValidFolder(rulePath))
            {
                if (rule.includeSubfolders)
                {
                    return assetPath.StartsWith(rulePath + "/");
                }
                else
                {
                    var parent = System.IO.Path.GetDirectoryName(assetPath)?.Replace("\\", "/");
                    return parent == rulePath;
                }
            }

            // 単一アセット
            return assetPath == rulePath;
        }

        private void ApplyMatchedRules(string assetPath, AddressableAssetSettings settings)
        {
            foreach (var rule in rules)
            {
                if (!Matches(assetPath, rule)) continue;

                var label = rule.label.Trim();
                if (string.IsNullOrEmpty(label)) continue;

                if (!settings.GetLabels().Contains(label))
                {
                    settings.AddLabel(label);
                }

                var guid = AssetDatabase.AssetPathToGUID(assetPath);
                var group = ResolveGroup(settings, rule);
                var entry = settings.FindAssetEntry(guid) ?? settings.CreateOrMoveEntry(guid, group, false, true);
                if (entry != null && entry.parentGroup != group)
                {
                    settings.MoveEntry(entry, group);
                }

                if (entry != null && !entry.labels.Contains(label))
                {
                    entry.SetLabel(label, true, true);
                    EditorUtility.SetDirty(settings);
                }
            }
        }

        private AddressableAssetGroup ResolveGroup(AddressableAssetSettings settings, Rule rule)
        {
            if (string.IsNullOrEmpty(rule.groupName)) return settings.DefaultGroup;
           
            var group = settings.FindGroup(rule.groupName);
            if (group == null && rule.createGroupIfMissing)
            {
                group = settings.CreateGroup(rule.groupName, false, false, false, null);
            }
            
            return group != null ? group : settings.DefaultGroup;
        }

        private static class ImportHook
        {
            private class Postprocessor : AssetPostprocessor
            {
                private static IReadOnlyList<AddressableLabelRules> _cachedRules;

                static Postprocessor()
                {
                    RefreshRuleCache();
                }

                private static void RefreshRuleCache()
                {
                    _cachedRules = LoadAllRuleAssets();
                }

                public override int GetPostprocessOrder()
                {
                    // Addressablesの設定が行われた後で実行させたいのでデフォルトより後ろにする
                    return 100;
                }

                static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
                {
                    if (_cachedRules == null || _cachedRules.Count == 0)
                    {
                        RefreshRuleCache();
                        if (_cachedRules == null || _cachedRules.Count == 0)
                        {
                            return;
                        }
                    }

                    var settings = AddressableAssetSettingsDefaultObject.Settings;
                    if (settings == null) return;

                    foreach (var assetPath in importedAssets)
                    {
                        foreach (var ruleAsset in _cachedRules)
                        {
                            if (ruleAsset == null) continue;
                            ruleAsset.ApplyMatchedRules(assetPath, settings);
                        }
                    }
                }
            }
        }
    }
}
