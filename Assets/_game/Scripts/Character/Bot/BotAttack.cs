using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAttack : CharacterAttack
{
    [SerializeField] private CharacterAnimation characterAnim;
    [SerializeField] private Bot bot;


    public void Update()
    {
        if (bot.enemyList.Count > 0)
        {
            FindNearestTarget();
        }
        else
        {
            enemy = null;
        }
    }

    public override IEnumerator Attack()
    {
        Vector3 enemyPos = Cache.GetTransform(enemy.gameObject).position;

        bot.DisplayOnHandWeapon();

        characterAnim.ChangeAnim(Constant.ATTACK);

        RotateToTarget();

        yield return new WaitForSeconds(0.4f);//thời gian vung tay cho đến khi vũ khí rời bàn tay là 0.4s
        if (character.isDead)
        {
            yield break;
        }

        bot.UnDisplayOnHandWeapon(); // tat hien thi weapon tren tay
        Weapon newWeapon = Cache.GetWeapon(weaponPool.GetObject()); // lay weapon tu` pool
        Cache.GetTransform(newWeapon.gameObject).position = Cache.GetTransform(rightHand.gameObject).position; // dat weapon vao tay phai character

        ReCaculateTargetWeapon(newWeapon.gameObject, enemyPos);// đảm bảo vũ khí bay từ lòng bàn tay chứ không phải từ bot.position
        newWeapon.Fly(targetWeapon.position, newWeapon.weaponData.flySpeed);

        //play sound
        if (AudioManager.instance.IsInDistance(Cache.GetTransform(this.gameObject)))
        {
            AudioManager.instance.Play(SoundType.Throw);
        }
        yield return null;
    }
}
