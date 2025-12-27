using Core.Attribute;

namespace Core.Editor.Attribute
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NUnit.Framework;
    using UnityEditor;
    using UnityEngine;


    [CustomPropertyDrawer(typeof(SelectableSerializeReferenceAttribute))]
    public class SelectableSerializeReferenceAttributeDrawer : PropertyDrawer
    {
        private class PropertyCache
        {
            public Type[] DerivedTypes { get; }
            public string[] DerivedTypeNames { get; }
            public string[] DerivedFullTypeNames { get; }

            /// <summary> アセンブリ一覧から指定したインターフェイスを実装している型を取得 </summary>
            private static Assembly GetAssembly(string name) =>
                AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name == name);

            public PropertyCache(SerializedProperty property)
            {
                var managedReferenceFieldTypanameSplit = property.managedReferenceFieldTypename.Split(' ').ToArray();
                var assemblyName = managedReferenceFieldTypanameSplit[0];
                var fieldTypeName = managedReferenceFieldTypanameSplit[1];
                var fieldType = GetAssembly(assemblyName).GetType(fieldTypeName); // 指定Typeを取得
            
                // Interfaceの継承先を見つける
                DerivedTypes = TypeCache.GetTypesDerivedFrom(fieldType)
                    .Where(x => !x.IsAbstract && !x.IsInterface)
                    .ToArray();

                DerivedTypeNames = new string[DerivedTypes.Length];
                DerivedFullTypeNames = new string[DerivedTypes.Length];
            
                for (var i = 0; i < DerivedTypes.Length; i++)
                {
                    DerivedTypeNames[i] = GetDisplayName(DerivedTypes[i]);
                    DerivedFullTypeNames[i] = DerivedTypes[i].FullName;
                }
            }
        
            /// <summary> 型のDisplayNameを取得 </summary>
            private string GetDisplayName(Type type)
            {
                // 1. DisplayNameフィールドがあるかチェック
                var displayNameField = type.GetField("DisplayName", BindingFlags.Public | BindingFlags.Static);
                if (displayNameField != null && displayNameField.FieldType == typeof(string))
                {
                    return (string)displayNameField.GetValue(null);
                }
            
                // 2. DisplayNameがない場合は型名を使用
                return ObjectNames.NicifyVariableName(type.Name);
            }
        }
    
        private readonly Dictionary<string, PropertyCache> _cachePerPathDict = new();
        private PropertyCache _cache;
        private int _selectedIndex; // 選択した継承先クラスのインデックス
    
    
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Interfaceに指定されてないものは弾く
            Assert.IsTrue(property.propertyType == SerializedPropertyType.ManagedReference);
        
            SetupCache(property);
        
            var fullTypeName = property.managedReferenceFullTypename.Split(' ').Last();
            _selectedIndex = Array.IndexOf(_cache.DerivedFullTypeNames, fullTypeName); // 現在選択中の継承先のクラスがどこにあるか
        
            using (var ccs = new EditorGUI.ChangeCheckScope())
            {
                var selectorPosition = position;

                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                selectorPosition.width -= EditorGUIUtility.labelWidth;
                selectorPosition.x += EditorGUIUtility.labelWidth;
                selectorPosition.height = EditorGUIUtility.singleLineHeight;

                var selectedTypeIndex = EditorGUI.Popup(selectorPosition, _selectedIndex, _cache.DerivedTypeNames); // Interfaceを継承したクラスから選択

                if (ccs.changed)
                {
                    _selectedIndex = selectedTypeIndex;
                
                    // 選択したクラスのインスタンスを保持させる
                    var selectedType = _cache.DerivedTypes[selectedTypeIndex];
                    property.managedReferenceValue =
                        selectedType == null ? null : Activator.CreateInstance(selectedType);
                }

                EditorGUI.indentLevel = indent;
            }

            EditorGUI.PropertyField(position, property, label, true); // 継承先クラスのSerialized変数まで表示させる
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SetupCache(property);

            // 継承先の中で表示すべきSerialize変数があればそれを表示するための領域を取得
            return string.IsNullOrEmpty(property.managedReferenceFullTypename) ? 
                EditorGUIUtility.singleLineHeight : EditorGUI.GetPropertyHeight(property, true);
        }
    
        /// <summary> キャッシュないになければ作成。あればそれを利用する </summary>
        private void SetupCache(SerializedProperty property)
        {
            if (_cachePerPathDict.TryGetValue(property.propertyPath, out _cache)) return;

            _cache = new PropertyCache(property);
            _cachePerPathDict.Add(property.propertyPath, _cache);
        }
    }
}