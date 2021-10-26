using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    Stat stat;

    public override void Init()
    {
        stat = GetComponent<PlayerStat>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateDie() { }
    protected override void UpdateMoving() { Debug.Log("Monster UpdateMoving"); }
    protected override void UpdateIdle() { Debug.Log("Monster UpdateIdle"); }
    protected override void UpdateSkill() { Debug.Log("Monster UpdateSkill"); }

    void OnHitEvent()
    {
        Debug.Log("Monster OnHitEvent");
    }
}
