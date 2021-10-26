using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    PlayerStat stat;
    Vector3 destPos;

    void Start()
    {
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

    [SerializeField]
    PlayerState state = PlayerState.Idle;

    public PlayerState State
    {
        get { return state; }
        set
        {
            state = value;

            Animator anim = GetComponent<Animator>();
            switch (state)
            {
                case PlayerState.Die:
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Moving:
                    anim.SetFloat("speed", stat.MoveSpeed);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Idle:
                    anim.SetFloat("speed", 0f);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Skill:
                    anim.SetBool("attack", true);
                    break;
                default:
                    break;
            }
        }
    }

    void UpdateDie()
    {
    }

    void UpdateMoving()
    {
        if (lockTarget != null)
        {
            float distance = (destPos - transform.position).magnitude;
            if (distance <= 1f)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        Vector3 dir = destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
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
                    State = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    void UpdateIdle()
    {

    }

    void UpdateSkill()
    {

    }

    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");

        State = PlayerState.Moving;
    }

    void Update()
    {
        switch (State)
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
            case PlayerState.Skill:
                UpdateSkill();
                break;
            default:
                break;
        }
    }

    int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    GameObject lockTarget;

    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == PlayerState.Die)
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
                        State = PlayerState.Moving;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            lockTarget = hit.collider.gameObject;
                        else
                            lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.PointerUp:
                break;
            case Define.MouseEvent.Click:
                break;
            default:
                break;
        }
    }
}
