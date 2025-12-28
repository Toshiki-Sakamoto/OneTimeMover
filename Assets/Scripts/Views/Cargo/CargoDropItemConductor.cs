using System.Linq;
using Core.Common;
using Core.Common.Messaging;
using Core.Game;
using Core.Stage;
using OneTripMover.Asset;
using UnityEngine;

namespace Views.Cargo
{
    public class CargoDropItemConductor : MonoBehaviour
    {
        private IStageUseCase _stageUseCase;
        private IAssetLoader _assetLoader;

        private void OnGamePhaseWillEnter(GamePhaseWillEnterEvent evt)
        {
            if (evt.NextPhase != GamePhase.Initialize) return;
            SetupDropItems();
        }

        private void SetupDropItems()
        {
            var items = GameObject.FindObjectsByType<CargoDropItemController>(FindObjectsSortMode.None);
            if (items == null || items.Length == 0) return;

            foreach (var item in items.Where(i => i != null))
            {
                var master = _stageUseCase.GetRandomDropCargoMaster();
                if (master?.CargoView == null) continue;

                var handle = _assetLoader.LoadInstantiate<CargoView>(master.CargoView, transform, false);
                var view = handle?.Asset;
                if (view == null) continue;

                view.SetMasterId(master.Id);
                item.AttachCargoView(view);
            }
        }
        
        private void Awake()
        {
            _stageUseCase = ServiceLocator.Resolve<IStageUseCase>();
            _assetLoader = ServiceLocator.Resolve<IAssetLoader>();

            var phaseSub = ServiceLocator.Resolve<ISubscriber<GamePhaseWillEnterEvent>>();
            phaseSub.Subscribe(OnGamePhaseWillEnter);
        }

    }
}
