using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Character : MonoBehaviour, ITurnBigger
{
    [Header("Character Properties:")]
    [SerializeField] protected CharacterAnimation characterAnim;
    [SerializeField] protected CapsuleCollider capsulCollider;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public MaterialType currentBodyMatType;

    [Header("Weapon Properties:")]
    public Weapon onHandWeapon;
    public WeaponPool weaponPool;
    protected WeaponType weaponType;

    [Header("Lists:")]
    public List<Character> enemyList = new List<Character>();
    public List<Weapon> pooledWeaponList = new List<Weapon>();

    [Header("Character limbs:")]
    [SerializeField] protected GameObject body;
    public Transform rightHand;

    [Header("Bool Variables:")]
    public bool isDead = false;
    public bool isMoving = false;
    public bool isHaveHat;
    public bool isHaveShield;


    public virtual void OnInit()
    {
        EnableCollider();
        ResetSize();
        enemyList.Clear();
        isDead = false;
    }

    public abstract void OnDeath();

    public abstract void EnableCollider();

    public abstract void DisableCollider();

    public virtual void SetPosAndRot(Vector3 pos, Vector3 rot)
    {
        Cache.GetTransform(this.gameObject).position = pos;//30f, -0.34f, -8.6f
        Cache.GetTransform(this.gameObject).rotation = Quaternion.Euler(rot);
    }

    public virtual void DisplayOnHandWeapon()
    {
        if (this.onHandWeapon != null)
        {
            this.onHandWeapon.gameObject.SetActive(true);
        }
    }

    public virtual void UnDisplayOnHandWeapon()
    {
        if (this.onHandWeapon != null)
        {
            this.onHandWeapon.gameObject.SetActive(false);
        }
    }

    public virtual void TurnBigger()
    {
        TurnBiggerBody();
        TurnBiggerWeapon();
        if(this is Player)
        {
            CameraController.instance.StartCoroutine(CameraController.instance.MoveHigher());
        }
    }

    public virtual void TurnBiggerBody()
    {
        Vector3 oldScale = Cache.GetTransform(body).localScale;
        Vector3 newScale = oldScale * Constant.SCALE_VALUE;
        Cache.GetTransform(body).localScale = newScale;
    }

    public virtual void TurnBiggerWeapon()
    {
        for(int i=0;i<weaponPool.pool.Count;i++)
        {
            GameObject wp = weaponPool.pool[i];
            Vector3 oldBodyScale = Cache.GetTransform(wp).localScale;
            Vector3 newBodyScale = oldBodyScale * Constant.SCALE_VALUE;
            Cache.GetTransform(wp).localScale = newBodyScale;
        }
    }

    public virtual void ResetSize()
    {
        ResetBodySize();
        ResetWeaponSize();
    }

    public virtual void ResetBodySize()
    {
        Cache.GetTransform(this.gameObject).localScale = Vector3.one;
    }

    public virtual void ResetWeaponSize()
    {
        for (int i = 0; i < weaponPool.pool.Count; i++)
        {
            GameObject wp = weaponPool.pool[i];
            Cache.GetTransform(wp).localScale = Vector3.one;
        }
    }

    public virtual void SetSkinnedMeshRenderer(MaterialType matType)
    {
        skinnedMeshRenderer.material = Colors.instance.GetMat(matType);
    }

    public virtual void SetCurrentBodyMatType(MaterialType matType)
    {
        currentBodyMatType = matType;
    }
}
