using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCloseWeaponShop : BaseButton
{
    [SerializeField] private Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    protected override void OnClick()
    {
        player.GetWeaponFromInventory();
        UIManager.Ins.OpenUI<MainMenu>();
    }
}
