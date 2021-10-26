using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat stat;
    Vector3 destPos;

    Texture2D attackIcon;
    Texture2D handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand
    }

    CursorType cursorType = CursorType.None;

    void Start()
    {
        attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

        stat = GetComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    PlayerState state = PlayerState.Idle;

    void UpdateDie()
    {
    }

    void UpdateMoving()
    {
        Vector3 dir = destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            state = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0f, dir.magnitude);
            nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == false)
                    state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", stat.MoveSpeed);
    }

    void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0f);
    }

    void Update()
    {
        UpdateMouseCursor();

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

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(attackIcon, new Vector2(attackIcon.width * 0.2f, 0f), CursorMode.Auto);
                    cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(handIcon, new Vector2(handIcon.width * 0.33f, 0f), CursorMode.Auto);
                    cursorType = CursorType.Hand;
                }
            }
        }
    }

    int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    GameObject lockTarget;
    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out RaycastHit hit, 100f, mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

        switch (evt)
        {
            case Define.MouseEvent.Press:
                {
                    if (lockTarget != null)
                        destPos = lockTarget.transform.position;
                    else if (raycastHit)
                        destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        destPos = hit.point;
                        state = PlayerState.Moving;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            lockTarget = hit.collider.gameObject;
                        else
                            lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.PointerUp:
                lockTarget = null;
                break;
            case Define.MouseEvent.Click:
                break;
            default:
                break;
        }
    }
}
