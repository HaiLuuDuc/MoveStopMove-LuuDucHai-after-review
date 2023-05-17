using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FullsetData", menuName = "ScriptableObjects/FullsetData", order = 1)]
public class FullsetData : ScriptableObject
{
    public GameObject head;
    public GameObject tail;
    public GameObject wing;
    public GameObject leftHandObject;
    public MaterialType materialType;
}
