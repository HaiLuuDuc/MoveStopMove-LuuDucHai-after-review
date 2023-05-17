using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : UICanvas
{
    [SerializeField] private TextMeshProUGUI tmp;
    public static Coin instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateCoinOnUI();
    }
    public void UpdateCoinOnUI()
    {
        tmp.text = DataManager.ins.playerData.coin.ToString();
    }
}
