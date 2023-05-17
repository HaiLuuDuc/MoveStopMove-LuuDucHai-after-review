using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSkin : BaseButton
{
    [SerializeField] private Player player;
    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
    }

    protected override void OnClick()
    {
        CameraController.instance.StartCoroutine(CameraController.instance.SwitchTo(CameraState.Skin));
        UIManager.Ins.CloseUI<MainMenu>();
        UIManager.Ins.OpenUI<SkinShopUI>();
        SkinShopManager.instance.OnOpen();
        player.Dance();
    }
}
