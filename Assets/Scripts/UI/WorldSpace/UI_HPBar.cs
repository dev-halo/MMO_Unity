using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
    }
}
