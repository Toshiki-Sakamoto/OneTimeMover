using System.Threading.Tasks;

namespace Core.Common
{
    /// <summary>
    /// シーン遷移サービス
    /// </summary>
    public interface ISceneTransitionService
    {
        /// <summary>
        /// 指定したシーン移動する
        /// </summary>
        Task LoadSceneAsTask(string sceneName);
    }
}