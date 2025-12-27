using UnityEngine;

namespace UI.GameOver
{
    /// <summary>
    /// ゲームオーバーUIのビュー。Animatorや表示切替をここで行う。
    /// </summary>
    public class GameOverUIView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _playTrigger = "Play";

        public void Play()
        {
            gameObject.SetActive(true);
            if (_animator != null && !string.IsNullOrEmpty(_playTrigger))
            {
                _animator.SetTrigger(_playTrigger);
            }
        }
    }
}
