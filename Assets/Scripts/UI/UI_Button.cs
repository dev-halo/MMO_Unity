using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    [SerializeField]
    Text text;

    int score = 0;

    public void OnButtonClicked()
    {
        ++score;
        text.text = $"Á¡¼ö : {score}";
    }
}
