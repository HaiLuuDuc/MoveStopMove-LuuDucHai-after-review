using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWearSkinItems : MonoBehaviour
{
    [Header("Character Skin Positions:")]
    public Transform leftHand;
    public Transform leftHand2;
    public Transform dinhdau;
    public Transform lung;
    public Transform tail;
    public SkinnedMeshRenderer pants;


    public virtual GameObject WearHat(int index)
    {
        if (index < 0)
        {
            return null;
        }
        GameObject newHat = Instantiate(Inventory.Instance.hats[index]);
        Quaternion hatOldRotation = Cache.GetTransform(newHat).rotation;
        Cache.GetTransform(newHat).SetParent(dinhdau.transform);
        Cache.GetTransform(newHat).localPosition = Vector3.zero;
        Cache.GetTransform(newHat).localRotation = hatOldRotation;
        return newHat;
    }

    public virtual void WearPants(int index)
    {
        pants.material = Inventory.Instance.pants[index];
    }

    public virtual GameObject WearShield(int index)
    {
        if (index < 0)
        {
            return null;
        }
        GameObject newShield = Instantiate(Inventory.Instance.shields[index]);
        Quaternion shieldOldRotation = Cache.GetTransform(newShield).rotation;
        Vector3 shieldOldScale = newShield.transform.localScale;
        Cache.GetTransform(newShield).SetParent(leftHand.transform);
        Cache.GetTransform(newShield).localPosition = Vector3.zero;
        Cache.GetTransform(newShield).localRotation = shieldOldRotation;
        Cache.GetTransform(newShield).localScale = shieldOldScale;
        return newShield;
    }

}
