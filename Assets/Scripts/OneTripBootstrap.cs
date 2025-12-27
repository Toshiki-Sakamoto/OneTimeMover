using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OneTripMover
{
    /// <summary>
    /// シーン読み込み後に簡易レベルとUIを自動生成するブートストラップ。
    /// エディタでの手配なしで「3箱積んで歩く」「ADD ONEで増やす」を再現。
    /// </summary>
    public class OneTripBootstrap : MonoBehaviour
    {
        private static bool _spawned;

        [Header("Positions")]
        [SerializeField] private Vector2 startPosition = new(-10f, 0.6f);
        [SerializeField] private Vector2 addOneZonePosition = new(-2f, 0.6f);
        [SerializeField] private Vector2 goalZonePosition = new(10f, 0.6f);

        [Header("Geometry")]
        [SerializeField] private float groundWidth = 30f;
        [SerializeField] private float groundThickness = 1f;
        [SerializeField] private Vector2 stepSize = new(2f, 0.8f);
        [SerializeField] private Vector2 stepPosition = new(1.5f, 0.4f);

        [Header("Stack")]
        [SerializeField] private int initialStackCount = 3;

      //  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoCreate()
        {
            if (_spawned) return;
            var go = new GameObject("OneTripBootstrap");
            go.AddComponent<OneTripBootstrap>();
            DontDestroyOnLoad(go);
            _spawned = true;
        }

        private void Start()
        {
            BuildWorld();
        }

        private void BuildWorld()
        {
            ConfigureCamera();

         //   var ground = CreateBox("Ground", new Vector2(groundWidth, groundThickness), new Vector2(0f, -groundThickness * 0.5f), new Color(0.45f, 0.8f, 0.45f));
         //   ground.isTrigger = false;

            var step = CreateBox("Step", stepSize, stepPosition, new Color(0.55f, 0.55f, 0.55f));
            step.isTrigger = false;

            /*
            var anchorGo = CreateAnchor();
            var anchorBody = anchorGo.GetComponent<Rigidbody2D>();

            var tower = anchorGo.AddComponent<StackTower>();
            tower.baseAnchor = anchorBody;
            tower.InitialCount = initialStackCount;

            var mover = anchorGo.AddComponent<StackMover>();
            mover.anchorBody = anchorBody;
            mover.stackTower = tower;
*/
            var canvas = BuildCanvas();
            var status = CreateText(canvas.transform, "StatusLabel", new Vector2(16f, -16f), 18, TextAnchor.UpperLeft);
            status.text = "矢印:移動 / A,D:バランス\nADD ONEが光ったら押して荷物追加";

            var addButton = CreateButton(canvas.transform, "AddOneButton", "ADD ONE", new Vector2(-120f, 80f));
            addButton.gameObject.SetActive(false);
      //      addButton.onClick.AddListener(tower.AddOne);
/*
            var addZone = CreateZone("AddOneZone", new Vector2(2f, 2f), addOneZonePosition, new Color(1f, 0.85f, 0.4f));
            var addZoneScript = addZone.gameObject.AddComponent<AddOneZone>();
            addZoneScript.anchor = anchorBody;
            addZoneScript.stackTower = tower;
            addZoneScript.addOneButton = addButton;

            var goalZone = CreateZone("GoalZone", new Vector2(2.5f, 2.5f), goalZonePosition, new Color(0.4f, 0.7f, 1f, 0.5f));
            var goalScript = goalZone.gameObject.AddComponent<GoalZone>();
            goalScript.anchor = anchorBody;
            goalScript.statusLabel = status;
*/
            status.text = "左/右矢印で歩く → 黄色ゾーンでADD ONE → 青ゾーンで停止";
        }

        private void ConfigureCamera()
        {
            var cam = Camera.main;
            if (cam == null) return;

            cam.orthographic = true;
            cam.orthographicSize = 6f;
//            cam.transform.position = new Vector3(startPosition.x + 4f, 3f, -10f);
//            cam.backgroundColor = new Color(0.15f, 0.17f, 0.22f);
        }

        private Rigidbody2D CreateAnchor()
        {
            var go = new GameObject("Anchor");
            go.transform.position = startPosition;
            go.transform.localScale = new Vector3(0.6f, 1.2f, 1f);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ProceduralSprite.SharedSquare;
            renderer.color = new Color(0.3f, 0.5f, 0.9f);
            renderer.sortingOrder = 2;

            var body = go.AddComponent<Rigidbody2D>();
            body.mass = 6f;
            body.linearDamping = 0.5f;
            body.angularDamping = 1f;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;

            var collider = go.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;

            return body;
        }

        private BoxCollider2D CreateBox(string name, Vector2 size, Vector2 position, Color color)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            go.transform.localScale = new Vector3(size.x, size.y, 1f);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ProceduralSprite.SharedSquare;
            renderer.color = color;

            var collider = go.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
            return collider;
        }

        private BoxCollider2D CreateZone(string name, Vector2 size, Vector2 position, Color color)
        {
            var zone = new GameObject(name);
            zone.transform.position = position;
            zone.transform.localScale = new Vector3(size.x, size.y, 1f);

            var renderer = zone.AddComponent<SpriteRenderer>();
            renderer.sprite = ProceduralSprite.SharedSquare;
            renderer.color = color;
            renderer.sortingOrder = 0;

            var collider = zone.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
            collider.isTrigger = true;
            return collider;
        }

        private Canvas BuildCanvas()
        {
            var canvasGO = new GameObject("UI Canvas");
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();

            return canvas;
        }

        private Button CreateButton(Transform parent, string name, string label, Vector2 anchoredPosition)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);

            var image = go.AddComponent<Image>();
            image.color = new Color(1f, 0.8f, 0.3f);

            var button = go.AddComponent<Button>();

            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(140f, 50f);
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = anchoredPosition;

            var textGO = new GameObject("Label");
            textGO.transform.SetParent(go.transform, false);
            var text = textGO.AddComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.text = label;
//            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.color = Color.black;
            text.fontSize = 20;

            var textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            return button;
        }

        private Text CreateText(Transform parent, string name, Vector2 anchoredPosition, int fontSize, TextAnchor alignment)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);

            var text = go.AddComponent<Text>();
            //text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.color = Color.white;

            var rect = text.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = new Vector2(380f, 80f);

            return text;
        }
    }
}
