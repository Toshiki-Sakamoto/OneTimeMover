using UnityEngine;

/// <summary>
/// ジョイントの角度が閾値を超えたら手動で破断させ、OnJointBreak2Dを発火させるユーティリティ。
/// </summary>
[RequireComponent(typeof(Joint2D))]
public class AngleJointBreaker2D : MonoBehaviour
{
    [SerializeField] private Joint2D _joint;
    [SerializeField] private float _breakAngleDeg = 30f;
    [SerializeField] private bool _destroyJointOnBreak = true;

    private enum ReferenceMode
    {
        ConnectedUp,   // 接続先のUpと比較
        WorldUp        // 地面（ワールドUp）と比較
    }

    [SerializeField] private ReferenceMode _reference = ReferenceMode.ConnectedUp;

    private bool _broken;

    private void Reset()
    {
        _joint = GetComponent<Joint2D>();
    }

    private void FixedUpdate()
    {
        if (_broken) return;
        if (_joint == null || !_joint.isActiveAndEnabled || _joint.connectedBody == null) return;

        var angle = ComputeAngle();
        if (Mathf.Abs(angle) >= _breakAngleDeg)
        {
            Break();
        }
    }

    private float ComputeAngle()
    {
        switch (_reference)
        {
            case ReferenceMode.WorldUp:
                return Vector2.SignedAngle(Vector2.up, transform.up);
            case ReferenceMode.ConnectedUp:
            default:
                Vector2 a = transform.up;
                Vector2 b = _joint.connectedBody.transform.up;
                return Vector2.SignedAngle(b, a);
        }
    }

    private void Break()
    {
        _broken = true;
        
        Debug.Log($"AngleJointBreaker2D: Angle: {ComputeAngle()}, Name: {name}", this);

        // OnJointBreak2Dを手動で通知
        gameObject.SendMessage("OnJointBreak2D", _joint, SendMessageOptions.DontRequireReceiver);
        if (_joint.connectedBody != null)
        {
            _joint.connectedBody.gameObject.SendMessage("OnJointBreak2D", _joint, SendMessageOptions.DontRequireReceiver);
        }

        if (_destroyJointOnBreak && _joint != null)
        {
            Destroy(_joint);
        }
    }
}
