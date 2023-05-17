using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class BotNameUI : NameUI
{
    public override void OnInit()
    {
        nameString = BotNamePool.instance.nameList[(int)Random.Range(0, BotNamePool.instance.nameList.Count)];
        SetNameUI(nameString);
    }

}
