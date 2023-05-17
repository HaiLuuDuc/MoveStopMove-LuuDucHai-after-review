using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameUI : NameUI
{
    public override void OnInit()
    {
        targetTransform = Cache.GetTransform(GameObject.FindGameObjectWithTag(Constant.PLAYER));
    }
}
