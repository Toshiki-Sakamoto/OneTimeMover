using UnityEngine;
using UnityEngine.SceneManagement;

namespace OneTripMover.Asset
{
    public interface IAssetAutoReleaseService
    {
        void Register(Object asset, GameObject owner);
        void Register(Object asset, Scene scene);
    }
}
