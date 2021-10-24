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

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (mode == Define.CameraMode.QuarterView)
        {
            transform.position = player.transform.position + delta;
            transform.LookAt(player.transform);
        }
    }

    public void SetQuarterView(Vector3 delta)
    {
        mode = Define.CameraMode.QuarterView;
        this.delta = delta;
    }
}
