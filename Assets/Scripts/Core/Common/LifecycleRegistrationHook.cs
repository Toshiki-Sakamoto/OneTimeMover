using UnityEngine;

namespace Core.Common
{
    /// <summary>
    /// 登録した対象がDestroyされたときにLifecycleManagerから解除するためのフック。
    /// </summary>
    internal class LifecycleRegistrationHook : MonoBehaviour
    {
        private LifecycleManager _manager;
        private object _target;

        public void Bind(LifecycleManager manager, object target)
        {
            _manager = manager;
            _target = target;
        }

        private void OnDestroy()
        {
            if (_manager != null && _target != null)
            {
                _manager.Unregister(_target);
            }
            _manager = null;
            _target = null;
        }
    }
}
