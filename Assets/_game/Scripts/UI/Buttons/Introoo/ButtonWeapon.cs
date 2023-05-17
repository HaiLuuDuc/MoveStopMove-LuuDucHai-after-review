using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWeapon : BaseButton
{
    protected override void OnClick()
    {
        UIManager.Ins.CloseUI<MainMenu>();
        UIManager.Ins.OpenUI<WeaponShopUI>();
    }
}
