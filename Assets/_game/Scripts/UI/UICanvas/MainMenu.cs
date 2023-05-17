using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UICanvas
{
    public InputField inputField;
    public static MainMenu instance;
    private void Awake()
    {
        instance = this;
    }

    public override void Open()
    {
        base.Open();
        DataManager.ins.LoadInputField();
    }

    public override void CloseDirectly()
    {
        SavePlayerName();
        base.CloseDirectly();
    }

    public void SavePlayerName()
    {
        DataManager.ins.playerData.playerName = inputField.text;
    }

}
