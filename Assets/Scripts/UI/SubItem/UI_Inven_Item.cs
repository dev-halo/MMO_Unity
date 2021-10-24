using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText
    }

    string itemName;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = itemName;

        Get<GameObject>((int)GameObjects.ItemIcon).AddUIEvent((data) => { Debug.Log($"æ∆¿Ã≈∆ ≈¨∏Ø! {itemName}"); });
    }

    public void SetInfo(string name)
    {
        itemName = name;
    }
}
