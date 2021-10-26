using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    GameObject player;
    HashSet<GameObject> monsters = new HashSet<GameObject>();

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Unknown:
                break;
            case Define.WorldObject.Player:
                player = go;
                break;
            case Define.WorldObject.Monster:
                monsters.Add(go);
                break;
            default:
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Unknown:
                break;
            case Define.WorldObject.Player:
                {
                    if (player == go)
                        player = null;
                }
                break;
            case Define.WorldObject.Monster:
                {
                    if (monsters.Contains(go))
                        monsters.Remove(go);
                }
                break;
            default:
                break;
        }

        Managers.Resource.Destroy(go);
    }
}
