using System;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : SingletonMono<CheatManager>
{
    private Dictionary<string, Action> _dicCheatAction = new Dictionary<string, Action>();

    protected override void Awake()
    {
        base.Awake();

        RegisterAllCheats();
    }

    public void ExecuteCheat(string cheatId)
    {
        bool isCheatAble = Application.isPlaying && _dicCheatAction.ContainsKey(cheatId.ToUpper());
        if (isCheatAble)
        {
            Debug.Log($"Execute Cheat {cheatId}");
            _dicCheatAction[cheatId.ToUpper()].Invoke();
        }
        else
        {
            Debug.LogWarning($"Cannot Execute Cheat {cheatId}");
        }
    }

    private void RegisterAllCheats()
    {
        RegisterCheat("ADDGOLD", AddGoldCheat);
        RegisterCheat("HEALPLAYER", HealPlayerCheat);
    }

    private void RegisterCheat(string cheatId, Action action)
    {
        if (_dicCheatAction.ContainsKey(cheatId) == false)
        {
            _dicCheatAction.Add(cheatId, action);
        }
    }

    private void AddGoldCheat()
    {
        int addGold = 100;
        ObjectManager.Instance.Player.Model.AddGold(addGold);
        Debug.Log($"add gold {addGold}");
    }

    private void HealPlayerCheat()
    {
        int addHp = 10;
        ObjectManager.Instance.Player.Model.AddHp(addHp);
        Debug.Log($"heal player {addHp}");
    }
}
