using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 10f;

    bool moveToDest = false;
    Vector3 destPos;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    void Update()
    {
        if (moveToDest)
        {
            Vector3 dir = destPos - transform.position;
            if (dir.magnitude < 0.0001f)
            {
                moveToDest = false;
            }
            else
            {
                float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0f, dir.magnitude);
                transform.position += dir.normalized * moveDist;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            }
        }

        if (moveToDest)
        {
            Animator anim = GetComponent<Animator>();
            anim.Play("RUN");
        }
        else
        {
            Animator anim = GetComponent<Animator>();
            anim.Play("WAIT");
        }
    }

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += speed * Time.deltaTime * Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += speed * Time.deltaTime * Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += speed * Time.deltaTime * Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += speed * Time.deltaTime * Vector3.right;
        }

        moveToDest = false;
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Wall")))
        {
            destPos = hit.point;
            moveToDest = true;
        }
    }
}
