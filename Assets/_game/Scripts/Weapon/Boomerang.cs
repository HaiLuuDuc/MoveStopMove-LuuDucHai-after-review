using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon, IRotateWeapon
{
    public override void Fly(Vector3 target, float flySpeed)
    {
        base.Fly(target, flySpeed);
        StartCoroutine(Rotate());
        
    }

    public override IEnumerator FlyStraight(Vector3 target, float flySpeed)
    {
        Vector3 returnPos = Cache.GetTransform(owner.gameObject).position; 
        while (Vector3.Distance(Cache.GetTransform(this.gameObject).position, target) > 0.1f && this.gameObject.activeSelf && !this.isStuckAtObstacle)
        {
            this.transform.position = Vector3.MoveTowards(Cache.GetTransform(this.gameObject).position, target, flySpeed * Time.deltaTime);
            yield return null;
        } 
        while (Vector3.Distance(Cache.GetTransform(this.gameObject).position, returnPos) > 0.1f && this.gameObject.activeSelf && !this.isStuckAtObstacle)
        {
            Cache.GetTransform(this.gameObject).position = Vector3.MoveTowards(Cache.GetTransform(this.gameObject).position, returnPos, flySpeed * Time.deltaTime);
            yield return null;
        }
        if (!isStuckAtObstacle)
        {
            this.weaponPool.ReturnToPool(this.gameObject);
        }
        yield return null;
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
