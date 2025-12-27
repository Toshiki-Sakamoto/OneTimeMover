using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Common
{
    /// <summary>
    /// シーン遷移サービス
    /// </summary>
    public class SceneTransitionService : MonoBehaviour, ISceneTransitionService
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private float _fadeDuration = 1f;
        
        public async Task LoadSceneAsTask(string sceneName)
        {
            await Fade(1);
            
            // 現在のアクティブシーンを取得（Commonシーン以外）
            var currentScene = SceneManager.GetActiveScene();
            
            // 新しいシーンを追加で読み込み
            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (op is { isDone: false }) await Task.Yield();
            
            // 新しいシーンをアクティブに設定
            var newScene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(newScene);
            
            // 前のシーンをアンロード（Commonシーンは残す）
            if (currentScene.name != "Common" && currentScene.IsValid())
            {
                SceneManager.UnloadSceneAsync(currentScene);
            }

            await Fade(0);
        }

        private async Task Fade(float targetAlpha)
        {
            var start = _fadeImage.color.a;
            var time = 0.0f;
            
            while (time < _fadeDuration)
            {
                time += Time.deltaTime;
                
                var a = Mathf.Lerp(start, targetAlpha, time / _fadeDuration);
                var c = _fadeImage.color;
                c.a = a;
                _fadeImage.color = c;

                await Task.Yield();
            }
            
            var final = _fadeImage.color;
            final.a = targetAlpha;
            _fadeImage.color = final;
        }
        
        private void Awake()
        {
            var c = _fadeImage.color;
            c.a = 0;
            _fadeImage.color = c;
        }
    }
}