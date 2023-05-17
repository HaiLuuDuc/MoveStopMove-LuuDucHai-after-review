using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCloseSkinShop : BaseButton
{
    [SerializeField] private Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    protected override void OnClick()
    {
        CameraController.instance.StartCoroutine(CameraController.instance.SwitchTo(CameraState.MainMenu));
        SkinShopManager.instance.OnClose();
        UIManager.Ins.OpenUI<MainMenu>();
        player.Idle();
    }
}
