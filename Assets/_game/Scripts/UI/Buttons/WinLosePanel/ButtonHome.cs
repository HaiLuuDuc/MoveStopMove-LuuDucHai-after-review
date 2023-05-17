using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHome : BaseButton
{
    protected override void OnClick()
    {
        LevelManager.instance.DeleteCharacters();
        LevelManager.instance.RespawnCharacters();
        LevelManager.instance.ResetTargetCircle();
        LevelManager.instance.SpawnMap(LevelManager.instance.currentLevelIndex);
        LevelManager.instance.SpawnNav(LevelManager.instance.currentLevelIndex);
        BotManager.instance.DeactiveAllBots();
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<MainMenu>();
        UIManager.Ins.OpenUI<Coin>();
        CameraController.instance.StartCoroutine(CameraController.instance.SwitchTo(CameraState.MainMenu));
    }
}
