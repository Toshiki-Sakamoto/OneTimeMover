using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Common
{
    /// <summary>
    /// IInitializable / IAsyncInitializable / IUpdatable を一元管理するランナー（DontDestroy）。
    /// </summary>
    public class LifecycleManager : MonoBehaviour
    {
        private static LifecycleManager _instance;
        public static LifecycleManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                var go = new GameObject("LifecycleManager");
                _instance = go.AddComponent<LifecycleManager>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }

        private readonly HashSet<IInitializable> _initialized = new();
        private readonly HashSet<IAsyncInitializable> _asyncInitialized = new();
        private readonly HashSet<IUpdatable> _updatables = new();

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // ServiceLocator に登録済みのシングルトンを登録
            RegisterFromServiceLocator();
        }

        private void Update()
        {
            foreach (var u in _updatables.ToArray())
            {
                u?.Update(Time.deltaTime);
            }
        }

        public void Register(object target, GameObject host = null)
        {
            if (target == null) return;

            // 初期化は一度だけ
            if (target is IInitializable init && _initialized.Add(init))
            {
                init.Initialize();
            }

            if (target is IAsyncInitializable asyncInit && _asyncInitialized.Add(asyncInit))
            {
                _ = RunAsync(asyncInit);
            }

            if (target is IUpdatable updatable)
            {
                _updatables.Add(updatable);
            }

            // 破棄時に自動解除するフック
            if (host != null)
            {
                var hook = host.GetComponent<LifecycleRegistrationHook>();
                if (hook == null)
                {
                    hook = host.AddComponent<LifecycleRegistrationHook>();
                }
                hook.Bind(this, target);
            }
        }

        public void Unregister(object target)
        {
            if (target == null) return;
            if (target is IUpdatable up) _updatables.Remove(up);
            // 初期化済みのセットは残しておく（再初期化しない）
        }

        public void RegisterScene(Scene scene)
        {
            if (!scene.IsValid()) return;
            /*
            foreach (var root in scene.GetRootGameObjects())
            {
                var behaviours = root.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var behaviour in behaviours)
                {
                    if (behaviour == null) continue;
                    if (behaviour is IInitializable or IAsyncInitializable or IUpdatable)
                    {
                        Register(iface, mb.gameObject);
                    }
                }
            }*/
        }

        private void RegisterFromServiceLocator()
        {
            // シングルトン登録されているものを取得
            foreach (var init in ServiceLocator.Resolve<IInitializable[]>())
            {
                Register(init);
            }
            foreach (var asyncInit in ServiceLocator.Resolve<IAsyncInitializable[]>())
            {
                Register(asyncInit);
            }
            foreach (var up in ServiceLocator.Resolve<IUpdatable[]>())
            {
                Register(up);
            }
        }

        private async Task RunAsync(IAsyncInitializable asyncInit)
        {
            await asyncInit.InitializeAsync();
        }
    }
}
