using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon, IRotateWeapon
{
    public override void Fly(Vector3 target, float flySpeed)
    {
        base.Fly(target, flySpeed);
        StartCoroutine(Rotate());
    }

    public IEnumerator Rotate()
    {
        float rotateSpeed = this.weaponData.rotateSpeed;
        while (!isStuckAtObstacle)
        {
            Cache.GetTransform(this.gameObject).Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
