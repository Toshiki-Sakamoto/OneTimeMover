using System.Collections.Generic;
using UnityEngine;

namespace OneTripMover
{
    /// <summary>
    /// タワー生成と「もう一つ積む」の処理を担当。
    /// FixedJoint2Dでゆらゆら繋ぎ、BreakForceで崩れる。
    /// </summary>
    public class StackTower : MonoBehaviour
    {
        [Header("Base")]
        public Rigidbody2D baseAnchor;

        [Header("Stacking")]
        [SerializeField] private int initialCount = 3;
        [SerializeField] private float spawnGap = 0.05f;

        [Header("Joint Settings")]
        [SerializeField] private float jointBreakForce = 180f;
        [SerializeField] private float jointBreakTorque = 120f;
        [SerializeField] private float jointDamping = 0.15f;
        [SerializeField] private float jointFrequency = 2f;

        private readonly List<CargoPreset> _presets = new();
        [SerializeField] private Rigidbody2D _topBody;

        public Rigidbody2D TopBody => _topBody;
        public int InitialCount { get => initialCount; set => initialCount = value; }

        private void Reset()
        {
            baseAnchor = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
           // EnsurePresets();
        }

        private void Start()
        {
            if (baseAnchor == null)
            {
                baseAnchor = GetComponent<Rigidbody2D>();
            }
/*
            for (int i = 0; i < initialCount; i++)
            {
                AddOne();
            }*/
        }

        /// <summary>
        /// ボタンから呼ばれるAPI。
        /// </summary>
        public void AddOne()
        {
            if (baseAnchor == null)
            {
                Debug.LogWarning("StackTower: baseAnchor 未設定です。");
                return;
            }

            var preset = _presets[Random.Range(0, _presets.Count)];
            float newHeight = GetHeight(preset);

            Vector2 spawnPos = ComputeSpawnPosition(newHeight);
            var newBody = CreateCargo(preset, spawnPos);

            var connectTo = _topBody != null ? _topBody : baseAnchor;
            var joint = newBody.gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = connectTo;
            joint.breakForce = jointBreakForce;
            joint.breakTorque = jointBreakTorque;
            joint.dampingRatio = jointDamping;
            joint.frequency = jointFrequency;
            joint.enableCollision = true;

            _topBody = newBody;
        }

        private Vector2 ComputeSpawnPosition(float newHeight)
        {
            float baseTop = GetCurrentTopY();
            float spawnY = baseTop + spawnGap + newHeight * 0.5f;
            float spawnX = _topBody != null ? _topBody.position.x : baseAnchor.position.x;
            return new Vector2(spawnX, spawnY);
        }

        private float GetCurrentTopY()
        {
            if (_topBody == null)
            {
                var anchorCollider = baseAnchor.GetComponent<Collider2D>();
                float baseHeight = anchorCollider != null ? anchorCollider.bounds.size.y : 0.5f;
                return baseAnchor.position.y + baseHeight * 0.5f;
            }

            var topPiece = _topBody.GetComponent<CargoPiece>();
            float topHeight = topPiece != null ? topPiece.Height : 1f;
            return _topBody.position.y + topHeight * 0.5f;
        }

        private Rigidbody2D CreateCargo(CargoPreset preset, Vector2 position)
        {
            var go = new GameObject($"Cargo - {preset.name}");
            go.transform.position = position;
            go.transform.localScale = new Vector3(preset.size.x, preset.size.y, 1f);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ProceduralSprite.SharedSquare;
            renderer.color = preset.color;
            renderer.sortingOrder = 1;

            Collider2D collider = AddCollider(go, preset);
            var body = go.AddComponent<Rigidbody2D>();
            body.mass = Mathf.Max(0.35f, preset.density * preset.size.x * preset.size.y);
            body.angularDamping = 0.05f;

            var cargo = go.AddComponent<CargoPiece>();
            cargo.Height = collider.bounds.size.y;

            return body;
        }

        private Collider2D AddCollider(GameObject go, CargoPreset preset)
        {
            switch (preset.collider)
            {
                case ColliderKind.Circle:
                    var circle = go.AddComponent<CircleCollider2D>();
                    circle.radius = 0.5f;
                    return circle;
                case ColliderKind.Capsule:
                    var capsule = go.AddComponent<CapsuleCollider2D>();
                    capsule.direction = CapsuleDirection2D.Vertical;
                    capsule.size = Vector2.one;
                    return capsule;
                default:
                    var box = go.AddComponent<BoxCollider2D>();
                    box.size = Vector2.one;
                    return box;
            }
        }

        private void EnsurePresets()
        {
            if (_presets.Count > 0) return;

            _presets.Add(new CargoPreset
            {
                name = "Box",
                size = new Vector2(1f, 1f),
                collider = ColliderKind.Box,
                density = 1f,
                color = new Color(0.85f, 0.7f, 0.45f)
            });

            _presets.Add(new CargoPreset
            {
                name = "Wide Box",
                size = new Vector2(1.3f, 0.8f),
                collider = ColliderKind.Box,
                density = 0.9f,
                color = new Color(0.9f, 0.6f, 0.4f)
            });

            _presets.Add(new CargoPreset
            {
                name = "Tall Box",
                size = new Vector2(0.8f, 1.4f),
                collider = ColliderKind.Capsule,
                density = 1.1f,
                color = new Color(0.75f, 0.55f, 0.35f)
            });

            _presets.Add(new CargoPreset
            {
                name = "Ball",
                size = new Vector2(0.9f, 0.9f),
                collider = ColliderKind.Circle,
                density = 0.8f,
                color = new Color(0.5f, 0.8f, 1f)
            });
        }

        private float GetHeight(CargoPreset preset)
        {
            return preset.collider == ColliderKind.Circle ? preset.size.x : preset.size.y;
        }

        private enum ColliderKind
        {
            Box,
            Circle,
            Capsule
        }

        private struct CargoPreset
        {
            public string name;
            public Vector2 size;
            public ColliderKind collider;
            public float density;
            public Color color;
        }
    }

    /// <summary>
    /// 単一ピースの縦寸法を覚えておくコンポーネント。
    /// </summary>
    public class CargoPiece : MonoBehaviour
    {
        public float Height { get; set; }
    }

    /// <summary>
    /// 1x1のシンプルなスプライトを使い回すヘルパー。
    /// </summary>
    public static class ProceduralSprite
    {
        private static Sprite _sharedSquare;

        public static Sprite SharedSquare
        {
            get
            {
                if (_sharedSquare == null)
                {
                    var tex = new Texture2D(2, 2);
                    tex.SetPixels(new[]
                    {
                        Color.white, Color.white,
                        Color.white, Color.white
                    });
                    tex.Apply();
                    _sharedSquare = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                    _sharedSquare.name = "ProceduralSquare";
                }

                return _sharedSquare;
            }
        }
    }
}
