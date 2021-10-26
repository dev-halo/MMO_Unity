using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    Texture2D attackIcon;
    Texture2D handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand
    }

    CursorType cursorType = CursorType.None;

    private void Start()
    {
        attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    private void Update()
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
}
