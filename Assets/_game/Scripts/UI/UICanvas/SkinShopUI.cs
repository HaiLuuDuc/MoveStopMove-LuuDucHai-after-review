using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShopUI : UICanvas
{
    public GameObject[] areas;
    public static SkinShopUI instance;
    private void Awake()
    {
        instance = this;
    }
    public void HideAllChooseAreas()
    {
        for (int i = 0; i < areas.Length; i++)
        {
            areas[i].gameObject.SetActive(false);
        }
    }
}
