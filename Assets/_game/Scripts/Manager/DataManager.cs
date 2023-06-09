﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.UI;
using JetBrains.Annotations;

[Serializable]
public struct IsPurchasedItems
{
    public ItemType itemType;
    public bool[] isPurchaseds;
}
public class DataManager : MonoBehaviour
{
    public static DataManager ins;
    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        this.LoadData();
    }
    public PlayerWearSkinItems player;
    public bool isLoaded = false;
    public PlayerData playerData;
    public const string PLAYER_DATA = "PLAYER_DATA";


    private void OnApplicationPause(bool pause) { SaveData(); }
    private void OnApplicationQuit() { SaveData(); }


    public void LoadData()
    {
        string d = PlayerPrefs.GetString(PLAYER_DATA, "");
        if (d != "")
        {
            playerData = JsonUtility.FromJson<PlayerData>(d);
        }
        else
        {
            playerData = new PlayerData();
        }

       //load
        //LoadIsPurchasedItems();
        LoadItemsOnPlayerBody();
        //LoadWeaponIndex();
        //LoadIsPurchasedWeapon();
        LoadMute();
        //LoadWeaponMaterialIndex();
        //LoadWeaponMaterialOutline();
        LoadCurrentLevelIndex();

             // sau khi hoàn thành tất cả các bước load data ở trên
             isLoaded = true;
        // FirebaseManager.Ins.OnSetUserProperty();  
    }

    public void SaveData()
    {
        if (!isLoaded) return;
        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PLAYER_DATA, json);
    }

    public void LoadIsPurchasedItems()//open skinshop
    {
        //duyet dict de tim tim ra itemcontroller, sau do set tat ca isPurchased = data.isPurchased
        for (int i = 0; i < SkinShopManager.instance.itemControllers.Length; i++)
        {
            for (int j = 0; j < playerData.dict[i].isPurchaseds.Length; j++)
            {
                SkinShopManager.instance.itemControllers[i].items[j].isPurchased = playerData.dict[i].isPurchaseds[j];
                if (SkinShopManager.instance.itemControllers[i].items[j].isPurchased == true)
                {
                    SkinShopManager.instance.UnlockSkin(SkinShopManager.instance.itemControllers[i].items[j]);
                }
            }
        }
        //mua usingitem
        for (int i = 0; i < playerData.usingItemIndexs.Length; i++)
        {
            if (playerData.usingItemIndexs[i] >= 0)
            {
                SkinShopManager.instance.itemControllers[i].OnButtonClick(playerData.usingItemIndexs[i]);
                SkinShopManager.instance.BuyItem(playerData.dict[i].itemType);
                break;
            }
        }
    }

    public void LoadItemsOnPlayerBody()//start game 
    {
        player.PutOnItemsAtFirst();
    }

    public void LoadInputField()//open mainmenu
    {
        InputField inputField = FindObjectOfType<InputField>();
        if(inputField != null)
        {
            inputField.text = playerData.playerName;
        }
    }

    public void LoadWeaponIndex()// open weapon shop
    {
        WeaponShopManager.Instance.usingWeaponIndex = this.playerData.usingWeaponIndex;
        WeaponShopManager.Instance.currentWeaponIndex = 0;
        
        for (int i = 0; i < WeaponShopManager.Instance.weapons.Length; i++)
        {
            Weapon wp = WeaponShopManager.Instance.weapons[i];
            wp.currentMaterialIndex = playerData.currentWeaponMaterialIndexs[i];
        }
    }

    public void LoadIsPurchasedWeapon()//open weapon shop
    {
        for(int i = 0; i < WeaponShopManager.Instance.weapons.Length; i++)
        {
            WeaponShopManager.Instance.weapons[i].isPurchased = this.playerData.isPurchasedWeapon[i];
        }
    }

    public void LoadMute()//start game 
    {
        AudioManager.instance.isMute = this.playerData.isMute;
    }

    public void LoadWeaponMaterialIndex()//open weapon shop
    {
        for(int i = 0; i < WeaponShopManager.Instance.weapons.Length; i++)
        {
            WeaponShopManager.Instance.weapons[i].currentMaterialIndex = this.playerData.currentWeaponMaterialIndexs[i];
        }
    }

    public void LoadWeaponMaterialOutline()//open weapon shop
    {
        for(int i = 0; i < WeaponShopManager.Instance.dictOutline.Length; i++)
        {
            WeaponShopManager.Instance.HideOutlinesWithSameWeaponType((WeaponType)i);
            WeaponShopManager.Instance.dictOutline[i].outlines[playerData.currentWeaponMaterialIndexs[i]].gameObject.SetActive(true);
        }
    }

    public void LoadCurrentLevelIndex()//start game 
    {
        LevelManager.instance.currentLevelIndex = this.playerData.currentLevelIndex;
    }

}


[System.Serializable]
public class PlayerData
{
    /*[Header("--------- Game Setting ---------")]
    public bool isNew = true;
    public bool isMusic = true;
    public bool isSound = true;
    public bool isVibrate = true;
    public bool isNoAds = false;
    public int starRate = -1;*/


    [Header("--------- Game Params ---------")]
    public bool isSetUp = false;
    public int level = 0;
    public int coin = 500;
    public int[] usingItemIndexs = new int[10];
    public IsPurchasedItems[] dict = new IsPurchasedItems[4];
    public MaterialType currentBodyMatType;
    public string playerName = Constant.EMPTY_STRING;
    public int usingWeaponIndex = 0;
    public int[] currentWeaponMaterialIndexs = new int[3];
    public bool[] isPurchasedWeapon = new bool[3];
    public bool isMute = false;
    public int currentLevelIndex = 0;
    

    public PlayerData()
    {
        if(isSetUp == true)
        {
            goto Label;
        }
        //setup usingItemIndexs:
        for(int i=0;i<usingItemIndexs.Length;i++)
        {
            usingItemIndexs[i] = -1;
        }
        //set up dict:
        SetUpDict();
        //set up isPurchasedWeapon:
        for(int i = 0; i < isPurchasedWeapon.Length; i++)
        {
            isPurchasedWeapon[i] = false;
        }
        isPurchasedWeapon[Constant.FIRST_INDEX] = true;
        isSetUp = true;
    Label:;
    }
    public void SetUpDict()
    {
        dict[0].itemType = ItemType.Hat;
        dict[0].isPurchaseds = new bool[9];
        dict[1].itemType = ItemType.Pants;
        dict[1].isPurchaseds = new bool[9];
        dict[2].itemType = ItemType.Shield;
        dict[2].isPurchaseds = new bool[2];
        dict[3].itemType = ItemType.FullSet;
        dict[3].isPurchaseds = new bool[3];
    }
}

