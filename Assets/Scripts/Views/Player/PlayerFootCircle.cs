using UnityEngine;

namespace OneTripMover.Views.Player
{
    public class PlayerFootCircle : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _angle;
        [SerializeField] private Transform _target;
        [SerializeField] private LayerMask _groundMask = ~0;
        [SerializeField] private float _groundCheckDistance = 1.5f;
        [SerializeField] private float _surfaceOffset = 0.02f;
        [SerializeField] private Color _gizmoColor = new Color(0.9f, 0.5f, 0.2f, 0.8f);
        [SerializeField] private Color _pointColor = new Color(0.2f, 0.8f, 1f, 0.9f);
        private RaycastHit2D[] _hits = new RaycastHit2D[1];

        public void AddAngle(float deltaAngle)
        {
            _angle += deltaAngle;
            
            UpdateWorldTargetPosition();
        }
        
        public Vector2 GetLocalPosition()
        {
            var radian = _angle * Mathf.Deg2Rad;
            var x = Mathf.Cos(radian) * _radius;
            var y = Mathf.Sin(radian) * _radius;
            return new Vector2(x, y);
        }

        /// <summary>
        /// ローカル円上の座標をワールドに変換し、地面があれば表面にスナップして返す。
        /// _targetが設定されていれば、そのTransformも同じ位置へ移動する。
        /// </summary>
        private Vector3 UpdateWorldTargetPosition()
        {
            var local = GetLocalPosition();
            var world = transform.TransformPoint(local);

            var rayStart = world + Vector3.up * _groundCheckDistance;
            if (Physics2D.RaycastNonAlloc(rayStart, Vector2.down, _hits, _groundCheckDistance * 2f, _groundMask) != 0)
            {
                // 地面に当たった時、world点よりも上だったらその位置にスナップする
                var hit = _hits[0];
                if (hit.point.y >= world.y)
                {
                    world = hit.point + (hit.normal * _surfaceOffset);
                }
            }

            if (_target != null)
            {
                _target.position = world;
            }

            return world;
        }

        private void OnDrawGizmosSelected()
        {
            // 円（ワールド空間）と現在のターゲット位置を可視化
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireSphere(transform.position, _radius);

            var world = UpdateWorldTargetPosition();

            Gizmos.color = _pointColor;
            Gizmos.DrawSphere(world, _radius * 0.08f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, world);
        }
    }
}
