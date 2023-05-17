using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonContinue : BaseButton
{
    protected override void OnClick()
    {
        UIManager.Ins.CloseUI<Settings>();
        UIManager.Ins.OpenUI<JoystickUI>();
    }
}
