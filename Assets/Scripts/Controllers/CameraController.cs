using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 delta = new Vector3(0f, 6f, -5f);

    [SerializeField]
    GameObject player = null;

    public void SetPlayer(GameObject player) { this.player = player; }

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (mode == Define.CameraMode.QuarterView)
        {
            if (player.IsValid() == false)
            {
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - player.transform.position).magnitude * 0.8f;
                transform.position = player.transform.position + delta.normalized * dist;
            }
            else
            {
                transform.position = player.transform.position + delta;
                transform.LookAt(player.transform);
            }
        }
    }

    public void SetQuarterView(Vector3 delta)
    {
        mode = Define.CameraMode.QuarterView;
        this.delta = delta;
    }
}
