using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int exp;
    [SerializeField]
    protected int gold;

    public int Exp
    {
        get { return exp; }
        set
        {
            exp = value;

            int level = Level;
            while (true)
            {
                if (Managers.Data.StatDict.TryGetValue(level + 1, out Data.Stat stat) == false)
                    break;

                if (exp < stat.totalExp)
                    break;

                ++level;
            }

            if (level != Level)
            {
                Debug.Log("Level Up");
                Level = level;
                SetStat(Level);
            }
        }
    }
    public int Gold { get { return gold; } set { gold = value; } }

    private void Start()
    {
        level = 1;
        
        defense = 5;
        moveSpeed = 5f;
        exp = 0;
        gold = 0;

        SetStat(level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        hp = stat.maxHp;
        maxHp = stat.maxHp;
        attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
