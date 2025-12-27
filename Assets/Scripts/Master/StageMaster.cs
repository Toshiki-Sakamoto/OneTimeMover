using Core.Cargo;
using Core.Adventure;
using Core.Stage;
using OneTripMover.Master;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Master
{
    /// <summary>
    /// ステージのマスター。Addressablesシーン参照を保持。
    /// </summary>
    [CreateAssetMenu(fileName = "StageMaster", menuName = "OneTripMover/Master/StageMaster")]
    public class StageMaster : AddressableMasterData<IStageMaster>, IStageMaster
    {
        // エディタ上でのみ表示・編集するためのフィールド
#if UNITY_EDITOR
        [SerializeField] private UnityEditor.SceneAsset _sceneAsset;
#endif
        
        [SerializeField] private int _stageID;
        [SerializeField, HideInInspector] private string _sceneName;
        [SerializeField] private int _initCargoNum = 3;
        [SerializeField] private CargoMaster[] _initCargoMasters;
        [SerializeField] private AdvText _introAdventure;
        [SerializeField] private AdvText _clearAdventure;

        public int StageId => _stageID;
        public string Scene => _sceneName;
        public int InitCargoNum => _initCargoNum;

        public ICargoMaster[] InitCargoMasters => _initCargoMasters;
        public AdvText IntroAdventure => _introAdventure;
        public AdvText ClearAdventure => _clearAdventure;

        protected override void OnValidateCore()
        {
#if UNITY_EDITOR
            if (_sceneAsset != null)
            {
                _sceneName = _sceneAsset.name;
            }
            else
            {
                _sceneName = string.Empty;
            }
#endif
        }
    }
}
