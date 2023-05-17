using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSettings : BaseButton
{
    protected override void OnClick()
    {
        UIManager.Ins.CloseUI<JoystickUI>();
        UIManager.Ins.OpenUI<Settings>();
    }
}
