using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Common
{
    /// <summary>
    /// ServieLocatorの準備が完了した後にIInitializerを実行するためのMonoBehaviour
    /// </summary>
    public class ServiceResolveSceneInitializeExecutor : MonoBehaviour
    {
        private bool _initialSceneInjected;

        private void Execute()
        {
            // ServiceRegisterを検索してAutoInjectを走らせる
            var serviceRegisters = FindObjectsByType<ServiceRegister>(FindObjectsSortMode.None);
            foreach (var serviceRegister in serviceRegisters)
            {
                serviceRegister.InjectAutoGameObject();
            }
        }

        private async Task ExecuteAsync()
        {
            await Task.CompletedTask;
        }
        
        /// <summary>
        /// 特定のシーンのServiceRegisterのみを対象にInjectを実行
        /// </summary>
        private void ExecuteForScene(string sceneName)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            if (!scene.IsValid()) return;
            
            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                var serviceRegisters = rootObject.GetComponentsInChildren<ServiceRegister>();
                foreach (var serviceRegister in serviceRegisters)
                {
                    serviceRegister.InjectAutoGameObject();
                }
            }

            // ライフサイクル登録
            LifecycleManager.Instance.RegisterScene(scene);
        }

        private IEnumerator ExecuteForSceneAfterAwake(string sceneName)
        {
            yield return null; // 1フレーム待機
            ExecuteForScene(sceneName);
        }
        
        private void Awake()
        {
            // シーンが読み込まれ後、自動的に[Inject]でMonoBehaviourに依存性を注入させる
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Commonシーンはスキップ
            if (scene.name == "Common") return;
            // 初期シーンはStartのExecuteで済ませているのでスキップ
            if (!_initialSceneInjected) return;
            
            // 1フレーム後にExecuteForSceneを実行（すべてのAwakeが完了後）
            StartCoroutine(ExecuteForSceneAfterAwake(scene.name));
        }

        private async void Start()
        {
            // グローバルランナーを起動
            _ = LifecycleManager.Instance;

            // Awakeのタイミングでは実行しない
            Execute();

            await ExecuteAsync();
            _initialSceneInjected = true;

            // 初期シーンのライフサイクル対象を登録
            LifecycleManager.Instance.RegisterScene(gameObject.scene);
        }
    }
}
