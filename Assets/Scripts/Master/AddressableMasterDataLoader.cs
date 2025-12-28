
using System.Threading.Tasks;
using Core.Common;
using Core.Stage;
using Core.Player;
using Master;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace OneTripMover.Master
{
    public class AddressableMasterDataLoader : IMasterDataLoader
    {
        public async Task LoadAsync()
        {
            var cargoRegistry = ServiceLocator.Resolve<ICargoMasterRegistry>();
            var stageRegistry = ServiceLocator.Resolve<IStageMasterRegistry>();
            var defineRegistry = ServiceLocator.Resolve<IDefineMasterRegistry>();
            var playerRegistry = ServiceLocator.Resolve<IPlayerMasterRegistry>();

            cargoRegistry.Clear();
            stageRegistry.Clear();
            defineRegistry.Clear();
            playerRegistry.Clear();

            await Task.WhenAll(
                LoadByLabel("Cargo", cargoRegistry),
                LoadByLabel("Stage", stageRegistry),
                LoadByLabel("Define", defineRegistry),
                LoadByLabel("Player", playerRegistry)
            );
            
            Debug.Log($"[MasterData] Loaded.");
        }

        private static async Task LoadByLabel<TMaster>(string label, IMasterDataRegistry<TMaster> registry)
            where TMaster : class, IMasterData<TMaster>
        {
            var handle = Addressables.LoadAssetsAsync<AddressableMasterData<TMaster>>(label, null);
            var assets = await handle.Task;

            try
            {
                foreach (var asset in assets)
                {
                    if (asset is TMaster master)
                    {
                        registry.Register(master);
                    }
                }
            }
            finally
            {
                Addressables.Release(handle);
            }
        }
    }
}
