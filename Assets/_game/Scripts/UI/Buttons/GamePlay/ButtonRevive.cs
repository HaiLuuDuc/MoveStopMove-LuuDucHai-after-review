using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRevive : BaseButton
{
    [SerializeField] private int reviveCost;
    [SerializeField] private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    protected override void OnClick()
    {
        if(DataManager.ins.playerData.coin >= reviveCost)
        {
            DataManager.ins.playerData.coin -= reviveCost;
            Coin.instance.UpdateCoinOnUI();
            BotManager.instance.ActiveAllBots();
            player.OnRevive();
            UIManager.Ins.CloseUI<Settings>();
            UIManager.Ins.CloseUI<Revive>();
            UIManager.Ins.OpenUI<JoystickUI>();
        }
    }
}
