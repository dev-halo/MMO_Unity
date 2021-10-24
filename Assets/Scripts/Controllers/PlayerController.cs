using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10f;

    Vector3 destPos;

    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        Managers.UI.ShowPopupUI<UI_Button>();

        Managers.UI.ClosePopupUI();
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    PlayerState state = PlayerState.Idle;

    void UpdateDie()
    {
    }

    void UpdateMoving()
    {
        Vector3 dir = destPos - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0f, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", speed);
    }

    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0f);
    }

    void Update()
    {
        switch (state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            default:
                break;
        }
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Wall")))
        {
            destPos = hit.point;
            state = PlayerState.Moving;
        }
    }
}
