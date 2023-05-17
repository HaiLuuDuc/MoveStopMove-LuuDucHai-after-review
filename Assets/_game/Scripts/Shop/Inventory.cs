using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private void Awake()
    {
        Instance = this;
    }
    public Weapon[] weapons;
    public GameObject[] hats;
    public Material[] pants;
    public GameObject[] shields;
    public FullsetData[] fullSetDatas;

    public GameObject GetWeapon()
    {
        return weapons[DataManager.ins.playerData.usingWeaponIndex].gameObject;
    }

}
