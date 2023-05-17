using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lose : UICanvas
{
    [SerializeField] private Text rankText;
    [SerializeField] private Text loseText;
    public static Lose Instance;
    private void Awake()
    {
        Instance = this;
    }
    public override void Open()
    {
        base.Open();
        UpdateRankText(LevelManager.instance.rank);
        UpdateLoseText(LevelManager.instance.enemyName);
        //UpdateLoseColor(LevelManager.instance.enemyMatType);
    }
    public void UpdateRankText(int rank)
    {
        rankText.text = '#' + rank.ToString();
    }
    public void UpdateLoseText(string enemyName)
    {
        loseText.text = Constant.YOU_VE_BEEN_KILLED_BY + enemyName;
    }
    /*public void UpdateLoseColor(MaterialType enemyMatType)
    {
        loseText.color = Colors.instance.GetMat(enemyMatType).color;
    }*/
}
