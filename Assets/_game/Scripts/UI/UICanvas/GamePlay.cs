using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    [SerializeField] private Text aliveText;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerNameUI playerNameUI;
    public static GamePlay instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateAliveTextOnUI();
    }
    public override void Open()
    {
        base.Open();

        playerNameUI.OnInit();
        playerNameUI.SetNameUI(DataManager.ins.playerData.playerName);
        BotNamePool.instance.OnInit();
        BotManager.instance.DespawnNameAndIndicators();
        BotManager.instance.SpawnNameAndIndicators();

        playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.OnInit();
    }
    
    public void UpdateAliveTextOnUI()
    {
        aliveText.text = LevelManager.instance.currentAlive.ToString();
    }
}
