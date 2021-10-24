using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } }

    InputManager input = new InputManager();
    ResourceManager resource = new ResourceManager();
    UIManager ui = new UIManager();

    public static InputManager Input { get { return Instance.input; } }
    public static ResourceManager Resource { get { return Instance.resource; } }
    public static UIManager UI { get { return Instance.ui; } }

    void Start()
    {
        Init();
    }

    void Update()
    {
        input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}
