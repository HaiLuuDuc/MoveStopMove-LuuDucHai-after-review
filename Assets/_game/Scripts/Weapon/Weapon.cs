using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Data:")]
    public WeaponData weaponData;
    public Transform child;
    public MeshRenderer meshRenderer;
    public WeaponType weaponType;
    [Header("Manage:")]
    protected WeaponPool weaponPool;
    protected Character owner;
    public int currentMaterialIndex;
    [Header("Bool Variables:")]
    public bool isStuckAtObstacle;
    public bool isPurchased;


    protected void Start()
    {
        currentMaterialIndex = DataManager.ins.playerData.currentWeaponMaterialIndexs[(int)weaponType];
        ChangeMaterial(currentMaterialIndex);
    }

    public void OnEnable()
    {
        isStuckAtObstacle = false;
    }

    public void ChangeMaterial(int index)
    {
        meshRenderer.material = weaponData.GetWeaponMaterial(index);
    }

    public Character GetOwner()
    {
        return this.owner;
    }

    public void SetOwnerAndWeaponPool(Character owner, WeaponPool weaponPool)
    {
        this.owner = owner;
        this.weaponPool = weaponPool;
    }

    public virtual void Fly(Vector3 target, float flySpeed)
    {
        StartCoroutine(FlyStraight(target,flySpeed));
    }

    public virtual IEnumerator FlyStraight(Vector3 target, float flySpeed)
    {
        while(Vector3.Distance(Cache.GetTransform(this.gameObject).position,target) > 0.1f && this.gameObject.activeSelf && !this.isStuckAtObstacle)
        {
            Cache.GetTransform(this.gameObject).position = Vector3.MoveTowards(Cache.GetTransform(this.gameObject).position, target, flySpeed * Time.deltaTime);
            yield return null;
        }
        if (!isStuckAtObstacle)
        {
            this.weaponPool.ReturnToPool(this.gameObject);
        }
        yield return null;
    }

    public IEnumerator StuckAtObstacle()
    {
        isStuckAtObstacle = true;
        yield return new WaitForSeconds(1);
        isStuckAtObstacle = false;
        weaponPool.ReturnToPool(this.gameObject);
        yield return null;
    }

    public virtual void ProcessHitCharacter(Character attacker, Character receiver)
    {
        if(attacker == receiver)
        {
            return;
        }

        weaponPool.ReturnToPool(this.gameObject);
        this.owner.TurnBigger();
        //play sound
        AudioManager.instance.PlayerSoundDie(Cache.GetTransform(receiver.gameObject));

        if (attacker is Bot && receiver is Bot)
        {
            (receiver as Bot).ChangeState(new DieState());
        }
        else if (attacker is Bot && receiver is Player)
        {
            LevelManager.instance.enemyName = (attacker as Bot).botName.nameString;
            LevelManager.instance.enemyMatType = (attacker as Bot).currentBodyMatType;
            (receiver as Player).OnDeath();
        }
        else if (attacker is Player && receiver is Bot)
        {
            (receiver as Bot).ChangeState(new DieState());
            DataManager.ins.playerData.coin += 10;
            Coin.instance.UpdateCoinOnUI();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (Cache.GetCharacter(other.gameObject) != null)
        {
            ProcessHitCharacter(this.owner, Cache.GetCharacter(other.gameObject));
        }
            
        if (other.gameObject.CompareTag(Constant.OBSTACLE))
        {
            StartCoroutine(StuckAtObstacle());
        }
    }
  
}

