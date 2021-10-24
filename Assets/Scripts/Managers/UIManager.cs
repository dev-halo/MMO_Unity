using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int order = 0;

    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        popupStack.Push(popup);
        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (popupStack.Count == 0)
            return;

        if (popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UI_Popup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        --order;
    }

    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }
}
