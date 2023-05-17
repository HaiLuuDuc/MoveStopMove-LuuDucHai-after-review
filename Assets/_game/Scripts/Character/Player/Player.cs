using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [Header("Player class:")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Vector3 initialRot;

    protected void Start()
    {
        SetBodyMatType();
        OnInit();
    }

    public override void OnInit() {
        base.OnInit();
        SetPosAndRot(initialPos, initialRot);
        StopMoving();
        GetWeaponFromInventory();
        characterAnim.ChangeAnim(Constant.IDLE);
        SetSkinnedMeshRenderer(currentBodyMatType);
        DataManager.ins.playerData.currentBodyMatType = currentBodyMatType;
        isMoving = false;
    }

    public override void OnDeath() {
        DisableCollider();
        characterAnim.ChangeAnim(Constant.DIE);
        SetSkinnedMeshRenderer(MaterialType.Black);
        DataManager.ins.playerData.currentBodyMatType = MaterialType.Black;
        LevelManager.instance.rank = LevelManager.instance.currentAlive;
        BotManager.instance.DeactiveAllBots();
        UIManager.Ins.CloseUI<JoystickUI>();
        UIManager.Ins.CloseUI<Settings>();
        UIManager.Ins.OpenUI<Revive>();
        LevelManager.instance.DeleteCharacterInOtherEnemyLists(this);
        isDead = true;
    }

    public void OnRevive()
    {
        EnableCollider();
        StopMoving();
        //GetWeaponFromInventory.Instance();
        characterAnim.ChangeAnim(Constant.IDLE);
        SetSkinnedMeshRenderer(currentBodyMatType);
        DataManager.ins.playerData.currentBodyMatType = currentBodyMatType;
        isDead = false;
        isMoving = false;
    }

    public override void EnableCollider()
    {
        capsulCollider.enabled = true;
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
    }

    public override void DisableCollider()
    {
        capsulCollider.enabled = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
    }

    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }

    public void Idle()
    {
        characterAnim.ChangeAnim(Constant.IDLE);
    }

    public void Dance()
    {
        StopMoving();
        characterAnim.ChangeAnim(Constant.DANCE);
    }

    public void GetWeaponFromInventory()
    {
        if (onHandWeapon != null)
        {
            Destroy(onHandWeapon.gameObject);
        }
        Weapon wp = Cache.GetWeapon(Instantiate(Inventory.Instance.GetWeapon()));
        onHandWeapon = wp;
        DisplayOnHandWeapon();
        Cache.GetTransform(onHandWeapon.gameObject).SetParent(rightHand.transform);
        Cache.GetTransform(onHandWeapon.gameObject).localPosition = Vector3.zero;
        Cache.GetTransform(onHandWeapon.gameObject).localRotation = Quaternion.Euler(Vector3.zero);
        Cache.GetBoxCollider(onHandWeapon.gameObject).enabled = false;
        Cache.GetWeapon(onHandWeapon.gameObject).SetOwnerAndWeaponPool(this, this.weaponPool);
        weaponPool.prefab = wp.gameObject;
        weaponPool.OnDestroy();
        weaponPool.OnInit();
    }


    public void SetBodyMatType()
    {
         currentBodyMatType = DataManager.ins.playerData.currentBodyMatType;
    }

}
