using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialType
{
    White = 0,
    Black = 1,
    Aqua = 2,
    Green = 3,
    Orange = 4,
    Pink = 5,
    Purple = 6,
    Red = 7,
    Yellow = 8,
    Devil = 9,
    Angel = 10,
    Witch = 11
}
public class Colors : MonoBehaviour
{
    public Material[] mats;
    public Material transparent100;

    public static Colors instance;
    private void Awake()
    {
        instance= this;
    }
    public Material GetMat(MaterialType materialType)
    {
        return mats[(int)materialType];
    }
}
